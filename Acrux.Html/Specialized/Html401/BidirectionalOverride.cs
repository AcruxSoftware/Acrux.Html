using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!ELEMENT BDO - - (%inline;)*          -- I18N BiDi over-ride --&gt;
    /// &lt;!ATTLIST BDO
    ///   %coreattrs;                          -- id, class, style, title --
    ///   lang        %LanguageCode; #IMPLIED  -- language code --
    ///   dir         (ltr|rtl)      #REQUIRED -- directionality --
    ///   &gt;
    /// 
    /// Attribute definitions
    /// 
    /// dir = LTR | RTL [CI] 
    ///     This mandatory attribute specifies the base direction of the element's text content. This direction overrides the inherent directionality of characters as defined in [UNICODE]. Possible values: 
    ///             LTR: Left-to-right text. 
    ///             RTL: Right-to-left text. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// lang (language information) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "BDO")]
    public sealed class BidirectionalOverride : HtmlElement
    {
        internal BidirectionalOverride(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
    }
}
