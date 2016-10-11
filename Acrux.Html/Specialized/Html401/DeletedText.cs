using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{
    /// <summary>
    /// &lt;!-- INS/DEL are handled by inclusion on BODY --&gt;
    /// &lt;!ELEMENT (INS|DEL) - - (%flow;)*      -- inserted text, deleted text --&gt;
    /// &lt;!ATTLIST (INS|DEL)
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   cite        %URI;          #IMPLIED  -- info on reason for change --
    ///   datetime    %Datetime;     #IMPLIED  -- date and time of change --
    ///   &gt;
    /// 
    /// Attribute definitions
    /// 
    /// cite = uri [CT] 
    ///     The value of this attribute is a URI that designates a source document or message. This attribute is intended to point to information explaining why a document was changed. 
    /// datetime = datetime [CS] 
    ///     The value of this attribute specifies the date and time when the change was made. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information ) 
    /// onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup (intrinsic events ) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "DEL")]
    public sealed class DeletedText : HtmlElement
    {
        internal DeletedText(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
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
