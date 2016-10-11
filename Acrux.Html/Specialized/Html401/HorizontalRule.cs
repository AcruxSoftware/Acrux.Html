using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{
    /// <summary>
    /// &lt;!ELEMENT HR - O EMPTY -- horizontal rule --&gt;
    /// &lt;!ATTLIST HR
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   &gt;
    /// 
    /// Attribute definitions
    /// 
    /// align = left|center|right [CI] 
    ///     Deprecated. This attribute specifies the horizontal alignment of the rule with respect to the surrounding context. Possible values: 
    ///         left: the rule is rendered flush left. 
    ///         center: the rule is centered. 
    ///         right: the rule is rendered flush right. 
    ///     The default is align=center.
    /// 
    /// noshade [CI] 
    ///     Deprecated. When set, this boolean attribute requests that the user agent render the rule in a solid color rather than as the traditional two-color "groove". 
    /// size = pixels [CI] 
    ///     Deprecated. This attribute specifies the height of the rule. The default value for this attribute depends on the user agent. 
    /// width = length [CI] 
    ///     Deprecated. This attribute specifies the width of the rule. The default width is 100%, i.e., the rule extends across the entire canvas. 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information) 
    /// onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "HR")]
    public sealed class HorizontalRule : HtmlElement
    {
        internal HorizontalRule(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
            : base(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition)
        { }

        #region Structure Definition

        internal override ElementClosingTagRequirement ClosingTagRequirement
        { get { return ElementClosingTagRequirement.Forbidden; } }

        internal override bool AllowNestedElements
        { get { return false; } }

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

        });

        private static List<string> s_IgnoreTags = new List<string>(new string[] {
            "AREA", "CAPTION", "COL", "COLGROUP", "OPTGROUP", "OPTION", "TBODY", "TD", "TFOOT", "TH", "THEAD", "TR"
        });

        internal override List<string> ResetTags
        { get { return s_ResetTags; } }

        internal override List<string> IgnoreTags
        { get { return s_IgnoreTags; } }

        #endregion
    }
}
