using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;![ %HTML.Frameset; [
    /// &lt;!-- reserved frame names start with "_" otherwise starts with letter --&gt;
    /// &lt;!ELEMENT FRAME - O EMPTY              -- subwindow --&gt;
    /// &lt;!ATTLIST FRAME
    ///   %coreattrs;                          -- id, class, style, title --
    ///   longdesc    %URI;          #IMPLIED  -- link to long description
    ///                                           (complements title) --
    ///   name        CDATA          #IMPLIED  -- name of frame for targetting --
    ///   src         %URI;          #IMPLIED  -- source of frame content --
    ///   frameborder (1|0)          1         -- request frame borders? --
    ///   marginwidth %Pixels;       #IMPLIED  -- margin widths in pixels --
    ///   marginheight %Pixels;      #IMPLIED  -- margin height in pixels --
    ///   noresize    (noresize)     #IMPLIED  -- allow users to resize frames? --
    ///   scrolling   (yes|no|auto)  auto      -- scrollbar or none --
    ///   &gt;
    /// ]]&gt;
    ///   
    /// 
    /// Attribute definitions
    /// 
    /// name = cdata [CI] 
    ///     This attribute assigns a name to the current frame. This name may be used as the target of subsequent links. 
    /// longdesc = uri [CT] 
    ///     This attribute specifies a link to a long description of the frame. This description should supplement the short description provided using the title attribute, and may be particularly useful for non-visual user agents. 
    /// src = uri [CT] 
    ///     This attribute specifies the location of the initial contents to be contained in the frame. 
    /// noresize [CI] 
    ///     When present, this boolean attribute tells the user agent that the frame window must not be resizeable. 
    /// scrolling = auto|yes|no [CI] 
    ///     This attribute specifies scroll information for the frame window. Possible values 
    ///         auto: This value tells the user agent to provide scrolling devices for the frame window when necessary. This is the default value. 
    ///         yes: This value tells the user agent to always provide scrolling devices for the frame window. 
    ///         no: This value tells the user agent not to provide scrolling devices for the frame window. 
    /// frameborder = 1|0 [CN] 
    ///     This attribute provides the user agent with information about the frame border. Possible values: 
    ///         1: This value tells the user agent to draw a separator between this frame and every adjoining frame. This is the default value. 
    ///         0: This value tells the user agent not to draw a separator between this frame and every adjoining frame. Note that separators may be drawn next to this frame nonetheless if specified by other frames. 
    /// marginwidth = pixels [CN] 
    ///     This attribute specifies the amount of space to be left between the frame's contents in its left and right margins. The value must be greater than zero (pixels). The default value depends on the user agent. 
    /// marginheight = pixels [CN] 
    ///     This attribute specifies the amount of space to be left between the frame's contents in its top and bottom margins. The value must be greater than zero (pixels). The default value depends on the user agent. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// title (element title) 
    /// style (inline style information) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "FRAME")]
    public sealed class Frame : HtmlElement
    {
        internal Frame(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
            : base(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition)
        { }

        #region Structure Definition

        internal override ElementClosingTagRequirement ClosingTagRequirement
        { get { return ElementClosingTagRequirement.Forbidden; } }

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

        });

        internal override List<string> ResetTags
        { get { return s_ResetTags; } }

        internal override List<string> IgnoreTags
        { get { return s_IgnoreTags; } }

        #endregion
    }
}
