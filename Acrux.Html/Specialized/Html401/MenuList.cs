using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{
    /// <summary>
    /// DEPRECATED
    /// The DIR element was designed to be used for creating multicolumn directory lists. The MENU element was designed to be used for single column menu lists. Both elements have the same structure as UL, just different rendering. In practice, a user agent will render a DIR or MENU list exactly as a UL list.
    ///
    /// &lt;!ELEMENT (DIR|MENU) - - (LI)+ -(%block;) -- directory list, menu list --&gt;
    /// &lt;!ATTLIST DIR
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   compact     (compact)      #IMPLIED -- reduced interitem spacing --
    ///   >
    /// &lt;!ATTLIST MENU
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   compact     (compact)      #IMPLIED -- reduced interitem spacing --
    ///   &gt;
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information) 
    /// onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events) 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "MENU")]
    public sealed class MenuList : HtmlElement
    {
        internal MenuList(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
            : base(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition)
        { }

        #region Structure Definition

        internal override ElementClosingTagRequirement ClosingTagRequirement
        { get { return ElementClosingTagRequirement.Required; } }

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
