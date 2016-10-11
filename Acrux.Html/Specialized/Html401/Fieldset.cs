using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!--
    ///   #PCDATA is to solve the mixed content problem,
    ///   per specification only whitespace is allowed there!
    ///  --&gt;
    /// &lt;!ELEMENT FIELDSET - - (#PCDATA,LEGEND,(%flow;)*) -- form control group --&gt;
    /// &lt;!ATTLIST FIELDSET
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   &gt;
    /// 
    /// &lt;!ELEMENT LEGEND - - (%inline;)*       -- fieldset legend --&gt;
    /// 
    /// &lt;!ATTLIST LEGEND
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   accesskey   %Character;    #IMPLIED  -- accessibility key character --
    ///   &gt;
    /// 
    /// LEGEND Attribute definitions
    /// 
    /// align = top|bottom|left|right [CI] 
    ///     Deprecated. This attribute specifies the position of the legend with respect to the fieldset. Possible values: 
    ///         top: The legend is at the top of the fieldset. This is the default value. 
    ///         bottom: The legend is at the bottom of the fieldset. 
    ///         left: The legend is at the left side of the fieldset. 
    ///         right: The legend is at the right side of the fieldset. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information) 
    /// accesskey (access keys) 
    /// onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events) 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "FIELDSET")]
    public sealed class Fieldset : HtmlElement
    {
        internal Fieldset(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
            "AREA", "CAPTION", "COL", "COLGROUP", "OPTGROUP", "OPTION", "TBODY", "TD", "TFOOT", "TH", "THEAD", "TR"
        });

        internal override List<string> ResetTags
        { get { return s_ResetTags; } }

        internal override List<string> IgnoreTags
        { get { return s_IgnoreTags; } }

        #endregion
    }
}
