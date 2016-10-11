using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!ELEMENT CAPTION  - - (%inline;)*     -- table caption --&gt;
    /// 
    /// &lt;!ATTLIST CAPTION
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   &gt;
    /// 
    /// Attribute definitions
    ///
    /// align = top|bottom|left|right [CI] 
    ///     Deprecated. For visual user agents, this attribute specifies the position of the caption with respect to the table. Possible values: 
    ///         top: The caption is at the top of the table. This is the default value. 
    ///         bottom: The caption is at the bottom of the table. 
    ///         left: The caption is at the left of the table. 
    ///         right: The caption is at the right of the table. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information ) 
    /// onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events ) 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "CAPTION")]
    public sealed class Caption : HtmlElement
    {
        internal Caption(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
            : base(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition)
        { }

        #region Structure Definition

        internal override bool IsDeprecated
        { get { return true; }}

        internal override ElementClosingTagRequirement ClosingTagRequirement
        { get { return ElementClosingTagRequirement.Required; } }

        internal override bool AllowNestedElements
        { get { return false; } }

        internal override bool IsHeadLevelElement
        { get { return false; } }

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
