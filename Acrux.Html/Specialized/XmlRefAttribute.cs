using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Globalization;

namespace Acrux.Html.Specialized
{
    internal sealed class XmlRefAttribute : XmlAttribute, IHtmlNodeReferenceHolder
    {
        private HtmlNode m_Reference = null;
        //private string m_CaseSensitiveName = null;

        internal XmlRefAttribute(string prefix, string localName, string namespaceURI, XmlDocument doc, HtmlNode reference)
            // NOTE: xml: and xmlns: prefxes as well as the xmlns name are set as lower case always
            //       See HtmlParser.ResolveNameSpace() for details.
            //       for now xmlns: prefixes are ignored !!! See how to handle them??
            : base( /*"xmlns".Equals(prefix) ? null : */prefix, 
                   "xml".Equals(prefix) || "xmlns".Equals(prefix) || "xmlns".Equals(localName) ? localName : localName.ToLower(CultureInfo.InvariantCulture),
                   /*"xmlns".Equals(prefix) ? string.Empty : */namespaceURI, 
                   doc)
        {
            Debug.Assert(reference != null);

            m_Reference = reference;
           // m_CaseSensitiveName = localName;
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
    }
}
