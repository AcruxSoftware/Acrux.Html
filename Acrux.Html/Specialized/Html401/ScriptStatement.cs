using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    public enum ScriptLanguage   
    {
        Tcl,
        JavaScript,
        VBScript
    }

    /// <summary>
    /// &lt;!ELEMENT SCRIPT - - %Script;          -- script statements --&gt;
    /// &lt;!ATTLIST SCRIPT
    ///   charset     %Charset;      #IMPLIED  -- char encoding of linked resource --
    ///   type        %ContentType;  #REQUIRED -- content type of script language --
    ///   src         %URI;          #IMPLIED  -- URI for an external script --
    ///   defer       (defer)        #IMPLIED  -- UA may defer execution of script --
    ///   &gt;
    /// 
    /// Attribute definitions
    /// 
    /// src = uri [CT] 
    ///     This attribute specifies the location of an external script. 
    /// type = content-type [CI] 
    ///     This attribute specifies the scripting language of the element's contents and overrides the default scripting language. The scripting language is specified as a content type (e.g., "text/javascript"). Authors must supply a value for this attribute. There is no default value for this attribute. 
    /// language = cdata [CI] 
    ///     Deprecated. This attribute specifies the scripting language of the contents of this element. Its value is an identifier for the language, but since these identifiers are not standard, this attribute has been deprecated in favor of type. 
    /// defer [CI] 
    ///     When set, this boolean attribute provides a hint to the user agent that the script is not going to generate any document content (e.g., no "document.write" in javascript) and thus, the user agent can continue parsing and rendering. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// charset(character encodings) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "SCRIPT")]
    public sealed class ScriptStatement : HtmlElement
    {
        internal ScriptStatement(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
            : base(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition)
        { }


        // TODO: Probably implement the parsing of the script here ...
        //       at least parsing the language specific comments

        // Supported scripts: "text/tcl", "text/javascript", "text/vbscript". TODO: Find more
        //
        // TODO: Is it possible to insert a comment in a script inside attribute parametter
        //
        //<INPUT NAME="num" onchange="if (!checkNum(this.value, 1, 10)) /*"> <BOZA> */ {this.focus();this.select();} else {thanks()}" VALUE="0">
        //
        
        // TODO: Default scripting language can be defined in 
        // 1) <META http-equiv="Content-Script-Type" content="...">
        // 2) The HTTP header: Content-Script-Type: type

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


        public ScriptLanguage Language
        {
            get
            {
                string lang = "javascript";

                if (this.Attributes["language"] != null)
                    lang = this.Attributes["language"].Value;
                else if (this.Attributes["lang"] != null)
                    lang = this.Attributes["lang"].Value;

                if ("vbscript".Equals(lang, StringComparison.InvariantCultureIgnoreCase))
                    return ScriptLanguage.VBScript;
                else if ("vbscript".Equals(lang, StringComparison.InvariantCultureIgnoreCase))
                    return ScriptLanguage.Tcl;
                else
                    return ScriptLanguage.JavaScript;
            }
        }

        internal string m_ScriptContent = null;

        public string Script
        {
            get { return m_ScriptContent != null ? m_ScriptContent : string.Empty; }
        }
    }
}
