using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Acrux.Html
{
    /// <summary>
    /// Provides the exception thrown when an error occurs while parsing an HTML document.
    /// </summary>
    [Serializable]
    public sealed class HtmlParserException : Exception 
    {
        internal HtmlParserException()
            : base()
        { }

        internal HtmlParserException(string message)
            : base(message)
        { }

        internal HtmlParserException(string message, Exception innerException)
            : base(message, innerException)
        { }

        private HtmlParserException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        { }

    }
}
