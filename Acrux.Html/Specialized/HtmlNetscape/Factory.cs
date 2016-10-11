using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Diagnostics;

namespace Acrux.Html.Specialized.HtmlNetscape
{
    internal static class Factory
    {
        public static HtmlElement CreateHtmlNetscapeElement(
            string prefix, 
            string localName, 
            string namespaceURI, 
            HtmlDocument doc,
            bool isEmptyTag, 
            NodePosition parsedPosition)
        {
            if (!string.IsNullOrEmpty(prefix))
                // We dont create elements with prefixes. All standard HTML elements must not have prefixes.
                // A default namespace could be used though.
                return null;

            string upperName = localName.ToUpper(CultureInfo.InvariantCulture);

            if (upperName.Equals("SERVER"))
                /* REQUIRED; BLOCK-LEVEL; NO-NESTING; CM_HEAD|CM_MIXED|CM_BLOCK|CM_INLINE */
                return new HtmlNetscape.ServerScript(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);


            return null;
        }
    }
}
