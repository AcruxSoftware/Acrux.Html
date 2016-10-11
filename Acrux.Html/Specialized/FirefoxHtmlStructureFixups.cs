using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html.Specialized
{
    internal static class FirefoxHtmlStructureFixups
    {
        private static List<string> ALLOWED_TAGS_IN_TABLE_STRUCTURE = new List<string>(new string[] { "script", "noscript"} );
        private static List<string> TAGS_IN_TABLE_STRUCTURE = new List<string>(new string[] { "table", "tr", "td", "th", "caption", "thead", "tbody", "tfoot", "colgroup", "col" });

        internal static void ApplyFixups(HtmlDocument doc)
        {
            CleanUpTables(doc);

        }

        private static void CleanUpTables(HtmlDocument doc)
        {
            foreach (HtmlElement tableNode in doc.SelectNodes("//table"))
            {
                List<HtmlNode> cleanUpElements = new List<HtmlNode>();

                CheckTableNode(tableNode, cleanUpElements);

                foreach (HtmlNode tag in cleanUpElements)
                {
                    if (ALLOWED_TAGS_IN_TABLE_STRUCTURE.IndexOf(tag.Name) == -1)
                        tableNode.ParentNode.InsertBefore(tag, tableNode);
                }
            }
        }

        private static void CheckTableNode(HtmlNode node, List<HtmlNode> cleanUpElements)
        {
            if (node is Html401.TableCell) return;

            foreach (HtmlNode child in node.ChildNodes)
            {
                if (TAGS_IN_TABLE_STRUCTURE.IndexOf(child.Name) > -1)
                    CheckTableNode(child, cleanUpElements);
                else
                    cleanUpElements.Add(child);
            }
        }
    }
}
