using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Acrux.Html.Specialized.Html401;
using System.Web;

namespace Acrux.Html.Specialized
{
    internal enum FixupType
    {
        None,
        UpdateAttribute,
        Add,
        Remove,
        UpdateElement
    }

    internal class HtmlNodeFixup
    {
        internal readonly FixupType Type;
        internal readonly HtmlNode HtmlNode;
        internal string OriginalValue;
        internal char ValueEnclosingCharacter;

        private HtmlNodeFixup()
        { }

        internal HtmlNodeFixup(FixupType type, HtmlNode node, string originalValue)
        {
            Type = type;
            HtmlNode = node;
            OriginalValue = originalValue;

            if (node is HtmlAttribute)
            {
                if (node.NodePosition.ValueStartPos < 0)
                    ValueEnclosingCharacter = '\"';
                else if (node.OwnerDocument.RawHtml.Length > node.NodePosition.ValueStartPos)
                {
                    ValueEnclosingCharacter = node.OwnerDocument.RawHtml[node.NodePosition.ValueStartPos - 1];
                    if ("'\"".IndexOf(ValueEnclosingCharacter) == -1) ValueEnclosingCharacter = '\x0';
                }
            }
        }
    }

    internal class FixupManager : IComparer<HtmlNodeFixup>
    {
        private List<HtmlNodeFixup> m_AllFixups = new List<HtmlNodeFixup>();

        private FixupType GetFixUpType(HtmlNode node)
        {
            foreach (HtmlNodeFixup fixUp in m_AllFixups)
            {
                if (fixUp.HtmlNode.Equals(node))
                    return fixUp.Type;
            }

            return FixupType.None;
        }

        internal HtmlNodeFixup AddOrUpdateFixUp(FixupType type, HtmlNode node, string originalValue)
        {
            HtmlNodeFixup fixUp = null;

            foreach (HtmlNodeFixup probeFixUp in m_AllFixups)
            {
                if (probeFixUp.HtmlNode.Equals(node))
                {
                    fixUp = probeFixUp;

                    Debug.Assert(type == fixUp.Type);

                    if (type != fixUp.Type)
                        throw new InvalidOperationException("Cannot update object with different fix-up type.");

                    break;
                }
            }

            if (fixUp == null)
            {
                fixUp = new HtmlNodeFixup(type, node, originalValue);
                m_AllFixups.Add(fixUp);
            }
            else
            {
                Debug.Assert(fixUp.OriginalValue == originalValue);
                // Nothing to do, the value will be retrieved via the HtmlNode
            }

            return fixUp;
        }

        internal string ApplyFixUps(HtmlDocument htmlDoc)
        {
            if (m_AllFixups.Count == 0)
                return htmlDoc.RawHtml;

            // Sort the fixup based on their starting position.
            m_AllFixups.Sort(this);

            StringBuilder outputHtml = new StringBuilder();
            string rawHtml = htmlDoc.RawHtml;

            int lastEndPosition = 0;

            for (int i = 0; i < m_AllFixups.Count; i++)
            {
                HtmlNodeFixup fixup = m_AllFixups[i];

                if (fixup.Type == FixupType.UpdateAttribute)
                {
                    outputHtml.Append(rawHtml.Substring(lastEndPosition, fixup.HtmlNode.NodePosition.ValueStartPos - lastEndPosition - 1));

                    // See if the char has to be skipped
                    char encBeg = rawHtml[fixup.HtmlNode.NodePosition.ValueStartPos - 1];
                    if (encBeg != '\'' && encBeg != '"') outputHtml.Append(rawHtml[fixup.HtmlNode.NodePosition.ValueStartPos - 1]);

                    // Add an opening quote if needed
                    bool encAdded = false;
                    if (fixup.ValueEnclosingCharacter != '\x0')
                    {
                        outputHtml.Append(fixup.ValueEnclosingCharacter);
                        encAdded = true;
                    }

                    // Add the value and make sure to escape the double quotes
                    outputHtml.Append(HttpUtility.HtmlEncode(fixup.HtmlNode.Value));

                    // Add the closing quote is needed
                    if (encAdded) outputHtml.Append(fixup.ValueEnclosingCharacter);

                    // See if need to remove closing quote
                    char encEnd = rawHtml[fixup.HtmlNode.NodePosition.ValueEndPos + 1];
                    if (encBeg != '\'' && encBeg != '"')
                    {
                        outputHtml.Append(rawHtml[fixup.HtmlNode.NodePosition.ValueEndPos + 1]);
                        lastEndPosition = fixup.HtmlNode.NodePosition.ValueEndPos + 1;
                    }
                    else
                        lastEndPosition = fixup.HtmlNode.NodePosition.ValueEndPos + 2;
                }
                else if (fixup.Type == FixupType.Remove)
                {
                    outputHtml.Append(rawHtml.Substring(lastEndPosition, fixup.HtmlNode.NodePosition.NameStartPos - lastEndPosition));
                    lastEndPosition = fixup.HtmlNode.NodePosition.ValueEndPos + 1;
                }
                else if (fixup.Type == FixupType.Add)
                {
                    // The "Add" fixups are added as first attributes after their parent name
                    bool hasModeAddFixupsForTheSameElement = false;
                    do
                    {
                        fixup = m_AllFixups[i];

                        outputHtml.Append(rawHtml.Substring(lastEndPosition, fixup.HtmlNode.ParentNode.NodePosition.NameEndPos - lastEndPosition + 1));
                        outputHtml.Append(" ");
                        outputHtml.Append(fixup.HtmlNode.Name);

                        // For added fixups we always use double quotes as value enclosure
                        outputHtml.Append("=\"");

                        // Add the value and make sure to escape the double quotes
                        outputHtml.Append(HttpUtility.HtmlEncode(fixup.HtmlNode.Value));

                        // For added fixups we always use double quotes as value enclosure
                        outputHtml.Append("\"");

                        // If there are more added attributes for this node, then all of them will 
                        // appear with the same sort index. So check the next nodes and apply fixups if needed
                        hasModeAddFixupsForTheSameElement = 
                            i + 1 < m_AllFixups.Count &&
                            m_AllFixups[i + 1].Type == FixupType.Add &&
                            m_AllFixups[i + 1].HtmlNode.ParentNode.Equals(fixup.HtmlNode.ParentNode);

                        if (hasModeAddFixupsForTheSameElement) i++;
                    }
                    while (hasModeAddFixupsForTheSameElement);

                    lastEndPosition = fixup.HtmlNode.ParentNode.NodePosition.NameEndPos + 1;
                }
                else if (fixup.Type == FixupType.UpdateElement)
                {
                    if (
                        fixup.HtmlNode is HtmlAttribute ||
                        fixup.HtmlNode is HtmlCommentElement ||
                        fixup.HtmlNode is HtmlTextElement)
                    {
                        outputHtml.Append(rawHtml.Substring(lastEndPosition, fixup.HtmlNode.NodePosition.NodeStartPos - lastEndPosition));
                        outputHtml.Append(fixup.OriginalValue);
                        lastEndPosition = fixup.HtmlNode.NodePosition.NodeEndPos + 1;
                    }
                    else
                        throw new NotSupportedException("Removing this node type is not supported.");
                }
            }

            if (lastEndPosition < rawHtml.Length - 1)
                outputHtml.Append(rawHtml.Substring(lastEndPosition));

            return outputHtml.ToString();
        }


        #region IComparer<HtmlNodeFixup> Members

        int IComparer<HtmlNodeFixup>.Compare(HtmlNodeFixup x, HtmlNodeFixup y)
        {
            if (x == null)
                throw new ArgumentNullException("x");

            if (y == null)
                throw new ArgumentNullException("y");

            // On of the two fix-ups is an "Add" fixup. Its position will be the position of the last attribute of it's parent
            // We compare the begining of the attribute name with the end of the element name (for FixupType.Add). All added 
            // attributes will be inserted as first attributes after the element name (for simplicity for the computations)

            int xPos;
            int yPos;

            if (x.Type == FixupType.Add)
                xPos = x.HtmlNode.ParentNode.NodePosition.NameEndPos;
            else if (x.Type == FixupType.UpdateElement)
                xPos = x.HtmlNode.NodePosition.NodeStartPos;
            else if (x.Type == FixupType.UpdateAttribute)
                xPos = x.HtmlNode.NodePosition.ValueStartPos;
            else
                xPos = x.HtmlNode.NodePosition.NameStartPos;

            if (y.Type == FixupType.Add)
                yPos = y.HtmlNode.ParentNode.NodePosition.NameEndPos;
            else if (y.Type == FixupType.UpdateElement)
                yPos = y.HtmlNode.NodePosition.NodeStartPos;
            else if (x.Type == FixupType.UpdateAttribute)
                yPos = y.HtmlNode.NodePosition.ValueStartPos;
            else
                yPos = y.HtmlNode.NodePosition.NameStartPos;

            return xPos.CompareTo(yPos);
        }

        #endregion
    }
}
