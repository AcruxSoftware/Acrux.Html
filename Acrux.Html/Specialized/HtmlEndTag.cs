using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;


namespace Acrux.Html.Specialized
{
    internal class HtmlEndTag : HtmlElement
    {
        internal HtmlEndTag(string localName, HtmlDocument doc)
            // NOTE: EndTags (which are not empty i.e. don't have attributes cannot be edited)
            : base(null, localName, null, doc, false, NodePosition.ReadOnly)
        { }

        internal HtmlEndTag(string prefix, string localName, HtmlDocument doc)
            // NOTE: EndTags (which are not empty i.e. don't have attributes cannot be edited)
            : base(prefix, localName, null, doc, false, NodePosition.ReadOnly)
        { }

        internal override bool IsEndTag
        {
            get
            {
                // A static boolean is faster than an "is" type check to determine whether this is an EndTag
                return true;
            }
        }

        internal override bool IsEmptyElementTag
        {
            get
            {
                return false;
            }
        }
    }
}
