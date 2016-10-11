using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;

namespace Acrux.Html.Specialized
{
    internal class XmlRefProcessingInstruction : XmlProcessingInstruction, IHtmlNodeReferenceHolder
    {
        private HtmlNode m_Reference = null;

        internal XmlRefProcessingInstruction(string target, string data, XmlDocument doc, HtmlNode reference)
            : base(target, data, doc)
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
