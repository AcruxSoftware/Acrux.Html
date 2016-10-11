using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized
{
    internal interface IHtmlNodeReferenceHolder
    {
        HtmlNode HtmlNodeReference { get; }
        //string CaseSensitiveName { get; }
    }
}
