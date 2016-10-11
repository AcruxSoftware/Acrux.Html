using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!ELEMENT SELECT - - (OPTGROUP|OPTION)+ -- option selector --&gt;
    /// &lt;!ATTLIST SELECT
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   name        CDATA          #IMPLIED  -- field name --
    ///   size        NUMBER         #IMPLIED  -- rows visible --
    ///   multiple    (multiple)     #IMPLIED  -- default is single selection --
    ///   disabled    (disabled)     #IMPLIED  -- unavailable in this context --
    ///   tabindex    NUMBER         #IMPLIED  -- position in tabbing order --
    ///   onfocus     %Script;       #IMPLIED  -- the element got the focus --
    ///   onblur      %Script;       #IMPLIED  -- the element lost the focus --
    ///   onchange    %Script;       #IMPLIED  -- the element value was changed --
    ///   &gt;
    /// 
    /// 
    /// Attribute definitions
    /// 
    /// name = cdata [CI] 
    ///     This attribute assigns the control name. 
    /// size = number [CN] 
    ///     If a SELECT element is presented as a scrolled list box, this attribute specifies the number of rows in the list that should be visible at the same time. Visual user agents are not required to present a SELECT element as a list box; they may use any other mechanism, such as a drop-down menu. 
    /// multiple [CI] 
    ///     If set, this boolean attribute allows multiple selections. If not set, the SELECT element only permits single selections. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information) 
    /// disabled (disabled input controls) 
    /// tabindex (tabbing navigation) 
    /// onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "SELECT")]
    public sealed class OptionSelector : HtmlElement
    {
        internal OptionSelector(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
