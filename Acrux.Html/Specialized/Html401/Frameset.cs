using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;![ %HTML.Frameset; [
    /// &lt;!ELEMENT FRAMESET - - ((FRAMESET|FRAME)+ &amp; NOFRAMES?) -- window subdivision--&gt;
    /// &lt;!ATTLIST FRAMESET
    ///   %coreattrs;                          -- id, class, style, title --
    ///   rows        %MultiLengths; #IMPLIED  -- list of lengths,
    ///                                           default: 100% (1 row) --
    ///   cols        %MultiLengths; #IMPLIED  -- list of lengths,
    ///                                           default: 100% (1 col) --
    ///   onload      %Script;       #IMPLIED  -- all the frames have been loaded  -- 
    ///   onunload    %Script;       #IMPLIED  -- all the frames have been removed -- 
    ///   &gt;
    /// ]]&gt;
    ///
    /// Attribute definitions
    /// 
    /// rows = multi-length-list [CN] 
    ///     This attribute specifies the layout of horizontal frames. It is a comma-separated list of pixels, percentages, and relative lengths. The default value is 100%, meaning one row. 
    /// cols = multi-length-list [CN] 
    ///     This attribute specifies the layout of vertical frames. It is a comma-separated list of pixels, percentages, and relative lengths. The default value is 100%, meaning one column. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// title (element title) 
    /// style (inline style information) 
    /// onload, onunload (intrinsic events) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "FRAMESET")]
    public sealed class Frameset : HtmlElement
    {
        internal Frameset(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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

        });

        internal override List<string> ResetTags
        { get { return s_ResetTags; } }

        internal override List<string> IgnoreTags
        { get { return s_IgnoreTags; } }

        #endregion
    }
}
