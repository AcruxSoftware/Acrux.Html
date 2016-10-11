using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Security.Permissions;
using System.Diagnostics;
using Acrux.Html.Specialized;
using System.Globalization;
using System.Web;
using System.Net;
using System.Threading;


namespace Acrux.Html
{
    public enum HtmlFixupSettings
    {
        Acrux,
        Firefox
    }

    /// <summary>
    /// Represents the event data for an Acrux.Html.HtmlDocument.OnParserWarning event.
    /// </summary>
    public class ParserWarningEventArgs : EventArgs
    {
        private string m_Message;
        private int m_Line;
        private int m_Pos;

        internal ParserWarningEventArgs(string message, int line, int pos)
        {
            m_Message = message;
            m_Line = line;
            m_Pos = pos;
        }

        /// <summary>
        /// The warning message.
        /// </summary>
        public string Message
        {
            get { return m_Message; }
        }

        /// <summary>
        /// The line number of the location where the warning was raised.
        /// </summary>
        public int Line
        {
            get { return m_Line; }
        }

        /// <summary>
        /// The position (column number) of the location where the warning was raised.
        /// </summary>
        public int Pos
        {
            get { return m_Pos; }
        }
    }

    /// <summary>
    /// Represents an HTML document.
    /// </summary>
    public sealed class HtmlDocument : HtmlNode 
    {
        internal struct WarningStruct
        {
            internal readonly string Message;
            internal readonly int Line;
            internal readonly int Position;

            internal WarningStruct(string message, int line, int position)
            {
                this.Message = message;
                this.Line = line;
                this.Position = position;
            }
        }

        /// <summary>
        /// Represents the DTD defined in the DOCTYPE declaration of the HTML document. If not specified HtmlDtd.Html401 will be used.
        /// </summary>
        public enum HtmlDtd
        {
            /// <summary>
            /// DTD HTML 4.01 (Strict)
            /// </summary>
            Html401,

            /// <summary>
            /// DTD XHTML 1.0 Transitional
            /// </summary>
            Html401Transitional,

            /// <summary>
            /// DTD XHTML 1.0 Frameset
            /// </summary>
            Html401Frameset,


            /// <summary>
            /// DTD XHTML 1.0 Strict
            /// </summary>
            XhtmlStrict,


            /// <summary>
            /// DTD HTML 4.01 Transitional
            /// </summary>
            XhtmlTransitional,

            /// <summary>
            /// DTD HTML 4.01 Frameset
            /// </summary>
            XhtmlFrameset,

            /// <summary>
            /// DTD HTML 3.2 Final
            /// </summary>
            Html32
        }

        // Default value is "null"
        internal EventHandler<ParserWarningEventArgs> m_OnParserWarning;

        /// <summary>
        /// The events is raised when the parser encounters a problem typically due to not well formed or ambigious HTML.
        /// </summary>
        public event EventHandler<ParserWarningEventArgs> OnParserWarning
        {
            add { m_OnParserWarning += value; }
            remove { m_OnParserWarning -= value; }
        }

        internal XmlDocument m_XmlDoc = new XmlDocument();
        private HtmlParser m_Parser = null;

        private Encoding m_Encoding = Encoding.ASCII;

        private HtmlDocTypeElement m_DocTypeElement = null;
        internal HtmlElement m_BodyElement = null;
        internal HtmlElement m_HeadElement = null;
        internal HtmlElement m_HtmlElement = null;
        private HtmlDtd m_HtmlDtd = HtmlDtd.Html401;

        private object m_SyncLock = new object();

        private static Dictionary<string, Type> s_TagMappersAndStructureProviders = new Dictionary<string, Type>();

        internal FixupManager m_FixupManager = new FixupManager();
        private HtmlFixupSettings m_FixupMode = HtmlFixupSettings.Acrux;

        /// <summary>
        /// Creates a new instance of an HTML document that enforces the Acrux algorithm for fixing up malformed HTML 
        /// </summary>
        public HtmlDocument()
            : base(XmlNodeType.Document, NodePosition.ReadOnly)
        {
            base.OnChildAppending += new OnChildAppendingDelegate(ChildAppending);
        }

        /// <summary>
        /// Creates a new instance of an HTML document that enforces the specified algorithm for fixing up malformed HTML.
        /// </summary>
        /// <param name="fixupAlgorithm">The fixup algorithm to be used by the HTML parser</param>
        public HtmlDocument(HtmlFixupSettings fixupAlgorithm)
            : this()
        {
            m_FixupMode = fixupAlgorithm;
        }

        /// <summary>
        /// Loads an HTML document from a local file.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">The fileName parameter is null.</exception>
        /// <exception cref="System.NotSupportedException">The file is bigger than 2,147,483,647 bytes.</exception>
        /// <exception cref="Acrux.Html.HtmlParserException">An error is encountered during the parsing operation.</exception>
        /// <param name="fileName">The path to the local file to be loaded.</param>
        public void Load(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            fileName = Path.GetFullPath(fileName);  

            Load(new Uri(fileName));
        }

        /// <summary>
        /// Loads an HTML document from a stream. 
        /// </summary>
        /// <exception cref="System.ArgumentNullException">The inputStream is null or does not support Seek operations.</exception>
        /// <exception cref="System.NotSupportedException">The file is bigger than 2,147,483,647 bytes.</exception>
        /// <exception cref="Acrux.Html.HtmlParserException">An error is encountered during the parsing operation.</exception>
        /// <param name="inputStream">The stream to load the HTML document from. It must support Seek operations and will be positioned at the beginning before 
        /// processing. If there is a byte order mark at the beginning of the stream it will be used to determine the encoding. If not - UTF8 will be used as default.</param>
        public void Load(Stream inputStream)
        {
            if (inputStream == null)
                throw new ArgumentNullException("inputStream");

            if (!inputStream.CanSeek)
                throw new ArgumentException("inputStream must support Seek operations.");

            if (inputStream.Length > Int32.MaxValue)
                throw new NotSupportedException("The file stream is too big.");

            // Move to the beginning of the stream again
            inputStream.Seek(0, SeekOrigin.Begin);

            // NOTE: Don't dispose the StreamReader as it will dispose the Stream
            //       The stream should be disposed by the called that passes it
            StreamReader rdr = new StreamReader(inputStream);

            m_RawHtml = rdr.ReadToEnd();
            m_Encoding = rdr.CurrentEncoding;

            LoadRaw();

            Encoding htmlEnc;
            if (NeedToReparse(out htmlEnc))
            {
                inputStream.Seek(0, SeekOrigin.Begin);
                rdr = new StreamReader(inputStream, htmlEnc);
                m_RawHtml = rdr.ReadToEnd();
                m_Encoding = htmlEnc;

                LoadRaw();
            }

            #region OLD CODE TO DETERMINE ENCODING
            //// Determine the encoding from the first few bytes
            //// ----------------------------------------------------------------------------
            //// Check for a Byte Order Mask                              |                 |
            //// ----------------------------------------------------------------------------
            //// 00 00 FE FF  UCS-4, big-endian machine (1234 order)      |  NOT SUPPORTED  |
            //// FF FE 00 00  UCS-4, little-endian machine (4321 order)   |  NOT SUPPORTED  |
            //// 00 00 FF FE  UCS-4, unusual octet order (2143)           |  NOT SUPPORTED  |
            //// FE FF 00 00  UCS-4, unusual octet order (3412)           |  NOT SUPPORTED  |
            //// FE FF ## ##  UTF-16, big-endian                          |                 |
            //// FF FE ## ##  UTF-16, little-endian                       |                 |
            //// EF BB BF     UTF-8                                       |                 |
            //// ----------------------------------------------------------------------------

            //if (inputStream.Length > 4)
            //{
            //    byte[] buff = new byte[4];
            //    inputStream.Read(buff, 0, 4);

            //    // Move to the beginning of the stream again
            //    inputStream.Seek(0, SeekOrigin.Begin);

            //    if (!DetermineEncoding(buff))
            //        throw new HtmlParserException("Encoding is not supported or could not be determined.");

            //    byte[] byteContent = new byte[inputStream.Length];
            //    Debug.Assert(inputStream.Length < Int32.MaxValue);
            //    if (inputStream.Length >= Int32.MaxValue ||
            //        inputStream.Length < 0)
            //    {
            //        throw new NotSupportedException("The file is too big.");
            //    }

            //    inputStream.Read(byteContent, 0, (int)inputStream.Length);
            //    char[] charData = m_Encoding.GetChars(byteContent);
                
            //    // If we have a byte order mask this should be the first char
            //    //Debug.Assert(!m_HasByteOrderMask || charData[0] == '\xFEFF');

            //    LoadRaw(charData);
            //}
            #endregion
        }

        /// <summary>
        /// Loads an HTML document using HttpWebRequest or opening the file directly for a 'file' schema.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">The documentUrl parameter is null.</exception>
        /// <exception cref="System.NotSupportedException">The Content-Type of the file is not 'text/html' or the file is too big.</exception>
        /// <exception cref="Acrux.Html.HtmlParserException">An error is encountered during the parsing operation.</exception>
        /// <param name="documentUrl">The Url of the HTML document to be loaded.</param>
        public void Load(Uri documentUrl)
        {
            CookieCollection cookies;
            Load(documentUrl, out cookies);
        }


        /// <summary>
        /// Loads an HTML document using HttpWebRequest or opening the file directly for a 'file' scheme. In  the case of HTTP/HTTPS scheme the cookies are returned as an output parameter.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">The documentUrl parameter is null.</exception>
        /// <exception cref="System.NotSupportedException">The Content-Type of the file is not 'text/html' or the file is too big.</exception>
        /// <exception cref="Acrux.Html.HtmlParserException">An error is encountered during the parsing operation.</exception>
        /// <param name="documentUrl">The Url of the HTML document to be loaded.</param>
        /// <param name="cookies">The cookies loaded from the HttpWebResponse. This parameter will be set to <b>null</b> the the Uri points to a local file.</param>
        public void Load(Uri documentUrl, out CookieCollection cookies)
        {
            cookies = new CookieCollection();
            WebHeaderCollection headers;
            Load(documentUrl, ref cookies, out headers);
        }


        public void Load(Uri documentUrl, ref CookieCollection cookies, out WebHeaderCollection headers)
        {
            if (documentUrl == null)
                throw new ArgumentNullException("documentUrl");

            headers = null;

            Debug.Assert(!string.IsNullOrEmpty(documentUrl.Scheme));

            if ("file".Equals(documentUrl.Scheme))
            {
                cookies = null;

                using (FileStream fileContent = new FileStream(documentUrl.LocalPath, FileMode.Open, FileAccess.Read))
                {
                    Load(fileContent);
                }
            }
            else
            {
                WebRequest req = HttpWebRequest.Create(documentUrl);
                (req as HttpWebRequest).CookieContainer = new CookieContainer();

                if (cookies != null)
                    (req as HttpWebRequest).CookieContainer.Add(cookies);

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                #region Getting the charset according to RFC-2616, sections 3.6, 3.7 and 14.17
                string contentType = resp.Headers["Content-Type"];
                string mimeType = contentType;
                Dictionary<string, string> contentParameters = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(contentType))
                {
                    string[] chunks = contentType.Split(';');
                    mimeType = chunks[0].Trim();

                    for (int i = 1; i < chunks.Length; i++)
                    {
                        string param = chunks[i].Trim();
                        string[] nameValuePair = param.Split('=');
                        if (nameValuePair.Length == 2)
                            contentParameters.Add(nameValuePair[0].ToLower(), nameValuePair[1]);
                    }

                    if (!string.IsNullOrEmpty(mimeType) && !mimeType.Equals("text/html", StringComparison.InvariantCultureIgnoreCase))
                    {
                        throw new NotSupportedException(string.Format("Content type {0} is not supported!", resp.Headers["Content-Type"]));
                    }
                }
                string charset = null;
                contentParameters.TryGetValue("charset", out charset);
                #endregion

                cookies = resp.Cookies;
                headers = resp.Headers;

                using (Stream repStream = resp.GetResponseStream())
                {
                    Encoding enc = Encoding.UTF8;
                    if (charset != null)
                    {
                        try
                        {
                            enc = Encoding.GetEncoding(charset);
                        }
                        catch (ArgumentException)
                        { }
                    }

                    // There is a nasty bug in .NET when trying to read from a stream using a buffer when depending on the size of the buffer
                    // and the encoding used the stream could come up partially read because of a 0 byte that may appear in the middle during 
                    // the buffered read. To workaround this we need to read the whole stream at once.
                    StreamReader rdr = new StreamReader(repStream, enc, true);
                    m_Encoding = rdr.CurrentEncoding;
                    m_RawHtml = rdr.ReadToEnd();

                    LoadRaw();

                    Encoding htmlEnc;
                    if (NeedToReparse(out htmlEnc))
                    {
                        // And if we need to reparse the document because the encoding specified in the HTML META tag is different
                        // that the one specified in the HTTP header then we need to get the response again because the ResponseStream()
                        // does not allow Seek operation. Depending on the IE settings the second time the content will most probably come
                        // from the cache on the local machine or a proxy. Again we need to read the whole stream at once.
                        using(Stream repStream2 = ((HttpWebResponse)req.GetResponse()).GetResponseStream())
                        {
                            rdr = new StreamReader(repStream2, htmlEnc, true);
                            m_Encoding = rdr.CurrentEncoding;
                            m_RawHtml = rdr.ReadToEnd(); 
                        }

                        LoadRaw();
                    }
                }
            }
        }

        /// <summary>
        /// Loads the HTML document from the specified string.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">The html parameter is null.</exception>
        /// <exception cref="Acrux.Html.HtmlParserException">An error is encountered during the parsing operation.</exception>
        /// <param name="html">The content to be loaded as HTML document.</param>
        public void LoadHtml(string html)
        {
            if (html == null)
                throw new ArgumentNullException("html");

            m_Encoding = Encoding.UTF8;
            m_RawHtml = html;

            LoadRaw();

            Encoding htmlEnc;
            if (NeedToReparse(out htmlEnc))
            {
                m_Encoding = htmlEnc;

                LoadRaw();
            }
        }

        /// <summary>
        /// The encoding of the HTML document. If the encoding was not determined by the byte-order mark then UTF8 is used.
        /// </summary>
        public Encoding ContentEncoding
        {
            get
            {
                return m_Encoding;
            }
        }

        internal string m_XmlEncoding;

        /// <summary>
        /// For XHTML documents this is the encoding specified in the XML declaration. Otherwise is null.
        /// </summary>
        public string XmlEncoding
        {
            get
            {
                return m_XmlEncoding;
            }
        }

        private string m_RawHtml = null;

        /// <summary>
        /// The raw HTML string used to load the document from. This doesn't include changes made to the document after it has been loaded.<br/>
        /// Use GetOuterHtml() to get the current mark-up including your changes done.
        /// </summary>
        public string RawHtml
        {
            get { return m_RawHtml; }
        }

        /// <summary>
        /// Returns the current HTML mark-up with all changes made.
        /// </summary>
        /// <returns>The current mark-up including the changes.</returns>
        public string GetOuterHtml()
        {
            return m_FixupManager.ApplyFixUps(this);
        }

        private string m_DocumentAsText = null;

        /// <summary>
        /// Returns the current content of the HTML page as a text. The text may not be in the same order as it appears 
        /// in a web browser as the CSS may change text alignment and order. Also dynamically added content to the page using 
        /// iframes, frames, JavaScript or AJAX will not be included.
        /// </summary>
        /// <returns>The current text of the mark-up.</returns>
        public string GetContentAsText()
        {
            if (m_DocumentAsText == null)
            {
                StringBuilder bld = new StringBuilder();
                GetTextRecursive(this.BodyElement, bld);

                m_DocumentAsText = bld.ToString();
            }

            return m_DocumentAsText;
        }

        private void GetTextRecursive(HtmlNode currElement, StringBuilder bld)
        {
            HtmlTextElement currTxt = currElement as HtmlTextElement;

            if (currTxt != null)
            {
                bld.Append(System.Web.HttpUtility.HtmlDecode(currTxt.InnerText));
                bld.Append(" ");
            }

            foreach (HtmlNode node in currElement.ChildNodes)
            {
                if (
                    "SCRIPT".Equals(node.Name, StringComparison.InvariantCultureIgnoreCase) ||
                    "STYLE".Equals(node.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Skill those
                }
                else
                    GetTextRecursive(node, bld);
            }
        }

        private bool NeedToReparse(out Encoding newEncoding)
        {
            newEncoding = m_Encoding;

            foreach (HtmlNode node in this.SelectNodes("/html/head/meta"))
            {
                bool isContentTypeMetaTag = false;

                if (node.Attributes["http-equiv"] != null &&
                    "Content-Type".Equals(node.Attributes["http-equiv"].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    isContentTypeMetaTag = true;
                }

                if (node.Attributes["name"] != null &&
                    "Content-Type".Equals(node.Attributes["name"].Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    isContentTypeMetaTag = true;
                }

                if (isContentTypeMetaTag &&
                    node.Attributes["content"] != null &&
                    node.Attributes["content"].Value != null)
                {
                    int eqIdx = node.Attributes["content"].Value.LastIndexOf('=');
                    if (eqIdx != -1)
                    {
                        string encoding = node.Attributes["content"].Value.Substring(eqIdx + 1).Trim();

                        try
                        {
                            newEncoding = Encoding.GetEncoding(encoding);

                            if (newEncoding.EncodingName != m_Encoding.EncodingName)
                                return true;
                        }
                        catch (ArgumentException)
                        { }
                    }
                }
            }

            return false;
        }

        private unsafe void LoadRaw()
        {
            Debug.Assert(m_XmlDoc != null);

            if (string.IsNullOrEmpty(m_RawHtml))
                return;

            lock (m_SyncLock)
            {
                m_DocTypeElement = null;
                m_BodyElement = null;
                m_HeadElement = null;
                m_HtmlElement = null;
                m_NamespaceManager = null;

                char[] rawData = m_Encoding.GetChars(m_Encoding.GetBytes(m_RawHtml));

                fixed (char* charData = rawData)
                {
                    if (m_Parser != null)
                    {
                        // If this is not the first parse operation then create a new doc
                        m_XmlDoc = new XmlDocument();
                        base.InitXmlNode(m_XmlDoc, this);
                    }

                    m_Parser = new HtmlParser(m_FixupMode);

                    m_Parser.Parse(rawData, charData, this);
                }
            }
        }

        internal const string XHTML_ST = "HTML 1.0 Strict";
        internal const string XHTML_TR = "HTML 1.0 Transitional";
        internal const string XHTML_FR = "HTML 1.0 Frameset";
        internal const string HTML_4_01 = "HTML 4.01";
        internal const string HTML_4_01_TR = "HTML 4.01 Transitional";
        internal const string HTML_4_01_FR = "HTML 4.01 Frameset";
        internal const string HTML_3_2 = "HTML 3.2 Final";

        private void ChildAppending(HtmlNode child, ref OnChildAppendingEventArgs e)
        {
            // NOTE: All "<!DOCTYPE" elements are *always* added to the HtmlDocument by the parser
            //       so we cannot miss a "<!DOCTYPE" on a wrong place
            if (child is HtmlDocTypeElement)
            {
                // Dont add the DocType element. This will stuff up the XPath queries
                e.Cancel = true;

                if (m_DocTypeElement == null)
                {
                    m_DocTypeElement = child as HtmlDocTypeElement;

                    //<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"
                    //<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
                    //<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd">
                    //<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
                    //<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/1999/REC-html401-19991224/loose.dtd">
                    //<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Frameset//EN" "http://www.w3.org/TR/1999/REC-html401-19991224/frameset.dtd">
                    //<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 3.2 Final//EN">

                    // By default use Html 4.01 Strict
                    m_HtmlDtd = HtmlDtd.Html401;

                    if (!string.IsNullOrEmpty(m_DocTypeElement.PublicId))
                    {
                        string[] tokens = m_DocTypeElement.PublicId.Split(new string[] { "//" } , StringSplitOptions.None);

                        if (tokens.Length > 2)
                        {
                            if (tokens[1] == "W3C")
                            {
                                string dtd = tokens[2].Substring(4);

                                if (dtd == XHTML_ST)
                                    m_HtmlDtd = HtmlDtd.XhtmlStrict;

                                if (dtd == XHTML_TR)
                                    m_HtmlDtd = HtmlDtd.XhtmlTransitional;

                                if (dtd == XHTML_FR)
                                    m_HtmlDtd = HtmlDtd.XhtmlFrameset;

                                if (dtd == HTML_4_01)
                                    m_HtmlDtd = HtmlDtd.Html401;

                                if (dtd == HTML_4_01_TR)
                                    m_HtmlDtd = HtmlDtd.Html401Transitional;

                                if (dtd == HTML_4_01_FR)
                                    m_HtmlDtd = HtmlDtd.Html401Frameset;

                                if (dtd == HTML_3_2)
                                    m_HtmlDtd = HtmlDtd.Html32;
                            }
                        }
                    }
                }
            }
        }

        internal HtmlDtd HtmlDocType
        {
            get
            {
                return m_HtmlDtd;
            }
        }

        private XmlNamespaceManager m_NamespaceManager = null;
        internal XmlNamespaceManager NamespaceManager
        {
            get
            {
                if (m_NamespaceManager == null)
                {
                    m_NamespaceManager = new XmlNamespaceManager(m_XmlDoc.NameTable);
                }

                return m_NamespaceManager;
            }
        }

        // TODO: Make this public when custom attributes functionality is implemeted
        internal static void RegisterTagAttributesMapper(Type elementType)
        {
            object[] attArray = elementType.GetCustomAttributes(typeof(HtmlTagAttributesMapperAttribute), false);

            if (attArray.Length != 1 || !(attArray[0] is HtmlTagAttributesMapperAttribute))
                throw new HtmlParserException(string.Format(CultureInfo.InvariantCulture, "Type '{0}' must use the '{1}' attribute in order to be registered as a tag attribute mapper!", elementType.FullName, typeof(HtmlTagAttributesMapperAttribute).FullName));

            Debug.Assert(attArray[0] is HtmlTagAttributesMapperAttribute);

            // All element names are lower case!
            string elementName = (attArray[0] as HtmlTagAttributesMapperAttribute).TagName.ToLower(CultureInfo.InvariantCulture);

            Debug.Assert(!string.IsNullOrEmpty(elementName));

            // Add it to the map
            try
            {
                s_TagMappersAndStructureProviders.Add(elementName, elementType);
            }
            catch (ArgumentException)
            {
                s_TagMappersAndStructureProviders[elementName] = elementType;
            }
        }

        internal HtmlElement CreateElement(
            string prefix, 
            string localName, 
            string namespaceURI,
            bool isEmptyTag,
            NodePosition parsedPosition)
        {
            HtmlElement element = null;
            Type customType = null;

            if (s_TagMappersAndStructureProviders.Count > 0)
            {
                try
                {
                    Debug.Assert(localName != null);

                    customType = s_TagMappersAndStructureProviders[localName.ToLower(CultureInfo.InvariantCulture)];
                }
                catch (KeyNotFoundException)
                {
                    customType = null;
                }
            }
            
            if (customType != null)
            {
                // NOTE: create the element using reflection or use a factory class for faster creation, but still register them the same way

                throw new NotImplementedException("Custom tag mappers are not supported yet.");
            }
            else
            {
                switch (m_HtmlDtd)
                {
                    case HtmlDtd.Html32:
                    case HtmlDtd.Html401:                    
                    case HtmlDtd.Html401Transitional:
                    case HtmlDtd.Html401Frameset:
                    case HtmlDtd.XhtmlStrict:
                    case HtmlDtd.XhtmlTransitional:
                    case HtmlDtd.XhtmlFrameset:

                        element = Html.Specialized.Html401.Factory.CreateHtml401Element(prefix, localName, namespaceURI, this, isEmptyTag, parsedPosition);

                        if (element == null)
                            element = Html.Specialized.HtmlProprietary.Factory.CreateHtmlProprietaryElement(prefix, localName, namespaceURI, this, isEmptyTag, parsedPosition);

                        if (element == null)
                            element = Html.Specialized.HtmlNetscape.Factory.CreateHtmlNetscapeElement(prefix, localName, namespaceURI, this, isEmptyTag, parsedPosition);

                        break;
                    default:
                        throw new HtmlParserException("Invalid HTML DTD.");
                }
            }

            if (element == null)
                return new HtmlElement(prefix, localName, namespaceURI, this, isEmptyTag, parsedPosition);
            else
                return element;
        }

        /// <summary>
        /// The XPathLocation for an HTML document is an empty string.
        /// </summary>
        public override string XPathLocation
        {
            get
            {
                return string.Empty;
            }
        }

        //public HtmlElement DocumentElement
        //{
        //    get
        //    {
        //        Debug.Assert(m_XmlDoc != null);

        //        if (m_XmlDoc != null)
        //        {
        //            XmlNode node = m_XmlDoc.SelectSingleNode("//html");

        //            if (node is IHtmlNodeReferenceHolder)
        //                return (HtmlElement)(node as IHtmlNodeReferenceHolder).HtmlNodeReference;
        //        }

        //        return null;
        //    }
        //}

        /// <summary>
        /// Returns the <b>&lt;!DOCTYPE&gt;</b> element in the document of a loaded document if defined. 
        /// </summary>
        public HtmlDocTypeElement DocumentType
        {
            get
            {
                return m_DocTypeElement;
            }
        }

        /// <summary>
        /// Returns the <b>&lt;body&gt;</b> element in the document or null if no document has been loaded yet. 
        /// The <b>&lt;body&gt;</b> element of a loaded document will always exist even if the parsed content does not explicitely define it.
        /// </summary>
        public HtmlElement BodyElement
        {
            get
            {
                return m_BodyElement;
            }
        }

        /// <summary>
        /// Returns the <b>&lt;head&gt;</b> element in the document or null if no document has been loaded yet. 
        /// The <b>&lt;head&gt;</b> element of a loaded document will always exist even if the parsed content does not explicitely define it.
        /// </summary>
        public HtmlElement HeadElement
        {
            get
            {
                return m_HeadElement;
            }
        }

        /// <summary>
        /// Returns the <b>&lt;html&gt;</b> element in the document or null if no document has been loaded yet. 
        /// The <b>&lt;html&gt;</b> element of a loaded document will always exist even if the parsed content does not explicitely define it.
        /// </summary>
        public HtmlElement HtmlElement
        {
            get
            {
                return m_HtmlElement;
            }
        }

        /// <summary>
        /// Saves the HTML document as an XML file including all changes made.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">The fileName is null.</exception>
        /// <exception cref="System.InvalidOperationException">The HTML document hasn't been loaded yet.</exception>
        /// <param name="fileName">The location of the file where you want to save the document.</param>
        public void SaveXml(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            if (m_XmlDoc != null)
                m_XmlDoc.Save(fileName);
            else
                throw new InvalidOperationException("There is no parsed document to save.");
        }

        /// <summary>
        /// Saves the HTML document including all changes made as an HTML file without reformatting the original content. Existing files will be overwritten.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">The fileName is null.</exception>
        /// <exception cref="System.InvalidOperationException">The HTML document hasn't been loaded yet.</exception>
        /// <param name="fileName">The location of the file where you want to save the document.</param>
        public void Save(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            if (m_XmlDoc != null)
            {
                using(FileStream outputFile = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                using (TextWriter writer = new StreamWriter(outputFile))
                {
                    writer.Write(GetOuterHtml());
                    writer.Flush();
                }
            }
            else
                throw new InvalidOperationException("There is no parsed document to save.");
        }
    }
}
