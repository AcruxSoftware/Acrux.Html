using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!ELEMENT OPTION - O (#PCDATA)         -- selectable choice --&gt;
    /// &lt;!ATTLIST OPTION
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   selected    (selected)     #IMPLIED
    ///   disabled    (disabled)     #IMPLIED  -- unavailable in this context --
    ///   label       %Text;         #IMPLIED  -- for use in hierarchical menus --
    ///   value       CDATA          #IMPLIED  -- defaults to element content --
    ///   &gt;
    /// 
    /// OPTION Attribute definitions
    /// 
    /// selected [CI] 
    ///     When set, this boolean attribute specifies that this option is pre-selected. 
    /// value = cdata [CS] 
    ///     This attribute specifies the initial value of the control. If this attribute is not set, the initial value is set to the contents of the OPTION element. 
    /// label = text [CS] 
    ///     This attribute allows authors to specify a shorter label for an option than the content of the OPTION element. When specified, user agents should use the value of this attribute rather than the content of the OPTION element as the option label. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information) 
    /// disabled (disabled input controls) 
    /// onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events) 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "OPTION")]
    public sealed class SelectableChoice : HtmlElement
    {
        internal SelectableChoice(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
        { get { return new string[] { "OPTGROUP", "SELECT" }; } }

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
