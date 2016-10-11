using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!ELEMENT OBJECT - - (PARAM | %flow;)*
    ///  -- generic embedded object --&gt;
    /// &lt;!ATTLIST OBJECT
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   declare     (declare)      #IMPLIED  -- declare but don't instantiate flag --
    ///   classid     %URI;          #IMPLIED  -- identifies an implementation --
    ///   codebase    %URI;          #IMPLIED  -- base URI for classid, data, archive--
    ///   data        %URI;          #IMPLIED  -- reference to object's data --
    ///   type        %ContentType;  #IMPLIED  -- content type for data --
    ///   codetype    %ContentType;  #IMPLIED  -- content type for code --
    ///   archive     CDATA          #IMPLIED  -- space-separated list of URIs --
    ///   standby     %Text;         #IMPLIED  -- message to show while loading --
    ///   height      %Length;       #IMPLIED  -- override height --
    ///   width       %Length;       #IMPLIED  -- override width --
    ///   usemap      %URI;          #IMPLIED  -- use client-side image map --
    ///   name        CDATA          #IMPLIED  -- submit as part of form --
    ///   tabindex    NUMBER         #IMPLIED  -- position in tabbing order --
    ///   &gt;
    /// 
    /// 
    /// Attribute definitions
    /// 
    /// classid = uri [CT] 
    ///     This attribute may be used to specify the location of an object's implementation via a URI. It may be used together with, or as an alternative to the data attribute, depending on the type of object involved. 
    /// codebase = uri [CT] 
    ///     This attribute specifies the base path used to resolve relative URIs specified by the classid, data, and archive attributes. When absent, its default value is the base URI of the current document. 
    /// codetype = content-type [CI] 
    ///     This attribute specifies the content type of data expected when downloading the object specified by classid. This attribute is optional but recommended when classid is specified since it allows the user agent to avoid loading information for unsupported content types. When absent, it defaults to the value of the type attribute. 
    /// data = uri [CT] 
    ///     This attribute may be used to specify the location of the object's data, for instance image data for objects defining images, or more generally, a serialized form of an object which can be used to recreate it. If given as a relative URI, it should be interpreted relative to the codebase attribute. 
    /// type = content-type [CI] 
    ///     This attribute specifies the content type for the data specified by data. This attribute is optional but recommended when data is specified since it allows the user agent to avoid loading information for unsupported content types. If the value of this attribute differs from the HTTP Content-Type returned by the server when the object is retrieved, the HTTP Content-Type takes precedence. 
    /// archive = uri-list [CT] 
    ///     This attribute may be used to specify a space-separated list of URIs for archives containing resources relevant to the object, which may include the resources specified by the classid and data attributes. Preloading archives will generally result in reduced load times for objects. Archives specified as relative URIs should be interpreted relative to the codebase attribute. 
    /// declare [CI] 
    ///     When present, this boolean attribute makes the current OBJECT definition a declaration only. The object must be instantiated by a subsequent OBJECT definition referring to this declaration. 
    /// standby = text [CS] 
    ///     This attribute specifies a message that a user agent may render while loading the object's implementation and data. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information) 
    /// onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events) 
    /// tabindex (tabbing navigation) 
    /// usemap (client side image maps) 
    /// name (form submission) 
    /// align, width, height, border, hspace, vspace (visual presentation of objects, images, and applets) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "OBJECT")]
    public sealed class Object : HtmlElement
    {
        internal Object(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
            "AREA", "CAPTION", "COL", "COLGROUP", "LEGEND", "OPTGROUP", "OPTION", "TBODY", "TD", "TFOOT", "TH", "THEAD", "TR"
        });

        internal override List<string> ResetTags
        { get { return s_ResetTags; } }

        internal override List<string> IgnoreTags
        { get { return s_IgnoreTags; } }

        #endregion
    }
}
