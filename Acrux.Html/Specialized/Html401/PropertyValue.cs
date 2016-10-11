using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{
    /// <summary>
    /// &lt;!ELEMENT PARAM - O EMPTY              -- named property value --&gt;
    /// &lt;!ATTLIST PARAM
    ///   id          ID             #IMPLIED  -- document-wide unique id --
    ///   name        CDATA          #REQUIRED -- property name --
    ///   value       CDATA          #IMPLIED  -- property value --
    ///   valuetype   (DATA|REF|OBJECT) DATA   -- How to interpret value --
    ///   type        %ContentType;  #IMPLIED  -- content type for value
    ///                                           when valuetype=ref --
    ///   &gt;
    /// 
    /// Attribute definitions
    /// 
    /// name = cdata 
    ///     This attribute defines the name of a run-time parameter, assumed to be known by the inserted object. Whether the property name is case-sensitive depends on the specific object implementation. 
    /// value = cdata 
    ///     This attribute specifies the value of a run-time parameter specified by name. Property values have no meaning to HTML; their meaning is determined by the object in question. 
    /// valuetype = data|ref|object [CI] 
    ///     This attribute specifies the type of the value attribute. Possible values: 
    ///         data: This is default value for the attribute. It means that the value specified by value will be evaluated and passed to the object's implementation as a string. 
    ///         ref: The value specified by value is a URI that designates a resource where run-time values are stored. This allows support tools to identify URIs given as parameters. The URI must be passed to the object as is, i.e., unresolved. 
    ///         object: The value specified by value is an identifier that refers to an OBJECT declaration in the same document. The identifier must be the value of the id attribute set for the declared OBJECT element. 
    /// type = content-type [CI] 
    ///     This attribute specifies the content type of the resource designated by the value attribute only in the case where valuetype is set to "ref". This attribute thus specifies for the user agent, the type of values that will be found at the URI designated by value. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id (document-wide identifiers) 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "PARAM")]
    public sealed class PropertyValue : HtmlElement
    {
        internal PropertyValue(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
            "AREA", "CAPTION", "COL", "COLGROUP", "TBODY", "TD", "TFOOT", "TH", "THEAD", "TR"
        });

        internal override List<string> ResetTags
        { get { return s_ResetTags; } }

        internal override List<string> IgnoreTags
        { get { return s_IgnoreTags; } }

        #endregion
    }
}
