using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!ELEMENT (TH|TD)  - O (%flow;)*       -- table header cell, table data cell--&gt;
    /// 
    /// &lt;!-- Scope is simpler than headers attribute for common tables --&gt;
    /// &lt;!ENTITY % Scope "(row|col|rowgroup|colgroup)"&gt;
    /// 
    /// &lt;!-- TH is for headers, TD for data, but for cells acting as both use TD --&gt;
    /// &lt;!ATTLIST (TH|TD)                      -- header or data cell --
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   abbr        %Text;         #IMPLIED  -- abbreviation for header cell --
    ///   axis        CDATA          #IMPLIED  -- comma-separated list of related headers--
    ///   headers     IDREFS         #IMPLIED  -- list of id's for header cells --
    ///   scope       %Scope;        #IMPLIED  -- scope covered by header cells --
    ///   rowspan     NUMBER         1         -- number of rows spanned by cell --
    ///   colspan     NUMBER         1         -- number of cols spanned by cell --
    ///   %cellhalign;                         -- horizontal alignment in cells --
    ///   %cellvalign;                         -- vertical alignment in cells --
    ///   &gt;
    /// 
    /// Attribute definitions
    /// 
    /// headers = idrefs [CS] 
    ///     This attribute specifies the list of header cells that provide header information for the current data cell. The value of this attribute is a space-separated list of cell names; those cells must be named by setting their id attribute. Authors generally use the headers attribute to help non-visual user agents render header information about data cells (e.g., header information is spoken prior to the cell data), but the attribute may also be used in conjunction with style sheets. See also the scope attribute. 
    /// scope = scope-name [CI] 
    ///     This attribute specifies the set of data cells for which the current header cell provides header information. This attribute may be used in place of the headers attribute, particularly for simple tables. When specified, this attribute must have one of the following values: 
    ///         row: The current cell provides header information for the rest of the row that contains it (see also the section on table directionality). 
    ///         col: The current cell provides header information for the rest of the column that contains it. 
    ///         rowgroup: The header cell provides header information for the rest of the row group that contains it. 
    ///         colgroup: The header cell provides header information for the rest of the column group that contains it. 
    /// abbr = text [CS] 
    ///     This attribute should be used to provide an abbreviated form of the cell's content, and may be rendered by user agents when appropriate in place of the cell's content. Abbreviated names should be short since user agents may render them repeatedly. For instance, speech synthesizers may render the abbreviated headers relating to a particular cell before rendering that cell's content. 
    /// axis = cdata [CI] 
    ///     This attribute may be used to place a cell into conceptual categories that can be considered to form axes in an n-dimensional space. User agents may give users access to these categories (e.g., the user may query the user agent for all cells that belong to certain categories, the user agent may present a table in the form of a table of contents, etc.). Please consult the section on categorizing cells for more information. The value of this attribute is a comma-separated list of category names. 
    /// rowspan = number [CN] 
    ///     This attribute specifies the number of rows spanned by the current cell. The default value of this attribute is one ("1"). The value zero ("0") means that the cell spans all rows from the current row to the last row of the table section (THEAD, TBODY, or TFOOT) in which the cell is defined. 
    /// colspan = number [CN] 
    ///     This attribute specifies the number of columns spanned by the current cell. The default value of this attribute is one ("1"). The value zero ("0") means that the cell spans all columns from the current column to the last column of the column group (COLGROUP) in which the cell is defined. 
    /// nowrap [CI] 
    ///     Deprecated. When present, this boolean attribute tells visual user agents to disable automatic text wrapping for this cell. Style sheets should be used instead of this attribute to achieve wrapping effects. Note. if used carelessly, this attribute may result in excessively wide cells. 
    /// width = length [CN] 
    ///     Deprecated. This attribute supplies user agents with a recommended cell width. 
    /// height = length [CN] 
    ///     Deprecated. This attribute supplies user agents with a recommended cell height. 
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
    [HtmlTagAttributesMapper(TagName = "TD")]
    public sealed class TableCell : HtmlElement
    {
        internal TableCell(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
        { get { return new string[] { "TR", "THEAD", "TFOOT", "TBODY", "TABLE" }; } }


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
