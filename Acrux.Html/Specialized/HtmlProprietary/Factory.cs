using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Diagnostics;

namespace Acrux.Html.Specialized.HtmlProprietary
{
    internal static class Factory
    {
        public static HtmlElement CreateHtmlProprietaryElement(
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

            if (upperName.Equals("NOBR"))
                /* REQUIRED; BLOCK-LEVEL; NO-NESTING */
                return new HtmlProprietary.NoLineBreak(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("WBR"))
                /* FORBIDDEN; INLINE; NO-NESTING*/
                return new HtmlProprietary.WordLineBreak(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            return null;
        }

    }
}
