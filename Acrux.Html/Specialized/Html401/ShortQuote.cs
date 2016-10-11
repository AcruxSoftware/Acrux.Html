using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!ELEMENT Q - - (%inline;)*            -- short inline quotation --&gt;
    /// &lt;!ATTLIST Q
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   cite        %URI;          #IMPLIED  -- URI for source document or msg --
    ///   &gt;
    /// 
    /// Attribute definitions
    /// 
    /// cite = uri [CT] 
    ///     The value of this attribute is a URI that designates a source document or message. This attribute is intended to give information about the source from which the quotation was borrowed. 
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
    [HtmlTagAttributesMapper(TagName = "Q")]
    public sealed class ShortQuote : HtmlElement
    {
        internal ShortQuote(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
            : base(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition)
        { }

        //TODO: Unit test for nested Qs. This is valid HTML:
        //
        // John said, <Q lang="en-us">I saw Lucy at lunch, she told me
        // <Q lang="en-us">Mary wants you
        // to get some ice cream on your way home.</Q> I think I will get
        // some at Ben and Jerry's, on Gloucester Road.</Q>

        #region Structure Definition

        internal override ElementClosingTagRequirement ClosingTagRequirement
        { get { return ElementClosingTagRequirement.Required; } }

        internal override bool AllowNestedElements
        { get { return true; } }

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
