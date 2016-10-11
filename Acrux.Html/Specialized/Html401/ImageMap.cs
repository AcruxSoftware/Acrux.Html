using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!ELEMENT MAP - - ((%block;) | AREA)+ -- client-side image map --&gt;
    /// &lt;!ATTLIST MAP
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   name        CDATA          #REQUIRED -- for reference by usemap --
    ///   &gt;
    /// 
    /// MAP attribute definitions
    /// 
    /// name = cdata [CI] 
    ///     This attribute assigns a name to the image map defined by a MAP element. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information) 
    /// name (submitting objects with forms) 
    /// alt (alternate text) 
    /// href (anchor reference) target (frame target information) 
    /// tabindex (tabbing navigation) 
    /// accesskey (access keys) 
    /// shape (image maps) 
    /// onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup, onfocus, onblur (intrinsic events) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "MAP")]
    public sealed class ImageMap : HtmlElement
    {
        internal ImageMap(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
            "CAPTION", "COL", "COLGROUP", "LEGEND", "OPTGROUP", "OPTION", "TBODY", "TD", "TFOOT", "TH", "THEAD", "TR"
        });

        internal override List<string> ResetTags
        { get { return s_ResetTags; } }

        internal override List<string> IgnoreTags
        { get { return s_IgnoreTags; } }

        #endregion
    }
}
