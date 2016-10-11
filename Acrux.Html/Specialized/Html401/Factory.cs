using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Diagnostics;

namespace Acrux.Html.Specialized.Html401
{
    internal static class Factory
    {
        public static HtmlElement CreateHtml401Element(
            string prefix, 
            string localName, 
            string namespaceURI, 
            HtmlDocument doc,
            bool isEmptyTag, 
            NodePosition parsedPosition)
        {
            if (!string.IsNullOrEmpty(prefix))
                // We dont create elements with prefixes. All standard HTML elements must not have prefixes.
                // A default namespace could be used though.
                return null;

            string upperName = localName.ToUpper(CultureInfo.InvariantCulture);

            if (upperName.Equals("A"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.Anchor(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("ABBR"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.AbbreviatedForm(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("ACRONYM"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.Acronym(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("ADDRESS"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.Address(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("APPLET"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.Applet(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("AREA"))
                /* FORBIDDEN */
                return new Html401.Area(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("B"))
                /* REQUIRED; INLINE; NESTING */
                return new Html401.BoldFont(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("BASE"))
                /* FORBIDDEN; HEAD */
                return new Html401.Base(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("BASEFONT"))
                /* FORBIDDEN */
                return new Html401.BaseFont(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("BDO"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.BidirectionalOverride(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("BIG"))
                /* REQUIRED; INLINE; NESTING */
                return new Html401.BigFont(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("BLOCKQUOTE"))
                /* REQUIRED; BLOCK-LEVEL; NESTING */
                return new Html401.BlockQuote(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("BODY"))
            {
                Debug.Assert(string.IsNullOrEmpty(namespaceURI));

                /* OPTIONAL; STRUCTURE */
                return new Html401.Body(prefix, localName, doc, isEmptyTag, parsedPosition);
            }

            if (upperName.Equals("BR"))
                /* FORBIDDEN */
                return new Html401.LineBreak(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("BUTTON"))
                /* REQUIRED; BLOCK-LEVEL; NO-NESTING */
                return new Html401.Button(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("CAPTION"))
                /* REQUIRED; INILINE; NO-NESTING */
                return new Html401.Caption(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("CENTER"))
                /* REQUIRED; BLOCK-LEVEL; NESTING */
                return new Html401.Center(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("CITE"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.Citation(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("CODE"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.Code(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("COL"))
                /* FORBIDDEN */
                return new Html401.Column(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("COLGROUP"))
                /* OPTIONAL; BLOCK-LEVEL; NO-NESTING */
                return new Html401.ColumnGroup(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("DD"))
                /* OPTIONAL; BLOCK-LEVEL; NO-NESTING */
                return new Html401.DefinitionDescription(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("DEL"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.DeletedText(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("DFN"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.DefiningInstance(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("DIR"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.DirectoryList(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("DIV"))
                /* REQUIRED; BLOCK-LEVEL; NO-NESTING */
                return new Html401.Div(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("DL"))
                /* REQUIRED; BLOCK-LEVEL; NO-NESTING */
                return new Html401.DefinitionList(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("DT"))
                /* OPTIONAL; BLOCK-LEVEL; NO-NESTING */
                return new Html401.DefinitionTerm(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("EM"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.Emphasis(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("FIELDSET"))
                /* REQUIRED; BLOCK-LEVEL; NO-NESTING */
                return new Html401.Fieldset(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("FONT"))
                /* REQUIRED; INLINE; NESTING */
                return new Html401.Font(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("FORM"))
                /* REQUIRED; BLOCK-LEVEL; NESTING */
                return new Html401.Form(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("FRAME"))
                /* FORBIDDEN */
                return new Html401.Frame(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("FRAMESET"))
                /* REQUIRED; BLOCK-LEVEL; NESTING */
                return new Html401.Frameset(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("H1"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.HeadingLevel1(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("H2"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.HeadingLevel2(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("H3"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.HeadingLevel3(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("H4"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.HeadingLevel4(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("H5"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.HeadingLevel5(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("H6"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.HeadingLevel6(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("HEAD"))
            {
                Debug.Assert(string.IsNullOrEmpty(namespaceURI));

                /* OPTIONAL; STRUCUTRE */
                return new Html401.Head(prefix, localName, doc, isEmptyTag, parsedPosition);
            }

            if (upperName.Equals("HR"))
                /* FORBIDDEN */
                return new Html401.HorizontalRule(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("HTML"))
                /* OPTIONAL; STRUCUTRE */
                return new Html401.Html(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("I"))
                /* REQUIRED; INLINE; NESTING */
                return new Html401.ItalicFont(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("IFRAME"))
                /* REQUIRED; BLOCK-LEVEL; NO-NESTING */
                return new Html401.IFrame(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("IMG"))
                /* FORBIDDEN */
                return new Html401.Image(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("INPUT"))
                /* REQUIRED; BLOCK-LEVEL; NO-NESTING */
                return new Html401.Input(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("INS"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.InsertedText(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("ISINDEX"))
                /* FORBIDDEN */
                return new Html401.Isindex(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("KBD"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.Kbd(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("LABEL"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.Label(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("LEGEND"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.FieldsetLegend(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("LI"))
                /* OPTIONAL; BLOCK-LEVEL; NO-NESTING */
                return new Html401.ListItem(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("LINK"))
                /* FORBIDDEN; HEAD */
                return new Html401.Link(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("MAP"))
                /* REQUIRED; BLOCK-LEVEL; NO-NESTING */
                return new Html401.ImageMap(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("MENU"))
                /* REQUIRED; BLOCK-LEVEL; NO-NESTING */
                return new Html401.MenuList(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("META"))
                /* FORBIDDEN; HEAD */
                return new Html401.MetaInformation(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("NOFRAMES"))
                /* REQUIRED; BLOCK-LEVEL; NO-NESTING */
                return new Html401.NoFrames(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("NOSCRIPT"))
                /* REQUIRED; BLOCK-LEVEL; NO-NESTING */
                return new Html401.NoScript(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("OBJECT"))
                /* REQUIRED; BLOCK-LEVEL; NO-NESTING */
                return new Html401.Object(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("OPTGROUP"))
                /* REQUIRED; BLOCK-LEVEL; NESTING */
                return new Html401.OptionGroup(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("OPTION"))
                /* OPTIONAL; BLOCK-LEVEL; NO-NESTING */
                return new Html401.SelectableChoice(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("OL"))
                /* REQUIRED; BLOCK-LEVEL; NESTING */
                return new Html401.OrderedList(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("P"))
                /* OPTIONAL; INLINE; NO-NESTING */
                return new Html401.Paragraph(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("PARAM"))
                /* FORBIDDEN */
                return new Html401.PropertyValue(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("PRE"))
                /* REQUIRED; BLOCK-LEVEL; NO-NESTING */
                return new Html401.PreformatedText(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("Q"))
                /* REQUIRED; INLINE; NESTING */
                return new Html401.ShortQuote(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("S"))
                /* REQUIRED; INLINE; NESTING */
                return new Html401.StrikeThroughFontShort(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("SAMP"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.Sample(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("SCRIPT"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.ScriptStatement(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("SELECT"))
                /* REQUIRED; BLOCK-LEVEL; NO-NESTING */
                return new Html401.OptionSelector(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("SMALL"))
                /* REQUIRED; INLINE; NESTING */
                return new Html401.SmallFont(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("SPAN"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.Span(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("STRIKE"))
                /* REQUIRED; BLOCK-LEVEL; NESTING */
                return new Html401.StrikeThroughFont(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("STRONG"))
                /* REQUIRED; BLOCK-LEVEL; NO-NESTING */
                return new Html401.StrongFont(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("STYLE"))
                /* REQUIRED; BLOCK-LEVEL; NO-NESTING; HEAD */
                return new Html401.Style(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("SUB"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.SubScript(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("SUP"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.SuperScript(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("TABLE"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.Table(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("TBODY"))
                /* OPTIONAL; BLOCK-LEVEL; NO-NESTING */
                return new Html401.TableBody(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("TD"))
                /* OPTIONAL; BLOCK-LEVEL; NO-NESTING */
                return new Html401.TableCell(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("TEXTAREA"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.TextArea(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("TFOOT"))
                /* OPTIONAL; BLOCK-LEVEL; NO-NESTING */
                return new Html401.TableFooter(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("TH"))
                /* OPTIONAL; BLOCK-LEVEL; NO-NESTING */
                return new Html401.TableHeaderCell(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("THEAD"))
                /* OPTIONAL; BLOCK-LEVEL; NO-NESTING */
                return new Html401.TableHead(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("TR"))
                /* OPTIONAL; BLOCK-LEVEL; NO-NESTING */
                return new Html401.TableRow(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("TITLE"))
                /* FORBIDDEN; HEAD */
                return new Html401.Title(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("TT"))
                /* REQUIRED; INLINE; NESTING */
                return new Html401.TeletypeFont(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("U"))
                /* REQUIRED; INLINE; NESTING */
                return new Html401.UnderlinedFont(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("UL"))
                /* REQUIRED; BLOCK-LEVEL; NESTING */
                return new Html401.UnorderedList(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            if (upperName.Equals("VAR"))
                /* REQUIRED; INLINE; NO-NESTING */
                return new Html401.Variable(prefix, localName, namespaceURI, doc, isEmptyTag, parsedPosition);

            return null;
        }

    }
}
