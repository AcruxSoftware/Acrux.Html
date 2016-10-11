using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{
    /// <summary>
    /// &lt;!-- %head.misc; defined earlier on as "SCRIPT|STYLE|META|LINK|OBJECT" --&gt;
    /// &lt;!ENTITY % head.content "TITLE & BASE?"&gt;
    /// 
    /// &lt;!ELEMENT HEAD O O (%head.content;) +(%head.misc;) -- document head --&gt;
    /// &lt;!ATTLIST HEAD
    ///   %i18n;                               -- lang, dir --
    ///   profile     %URI;          #IMPLIED  -- named dictionary of meta info --
    ///   &gt;
    /// 
    /// Attribute definitions
    /// 
    /// profile = uri [CT] 
    ///     This attribute specifies the location of one or more meta data profiles, separated by white space. For future extensions, user agents should consider the value to be a list even though this specification only considers the first URI to be significant. Profiles are discussed below in the section on meta data. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// lang (language information), dir (text direction) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "HEAD")]
    public sealed class Head : HtmlElement
    {
        internal Head(string prefix, string localName, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
