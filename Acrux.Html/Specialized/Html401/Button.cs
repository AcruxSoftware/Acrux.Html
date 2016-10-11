using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!ELEMENT BUTTON - -
    ///      (%flow;)* -(A|%formctrl;|FORM|FIELDSET)
    ///      -- push button --&gt;
    /// &lt;!ATTLIST BUTTON
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   name        CDATA          #IMPLIED
    ///   value       CDATA          #IMPLIED  -- sent to server when submitted --
    ///   type        (button|submit|reset) submit -- for use as form button --
    ///   disabled    (disabled)     #IMPLIED  -- unavailable in this context --
    ///   tabindex    NUMBER         #IMPLIED  -- position in tabbing order --
    ///   accesskey   %Character;    #IMPLIED  -- accessibility key character --
    ///   onfocus     %Script;       #IMPLIED  -- the element got the focus --
    ///   onblur      %Script;       #IMPLIED  -- the element lost the focus --
    ///   &gt;
    ///
    /// Attribute definitions
    /// 
    /// name = cdata [CI] 
    ///     This attribute assigns the control name. 
    /// value = cdata [CS] 
    ///     This attribute assigns the initial value to the button. 
    /// type = submit|button|reset [CI] 
    ///     This attribute declares the type of the button. Possible values: 
    ///         submit: Creates a submit button. This is the default value. 
    ///         reset: Creates a reset button. 
    ///         button: Creates a push button. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information) 
    /// disabled (disabled input controls) 
    /// accesskey (access keys) 
    /// tabindex (tabbing navigation) 
    /// onfocus, onblur, onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events) 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "BUTTON")]
    public sealed class Button : HtmlElement
    {
        internal Button(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
            "AREA", "CAPTION", "COL", "COLGROUP", "INPUT", "LABEL", "LEGEND", "OPTGROUP", "OPTION", "SELECT", "TBODY", "TD", "TEXTAREA", "TFOOT", "TH", "THEAD", "TR"
        });

        internal override List<string> ResetTags
        { get { return s_ResetTags; } }

        internal override List<string> IgnoreTags
        { get { return s_IgnoreTags; } }

        #endregion
    }
}
