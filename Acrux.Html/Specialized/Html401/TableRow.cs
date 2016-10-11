using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!ELEMENT TR       - O (TH|TD)+        -- table row --&gt;
    /// &lt;!ATTLIST TR                           -- table row --
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   %cellhalign;                         -- horizontal alignment in cells --
    ///   %cellvalign;                         -- vertical alignment in cells --
    ///   &gt;
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information ) 
    /// onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events ) 
    /// bgcolor (background color) 
    /// align, char, charoff, valign (cell alignment) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "TR")]
    public sealed class TableRow : HtmlElement
    {
        internal TableRow(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
        { get { return OptionalClosingTagType.MustBeOnlyContainedInsideSpeficiedTags; } }

        internal override string[] OptionalClosingTagSpecifiedTags
        { get { return new string[] { "THEAD", "TFOOT", "TBODY", "TABLE" }; } }

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
