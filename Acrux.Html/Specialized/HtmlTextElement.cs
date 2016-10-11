using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Acrux.Html.Specialized
{
    public sealed class HtmlTextElement : HtmlNode
    {
        internal HtmlTextElement(HtmlDocument htmlDoc, string textData, NodePosition parsedPosition)
            : base(XmlNodeType.Text, textData, htmlDoc, parsedPosition)
        { 
        }
    }
}
