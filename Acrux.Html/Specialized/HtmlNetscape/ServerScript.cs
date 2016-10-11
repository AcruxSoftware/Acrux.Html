using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.HtmlNetscape
{
    /// <summary>
    /// SERVER
    /// (server-side script)
    /// The SERVER tag specifies server-side JavaScript statements that are used in a JavaScript application on the server. (Contrast this with theSCRIPT tag, which specifies client-side JavaScript statements that run in the browser.) When a script is specified within the SERVER tag, it is run on the server before the page is passed to the browser. See Writing Server-Side JavaScript Applications for more information.
    ///
    /// Syntax &lt;SERVER&gt;...&lt;/SERVER&gt;
    ///
    /// Example
    /// &lt;P&gt;Your IP address is &lt;SERVER&gt;write(request.ip);&lt;/SERVER&gt;
    ///
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "SERVER")]
    public sealed class ServerScript : HtmlElement
    {
        internal ServerScript(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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

        });

        internal override List<string> ResetTags
        { get { return s_ResetTags; } }

        internal override List<string> IgnoreTags
        { get { return s_IgnoreTags; } }

        #endregion
    }
}
