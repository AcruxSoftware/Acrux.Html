using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{
    /// <summary>
    /// &lt;!ELEMENT BASEFONT - O EMPTY           -- base font size --&lt;
    /// &lt;!ATTLIST BASEFONT
    ///   id          ID             #IMPLIED  -- document-wide unique id --
    ///   size        CDATA          #REQUIRED -- base font size for FONT elements --
    ///   color       %Color;        #IMPLIED  -- text color --
    ///   face        CDATA          #IMPLIED  -- comma-separated list of font names --
    ///   &lt;
    /// 
    /// Attribute definitions
    /// 
    /// size  = cdata [CN] 
    ///     Deprecated. This attribute sets the size of the font. Possible values: 
    ///     An integer between 1 and 7. This sets the font to some fixed size, whose rendering depends on the user agent. Not all user agents may render all seven sizes. 
    ///     A relative increase in font size. The value "+1" means one size larger. The value "-3" means three sizes smaller. All sizes belong to the scale of 1 to 7. 
    /// color = color [CI] 
    ///     Deprecated. This attribute sets the text color. 
    /// face = cdata [CI] 
    ///     Deprecated. This attribute defines a comma-separated list of font names the user agent should search for in order of preference. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "BASEFONT")]
    public sealed class BaseFont : HtmlElement
    {
        internal BaseFont(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
