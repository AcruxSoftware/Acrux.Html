using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{
    /// <summary>
    /// &lt;!ELEMENT BODY O O (%block;|SCRIPT)+ +(INS|DEL) -- document body --&gt;
    /// &lt;!ATTLIST BODY
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   onload          %Script;   #IMPLIED  -- the document has been loaded --
    ///   onunload        %Script;   #IMPLIED  -- the document has been removed --
    ///   &gt;
    /// 
    /// Attribute definitions
    /// 
    /// background = uri [CT] 
    ///     Deprecated. The value of this attribute is a URI that designates an image resource. The image generally tiles the background (for visual browsers). 
    /// text = color [CI] 
    ///     Deprecated. This attribute sets the foreground color for text (for visual browsers). 
    /// link = color [CI] 
    ///     Deprecated. This attribute sets the color of text marking unvisited hypertext links (for visual browsers). 
    /// vlink = color [CI] 
    ///     Deprecated. This attribute sets the color of text marking visited hypertext links (for visual browsers). 
    /// alink = color [CI] 
    ///     Deprecated. This attribute sets the color of text marking hypertext links when selected by the user (for visual browsers). 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information) 
    /// bgcolor (background color) 
    /// onload, onunload (intrinsic events) 
    /// onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events) 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "BODY")]
    public sealed class Body : HtmlElement
    {
        internal Body(string prefix, string localName, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
            : base(prefix, localName, "", doc, isEmptyTag, parsedPosition)
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
        { get { return true; } }

        internal override OptionalClosingTagType OptionalClosingTagMode
        { get { return OptionalClosingTagType.MustBeOnlyContainedInsideSpeficiedTags; } }

        internal override string[] OptionalClosingTagSpecifiedTags
        { get { return new string[] { "HTML" }; } }

        #endregion
    }
}
