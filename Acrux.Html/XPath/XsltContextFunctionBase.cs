using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace Acrux.Html.XPath
{
    internal struct XsltFunctionEntry
    {
        internal readonly int MinArgs;
        internal readonly int MaxArgs;
        internal readonly XPathResultType ReturnType;
        internal readonly XPathResultType[] ArgTypes;
        internal readonly string FunctionName;
        internal readonly string Prefix;

        internal XsltFunctionEntry(
            string prefix,
            string functionName, 
            int minArgs, 
            int maxArgs, 
            XPathResultType returnType, 
            XPathResultType[] argTypes)
        {
            this.MinArgs = minArgs;
            this.MaxArgs = maxArgs;
            this.ReturnType = returnType;
            this.ArgTypes = argTypes;
            this.FunctionName = functionName;
            this.Prefix = prefix;
        }
    }

    internal abstract class XsltContextFunctionBase : IXsltContextFunction
    {
        internal static IXsltContextFunction ResolveFunction(
            string prefix, 
            string name, 
            XPathResultType[] ArgTypes)
        {

            foreach (XsltFunctionEntry functionDefinition in XPath2Functions.FUNCTIONS)
            {
                if (functionDefinition.Prefix.Equals(prefix) && functionDefinition.FunctionName.Equals(name))
                    return new XPath2Functions(functionDefinition);
            }

            return null;
        }

        protected XsltFunctionEntry m_FunctionDef;

        internal abstract object Invoke(
            XsltContext xsltContext,
            object[] args,
            System.Xml.XPath.XPathNavigator docContext);

        // Constructor that is used in the ResolveFunction method of the custom XsltContext class (CustomContext) 
        // to create and to return an instance of the IXsltContextFunction object to execute a specified 
        // user-defined XPath extension function at run time.
        internal XsltContextFunctionBase(XsltFunctionEntry functionDef)
        {
            this.m_FunctionDef = functionDef;
        }

        #region IXsltContextFunction implementation
        XPathResultType[] IXsltContextFunction.ArgTypes
        {
            get
            {
                return m_FunctionDef.ArgTypes;
            }
        }

        int IXsltContextFunction.Maxargs
        {
            get
            {
                return m_FunctionDef.MaxArgs;
            }
        }

        int IXsltContextFunction.Minargs
        {
            get
            {
                return m_FunctionDef.MinArgs;
            }
        }

        XPathResultType IXsltContextFunction.ReturnType
        {
            get
            {
                return m_FunctionDef.ReturnType;
            }
        }

        object IXsltContextFunction.Invoke(
            XsltContext xsltContext, 
            object[] args,
            System.Xml.XPath.XPathNavigator docContext)
        {
            return this.Invoke(xsltContext, args, docContext);
        }
        #endregion
    }
}
