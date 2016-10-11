using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;

namespace Acrux.Html.Specialized
{
    public sealed class HtmlDocTypeElement : HtmlNode 
    {
        public enum IdType 
        {
            NotSpecified,
            Public,
            System
        }

        // Default value is null
        internal string m_Comment;

        internal HtmlDocTypeElement(
            string name, 
            string externalIdName, 
            string publicLiteral,
            string systemLiteral,
            string internalSubset,
            HtmlDocument doc)
            : base(XmlNodeType.DocumentType, name, publicLiteral, null, internalSubset, doc, NodePosition.ReadOnly)
        {
            //NOTE: Passing a valid systemId causes the XML namespace to try and validate the document against the given DTD
            //      do we want this? (not really). Expose the SystemId here and pass null to the base class (XmlRefDocumentType)

            m_ExternalIdName = externalIdName;
            m_SystemLiteral = systemLiteral;

            if (m_ExternalIdName != null)
            {
                if (m_ExternalIdName.Equals("PUBLIC", StringComparison.CurrentCultureIgnoreCase))
                    m_ExternalIdType = IdType.Public;
                else if (m_ExternalIdName.Equals("SYSTEM", StringComparison.CurrentCultureIgnoreCase))
                    m_ExternalIdType = IdType.System;
            }
        }

        private string m_ExternalIdName = null;

        private IdType m_ExternalIdType = IdType.NotSpecified;

        public IdType ExternalIdType
        {
            get
            {
                return m_ExternalIdType;
            }
        }

        private string m_SystemLiteral = null;

        public string ExternalIdName
        {
            get
            {
                return m_ExternalIdName;
            }
        }

        public string SystemId
        {
            get
            {
                return m_SystemLiteral;
            }
        }

        public string PublicId
        {
            get
            {
                Debug.Assert(m_XmlNode != null);
                Debug.Assert(m_XmlNode is XmlDocumentType);

                return (m_XmlNode as XmlDocumentType).PublicId;
            }
        }

        public string InternalSubset
        {
            get
            {
                Debug.Assert(m_XmlNode != null);
                Debug.Assert(m_XmlNode is XmlDocumentType);

                return (m_XmlNode as XmlDocumentType).InternalSubset;
            }
        }

        public string Comment
        {
            get
            {
                return m_Comment;
            }
        }

    }
}
