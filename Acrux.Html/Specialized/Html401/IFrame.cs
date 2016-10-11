using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!ELEMENT IFRAME - - (%flow;)*         -- inline subwindow --&gt;
    /// &lt;!ATTLIST IFRAME
    ///   %coreattrs;                          -- id, class, style, title --
    ///   longdesc    %URI;          #IMPLIED  -- link to long description
    ///                                           (complements title) --
    ///   name        CDATA          #IMPLIED  -- name of frame for targetting --
    ///   src         %URI;          #IMPLIED  -- source of frame content --
    ///   frameborder (1|0)          1         -- request frame borders? --
    ///   marginwidth %Pixels;       #IMPLIED  -- margin widths in pixels --
    ///   marginheight %Pixels;      #IMPLIED  -- margin height in pixels --
    ///   scrolling   (yes|no|auto)  auto      -- scrollbar or none --
    ///   align       %IAlign;       #IMPLIED  -- vertical or horizontal alignment --
    ///   height      %Length;       #IMPLIED  -- frame height --
    ///   width       %Length;       #IMPLIED  -- frame width --
    ///   &gt;
    /// 
    /// Attribute definitions
    /// 
    /// longdesc = uri [CT] 
    ///     This attribute specifies a link to a long description of the frame. This description should supplement the short description provided using the title attribute, and is particularly useful for non-visual user agents. 
    /// name = cdata [CI] 
    ///     This attribute assigns a name to the current frame. This name may be used as the target of subsequent links. 
    /// width = length [CN] 
    ///     The width of the inline frame. 
    /// height = length [CN] 
    ///     The height of the inline frame. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// title (element title) 
    /// style (inline style information) 
    /// name, src, frameborder, marginwidth, marginheight, scrolling (frame controls and decoration) 
    /// align (alignment) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "IFRAME")]
    public sealed class IFrame : HtmlElement
    {
        internal IFrame(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
