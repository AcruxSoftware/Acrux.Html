using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{
    /// <summary>
    /// &lt;!ELEMENT LINK - O EMPTY               -- a media-independent link --&gt;
    /// &lt;!ATTLIST LINK
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   charset     %Charset;      #IMPLIED  -- char encoding of linked resource --
    ///   href        %URI;          #IMPLIED  -- URI for linked resource --
    ///   hreflang    %LanguageCode; #IMPLIED  -- language code --
    ///   type        %ContentType;  #IMPLIED  -- advisory content type --
    ///   rel         %LinkTypes;    #IMPLIED  -- forward link types --
    ///   rev         %LinkTypes;    #IMPLIED  -- reverse link types --
    ///   media       %MediaDesc;    #IMPLIED  -- for rendering on these media --
    ///   &gt;
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information ) 
    /// onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events ) 
    /// href, hreflang, type, rel, rev (links and anchors) 
    /// target (target frame information) 
    /// media (header style information) 
    /// charset(character encodings) 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "LINK")]
    public sealed class Link : HtmlElement
    {

        internal Link(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
            : base(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition)
        { }

        #region Structure Definition

        internal override ElementClosingTagRequirement ClosingTagRequirement
        { get { return ElementClosingTagRequirement.Forbidden; } }

        internal override bool AllowNestedElements
        { get { return false; } }

        internal override bool IsHeadLevelElement
        { get { return true; } }

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
