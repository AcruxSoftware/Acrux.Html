using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{
    /// <summary>
    /// &lt;!ELEMENT P - O (%inline;)*            -- paragraph --&gt;
    /// &lt;!ATTLIST P
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   &gt;
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information ) 
    /// align (alignment) 
    /// onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events ) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "P")]
    public sealed class Paragraph : HtmlElement
    {

        internal Paragraph(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
            : base(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition)
        { }


        // TODO: For the unit tests. Every starting Block-Level Element should be considered as outside the <P> and therefore the <P> has to be closed (optional end tag)
        // TODO: Carefully review ALL bock-level elements, test them in browser (i.e. test that they will be rendered outside the <P> and then create a unit test 
        // TODO: Carefully review ALL inline elements, test them in browser and create unit tests

        #region Structure Definition

        internal override ElementClosingTagRequirement ClosingTagRequirement
        { get { return ElementClosingTagRequirement.Optional; } }

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

        internal override EndTagWithoutStartTagHandling EndTagWithoutStartTagMode
        { get { return EndTagWithoutStartTagHandling.HandleAsStartTag; } }

        internal override string[] OptionalClosingTagSpecifiedTags
        { get { return null; } }

        private static List<string> s_ResetTags = new List<string>(new string[] { 
        /*5 */ "ADDRESS", "BLOCKQUOTE", "CENTER", "DD", "DIR", "DIV", "DL", "DT", "FIELDSET", "FORM", 
               "H1", "H2", "H3", "H4", "H5", "H6", "LI", "MENU", "NOFRAMES", "OL", "PRE", "UL",
        /*5a*/ "HR", "ISINDEX", "PARAM"
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
