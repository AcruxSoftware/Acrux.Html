using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized.Html401
{

    /// <summary>
    /// &lt;!ELEMENT AREA - O EMPTY               -- client-side image map area --&gt;
    /// &lt;!ATTLIST AREA
    ///   %attrs;                              -- %coreattrs, %i18n, %events --
    ///   shape       %Shape;        rect      -- controls interpretation of coords --
    ///   coords      %Coords;       #IMPLIED  -- comma-separated list of lengths --
    ///   href        %URI;          #IMPLIED  -- URI for linked resource --
    ///   nohref      (nohref)       #IMPLIED  -- this region has no action --
    ///   alt         %Text;         #REQUIRED -- short description --
    ///   tabindex    NUMBER         #IMPLIED  -- position in tabbing order --
    ///   accesskey   %Character;    #IMPLIED  -- accessibility key character --
    ///   onfocus     %Script;       #IMPLIED  -- the element got the focus --
    ///   onblur      %Script;       #IMPLIED  -- the element lost the focus --
    ///   &gt;
    ///
    /// AREA attribute definitions
    /// 
    /// shape = default|rect|circle|poly [CI] 
    ///     This attribute specifies the shape of a region. Possible values: 
    ///         default: Specifies the entire region. 
    ///         rect: Define a rectangular region. 
    ///         circle: Define a circular region. 
    ///         poly: Define a polygonal region. 
    /// coords = coordinates [CN] 
    ///     This attribute specifies the position and shape on the screen. The number and order of values depends on the shape being defined. Possible combinations: 
    ///         rect: left-x, top-y, right-x, bottom-y. 
    ///         circle: center-x, center-y, radius. Note. When the radius value is a percentage value, user agents should calculate the final radius value based on the associated object's width and height. The radius should be the smaller value of the two. 
    ///         poly: x1, y1, x2, y2, ..., xN, yN. The first x and y coordinate pair and the last should be the same to close the polygon. When these coordinate values are not the same, user agents should infer an additional coordinate pair to close the polygon. 
    ///     Coordinates are relative to the top, left corner of the object. All values are lengths. All values are separated by commas.
    ///  
    ///  nohref [CI] 
    ///     When set, this boolean attribute specifies that a region has no associated link. 
    /// 
    /// Attributes defined elsewhere
    /// 
    /// id, class (document-wide identifiers) 
    /// lang (language information), dir (text direction) 
    /// title (element title) 
    /// style (inline style information) 
    /// name (submitting objects with forms) 
    /// alt (alternate text) 
    /// href (anchor reference) target (frame target information) 
    /// tabindex (tabbing navigation) 
    /// accesskey (access keys) 
    /// shape (image maps) 
    /// onclick, ondblclick, onmousedown, onmouseup, onmouseover, onmousemove, onmouseout, onkeypress, onkeydown, onkeyup, onfocus, onblur (intrinsic events) 
    /// 
    /// </summary>
    [HtmlTagAttributesMapper(TagName = "AREA")]
    public sealed class Area : HtmlElement
    {
        internal Area(string prefix, string localName, string namespaceURI, HtmlDocument doc, bool isEmptyTag, NodePosition parsedPosition)
            : base(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition)
        { }

        #region Structure Definition

        internal override bool IsDeprecated
        { get { return true; } }

        internal override ElementClosingTagRequirement ClosingTagRequirement
        { get { return ElementClosingTagRequirement.Forbidden; } }

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
