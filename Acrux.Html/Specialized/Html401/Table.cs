using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{
    /// <summary>
    /// &lt;!ELEMENT TABLE - -
    ///      (CAPTION?, (COL*|COLGROUP*), THEAD?, TFOOT?, TBODY+)&lt;
    /// &lt;!ATTLIST TABLE                        -- table element --
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   summary     %Text;         #IMPLIED  -- purpose/structure for speech output--
    ///   width       %Length;       #IMPLIED  -- table width --
    ///   border      %Pixels;       #IMPLIED  -- controls frame width around table --
    ///   frame       %TFrame;       #IMPLIED  -- which parts of frame to render --
    ///   rules       %TRules;       #IMPLIED  -- rulings between rows and cols --
    ///   cellspacing %Length;       #IMPLIED  -- spacing between cells --
    ///   cellpadding %Length;       #IMPLIED  -- spacing within cells --
    ///   &lt;
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information ) 
    /// onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events) 
    /// bgcolor (background color) 
    /// frame, rules, border (borders and rules) 
    /// cellspacing, cellpadding (cell margins) 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "TABLE")]
    public sealed class Table : HtmlElement
    {
        internal Table(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
