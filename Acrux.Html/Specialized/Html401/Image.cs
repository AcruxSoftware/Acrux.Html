using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!-- To avoid problems with text-only UAs as well as 
    ///    to make image content understandable and navigable 
    ///    to users of non-visual UAs, you need to provide
    ///    a description with ALT, and avoid server-side image maps --&gt;
    /// &lt;!ELEMENT IMG - O EMPTY                -- Embedded image --&gt;
    /// &lt;!ATTLIST IMG
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   src         %URI;          #REQUIRED -- URI of image to embed --
    ///   alt         %Text;         #REQUIRED -- short description --
    ///   longdesc    %URI;          #IMPLIED  -- link to long description
    ///                                           (complements alt) --
    ///   name        CDATA          #IMPLIED  -- name of image for scripting --
    ///   height      %Length;       #IMPLIED  -- override height --
    ///   width       %Length;       #IMPLIED  -- override width --
    ///   usemap      %URI;          #IMPLIED  -- use client-side image map --
    ///   ismap       (ismap)        #IMPLIED  -- use server-side image map --
    ///   &gt;
    /// 
    ///
    /// Attribute definitions
    /// 
    /// src = uri [CT] 
    ///     This attribute specifies the location of the image resource. Examples of widely recognized image formats include GIF, JPEG, and PNG. 
    /// longdesc = uri [CT] 
    ///     This attribute specifies a link to a long description of the image. This description should supplement the short description provided using the alt attribute. When the image has an associated image map, this attribute should provide information about the image map's contents. This is particularly important for server-side image maps. Since an IMG element may be within the content of an A element, the user agent's mechanism in the user interface for accessing the "longdesc" resource of the former must be different than the mechanism for accessing the href resource of the latter. 
    /// name = cdata [CI] 
    ///     This attribute names the element so that it may be referred to from style sheets or scripts. Note. This attribute has been included for backwards compatibility. Applications should use the id attribute to identify elements. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// alt (alternate text) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information) 
    /// onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events) 
    /// ismap, usemap (client side image maps) 
    /// align, width, height, border, hspace, vspace (visual presentation of objects, images, and applets) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "IMG")]
    public sealed class Image : HtmlElement
    {
        internal Image(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
