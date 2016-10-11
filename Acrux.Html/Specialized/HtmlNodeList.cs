using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;

namespace Acrux.Html.Specialized
{
    public sealed class HtmlNodeList : IEnumerable<HtmlNode>
    {
        internal List<HtmlNode> m_List = null;

        internal HtmlNodeList(XmlNodeList nodeList)
        {
            Debug.Assert(nodeList != null);

            m_List = new List<HtmlNode>();

            foreach (XmlNode node in nodeList)
            {
                IHtmlNodeReferenceHolder nodeRef = node as IHtmlNodeReferenceHolder;

                if (nodeRef != null)
                    m_List.Add(nodeRef.HtmlNodeReference);
                else
                    throw new HtmlParserException("Unexpected node type!");
            }
        }

        IEnumerator<HtmlNode> IEnumerable<HtmlNode>.GetEnumerator()
        {
            Debug.Assert(m_List != null);

            return m_List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            Debug.Assert(m_List != null);

            return m_List.GetEnumerator();
        }

       
        public int Count 
        {
            get
            {
                Debug.Assert(m_List != null);

                return m_List.Count;
            }
        }

        public HtmlNode this[int index] 
        {
            get
            {
                Debug.Assert(m_List != null);

                return m_List[index];
            }
        }

    }
}
