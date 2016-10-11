using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Security.Permissions;

namespace Acrux.Html.XPath
{
    /// <summary>
    /// See http://support.microsoft.com/default.aspx?scid=kb;EN-US;324462 for more info.
    /// </summary>
    internal class HtmlXsltContext : XsltContext
    {
        private static HtmlXsltContext s_Singleton = new HtmlXsltContext();
        private static object s_SynkLock = new object();
        private XsltContext m_AdditionalContext = null;

        internal static HtmlXsltContext GetInstance(XsltContext additionalContext)
        {
            lock (s_SynkLock)
            {
                s_Singleton.SetAdditionalXsltContext(additionalContext);
            }

            return s_Singleton;
        }

        internal HtmlXsltContext()
            : base()
        {
        }

        internal HtmlXsltContext(NameTable nt)
            : base(nt)
        { }

        private void SetAdditionalXsltContext(XsltContext additionalContext)
        {
            m_AdditionalContext = additionalContext;
        }

        public override int CompareDocument(string baseUri, string nextbaseUri)
        {
            return 0;
        }

        public override bool PreserveWhitespace(System.Xml.XPath.XPathNavigator node)
        {
            return false;
        }

        public override IXsltContextFunction ResolveFunction(string prefix, string name,
            System.Xml.XPath.XPathResultType[] ArgTypes)
        {
            //// TODO: Add some useful custom functions ???
            //// TODO: Implement all standart XPath and XSLT function and also build unit tests for them !!!
            ////       http://www.w3.org/TR/xquery-operators/
            ////       http://msdn.microsoft.com/msdnmag/issues/02/03/xml/
            //
            // Functions supported by Microsoft.NET
            //
            //public enum FunctionType
            //{
            //    FuncLast,
            //    FuncPosition,
            //    FuncCount,
            //    FuncID,
            //    FuncLocalName,
            //    FuncNameSpaceUri,
            //    FuncName,
            //    FuncString,
            //    FuncBoolean,
            //    FuncNumber,
            //    FuncTrue,
            //    FuncFalse,
            //    FuncNot,
            //    FuncConcat,
            //    FuncStartsWith,
            //    FuncContains,
            //    FuncSubstringBefore,
            //    FuncSubstringAfter,
            //    FuncSubstring,
            //    FuncStringLength,
            //    FuncNormalize,
            //    FuncTranslate,
            //    FuncLang,
            //    FuncSum,
            //    FuncFloor,
            //    FuncCeiling,
            //    FuncRound,
            //    FuncUserDefined
            //}

            IXsltContextFunction function = XsltContextFunctionBase.ResolveFunction(prefix, name, ArgTypes);
            if (function != null)
                return function;

            if (m_AdditionalContext != null)
                return m_AdditionalContext.ResolveFunction(prefix, name, ArgTypes);
            else
                return null;
        }

        public override IXsltContextVariable ResolveVariable(string prefix, string name)
        {
            if (m_AdditionalContext != null)
                return m_AdditionalContext.ResolveVariable(prefix, name);
            else
                return null;
        }

        public override bool Whitespace
        {
            get { return false; }
        }
    }
}
