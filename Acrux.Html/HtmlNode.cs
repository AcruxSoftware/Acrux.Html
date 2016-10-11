using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using Acrux.Html.Specialized;
using System.Globalization;

namespace Acrux.Html
{
    internal struct NodePosition
    {
        internal int NameStartPos;
        internal int NameEndPos;
        internal int ValueStartPos;
        internal int ValueEndPos;


        private int m_NodeStartPos;
        private int m_NodeEndPos;

        internal int NodeStartPos
        {
            get 
            {
                return m_NodeStartPos == 0 ? NameStartPos : m_NodeStartPos; 
            }
            set 
            { 
                m_NodeStartPos = value; 
            }
        }

        internal int NodeEndPos
        {
            get 
            {
                return m_NodeEndPos == 0 ? ValueEndPos : m_NodeEndPos; 
            }
            set
            { 
                m_NodeEndPos = value; 
            }
        }

        private NodePosition(int startName, int endName, int startVal, int endVal)
        {
            // --> Fixing error CS0188. Fields should be assigned before using properties
            m_NodeStartPos = 0;
            m_NodeEndPos = 0;
            // <-- Fixing error CS0188
            
            NameEndPos = endName;
            ValueStartPos = startVal;

            NameStartPos = startName;
            ValueEndPos = endVal;
        }

        internal static readonly NodePosition ReadOnly = new NodePosition();
        internal static readonly NodePosition Inserted = new NodePosition(-1, -1, -1, -1);
    }

    /// <summary>
    /// Represents a single node in the HTML document.
    /// </summary>
    public class HtmlNode : IEnumerable<HtmlNode>
    {
        internal class OnChildAppendingEventArgs : EventArgs 
        {
            // Default value is "false"
            internal bool Cancel;
        }

        private static int s_IdCounter = -1;
        private int m_Id = -1;
        private object m_SyncObj = new object();


        private HtmlDocument m_Doc;

        // Default value is "null"
        internal XmlNode m_XmlNode;

        private HtmlAttributeCollection m_Attributes;

        private HtmlNodeList m_ChildNodeList;

        internal delegate void OnChildAppendedDelegate(HtmlNode child);
        internal delegate void OnChildAppendingDelegate(HtmlNode child, ref OnChildAppendingEventArgs e);

        // Default value is "null"
        internal OnChildAppendedDelegate OnChildAppended;
        // Default value is "null"
        internal OnChildAppendingDelegate OnChildAppending;

        internal NodePosition m_NodePosition;

        internal NodePosition NodePosition
        {
            get { return m_NodePosition; }
        }

        private void SetNodeId()
        {
            lock (m_SyncObj)
            {
                s_IdCounter++;
                m_Id = s_IdCounter;
            }
        }

        private HtmlNode(NodePosition parsedPosition)
        {
            m_NodePosition = parsedPosition;
        }

        internal HtmlNode(XmlNodeType nodeType, NodePosition parsedPosition)
            : this(parsedPosition)
        {
            SetNodeId();

            Debug.Assert(nodeType == XmlNodeType.Document);
            if (nodeType != XmlNodeType.Document)
                throw new HtmlParserException("This constructor can only be called for XmlNodeType.Document node type.");

            Debug.Assert(this is HtmlDocument);

            Debug.Assert(this is HtmlDocument);

            m_Doc = this as HtmlDocument;
            m_XmlNode = (this as HtmlDocument).m_XmlDoc;
            m_UpperName = null;

            m_Attributes = new HtmlAttributeCollection(this);
            m_ChildNodeList = new HtmlNodeList(m_XmlNode.ChildNodes);
        }

        internal HtmlNode(
                XmlNodeType nodeType, 
                string name, 
                string publicId, 
                string systemId, 
                string internalSubset, 
                HtmlDocument doc,
                NodePosition parsedPosition)
            : this(parsedPosition)
        {
            SetNodeId();

            //m_Doc = doc;

            Debug.Assert(nodeType == XmlNodeType.DocumentType);
            if (nodeType != XmlNodeType.DocumentType)
                throw new HtmlParserException("This constructor can only be called for XmlNodeType.DocumentType node type.");

            XmlRefDocumentType att = new XmlRefDocumentType(name, publicId, systemId, internalSubset, doc.m_XmlDoc, this);
            InitXmlNode(att as XmlNode, doc);
        }

        internal HtmlNode(XmlNodeType nodeType, string commentOrText, HtmlDocument doc, NodePosition parsedPosition)
            : this(parsedPosition)
        {
            SetNodeId();

           // m_Doc = doc;

            Debug.Assert(nodeType == XmlNodeType.Comment || nodeType == XmlNodeType.Text || nodeType == XmlNodeType.XmlDeclaration);
            if (nodeType != XmlNodeType.Comment && nodeType != XmlNodeType.Text && nodeType != XmlNodeType.XmlDeclaration)
                throw new HtmlParserException("This constructor can only be called for XmlNodeType.Comment or XmlNodeType.Text or XmlNodeType.XmlDeclaration node types.");

            if (nodeType == XmlNodeType.Comment)
            {
                XmlRefComment att = new XmlRefComment(commentOrText, doc.m_XmlDoc, this);
                InitXmlNode(att as XmlNode, doc);
            }
            else if (nodeType == XmlNodeType.Text)
            {
                XmlRefText att = new XmlRefText(commentOrText, doc.m_XmlDoc, this);
                InitXmlNode(att as XmlNode, doc);
            }
            else if (nodeType == XmlNodeType.XmlDeclaration)
            {
                XmlRefXmlDeclaration att = new XmlRefXmlDeclaration(doc.m_XmlDoc, this);
                InitXmlNode(att as XmlNode, doc);
            }
            else
            {
                Debug.Assert(false);
                throw new HtmlParserException();
            }
        }

        internal HtmlNode(XmlNodeType nodeType, string target, string data, HtmlDocument doc, NodePosition parsedPosition)
            : this(parsedPosition)
        {
            SetNodeId();

            //m_Doc = doc;

            Debug.Assert(nodeType == XmlNodeType.ProcessingInstruction);
            if (nodeType != XmlNodeType.ProcessingInstruction)
                throw new HtmlParserException("This constructor can only be called for XmlNodeType.ProcessingInstruction");

            XmlRefProcessingInstruction att = new XmlRefProcessingInstruction(target, data, doc.m_XmlDoc, this);

            InitXmlNode(att as XmlNode, doc);
        }

        internal HtmlNode(XmlNodeType nodeType, string prefix, string localName, string namespaceURI, HtmlDocument doc, NodePosition parsedPosition)
            : this(parsedPosition)
        {
            SetNodeId();

            Debug.Assert(doc != null);

            //m_Doc = doc;

            if (nodeType == XmlNodeType.Attribute)
            {
                XmlRefAttribute att = new XmlRefAttribute(prefix, localName, namespaceURI, doc.m_XmlDoc, this);
                InitXmlNode(att as XmlNode, doc); 
            }
            else if (nodeType == XmlNodeType.Element)
            {
                XmlRefElement att = new XmlRefElement(prefix, localName, namespaceURI, doc.m_XmlDoc, this);
                InitXmlNode(att as XmlNode, doc);
            }
            else
            {
                Debug.Assert(false);
                throw new HtmlParserException();
            }
        }

        protected void InitXmlNode(XmlNode xmlNode, HtmlDocument htmlDoc)
        {
            Debug.Assert(xmlNode != null);
            Debug.Assert(
                xmlNode.OwnerDocument == htmlDoc.m_XmlNode.OwnerDocument ||
                xmlNode.OwnerDocument == htmlDoc.m_XmlNode
                );

            m_XmlNode = xmlNode;
            m_UpperName = null;
            m_Doc = htmlDoc;

            m_Attributes = new HtmlAttributeCollection(this);
            m_ChildNodeList = new HtmlNodeList(m_XmlNode.ChildNodes);

        }


        internal HtmlNode AppendChild(HtmlNode node)
        {
            Debug.Assert(m_XmlNode != null);
            Debug.Assert(m_XmlNode.ParentNode != null || (this is HtmlDocument));
            Debug.Assert(m_ChildNodeList != null);
            Debug.Assert(!(node is HtmlElement) || !(node as HtmlElement).IsEndTag, "If the child is an HtmlElement it must not be an 'End Tag'!");

            OnChildAppendingEventArgs ev = new OnChildAppendingEventArgs();

            if (OnChildAppending != null)
                OnChildAppending(node, ref ev);

            if (!ev.Cancel)
            {
                Debug.WriteLine("Adding a (" + node.Name + ") tag as a child of a (" + this.Name + ") tag.");

                Debug.Assert(
                    (m_XmlNode == node.m_XmlNode.OwnerDocument) ||
                    (m_XmlNode.OwnerDocument == node.m_XmlNode.OwnerDocument)
                    );

                m_XmlNode.AppendChild(node.m_XmlNode);
                m_ChildNodeList.m_List.Add(node);

                if (OnChildAppended != null)
                    OnChildAppended(node);

                Debug.Assert(this.Equals(node.ParentNode));
                Debug.Assert(m_XmlNode == node.m_XmlNode.ParentNode);
                Debug.Assert(m_XmlNode.ParentNode != null || (this is HtmlDocument));

                return node;
            }
            else
                return null;
        }

        internal HtmlNode InsertBefore(HtmlNode node, HtmlNode refChild)
        {
            Debug.Assert(m_XmlNode != null);
            Debug.Assert(m_XmlNode.ParentNode != null || (this is HtmlDocument));
            Debug.Assert(m_ChildNodeList != null);
            Debug.Assert(!(node is HtmlElement) || !(node as HtmlElement).IsEndTag, "If the child is an HtmlElement it must not be an 'End Tag'!");

            OnChildAppendingEventArgs ev = new OnChildAppendingEventArgs();

            if (OnChildAppending != null)
                OnChildAppending(node, ref ev);

            if (!ev.Cancel)
            {
                Debug.WriteLine("Adding a (" + node.Name + ") tag as a child of a (" + this.Name + ") tag.");

                node.ParentNode.ChildNodes.m_List.Remove(node);

                m_XmlNode.InsertBefore(node.m_XmlNode, refChild == null ? null : refChild.m_XmlNode);

                if (refChild == null)
                {
                    m_ChildNodeList.m_List.Add(node);
                }
                else
                {
                    int refIdx = m_ChildNodeList.m_List.IndexOf(refChild);
                    Debug.Assert(refIdx != -1);
                    m_ChildNodeList.m_List.Insert(refIdx, node);
                }


                if (OnChildAppended != null)
                    OnChildAppended(node);

                Debug.Assert(m_XmlNode == node.m_XmlNode.ParentNode);
                Debug.Assert(m_XmlNode.ParentNode != null || (this is HtmlDocument));

                return node;
            }
            else
                return null;
        }

        /// <summary>
        /// Gets an HtmlAttributeCollection containing the attributes of this node.
        /// </summary>
        public HtmlAttributeCollection Attributes
        {
            get
            {
                return m_Attributes;
            }
        }

        /// <summary>
        /// Returns the base URI of the current node.
        /// </summary>
        public string BaseURI
        {
            get
            {
                Debug.Assert(m_XmlNode != null);
                return m_XmlNode.BaseURI;
            }
        }

        /// <summary>
        /// Gets all the child nodes of the current node.
        /// </summary>
        public HtmlNodeList ChildNodes
        {
            get
            {
                return m_ChildNodeList;
            }
        }

        IEnumerator<HtmlNode> IEnumerable<HtmlNode>.GetEnumerator()
        {
            Debug.Assert(m_ChildNodeList != null);
            return ((IEnumerable<HtmlNode>)m_ChildNodeList).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that itterates through the children colletion of the current HtmlNode.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            Debug.Assert(m_ChildNodeList != null);
            return ((IEnumerable)m_ChildNodeList).GetEnumerator();
        }

        /// <summary>
        /// Gets the first child of the node.
        /// </summary>
        public HtmlNode FirstChild
        {
            get
            {
                Debug.Assert(m_XmlNode != null);
                XmlNode node = m_XmlNode.FirstChild;

                if (node == null)
                    return null;
                else
                {
                    Debug.Assert(node is IHtmlNodeReferenceHolder);
                    return (node as IHtmlNodeReferenceHolder).HtmlNodeReference;
                }
            }
        }

        /// <summary>
        /// Looks up the closest xmlns declaration for the given prefix that is in scope for the current node and returns the namespace URI in the declaration.<br/><br/>
        /// Standard HTML nodes will always have the default namespace to simplify XPath queries even if an xmlns is explicitely specified in XHTML documents. <br/>
        /// For all non-standard HTML 4.01 elements that define an xmlns, <b>NamespaceURI</b> will return the namespace URI.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public string GetNamespaceOfPrefix(string prefix)
        {
            Debug.Assert(m_XmlNode != null);
            return m_XmlNode.GetNamespaceOfPrefix(prefix);
        }

        /// <summary>
        /// Looks up the closest xmlns declaration for the given namespace URI that is in scope for the current node and returns the prefix defined in the declaration.<br/><br/>
        /// Standard HTML nodes will always have the default namespace to simplify XPath queries even if an xmlns is explicitely specified in XHTML documents. <br/>
        /// For all non-standard HTML 4.01 elements that define an xmlns, <b>NamespaceURI</b> will return the namespace URI.
        /// </summary>
        /// <param name="namespaceUri"></param>
        /// <returns></returns>
        public string GetPrefixOfNamespace(string namespaceUri)
        {
            Debug.Assert(m_XmlNode != null);
            return m_XmlNode.GetPrefixOfNamespace(namespaceUri);
        }

        /// <summary>
        /// Creates an System.Xml.XPath.XPathNavigator for navigating this object
        /// </summary>
        /// <returns></returns>
        public XPathNavigator CreateNavigator()
        {
            Debug.Assert(m_XmlNode != null);
            return m_XmlNode.CreateNavigator();
        }

        /// <summary>
        /// Gets a value indicating whether this node has any child nodes.
        /// </summary>
        public bool HasChildNodes
        {
            get
            {
                Debug.Assert(m_XmlNode != null);
                return m_XmlNode.HasChildNodes;
            }
        }

        public virtual string InnerXml
        {
            get
            {
                Debug.Assert(m_XmlNode != null);
                return m_XmlNode.InnerXml;
            }
        }

        /// <summary>
        /// Gets the concatinated values of the node and all its child nodes.
        /// </summary>
        public virtual string InnerText
        {
            get
            {
                Debug.Assert(m_XmlNode != null);
                return m_XmlNode.InnerText;
            }
        }

        /// <summary>
        /// Gets the markup representing the node and all its child nodes.
        /// </summary>
        public virtual string OuterXml
        {
            get
            {
                Debug.Assert(m_XmlNode != null);
                return m_XmlNode.OuterXml;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current is read only. Returns <b>false</b> if the node can be modified.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return NodePosition.ReadOnly.Equals(m_NodePosition);
            }
        }

        internal bool IsInserted
        {
            get
            {
                return NodePosition.Inserted.Equals(m_NodePosition);
            }
        }

        /// <summary>
        /// Gets the last child of the current node.
        /// </summary>
        public HtmlNode LastChild
        {
            get
            {
                Debug.Assert(m_XmlNode != null);
                XmlNode node = m_XmlNode.LastChild;

                if (node == null)
                    return null;
                else
                {
                    Debug.Assert(node is IHtmlNodeReferenceHolder);
                    return (node as IHtmlNodeReferenceHolder).HtmlNodeReference;
                }
            }
        }

        /// <summary>
        /// Gets the local name of the node.
        /// </summary>
        public string LocalName
        {
            get
            {
                Debug.Assert(m_XmlNode != null);
                return m_XmlNode.LocalName;
            }
        }

        private string m_UpperName = null;
        internal string UpperName
        {
            get
            {
                Debug.Assert(m_XmlNode != null);
                if (m_UpperName == null)
                    m_UpperName = m_XmlNode.LocalName != null ? m_XmlNode.LocalName.ToUpper() : string.Empty;

                return m_UpperName;
            }
        }


        /// <summary>
        /// Gets the qualified name of the node.
        /// </summary>
        public string Name
        {
            get
            {
                Debug.Assert(m_XmlNode != null);

                //if (m_XmlNode is IHtmlNodeReferenceHolder)
                //    return (m_XmlNode as IHtmlNodeReferenceHolder).CaseSensitiveName;
                //else
                    return m_XmlNode.Name;
            }
        }

        /// <summary>
        /// Gets the namespace URI of the node. <br/><br/>
        /// Standard HTML nodes will always have the default namespace to simplify XPath queries even if an xmlns is explicitely specified in XHTML documents. <br/>
        /// For all non-standard HTML 4.01 elements that define an xmlns, <b>NamespaceURI</b> will return the namespace URI.
        /// </summary>
        public string NamespaceURI
        {
            get
            {
                Debug.Assert(m_XmlNode != null);
                return m_XmlNode.NamespaceURI;
            }
        }


        /// <summary>
        /// Gets the node immediately following this node.
        /// </summary>
        public HtmlNode NextSibling
        {
            get
            {
                Debug.Assert(m_XmlNode != null);
                XmlNode node = m_XmlNode.NextSibling;

                if (node == null)
                    return null;
                else
                {
                    Debug.Assert(node is IHtmlNodeReferenceHolder);
                    return (node as IHtmlNodeReferenceHolder).HtmlNodeReference;
                }
            }
        }

        /// <summary>
        /// Gets the type of the current node.
        /// </summary>
        public XmlNodeType NodeType
        {
            get
            {
                Debug.Assert(m_XmlNode != null);
                return m_XmlNode.NodeType;
            }
        }

        /// <summary>
        /// Gets the Acrux.HtmlDocument to which this node belongs.
        /// </summary>
        public HtmlDocument OwnerDocument
        {
            get
            {
                Debug.Assert(m_Doc != null);
                return m_Doc;
            }
        }

        internal HtmlNode m_CachedParent = null;


        /// <summary>
        /// Gets the parent of this node (for nodes that can have parents)
        /// </summary>
        public HtmlNode ParentNode
        {
            get
            {
                if (m_CachedParent == null)
                {
                    Debug.Assert(m_XmlNode != null);
                    XmlNode node = m_XmlNode.ParentNode;

                    Debug.Assert(node is IHtmlNodeReferenceHolder || node is System.Xml.XmlDocument || node == null /* When no document is loaded */);

                    IHtmlNodeReferenceHolder nodeRef = node as IHtmlNodeReferenceHolder;
                    if (nodeRef != null)
                        return nodeRef.HtmlNodeReference;
                    else
                    {
                        if (node is System.Xml.XmlDocument)
                            return m_Doc;
                        else
                            if (m_Attributes != null &&
                                m_Attributes.m_InternalCollection != null &&
                                m_Attributes.m_InternalCollection.ParentNode != null)
                                return m_Attributes.m_InternalCollection.ParentNode;
                            else
                                throw new InvalidOperationException("No parent has been set.");
                    }
                }

                return m_CachedParent;
            }
        }

        /// <summary>
        /// Gets the namespace prefix for this node.<br/><br/>
        /// Standard HTML nodes will always have the default namespace to simplify XPath queries even if an xmlns is explicitely specified in XHTML documents. <br/>
        /// For all non-standard HTML 4.01 elements that define an xmlns, <b>NamespaceURI</b> will return the namespace URI.
        /// </summary>
        public string Prefix
        {
            get
            {
                Debug.Assert(m_XmlNode != null);
                return m_XmlNode.Prefix;
            }
        }

        /// <summary>
        /// Gets the node immediately preceding this node.
        /// </summary>
        public HtmlNode PreviousSibling
        {
            get
            {
                Debug.Assert(m_XmlNode != null);
                XmlNode node = m_XmlNode.PreviousSibling;

                if (node == null)
                    return null;
                else
                {
                    Debug.Assert(node is IHtmlNodeReferenceHolder);
                    return (node as IHtmlNodeReferenceHolder).HtmlNodeReference;
                }
            }
        }

        //internal void RemoveAll()
        //{
        //    // TODO: After the library is completed, check if this routine is used internally at all, and remove it if its not used
        //    throw new NotImplementedException();
        //}

        //internal HtmlNode RemoveChild(HtmlNode oldChild)
        //{
        //    // TODO: After the library is completed, check if this routine is used internally at all, and remove it if its not used
        //    throw new NotImplementedException();
        //}

        //internal HtmlNode ReplaceChild(HtmlNode newChild, HtmlNode oldChild)
        //{
        //    // TODO: After the library is completed, check if this routine is used internally at all, and remove it if its not used
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Evaluates the XPath expression and retuns the typed result
        /// </summary>
        /// <param name="xpath">The XPath expression to be evaluated</param>
        /// <returns></returns>
        public object EvaluateXPath(string xpath)
        {
            return EvaluateXPath(xpath, null);
        }

        internal object EvaluateXPath(string xpath, System.Xml.Xsl.XsltContext additionalContext)
        {
            Debug.Assert(m_XmlNode != null);

            Debug.Assert(xpath != null);
            if (xpath == null)
                throw new ArgumentNullException("xpath");

            //TODO: [MOVE AS A DEVELOPMENT TASK] - Implement XPath Expression Parser
            //      The XPATH is mixed case because if not some XPath functions will not work
            //      May be we need an XPathExpression parser, which will only change the 
            //      case of the elements and attribute names !!! Will be very cool.
            XPathNavigator nav = m_XmlNode.CreateNavigator();
            XPathExpression expr = nav.Compile(xpath);
            expr.SetContext(XPath.HtmlXsltContext.GetInstance(additionalContext));

            try
            {
                object result = nav.Evaluate(expr);
                return result;
            }
            catch (XPathException ex)
            {
                throw new XPathException(string.Format(CultureInfo.InvariantCulture, "Error evaluating X-Path expression: {0}", xpath), ex);
            }
        }

        /// <summary>
        /// Selects the list of nodes matching the XPath expression. 
        /// </summary>
        /// <param name="xpath">The XPath expression.</param>
        /// <returns>An Acrux.Html.HtmlNodeList containing a collection of nodes matching the XPath
        ///     query. The HtmlNodeList should not be expected to be connected "live" to the
        ///     HTML document. That is, changes that appear in the HTML document may not appear
        ///     in the HtmlNodeList, and vice versa.</returns>
        public HtmlNodeList SelectNodes(string xpath)
        {
            return SelectNodes(xpath, null);
        }

        /// <summary>
        /// Selects the list of nodes matching the XPath expression. External user-defined functions will be resolved using the supplied System.Xml.Xsl.XsltContext.
        /// </summary>
        /// <param name="xpath">The XPath expression.</param>
        /// <param name="additionalContext">An System.Xml.Xsl.XsltContext to use for resolving any user-defined functions 
        ///     used in the XPath expression.</param>
        /// <exception cref="System.Xml.XPath.XPathException"></exception>
        /// <exception cref="Acrux.Html.HtmlParserException"></exception>
        /// <returns>An Acrux.Html.HtmlNodeList containing a collection of nodes matching the XPath
        ///     query. The HtmlNodeList should not be expected to be connected "live" to the
        ///     HTML document. That is, changes that appear in the HTML document may not appear
        ///     in the HtmlNodeList, and vice versa.</returns>
        internal HtmlNodeList SelectNodes(string xpath, System.Xml.Xsl.XsltContext additionalContext)
        {
            Debug.Assert(m_XmlNode != null);

            Debug.Assert(xpath != null);
            if (xpath == null)
                throw new ArgumentNullException("xpath");

            //TODO: The XPATH is mixes case because if not some XPath functions will not work
            //      May be we need an XPathExpression parser, which will only change the 
            //      case of the elements and attribute names !!! Will be very cool.

            try
            {
                XmlNodeList result = m_XmlNode.SelectNodes(xpath, XPath.HtmlXsltContext.GetInstance(additionalContext));

                return new HtmlNodeList(result);
            }
            catch (Exception ex)
            {
                throw new XPathException(string.Format(CultureInfo.InvariantCulture, "Error selecting nodes. X-Path expression: {0}", xpath), ex);
            }
        }

        /// <summary>
        /// Selects the first HtmlNode that matches the the XPath expression. 
        /// </summary>
        /// <param name="xpath">The XPath expression.</param>
        /// <exception cref="System.Xml.XPath.XPathException"></exception>
        /// <returns>The first HtmlNode that matches the XPath query or null if no matching node
        ///     is found. The HtmlNode should not be expected to be connected "live" to the
        ///     XML document. That is, changes that appear in the XML document may not appear
        ///     in the HtmlNode, and vice versa.</returns>
        public HtmlNode SelectSingleNode(string xpath)
        {
            return SelectSingleNode(xpath, null);
        }

        internal HtmlNode SelectSingleNode(string xpath, System.Xml.Xsl.XsltContext additionalContext)
        {
            Debug.Assert(m_XmlNode != null);

            Debug.Assert(xpath != null);
            if (xpath == null)
                throw new ArgumentNullException("xpath");

            //TODO: The XPATH is mixes case because if not some XPath functions will not work
            //      May be we need an XPathExpression parser, which will only change the 
            //      case of the elements and attribute names !!! Will be very cool.
            XmlNode result = m_XmlNode.SelectSingleNode(xpath, XPath.HtmlXsltContext.GetInstance(additionalContext));

            if (result == null || (!(result is XmlRefElement) && !(result is XmlRefAttribute)))
                return null;
            else
            {
                IHtmlNodeReferenceHolder resRef = result as IHtmlNodeReferenceHolder;
                if (resRef != null)
                    return resRef.HtmlNodeReference;

                Debug.Assert(false);
                throw new XPathException("The type returned by an XPath operation was unexpected!");
            }
        }

        /// <summary>
        /// Gets or sets the value of the node.
        /// </summary>
        /// <exception cref="System.ArgumentException" />
        /// <exception cref="System.InvalidOperationException" />
        public string Value
        {
            get
            {
                Debug.Assert(m_XmlNode != null);

                return m_XmlNode.Value;
            }
            set
            {
                Debug.Assert(this.ParentNode != null);
                Debug.Assert(this.ParentNode is HtmlElement);

                HtmlElement parent = this.ParentNode as HtmlElement;

                if (parent != null)
                    parent.Attributes.m_InternalCollection.SetAttributeValue(this.Name, value);
                else
                    throw new InvalidOperationException("The node is invalid or corrupted.");
            }
        }


        // TODO:
        //public int StartsAt
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public int EndsAt
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public bool IsWellFormed
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}


        //public HtmlNode CloneNode(bool deep)
        //{
        //    // TODO: Implement cloning

        //    throw new NotImplementedException();
        //}

        //public HtmlNode CloneNode()
        //{
        //    // TODO: Implement cloning
        //    throw new NotImplementedException();
        //}


        /// <summary>
        /// Gets an XPath expression that uniquely identifies the current node. For repeated elements an index will be used.<br/><br/>
        /// Example:<i>/html/body/table/tr[5]/td[2]/img</i>
        /// </summary>
        public virtual string XPathLocation
        {
            get
            {
                try
                {
                    Debug.Assert(ParentNode != null);

                    string STR_FMT = "{0}/{1}";

                    if (!(this is HtmlAttribute))
                    {
                        int thisNodePosition = 0;
                        foreach (HtmlNode child in ParentNode.ChildNodes)
                        {
                            if (HtmlParser.AreSameTypeElements(child, this))
                                thisNodePosition++;

                            if (child.Equals(this))
                                break;
                        }

                        if (thisNodePosition > 1)
                            STR_FMT = string.Format(CultureInfo.InvariantCulture, "{{0}}/{{1}}[{0}]", thisNodePosition);
                    }
                    else
                        STR_FMT = "{0}/@{1}";


                    return string.Format(CultureInfo.InvariantCulture, STR_FMT, ParentNode.XPathLocation, m_XmlNode.Name);
                }
                catch (Exception ex)
                {
                    return ex.GetType() + " : " + ex.Message;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified Acrux.Html object is equal to the current Acrux.Html object.
        /// </summary>
        /// <param name="obj">The Acrux.Html object to compare with the current object.</param>
        /// <returns>true if obj is a System.String and its value is the same as this instance;<br/>otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            HtmlNode objNode = obj as HtmlNode;

            if (objNode == null)
                return false;

            return objNode.m_Id == this.m_Id;            
        }

        /// <summary>
        /// Determines whether two specified Acrux.HtmlNode objects have the same value.
        /// </summary>
        /// <param name="a">An Acrux.HtmlNode or null.</param>
        /// <param name="b">An Acrux.HtmlNode or null.</param>
        /// <returns>true if the value of a is the same as the value of b; otherwise, false.</returns>
        public static bool operator ==(HtmlNode a, HtmlNode b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether two specified System.String objects have different values.
        /// </summary>
        /// <param name="a">An Acrux.HtmlNode or null.</param>
        /// <param name="b">An Acrux.HtmlNode or null.</param>
        /// <returns>true if the value of a is different from the value of b; otherwise, false.</returns>
        public static bool operator !=(HtmlNode a, HtmlNode b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Serves as a hash function for a particualr type
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.m_Id;
        }
    }

}
