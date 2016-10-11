using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!ELEMENT APPLET - - (PARAM | %flow;)* -- Java applet --&gt;
    /// &lt;!ATTLIST APPLET
    ///   %coreattrs;                          -- id, class, style, title --
    ///   codebase    %URI;          #IMPLIED  -- optional base URI for applet --
    ///   archive     CDATA          #IMPLIED  -- comma-separated archive list --
    ///   code        CDATA          #IMPLIED  -- applet class file --
    ///   object      CDATA          #IMPLIED  -- serialized applet file --
    ///   alt         %Text;         #IMPLIED  -- short description --
    ///   name        CDATA          #IMPLIED  -- allows applets to find each other --
    ///   width       %Length;       #REQUIRED -- initial width --
    ///   height      %Length;       #REQUIRED -- initial height --
    ///   align       %IAlign;       #IMPLIED  -- vertical or horizontal alignment --
    ///   hspace      %Pixels;       #IMPLIED  -- horizontal gutter --
    ///   vspace      %Pixels;       #IMPLIED  -- vertical gutter --
    ///   &gt;
    /// 
    /// Attribute definitions
    /// 
    /// codebase = uri [CT] 
    ///     This attribute specifies the base URI for the applet. If this attribute is not specified, then it defaults the same base URI as for the current document. Values for this attribute may only refer to subdirectories of the directory containing the current document. Note. While the restriction on subdirectories is a departure from common practice and the HTML 3.2 specification, the HTML Working Group has chosen to leave the restriction in this version of the specification for security reasons. 
    /// code = cdata [CS] 
    ///     This attribute specifies either the name of the class file that contains the applet's compiled applet subclass or the path to get the class, including the class file itself. It is interpreted with respect to the applet's codebase. One of code or object must be present. 
    /// name = cdata [CS] 
    ///     This attribute specifies a name for the applet instance, which makes it possible for applets on the same page to find (and communicate with) each other. 
    /// archive = uri-list [CT] 
    ///     This attribute specifies a comma-separated list of URIs for archives containing classes and other resources that will be "preloaded". The classes are loaded using an instance of an AppletClassLoader with the given codebase. Relative URIs for archives are interpreted with respect to the applet's codebase. Preloading resources can significantly improve the performance of applets. 
    /// object = cdata [CS] 
    ///     This attribute names a resource containing a serialized representation of an applet's state. It is interpreted relative to the applet's codebase. The serialized data contains the applet's class name but not the implementation. The class name is used to retrieve the implementation from a class file or archive. 
    ///     When the applet is "deserialized" the start() method is invoked but not the init() method. Attributes valid when the original object was serialized are not restored. Any attributes passed to this APPLET instance will be available to the applet. Authors should use this feature with extreme caution. An applet should be stopped before it is serialized.
    /// 
    ///     Either code or object must be present. If both code and object are given, it is an error if they provide different class names.
    /// 
    /// width = length [CI] 
    ///     This attribute specifies the initial width of the applet's display area (excluding any windows or dialogs that the applet creates). 
    /// height = length [CI] 
    ///     This attribute specifies the initial height of the applet's display area (excluding any windows or dialogs that the applet creates). 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// title (element title) 
    /// style (inline style information) 
    /// alt (alternate text) 
    /// align, hspace, vspace (visual presentation of objects, images, and applets) 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "APPLET")]
    public sealed class Applet : HtmlElement
    {
        internal Applet(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
