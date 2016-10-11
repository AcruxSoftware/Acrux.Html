using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!ELEMENT COLGROUP - O (COL)*          -- table column group --&gt;
    /// &lt;!ATTLIST COLGROUP
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   span        NUMBER         1         -- default number of columns in group --
    ///   width       %MultiLength;  #IMPLIED  -- default width for enclosed COLs --
    ///   %cellhalign;                         -- horizontal alignment in cells --
    ///   %cellvalign;                         -- vertical alignment in cells --
    ///   &gt;
    /// 
    /// Attribute definitions
    /// 
    /// span = number [CN] 
    ///     This attribute, which must be an integer > 0, specifies the number of columns in a column group. Values mean the following: 
    ///         * In the absence of a span attribute, each COLGROUP defines a column group containing one column. 
    ///         * If the span attribute is set to N > 0, the current COLGROUP element defines a column group containing N columns. 
    ///     User agents must ignore this attribute if the COLGROUP element contains one or more COL elements.
    /// 
    /// width = multi-length [CN] 
    ///     This attribute specifies a default width for each column in the current column group. In addition to the standard pixel, percentage, and relative values, this attribute allows the special form "0*" (zero asterisk) which means that the width of the each column in the group should be the minimum width necessary to hold the column's contents. This implies that a column's entire contents must be known before its width may be correctly computed. Authors should be aware that specifying "0*" will prevent visual user agents from rendering a table incrementally.
    /// 
    ///     This attribute is overridden for any column in the column group whose width is specified via a COL element.
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information ) 
    /// onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events ) 
    /// align, char, charoff, valign (cell alignment) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "COLGROUP")]
    public sealed class ColumnGroup : HtmlElement
    {
        internal ColumnGroup(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
            : base(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition)
        { }

        #region Structure Definition

        internal override ElementClosingTagRequirement ClosingTagRequirement
        { get { return ElementClosingTagRequirement.Optional; } }

        internal override bool AllowNestedElements
        { get { return false; } }

        internal override bool IsHeadLevelElement
        { get { return false; } }

        internal override bool IsInlineElement
        { get { return false; } }

        internal override bool IsStructureModuleElement
        { get { return false; } }

        internal override OptionalClosingTagType OptionalClosingTagMode
        { get { return OptionalClosingTagType.MustOnlyContainSpecifiedTags; } }

        internal override string[] OptionalClosingTagSpecifiedTags
        { get { return new string[] { "COL" }; } }

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
