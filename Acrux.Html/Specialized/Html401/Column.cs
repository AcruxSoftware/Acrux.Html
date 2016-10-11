using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!ELEMENT COL      - O EMPTY           -- table column --&gt;
    /// &lt;!ATTLIST COL                          -- column groups and properties --
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   span        NUMBER         1         -- COL attributes affect N columns --
    ///   width       %MultiLength;  #IMPLIED  -- column width specification --
    ///   %cellhalign;                         -- horizontal alignment in cells --
    ///   %cellvalign;                         -- vertical alignment in cells --
    ///   &gt;
    /// 
    /// Attribute definitions
    /// 
    /// span = number [CN] 
    ///     This attribute, whose value must be an integer > 0, specifies the number of columns "spanned" by the COL element; the COL element shares its attributes with all the columns it spans. The default value for this attribute is 1 (i.e., the COL element refers to a single column). If the span attribute is set to N > 1, the current COL element shares its attributes with the next N-1 columns. 
    /// width = multi-length [CN] 
    ///     This attribute specifies a default width for each column spanned by the current COL element. It has the same meaning as the width attribute for the COLGROUP element and overrides it. 
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
    [HtmlTagAttributesMapper(TagName = "COL")]
    public sealed class Column : HtmlElement 
    {
        internal Column(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
