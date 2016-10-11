using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;

namespace Acrux.Html.Specialized
{
    internal sealed class XmlRefText : XmlText, IHtmlNodeReferenceHolder
    {
        private HtmlNode m_Reference = null;

        internal XmlRefText(string strData, XmlDocument doc, HtmlNode reference)
            : base(strData, doc)
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
