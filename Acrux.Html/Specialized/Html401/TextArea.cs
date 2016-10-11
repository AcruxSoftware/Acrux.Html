using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!ELEMENT TEXTAREA - - (#PCDATA)       -- multi-line text field --&gt;
    /// &lt;!ATTLIST TEXTAREA
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   name        CDATA          #IMPLIED
    ///   rows        NUMBER         #REQUIRED
    ///   cols        NUMBER         #REQUIRED
    ///   disabled    (disabled)     #IMPLIED  -- unavailable in this context --
    ///   readonly    (readonly)     #IMPLIED
    ///   tabindex    NUMBER         #IMPLIED  -- position in tabbing order --
    ///   accesskey   %Character;    #IMPLIED  -- accessibility key character --
    ///   onfocus     %Script;       #IMPLIED  -- the element got the focus --
    ///   onblur      %Script;       #IMPLIED  -- the element lost the focus --
    ///   onselect    %Script;       #IMPLIED  -- some text was selected --
    ///   onchange    %Script;       #IMPLIED  -- the element value was changed --
    ///   &gt;
    /// 
    /// Attribute definitions
    /// 
    /// name = cdata [CI] 
    ///     This attribute assigns the control name. 
    /// rows = number [CN] 
    ///     This attribute specifies the number of visible text lines. Users should be able to enter more lines than this, so user agents should provide some means to scroll through the contents of the control when the contents extend beyond the visible area. 
    /// cols = number [CN] 
    ///     This attribute specifies the visible width in average character widths. Users should be able to enter longer lines than this, so user agents should provide some means to scroll through the contents of the control when the contents extend beyond the visible area. User agents may wrap visible text lines to keep long lines visible without the need for scrolling. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information) 
    /// readonly (read-only input controls) 
    /// disabled (disabled input controls) 
    /// tabindex (tabbing navigation) 
    /// onfocus, onblur, onselect, onchange, onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "TEXTAREA")]
    public sealed class TextArea : HtmlElement
    {
        internal TextArea(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
