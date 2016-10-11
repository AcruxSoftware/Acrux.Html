using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using System.Globalization;

namespace Acrux.Html
{
    internal abstract class HtmlPredicateParser
    {
        protected abstract unsafe void InitParser(char[] rawData, char* charData, Encoding encoding);

        protected abstract char CurrentChar { get; }

        protected abstract char NextChar { get; }

        protected abstract char SecondNextChar { get; }

        protected abstract char PreviousChar { get; }

        protected abstract char SecondPreviousChar { get; }

        protected abstract int CurrentIndex { get; }

        protected abstract bool HasMoreChars { get; }

        protected abstract int CurrentLineNo { get; }

        protected abstract int CurrentColNo { get; }

        protected abstract bool ReadChar();

        protected abstract string ReadChars(int numberOfCharsToRead);


        protected abstract string GetParsedDataString();

        protected abstract void MarkNewReadingBuffer();


#if DEBUG
        protected abstract void Push();

        protected abstract void PopAndRestore();

        protected abstract void PopWithoutRestore();
        
        protected abstract void RestoreWithoutPop();
#endif

        protected abstract bool SkipWhiteSpaces();

        protected bool SequenceFollowsIgnoreCase(
            string upperCaseString,
            char[] delimiterCharSet)
        {
            return SequenceFollowsIgnoreCase(upperCaseString, delimiterCharSet, true, false);
        }


        //protected bool SequenceFollowsIgnoreCase(
        //    string upperCaseString,
        //    char[] delimiterCharSet,
        //    bool startFromCurrentChar)
        //{
        //    return SequenceFollowsIgnoreCase(upperCaseString, delimiterCharSet, startFromCurrentChar, false);
        //}

        protected abstract bool SequenceFollowsIgnoreCase(
            string upperCaseString, 
            char[] delimiterCharSet, 
            bool startFromCurrentChar, 
            bool dontRestorePositionIfSuccessful);


        /// <summary>
        /// Attempts to read a Name predicate starting from the current position. The name predicate <b>must</b> be followed 
        /// by a white space. If the Name predicate is succesfully parsed the function returns true and the <b>name</b> 
        /// parameter will be intialized with the name read. If the predicate is nor recognized then the function returns false,
        /// <b>name</b> output parameter will have a value of <b>null</b> and the position will be restored to the current
        /// possition before the function was called. If the predicate is recognized and parsed the position remains on the
        /// white space immediately after the predicate.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected abstract bool ReadNamePredicate(out string name);


        protected abstract bool ReadPubidLiteral(out string pubidLiteral);


        protected abstract bool ReadSystemLiteral(out string systemLiteral);

        /// <summary>
        /// Important when when calling this method the cursor must be positioned on the first "-" of the <b>&lt;!-- </b>
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        protected abstract bool ReadComment(out string comment);
    }
}
