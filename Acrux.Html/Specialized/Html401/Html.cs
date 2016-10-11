using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{
    /// <summary>
    /// <!ENTITY % html.content "HEAD, BODY">
    /// 
    /// <!ELEMENT HTML O O (%html.content;)    -- document root element -->
    /// <!ATTLIST HTML
    ///   %i18n;                               -- lang, dir --
    ///   >
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "HTML")]
    public sealed class Html : HtmlElement
    {
        internal string m_NamespaceURI;

        internal Html(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
            : base(prefix, localName, "", doc, isEmptyTag, parsedPosition)
        { 
            // We ignore the namespace of the HTML element for easier XPath queries
            // Still we provide the namespace through the NamespaceURI property
            this.m_NamespaceURI = namespaceURI;
        }

        public new string NamespaceURI
        {
            get { return m_NamespaceURI; }
        }

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
        { get { return OptionalClosingTagType.None; } }

        internal override string[] OptionalClosingTagSpecifiedTags
        { get { return null; } }

        #endregion
    }
}
