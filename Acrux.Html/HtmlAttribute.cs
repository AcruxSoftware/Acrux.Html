using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Acrux.Html.Specialized;
using System.Web;

namespace Acrux.Html
{
    /// <summary>
    /// Represents an HTML attribute.
    /// </summary>
    public sealed class HtmlAttribute : HtmlNode
    {
        private List<string> m_DuplicatedValues = null;

        internal HtmlAttribute(string prefix, string localName, string namespaceURI, HtmlDocument doc, NodePosition parsedPosition, string attValue, bool decodeValue)
            : base(XmlNodeType.Attribute, prefix, localName, namespaceURI, doc, parsedPosition)
        {
            if (prefix == "xmlns" && string.IsNullOrEmpty(attValue))
            {
                attValue = "http://schemas.acruxsoftware.net/missing-namespace-uri";

                m_NodePosition = NodePosition.ReadOnly;
            }

            if (decodeValue)
                m_XmlNode.Value = HttpUtility.HtmlDecode(attValue);
            else
                m_XmlNode.Value = attValue;
        }

        internal void AddDuplicatedValues(IEnumerable<string> allValues)
        {
            if (m_DuplicatedValues == null)
                m_DuplicatedValues = new List<string>();

            m_DuplicatedValues.AddRange(allValues);
        }

        internal bool HasDuplicatedValues
        {
            get { return m_DuplicatedValues != null; }
        }

        internal IEnumerable<string> AllDuplicatedValues
        {
            get { return m_DuplicatedValues;  }
        }

     }
}
