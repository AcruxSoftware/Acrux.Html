using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!ENTITY % InputType
    ///   "(TEXT | PASSWORD | CHECKBOX |
    ///     RADIO | SUBMIT | RESET |
    ///     FILE | HIDDEN | IMAGE | BUTTON)"
    ///    &gt;
    /// 
    /// &lt;!-- attribute name required for all but submit and reset --&gt;
    /// &lt;!ELEMENT INPUT - O EMPTY              -- form control --&gt;
    /// &lt;!ATTLIST INPUT
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   type        %InputType;    TEXT      -- what kind of widget is needed --
    ///   name        CDATA          #IMPLIED  -- submit as part of form --
    ///   value       CDATA          #IMPLIED  -- Specify for radio buttons and checkboxes --
    ///   checked     (checked)      #IMPLIED  -- for radio buttons and check boxes --
    ///   disabled    (disabled)     #IMPLIED  -- unavailable in this context --
    ///   readonly    (readonly)     #IMPLIED  -- for text and passwd --
    ///   size        CDATA          #IMPLIED  -- specific to each type of field --
    ///   maxlength   NUMBER         #IMPLIED  -- max chars for text fields --
    ///   src         %URI;          #IMPLIED  -- for fields with images --
    ///   alt         CDATA          #IMPLIED  -- short description --
    ///   usemap      %URI;          #IMPLIED  -- use client-side image map --
    ///   ismap       (ismap)        #IMPLIED  -- use server-side image map --
    ///   tabindex    NUMBER         #IMPLIED  -- position in tabbing order --
    ///   accesskey   %Character;    #IMPLIED  -- accessibility key character --
    ///   onfocus     %Script;       #IMPLIED  -- the element got the focus --
    ///   onblur      %Script;       #IMPLIED  -- the element lost the focus --
    ///   onselect    %Script;       #IMPLIED  -- some text was selected --
    ///   onchange    %Script;       #IMPLIED  -- the element value was changed --
    ///   accept      %ContentTypes; #IMPLIED  -- list of MIME types for file upload --
    ///   &gt;
    /// 
    /// 
    /// Attribute definitions
    /// 
    /// type = text|password|checkbox|radio|submit|reset|file|hidden|image|button [CI] 
    ///     This attribute specifies the type of control to create. The default value for this attribute is "text". 
    /// name = cdata [CI] 
    ///     This attribute assigns the control name. 
    /// value = cdata [CA] 
    ///     This attribute specifies the initial value of the control. It is optional except when the type attribute has the value "radio" or "checkbox". 
    /// size = cdata [CN] 
    ///     This attribute tells the user agent the initial width of the control. The width is given in pixels except when type attribute has the value "text" or "password". In that case, its value refers to the (integer) number of characters. 
    /// maxlength = number [CN] 
    ///     When the type attribute has the value "text" or "password", this attribute specifies the maximum number of characters the user may enter. This number may exceed the specified size, in which case the user agent should offer a scrolling mechanism. The default value for this attribute is an unlimited number. 
    /// checked [CI] 
    ///     When the type attribute has the value "radio" or "checkbox", this boolean attribute specifies that the button is on. User agents must ignore this attribute for other control types. 
    /// src = uri [CT] 
    ///     When the type attribute has the value "image", this attribute specifies the location of the image to be used to decorate the graphical submit button. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information) 
    /// alt (alternate text) 
    /// align (alignment) 
    /// accept (legal content types for a server) 
    /// readonly (read-only input controls) 
    /// disabled (disabled input controls) 
    /// tabindex (tabbing navigation) 
    /// accesskey (access keys) 
    /// usemap (client-side image maps) 
    /// ismap (server-side image maps) 
    /// onfocus, onblur, onselect, onchange, onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "INPUT")]
    public sealed class Input : HtmlElement
    {
        internal Input(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
            : base(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition)
        { }

        #region Structure Definition

        internal override ElementClosingTagRequirement ClosingTagRequirement
        { get { return ElementClosingTagRequirement.Forbidden; } }

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
            "AREA", "CAPTION", "COL", "COLGROUP", "OPTGROUP", "OPTION", "TBODY", "TD", "TFOOT", "TH", "THEAD", "TR"
        });

        internal override List<string> ResetTags
        { get { return s_ResetTags; } }

        internal override List<string> IgnoreTags
        { get { return s_IgnoreTags; } }

        #endregion
    }
}
