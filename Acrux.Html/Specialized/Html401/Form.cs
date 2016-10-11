using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{
    /// <summary>
    /// &lt;!ELEMENT FORM - - (%block;|SCRIPT)+ -(FORM) -- interactive form --&gt;
    /// &lt;!ATTLIST FORM
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   action      %URI;          #REQUIRED -- server-side form handler --
    ///   method      (GET|POST)     GET       -- HTTP method used to submit the form--
    ///   enctype     %ContentType;  "application/x-www-form-urlencoded"
    ///   accept      %ContentTypes; #IMPLIED  -- list of MIME types for file upload --
    ///   name        CDATA          #IMPLIED  -- name of form for scripting --
    ///   onsubmit    %Script;       #IMPLIED  -- the form was submitted --
    ///   onreset     %Script;       #IMPLIED  -- the form was reset --
    ///   accept-charset %Charsets;  #IMPLIED  -- list of supported charsets --
    ///   &gt;
    /// 
    /// Attribute definitions
    /// 
    /// action = uri [CT] 
    ///     This attribute specifies a form processing agent. User agent behavior for a value other than an HTTP URI is undefined. 
    /// method = get|post [CI] 
    ///     This attribute specifies which HTTP method will be used to submit the form data set. Possible (case-insensitive) values are "get" (the default) and "post". See the section on form submission for usage information. 
    /// enctype = content-type [CI] 
    ///     This attribute specifies the content type used to submit the form to the server (when the value of method is "post"). The default value for this attribute is "application/x-www-form-urlencoded". The value "multipart/form-data" should be used in combination with the INPUT element, type="file". 
    /// accept-charset = charset list [CI] 
    ///     This attribute specifies the list of character encodings for input data that is accepted by the server processing this form. The value is a space- and/or comma-delimited list of charset values. The client must interpret this list as an exclusive-or list, i.e., the server is able to accept any single character encoding per entity received. 
    /// 
    ///     The default value for this attribute is the reserved string "UNKNOWN". User agents may interpret this value as the character encoding that was used to transmit the document containing this FORM element.
    /// 
    /// accept = content-type-list [CI] 
    ///     This attribute specifies a comma-separated list of content types that a server processing this form will handle correctly. User agents may use this information to filter out non-conforming files when prompting a user to select files to be sent to the server (cf. the INPUT element when type="file"). 
    /// name = cdata [CI] 
    ///     This attribute names the element so that it may be referred to from style sheets or scripts. Note. This attribute has been included for backwards compatibility. Applications should use the id attribute to identify elements. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// style (inline style information) 
    /// title (element title) 
    /// target (target frame information) 
    /// onsubmit, onreset, onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "FORM")]
    public sealed class Form : HtmlElement
    {
        internal Form(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
            : base(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition)
        { }

        #region Structure Definition

        internal override ElementClosingTagRequirement ClosingTagRequirement
        { get { return ElementClosingTagRequirement.Required; } }

        internal override bool AllowNestedElements
        { get { return true; } }

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
