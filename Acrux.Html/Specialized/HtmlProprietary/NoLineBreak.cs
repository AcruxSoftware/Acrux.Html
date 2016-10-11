using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.HtmlProprietary
{

    /// <summary>
    /// Permitted Context: %Body.Content, %flow, %block
    /// Content Model: %text 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "NOBR")]
    public sealed class NoLineBreak : HtmlElement
    {
        internal NoLineBreak(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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

        internal override EndTagWithoutStartTagHandling EndTagWithoutStartTagMode
        { get { return EndTagWithoutStartTagHandling.HandleAsStartTag; } }

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
