using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;

namespace Acrux.Html.Specialized
{
    internal sealed class XmlRefXmlDeclaration : XmlDeclaration, IHtmlNodeReferenceHolder
    {
        private HtmlNode m_Reference = null;

        internal XmlRefXmlDeclaration(XmlDocument doc, HtmlNode reference)
            : base("1.0", "utf-8", null, doc)
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
        //        return null;
        //    }
        //}
    }
}
