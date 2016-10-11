using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using Acrux.Html.Specialized;
using System.Security.Permissions;
using System.Globalization;
using System.Xml;
using Acrux.Html.Specialized.Html401;
using System.Web;

namespace Acrux.Html
{


    /// <summary>
    /// -------------------------------------------------------------------------------------------------
    ///    BEFORE          |                                  AFTER                                     |
    /// -------------------------------------------------------------------------------------------------
    ///   Parent           |  Parent             Parent           Parent                Parent (*)      |
    ///      Current (*)   |     Current             Current          Current (*)          Current      |
    ///                    |          New (*)        New (*)             New               New          |
    /// -------------------------------------------------------------------------------------------------
    ///     TYPE 0         :     TYPE1              TYPE2           TYPE3                TYPE4          |
    /// -------------------------------------------------------------------------------------------------
    ///                                                                                                 |
    ///  Current = m_CurrentNode (the current one)                                                      |
    ///  Parent  = m_CurrentNode.Parent  (the current one's parent)                                     |
    ///  New     = newlyParsedElement                                                                   |
    ///  (*)     = Which will be the current node after we exit this method i.e. the new m_CurrentNode  |
    /// -------------------------------------------------------------------------------------------------
    /// </summary>
    internal enum TagProcessingType
    {
        IgnoreNodeAndKeepCurrent,                  /* TYPE 0 */
        AddToCurrentAndSetAsCurrent,               /* TYPE 1 */
        AddToCurrentsParentAndSetAsCurrent,        /* TYPE 2 */
        AddToCurrentAndKeepItCurrent,              /* TYPE 3 */
        AddToCurrentsParentAndSetParentAsCurrent,  /* TYPE 4 */
        ChangeCurrentToHtml,                       /* Used when the first HTML is found */
        ChangeCurrentToBody,                       /* Used when the first BODY is found */
        ChangeCurrentToHead                        /* Used when the first HEAD is found */ 
    }

    internal sealed class HtmlParser : HtmlPtrPredicateParser, IHtmlParserStatus
    {
        private HtmlDocument m_Document;
        private HtmlNode m_CurrentNode;
        private HtmlNode m_LastAddedElement;

        private HtmlNodeStack m_CurrentNodeStack;

        private HtmlParserSettings m_ParserSettings;
        private HtmlFixupSettings m_FixupMode;

        private TableContext m_TableContext = new TableContext();

        internal HtmlParser(HtmlFixupSettings fixupMode)
        {
            m_FixupMode = fixupMode;
            m_ParserSettings = fixupMode == HtmlFixupSettings.Firefox ? HtmlParserSettings.FirefoxSettings : HtmlParserSettings.AcruxSettings;
        }

        internal unsafe void Parse(char[] rawData, char* charData, HtmlDocument htmlDoc)
        {
            Debug.Assert(rawData != null);
            Debug.Assert(htmlDoc != null);

            m_Document = htmlDoc;

            Reflector refl = new Reflector(m_Document.m_XmlDoc);
            refl.SetReflectedValue("IsLoading", true);
            try
            {
                InitParser(rawData, charData, htmlDoc.ContentEncoding);

                m_CurrentNodeStack = new HtmlNodeStack(this, m_Document, m_ParserSettings);
                m_CurrentNodeStack.Clear();

                m_TableContext.Reset(this as IHtmlParserStatus);

                ParseInternal();

                switch (m_FixupMode)
                {
                    case HtmlFixupSettings.Acrux:
                        AcruxHtmlStructureFixups.ApplyFixups(htmlDoc);
                        break;

                    case HtmlFixupSettings.Firefox:
                        FirefoxHtmlStructureFixups.ApplyFixups(htmlDoc);
                        break;
                }
            }
            finally
            {
                refl.SetReflectedValue("IsLoading", false);
            }
        }



        private void ParseInternal()
        {
            try
            {
                // Always add a fake 'HTML' element first. This is done because the XMLDocument does not allow more than 1 child! 
                // Later when the real HTML element is found, this fake one will be replaced and removed            
                HtmlElement rootHtmlElement = m_Document.CreateElement("", "HTML", "", false, NodePosition.ReadOnly);
                m_Document.AppendChild(rootHtmlElement);

                // Also add a fake 'HEAD' element
                HtmlElement headElement = m_Document.CreateElement("", "HEAD", "", false, NodePosition.ReadOnly);
                rootHtmlElement.AppendChild(headElement);

                // Also add a fake 'BODY' element
                HtmlElement bodyElement = m_Document.CreateElement("", "BODY", "", false, NodePosition.ReadOnly);
                rootHtmlElement.AppendChild(bodyElement);

                // Make the BODY element current
                // TODO: Special handling for head tags with missing HEAD element ??
                Debug.Assert(rootHtmlElement != null);
                m_CurrentNodeStack.Push(rootHtmlElement);
                Debug.Assert(bodyElement != null);
                m_CurrentNodeStack.Push(bodyElement);
                m_CurrentNode = m_CurrentNodeStack.Peek();
                m_LastAddedElement = null;


                HtmlNode currentlyParsedElement;
                // The document is consisted of zero or more element declarations, 
                // where 'element declaration' can be pretty much anything. 
                while (ParseElement(out currentlyParsedElement))
                {
                    Debug.WriteLine("Parsing next element");

                    MarkNewReadingBuffer();
                };


                if (m_Document.HtmlElement == null)
                {
                    Debug.Assert(rootHtmlElement.Equals((m_Document.SelectSingleNode("/html"))));
                    m_Document.m_HtmlElement = rootHtmlElement;
                }

                if (m_Document.BodyElement == null)
                {
                    Debug.Assert(bodyElement.Equals((m_Document.SelectSingleNode("/html/body"))));
                    m_Document.m_BodyElement = bodyElement;
                }

                if (m_Document.HeadElement == null)
                {
                    Debug.Assert(headElement.Equals((m_Document.SelectSingleNode("/html/head"))));
                    m_Document.m_HeadElement = headElement;
                }
            }
            catch (Exception ex)
            {
                if (m_CurrentNode != null)
                    Trace.WriteLine(m_CurrentNode.XPathLocation + " : " + ex.Message);

                throw new HtmlParserException("An unanticipated error occured duing the parse operation. Check the inner exception for more information.", ex);
            }
        }

        private enum CommentClosingSequence
        {
            Undefined,
            EndOfLine,
            JavaScriptBlock
        }

        private bool ParseElement(out HtmlNode newlyParsedElement)
        {

#if DEBUG
                Push();
#endif

                // First skip all white space characters. The text element of the <PRE> tag will be parsed when the tag is parsed
                // For the rest of the text elements white spaces are ignored.
                // NOTE: At the moment the CSS "white-space: pre" property is not supported. May add this in future if CSS support is added

                NodePosition textElementPosition = new NodePosition();
                textElementPosition.ValueStartPos = m_CurrentIndex;
                newlyParsedElement = null;

                if (!SkipWhiteSpaces())
                    // We have reached the end of the file
                    return false;

                bool textElementPresent = false;
                bool nonTextElementRecognized = false;

                StringBuilder textElementContent = new StringBuilder();

                while (HasMoreChars && !nonTextElementRecognized)
                {

                    #region Reading all before the next '<' as a text element
                    while (
                             HasMoreChars &&
                             (CurrentChar != '<' /*|| isInComment || isInStringConstant || scriptElement != null*/)
                           )
                    {
                        textElementPresent = true;
                        ReadChar();
                    }
                    #endregion


                    if (textElementPresent)
                    {
                        string textSoFar = GetParsedDataString();
                        MarkNewReadingBuffer();
                        textElementContent.Append(textSoFar);
                    }

                    if (HasMoreChars)
                    {
                        // See if this "<" is a beginning of a new element ...
                        if (ParseElementStartingFromLessThanChar(out newlyParsedElement))
                        {
                            // The found "<" is a beginning of a recognized element
                            nonTextElementRecognized = true;

                            // if this is a <SCRIPT> or a <STYLE> then parse the inner bits here 
                            // and then add them as a #text child element 

                            if (newlyParsedElement.Name == "script")
                                ParseScript(newlyParsedElement as Specialized.Html401.ScriptStatement);
                            else if (newlyParsedElement.Name == "style")
                                ParseStyle(newlyParsedElement as Specialized.Html401.Style);
                        }
                        else
                        {
                            // The found "<" was not recognized/parsed as an element
                            // so continue searching for the beginning of the next element
                            // storing all read chars as a TEXT element (later)
                            textElementPresent = true;

                            Debug.WriteLine("The '<' is not the begining of an element.");
                        }
                    }
                }


                // Add a text element if present
                if (textElementPresent)
                {
                    string textElementValue = textElementContent.ToString();
                    textElementPosition.ValueEndPos = textElementPosition.ValueStartPos + textElementValue.Length - 1;
                    AddTextElement(textElementValue, textElementPosition);
                }


                if (newlyParsedElement != null &&
                    !(newlyParsedElement.m_XmlNode is XmlDeclaration))
                {
                    // Add the new element to the structure and update the current element
                    AddNewlyParsedElementToTheStructure(newlyParsedElement);
                }

#if DEBUG
                PopWithoutRestore();
#endif
                return HasMoreChars;
        }

        private void ParseScript(Specialized.Html401.ScriptStatement newlyParsedScriptElement)
        {
            if (newlyParsedScriptElement != null &&
                !newlyParsedScriptElement.IsEmptyElementTag)
            {
                MarkNewReadingBuffer();

                while (
                         HasMoreChars &&
                         !SequenceFollowsIgnoreCase("</SCRIPT", null, true, false)
                       )
                {
                    ReadChar();
                }

                // Add the sctyle as text
                NodePosition stylePos = new NodePosition();
                stylePos.NodeStartPos = base.ReadingBufferStartPosition;
                newlyParsedScriptElement.m_ScriptContent = GetParsedDataString();
                stylePos.NodeEndPos = stylePos.NodeStartPos + newlyParsedScriptElement.m_ScriptContent.Length - 1;
                newlyParsedScriptElement.AppendChild(new HtmlTextElement(m_Document, newlyParsedScriptElement.m_ScriptContent, stylePos));

                MarkNewReadingBuffer();
            }
        }

        private void ParseStyle(Specialized.Html401.Style newlyParsedStyleElement)
        {
            if (newlyParsedStyleElement != null &&
                !newlyParsedStyleElement.IsEmptyElementTag)
            {
                MarkNewReadingBuffer();

                while (
                         HasMoreChars &&
                         !SequenceFollowsIgnoreCase("</STYLE", null, true, false)
                       )
                {
                    ReadChar();
                }

                // Add the sctyle as text
                NodePosition stylePos = new NodePosition();
                stylePos.NodeStartPos = base.ReadingBufferStartPosition;
                newlyParsedStyleElement.m_StyleContent = GetParsedDataString();
                stylePos.NodeEndPos = stylePos.NodeStartPos + newlyParsedStyleElement.m_StyleContent.Length - 1;
                newlyParsedStyleElement.AppendChild(new HtmlTextElement(m_Document, newlyParsedStyleElement.m_StyleContent, stylePos));

                MarkNewReadingBuffer();
            }
        }

        private void AddNewlyParsedElementToTheStructure(HtmlNode newlyParsedElement)
        {
            m_LastAddedElement = newlyParsedElement;

            Debug.Assert(m_CurrentNode != null);
            Debug.Assert(newlyParsedElement != null);

            HtmlElement newElement = newlyParsedElement as HtmlElement;

            if (newElement != null)
            {
                if (newElement.IsEndTag)
                {
                    // This is an end tag. It may also be an end tag appearing on the wrong place
                    // i.e. there is no open tag that this one will closes.

                    int positionFromTop = 0;
                    bool startTagFound = false;

                    // TODO: All comparisons must be by NAME. Dont use " is " type comparison with Html40 classes
                    //       because we may want to define other Html spec tags in future !

                    if (!ExitSpecialElementsIfNeeded(newElement))
                        return;

                    if (newlyParsedElement.Name.Equals("HTML", StringComparison.CurrentCultureIgnoreCase) && m_Document.HtmlElement == null)
                    {
                        // Ignore all closing </HTML> tags appearing before the real <HTML> tag has been found!!

                        AddWarningMessage("Ignoring a closing HTML tag which is found before the real HTML start tag.");
                    }
                    else
                    {
                        foreach (HtmlNode node in m_CurrentNodeStack)
                        {
                            Debug.Assert(node.Name != null);
                            positionFromTop++;

                            if (node.Name.Equals(newlyParsedElement.Name, StringComparison.InvariantCultureIgnoreCase))
                            {
                                startTagFound = true;
                                break;
                            }
                        }
                    }

                    if (!startTagFound)
                    {
                        Debug.WriteLine(" could not find a corresponding start tag");

                        // If we cannot find the opening tag for this closing tag, just ignore it
                        AddWarningMessage(string.Format(CultureInfo.InvariantCulture, "Cannot locate an opened start tag for closing tag '{0}'", newElement.Name));
                    }
                    else
                    {
                        HtmlElement popedElement = null;

                        for (int i = 0; i < positionFromTop - 1; i++)
                        {
                            popedElement = m_CurrentNodeStack.Pop() as HtmlElement;
                            Debug.Assert(popedElement != null);

                            if (popedElement.ClosingTagRequirement == ElementClosingTagRequirement.Required)
                            {
                                AddWarningMessage(string.Format(CultureInfo.InvariantCulture, "The closing tag for the '{0}' element is missing.", popedElement.Name));
                            }
                        }

                        popedElement = m_CurrentNodeStack.Pop() as HtmlElement;

                        Debug.Assert(popedElement != null);
                        Debug.Assert(newElement.Name.Equals(popedElement.Name, StringComparison.CurrentCultureIgnoreCase));

                        if (m_CurrentNodeStack.Count > 1)
                        {
                            m_CurrentNode = m_CurrentNodeStack.Peek();
                        }
                        else if (m_CurrentNodeStack.Count == 1)
                        {
                            HtmlElement root = (HtmlElement)m_CurrentNodeStack.Peek();
                            Debug.Assert(root != null);
                            Debug.Assert(root.LocalName.Equals("html"));

                            m_CurrentNode = m_Document.SelectSingleNode("/html/body");
                            Debug.Assert(m_CurrentNode != null);
                            m_CurrentNodeStack.Push(m_CurrentNode);
                        }
                        else
                        {
                            Debug.Assert(m_CurrentNodeStack.Count == 0);

                            m_CurrentNode = m_Document.SelectSingleNode("/html");
                            Debug.Assert(m_CurrentNode != null);
                            m_CurrentNodeStack.Push(m_CurrentNode);

                            m_CurrentNode = m_Document.SelectSingleNode("/html/body");
                            Debug.Assert(m_CurrentNode != null);
                            m_CurrentNodeStack.Push(m_CurrentNode);
                        }

                    }

                }
                else
                {
                    // It is a start tag
                    Debug.Write("Processing a 'Start Tag' (" + newlyParsedElement.Name + ") ...");

                    // -------------------------------------------------------------------------------------------------
                    //    BEFORE          |                                  AFTER                                     |
                    // -------------------------------------------------------------------------------------------------
                    //   Parent           |  Parent             Parent           Parent                Parent (*)      |
                    //      Current (*)   |     Current             Current          Current (*)          Current      |
                    //                    |          New (*)        New (*)             New               New          |
                    // -------------------------------------------------------------------------------------------------
                    //     PROCESSING TYPE:     TYPE1              TYPE2           TYPE3                TYPE4          |
                    // -------------------------------------------------------------------------------------------------
                    //                                                                                                 |
                    //  Current = m_CurrentNode (the current one)                                                      |
                    //  Parent  = m_CurrentNode.Parent  (the current one's parent)                                     |
                    //  New     = newlyParsedElement                                                                   |
                    //  (*)     = Which will be the current node after we exit this method i.e. the new m_CurrentNode  |
                    // -------------------------------------------------------------------------------------------------


                    TagProcessingType processingType = TagProcessingType.AddToCurrentAndSetAsCurrent;

                    // ------------------------------------------------------------------------------------------------------------
                    // --> Determine how the new tag will be added to the structure. See TagProcessingType enum
                    if (newElement.IsStructureModuleElement)
                    {
                        // Special handling for HTML, BODY and HEAD tags (structure module elements)
                        // These 3 are always added at the beginning. This function determines which
                        // occurance is this for each of them. At the first occurance the current tag
                        // is set to them, the next occurances are ignored
                        ProcessStructureElement(newElement, ref processingType);
                    }
                    else
                    {
                        if (m_CurrentNode is HtmlElement)
                        {
                            // This function adds TBODY to a TABLE, etc.
                            if (!InsertSpecialElementsIfNeeded(newElement))
                                return;

                            HtmlElement currentElement = m_CurrentNode as HtmlElement;

                            // First check the special element level IGNORE-TAGS and RESET-TAGS of the current tag
                            if (currentElement.IgnoreTags != null &&
                                currentElement.IgnoreTags.IndexOf(newElement.UpperName) > -1)
                            {
                                if (m_ParserSettings.DontResetFormTag &&
                                    currentElement.Name == "form")
                                {
                                    // If the form shouldn't be reset (not Firefox default) then just keep adding below
                                    processingType = TagProcessingType.AddToCurrentAndSetAsCurrent;
                                }
                                else if (m_ParserSettings.DontIgnoreTags)
                                {
                                    // Use "IGNORE" tags as "RESET" tags
                                    // TODO: Implement special processing for FORMS i.e. don't add to parent but add to the form instead
                                    if (currentElement.ClosingTagRequirement == ElementClosingTagRequirement.Forbidden)
                                        processingType = TagProcessingType.AddToCurrentsParentAndSetParentAsCurrent;
                                    else
                                        processingType = TagProcessingType.AddToCurrentsParentAndSetAsCurrent;
                                }
                                else
                                    processingType = TagProcessingType.IgnoreNodeAndKeepCurrent;

                            }
                            else if (currentElement.ResetTags != null &&
                                     currentElement.ResetTags.IndexOf(newElement.UpperName) > -1)
                            {
                                // The newly found element is defined as "reset tag" for the current element
                                // This means that the current element must be closed and the new element has to
                                // be added as a child to current's parent.
                                if (currentElement.ClosingTagRequirement == ElementClosingTagRequirement.Forbidden)
                                    processingType = TagProcessingType.AddToCurrentsParentAndSetParentAsCurrent;
                                else
                                    processingType = TagProcessingType.AddToCurrentsParentAndSetAsCurrent;
                            }
                            else
                            {
                                // There are no speial element definition rules for this concrete parent and child 
                                // so apply the generic rules 

                                if (!ApplyNestedTagsRules(newElement, ref processingType))
                                {
                                    // NOTE: Ignore the "inline" and "block level" stuff. We are adding according to the 
                                    //       tested Firefox-Firebug rules. All which is not a rule is a standard TYPE 1 processing

                                    ApplyCloseTagRequirementRules(newElement, ref processingType);
                                }

                                // The optional end tag rules will be implemented later
                                // TODO: Implement some optional rules at least ?? 
                                // IDEA: examine all optional end tag elements.... hang on we actually dont need to implement this
                                //       all tags which will close an optional tag are already included in the ResetTags list !!!
                                //       So there are no generic optional tag rules, only specific optional tag rules
                                //       implement and test all the TABLE and SELECT/OPTION related stuff (may be later)

                                //// -------------------------------------------------------------------------------------------------------
                                //// --> Rules when the current element has OPTIONAL end tag requirements
                                //if (currentElement.ClosingTagRequirement == ElementClosingTagRequirement.Optional)
                                //{
                                //    if (newElement.IsBlockLevelElement &&
                                //        currentElement.IsInlineElement)
                                //    {
                                //        Debug.WriteLine("The current element is optional end tag, inline element. The new element is block level element so the optional tag will be 'closed' before this new element.");

                                //        if (processingType == TagProcessingType.AddToCurrentAndKeepItCurrent)
                                //        {
                                //            processingType = TagProcessingType.AddToCurrentsParentAndSetParentAsCurrent;
                                //        }
                                //        else
                                //        {
                                //            processingType = TagProcessingType.AddToCurrentsParentAndSetAsCurrent;
                                //        }
                                //    }
                                //}
                                //// <-- Rules when the current element has OPTIONAL end tag requirements
                                //// -------------------------------------------------------------------------------------------------------
                            }
                        }
                        else
                            throw new NotImplementedException("Only HtmlElements can be added to the structure for now.");
                    }
                    // <-- Determine how the new tag will be added to the structure. See TagProcessingType enum
                    // ------------------------------------------------------------------------------------------------------------

                    // If the new tag has forbidden close element, then is cannot be set as current
                    // In this case the parent must remain current
                    if (newElement.ClosingTagRequirement == ElementClosingTagRequirement.Forbidden)
                    {
                        if (processingType == TagProcessingType.AddToCurrentsParentAndSetAsCurrent)
                            processingType = TagProcessingType.AddToCurrentsParentAndSetParentAsCurrent;
                        else if (processingType == TagProcessingType.AddToCurrentAndSetAsCurrent)
                            processingType = TagProcessingType.AddToCurrentAndKeepItCurrent;
                    }


                    Debug.WriteLine("'Start Tag' processing type is " + processingType);


                    // ------------------------------------------------------------------------------------------------------------
                    // --> Add the newly parsed tag as a child (i.e. add it to the structure                    
                    if (processingType == TagProcessingType.AddToCurrentsParentAndSetAsCurrent ||
                        processingType == TagProcessingType.AddToCurrentsParentAndSetParentAsCurrent)
                    {
                        Debug.Assert(m_CurrentNode.ParentNode != null);

                        if (m_CurrentNode.ParentNode != null)
                            // Add the child to the current node's parent
                            m_CurrentNode.ParentNode.AppendChild(newlyParsedElement);
                        else
                        {
                            Debug.Assert(false);
                            throw new HtmlParserException();
                        }
                    }
                    else if (processingType == TagProcessingType.AddToCurrentAndKeepItCurrent ||
                             processingType == TagProcessingType.AddToCurrentAndSetAsCurrent)
                    {
                        // Add the child to the current node
                        m_CurrentNode.AppendChild(newlyParsedElement);
                    }
                    else
                    {
                        // Dont add the child.

                        Debug.Assert(
                            processingType == TagProcessingType.ChangeCurrentToBody ||
                            processingType == TagProcessingType.ChangeCurrentToHead ||
                            processingType == TagProcessingType.ChangeCurrentToHtml ||
                            processingType == TagProcessingType.IgnoreNodeAndKeepCurrent);
                    }
                    // <-- Add the newly parsed tag as a child (i.e. add it to the structure  
                    // ------------------------------------------------------------------------------------------------------------


                    if (processingType == TagProcessingType.AddToCurrentAndSetAsCurrent)
                    {
                        Debug.Assert(newlyParsedElement != null);
                        m_CurrentNodeStack.Push(newlyParsedElement);
                        m_CurrentNode = m_CurrentNodeStack.Peek();
                    }
                    else if (processingType == TagProcessingType.AddToCurrentsParentAndSetAsCurrent)
                    {
                        HtmlNode node = m_CurrentNodeStack.Pop();
                        Debug.Assert(node == m_CurrentNode);

                        Debug.Assert(newlyParsedElement != null);
                        m_CurrentNodeStack.Push(newlyParsedElement);
                        m_CurrentNode = m_CurrentNodeStack.Peek();
                    }
                    else if (processingType == TagProcessingType.AddToCurrentAndKeepItCurrent)
                    {
                        // Don't change anything. The child has been already added and the current node should be kept
                    }
                    else if (processingType == TagProcessingType.AddToCurrentsParentAndSetParentAsCurrent)
                    {
                        Debug.Assert(m_CurrentNode.ParentNode != null);
                        if (m_CurrentNode.ParentNode != null)
                        {
                            HtmlNode node = m_CurrentNodeStack.Pop();
                            Debug.Assert(node == m_CurrentNode);
                            Debug.Assert(m_CurrentNode.ParentNode == m_CurrentNodeStack.Peek());
                            m_CurrentNode = m_CurrentNodeStack.Peek();
                        }
                        else
                        {
                            Debug.Assert(false);
                            throw new HtmlParserException();
                        }
                    }
                    else if (processingType == TagProcessingType.ChangeCurrentToHtml)
                    {
                        // Special handling for HTML element

                        Debug.Assert(m_Document.HtmlElement != null);

                        m_CurrentNode = m_Document.HtmlElement;
                        m_CurrentNodeStack.Clear();
                        Debug.Assert(m_CurrentNode != null);
                        m_CurrentNodeStack.Push(m_CurrentNode);

                    }
                    else if (processingType == TagProcessingType.ChangeCurrentToBody)
                    {
                        // Special handling for BODY element

                        Debug.Assert(m_Document.BodyElement != null);

                        m_CurrentNode = m_Document.BodyElement;
                        m_CurrentNodeStack.Clear();

                        if (m_Document.HtmlElement == null)
                        {
                            // The BODY tag appears before HTML tag
                            HtmlElement htmlNode = (HtmlElement)m_Document.SelectSingleNode("/html");
                            Debug.Assert(htmlNode != null);
                            m_Document.m_HtmlElement = htmlNode;
                        }

                        m_CurrentNodeStack.Push(m_Document.HtmlElement);

                        Debug.Assert(m_CurrentNode != null);
                        m_CurrentNodeStack.Push(m_CurrentNode);
                    }
                    else if (processingType == TagProcessingType.ChangeCurrentToHead)
                    {
                        // Special handling for HEAD element

                        Debug.Assert(m_Document.HeadElement != null);

                        m_CurrentNode = m_Document.HeadElement;
                        m_CurrentNodeStack.Clear();

                        if (m_Document.HtmlElement == null)
                        {
                            // The HEAD tag appears before HTML tag
                            HtmlElement htmlNode = (HtmlElement)m_Document.SelectSingleNode("/html");
                            Debug.Assert(htmlNode != null);
                            m_Document.m_HtmlElement = htmlNode;
                        }

                        m_CurrentNodeStack.Push(m_Document.HtmlElement);
                        Debug.Assert(m_CurrentNode != null);
                        m_CurrentNodeStack.Push(m_CurrentNode);
                    }
                    else if (processingType == TagProcessingType.IgnoreNodeAndKeepCurrent)
                    {
                        // Ingore second HEAD / BODY or HTML tag
                        // Ignore any other tag that has to be ignored
                    }
                    else
                    {
                        string message = "Invalid processing type";
                        Debug.Assert(false, message);
                        throw new HtmlParserException(message);
                    }

                }
            }
            else if (newlyParsedElement is HtmlDocTypeElement)
            {
                // Add the <!DOCTYPE declarations only to the document 
                if (m_Document.DocumentType != null)
                    AddWarningMessage("Another <!DOCTYPE element located. It will be ignored!");
                else
                {
                    HtmlNode insertBefore = m_Document.HtmlElement;
                    if (insertBefore == null)
                        insertBefore = m_Document.SelectSingleNode("//html");

                    if (insertBefore != null)
                        m_Document.InsertBefore(newlyParsedElement, insertBefore);
                    else
                        m_Document.AppendChild(newlyParsedElement);
                }
            }
            else if (newlyParsedElement is HtmlProcessingInstruction)
            {
                HtmlNode insertBefore = m_Document.HtmlElement;
                if (insertBefore == null)
                    insertBefore = m_Document.SelectSingleNode("//html");

                if (insertBefore != null)
                    m_Document.InsertBefore(newlyParsedElement, insertBefore);
                else
                    m_Document.AppendChild(newlyParsedElement);
            }
            else
            {
                // Only real tag elements (derived from HtmlElement) can have end tags. 
                // If it is something different (<!--, <@, etc) always add it to the current node's children 
                // BUT don't change the current node.
                m_CurrentNode.AppendChild(newlyParsedElement);
            }
        }

        //private static List<string> s_TBODY_Tags = new List<string>(new string[] { "TD", "TR" });

        private bool InsertSpecialElementsIfNeeded(HtmlNode newElement)
        {
            if (m_CurrentNode != null && 
                m_ParserSettings.BuildMissingTableElements)
            {
                if (newElement.Name == "table")
                {
                    m_TableContext.EnterTable(newElement);
                }
                else if (
                    newElement.Name == "tbody" ||
                    newElement.Name == "thead" ||
                    newElement.Name == "tfoot")
                {
                    m_TableContext.EnterTableInnerBlock(newElement);
                }
                else if (newElement.Name == "tr")
                {
                    m_TableContext.EnterTableRow(newElement);
                }
                else if (newElement.Name == "td")
                {
                    m_TableContext.EnterTableCell(newElement);
                }

                // Special rules: If we are adding a text element to a table element which is not TD
                // Then add it to the <table> immediate parent, right before the table
                if (
                    newElement.Name == "#text" &&
                    (m_CurrentNode.Name == "tr" ||
                    m_CurrentNode.Name == "thead" ||
                    m_CurrentNode.Name == "tbody" ||
                    m_CurrentNode.Name == "tfoot" ||
                    m_CurrentNode.Name == "table"))
                {
                    if (m_TableContext.AddTextNodeOutsideTable(newElement as HtmlTextElement))
                        // The text node has been successfully added. No need to process it further. Return false
                        return false;
                    else
                        // If operation is not successful, then return "true"
                        // so the text node will be added anyway
                        // TODO: Add this as a warning
                        return true;
                }

            }

            if (m_Document.BodyElement == null &&
                newElement.Name == "script" &&
                m_CurrentNode.Name == "body" &&
                m_CurrentNode.ParentNode != null &&
                m_CurrentNode.ParentNode.ChildNodes.Count == 2)
            {
                // All scripts found before the BODY is parsed go to the HEAD element

                if (m_CurrentNode.ParentNode.ChildNodes[0].Name == "head")
                    m_CurrentNode.ParentNode.ChildNodes[0].AppendChild(newElement);

                return false;
            }

            if (newElement.Name == "input" &&
                m_ParserSettings.AddINPUTsToFirstOuternTD &&
                (m_CurrentNode.Name == "tbody" || m_CurrentNode.Name == "table" || m_CurrentNode.Name == "tr")
                )
            {
                // TODO: Add unit tests for INPUT elements inside a table without TDs
                HtmlNode lastTDElement = m_CurrentNode;
                while (lastTDElement != null &&
                       !lastTDElement.Equals(lastTDElement.ParentNode) &&
                       lastTDElement.Name != "td")
                {
                    lastTDElement = lastTDElement.ParentNode;
                }

                if (lastTDElement != null &&
                    lastTDElement.Name == "td")
                {
                    lastTDElement.AppendChild(newElement);

                    return false;
                }
            }

            if (
                newElement.Name == "form" &&
                m_CurrentNode != null &&
                m_CurrentNode.Name != "body" &&
                m_ParserSettings.UseEmptyFormTags &&
                m_TableContext.Position != TablePosition.NotInTable
                )
            {
                // TODO: This is a weird behaviour of FireFox. If the form is the first element of the body it allows other tags, if it is in a table 
                //       it doesn't. May be there is more to it. Needs more automated testing.

                // Add the form, but don't change the current element, so the form element will be empty
                // also ignore closing form tags
                m_CurrentNode.AppendChild(newElement);

                return false;
            }

            return true;
        }

        private bool ExitSpecialElementsIfNeeded(HtmlNode newElement)
        {
            if (m_CurrentNode != null &&
                m_ParserSettings.BuildMissingTableElements)
            {
                if (newElement.Name == "table")
                {
                    m_TableContext.ExitTable();
                }
                else if (
                    newElement.Name == "tbody" ||
                    newElement.Name == "thead" ||
                    newElement.Name == "tfoot")
                {
                    m_TableContext.ExitTableInnerBlock();
                }
                else if (newElement.Name == "tr")
                {
                    m_TableContext.ExitTableRow();
                }
                else if (newElement.Name == "td")
                {
                    m_TableContext.ExitTableCell();
                }
            }

            if (newElement.Name == "form" &&
               m_ParserSettings.UseEmptyFormTags)
            {
                // Just ignore all closing form tags
                return false;
            }

            return true;
        }


        private bool ApplyNestedTagsRules(HtmlElement newElement, ref TagProcessingType processingType)
        {
            Debug.Assert(newElement != null);

            if (!newElement.AllowNestedElements &&
                 AreSameTypeElements(m_CurrentNode, newElement))
            {
                // Nested tags detected, when they are not allowed to be nested.
                // In this case add the new tag as a child of the current tag's parent
                // and make it a current tag
                processingType = TagProcessingType.AddToCurrentsParentAndSetAsCurrent;

                Debug.WriteLine("Nesting tags detected when the current element does not allow nested tags. It will be added to the current element's parent");
                return true;
            }

            return false;
        }

        private void ApplyCloseTagRequirementRules(HtmlElement newElement, ref TagProcessingType processingType)
        {
            Debug.Assert(newElement != null);

            if ((newElement).IsEmptyElementTag)
            {
                if (newElement.ClosingTagRequirement != ElementClosingTagRequirement.Forbidden)
                {
                    // If it found empty but by definition it is not supposed to be an empty tag
                    // In this case we consider that it is just closed here and this is okay
                    // Add a warning message though
                    AddWarningMessage(string.Format(CultureInfo.InvariantCulture, "The tag '{0}' is not meant to be an empty tag, but the found one is empty. The parser will process it as tag with no children.", newElement.Name));
                }

                // This is an empty element tag so dont change the current element
                processingType = TagProcessingType.AddToCurrentAndKeepItCurrent;

                Debug.WriteLine("This is an empty element tag so the current element will not be changed");
            }
            else
            {
                if (newElement.ClosingTagRequirement == ElementClosingTagRequirement.Forbidden)
                {
                    // If the new element forbids having a closing tag - it cannot have children nodes
                    // So dont set it as current node then.
                    processingType = TagProcessingType.AddToCurrentAndKeepItCurrent;

                    Debug.WriteLine("The new element could not have children so the current element will not be changed");
                }
                else
                {
                    processingType = TagProcessingType.AddToCurrentAndSetAsCurrent;

                    Debug.WriteLine("The new element could have children so it will be set as current");
                }
            }
        }

        private void ProcessStructureElement(HtmlElement newElement, ref TagProcessingType processingType)
        {
            if (newElement.Name.Equals("HTML", StringComparison.CurrentCultureIgnoreCase))
            {
                if (m_Document.HtmlElement == null)
                {
                    // Replace the fake "HTML" added at the beginning of the parse operation with this real one
                    HtmlElement fakeHtml = (HtmlElement)m_Document.SelectSingleNode("/html");

                    Debug.Assert(fakeHtml != null);
                    Debug.Assert(m_Document.Equals(fakeHtml.ParentNode));

                    foreach (HtmlAttribute attribute in newElement.Attributes)
                    {
                        fakeHtml.Attributes.m_InternalCollection.Add(attribute);
                    }

                    // It's not fake any more!
                    m_Document.m_HtmlElement = fakeHtml;

                    processingType = TagProcessingType.ChangeCurrentToHtml;
                }
                else
                {
                    // This is a second HTML tag or HTML appeating after BODY/HEAD Ignore it!
                    processingType = TagProcessingType.IgnoreNodeAndKeepCurrent;
                }
            }

            if (newElement.Name.Equals("BODY", StringComparison.CurrentCultureIgnoreCase))
            {
                if (m_Document.BodyElement == null)
                {
                    // Replace the fake "BODY" added at the beginning of the parse operation with this real one
                    HtmlElement fakeBody = (HtmlElement)m_Document.SelectSingleNode("/html/body");

                    Debug.Assert(fakeBody != null);

                    foreach (HtmlAttribute attribute in newElement.Attributes)
                    {
                        fakeBody.Attributes.m_InternalCollection.Add(attribute);
                    }

                    // It's not fake any more!
                    m_Document.m_BodyElement = fakeBody;

                    processingType = TagProcessingType.ChangeCurrentToBody;
                }
                else
                {
                    // This is a second BODY tag! Ignore it!
                    processingType = TagProcessingType.IgnoreNodeAndKeepCurrent;
                }
            }

            if (newElement.Name.Equals("HEAD", StringComparison.CurrentCultureIgnoreCase))
            {
                if (m_Document.HeadElement == null)
                {
                    // Replace the fake "HEAD" added at the beginning of the parse operation with this real one
                    HtmlElement fakeHead = (HtmlElement)m_Document.SelectSingleNode("/html/head");

                    Debug.Assert(fakeHead != null);

                    foreach (HtmlAttribute attribute in newElement.Attributes)
                    {
                        fakeHead.Attributes.m_InternalCollection.Add(attribute);
                    }

                    // It's not fake any more!
                    m_Document.m_HeadElement = fakeHead;

                    processingType = TagProcessingType.ChangeCurrentToHead;
                }
                else
                {
                    // This is a second HEAD tag! Ignore it!
                    processingType = TagProcessingType.IgnoreNodeAndKeepCurrent;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element1"></param>
        /// <param name="element2"></param>
        /// <returns></returns>
        internal static bool AreSameTypeElements(HtmlNode element1, HtmlNode element2)
        {
            Debug.Assert(element1 != null);
            Debug.Assert(element2 != null);

            if (element1.NodeType != element2.NodeType)
                return false;

            Debug.Assert(element1.Name != null);
            Debug.Assert(element2.Name != null);

            return
                element1.Name.Equals(element2.Name, StringComparison.InvariantCultureIgnoreCase) &&
                (
                    (element1.NamespaceURI != null && element1.NamespaceURI.Equals(element2.NamespaceURI)) ||
                    (element2.NamespaceURI == null && element1.NamespaceURI == null)
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns><b>true</b> if an element was successfully recognized and parsed.</returns>
        private bool ParseElementStartingFromLessThanChar(out HtmlNode newlyParsedElement)
        {
            newlyParsedElement = null;

            if (HasMoreChars)
            {
                // TODO: Add all warning messages while parsing into a list, or call a callback delegate 

                // TODO: Think about the classes which will register to process certain sequences 
                //       <?               --> Always Parse as processing instruction
                //                            Allow custom processing instructions to be mapped. However the PI must always ends on "?>" so
                //                            first the end should be located. 
                //       <_
                //       <:
                //       <Letter         --> Standard Begin Tag. After the element is read we can call a custom mapper ( to possibly
                //                           expose some of the attributes as properties, etc)
                //                           
                //       </              --> Standard End Tag.
                //       <!              --> First check for comment. Parse the whole comment according to the rules
                //                           Then if the comment happens to have the structure: <!-- S? Name, search for
                //                           class registered to parse comments with the read name and call it (to map the stuff)
                //                       --> If its any of the known: ELEMENT, ATTLIST, ENTITY, DOCTYPE, NOTATION, <![ 
                //                           then parse the standard way
                //                           TODO: Check the MS Word specific <![If (condition)]> <![EndIf]> Syntax
                //      <@               --> Try to parse as a normal element 
                //      <@#              --> Parse as code C# or VB.NET depending on the other <Page or Control tags
                //                           TODO: Think about how to promote some document level properties after a tag has been parsed
                //                           i.e. after a <@Page tag is parsed is should register a property "CodeLanguage" (C#, VB.NET, etc)
                //                           it should be possible at any time to read those document properties like HtmlDocument.Properties["CodeLanguage"] for example
                //      <(Char - '_:/!@?' - Letter 
                //                       --> Look for registered special element parser. If there is no element parser for the given char
                //                           then 
                //                       --> <script> element must also parse the script itself up to the </script> (including. i.e. the whole lot)
                //                           
                //   There will be 2 types of classes. The first one will parse everything after the starting "<"
                //   The second one will just map the attributes to special properties and will expose them
                //   both classes after completing their job could register (add) some document level properties
                //   according to the parsed attribute values. Also may be each of them will register possible end tag occurance
                //   which may not be found (not well formed HTML, or optional end tag).
                //
                //   <td>
                //   click 
                //   <a href="">here</a>
                //   to see something
                //   </td> 
                //
                //  should be parsed as:
                //
                //  TD-|
                //     |- Children
                //           |     
                //           |- TextNode ('click')
                //           |
                //           |- A
                //           |  |
                //           |  |- Children 
                //           |       |
                //           |       - TextNode ('here')
                //           |
                //           |- TextNode ('to see something')

                if (!ReadChar())
                {
                    AddWarningMessage("Unexpected end of file after '<'.");
                    // TODO: what to do with the final "<", just ignore or include as part of the previous TextNode if any ??
                    Debug.Assert(false);
                    return false;
                }

                if (CurrentChar == '?')
                {
                    newlyParsedElement = ParseProcessingInstruction();
                    return newlyParsedElement != null;
                }
                else if (CurrentChar == '!')
                {
                    //[45]    elementdecl   ::=    '<!ELEMENT' S Name S contentspec S? '> 
                    //[52]    AttlistDecl   ::=    '<!ATTLIST' S Name AttDef* S? '>' 
                    //[71]    GEDecl        ::=    '<!ENTITY' S Name S EntityDef S? '>' 
                    //[28]    doctypedecl   ::=    '<!DOCTYPE' S Name (S ExternalID)? S? ('[' intSubset ']' S?)? '>' 
                    //[82]    NotationDecl  ::=    '<!NOTATION' S Name S (ExternalID | PublicID) S? '>' 
                    //[62]    includeSect   ::=    '<![' S? 'INCLUDE' S? '[' extSubsetDecl ']]>'  
                    //[63]    ignoreSect    ::=    '<![' S? 'IGNORE' S? '[' ignoreSectContents* ']]>' 
                    //[15]    Comment       ::=    '<!--' ((Char - '-') | ('-' (Char - '-')))* '-->' 

                    // Move to the next char after the (!)
                    ReadChar();

                    // --------------------------------------------------------------------------------------------
                    // --> Attempt to read a comment firt. This what we are most likely to find after (<!)
                    string comment;
                    NodePosition elementPosition = new NodePosition();
                    elementPosition.NodeStartPos = m_CurrentIndex - 2;

                    if (ReadComment(out comment))
                    {
                        // It's a comment. 

                        if (CurrentChar == '>')
                        {
                            // Skip the final '>'
                            ReadChar();
                        }
                        else
                        {

                        }

                        elementPosition.NodeEndPos = m_CurrentIndex - (CurrentChar == '<' ? 1 : 0);

                        //Try to parse its content as an element.
                        newlyParsedElement = ProcessComment(comment);
                        return newlyParsedElement != null;
                    }
                    // <-- Attempt to read a comment first. This is what we are most likely to find after (<!)
                    // --------------------------------------------------------------------------------------------


                    if (SequenceFollowsIgnoreCase("DOCTYPE", CharacterClasses.WHITE_SPACES))
                    {
                        // This is a <!DOCTYPE declaration
                        newlyParsedElement = ParseDocTypeDeclaration();
                        return newlyParsedElement != null;
                    }

                }
                else if (CurrentChar == '@')
                {
                    //        Server Code          '<@' ('#'? (C# Code | VB Code)) | Name
                    Debug.Assert(false);
                    throw new NotSupportedException("'@' is not supported at this time.");


                }
                else if (CurrentChar == '/')
                {
                    //[42]    ETag          ::=    '</' Name S? '>' 
                    newlyParsedElement = ParseEndTag();
                    return newlyParsedElement != null;
                }
                else if (
                        CurrentChar == '_' ||
                    //CurrentChar == ':' || NOTE: Is this allowed?
                        CharacterClasses.IsLetter(CurrentChar))
                {
                    newlyParsedElement = ParseElementTag();
                    return newlyParsedElement != null;
                }


                return false;

            }
            else
                // No more chars. We have reached the end of the data to parse
                return false;
        }

        private static char s_CHAR_SLASH = '/';
        private static char s_CHAR_QUESTION = '?';
        private static char s_CHAR_EMPTY = '\x0';

        /// <summary>
        /// Parses a processing instruction which could be a proper &lt;?XML tag or 
        /// any PITarget. The <b>XML</b> in the &lt;?XML tag can be mixed case. 
        /// </summary>
        /// <returns><b>true</b> if a valid processing instruction has been found.</returns>
        private HtmlNode ParseProcessingInstruction()
        {
            //----- any processing instruction ... -----
            //[16]    PI            ::=    '<?' PITarget (S (Char* - (Char* '?>' Char*)))? '?>' 
            //[17]    PITarget      ::=    Name - (('X' | 'x') ('M' | 'm') ('L' | 'l')) 
            //----- ... or a proper XML declaration -----

            // Skip the "?" and move to the next char
            ReadChar();

            string tagName;

            if (!ReadNamePredicate(out tagName))
            {
                // There MUST be a [Name] right after the '<?'
                // Retrun null so the text element parsing can continue
                return null;
            }

            if ("xml".Equals(tagName, StringComparison.InvariantCultureIgnoreCase))
            {
                // This is an XML declaration
                //[23]    XMLDecl       ::=    '<?xml' VersionInfo EncodingDecl? SDDecl? S? '?>' 
                //[24]    VersionInfo   ::=    S 'version' Eq ("'" VersionNum "'" | '"' VersionNum '"') 
                //[25]    Eq            ::=    S? '=' S? 
                //[26]    VersionNum    ::=    '1.0' 
                //[32]    SDDecl        ::=    S 'standalone' Eq (("'" ('yes' | 'no') "'") | ('"' ('yes' | 'no') '"'))  
                //[80]    EncodingDecl  ::=    S 'encoding' Eq ('"' EncName '"' | "'" EncName "'" )  
                //[81]    EncName       ::=    [A-Za-z] ([A-Za-z0-9._] | '-')* 
                // In an encoding declaration, the values "UTF-8", "UTF-16", "ISO-10646-UCS-2", and "ISO-10646-UCS-4" SHOULD be 
                // used for the various encodings and transformations of Unicode / ISO/IEC 10646, the values "ISO-8859-1", 
                // "ISO-8859-2", ... "ISO-8859-n" (where n is the part number) SHOULD be used for the parts of ISO 8859, and the 
                // values "ISO-2022-JP", "Shift_JIS", and "EUC-JP" SHOULD be used for the various encoded forms of JIS X-0208-1997. 
                // It is RECOMMENDED that character encodings registered (as charsets) with the Internet Assigned Numbers Authority 
                // [IANA-CHARSETS], other than those just listed, be referred to using their registered names; other encodings SHOULD 
                // use names starting with an "x-" prefix. XML processors SHOULD match character encoding names in a case-insensitive 
                // way and SHOULD either interpret an IANA-registered name as the encoding registered at IANA for that name or treat it 
                // as unknown (processors are, of course, not required to support all IANA-registered encodings).

                AttributeDictionary<string> attDict;
                AttributeDictionary<NodePosition> attPositions;
                bool isEmptyTag;

                if (!ParseElementAttributes(tagName, s_CHAR_QUESTION, out attDict, out attPositions, out isEmptyTag))
                {
                    Debug.Assert(false);

                    // TODO: Cant parse attributes
                    string location = null;
                    try
                    {
                        location = m_LastAddedElement.XPathLocation;
                    }
                    catch
                    { }

                    throw new HtmlParserException(string.Format(CultureInfo.InvariantCulture, "Cannot parse attributes at location: {0}", location));
                }

                if (!isEmptyTag)
                {
                    // We are not strict about an ending "?>". Just ignore it and move to the next element
                    // TODO: Register this as a parsing error so it will be double checked if occurs during automated testing
                    Debug.Assert(false);
                    AddWarningMessage("xml processing instruction is not ending on '?>'");
                }

                // TODO: Should check the XML version and the encoding specified.
                //       Update the encoding specified and then just ignore the tag
                string xmlVersion = attDict["version", true];
                Debug.Assert("1.0".Equals(xmlVersion));

                m_Document.m_XmlEncoding = attDict["encoding", true];

                // This is a workaround to skip the current element and not to add it as a text or element
                return new HtmlNode(XmlNodeType.XmlDeclaration, null, m_Document, NodePosition.ReadOnly);
            }
            else
            {
                while (HasMoreChars && CurrentChar != '?' && NextChar != '>')
                {
                    ReadChar();
                }

                string data = GetParsedDataString();

                if (HasMoreChars && CurrentChar == '?' && NextChar == '>')
                {
                    ReadChar();
                    ReadChar();
                }

                return new HtmlProcessingInstruction(tagName, data, m_Document);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private HtmlNode ParseEndTag()
        {
            // ------------------------------------------------------------------------------------
            // XML 1.0 Standard:
            //[42]    ETag          ::=    '</' Name S? '>' 
            // ------------------------------------------------------------------------------------
            // We will be parsing this way: 
            //        Tag           ::=    '</' Name (S+ Attribute)* S? '/'? '>'
            //        Name          ::=    (Letter | '_' | ':') (NameChar)*
            //        Attribute     ::=    Name S? (Eq S? AttValue? )?
            //        AttValue      ::=    ('"' [^"]* '"') | ("'" [^']* "'") | (Char - S - [<&])*
            // ------------------------------------------------------------------------------------

            // Skip the "/"
            ReadChar();

            string tagName;
            if (ReadNamePredicate(out tagName))
            {
                // Parse attributes but ignore them
                // </b  name='>' val='<new>' >

                AttributeDictionary<string> attDict;
                AttributeDictionary<NodePosition> attPositions;
                bool isEmptyTag;

                // TODO: Attributes defined in the end tag??? Add them as atributes of the corresponding opening tag?

                if (!ParseElementAttributes(tagName, s_CHAR_EMPTY, out attDict, out attPositions, out isEmptyTag))
                {
                    Debug.Assert(false);

                    string location = null;
                    try
                    {
                        location = m_LastAddedElement.XPathLocation;
                    }
                    catch
                    { }

                    throw new HtmlParserException(string.Format(CultureInfo.InvariantCulture, "Cannot parse attributes at location: {0}", location));
                }

                if (isEmptyTag)
                {
                    // This is </TAG .... /> 
                    Debug.Assert(false);
                    AddWarningMessage(string.Format("Empty tag '</{0} />' was closed twice", tagName));
                }

                Debug.Assert(tagName != null);
                Debug.Assert(m_Document != null);

                if (tagName.IndexOf(':') > 0)
                {
                    string prefix = tagName.Substring(0, tagName.IndexOf(':'));
                    string tag = tagName.Substring(tagName.IndexOf(':') + 1);

                    if (tag == string.Empty)
                        // It is not allowed the element to have a blank name
                        return null;
                    else
                        return new HtmlEndTag(prefix, tag, m_Document);
                }
                else
                    return new HtmlEndTag(tagName, m_Document);
            }
            else
            {
                // There is no valid name after the "</" so no element was parsed
                return null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private HtmlNode ParseElementTag()
        {
#if DEBUG
            Push();
#endif
            NodePosition elementPosition = new NodePosition();

            // ------------------------------------------------------------------------------------------------
            // According to the XML 1.0 Standard:
            // ------------------------------------------------------------------------------------------------
            //[44]    EmptyElemTag  ::=    '<' Name (S Attribute)* S? '/>' 
            //[40]    STag          ::=    '<' Name (S Attribute)* S? '>' [WFC: Unique Att Spec] 
            //[5]     Name          ::=    (Letter | '_' | ':') (NameChar)*
            //[41]    Attribute     ::=    Name Eq AttValue  
            //[25]    Eq            ::=    S? '=' S? 
            //[10]    AttValue      ::=    '"' ([^<&"] | Reference)* '"'  |  "'" ([^<&'] | Reference)* "'" 
            //-------------------------------------------------------------------------------------------------
            // We will be parsing this way: 
            //        Tag           ::=    '<' Name (S+ Attribute)* S? '/'? '>'
            //        Name          ::=    (Letter | '_' | ':') (NameChar)*
            //        Attribute     ::=    Name S? (Eq S? AttValue? )?
            //        AttValue      ::=    ('"' [^"]* '"') | ("'" [^']* "'") | (Char - S - [<&])*
            // -------------------------------------------------------------------------------------------------

            string tagName;

            elementPosition.NameStartPos = m_CurrentIndex;
            if (!ReadNamePredicate(out tagName))
            {
                // This is not an element. There MUST be a [Name] right after the '<'
                // Retrun null so the text element parsing can continue
                return null;
            }

            elementPosition.NameEndPos = m_CurrentIndex - 1;

            AttributeDictionary<string> attDict;
            AttributeDictionary<NodePosition> attPositions;
            bool isEmptyTag;

            if (!ParseElementAttributes(tagName, s_CHAR_SLASH, out attDict, out attPositions, out isEmptyTag))
            {
                Debug.Assert(false);

                string location = null;
                try
                {
                    location = m_LastAddedElement.XPathLocation;
                }
                catch
                { }

#if DEBUG
                PopAndRestore();
#endif
                string whatsNext = ReadChars(256);
                throw new HtmlParserException(string.Format(CultureInfo.InvariantCulture, "Cannot parse attributes at location: {0}\r\nSource: {1}", location, whatsNext));
            }

            if (tagName == null ||
                tagName.EndsWith(":"))
            {
                // May look like a legitimit element but it is not allowed to have a blank name
                return null;
            }
            else
                return BuildHtmlElementStructure(tagName, isEmptyTag, attDict, elementPosition, attPositions);
        }

        private enum AttribScanningState
        {
            ExpectingName,
            ReadingName,
            ExpectingEqualChar,
            ExpectingValue,
            ReadingValue,
            Finished
        }

        private bool ParseElementAttributes(
            string tagName, 
            char emptyTagChar,
            out AttributeDictionary<string> attDict,
            out AttributeDictionary<NodePosition> attPositions,
            out bool isEmptyTag)
        {
#if DEBUG
            Push();
#endif
            try
            {
                // TODO: Revise the attribute parsing code to include annonymous attributes (only value defined)
                //       and ignore one double/single quote such as <test " koza = "12" "annonymous value" >. 
                //       Ignore the first double quote but parse the annonymous value

                isEmptyTag = false;

                // This is needed to handle "<tag />" i.e. where this is a closed tag with a space between the element name and the closing "/>"
                SkipWhiteSpaces();

                attDict = new AttributeDictionary<string>();
                attPositions = new AttributeDictionary<NodePosition>();

                AttribScanningState state = AttribScanningState.ExpectingName;
                NodePosition currAttPos = new NodePosition();
                StringBuilder attName = new StringBuilder();
                StringBuilder attValue = new StringBuilder();
                char closeValueChar = '\0';
                bool columnInNameAlready = false;
                //int annonymousAttCounter = 0;

                

                while (
                    HasMoreChars &&
                    state != AttribScanningState.Finished)
                {

                    switch (state)
                    {
                        case AttribScanningState.ExpectingName:
                            if (!SkipWhiteSpaces())
                            {
                                // This is unfinished <TAG tag. We've reached the end of the document
                                // TODO: Add parser warning/error
                                return true;
                            }

                            if (CurrentChar == '>' ||
                                (NextChar == '>' && (emptyTagChar == CurrentChar || emptyTagChar == s_CHAR_EMPTY)))
                            {
                                state = AttribScanningState.Finished;
                                ReadChar();
                                if (CurrentChar == '>')
                                {
                                    ReadChar();
                                    isEmptyTag = "/?%!@".IndexOf(SecondPreviousChar) > -1;
                                }
                            }
                            else
                            {
                                if (!CharacterClasses.IsLetter(CurrentChar) && CurrentChar != '_' && CurrentChar != ':')
                                {
                                    if (CurrentChar == '\'' || CurrentChar != '"')
                                    {
                                        //annonymousAttCounter++;
                                        //attName = new StringBuilder(string.Format("annonymous{0}", annonymousAttCounter));
                                        //attValue = new StringBuilder();
                                        //closeValueChar = CurrentChar;
                                        //state = AttribScanningState.ReadingValue;
                                    }
                                    else
                                    {
                                        // We are expecting to read the first character, which is invalid
                                        // so we just ignore/skip this character and go on still expecting name
                                        // TODO: Add warning message 
                                    }
                                }
                                else
                                {
                                    // The first character is found. We move into reading name mode
                                    state = AttribScanningState.ReadingName;
                                    columnInNameAlready = CurrentChar == ':';
                                    attName = new StringBuilder(CurrentChar.ToString());

                                    currAttPos = new NodePosition();
                                    currAttPos.NameStartPos = m_CurrentIndex;
                                    currAttPos.NameEndPos = m_CurrentIndex;
                                }

                                ReadChar();
                            }
                            break;

                        case AttribScanningState.ReadingName:

                            if (CharacterClasses.IsNameChar(CurrentChar))
                            {
                                if (columnInNameAlready && CurrentChar == ':')
                                {
                                    // This is a second ":" in the name. This is not allowed
                                    state = AttribScanningState.ExpectingEqualChar;
                                }
                                else
                                {
                                    attName.Append(CurrentChar);
                                    currAttPos.NameEndPos = m_CurrentIndex;

                                    if (CurrentChar == ':')
                                        columnInNameAlready = true;

                                    ReadChar();
                                }
                            }
                            else if (
                                CurrentChar == '>' ||
                                (NextChar == '>' && emptyTagChar == CurrentChar))
                            {
                                AddElementAttribute(attDict, attName.ToString(), string.Empty, tagName, attPositions, currAttPos);
                                state = AttribScanningState.Finished;
                                ReadChar();
                                if (CurrentChar == '>')
                                {
                                    ReadChar();
                                    isEmptyTag = emptyTagChar == SecondPreviousChar;
                                }
                            }
                            else
                            {
                                // This is not a name char, so we finished reading the name as this char is not allowed in a name
                                state = AttribScanningState.ExpectingEqualChar;
                            }

                            break;

                        case AttribScanningState.ExpectingEqualChar:
                            if (!SkipWhiteSpaces())
                            {
                                // This is unfinished <TAG tag. We've reached the end of the document
                                // TODO: Add parser warning/error
                                return true;
                            }

                            if (CurrentChar == '=')
                            {
                                state = AttribScanningState.ExpectingValue;
                                ReadChar();
                            }
                            else if (
                                CurrentChar == '>' ||
                                (NextChar == '>' && emptyTagChar == CurrentChar))
                            {
                                AddElementAttribute(attDict, attName.ToString(), string.Empty, tagName, attPositions, currAttPos);
                                state = AttribScanningState.Finished;
                                ReadChar();
                                if (CurrentChar == '>')
                                {
                                    ReadChar();
                                    isEmptyTag = emptyTagChar == SecondPreviousChar;
                                }
                            }
                            else
                            {
                                if (CharacterClasses.IsLetter(CurrentChar) || CurrentChar == '_' || CurrentChar == ':')
                                {
                                    // The current char is a valid name char, so we add the existing attribute
                                    AddElementAttribute(attDict, attName.ToString(), string.Empty, tagName, attPositions, currAttPos);

                                    // And then initiate a new element name parsing
                                    attName = new StringBuilder(CurrentChar.ToString());
                                    state = AttribScanningState.ReadingName;
                                    columnInNameAlready = CurrentChar == ':';

                                    currAttPos = new NodePosition();
                                    currAttPos.NameStartPos = m_CurrentIndex + (columnInNameAlready ? 1 : 0);
                                    currAttPos.NameEndPos = currAttPos.NameStartPos;
                                }
                                else
                                {
                                    // If the next char is invalid first letter char then ignore it
                                    // TODO: Add warning message
                                }

                                ReadChar();
                            }
                            break;

                        case AttribScanningState.ExpectingValue:

                            if (!SkipWhiteSpaces())
                            {
                                // This is unfinished <TAG tag. We've reached the end of the document
                                // TODO: Add parser warning/error
                                return true;
                            }

                            if (CurrentChar == '>' ||
                                (NextChar == '>' && emptyTagChar == CurrentChar))
                            {
                                AddElementAttribute(attDict, attName.ToString(), string.Empty, tagName, attPositions, currAttPos);
                                state = AttribScanningState.Finished;
                                ReadChar();
                                if (CurrentChar == '>')
                                {
                                    ReadChar();
                                    isEmptyTag = emptyTagChar == SecondPreviousChar;
                                }
                            }
                            else
                            {
                                attValue = new StringBuilder();

                                if (CurrentChar == '\'' || CurrentChar == '"')
                                {
                                    closeValueChar = CurrentChar;
                                    currAttPos.ValueStartPos = m_CurrentIndex + 1;
                                    currAttPos.ValueEndPos = m_CurrentIndex + 1;
                                }
                                else
                                {
                                    closeValueChar = '\0';
                                    attValue.Append(CurrentChar);

                                    currAttPos.ValueStartPos = m_CurrentIndex;
                                    currAttPos.ValueEndPos = m_CurrentIndex;
                                }

                                state = AttribScanningState.ReadingValue;
                                ReadChar();
                            }

                            break;

                        case AttribScanningState.ReadingValue:

                            if (closeValueChar == '\0' &&
                                (CurrentChar == '>' || (NextChar == '>' && emptyTagChar == CurrentChar)))
                            {
                                AddElementAttribute(attDict, attName.ToString(), attValue.ToString(), tagName, attPositions, currAttPos);
                                state = AttribScanningState.Finished;
                                ReadChar();
                                if (CurrentChar == '>')
                                {
                                    ReadChar();
                                    isEmptyTag = emptyTagChar == SecondPreviousChar;
                                }
                            }
                            else
                            {
                                if (
                                    (CurrentChar == ' ' && closeValueChar == '\0') ||
                                    (CurrentChar == closeValueChar)
                                    )
                                {
                                    currAttPos.NodeEndPos = m_CurrentIndex;

                                    AddElementAttribute(attDict, attName.ToString(), attValue.ToString(), tagName, attPositions, currAttPos);
                                    state = AttribScanningState.ExpectingName;
                                }
                                else
                                {
                                    attValue.Append(CurrentChar);
                                    currAttPos.ValueEndPos = m_CurrentIndex;
                                }

                                ReadChar();
                            }

                            break;
                    }

                }

                return true;
            }
            finally
            {
#if DEBUG
                PopWithoutRestore();
#endif
            }
        }

        private void AddElementAttribute(
            AttributeDictionary<string> attDict, 
            string attName, 
            string attValue, 
            string tagName,
            AttributeDictionary<NodePosition> attPositions,
            NodePosition attPos)
        {
            try
            {
                attDict.Add(attName, attValue);
            }
            catch (DuplicatedAttributeOverwrittenException<string> ex)
            {
                string xpathloc = "N/A";
                try
                {
                    xpathloc = m_CurrentNode.XPathLocation;
                }
                catch (Exception)
                { }


                // Key duplicated exception. Overwrite the previous value
                AddWarningMessage(
                    string.Format(CultureInfo.InvariantCulture, "Error parsing tag '{0}'. The error message is: '{1}'. Current node: {2}",
                    tagName,
                    ex.Message,
                    xpathloc));
            }

            try
            {
                attPositions.Add(attName, attPos);
            }
            catch (DuplicatedAttributeOverwrittenException<NodePosition> ex)
            {
                string xpathloc = "N/A";
                try
                {
                    xpathloc = m_CurrentNode.XPathLocation;
                }
                catch (Exception)
                { }


                // Key duplicated exception. Overwrite the previous value
                AddWarningMessage(
                    string.Format(CultureInfo.InvariantCulture, "Error parsing tag '{0}'. The error message is: '{1}'. Current node: {2}",
                    tagName,
                    ex.Message,
                    xpathloc));
            }
            
        }

        private HtmlElement BuildHtmlElementStructure(
            string tagName, 
            bool isEmptyTag, 
            AttributeDictionary<string> attDict,
            NodePosition elementPosition,
            AttributeDictionary<NodePosition> attPositions)
        {
            if (tagName.EndsWith(":"))
            {
                string newTagName = tagName.Substring(0, tagName.Length - 1);

                AddWarningMessage(string.Format("Element name '{0}' is invalid. The assumed name will be '{1}'", tagName, newTagName));

                tagName = newTagName;
                elementPosition.NameEndPos--;
            }

            string[] prefAndName = tagName.Split(new char[] { ':' }, 2);
            string prefix = prefAndName.Length == 2 ? prefAndName[0] : null;
            string localName = prefAndName.Length == 2 ? prefAndName[1] : tagName;
            string nameSpaceUri = null;

            ResolveNameSpace(ref prefix, ref localName, ref nameSpaceUri, tagName, attDict, m_CurrentNode);

            Debug.Assert(m_Document != null);

            // Ignore a trailing ":" in the name as this means an empty string for a name
            if (localName.StartsWith(":") || localName.EndsWith(":"))
            {
                string newLocalName = localName.Trim(new char[] { ':' }); ;

                AddWarningMessage(string.Format("Element name '{0}' is invalid. A namespace prefix '{1}' and element name '{2}' are assumed.", tagName, prefix, newLocalName));

                // TODO: Add error/warning message
                localName = newLocalName;

                // Mark the node as 'read only'
                elementPosition = NodePosition.ReadOnly;
            }

            if (localName != null &&
                localName.IndexOf(":") > -1)
            {
                // TODO: Add an error/warning message
                localName = localName.Replace(":", "_");
            }

            HtmlElement thisElement = m_Document.CreateElement(prefix, localName, nameSpaceUri, isEmptyTag, elementPosition);

            Trace.Assert(attDict.Count == attPositions.Count);

            foreach (string attName in attDict.Keys)
            {
                prefAndName = attName.Split(new char[] { ':' }, 2);
                prefix = prefAndName.Length == 2 ? prefAndName[0] : null;
                localName = prefAndName.Length == 2 ? prefAndName[1] : attName;
                nameSpaceUri = null;

                ResolveNameSpace(ref prefix, ref localName, ref nameSpaceUri, attName, attDict, m_CurrentNode);

                if (!string.IsNullOrEmpty(localName))
                {
                    NodePosition attPosition = new NodePosition();
                    try
                    {
                        attPosition = attPositions[attName];
                    }
                    catch (KeyNotFoundException)
                    {
                        Debug.Assert(false);
                    }

                    HtmlAttribute att = new HtmlAttribute(prefix, localName, nameSpaceUri, m_Document, attPosition, attDict[attName], true);

                    if (attDict.HasDuplicatedValues(attName))
                        att.AddDuplicatedValues(attDict.AllValues(attName));

                    thisElement.Attributes.m_InternalCollection.Add(att);
                }
            }

            return thisElement;
        }


        /// <summary>
        /// Resolves the namespaceUri in accordance with http://www.w3.org/TR/REC-xml-names/ (3), (6.1) and (6.2)
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="localName"></param>
        /// <param name="nameSpaceUri"></param>
        /// <param name="name"></param>
        /// <param name="attDict"></param>
        internal static void ResolveNameSpace(
            ref string prefix, 
            ref string localName, 
            ref string nameSpaceUri, 
            string name, 
            AttributeDictionary<string> attDict,
            HtmlNode currentNode)
        {
            if (prefix != null)
            {
                if (prefix.Equals("XML", StringComparison.InvariantCultureIgnoreCase))
                {
                    // See http://www.w3.org/TR/REC-xml-names/ (3)
                    nameSpaceUri = "http://www.w3.org/XML/1998/namespace";
                    return;
                }

                if (prefix.Equals("XMLNS", StringComparison.InvariantCultureIgnoreCase))
                {
                    // See http://www.w3.org/TR/REC-xml-names/ (3)
                    nameSpaceUri = "http://www.w3.org/2000/xmlns/";
                    return;
                }

                // First see if this namespace is defined in the current element
                if (attDict != null)
                    nameSpaceUri = attDict[string.Format(CultureInfo.InvariantCulture, "xmlns:{0}", prefix), true];

                if (nameSpaceUri == null)
                {
                    Debug.Assert(currentNode != null);

                    // The name space is not defined in the last added element. Try to locate it from the current element
                    // TODO: See exacty how this works in reality
                    nameSpaceUri = currentNode.GetNamespaceOfPrefix(prefix);
                }

                if (nameSpaceUri == null)
                {
                    // If namespace for this prefix could not be located, then
                    // add the element without a prefix. i.e. include the prefix in its name
                    prefix = null;
                    localName = name;
                }
            }
            else
            {
                if (name.Equals("XMLNS", StringComparison.InvariantCultureIgnoreCase))
                {
                    // See http://www.w3.org/TR/REC-xml-names/ (3)
                    nameSpaceUri = "http://www.w3.org/2000/xmlns/";
                    return;
                }

                // NOTE: Ignore the default namespace. This will greatly simplify the XPath queries.

                if (nameSpaceUri == null)
                {
                    // Check for default namespace the last added element.
                    nameSpaceUri = currentNode.GetNamespaceOfPrefix(string.Empty);
                }
            }

            // Making sure no standard HTML namespaces will be included. This will stuff up the XPath queries.
            if ("http://www.w3.org/1999/xhtml".Equals(nameSpaceUri, StringComparison.InvariantCultureIgnoreCase))
                nameSpaceUri = string.Empty;

            if ("http://www.w3.org/TR/REC-html40".Equals(nameSpaceUri, StringComparison.InvariantCultureIgnoreCase))
                nameSpaceUri = string.Empty;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private HtmlNode ParseDocTypeDeclaration()
        {
            // --------------------------------------------------------------------------------------------------------------
            // According to the XML 1.0 specification
            // --------------------------------------------------------------------------------------------------------------
            //[28]    doctypedecl       ::=    '<!DOCTYPE' S Name (S ExternalID)? S? ('[' intSubset ']' S?)? '>'
            //[75]    ExternalID        ::=    'SYSTEM' S SystemLiteral | 'PUBLIC' S PubidLiteral S SystemLiteral 
            //[11]    SystemLiteral     ::=    ('"' [^"]* '"') | ("'" [^']* "'")  
            //[12]    PubidLiteral      ::=    '"' PubidChar* '"' | "'" (PubidChar - "'")* "'" 
            //[13]    PubidChar         ::=    #x20 | #xD | #xA | [a-zA-Z0-9] | [-'()+,./:=?;!*#@$_%] 
            //[28b]   intSubset         ::=    (markupdecl | DeclSep)* 
            //[29]    markupdecl        ::=    elementdecl | AttlistDecl | EntityDecl | NotationDecl | PI | Comment 
            //[28a]   DeclSep           ::=    PEReference | S  
            //[69]    PEReference       ::=    '%' Name ';'  
            // --------------------------------------------------------------------------------------------------------------


            try
            {
                string doctypeStr = ReadChars(7);

                Debug.Assert(doctypeStr.Equals("DOCTYPE", StringComparison.CurrentCultureIgnoreCase));

                if (!SkipWhiteSpaces())
                {
                    // Unfinished "<!DOCTYPE" declaration
                    // TODO: Add the element an ignore it
                    Debug.Assert(false);
                }

                string elementName;

                if (!ReadNamePredicate(out elementName))
                {
                    if (HasMoreChars)
                    {
                        // Unfinished "'<!DOCTYPE' S Name" declaration
                        Debug.Assert(false);
                    }
                    else
                    {
                        // a Name predicate could not be located
                        // TODO: end the element here and start parsing a Text Element?
                        Debug.Assert(false);
                    }
                }

                if (!SkipWhiteSpaces())
                {
                    // Unfinished "'<!DOCTYPE' S Name" declaration
                    Debug.Assert(false);
                }

                string pubidLiteral = null;
                string systemLiteral = null;
                string externalIdName = null;
                HtmlDocTypeElement.IdType externalIdType = HtmlDocTypeElement.IdType.NotSpecified;

                if (SequenceFollowsIgnoreCase("SYSTEM", CharacterClasses.WHITE_SPACES, true, true))
                {
                    externalIdType = HtmlDocTypeElement.IdType.System;
                    externalIdName = "SYSTEM";
                }
                else if (SequenceFollowsIgnoreCase("PUBLIC", CharacterClasses.WHITE_SPACES, true, true))
                {
                    externalIdType = HtmlDocTypeElement.IdType.Public;
                    externalIdName = "PUBLIC";
                }

                SkipWhiteSpaces();

                if (externalIdType == HtmlDocTypeElement.IdType.Public)
                {
                    // Parse the "PubidLiteral" from: 'PUBLIC' S PubidLiteral S SystemLiteral 
                    if (ReadPubidLiteral(out pubidLiteral))
                    {
                        SkipWhiteSpaces();
                    }
                    else
                    {
                        //TODO: So what if we cant read a pubidLiteral??
                        //TODO: Should we be strict about a DOCTYPE declaration or not really?
                        Debug.Assert(false);
                    }
                }

                if (externalIdType != HtmlDocTypeElement.IdType.NotSpecified)
                {
                    // Parse the "SystemLiteral" from: 'PUBLIC' S PubidLiteral S SystemLiteral 
                    //                             or: 'SYSTEM' S SystemLiteral
                    if (!ReadSystemLiteral(out systemLiteral))
                    {
                        AddWarningMessage("The 'SystemLiteral' was not found in a <!DOCTYPE declaration.");
                    }
                }

                if (externalIdType == HtmlDocTypeElement.IdType.NotSpecified)
                {
                    AddWarningMessage("No 'ExternalID' could be located for the DOCTYPE element ('<!DOCTYPE' S Name (S ExternalID)? S? ('[' intSubset ']' S?)? '>').");

                    externalIdName = null;
                }

                if (!SkipWhiteSpaces())
                {
                    // Unfinished "'<!DOCTYPE' S Name" declaration
                    Debug.Assert(false);
                }

                if (CurrentChar == '[')
                {
                    //TODO: Parse ('[' intSubset ']' S?)?
                    Debug.Assert(false);
                    throw new NotImplementedException("<!DOCTYPE: ('[' intSubset ']' S?)? parser not implemented yet!");


                    SkipWhiteSpaces();
                }

                // It is also possible that we have a comment at the end of the tag
                // Try to read a comment, which will be added as a value later on
                string comment = null;
                ReadComment(out comment);

                if (CurrentChar == '>')
                {
                    ReadChar();
                }
                else
                {
                    // We are not strict about an ending ">". Just ignore it and move to the next element
                    Debug.Assert(false);
                }

                // TODO: add the intSubset when implemented
                // TODO: add more system literals in the case of free ExternalId parsing
                HtmlDocTypeElement node = new HtmlDocTypeElement(elementName, externalIdName, pubidLiteral, systemLiteral, null, m_Document);

                if (!string.IsNullOrEmpty(comment))
                {
                    // Add comments as a value
                    node.m_Comment = comment;
                }

                return node;
            }
            catch (EndOfStreamException)
            {
                // This shouldn't happen!
                Debug.Assert(false);
                throw;
            }
        }

        private HtmlNode ProcessComment(string comment)
        {
            HtmlCommentElement commentEl = new HtmlCommentElement(m_Document, comment);
            return commentEl;
        }

        private void AddWarningMessage(string message)
        {
            Debug.Assert(m_Document != null);

            if (m_Document != null && m_Document.m_OnParserWarning != null)
                m_Document.m_OnParserWarning(this, new ParserWarningEventArgs(message, base.CurrentLineNo, base.CurrentColNo));
        }


        private void AddTextElement(string textContent, NodePosition parsedPosition)
        {
            if (textContent != null)
                textContent = HttpUtility.HtmlDecode(textContent);

            HtmlTextElement newElement = new HtmlTextElement(m_Document, textContent, parsedPosition);

            Debug.Assert(m_CurrentNode != null);

            if (InsertSpecialElementsIfNeeded(newElement))
                m_CurrentNode.AppendChild(newElement);
        }

        #region IHtmlParserStatus Members

        HtmlDocument IHtmlParserStatus.Document
        {
            get { return m_Document; }
        }

        HtmlNode IHtmlParserStatus.CurrentNode
        {
            get { return m_CurrentNode; }
            set { m_CurrentNode = value; }
        }

        HtmlNode IHtmlParserStatus.LastAddedElement
        {
            get { return m_LastAddedElement; }
            set { m_LastAddedElement = value; }
        }

        Stack<HtmlNode> IHtmlParserStatus.CurrentNodeStack 
        {
            get { return m_CurrentNodeStack; }
        }

        #endregion
    }
}
