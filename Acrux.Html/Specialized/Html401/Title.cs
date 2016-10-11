using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{
    /// <summary>
    /// &lt;!-- The TITLE element is not considered part of the flow of text.
    ///        It should be displayed, for example as the page header or
    ///        window title. Exactly one title is required per document.
    ///     --&gt;
    /// &lt;!ELEMENT TITLE - - (#PCDATA) -(%head.misc;) -- document title --&gt;
    /// &lt;!ATTLIST TITLE %i18n&gt;
    /// 
    /// Attributes defined elsewhere
    /// 
    /// lang (language information), dir (text direction) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "TITLE")]
    public sealed class Title : HtmlElement
    {
        internal Title(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
            : base(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition)
        { }

        #region Structure Definition

        internal override ElementClosingTagRequirement ClosingTagRequirement
        { get { return ElementClosingTagRequirement.Required; } }

        internal override bool AllowNestedElements
        { get { return false; } }

        internal override bool IsHeadLevelElement
        { get { return true; } }

        internal override bool IsInlineElement
        { get { return true; } }

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
    }
}
