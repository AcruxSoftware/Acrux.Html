using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!ELEMENT LABEL - - (%inline;)* -(LABEL) -- form field label text --&gt;
    /// &lt;!ATTLIST LABEL
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   for         IDREF          #IMPLIED  -- matches field ID value --
    ///   accesskey   %Character;    #IMPLIED  -- accessibility key character --
    ///   onfocus     %Script;       #IMPLIED  -- the element got the focus --
    ///   onblur      %Script;       #IMPLIED  -- the element lost the focus --
    ///  &gt;
    /// 
    /// Attribute definitions
    /// 
    /// for = idref [CS] 
    ///     This attribute explicitly associates the label being defined with another control. When present, the value of this attribute must be the same as the value of the id attribute of some other control in the same document. When absent, the label being defined is associated with the element's contents. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information) 
    /// accesskey (access keys) 
    /// onfocus, onblur, onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "LABEL")]
    public sealed class Label : HtmlElement
    {
        internal Label(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
        /*3 */ "ADDRESS", "BLOCKQUOTE", "CENTER", "DIR", "DIV", "DL", "FIELDSET", "FORM", "H1", "H2", "H3", "H4", "H5", "H6", 
               "LI", "MENU", "NOFRAMES", "OL", "P", "PRE", "UL",
        /*3a*/ "HR", "ISINDEX", "PARAM"        
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
