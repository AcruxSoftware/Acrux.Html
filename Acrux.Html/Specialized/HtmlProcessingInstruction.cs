using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Xml;

namespace Acrux.Html.Specialized
{
    public sealed class HtmlProcessingInstruction : HtmlNode
    {
        private string m_Target;
        private string m_Data;

        internal HtmlProcessingInstruction(string target, string data, HtmlDocument doc)
            : base(XmlNodeType.ProcessingInstruction, target, data, doc, NodePosition.ReadOnly)
        {
            m_Target = target;
            m_Data = data;
        }

        public string Target
        {
            get
            {
                return m_Target;
            }
        }

        public string Data
        {
            get
            {
                return m_Data;
            }
        }
    }
}
