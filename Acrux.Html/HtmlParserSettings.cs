using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html
{
    internal class HtmlParserSettings
    {
        internal static HtmlParserSettings AcruxSettings = new HtmlParserSettings(false);
        internal static HtmlParserSettings FirefoxSettings = new HtmlParserSettings(true);

        internal bool BuildMissingTableElements = true;
        internal bool DontIgnoreTags = true;

        internal bool DontResetFormTag = false;  /* Firefox 2.0.0.14 version is FALSE i.e. the Form is reset if in between TABLE and TR for example */
        internal bool AddINPUTsToFirstOuternTD = true; /* Firefox 2.0.0.14 version is TRUE */
        internal bool UseEmptyFormTags = true; /* Firefox 2.0.0.17 version is TRUE */
        internal bool NoScriptsBeforeBodyAddedToHtml = true; /* Firefox 2.0.0.17 version is TRUE */
        internal bool AddKnownHtmlTagsToKnownHtmlTagsOnly = true; /* Firefox 2.0.0.17 version is TRUE */

        private HtmlParserSettings(bool isFirefox)
        {
            if (!isFirefox)
            {
                BuildMissingTableElements = false;
                DontResetFormTag = true;
                AddINPUTsToFirstOuternTD = false;
                UseEmptyFormTags = false;
                NoScriptsBeforeBodyAddedToHtml = false;

                // TODO: Implement the other tags (microsoft, netscape, sun)
                //       Create tests to see if they are considered unknown

                AddKnownHtmlTagsToKnownHtmlTagsOnly = false;
            }
        }
    }
}
