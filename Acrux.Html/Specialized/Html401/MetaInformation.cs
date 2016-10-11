using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!ELEMENT META - O EMPTY               -- generic metainformation --&gt;
    /// &lt;!ATTLIST META
    ///   %i18n;                               -- lang, dir, for use with content --
    ///   http-equiv  NAME           #IMPLIED  -- HTTP response header name  --
    ///   name        NAME           #IMPLIED  -- metainformation name --
    ///   content     CDATA          #REQUIRED -- associated information --
    ///   scheme      CDATA          #IMPLIED  -- select form of content --
    ///   &gt;
    /// 
    /// Attribute definitions
    /// 
    /// For the following attributes, the permitted values and their interpretation are profile dependent:
    /// 
    /// name = name [CS] 
    ///     This attribute identifies a property name. This specification does not list legal values for this attribute. 
    /// content = cdata [CS] 
    ///     This attribute specifies a property's value. This specification does not list legal values for this attribute. 
    /// scheme = cdata [CS] 
    ///     This attribute names a scheme to be used to interpret the property's value (see the section on profiles for details). 
    /// http-equiv = name [CI] 
    ///     This attribute may be used in place of the name attribute. HTTP servers use this attribute to gather information for HTTP response message headers. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// lang (language information), dir (text direction) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "META")]
    public sealed class MetaInformation : HtmlElement
    {
        internal MetaInformation(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
