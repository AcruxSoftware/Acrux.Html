using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;

namespace Acrux.Html.Specialized
{
    internal sealed class XmlRefDocumentType : XmlDocumentType, IHtmlNodeReferenceHolder
    {
        private HtmlNode m_Reference = null;

        internal XmlRefDocumentType(string name, string publicId, string systemId, string internalSubset, XmlDocument doc, HtmlNode reference)
            : base(name, publicId, systemId, internalSubset, doc)
        {
            Debug.Assert(reference != null);

            m_Reference = reference;
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
        //        return base.Name;
        //    }
        //}

        // TODO: All elements stored in the XmlDocument must be XmlRefElements so the XPath queries will return XmlNodes which
        //       can be casted to XmlRefElement/XmlRefAttribute and then the HtmlElement/HtmlAttribute retrieved from there
        //
        //       There will be a base class HtmlNode which delegates the exposed XmlNode methods to the private XmlNode member
        //       then casts it to XmlRefElement/XmlRefAttribute  and returns the HtmlElement/HtmlAttribute
    }
}
