using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Acrux.Html.Specialized
{
    public sealed class HtmlCommentElement : HtmlNode
    {
        private string m_Comment = null;

        private static string GetSafeComment(string comment)
        {
            comment = comment.Replace("--", "- - ");
            if (comment.EndsWith("-"))
                comment = comment + " ";
            return comment;
        }

        internal HtmlCommentElement(HtmlDocument htmlDoc, string comment)
            : base(XmlNodeType.Comment, 
                   GetSafeComment(comment), // We escape the comment so the System.Xml namespace doesn't throw 
                                            // an exception "Xml comment cannot contain '--' and cannot end with '-'"
                   htmlDoc,
                    NodePosition.ReadOnly
            )
        {
            m_Comment = comment;
        }

        /// <summary>
        /// Gets the Html comment (the original comment)
        /// </summary>
        public string Comment
        {
            get
            {
                return m_Comment;
            }
        }
    }
}
