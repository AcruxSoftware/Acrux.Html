using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;

namespace Acrux.Html.Specialized
{
    internal sealed class XmlRefComment : XmlComment, IHtmlNodeReferenceHolder
    {
        private HtmlNode m_Reference = null;

        internal XmlRefComment(string comment, XmlDocument doc, HtmlNode reference)
            : base(comment, doc)
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
