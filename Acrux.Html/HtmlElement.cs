using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Acrux.Html.Specialized;
using System.Diagnostics;

namespace Acrux.Html
{
    internal enum ElementClosingTagRequirement
    {
        Required,
        Optional,
        Forbidden
    }

    internal enum OptionalClosingTagType
    {
        None,
        MustOnlyContainSpecifiedTags,
        MustBeOnlyContainedInsideSpeficiedTags
    }

    internal enum EndTagWithoutStartTagHandling
    {
        Ignore,
        HandleAsStartTag
    }

    /// <summary>
    /// Represents an HTML element.
    /// </summary>
    public class HtmlElement : HtmlNode
    {
        private bool m_IsEmptyTag = false;

        #region Constructors

        internal HtmlElement(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
            : base(XmlNodeType.Element, prefix, localName, namespaceURI, doc, parsedPosition)
        {
            m_IsEmptyTag = isEmptyTag;

            base.OnChildAppending += new OnChildAppendingDelegate(ChildAppending);
        }
        #endregion


        #region Structure Definition

        internal virtual ElementClosingTagRequirement ClosingTagRequirement
        { get { return ElementClosingTagRequirement.Required; } }

        internal virtual bool IsHeadLevelElement
        { get { return false; } }

        /// <summary>
        /// According to the block-level and inline (text-level) definitions in HTML 4.01 Specs
        /// http://www.w3.org/TR/html401/struct/global.html#block-inline
        /// Generally, inline elements may contain only data and other inline elements.
        /// </summary>
        internal virtual bool IsInlineElement
        { get { return false;  } }

        /// <summary>
        /// When <b>false</b>: actually finding a nested element will cause the parser<br/>
        /// to close the previous element and create the new one in the same level<br/>
        /// </summary>
        internal virtual bool AllowNestedElements
        { get { return true;  } }

        /// <summary>
        /// There are exactly 3 structure module elements: HTML, HEAD and BODY.
        /// The TITLE will not be considered a structure module element in this parser
        /// because it does not have a special meaning for the parser despite the fact 
        /// that TITLE is actually a Structure Module element according to the XHTML specs.
        /// </summary>
        internal virtual bool IsStructureModuleElement
        { get { return false; } }


        internal virtual OptionalClosingTagType OptionalClosingTagMode
        { get { return OptionalClosingTagType.None; } }


        internal virtual EndTagWithoutStartTagHandling EndTagWithoutStartTagMode
        { get { return EndTagWithoutStartTagHandling.Ignore; } }


        internal virtual bool IsDeprecated
        { get { return false; } }

        internal virtual string[] OptionalClosingTagSpecifiedTags
        { get { return null; } }

        // TODO: [MOVE TO TASK LIST] see if "style = "display: block|inline" changes anything ??
        //
        // Variations of current elements with optional or required end tags
        //
        // -----------------------------------------------------------------------------------------------
        // Test expression: <CURR> before <SPECIAL> in </SPECIAL> after </CURR>
        // -----------------------------------------------------------------------------------------------
        // Results and types:
        // 
        // (1) - No changes to the structure (depending on the allowance of a closing tag for the 
        //       SPECIAL element
        // ----------------------------------------------------------------------------------------------
        // SEPCIAL: Optional / Required end tag   |              Forbidden end tag
        // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -
        // <CURR>                                                <CURR>
        //    before                                                before
        //    <SPECIAL> in </SPECIAL>          -- or --             <SPECIAL/> 
        //    after                                                 in  after 
        // </CURR>                                               </CURR>
        //
        //
        // (1-A) - Same as (1) for forbidden tag, but the closing tag is parsed as a new tag 
        //         example: <a> before <br> in </br> after </a> 
        // (1-B)   <a> before <select> in </select> after </a>        
        // (1-C)   <a> before <table> in </table> after </a>       
        // ----------------------------------------------------------------------------------------------
        //  1-A                          1-B                  1-C
        //
        // <CURR>                        <CURR>               <CURR> 
        //    before                     before               before
        //    <SPECIAL/>                 <SPECIAL/>           in
        //    in                         after                <SPECIAL/> 
        //    <SPECIAL/>                 </CURR>              after
        //    after                                           </CURR>
        // </CURR>
        //
        // ----------------------------------------------------------------------------------------------
        //
        //
        // (2) - Totally ignoring the SPECIAL tag, but keeping its inner content
        //     - Or moving the tag to the HEAD element (when it is a head tag)
        // ----------------------------------------------------------------------------------------------
        // SPECIAL is BODY element               |                Head element
        // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -
        // <CURR> before in after </CURR>                     <CURR> before after </CURR>
        //
        // ----------------------------------------------------------------------------------------------
        //
        //
        // (3) - Closes the current tag and creates SPECIAL at the same level as the current
        // ----------------------------------------------------------------------------------------------
        // SEPCIAL: Optional / Required end tag   |              Forbidden end tag
        // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -
        // <CURR> before </CURR>                            <CURR> before </CURR>
        // <SPECIAL> in </SPECIAL>             -- or --     <SPECIAL/>
        // after                                             in after
        //
        // ----------------------------------------------------------------------------------------------
        //                       
        //
        // (4) - Weired one (<a> before <li> in </li> after </a>)
        //                  (<a> before <param> in </param> after </a>)
        // ----------------------------------------------------------------------------------------------
        // SEPCIAL: Optional / Required end tag   |              Forbidden end tag
        // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -
        // <CURR> before </CURR>                                <CURR> before </CURR>           
        // <SPECIAL>                           -- or --         <SPECIAL/>
        //    <CURR> in </CURR>                                 <CURR> in after </CURR>
        // </SPECIAL>
        // <CURR> after </CURR>
        //
        //

        internal virtual List<string> ResetTags
        { get { return null; } }

        internal virtual List<string> IgnoreTags
        { get { return null; } }

        //internal bool IsBlockLevelElement
        //{
        //    get
        //    {
        //        return !IsHeadLevelElement && !IsInlineElement;
        //    }
        //}
        #endregion


        /// <summary>
        /// Returns <b>true</b> if the element is an empty element tag ('&lt;' Name (S Attribute)* S? '/&gt;')
        /// </summary>
        internal virtual bool IsEmptyElementTag
        {
            get
            {
                return m_IsEmptyTag;
            }
        }

        internal virtual bool IsEndTag
        {
            get
            {
                // A static boolean is faster than an "is" type check to determine whether this is an EndTag
                return false;
            }
        }

        //public bool HasClosingTag
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public int ClosingTagStartsAt
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public int ClosingTagEndsAt
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        /// <summary>
        /// Returns the value of the specified attribute of the current HTML element. The attribute names are in lower case.
        /// </summary>
        /// <param name="attributeName">The attribute name.</param>
        /// <returns>Returns the value of the attribute if present or null.</returns>
        protected string GetAttributeValue(string attributeName)
        {
            if (base.Attributes[attributeName] != null)
                return base.Attributes[attributeName].Value;
            else
                return null;
        }

        /// <summary>
        /// Removes the specified attribute if exists.
        /// </summary>
        /// <param name="attributeName">The attribute to be removed</param>
        /// <returns><b>true</b> if the attribute has been removed.</returns>
        public bool RemoveAttribute(string attributeName)
        {
            return this.Attributes.Remove(attributeName);
        }

        /// <summary>
        /// Removes the specified child element if exists.
        /// </summary>
        /// <param name="childNode">The child to be removed</param>
        /// <returns><b>true</b> if the child has been removed.</returns>
        public bool RemoveChild(HtmlNode childNode)
        {
            // TODO: Do the trick. We need this to remove the JavaScrip content so we can do automated testing in Firefox

            // TODO: Just add a simple flag whether to include it in the output HTML or not
            //       Make sure the node is wellformed and well defined i.e. where is the closing tag? Only remove the child if it doesn't have children
            //       If this is a TEXT node then special handling maybe (if any benefit)
            //       Do we remember the positions of the node: start tag start <, end tag end >, text start pos and end pos

            throw new NotImplementedException();
        }


        /// <summary>
        /// Removes all specified attributes and children of the current node.
        /// </summary>
        public void RemoveAll()
        {
            List<HtmlNode> children = new List<HtmlNode>();
            List<string> attNames = new List<string>();

            foreach (HtmlAttribute att in this.Attributes)
                attNames.Add(att.Name);

            foreach (HtmlNode child in this.ChildNodes)
                children.Add(child);


            foreach (string attName in attNames)
                RemoveAttribute(attName);

            foreach (HtmlNode child in children)
                RemoveChild(child);
        }

        private void ChildAppending(HtmlNode child, ref OnChildAppendingEventArgs e)
        {
            //Debug.Assert(child != null);

            //if (child != null && child is HtmlElement)
            //{
            //    if ((child as HtmlElement).IsStructureModuleElement)
            //    {
            //        Debug.Assert(child.Name != null);

            //        if (child.Name.Equals("BODY", StringComparison.CurrentCultureIgnoreCase))
            //        {
            //            // This is a <BODY> tag
            //            if (OwnerDocument.BodyElement == null)
            //                OwnerDocument.m_BodyElement = child as HtmlElement;
            //            else
            //            {
            //                // Don't add a second body tag, ignore it!
            //                e.Cancel = true;
                            
            //                Debug.WriteLine("Ignoring a BODY tag because another BODY was already found at " + OwnerDocument.BodyElement.XPathLocation);
            //            }
            //        }

            //        if (child.Name.Equals("HEAD", StringComparison.CurrentCultureIgnoreCase))
            //        {
            //            // This is a <HEAD> tag
            //            if (OwnerDocument.HeadElement == null)
            //                OwnerDocument.m_HeadElement = child as HtmlElement;
            //            else
            //            {
            //                // Don't add a second head tag, ignore it!
            //                e.Cancel = true;

            //                Debug.WriteLine("Ignoring a HEAD tag because another HEAD was already found at " + OwnerDocument.HeadElement.XPathLocation);
            //            }
            //        }

            //        if (child.Name.Equals("HTML", StringComparison.CurrentCultureIgnoreCase))
            //        {
            //            // This is a <HTML> tag
            //            if (OwnerDocument.HtmlElement == null)
            //            {
            //                Debug.Assert(false);
            //                throw new HtmlParserException();

            //                // We should never come here. The HTML elements are handled by the HtmlParser's ProcessHtmlElement() routine
            //            }
            //            else
            //            {
            //                // Don't add a second html tag, ignore it!
            //                e.Cancel = true;

            //                Debug.WriteLine("Ignoring an HTML tag because another HTML was already found at " + OwnerDocument.HtmlElement.XPathLocation);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    // It could be a comment, <%@, <![ ... etc node
            //}
        }
    }
}
