using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{
    /// <summary>
    /// &lt;!ELEMENT UL - - (LI)+                 -- unordered list --&gt;
    /// &lt;!ATTLIST UL
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   &gt;
    /// &lt;!ELEMENT OL - - (LI)+                 -- ordered list --&gt;
    /// &lt;!ATTLIST OL
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   &gt;
    /// 
    /// Attribute definitions
    /// 
    /// type  =  style-information [CI] 
    ///     Deprecated. This attribute sets the style of a list item. Currently available values are intended for visual user agents. Possible values are described below (along with case information). 
    /// start = number [CN] 
    ///     Deprecated. For OL only. This attribute specifies the starting number of the first item in an ordered list. The default starting number is "1". Note that while the value of this attribute is an integer, the corresponding label may be non-numeric. Thus, when the list item style is uppercase latin letters (A, B, C, ...), start=3 means "C". When the style is lowercase roman numerals, start=3 means "iii", etc. 
    /// value = number [CN] 
    ///     Deprecated. For LI only. This attribute sets the number of the current list item. Note that while the value of this attribute is an integer, the corresponding label may be non-numeric (see the start attribute). 
    /// compact [CI] 
    ///     Deprecated. When set, this boolean attribute gives a hint to visual user agents to render the list in a more compact way. The interpretation of this attribute depends on the user agent. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information) 
    /// onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "UL")]
    public sealed class UnorderedList : HtmlElement
    {
        internal UnorderedList(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
            : base(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition)
        { }

        #region Structure Definition

        internal override ElementClosingTagRequirement ClosingTagRequirement
        { get { return ElementClosingTagRequirement.Required; } }

        internal override bool AllowNestedElements
        { get { return true; } }

        internal override bool IsHeadLevelElement
        { get { return false; } }

        internal override bool IsInlineElement
        { get { return false; } }

        internal override bool IsStructureModuleElement
        { get { return false; } }

        internal override OptionalClosingTagType OptionalClosingTagMode
        { get { return OptionalClosingTagType.None; } }

        internal override string[] OptionalClosingTagSpecifiedTags
        { get { return null; } }

        private static List<string> s_ResetTags = new List<string>(new string[] { 
        /*3a*/ "PARAM"
        });

        private static List<string> s_IgnoreTags = new List<string>(new string[] { 
            "AREA", "CAPTION", "COL", "COLGROUP", "LEGEND", "OPTGROUP", "OPTION", "TBODY", "TD", "TFOOT", "TH", "THEAD", "TR"
        });

        internal override List<string> ResetTags
        { get { return s_ResetTags; } }

        internal override List<string> IgnoreTags
        { get { return s_IgnoreTags; } }

        #endregion
    }
}
