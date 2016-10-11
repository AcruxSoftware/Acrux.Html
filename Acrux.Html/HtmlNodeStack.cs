using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Acrux.Html
{
    internal class HtmlNodeStack : Stack<HtmlNode>
    {
        private HtmlParser m_Parser;
        private HtmlParserSettings m_ParserSettings;
        private HtmlDocument m_Document;

        internal HtmlNodeStack(HtmlParser parser, HtmlDocument document, HtmlParserSettings parserSettings)
        {
            m_Parser = parser;
            m_ParserSettings = parserSettings;
            m_Document = document;
        }
        
        public new HtmlNode Pop()
        {
            HtmlNode node = base.Pop();

            ApplyParseTimeStructureFixups(node as HtmlElement);

            return node;
        }

        private void ApplyParseTimeStructureFixups(HtmlElement popedUpNode)
        {
            if (popedUpNode == null) return;

            // If this is a HEAD element outside the HEAD then make sure to add it in the HEAD
            if (popedUpNode.IsHeadLevelElement &&
                popedUpNode.ParentNode != null &&
                popedUpNode.ParentNode.Name != "head")
            {
                // Add it to the HEAD and make it current, in case it has text node associated

                // Make sure we get the head with XPath. The m_Document.HeadElement may be null
                // if the head is not found yet

                HtmlElement headElement = (HtmlElement)m_Document.SelectSingleNode("/html/head");

                if (headElement != null)
                {
                    HtmlNode lastHeadNode = headElement.ChildNodes.Count > 0 ? headElement.ChildNodes[headElement.ChildNodes.Count - 1] : null;
                    headElement.InsertBefore(popedUpNode, lastHeadNode);

                    return;
                }
            }

            if (m_ParserSettings.NoScriptsBeforeBodyAddedToHtml &&
                m_Document.BodyElement == null &&
                popedUpNode.Name == "noscript")
            {
                // All noscripts found before the BODY is parsed go to the HTML element

                HtmlNode htmlNode = m_Document.SelectSingleNode("/html");
                HtmlNode fakeBodyNode = m_Document.SelectSingleNode("/html/body");

                if (htmlNode != null && fakeBodyNode != null)
                {
                    htmlNode.InsertBefore(popedUpNode, fakeBodyNode);
                    return;
                }
            }
        }

        public new void Push(HtmlNode item)
        {
            base.Push(item);
        }
    }
}
