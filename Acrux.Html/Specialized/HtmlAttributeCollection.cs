using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Globalization;

namespace Acrux.Html.Specialized
{
    public sealed class HtmlAttributeCollection : IEnumerable<HtmlAttribute>
    {
        internal HtmlAttributeCollectionInternal m_InternalCollection;

        internal HtmlAttributeCollection(HtmlNode parent)
        {
            m_InternalCollection = new HtmlAttributeCollectionInternal(parent);
        }

        IEnumerator<HtmlAttribute> IEnumerable<HtmlAttribute>.GetEnumerator()
        {
            return ((IEnumerable<HtmlAttribute>)m_InternalCollection).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)m_InternalCollection).GetEnumerator();
        }

        /// <summary>
        /// Gets the HtmlAttribute with the given name.
        /// </summary>
        /// <param name="attributeName">The lower case attribute name to get</param>
        /// <returns></returns>
        public HtmlAttribute this[string attributeName]
        {
            get
            {
                if (m_InternalCollection == null)
                    return null;
                else
                    return m_InternalCollection[attributeName];
            }
        }

        internal class HtmlAttributeCollectionInternal : ICollection<HtmlAttribute>, IEnumerable<HtmlAttribute>
        {
            // Hides the Collection for internal use only. I.e. external client will not be able to add attributes

            private List<HtmlAttribute> m_HtmlAttList = new List<HtmlAttribute>();

            //private XmlAttributeCollection m_Attributes = null;
            internal readonly HtmlNode ParentNode = null;

            internal HtmlAttributeCollectionInternal(HtmlNode parent)
            {
                Debug.Assert(parent != null);
                Debug.Assert(parent.m_XmlNode != null);

                ParentNode = parent as HtmlNode;
                // TODO: Why error ??? Investigate!
                //if (ParentNode == null ||
                //    ParentNode is HtmlAttribute)
                //    Trace.WriteLine("ERROR " + ParentNode == null ? "NULL"  : ParentNode.GetType().ToString());
            }

            internal HtmlAttribute this[string attributeName]
            {
                get
                {
                    if (ParentNode == null)
                        throw new InvalidOperationException("No parent has been set.");

                    if (ParentNode.m_XmlNode.Attributes == null)
                        return null;

                    // First try the same case
                    XmlAttribute att = ParentNode.m_XmlNode.Attributes[attributeName];
                    if (att == null)
                    {
                        foreach (XmlAttribute attTest in ParentNode.m_XmlNode.Attributes)
                        {
                            if (attTest.Name.Equals(attributeName, StringComparison.InvariantCultureIgnoreCase))
                            {
                                att = attTest;
                                break;
                            }
                        }
                    }

                    IHtmlNodeReferenceHolder attNodeRef = att as IHtmlNodeReferenceHolder;
                    if (att != null)
                    {
                        HtmlNode attNode = attNodeRef.HtmlNodeReference;

                        Debug.Assert(attNode is HtmlAttribute);
                        return attNode as HtmlAttribute;
                    }
                    else
                        return null;
                }
            }


            void ICollection<HtmlAttribute>.Add(HtmlAttribute node)
            {
                Add(node);
            }

            internal void Add(HtmlAttribute node)
            {
                if (ParentNode == null)
                    throw new InvalidOperationException("No parent has been set.");

                Debug.Assert(node != null);
                Debug.Assert(node.m_XmlNode is XmlAttribute);
                Debug.Assert(!(ParentNode is HtmlAttribute));

                m_HtmlAttList.Add(node);

                // TODO: HACK: This is a workaround for the correct parent to be picked up.
                //             Review this code and findout why the wrong parent is picked !!!
                node.m_CachedParent = ParentNode;
                
                ParentNode.m_XmlNode.Attributes.Append((XmlAttribute)node.m_XmlNode);
            }

            internal bool RemoveAttribute(string attName)
            {
                XmlAttribute xmlAtt = ParentNode.m_XmlNode.Attributes[attName];
                IHtmlNodeReferenceHolder refHolder = xmlAtt as IHtmlNodeReferenceHolder;
                if (xmlAtt != null &&
                    refHolder != null)
                {
                    ParentNode.m_XmlNode.Attributes.Remove(xmlAtt);
                    m_HtmlAttList.Remove((HtmlAttribute)refHolder.HtmlNodeReference);

                    ParentNode.OwnerDocument.m_FixupManager.AddOrUpdateFixUp(FixupType.Remove, refHolder.HtmlNodeReference, xmlAtt.Value);

                    return true;
                }

                return false;
            }

            internal HtmlAttribute SetAttributeValue(string attributeName, string value)
            {
                if (ParentNode.OwnerDocument == null)
                    throw new InvalidOperationException("This node cannot be edited.");

                HtmlAttribute att = this[attributeName];

                if (att != null)
                {
                    if (att.IsReadOnly)
                        throw new InvalidOperationException(string.Format("The attribute '{0}' is readonly and cannot be edited.", attributeName));

                    HtmlNodeFixup fixup = ParentNode.OwnerDocument.m_FixupManager.AddOrUpdateFixUp(att.IsInserted ? FixupType.Add : FixupType.UpdateAttribute, att, att.m_XmlNode.Value);

                    if (value.IndexOf("'") > -1 &&
                        fixup.ValueEnclosingCharacter != '"')
                    {
                        fixup.ValueEnclosingCharacter = '"';
                    }

                    // This will automatically escape double quotes to &quot;
                    att.m_XmlNode.Value = value;
                }

                return att;
            }

            internal HtmlAttribute AddAttribute(string name, string value)
            {
                string[] prefAndName = name.Split(new char[] { ':' }, 2);
                string prefix = prefAndName.Length == 2 ? prefAndName[0] : null;
                string localName = prefAndName.Length == 2 ? prefAndName[1] : name;
                string nameSpaceUri = null;

                HtmlParser.ResolveNameSpace(ref prefix, ref localName, ref nameSpaceUri, name, null, ParentNode);

                if (!string.IsNullOrEmpty(localName))
                {
                    HtmlAttribute newAttribute = new HtmlAttribute(prefix, localName, nameSpaceUri, ParentNode.OwnerDocument, NodePosition.Inserted, value, false);
                    ParentNode.Attributes.m_InternalCollection.Add(newAttribute);

                    ParentNode.OwnerDocument.m_FixupManager.AddOrUpdateFixUp(FixupType.Add, newAttribute, value);

                    return newAttribute;
                }

                return null;
            }

            void ICollection<HtmlAttribute>.Clear()
            {
                if (ParentNode == null)
                    throw new InvalidOperationException("No parent has been set.");

                m_HtmlAttList.Clear();
                ParentNode.m_XmlNode.Attributes.RemoveAll();
            }

            bool ICollection<HtmlAttribute>.Contains(HtmlAttribute node)
            {
                foreach (HtmlAttribute htmlAtt in m_HtmlAttList)
                {
                    if (htmlAtt == node)
                        return true;
                }

                return false;
            }

            void ICollection<HtmlAttribute>.CopyTo(HtmlAttribute[] nodes, int index)
            {
                Debug.Assert(nodes != null);

                Debug.Assert(false);
                throw new NotImplementedException();

                // TODO: Uncomment this when CloneNode() is implemented
                //int from = 0;
                //int to = m_HtmlAttList.Count;
                //while (from < to)
                //{
                //    nodes[index] = (HtmlAttribute)((HtmlNode)m_HtmlAttList[from]).CloneNode(true);
                //    from++;
                //    index++;
                //}
            }

            bool ICollection<HtmlAttribute>.Remove(HtmlAttribute node)
            {
                if (ParentNode == null)
                    throw new InvalidOperationException("No parent has been set.");

                Debug.Assert(node != null);
                Debug.Assert(node.m_XmlNode is XmlAttribute);

                XmlAttribute removedAtt = ParentNode.m_XmlNode.Attributes.Remove((XmlAttribute)node.m_XmlNode);
                bool removed = m_HtmlAttList.Remove(node);

                Debug.Assert(removed == (removedAtt != null));

                return removed;
            }

            int ICollection<HtmlAttribute>.Count
            {
                get
                {
                    if (ParentNode == null)
                        throw new InvalidOperationException("No parent has been set.");

                    Debug.Assert(m_HtmlAttList.Count == ParentNode.m_XmlNode.Attributes.Count /* m_Attributes.Count */ );

                    return m_HtmlAttList.Count;
                }
            }

            bool ICollection<HtmlAttribute>.IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            IEnumerator<HtmlAttribute> IEnumerable<HtmlAttribute>.GetEnumerator()
            {
                return m_HtmlAttList.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return m_HtmlAttList.GetEnumerator();
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the Acrux.Html.HtmlAttributeCollection.
        /// </summary>
        public long Count
        {
            get
            {
                return (m_InternalCollection as ICollection<HtmlAttribute>).Count;
            }
        }

        public HtmlAttribute Add(string name, string value)
        {
            if (m_InternalCollection[name] != null) 
                throw new ArgumentException(string.Format("Attribute with name '' already exists.", name));

            return m_InternalCollection.AddAttribute(name, value);
        }

        /// <summary>
        /// Removes the specified attribute if exists.
        /// </summary>
        /// <param name="attributeName">The attribute to be removed</param>
        /// <returns><b>true</b> if the attribute has been removed.</returns>
        public bool Remove(string attributeName)
        {
            if (attributeName == null)
                throw new ArgumentNullException("attributeName");

            return m_InternalCollection.RemoveAttribute(attributeName);
        }
    }
}
