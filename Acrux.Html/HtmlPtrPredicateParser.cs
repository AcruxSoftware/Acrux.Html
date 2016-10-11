using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using System.Globalization;


namespace Acrux.Html
{
    internal class HtmlPtrPredicateParser : HtmlPredicateParser
    {
        protected char[] m_RawData;
        protected unsafe char* m_RawDataPtr;
        private unsafe char* m_FirstRawDataPtr;

        protected int m_CurrentIndex;
        private int m_CurrentLineNo;
        private int m_CurrentColNo;

        private unsafe char* m_ReadingBufferPositionPtr;

        protected unsafe int ReadingBufferStartPosition
        {
            get { return (int)(m_ReadingBufferPositionPtr - m_FirstRawDataPtr); }
        }

        #region Basic Initialization and Position management

        protected override unsafe void InitParser(char[] rawData, char* charData, Encoding encoding)
        {
            m_RawData = rawData;
            m_RawDataPtr = charData;
            m_FirstRawDataPtr = charData;

            m_ReadingBufferPositionPtr = m_RawDataPtr;

            // We are positioned on the 1-st char of the 1-st line
            m_CurrentLineNo = 1;
            m_CurrentColNo = 1;

#if DEBUG

            m_StartIndex = (long)m_RawDataPtr;
#endif
        }

        protected override unsafe char CurrentChar
        {
            get
            {
                return *m_RawDataPtr;
            }
        }

        protected override unsafe char NextChar
        {
            get
            {
                try
                {
                    return *(m_RawDataPtr + 1);
                }
                catch (IndexOutOfRangeException)
                {
                    // TODO: Try to read after the end of the buffer and see what happens
                    // TODO: Build unit tests only for the HtmlPredicateParser class to test those sort of things!
                    return '\x0';
                }
            }
        }

        protected override unsafe char SecondNextChar
        {
            get
            {
                try
                {
                    return *(m_RawDataPtr + 2);
                }
                catch (IndexOutOfRangeException)
                {
                    // TODO: Try to read after the end of the buffer and see what happens
                    // TODO: Build unit tests only for the HtmlPredicateParser class to test those sort of things!
                    return '\x0';
                }
            }
        }

        protected override unsafe char PreviousChar
        {
            get
            {
                try
                {
                    return *(m_RawDataPtr - 1);
                }
                catch (IndexOutOfRangeException)
                {
                    // TODO: Try to read after the end of the buffer and see what happens
                    // TODO: Build unit tests only for the HtmlPredicateParser class to test those sort of things!
                    return '\x0';
                }
            }
        }

        protected override unsafe char SecondPreviousChar
        {
            get
            {
                try
                {
                    return *(m_RawDataPtr - 2);
                }
                catch (IndexOutOfRangeException)
                {
                    // TODO: Try to read after the end of the buffer and see what happens
                    // TODO: Build unit tests only for the HtmlPredicateParser class to test those sort of things!
                    return '\x0';
                }
            }
        }

        protected override int CurrentIndex
        {
            get
            {
                return m_CurrentIndex;
            }
        }

        protected override unsafe bool HasMoreChars
        {
            get
            {
                return *m_RawDataPtr != '\0';
            }
        }

        protected override int CurrentLineNo
        {
            get
            {
                return m_CurrentLineNo;
            }
        }

        protected override int CurrentColNo
        {
            get
            {
                return m_CurrentColNo;
            }
        }

        protected override unsafe bool ReadChar()
        {
            m_CurrentIndex++;
            m_RawDataPtr++;

            bool moreChars = *m_RawDataPtr != '\0';

            if (moreChars)
                CalcColLine();

            return moreChars;
        }

        protected override unsafe string ReadChars(int numberOfCharsToRead)
        {
            string retVal = new string(m_RawDataPtr, 0, numberOfCharsToRead);

            m_RawDataPtr += numberOfCharsToRead;
            m_CurrentIndex += numberOfCharsToRead;

            return retVal;
        }

        private void CalcColLine()
        {
            if (CurrentChar == '\xA')
            {
                m_CurrentLineNo++;
                m_CurrentColNo = 0;
            }
            else
                m_CurrentColNo++;
        }

        protected override unsafe void MarkNewReadingBuffer()
        {
            m_ReadingBufferPositionPtr = m_RawDataPtr;
        }

        protected override unsafe string GetParsedDataString()
        {
            return GetParsedDataString(m_ReadingBufferPositionPtr);
        }

        private unsafe string GetParsedDataString(char* startFrom)
        {
            string retVal = new string(startFrom, 0, (int)(m_RawDataPtr - startFrom));
            return retVal;
        }

        protected override unsafe bool SkipWhiteSpaces()
        {
            while (
                *m_RawDataPtr != '\0' &&
                (*m_RawDataPtr == '\x20' || *m_RawDataPtr == '\x09' || *m_RawDataPtr == '\xD' || *m_RawDataPtr == '\xA')
                )
            {
                m_CurrentIndex++;
                m_RawDataPtr++;

                CalcColLine();
            }

            return *m_RawDataPtr != '\0';
        }

        protected override unsafe bool SequenceFollowsIgnoreCase(
            string upperCaseString,
            char[] delimiterCharSet,
            bool startFromCurrentChar,
            bool dontRestorePositionIfSuccessful)
        {
            int len = upperCaseString.Length;

            char* followingStuff = stackalloc char[len];
            followingStuff = m_RawDataPtr + (!startFromCurrentChar ? 1 : 0);

            for (int i = 0; i < len; i++)
            {
                if (*(followingStuff + i) == '\0')
                {
                    return false;
                }
            }

            if (delimiterCharSet != null)
            {
                bool delimiterFollows = false;

                foreach (char ch in delimiterCharSet)
                {
                    if (ch == *(followingStuff + len))
                    {
                        delimiterFollows = true;
                        break;
                    }
                }

                if (!delimiterFollows)
                    return false;
            }

            string whatFollows = new string(followingStuff, 0, len);
            bool follows = whatFollows.Equals(upperCaseString, StringComparison.InvariantCultureIgnoreCase);

            if (follows && /* If the sequence is found */
                dontRestorePositionIfSuccessful /* And we shouldn't restore the position */)
            {
                // Then move the pointer 
                m_RawDataPtr += len + (!startFromCurrentChar ? 1 : 0);
                m_CurrentIndex += len + (!startFromCurrentChar ? 1 : 0);
            }

            return follows;
        }

        #endregion

        #region Predicate Parsing

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
        protected override unsafe bool ReadNamePredicate(out string name)
        {
            //[4]    NameChar    ::=    Letter | Digit | '.' | '-' | '_' | ':' | CombiningChar | Extender  
            //[5]    Name        ::=    (Letter | '_' | ':') (NameChar)* 

            name = null;

            long startPos = (long)m_RawDataPtr;
            if (!CharacterClasses.IsLetter(m_RawDataPtr) && *m_RawDataPtr != '_' && *m_RawDataPtr != ':')
            {
                // Restore the starting position as the parsing was unsuccesfull
                m_RawDataPtr = (char*)startPos;
                return false;
            }


            if (ReadChar())
            {
                while (CharacterClasses.IsNameChar(m_RawDataPtr))
                {
                    if (!ReadChar())
                    {
                        // Restore the starting position as the parsing was unsuccesfull
                        m_RawDataPtr = (char*)startPos;
                        return false;
                    }
                }

                name = GetParsedDataString((char*)startPos);
            }

            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override unsafe bool ReadPubidLiteral(out string pubidLiteral)
        {
            //[12]    PubidLiteral      ::=    '"' PubidChar* '"' | "'" (PubidChar - "'")* "'" 
            //[13]    PubidChar         ::=    #x20 | #xD | #xA | [a-zA-Z0-9] | [-'()+,./:=?;!*#@$_%] 

            pubidLiteral = null;

            long startPos = (long)m_RawDataPtr;

            if (CurrentChar != '"' && CurrentChar != '\'')
            {
                // Restore the starting position as the parsing was unsuccesfull
                m_RawDataPtr = (char*)startPos;
                return false;
            }

            char beginChar = CurrentChar;

            if (!ReadChar())
            {
                // Restore the starting position as the parsing was unsuccesfull
                m_RawDataPtr = (char*)startPos;
                return false;
            }

            long startReadingPos = (long)m_RawDataPtr;

            while (CharacterClasses.IsPubidChar(m_RawDataPtr) && *m_RawDataPtr != beginChar)
            {
                if (!ReadChar())
                {
                    // We have reached the end of the file
                    // Restore the starting position as the parsing was unsuccesfull
                    m_RawDataPtr = (char*)startPos;
                    return false;
                }
            }

            if (CurrentChar != beginChar)
            {
                // TODO: Invalid charecter in the pubidLiteral
                //       What to do??
            }

            pubidLiteral = GetParsedDataString((char*)startReadingPos);

            // Move to the next character after the closing quote
            ReadChar();

            return true;
        }


        protected override unsafe bool ReadSystemLiteral(out string systemLiteral)
        {
            //[11]    SystemLiteral     ::=    ('"' [^"]* '"') | ("'" [^']* "'") 

            systemLiteral = null;

            long startPos = (long)m_RawDataPtr;

            if (CurrentChar != '"' && CurrentChar != '\'')
            {
                // Restore the starting position as the parsing was unsuccesfull
                m_RawDataPtr = (char*)startPos;
                return false;
            }

            char beginChar = CurrentChar;

            if (!ReadChar())
            {
                // Restore the starting position as the parsing was unsuccesfull
                m_RawDataPtr = (char*)startPos;
                return false;
            }

            long startReadingPos = (long)m_RawDataPtr;

            while (CurrentChar != beginChar)
            {
                if (!ReadChar())
                {
                    // We have reached the end of the file
                    // Restore the starting position as the parsing was unsuccesfull
                    m_RawDataPtr = (char*)startPos;
                    return false;
                }
            }

            systemLiteral = GetParsedDataString((char*)startReadingPos);

            // Move to the next character after the closing quote
            ReadChar();

            return true;
        }


        /// <summary>
        /// Important when when calling this method the cursor must be positioned on the first "-" of the <b>&lt;!-- </b>
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        protected override unsafe bool ReadComment(out string comment)
        {
            // ----------------------------------------------------------------------------------------------------
            // The XML rule for a comment is shown below, however we use a different one
            //[15]    Comment    ::=    '<!--' ((Char - '-') | ('-' (Char - '-')))* '-->' 
            // ----------------------------------------------------------------------------------------------------

            comment = null;

            long startPos = (long)m_RawDataPtr;

            if (CurrentChar != '-' || NextChar != '-')
            {
                // Restore the starting position as the parsing was unsuccesfull
                m_RawDataPtr = (char*)startPos;
                return false;
            }

            // Skip the two "-", already validated
            ReadChar();
            if (ReadChar())
            {
                long startReadingPos = (long)m_RawDataPtr;

                // These will be considered comments (i.e. second -- will not be considered the end of the comment:
                // <!-- This a="--" is -->    
                // <!-- This a='--' is -->    
                // <!-- This '--' is -->    
                // <!-- This "--" is -->    
                // <!-- This a=-- is -->    
                // <!-- This a=""--" is -->
                // In these cases the second -- will be considered as the end of the comment:
                // <!-- This a="-->" is -->
                // <!-- This a='-->' is -->
                // Unfinished comments considered empty comments
                // <!--[EOF]

                while (!(CurrentChar == '-' && NextChar == '-' && SecondNextChar == '>'))
                {
                    if (!ReadChar())
                    {
                        // We have reached the end of the file. We consider this as a successfully parsed comment (which was not closed)
                        break;
                    }
                }

                comment = GetParsedDataString((char*)startReadingPos);

                if (HasMoreChars)
                {
                    // Skip the 2 ending --
                    Debug.Assert(CurrentChar == '-');
                    ReadChar();
                    Debug.Assert(CurrentChar == '-');
                    ReadChar();
                }
            }
            else
            {
                // We have reached the end of the file after the '<!--'
                // We consider this as a successfully parsed empty comment (which was not closed)

                comment = string.Empty;
            }

            return true;
        }



        #endregion


#if DEBUG

        private long m_StartIndex = -1;
        private Stack<int> m_PositionStack = new Stack<int>();
        private Stack<int> m_PositionLineNoStack = new Stack<int>();
        private Stack<int> m_PositionColNoStack = new Stack<int>();
        private Stack<int> m_ReadingBufferPositionsStack = new Stack<int>();
        private Stack<int> m_ReadingBufferPosStackCountStack = new Stack<int>();


        protected override void Push()
        {
            m_PositionStack.Push(m_CurrentIndex);

            m_PositionColNoStack.Push(m_CurrentColNo);
            m_PositionLineNoStack.Push(m_CurrentLineNo);

            // Save the current number of read buffer indexes. We will have to restore this on a Pop()
            m_ReadingBufferPosStackCountStack.Push(m_ReadingBufferPositionsStack.Count);
        }

        protected override void PopAndRestore()
        {
            RestoreWithoutPop();
            PopWithoutRestore();
        }

        protected override void PopWithoutRestore()
        {
            Debug.Assert(m_PositionStack.Count - 1 >= 0);
            Debug.Assert(m_PositionColNoStack.Count - 1 >= 0);
            Debug.Assert(m_PositionLineNoStack.Count - 1 >= 0);
            Debug.Assert(m_ReadingBufferPosStackCountStack.Count - 1 >= 0);

            if (m_PositionStack.Count - 1 >= 0)
            {
                m_PositionStack.Pop();
                m_PositionColNoStack.Pop();
                m_PositionLineNoStack.Pop();
                m_ReadingBufferPosStackCountStack.Pop();
            }
            else
                throw new HtmlParserException("Attempting to POP from an empty stack!");
        }

        protected override unsafe void RestoreWithoutPop()
        {
            Debug.Assert(m_PositionStack.Count - 1 >= 0);
            Debug.Assert(m_PositionColNoStack.Count - 1 >= 0);
            Debug.Assert(m_PositionLineNoStack.Count - 1 >= 0);
            Debug.Assert(m_ReadingBufferPosStackCountStack.Count - 1 >= 0);

            if (m_PositionStack.Count - 1 >= 0)
            {
                m_CurrentIndex = m_PositionStack.Peek();
                m_RawDataPtr = (char*)(m_StartIndex + m_CurrentIndex);
                    
                m_CurrentColNo = m_PositionColNoStack.Peek();
                m_CurrentLineNo = m_PositionLineNoStack.Peek();
                int startingReadBuffPosCount = m_ReadingBufferPosStackCountStack.Peek();

                while (m_ReadingBufferPositionsStack.Count > startingReadBuffPosCount)
                    // Remove all read buffer indexes added after the Push()
                    m_ReadingBufferPositionsStack.Pop();
            }
            else
                throw new HtmlParserException("Attempting to POP from an empty stack!");
        }
#endif
    }
}
