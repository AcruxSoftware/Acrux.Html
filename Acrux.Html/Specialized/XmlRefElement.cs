using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Globalization;

namespace Acrux.Html.Specialized
{
    internal sealed class XmlRefElement : XmlElement, IHtmlNodeReferenceHolder
    {
        private HtmlNode m_Reference = null;
        private string m_CaseSensitiveName = null;

        internal XmlRefElement(string prefix, string localName, string namespaceURI, XmlDocument doc, HtmlNode reference)
            : base(prefix, localName.ToLower(CultureInfo.InvariantCulture), namespaceURI, doc)
        {
            Debug.Assert(reference != null);

            m_Reference = reference;
            m_CaseSensitiveName = localName;
        }

        HtmlNode IHtmlNodeReferenceHolder.HtmlNodeReference
        {
            get
            {
                
                return m_Reference;
            }
        }

        //string IHtmlNodeReferenceHolder.CaseSensitiveName
        //{
        //    get
        //    {
        //        return m_CaseSensitiveName;
        //    }
        //}

        // TODO: All elements stored in the XmlDocument must be XmlRefElements so the XPath queries will return XmlNodes which
        //       can be casted to XmlRefElement/XmlRefAttribute and then the HtmlElement/HtmlAttribute retrieved from there
        //
        //       There will be a base class HtmlNode which delegates the exposed XmlNode methods to the private XmlNode member
        //       then casts it to XmlRefElement/XmlRefAttribute  and returns the HtmlElement/HtmlAttribute

    }
}
