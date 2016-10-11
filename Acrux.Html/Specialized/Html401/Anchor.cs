using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{
    /// <summary>
    /// &lt;!ELEMENT A - - (%inline;)* -(A)       -- <b>anchor</b> --&gt; <br/>
    /// &lt;!ATTLIST A<br/>
    ///   %attrs;                              -- %coreattrs, %i18n, %events --<br/>
    ///   charset     %Charset;      #IMPLIED  -- char encoding of linked resource --<br/>
    ///   type        %ContentType;  #IMPLIED  -- advisory content type --<br/>
    ///   name        CDATA          #IMPLIED  -- named link end --<br/>
    ///   href        %URI;          #IMPLIED  -- URI for linked resource --<br/>
    ///   hreflang    %LanguageCode; #IMPLIED  -- language code --<br/>
    ///   rel         %LinkTypes;    #IMPLIED  -- forward link types --<br/>
    ///   rev         %LinkTypes;    #IMPLIED  -- reverse link types --<br/>
    ///   accesskey   %Character;    #IMPLIED  -- accessibility key character --<br/>
    ///   shape       %Shape;        rect      -- for use with client-side image maps --<br/>
    ///   coords      %Coords;       #IMPLIED  -- for use with client-side image maps --<br/>
    ///   tabindex    NUMBER         #IMPLIED  -- position in tabbing order --<br/>
    ///   onfocus     %Script;       #IMPLIED  -- the element got the focus --<br/>
    ///   onblur      %Script;       #IMPLIED  -- the element lost the focus --<br/>
    /// &gt;<br/>
    /// <br/>
    /// Attributes defined elsewhere<br/>
    /// <br/>
    /// id, class (document-wide identifiers) <br/>
    /// lang (language information), dir (text direction) <br/>
    /// title (element title) <br/>
    /// style (inline style information ) <br/>
    /// shape and coords (image maps) <br/>
    /// onfocus, onblur, onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events ) <br/>
    /// target (target frame information) <br/>
    /// tabindex (tabbing navigation) <br/>
    /// accesskey (access keys) <br/>
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "A")]
    public sealed class Anchor : HtmlElement
    {
        internal Anchor(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
            : base(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition)
        {

        }

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
            "AREA", "CAPTION", "COL", "COLGROUP", "LEGEND", "OPTGROUP", "OPTION", "TBODY", "TD", "TFOOT", "TH", "THEAD", "TR"
        });

        internal override List<string> ResetTags
        { get { return s_ResetTags; } }

        internal override List<string> IgnoreTags
        { get { return s_IgnoreTags; } }


        #endregion


        /// <summary>
        /// char encoding of linked resource
        /// </summary>
        public string Charset
        {
            get
            {
                return GetAttributeValue("charset");
            }
        }

        /// <summary>
        /// advisory content type
        /// </summary>
        public string Type
        {
            get
            {
                return GetAttributeValue("type");
            }
        }

        /// <summary>
        /// named link end
        /// </summary>
        public string NameAttr
        {
            get
            {
                return GetAttributeValue("name");
            }
        }

        /// <summary>
        /// URI for linked resource
        /// </summary>
        public string Href
        {
            get
            {
                return GetAttributeValue("href");
            }
        }

        /// <summary>
        /// language code
        /// </summary>
        public string HrefLang
        {
            get
            {
                return GetAttributeValue("hreflang");
            }
        }

        /// <summary>
        /// forward link types
        /// </summary>
        public string Rel
        {
            get
            {
                return GetAttributeValue("rel");
            }
        }


        /// <summary>
        /// reverse link types
        /// </summary>
        public string Rev
        {
            get
            {
                return GetAttributeValue("rev");
            }
        }


        /// <summary>
        /// accessibility key character
        /// </summary>
        public string AccessKey
        {
            get
            {
                return GetAttributeValue("accesskey");
            }
        }


        /// <summary>
        /// for use with client-side image maps
        /// </summary>
        public string Shape
        {
            get
            {
                return GetAttributeValue("shape");
            }
        }


        /// <summary>
        /// for use with client-side image maps
        /// </summary>
        public string Coords
        {
            get
            {
                return GetAttributeValue("coords");
            }
        }


        /// <summary>
        /// position in tabbing order
        /// </summary>
        public string TabIndex
        {
            get
            {
                return GetAttributeValue("tabindex");
            }
        }


        /// <summary>
        /// the element got the focus
        /// </summary>
        public string OnFocus
        {
            get
            {
                return GetAttributeValue("onfocus");
            }
        }


        /// <summary>
        /// the element lost the focus
        /// </summary>
        public string OnBlur
        {
            get
            {
                return GetAttributeValue("onblur");
            }
        }
    }
}
