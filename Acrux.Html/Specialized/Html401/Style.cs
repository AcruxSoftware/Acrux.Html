using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Acrux.Html.Specialized.Html401
{
    /// <summary>
    /// &lt;!ELEMENT STYLE - - %StyleSheet        -- style info --&gt;
    /// &lt;!ATTLIST STYLE
    ///   %i18n;                               -- lang, dir, for use with title --
    ///   type        %ContentType;  #REQUIRED -- content type of style language --
    ///   media       %MediaDesc;    #IMPLIED  -- designed for use with these media --
    ///   title       %Text;         #IMPLIED  -- advisory title --
    ///   &gt;
    /// 
    /// Attributes defined elsewhere
    ///
    ///lang (language information), dir (text direction) 
    ///title (element title) 
    ///
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "STYLE")]
    public sealed class Style : HtmlElement
    {
        private const string LANGUAGE_CSS = "TEXT/CSS";

        internal Style(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
            : base(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition)
        {
            //TODO: Think about how to parse CSS and also JS/VBS. Should it be parsed 
            //      with the whole element at once, or should it set a "mode" for parsing the TEXT content of the node

            string language = base.Attributes["type"] != null ? base.Attributes["type"].Value : null;
            if (string.IsNullOrEmpty(language))
                language = LANGUAGE_CSS;

            if (!language.Equals(LANGUAGE_CSS, StringComparison.InvariantCultureIgnoreCase))
            {
                //NOTE: We don't want to stop or crash on not well formed HTML so we use CSS language by default
                language = LANGUAGE_CSS;
                //    throw new HtmlParserException(string.Format(CultureInfo.InvariantCulture, "Unsupported language '{0}' for a <STYLE> element!", language));
            }
        }


        #region Structure Definition

        internal override ElementClosingTagRequirement ClosingTagRequirement
        { get { return ElementClosingTagRequirement.Required; } }

        internal override bool AllowNestedElements
        { get { return false; } }

        internal override bool IsHeadLevelElement
        { get { return true; } }

        internal override bool IsInlineElement
        { get { return false; } }

        internal override bool IsStructureModuleElement
        { get { return false; } }

        internal override OptionalClosingTagType OptionalClosingTagMode
        { get { return OptionalClosingTagType.None; } }

        internal override string[] OptionalClosingTagSpecifiedTags
        { get { return null; } }

        private static List<string> s_ResetTags = new List<string>(new string[] { 

        });

        private static List<string> s_IgnoreTags = new List<string>(new string[] {

        });

        internal override List<string> ResetTags
        { get { return s_ResetTags; } }

        internal override List<string> IgnoreTags
        { get { return s_IgnoreTags; } }

        #endregion

        internal string m_StyleContent = null;

        public string CSS
        {
            get { return m_StyleContent != null ? m_StyleContent : string.Empty; }
        }
    }
}
