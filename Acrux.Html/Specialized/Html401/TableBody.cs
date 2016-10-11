using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!ELEMENT TBODY    O O (TR)+           -- table body --&gt;
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
    [HtmlTagAttributesMapper(TagName = "TBODY")]
    public sealed class TableBody : HtmlElement
    {
        internal TableBody(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
        { get { return new string[] { "TR" }; } }

        #endregion

        private static List<string> s_ResetTags = new List<string>(new string[] { 

        });

        private static List<string> s_IgnoreTags = new List<string>(new string[] {

        });

        internal override List<string> ResetTags
        { get { return s_ResetTags; } }

        internal override List<string> IgnoreTags
        { get { return s_IgnoreTags; } }

        // TODO: A table can have more than one blocks of data. Build the appropriate unit tests
        //
        //<TABLE>
        //<THEAD>
        //     <TR> ...header information...
        //</THEAD>
        //<TFOOT>
        //     <TR> ...footer information...
        //</TFOOT>
        //<TBODY>
        //     <TR> ...first row of block one data...
        //     <TR> ...second row of block one data...
        //</TBODY>
        //<TBODY>
        //     <TR> ...first row of block two data...
        //     <TR> ...second row of block two data...
        //     <TR> ...third row of block two data...
        //</TBODY>
        //</TABLE>
    }
}
