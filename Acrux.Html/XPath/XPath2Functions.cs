using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Globalization;
using System.Reflection;

namespace Acrux.Html.XPath
{
    internal class XPath2Functions : XsltContextFunctionBase
    {
        // http://www.w3.org/TR/xpath20/
        // http://www.w3.org/TR/xpath-functions/

        private const string PREFIX_FN = "fn";
        private const string PREFIX_OP = "op";

        private const string OPERATION_NUMERIC_ADD = "numeric-add";
        private const string OPERATION_NUMERIC_SUBTRACT = "numeric-subtract";
        private const string OPERATION_NUMERIC_MULTIPLY = "numeric-multiply";
        private const string OPERATION_NUMERIC_DIVIDE = "numeric-divide";
        private const string OPERATION_NUMERIC_INTEGER_DIVIDE = "numeric-integer-divide";
        private const string OPERATION_NUMERIC_MOD = "numeric-mod";
        private const string OPERATION_NUMERIC_UNARY_PLUS = "numeric-unary-plus";
        private const string OPERATION_NUMERIC_UNARY_MINUS = "numeric-unary-minus";
        private const string OPERATION_NUMERIC_EQUAL = "numeric-equal";
        private const string OPERATION_NUMERIC_LESS_THAN = "numeric-less-than";
        private const string OPERATION_NUMERIC_GREATER_THAN = "numeric-greater-than";
        private const string FUNCTION_ABS = "abs";
        private const string FUNCTION_CEILING = "ceiling";
        private const string FUNCTION_FLOOR = "floor";
        private const string FUNCTION_ROUND = "round";
        private const string FUNCTION_ROUND_HALF_TO_EVEN = "round-half-to-even";
        private const string FUNCTION_STR_COMPARE = "compare";
        private const string FUNCTION_STR_NORMALIZE_UNICODE = "normalize-unicode";
        private const string FUNCTION_STR_UPPER_CASE = "upper-case";
        private const string FUNCTION_STR_LOWER_CASE = "lower-case";
        private const string FUNCTION_STR_ENCODE_FOR_URI = "encode-for-uri";
        private const string FUNCTION_STR_IRI_TO_URI = "iri-to-uri";
        private const string FUNCTION_STR_ESCAPE_HTML_URI = "escape-html-uri";
        private const string FUNCTION_REGEX_MATCHES = "matches";
        private const string FUNCTION_REGEX_MATCH = "match";
        private const string FUNCTION_REGEX_REPLACE = "replace";
        private const string FUNCTION_RESOLVE_URI = "resolve-uri";
        private const string OPERATION_BOOL_EQUAL = "boolean-equal";
        private const string OPERATION_BOOL_LESS_THAN = "boolean-less-than";
        private const string OPERATION_BOOL_GREATER_THAN = "boolean-greater-than";

        private const string VALUE_NAN = "NaN";
        private const string VALUE_INF = "INF";


        internal static XsltFunctionEntry[] FUNCTIONS = new XsltFunctionEntry[] 
        { 
            new XsltFunctionEntry(PREFIX_OP, OPERATION_NUMERIC_ADD, 2, 2, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number, XPathResultType.Number}),
            new XsltFunctionEntry(PREFIX_OP, OPERATION_NUMERIC_SUBTRACT, 2, 2, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number, XPathResultType.Number}),
            new XsltFunctionEntry(PREFIX_OP, OPERATION_NUMERIC_MULTIPLY, 2, 2, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number, XPathResultType.Number}),
            new XsltFunctionEntry(PREFIX_OP, OPERATION_NUMERIC_DIVIDE, 2, 2, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number, XPathResultType.Number}),
            new XsltFunctionEntry(PREFIX_OP, OPERATION_NUMERIC_INTEGER_DIVIDE, 2, 2, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number, XPathResultType.Number}),
            new XsltFunctionEntry(PREFIX_OP, OPERATION_NUMERIC_MOD, 2, 2, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number, XPathResultType.Number}),
            new XsltFunctionEntry(PREFIX_OP, OPERATION_NUMERIC_UNARY_PLUS, 1, 1, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number}),
            new XsltFunctionEntry(PREFIX_OP, OPERATION_NUMERIC_UNARY_MINUS, 1, 1, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number}),
            new XsltFunctionEntry(PREFIX_OP, OPERATION_NUMERIC_EQUAL, 2, 2, XPathResultType.Boolean, new XPathResultType[] { XPathResultType.Number, XPathResultType.Number}),
            new XsltFunctionEntry(PREFIX_OP, OPERATION_NUMERIC_LESS_THAN, 2, 2, XPathResultType.Boolean, new XPathResultType[] { XPathResultType.Number, XPathResultType.Number}),
            new XsltFunctionEntry(PREFIX_OP, OPERATION_NUMERIC_GREATER_THAN, 2, 2, XPathResultType.Boolean, new XPathResultType[] { XPathResultType.Number, XPathResultType.Number}),
            new XsltFunctionEntry(PREFIX_FN, FUNCTION_ABS, 1, 1, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number}),
            new XsltFunctionEntry(PREFIX_FN, FUNCTION_CEILING, 1, 1, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number}),
            new XsltFunctionEntry(PREFIX_FN, FUNCTION_FLOOR, 1, 1, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number}),
            new XsltFunctionEntry(PREFIX_FN, FUNCTION_ROUND, 1, 1, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number}),
            new XsltFunctionEntry(PREFIX_FN, FUNCTION_ROUND_HALF_TO_EVEN, 1, 2, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number, XPathResultType.Number}),
            new XsltFunctionEntry(PREFIX_FN, FUNCTION_STR_COMPARE, 2, 3, XPathResultType.Number, new XPathResultType[] { XPathResultType.String, XPathResultType.String, XPathResultType.String}),
            new XsltFunctionEntry(PREFIX_FN, FUNCTION_STR_NORMALIZE_UNICODE, 1, 2, XPathResultType.String, new XPathResultType[] { XPathResultType.String, XPathResultType.String}),
            new XsltFunctionEntry(PREFIX_FN, FUNCTION_STR_UPPER_CASE, 1, 1, XPathResultType.String, new XPathResultType[] { XPathResultType.String}),
            new XsltFunctionEntry(PREFIX_FN, FUNCTION_STR_LOWER_CASE, 1, 1, XPathResultType.String, new XPathResultType[] { XPathResultType.String}),
            new XsltFunctionEntry(PREFIX_FN, FUNCTION_STR_ENCODE_FOR_URI, 1, 1, XPathResultType.String, new XPathResultType[] { XPathResultType.String}),
            new XsltFunctionEntry(PREFIX_FN, FUNCTION_STR_IRI_TO_URI, 1, 1, XPathResultType.String, new XPathResultType[] { XPathResultType.String}),
            new XsltFunctionEntry(PREFIX_FN, FUNCTION_STR_ESCAPE_HTML_URI, 1, 1, XPathResultType.String, new XPathResultType[] { XPathResultType.String}),
            new XsltFunctionEntry(PREFIX_FN, FUNCTION_REGEX_MATCHES, 2, 3, XPathResultType.Boolean, new XPathResultType[] { XPathResultType.String, XPathResultType.String, XPathResultType.String}),
            new XsltFunctionEntry(PREFIX_FN, FUNCTION_REGEX_MATCH, 2, 3, XPathResultType.String, new XPathResultType[] { XPathResultType.String, XPathResultType.String, XPathResultType.String}),
            new XsltFunctionEntry(PREFIX_FN, FUNCTION_REGEX_REPLACE, 3, 4, XPathResultType.String, new XPathResultType[] { XPathResultType.String, XPathResultType.String, XPathResultType.String, XPathResultType.String}),
            new XsltFunctionEntry(PREFIX_FN, FUNCTION_RESOLVE_URI, 1, 2, XPathResultType.String, new XPathResultType[] { XPathResultType.String, XPathResultType.String}),
            new XsltFunctionEntry(PREFIX_OP, OPERATION_BOOL_EQUAL, 2, 2, XPathResultType.Boolean, new XPathResultType[] { XPathResultType.Boolean, XPathResultType.Boolean}),
            new XsltFunctionEntry(PREFIX_OP, OPERATION_BOOL_LESS_THAN, 2, 2, XPathResultType.Boolean, new XPathResultType[] { XPathResultType.Boolean, XPathResultType.Boolean}),
            new XsltFunctionEntry(PREFIX_OP, OPERATION_BOOL_GREATER_THAN, 2, 2, XPathResultType.Boolean, new XPathResultType[] { XPathResultType.Boolean, XPathResultType.Boolean}),

            new XsltFunctionEntry(string.Empty, VALUE_NAN, 0, 0, XPathResultType.Number, new XPathResultType[] { }),
            new XsltFunctionEntry(string.Empty, VALUE_INF, 0, 0, XPathResultType.Number, new XPathResultType[] { }),
            new XsltFunctionEntry(string.Empty, OPERATION_NUMERIC_ADD, 2, 2, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number, XPathResultType.Number}),
            new XsltFunctionEntry(string.Empty, OPERATION_NUMERIC_SUBTRACT, 2, 2, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number, XPathResultType.Number}),
            new XsltFunctionEntry(string.Empty, OPERATION_NUMERIC_MULTIPLY, 2, 2, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number, XPathResultType.Number}),
            new XsltFunctionEntry(string.Empty, OPERATION_NUMERIC_DIVIDE, 2, 2, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number, XPathResultType.Number}),
            new XsltFunctionEntry(string.Empty, OPERATION_NUMERIC_INTEGER_DIVIDE, 2, 2, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number, XPathResultType.Number}),
            new XsltFunctionEntry(string.Empty, OPERATION_NUMERIC_MOD, 2, 2, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number, XPathResultType.Number}),
            new XsltFunctionEntry(string.Empty, OPERATION_NUMERIC_UNARY_PLUS, 1, 1, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number}),
            new XsltFunctionEntry(string.Empty, OPERATION_NUMERIC_UNARY_MINUS, 1, 1, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number}),
            new XsltFunctionEntry(string.Empty, OPERATION_NUMERIC_EQUAL, 2, 2, XPathResultType.Boolean, new XPathResultType[] { XPathResultType.Number, XPathResultType.Number}),
            new XsltFunctionEntry(string.Empty, OPERATION_NUMERIC_LESS_THAN, 2, 2, XPathResultType.Boolean, new XPathResultType[] { XPathResultType.Number, XPathResultType.Number}),
            new XsltFunctionEntry(string.Empty, OPERATION_NUMERIC_GREATER_THAN, 2, 2, XPathResultType.Boolean, new XPathResultType[] { XPathResultType.Number, XPathResultType.Number}),
            new XsltFunctionEntry(string.Empty, FUNCTION_ABS, 1, 1, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number}),
            new XsltFunctionEntry(string.Empty, FUNCTION_CEILING, 1, 1, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number}),
            new XsltFunctionEntry(string.Empty, FUNCTION_FLOOR, 1, 1, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number}),
            new XsltFunctionEntry(string.Empty, FUNCTION_ROUND, 1, 1, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number}),
            new XsltFunctionEntry(string.Empty, FUNCTION_ROUND_HALF_TO_EVEN, 1, 2, XPathResultType.Number, new XPathResultType[] { XPathResultType.Number, XPathResultType.Number}),
            new XsltFunctionEntry(string.Empty, FUNCTION_STR_COMPARE, 2, 3, XPathResultType.Number, new XPathResultType[] { XPathResultType.String, XPathResultType.String, XPathResultType.String}),
            new XsltFunctionEntry(string.Empty, FUNCTION_STR_NORMALIZE_UNICODE, 1, 2, XPathResultType.String, new XPathResultType[] { XPathResultType.String, XPathResultType.String}),
            new XsltFunctionEntry(string.Empty, FUNCTION_STR_UPPER_CASE, 1, 1, XPathResultType.String, new XPathResultType[] { XPathResultType.String}),
            new XsltFunctionEntry(string.Empty, FUNCTION_STR_LOWER_CASE, 1, 1, XPathResultType.String, new XPathResultType[] { XPathResultType.String}),
            new XsltFunctionEntry(string.Empty, FUNCTION_STR_ENCODE_FOR_URI, 1, 1, XPathResultType.String, new XPathResultType[] { XPathResultType.String}),
            new XsltFunctionEntry(string.Empty, FUNCTION_STR_IRI_TO_URI, 1, 1, XPathResultType.String, new XPathResultType[] { XPathResultType.String}),
            new XsltFunctionEntry(string.Empty, FUNCTION_STR_ESCAPE_HTML_URI, 1, 1, XPathResultType.String, new XPathResultType[] { XPathResultType.String}),
            new XsltFunctionEntry(string.Empty, FUNCTION_REGEX_MATCHES, 2, 3, XPathResultType.Boolean, new XPathResultType[] { XPathResultType.String, XPathResultType.String, XPathResultType.String}),
            new XsltFunctionEntry(string.Empty, FUNCTION_REGEX_MATCH, 2, 3, XPathResultType.String, new XPathResultType[] { XPathResultType.String, XPathResultType.String, XPathResultType.String}),
            new XsltFunctionEntry(string.Empty, FUNCTION_REGEX_REPLACE, 3, 4, XPathResultType.String, new XPathResultType[] { XPathResultType.String, XPathResultType.String, XPathResultType.String, XPathResultType.String}),
            new XsltFunctionEntry(string.Empty, FUNCTION_RESOLVE_URI, 1, 2, XPathResultType.String, new XPathResultType[] { XPathResultType.String, XPathResultType.String}),
            new XsltFunctionEntry(string.Empty, OPERATION_BOOL_EQUAL, 2, 2, XPathResultType.Boolean, new XPathResultType[] { XPathResultType.Boolean, XPathResultType.Boolean}),
            new XsltFunctionEntry(string.Empty, OPERATION_BOOL_LESS_THAN, 2, 2, XPathResultType.Boolean, new XPathResultType[] { XPathResultType.Boolean, XPathResultType.Boolean}),
            new XsltFunctionEntry(string.Empty, OPERATION_BOOL_GREATER_THAN, 2, 2, XPathResultType.Boolean, new XPathResultType[] { XPathResultType.Boolean, XPathResultType.Boolean})
        };



        internal XPath2Functions(XsltFunctionEntry functionDef)
            : base(functionDef)
        { }

        internal override object Invoke(
            XsltContext xsltContext,
            object[] args,
            XPathNavigator docContext)
        {
            #region Numeric Operations
            if (m_FunctionDef.FunctionName.Equals(VALUE_NAN))
                return NaN(args);

            if (m_FunctionDef.FunctionName.Equals(VALUE_INF))
                return INF(args);

            if (m_FunctionDef.FunctionName.Equals(OPERATION_NUMERIC_ADD))
                return NumericAdd(args);

            if (m_FunctionDef.FunctionName.Equals(OPERATION_NUMERIC_SUBTRACT))
                return NumericSubtract(args);

            if (m_FunctionDef.FunctionName.Equals(OPERATION_NUMERIC_MULTIPLY))
                return NumericMultiply(args);

            if (m_FunctionDef.FunctionName.Equals(OPERATION_NUMERIC_DIVIDE))
                return NumericDivide(args);

            if (m_FunctionDef.FunctionName.Equals(OPERATION_NUMERIC_INTEGER_DIVIDE))
                return NumericIntegerDivide(args);

            if (m_FunctionDef.FunctionName.Equals(OPERATION_NUMERIC_MOD))
                return NumericMod(args);

            if (m_FunctionDef.FunctionName.Equals(OPERATION_NUMERIC_UNARY_PLUS))
                return NumericUnaryPlus(args);

            if (m_FunctionDef.FunctionName.Equals(OPERATION_NUMERIC_UNARY_MINUS))
                return NumericUnaryMinus(args);

            if (m_FunctionDef.FunctionName.Equals(OPERATION_NUMERIC_EQUAL))
                return NumericEqual(args);

            if (m_FunctionDef.FunctionName.Equals(OPERATION_NUMERIC_LESS_THAN))
                return NumericLessThan(args);

            if (m_FunctionDef.FunctionName.Equals(OPERATION_NUMERIC_GREATER_THAN))
                return NumericGreaterThan(args);            
            #endregion

            #region Numeric Functions
            if (m_FunctionDef.FunctionName.Equals(FUNCTION_ABS))
                return Abs(args);

            if (m_FunctionDef.FunctionName.Equals(FUNCTION_CEILING))
                return Celing(args);

            if (m_FunctionDef.FunctionName.Equals(FUNCTION_FLOOR))
                return Floor(args);

            if (m_FunctionDef.FunctionName.Equals(FUNCTION_ROUND))
                return Round(args);

            if (m_FunctionDef.FunctionName.Equals(FUNCTION_ROUND_HALF_TO_EVEN))
                return RoundHalfToEven(args);
            #endregion

            #region String Functions
            if (m_FunctionDef.FunctionName.Equals(FUNCTION_STR_COMPARE))
                return StrCompare(args);

            if (m_FunctionDef.FunctionName.Equals(FUNCTION_STR_NORMALIZE_UNICODE))
                return StrNormalizeUnicode(args);

            if (m_FunctionDef.FunctionName.Equals(FUNCTION_STR_UPPER_CASE))
                return StrUpperCase(args);

            if (m_FunctionDef.FunctionName.Equals(FUNCTION_STR_LOWER_CASE))
                return StrLowerCase(args);

            if (m_FunctionDef.FunctionName.Equals(FUNCTION_STR_ENCODE_FOR_URI))
                return StrEncodeForUri(args);

            if (m_FunctionDef.FunctionName.Equals(FUNCTION_STR_IRI_TO_URI))
                return StrIriToUri(args);

            if (m_FunctionDef.FunctionName.Equals(FUNCTION_STR_ESCAPE_HTML_URI))
                return StrEscapeHtmlUri(args);

            if (m_FunctionDef.FunctionName.Equals(FUNCTION_RESOLVE_URI))
                return ResolveUri(args);
            #endregion

            #region Regular Expressions
            if (m_FunctionDef.FunctionName.Equals(FUNCTION_REGEX_MATCHES))
                return RegexMatches(args);

            if (m_FunctionDef.FunctionName.Equals(FUNCTION_REGEX_MATCH))
                return RegexMatch(args);

            if (m_FunctionDef.FunctionName.Equals(FUNCTION_REGEX_REPLACE))
                return RegexReplace(args);
            #endregion

            #region Boolean Operations
            if (m_FunctionDef.FunctionName.Equals(OPERATION_BOOL_EQUAL))
                return BoolEquals(args);

            if (m_FunctionDef.FunctionName.Equals(OPERATION_BOOL_LESS_THAN))
                return BoolLessThan(args);

            if (m_FunctionDef.FunctionName.Equals(OPERATION_BOOL_GREATER_THAN))
                return BoolGreaterThan(args);
            #endregion

            return null;
        }

        private static bool IsNumeric(object value)
        {
            if (value is Int16 ||
                value is Int32 ||
                value is Int64 ||
                value is float ||
                value is double)
            {
                return true;
            }

            return false;
        }

        //private bool IsInteger(object value)
        //{
        //    if (value is Int16 ||
        //        value is Int32 ||
        //        value is Int64)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        #region Numeric Operations Implementation

        private static double NaN(object[] args)
        {
            return double.NaN;
        }

        private static double INF(object[] args)
        {
            return double.PositiveInfinity;
        }

        private static double NumericAdd(object[] args)
        {
            // 6.2.1 op:numeric-add
            // op:numeric-add($arg1 as numeric, $arg2 as numeric) as numeric
            // Summary: Backs up the +ACIAKwAi- operator and returns the arithmetic sum of its operands: ($arg1 +- $arg2).

            // Note:

            // For xs:float or xs:double values, if one of the operands is a zero or a finite number and the other is 
            // INF or -INF, INF or -INF is returned. If both operands are INF, INF is returned. If both operands are -INF, -INF is returned. 
            // If one of the operands is INF and the other is -INF, NaN is returned.

            if (args == null || args.Length != 2)
                throw new ArgumentException("'numeric-add' expects 2 arguments.");
            
            if (!IsNumeric(args[0]))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'numeric-add': first argument '{0}' must be a number.", args[0]));

            if (!IsNumeric(args[1]))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'numeric-add': second argument '{0}' must be a number.", args[1]));

            double op1 = Convert.ToDouble(args[0], CultureInfo.InvariantCulture);
            double op2 = Convert.ToDouble(args[1], CultureInfo.InvariantCulture);

            return op1 + op2;
        }

        private static double NumericSubtract(object[] args)
        {
            // 6.2.2 op:numeric-subtract
            // op:numeric-subtract($arg1 as numeric, $arg2 as numeric) as numeric
            // Summary: Backs up the "-" operator and returns the arithmetic difference of its operands: ($arg1 - $arg2).

            // Note:

            // For xs:float or xs:double values, if one of the operands is a zero or a finite number and the other is 
            // INF or -INF, an infinity of the appropriate sign is returned. If both operands are INF or -INF, NaN is returned. 
            // If one of the operands is INF and the other is -INF, an infinity of the appropriate sign is returned.


            if (args == null || args.Length != 2)
                throw new ArgumentException("'numeric-subtract' expects 2 arguments.");

            if (!IsNumeric(args[0]))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'numeric-subtract': first argument '{0}' must be a number.", args[0]));

            if (!IsNumeric(args[1]))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'numeric-subtract': second argument '{0}' must be a number.", args[1]));

            double op1 = Convert.ToDouble(args[0], CultureInfo.InvariantCulture);
            double op2 = Convert.ToDouble(args[1], CultureInfo.InvariantCulture);

            return op1 - op2;
        }

        private static double NumericMultiply(object[] args)
        {
            // 6.2.3 op:numeric-multiply
            // op:numeric-multiply($arg1 as numeric, $arg2 as numeric) as numeric
            // Summary: Backs up the +ACIAKgAi- operator and returns the arithmetic product of its operands: ($arg1 * $arg2).

            // Note:

            // For xs:float or xs:double values, if one of the operands is a zero and the other is an infinity, NaN is returned. 
            // If one of the operands is a non-zero number and the other is an infinity, an infinity with the appropriate sign is returned.

            if (args == null || args.Length != 2)
                throw new ArgumentException("'numeric-multiply' expects 2 arguments.");

            if (!IsNumeric(args[0]))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'numeric-multiply': first argument '{0}' must be a number.", args[0]));

            if (!IsNumeric(args[1]))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'numeric-multiply': second argument '{0}' must be a number.", args[1]));

            double op1 = Convert.ToDouble(args[0], CultureInfo.InvariantCulture);
            double op2 = Convert.ToDouble(args[1], CultureInfo.InvariantCulture);

            return op1 * op2;
        }

        private static double NumericDivide(object[] args)
        {
            // 6.2.4 op:numeric-divide
            // op:numeric-divide($arg1 as numeric, $arg2 as numeric) as numeric
            // Summary: Backs up the "div" operator and returns the arithmetic quotient of its operands: ($arg1 div $arg2).

            // As a special case, if the types of both $arg1 and $arg2 are xs:integer, then the return type is xs:decimal.

            // Notes:

            // For xs:decimal and xs:integer operands, if the divisor is (positive or negative) zero, an error is raised [err:FOAR0001]. 
            // For xs:float and xs:double operands, floating point division is performed as specified in [IEEE 754-1985].

            // For xs:float or xs:double values, a positive number divided by positive zero returns INF. A negative number divided 
            // by positive zero returns -INF. Division by negative zero returns -INF and INF, respectively. Positive or negative 
            // zero divided by positive or negative zero returns NaN. Also, INF or -INF divided by INF or -INF returns NaN.

            if (args == null || args.Length != 2)
                throw new ArgumentException("'numeric-divide' expects 2 arguments.");

            if (!IsNumeric(args[0]))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'numeric-divide': first argument '{0}' must be a number.", args[0]));

            if (!IsNumeric(args[1]))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'numeric-divide': second argument '{0}' must be a number.", args[1]));

            double op1 = Convert.ToDouble(args[0], CultureInfo.InvariantCulture);
            double op2 = Convert.ToDouble(args[1], CultureInfo.InvariantCulture);

            return op1 / op2;
        }

        private static int NumericIntegerDivide(object[] args)
        {
            // 6.2.5 op:numeric-integer-divide
            // op:numeric-integer-divide($arg1 as numeric, $arg2 as numeric) as xs:integer
            // Summary: This function backs up the "idiv" operator and performs an integer division: that is, 
            // it divides the first argument by the second, and returns the integer obtained by truncating the fractional 
            // part of the result. The division is performed so that the sign of the fractional part is the same as the sign of the dividend.

            // If the dividend, $arg1, is not evenly divided by the divisor, $arg2, then the quotient is the xs:integer 
            // value obtained, ignoring (truncating) any remainder that results from the division (that is, no rounding is performed).
            // Thus, the semantics " $a idiv $b " are equivalent to " ($a div $b) cast as xs:integer " except for error situations.

            // If the divisor is (positive or negative) zero, then an error is raised [err:FOAR0001]. If either operand 
            // is NaN or if $arg1 is INF or -INF then an error is raised [err:FOAR0002].

            //Note:

            //The semantics of this function are different from integer division as defined in programming languages such as Java and C+-+-.

            // 6.2.5.1 Examples
            // op:numeric-integer-divide(10,3) returns 3
            // op:numeric-integer-divide(3,-2) returns -1
            // op:numeric-integer-divide(-3,2) returns -1
            // op:numeric-integer-divide(-3,-2) returns 1
            // op:numeric-integer-divide(9.0,3) returns 3
            // op:numeric-integer-divide(-3.5,3) returns -1
            // op:numeric-integer-divide(3.0,4) returns 0
            // op:numeric-integer-divide(3.1E1,6) returns 5
            // op:numeric-integer-divide(3.1E1,7) returns 4

            if (args == null || args.Length != 2)
                throw new ArgumentException("'numeric-integer-divide' expects 2 arguments.");

            if (!IsNumeric(args[0]))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'numeric-integer-divide': first argument '{0}' must be a number.", args[0]));

            if (!IsNumeric(args[1]))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'numeric-integer-divide': second argument '{0}' must be a number.", args[1]));

            double op1 = Convert.ToDouble(args[0], CultureInfo.InvariantCulture);
            double op2 = Convert.ToDouble(args[1], CultureInfo.InvariantCulture);

            if (op2 == 0)
                throw new XPathException("err:FOAR0001: The divisor cannot be zero.");

            if (double.IsNaN(op1) || double.IsNaN(op2))
                throw new XPathException("err:FOAR0002: The arguments cannot be NaN.");

            if (double.IsInfinity(op1))
                throw new XPathException("err:FOAR0002: The dividend cannot be infinity.");

            double dblRes = op1 / op2;
            return (int)dblRes;
        }

        private static double NumericMod(object[] args)
        {
            // 6.2.6 op:numeric-mod
            // op:numeric-mod($arg1 as numeric, $arg2 as numeric) as numeric
            // Summary: Backs up the "mod" operator. Informally, this function returns the remainder resulting from 
            // dividing $arg1, the dividend, by $arg2, the divisor. The operation a mod b for operands that are xs:integer 
            // or xs:decimal, or types derived from them, produces a result such that (a idiv b)*b+-(a mod b) is equal to a
            // and the magnitude of the result is always less than the magnitude of b. This identity holds even in the special 
            // case that the dividend is the negative integer of largest possible magnitude for its type and the divisor is -1 
            // (the remainder is 0). It follows from this rule that the sign of the result is the sign of the dividend.

            // For xs:integer and xs:decimal operands, if $arg2 is zero, then an error is raised [err:FOAR0001].

            // For xs:float and xs:double operands the following rules apply:
            // If either operand is NaN, the result is NaN.
            // If the dividend is positive or negative infinity, or the divisor is positive or negative zero (0), or both, the result is NaN.
            // If the dividend is finite and the divisor is an infinity, the result equals the dividend.
            // If the dividend is positive or negative zero and the divisor is finite, the result is the same as the dividend.

            // In the remaining cases, where neither positive or negative infinity, nor positive or negative zero, nor NaN is involved, 
            // the result obeys (a idiv b)*b+-(a mod b) = a. Division is truncating division, analogous to integer division, not 
            // [IEEE 754-1985] rounding division i.e. additional digits are truncated, not rounded to the required precision.

            // 6.2.6.1 Examples
            // op:numeric-mod(10,3) returns 1.
            // op:numeric-mod(6,-2) returns 0.
            // op:numeric-mod(4.5,1.2) returns 0.9.
            // op:numeric-mod(1.23E2, 0.6E1) returns 3.0E0.

            if (args == null || args.Length != 2)
                throw new ArgumentException("'numeric-mod' expects 2 arguments.");

            if (!IsNumeric(args[0]))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'numeric-mod': first argument '{0}' must be a number.", args[0]));

            if (!IsNumeric(args[1]))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'numeric-mod': second argument '{0}' must be a number.", args[1]));

            double op1 = Convert.ToDouble(args[0], CultureInfo.InvariantCulture);
            double op2 = Convert.ToDouble(args[1], CultureInfo.InvariantCulture);

            if (double.IsNaN(op1) || double.IsNaN(op2))
                return double.NaN;

            if (double.IsInfinity(op1) || op2 == 0)
                return double.NaN;

            if (!double.IsInfinity(op1) && !double.IsNaN(op1) && double.IsInfinity(op2))
                return op1;

            double dblRes = op1 / op2;

            return op1 - (op2 * (int)dblRes);
        }

        private static double NumericUnaryPlus(object[] args)
        {
            // 6.2.7 op:numeric-unary-plus
            // op:numeric-unary-plus($arg as numeric) as numeric
            // Summary: Backs up the unary +ACIAKwAi- operator and returns its operand with the sign unchanged: (+- $arg). 
            // Semantically, this operation performs no operation.
            if (args == null || args.Length != 1)
                throw new ArgumentException("'numeric-unary-plus' expects 1 argument.");

            if (!IsNumeric(args[0]))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'numeric-unary-plus': first argument '{0}' must be a number.", args[0]));

            double op1 = Convert.ToDouble(args[0], CultureInfo.InvariantCulture);

            return op1;
        }

        private static double NumericUnaryMinus(object[] args)
        {
            // 6.2.8 op:numeric-unary-minus
            // op:numeric-unary-minus($arg as numeric) as numeric
            // Summary: Backs up the unary "-" operator and returns its operand with the sign reversed: (- $arg). 
            // If $arg is positive, its negative is returned; if it is negative, its positive is returned.

            // For xs:integer and xs:decimal arguments, 0 and 0.0 return 0 and 0.0, respectively. 
            // For xs:float and xs:double arguments, NaN returns NaN, 0.0E0 returns -0.0E0 and vice versa.
            // INF returns -INF. -INF returns INF.

            if (args == null || args.Length != 1)
                throw new ArgumentException("'numeric-unary-minus' expects 1 argument.");

            if (!IsNumeric(args[0]))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'numeric-unary-minus': first argument '{0}' must be a number.", args[0]));

            double op1 = Convert.ToDouble(args[0], CultureInfo.InvariantCulture);

            return -op1;
        }

        private static bool NumericEqual(object[] args)
        {
            // 6.3.1 op:numeric-equal
            // op:numeric-equal($arg1 as numeric, $arg2 as numeric) as xs:boolean
            // Summary: Returns true if and only if the value of $arg1 is equal to the value of $arg2. 
            // For xs:float and xs:double values, positive zero and negative zero compare equal. 
            // INF equals INF and -INF equals -INF. NaN does not equal itself.

            // This function backs up the "eq", "ne", "le" and "ge" operators on numeric values.

            if (args == null || args.Length != 2)
                throw new ArgumentException("'numeric-equal' expects 2 arguments.");

            if (!IsNumeric(args[0]))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'numeric-equal': first argument '{0}' must be a number.", args[0]));

            if (!IsNumeric(args[1]))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'numeric-equal': second argument '{0}' must be a number.", args[1]));

            double op1 = Convert.ToDouble(args[0], CultureInfo.InvariantCulture);
            double op2 = Convert.ToDouble(args[1], CultureInfo.InvariantCulture);

            if (double.IsNaN(op1) && double.IsNaN(op2))
                return false;

            return op1.Equals(op2);
        }

        private static bool NumericLessThan(object[] args)
        {
            // 6.3.2 op:numeric-less-than
            // op:numeric-less-than($arg1 as numeric, $arg2 as numeric) as xs:boolean
            // Summary: Returns true if and only if $arg1 is less than $arg2. For xs:float and xs:double values, 
            // positive infinity is greater than all other non-NaN values; negative infinity is less than all other
            // non-NaN values. If $arg1 or $arg2 is NaN, the function returns false.

            // This function backs up the "lt" and "le" operators on numeric values.

            if (args == null || args.Length != 2)
                throw new ArgumentException("'numeric-less-than' expects 2 arguments.");

            if (!IsNumeric(args[0]))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'numeric-less-than': first argument '{0}' must be a number.", args[0]));

            if (!IsNumeric(args[1]))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'numeric-less-than': second argument '{0}' must be a number.", args[1]));

            double op1 = Convert.ToDouble(args[0], CultureInfo.InvariantCulture);
            double op2 = Convert.ToDouble(args[1], CultureInfo.InvariantCulture);

            if (double.IsNaN(op1) || double.IsNaN(op2))
                return false;

            return op1 < op2;
        }

        private static bool NumericGreaterThan(object[] args)
        {
            // 6.3.3 op:numeric-greater-than
            // op:numeric-greater-than($arg1 as numeric, $arg2 as numeric) as xs:boolean
            // Summary: Returns true if and only if $arg1 is greater than $arg2. For xs:float and xs:double values, 
            // positive infinity is greater than all other non-NaN values; negative infinity is less than all other non-NaN values.
            // If $arg1 or $arg2 is NaN, the function returns false.

            // This function backs up the "gt" and "ge" operators on numeric values.

            if (args == null || args.Length != 2)
                throw new ArgumentException("'numeric-greater-than' expects 2 arguments.");

            if (!IsNumeric(args[0]))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'numeric-greater-than': first argument '{0}' must be a number.", args[0]));

            if (!IsNumeric(args[1]))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'numeric-greater-than': second argument '{0}' must be a number.", args[1]));

            double op1 = Convert.ToDouble(args[0], CultureInfo.InvariantCulture);
            double op2 = Convert.ToDouble(args[1], CultureInfo.InvariantCulture);

            if (double.IsNaN(op1) || double.IsNaN(op2))
                return false;

            return op1 > op2;
        }
        #endregion

        #region Numeric Functions Implementation
        private static double Abs(object[] args)
        {
            // 6.4.1 fn:abs
            // fn:abs($arg as numeric?) as numeric?
            // Summary: Returns the absolute value of $arg. If $arg is negative returns -$arg otherwise returns $arg. 
            // If type of $arg is one of the four numeric types xs:float, xs:double, xs:decimal or xs:integer the type 
            // of the result is the same as the type of $arg. If the type of $arg is a type derived from one of the numeric
            // types, the result is an instance of the base numeric type.

            // For xs:float and xs:double arguments, if the argument is positive zero or negative zero, then positive zero
            // is returned. If the argument is positive or negative infinity, positive infinity is returned.

            // For detailed type semantics, see Section 7.2.3 The fn:abs, fn:ceiling, fn:floor, fn:round, and fn:round-half-to-even functionsFS

            // 6.4.1.1 Examples
            // fn:abs(10.5) returns 10.5.
            // fn:abs(-10.5) returns 10.5.

            if (args == null || args.Length != 1)
                throw new ArgumentException("'abs' expects 1 argument.");

            if (!IsNumeric(args[0]))
                return double.NaN;

            double op1 = Convert.ToDouble(args[0], CultureInfo.InvariantCulture);

            return Math.Abs(op1);
        }

        private static double Celing(object[] args)
        {
            // 6.4.2 fn:ceiling
            // fn:ceiling($arg as numeric?) as numeric?
            // Summary: Returns the smallest (closest to negative infinity) number with no fractional part that is 
            // not less than the value of $arg. If type of $arg is one of the four numeric types xs:float, xs:double, 
            // xs:decimal or xs:integer the type of the result is the same as the type of $arg. If the type of $arg is a 
            // type derived from one of the numeric types, the result is an instance of the base numeric type.

            // For xs:float and xs:double arguments, if the argument is positive zero, then positive zero is returned. 
            // If the argument is negative zero, then negative zero is returned. If the argument is less than zero and greater than -1,
            // negative zero is returned.

            // For detailed type semantics, see Section 7.2.3 The fn:abs, fn:ceiling, fn:floor, fn:round, and fn:round-half-to-even functionsFS

            // 6.4.2.1 Examples
            // fn:ceiling(10.5) returns 11.
            // fn:ceiling(-10.5) returns -10.

            if (args == null || args.Length != 1)
                throw new ArgumentException("'ceiling' expects 1 argument.");

            if (!IsNumeric(args[0]))
                return double.NaN;

            double op1 = Convert.ToDouble(args[0], CultureInfo.InvariantCulture);

            return Math.Ceiling(op1);
        }

        private static double Floor(object[] args)
        {
            // 6.4.3 fn:floor
            // fn:floor($arg as numeric?) as numeric?
            // Summary: Returns the largest (closest to positive infinity) number with no fractional part that is not greater 
            // than the value of $arg. If type of $arg is one of the four numeric types xs:float, xs:double, xs:decimal or 
            // xs:integer the type of the result is the same as the type of $arg. If the type of $arg is a type derived from one 
            // of the numeric types, the result is an instance of the base numeric type.

            // For float and double arguments, if the argument is positive zero, then positive zero is returned. If the argument 
            // is negative zero, then negative zero is returned.

            // For detailed type semantics, see Section 7.2.3 The fn:abs, fn:ceiling, fn:floor, fn:round, and fn:round-half-to-even functionsFS

            // 6.4.3.1 Examples
            // fn:floor(10.5) returns 10.
            // fn:floor(-10.5) returns -11.

            if (args == null || args.Length != 1)
                throw new ArgumentException("'floor' expects 1 argument.");

            if (!IsNumeric(args[0]))
                return double.NaN;

            double op1 = Convert.ToDouble(args[0], CultureInfo.InvariantCulture);

            return Math.Floor(op1);
        }

        private static double Round(object[] args)
        {
            // 6.4.4 fn:round
            // fn:round($arg as numeric?) as numeric?
            // Summary: Returns the number with no fractional part that is closest to the argument. If there are two such numbers,
            // then the one that is closest to positive infinity is returned. If type of $arg is one of the four numeric types xs:float, 
            // xs:double, xs:decimal or xs:integer the type of the result is the same as the type of $arg. If the type of $arg is a type 
            // derived from one of the numeric types, the result is an instance of the base numeric type.

            // For xs:float and xs:double arguments, if the argument is positive infinity, then positive infinity is returned. 
            // If the argument is negative infinity, then negative infinity is returned. If the argument is positive zero, then positive 
            // zero is returned. If the argument is negative zero, then negative zero is returned. If the argument is less than zero,
            // but greater than or equal to -0.5, then negative zero is returned. In the cases where positive zero or negative zero
            // is returned, negative zero or positive zero may be returned as [XML Schema Part 2: Datatypes Second Edition] does not 
            // distinguish between the values positive zero and negative zero.

            // For the last two cases, note that the result is not the same as fn:floor(x+-0.5).

            // For detailed type semantics, see Section 7.2.3 The fn:abs, fn:ceiling, fn:floor, fn:round, and fn:round-half-to-even functionsFS

            // 6.4.4.1 Examples
            // fn:round(2.5) returns 3.
            // fn:round(2.4999) returns 2.
            // fn:round(-2.5) returns -2 (not the possible alternative, -3).

            if (args == null || args.Length != 1)
                throw new ArgumentException("'round' expects 1 argument.");

            if (!IsNumeric(args[0]))
                return double.NaN;

            double op1 = Convert.ToDouble(args[0], CultureInfo.InvariantCulture);

            if (op1 > 0)
                return Math.Round(op1, MidpointRounding.AwayFromZero);
            else
                return Math.Round(op1);
        }

        private static object RoundHalfToEven(object[] args)
        {
            // 6.4.5 fn:round-half-to-even
            // fn:round-half-to-even($arg as numeric?) as numeric?
            // fn:round-half-to-even($arg as numeric?, $precision as xs:integer) as numeric?
            // Summary: The value returned is the nearest (that is, numerically closest) value to $arg that is a multiple of ten 
            // to the power of minus $precision. If two such values are equally near (e.g. if the fractional part in $arg is 
            // exactly .500...), the function returns the one whose least significant digit is even.

            // If the type of $arg is one of the four numeric types xs:float, xs:double, xs:decimal or xs:integer the type
            // of the result is the same as the type of $arg. If the type of $arg is a type derived from one of the numeric types, 
            // the result is an instance of the base numeric type.

            // The first signature of this function produces the same result as the second signature with $precision=0.

            // For arguments of type xs:float and xs:double, if the argument is NaN, positive or negative zero, or positive or 
            // negative infinity, then the result is the same as the argument. In all other cases, the argument is cast to xs:decimal,
            // the function is applied to this xs:decimal value, and the resulting xs:decimal is cast back to xs:float or xs:double 
            // as appropriate to form the function result. If the resulting xs:decimal value is zero, then positive or negative zero 
            // is returned according to the sign of the original argument.

            // Note that the process of casting to xs:decimal may result in an error [err:FOCA0001].

            // If $arg is of type xs:float or xs:double, rounding occurs on the value of the mantissa computed with exponent = 0.

            // For detailed type semantics, see Section 7.2.3 The fn:abs, fn:ceiling, fn:floor, fn:round, and fn:round-half-to-even functionsFS

            // Note:

            // This function is typically used in financial applications where the argument is of type xs:decimal. For arguments of type 
            // xs:float and xs:double the results may be counterintuitive. For example, consider round-half-to-even(xs:float(150.0150), 2).

            // An implementation that supports 18 digits for xs:decimal will convert the argument to the xs:decimal 150.014999389... 
            // which will then be rounded to the xs:decimal 150.01 which will be converted back to the xs:float whose exact value 
            // is 150.0099945068... whereas round-half-to-even(xs:decimal(150.0150), 2) will result in the xs:decimal whose exact value is 150.02.

            // 6.4.5.1 Examples
            // fn:round-half-to-even(0.5) returns 0.
            // fn:round-half-to-even(1.5) returns 2.
            // fn:round-half-to-even(2.5) returns 2.
            // fn:round-half-to-even(3.567812E+-3, 2) returns 3567.81E0.
            // fn:round-half-to-even(4.7564E-3, 2) returns 0.0E0.
            // fn:round-half-to-even(35612.25, -2) returns 35600.

            if (args == null || args.Length < 1 || args.Length > 2)
                throw new ArgumentException("'round-half-to-even' expects 1 or 2 argument(s).");

            if (!IsNumeric(args[0]))
                return double.NaN;

            double op1dbl = Convert.ToDouble(args[0], CultureInfo.InvariantCulture);
            if (double.IsNaN(op1dbl)) return double.NaN;
            if (double.IsPositiveInfinity(op1dbl)) return double.PositiveInfinity;
            if (double.IsNegativeInfinity(op1dbl)) return double.NegativeInfinity;

            decimal op1 = Convert.ToDecimal(args[0], CultureInfo.InvariantCulture);
            int precision = 0;
            if (args.Length == 2)
            {
                if (!IsNumeric(args[1]))
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'round-half-to-even': argument 2 '{0} must be integer.'", args[1]));

                precision = Convert.ToInt32(args[1], CultureInfo.InvariantCulture);

                if (precision < 0)
                {
                    op1 = decimal.Truncate(decimal.Truncate(op1) / (int)Math.Pow(10, Math.Abs(precision))) * (int)Math.Pow(10, Math.Abs(precision));
                    return op1;
                }
            }

            return Math.Round(op1, precision);
        }
        #endregion

        #region String Functions Implementation
        private static int StrCompare(object[] args)
        {
            // 7.3.2 fn:compare
            // fn:compare($comparand1 as xs:string?, $comparand2 as xs:string?) as xs:integer?
            // fn:compare( $comparand1  as xs:string?, 
            // $comparand2  as xs:string?, 
            // $collation  as xs:string) as xs:integer? 

            // Summary: Returns -1, 0, or 1, depending on whether the value of the $comparand1 is respectively less than,
            // equal to, or greater than the value of $comparand2, according to the rules of the collation that is used.

            // The collation used by the invocation of this function is determined according to the rules in 7.3.1 Collations.

            // If either argument is the empty sequence, the result is the empty sequence.

            // This function, invoked with the first signature, backs up the "eq", "ne", "gt", "lt", "le" and "ge" operators on string values.

            // 7.3.2.1 Examples
            // fn:compare('abc', 'abc') returns 0.
            // fn:compare('Strasse', 'Stra+AN8-e') returns 0 if and only if the default collation includes provisions 
            //      that equate "ss" and the (German) character +ACIA3wAi- ("sharp-s"). (Otherwise, the returned value depends on the
            //      semantics of the default collation.)
            // fn:compare('Strasse', 'Stra+AN8-e', 'deutsch') returns 0 if the collation identified by the relative URI constructed 
            //      from the string value "deutsch" includes provisions that equate "ss" and the (German) character +ACIA3wAi- ("sharp-s"). 
            //      (Otherwise, the returned value depends on the semantics of that collation.)
            // fn:compare('Strassen', 'Stra+AN8-e') returns 1 if the default collation includes provisions that treat
            //      differences between "ss" and the (German) character +ACIA3wAi- ("sharp-s") with less strength than the differences
            //      between the base characters, such as the final "n".

            if (args == null || args.Length == 3)
                throw new ArgumentException("'compare' expects 2 arguments. Collation is not supported.");

            if (args == null || args.Length != 2)
                throw new ArgumentException("'compare' expects 2 arguments.");

            string op1 = Convert.ToString(args[0], CultureInfo.InvariantCulture);
            string op2 = Convert.ToString(args[1], CultureInfo.InvariantCulture);

            return op1.CompareTo(op2);
        }

        private static string StrNormalizeUnicode(object[] args)
        {
            // 7..4.6 fn:normalize-unicode
            // fn:normalize-unicode($arg as xs:string?) as xs:string
            // fn:normalize-unicode( $arg  as xs:string?, 
            // $normalizationForm  as xs:string) as xs:string 

            // Summary: Returns the value of $arg normalized according to the normalization criteria for a normalization form 
            // identified by the value of $normalizationForm. The effective value of the $normalizationForm is computed by removing 
            // leading and trailing blanks, if present, and converting to upper case.

            // If the value of $arg is the empty sequence, returns the zero-length string.

            // See [Character Model for the World Wide Web 1.0: Normalization] for a description of the normalization forms.

            // If the $normalizationForm is absent, as in the first format above, it shall be assumed to be "NFC"

            // If the effective value of $normalizationForm is "NFC", then the value returned by the function is the value of 
            // $arg in Unicode Normalization Form C (NFC).

            // If the effective value of $normalizationForm is "NFD", then the value returned by the function is the value of 
            // $arg in Unicode Normalization Form D (NFD).

            // If the effective value of $normalizationForm is "NFKC", then the value returned by the function is the value of
            // $arg in Unicode Normalization Form KC (NFKC).

            // If the effective value of $normalizationForm is "NFKD", then the value returned by the function is the value of 
            // $arg in Unicode Normalization Form KD (NFKD).

            // If the effective value of $normalizationForm is "FULLY-NORMALIZED", then the value returned by the function is
            // the value of $arg in the fully normalized form.

            // If the effective value of $normalizationForm is the zero-length string, no normalization is performed and $arg is returned.

            // Conforming implementations +ALc-must+ALc- support normalization form "NFC" and +ALc-may+ALc- support normalization forms 
            // "NFD", "NFKC", "NFKD", "FULLY-NORMALIZED". They +ALc-may+ALc- also support other normalization forms with +ALc-implementation-defined+ALc-
            // semantics. If the effective value of the $normalizationForm is other than one of the values supported by the implementation,
            // then an error is raised [err:FOCH0003].


            throw new NotImplementedException("fn:normalize-unicode() is not implemented.");
        }

        private static string StrUpperCase(object[] args)
        {
            // 7.4.7 fn:upper-case
            // fn:upper-case($arg as xs:string?) as xs:string
            // Summary: Returns the value of $arg after translating every character to its upper-case correspondent as defined in 
            // the appropriate case mappings section in the Unicode standard [The Unicode Standard]. For versions of Unicode beginning
            // with the 2.1.8 update, only locale-insensitive case mappings should be applied. Beginning with version 3.2.0 
            // (and likely future versions) of Unicode, precise mappings are described in default case operations, which are full 
            // case mappings in the absence of tailoring for particular languages and environments. Every lower-case character that
            // does not have an upper-case correspondent, as well as every upper-case character, is included in the returned value
            // in its original form.

            // If the value of $arg is the empty sequence, the zero-length string is returned.

            // Note:

            // Case mappings may change the length of a string. In general, the two functions are not inverses of each other 
            // fn:lower-case(fn:upper-case($arg)) is not guaranteed to return $arg, nor is fn:upper-case(fn:lower-case($arg)).
            // The Latin small letter dotless i (as used in Turkish) is perhaps the most prominent lower-case letter which will 
            // not round-trip. The Latin capital letter i with dot above is the most prominent upper-case letter which will not round trip;
            // there are others.

            // These functions may not always be linguistically appropriate (e.g. Turkish i without dot) or appropriate for the application 
            // (e.g. titlecase). In cases such as Turkish, a simple translation should be used first.

            // Results may violate user expectations (in Quebec, for example, the standard uppercase equivalent of +ACIA6AAi- is +ACIAyAAi-, 
            // while in metropolitan France it is more commonly "E"; only one of these is supported by the functions as defined).

            // Many characters of class Ll lack uppercase equivalents in the Unicode case mapping tables; many characters of class
            // Lu lack lowercase equivalents.

            // 7.4.7.1 Examples
            // fn:upper-case("abCd0") returns "ABCD0".

            if (args == null || args.Length != 1)
                throw new ArgumentException("'upper-case' expects 1 argument.");

            string op1 = Convert.ToString(args[0], CultureInfo.InvariantCulture);

            if (op1 != null)
                return op1.ToUpper(CultureInfo.InvariantCulture);
            else
                return null;
        }

        private static string StrLowerCase(object[] args)
        {
            // 7.4.8 fn:lower-case
            // fn:lower-case($arg as xs:string?) as xs:string
            // Summary: Returns the value of $arg after translating every character to its lower-case correspondent as 
            // defined in the appropriate case mappings section in the Unicode standard [The Unicode Standard]. For versions 
            // of Unicode beginning with the 2.1.8 update, only locale-insensitive case mappings should be applied. Beginning with 
            // version 3.2.0 (and likely future versions) of Unicode, precise mappings are described in default case operations, 
            // which are full case mappings in the absence of tailoring for particular languages and environments. Every upper-case
            // character that does not have a lower-case correspondent, as well as every lower-case character, is included in the 
            // returned value in its original form.

            // If the value of $arg is the empty sequence, the zero-length string is returned.

            // Note:

            // Case mappings may change the length of a string. In general, the two functions are not inverses of each other 
            // fn:lower-case(fn:upper-case($arg)) is not guaranteed to return $arg, nor is fn:upper-case(fn:lower-case($arg)). 
            // The Latin small letter dotless i (as used in Turkish) is perhaps the most prominent lower-case letter which will 
            // not round-trip. The Latin capital letter i with dot above is the most prominent upper-case letter which will not round trip;
            // there are others.

            // These functions may not always be linguistically appropriate (e.g. Turkish i without dot) or appropriate for the application
            // (e.g. titlecase). In cases such as Turkish, a simple translation should be used first.

            // Results may violate user expectations (in Quebec, for example, the standard uppercase equivalent of +ACIA6AAi- is +ACIAyAAi-, 
            // while in metropolitan France it is more commonly "E"; only one of these is supported by the functions as defined).

            // Many characters of class Ll lack uppercase equivalents in the Unicode case mapping tables; many characters of class 
            // Lu lack lowercase equivalents.

            // 7.4.8.1 Examples
            // fn:lower-case("ABc!D") returns "abc!d".

            if (args == null || args.Length != 1)
                throw new ArgumentException("'lower-case' expects 1 argument.");

            string op1 = Convert.ToString(args[0], CultureInfo.InvariantCulture);

            if (op1 != null)
                return op1.ToLower(CultureInfo.InvariantCulture);
            else
                return null;
        }

        private static string StrEncodeForUri(object[] args)
        {
            // 7.4.10 fn:encode-for-uri
            // fn:encode-for-uri($uri-part as xs:string?) as xs:string
            // Summary: This function encodes reserved characters in an xs:string that is intended to be used in the path segment 
            // of a URI. It is invertible but not idempotent. This function applies the URI escaping rules defined in section 2 
            // of [RFC 3986] to the xs:string supplied as $uri-part. The effect of the function is to escape reserved characters. 
            // Each such character in the string is replaced with its percent-encoded form as described in [RFC 3986].

            // If $uri-part is the empty sequence, returns the zero-length string.

            // All characters are escaped except those identified as "unreserved" by [RFC 3986], that is the upper- and lower-case 
            // letters A-Z, the digits 0-9, HYPHEN-MINUS ("-"), LOW LINE (+ACIAXwAi-), FULL STOP ".", and TILDE +ACIAfgAi-.

            // Note that this function escapes URI delimiters and therefore cannot be used indiscriminately to encode "invalid" 
            // characters in a path segment.

            // Since [RFC 3986] recommends that, for consistency, URI producers and normalizers should use uppercase hexadecimal 
            // digits for all percent-encodings, this function must always generate hexadecimal values using the upper-case letters A-F.

            // 7.4.10.1 Examples
            // fn:encode-for-uri("http://www.example.com/00/Weather/CA/Los%20Angeles#ocean") returns "http%3A%2F%2Fwww.example.com%2F00%2FWeather%2FCA%2FLos%2520Angeles%23ocean". This is probably not what the user intended because all of the delimiters have been encoded.
            // concat("http://www.example.com/", encode-for-uri(+ACIAfg-b+AOk-b+AOkAIg-)) returns "http://www.example.com/+AH4-b%C3%A9b%C3%A9".
            // concat("http://www.example.com/", encode-for-uri("100% organic")) returns "http://www.example.com/100%25%20organic".

            if (args == null || args.Length != 1)
                throw new ArgumentException("'encode-for-uri' expects 1 argument.");

            string op1 = args[0] is string ? (string)args[0] : Convert.ToString(args[0], CultureInfo.InvariantCulture);
            StringBuilder escaped = new StringBuilder();
            foreach (char ch in op1)
            {
                if ("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_.~".IndexOf(ch) == -1)
                {
                    int chasint = (int)ch;
                    int ch1 = chasint / 256;
                    int ch2 = chasint % 256;
                    if (ch1 != 0)
                    {
                        escaped.Append("%");
                        escaped.Append(Convert.ToString(ch1, 16).ToUpper(CultureInfo.InvariantCulture));
                    }

                    escaped.Append("%");
                    escaped.Append(Convert.ToString(ch2, 16).ToUpper(CultureInfo.InvariantCulture));

                }
                else
                    escaped.Append(ch);
            }

            return escaped.ToString();
        }

        private static string StrIriToUri(object[] args)
        {
            // 7.4.11 fn:iri-to-uri
            // fn:iri-to-uri($iri as xs:string?) as xs:string
            // Summary: This function converts an xs:string containing an IRI into a URI according to the rules spelled out in 
            // Section 3.1 of [RFC 3987]. It is idempotent but not invertible.

            // If $iri contains a character that is invalid in an IRI, such as the space character (see note below), the invalid 
            // character is replaced by its percent-encoded form as described in [RFC 3986] before the conversion is performed.

            // If $iri is the empty sequence, returns the zero-length string.

            // Since [RFC 3986] recommends that, for consistency, URI producers and normalizers should use uppercase hexadecimal 
            // digits for all percent-encodings, this function must always generate hexadecimal values using the upper-case letters A-F.

            // Notes:

            // This function does not check whether $iri is a legal IRI. It treats it as an xs:string and operates on the characters
            // in the xs:string.

            // The following printable ASCII characters are invalid in an IRI: "<", ">", " " " (double quote), space, "{", "}", 
            // "|", "\", "^", and "`". Since these characters should not appear in an IRI, if they do appear in $iri they will be 
            // percent-encoded. In addition, characters outside the range x20-x126 will be percent-encoded because they are invalid in a URI.

            // Since this function does not escape the PERCENT SIGN "%" and this character is not allowed in data within a URI, 
            // users wishing to convert character strings, such as file names, that include "%" to a URI should manually escape "%"
            // by replacing it with "%25".

            // 7.4.11.1 Examples
            // fn:iri-to-uri ("http://www.example.com/00/Weather/CA/Los%20Angeles#ocean") returns "http://www.example.com/00/Weather/CA/Los%20Angeles#ocean".
            // fn:iri-to-uri ("http://www.example.com/~bébé") returns "http://www.example.com/~b%C3%A9b%C3%A9".


            if (args == null || args.Length != 1)
                throw new ArgumentException("'iri-to-uri' expects 1 argument.");

            string op1 = args[0] is string ? (string)args[0] : Convert.ToString(args[0], CultureInfo.InvariantCulture);
            StringBuilder escaped = new StringBuilder();
            foreach (char ch in op1)
            {
                if ("<>\" {}|\\^`".IndexOf(ch) != -1 ||
                    (ch < '\x14' || ch > '\x7E') )
                {
                    int chasint = (int)ch;
                    int ch1 = chasint / 256;
                    int ch2 = chasint % 256;
                    if (ch1 != 0)
                    {
                        escaped.Append("%");
                        escaped.Append(Convert.ToString(ch1, 16).ToUpper(CultureInfo.InvariantCulture));
                    }

                    escaped.Append("%");
                    escaped.Append(Convert.ToString(ch2, 16).ToUpper(CultureInfo.InvariantCulture));

                }
                else
                    escaped.Append(ch);
            }

            return escaped.ToString();
        }

        private static string StrEscapeHtmlUri(object[] args)
        {
            // 7.4.12 fn:escape-html-uri
            // fn:escape-html-uri($uri as xs:string?) as xs:string
            // Summary: This function escapes all characters except printable characters of the US-ASCII coded character set, 
            // specifically the octets ranging from 32 to 126 (decimal). The effect of the function is to escape a URI in the 
            // manner html user agents handle attribute values that expect URIs. Each character in $uri to be escaped is replaced 
            // by an escape sequence, which is formed by encoding the character as a sequence of octets in UTF-8, and then
            // representing each of these octets in the form %HH, where HH is the hexadecimal representation of the octet. 
            // This function must always generate hexadecimal values using the upper-case letters A-F.

            // If $uri is the empty sequence, returns the zero-length string.

            // Note:

            // The behavior of this function corresponds to the recommended handling of non-ASCII characters in URI attribute 
            // values as described in [HTML 4.0] Appendix B.2.1.

            // 7.4.12.1 Examples
            // fn:escape-html-uri ("http://www.example.com/00/Weather/CA/Los Angeles#ocean") returns "http://www.example.com/00/Weather/CA/Los Angeles#ocean".
            // fn:escape-html-uri ("javascript:if (navigator.browserLanguage == 'fr') window.open('http://www.example.com/~bébé');") returns "javascript:if (navigator.browserLanguage == 'fr') window.open('http://www.example.com/~b%C3%A9b%C3%A9');".

            
            if (args == null || args.Length != 1)
                throw new ArgumentException("'escape-html-uri' expects 1 argument.");

            string op1 = args[0] is string ? (string)args[0] : Convert.ToString(args[0], CultureInfo.InvariantCulture);
            StringBuilder escaped = new StringBuilder();
            foreach (char ch in op1)
            {
                if (ch < '\x20' || ch > '\x7E')
                {
                    int chasint = (int)ch;
                    int ch1 = chasint / 256;
                    int ch2 = chasint % 256;
                    if (ch1 != 0)
                    {
                        escaped.Append("%");
                        escaped.Append(Convert.ToString(ch1, 16).ToUpper(CultureInfo.InvariantCulture));
                    }

                    escaped.Append("%");
                    escaped.Append(Convert.ToString(ch2, 16).ToUpper(CultureInfo.InvariantCulture));

                }
                else
                    escaped.Append(ch);
            }

            return escaped.ToString();
        }

        private static string ResolveUri(object[] args)
        {
            // 8.1 fn:resolve-uri
            // fn:resolve-uri($relative as xs:string?) as xs:anyURI?
            // fn:resolve-uri($relative as xs:string?, $base as xs:string) as xs:anyURI?
            // Summary: The purpose of this function is to enable a relative URI to be resolved against an absolute URI.

            // The first form of this function resolves $relative against the value of the base-uri property from the static context. 
            // If the base-uri property is not initialized in the static context an error is raised [err:FONS0005].

            // If $relative is a relative URI reference, it is resolved against $base, or the base-uri property from the static 
            // context, using an algorithm such as the ones described in [RFC 2396] or [RFC 3986], and the resulting absolute URI 
            // reference is returned. An error may be raised [err:FORG0009] in the resolution process.

            // If $relative is an absolute URI reference, it is returned unchanged.

            // If $relative or $base is not a valid xs:anyURI an error is raised [err:FORG0002].

            // If $relative is the empty sequence, the empty sequence is returned.

            // Note:

            // Resolving a URI does not dereference it. This is merely a syntactic operation on two character strings.

            throw new NotImplementedException("fn:resolve-uri() is not implemented.");
        }
        #endregion

        #region Regular Expressions Implementation

        // 7.6.1.1 Flags
        // All these functions provide an optional parameter, $flags, to set options for the interpretation of the 
        // regular expression. The parameter accepts a xs:string, in which individual letters are used to set options. 
        // The presence of a letter within the string indicates that the option is on; its absence indicates that the 
        // option is off. Letters may appear in any order and may be repeated. If there are characters present that 
        // are not defined here as flags, then an error is raised [err:FORX0001].

        // The following options are defined:

        // s: If present, the match operates in "dot-all" mode. (Perl calls this the single-line mode.) If the s flag 
        // is not specified, the meta-character . matches any character except a newline (#x0A) character. In dot-all mode, 
        // the meta-character . matches any character whatsoever. Suppose the input contains "hello" and "world" on two lines.
        // This will not be matched by the regular expression "hello.*world" unless dot-all mode is enabled.

        // m: If present, the match operates in multi-line mode. By default, the meta-character ^ matches the start of the 
        // entire string, while $ matches the end of the entire string. In multi-line mode, ^ matches the start of any line 
        // (that is, the start of the entire string, and the position immediately after a newline character), while $ matches 
        // the end of any line (that is, the end of the entire string, and the position immediately before a newline character). 
        // Newline here means the character #x0A only.

        // i: If present, the match operates in case-insensitive mode. The detailed rules are as follows. In these rules, 
        // a character C2 is considered to be a case-variant of another character C1 if the following XPath expression returns 
        // true when the two characters are considered as strings of length one, and the Unicode codepoint collation is used:

        // fn:lower-case(C1) eq fn:lower-case(C2)

        // or

        // fn:upper-case(C1) eq fn:upper-case(C2)

        // Note that the case-variants of a character under this definition are always single characters.

        // When a normal character (Char) is used as an atom, it represents the set containing that character and all its 
        // case-variants. For example, the regular expression "z" will match both "z" and "Z".

        // A character range (charRange) represents the set containing all the characters that it would match in the absence of 
        // the "i" flag, together with their case-variants. For example, the regular expression "[A-Z]" will match all the 
        // letters A-Z and all the letters a-z. It will also match certain other characters such as #x212A (KELVIN SIGN), 
        // since fn:lower-case("#x212A") is "k".

        // This rule applies also to a character range used in a character class subtraction (charClassSub): thus [A-Z-[IO]] 
        // will match characters such as "A", "B", "a", and "b", but will not match "I", "O", "i", or "o".

        // The rule also applies to a character range used as part of a negative character group: thus [^Q] will match every 
        // character except "Q" and "q" (these being the only case-variants of "Q" in Unicode).

        // A back-reference is compared using case-blind comparison: that is, each character must either be the same as the 
        // corresponding character of the previously matched string, or must be a case-variant of that character. For example, 
        // the strings "Mum", "mom", "Dad", and "DUD" all match the regular expression "([md])[aeiou]\1" when the "i" flag is used.

        // All other constructs are unaffected by the "i" flag. For example, "\p{Lu}" continues to match upper-case letters only.

        // x: If present, whitespace characters (#x9, #xA, #xD and #x20) in the regular expression are removed prior to matching 
        // with one exception: whitespace characters within character class expressions (charClassExpr) are not removed. This flag 
        // can be used, for example, to break up long regular expressions into readable lines.

        // Examples:
        // fn:matches("helloworld", "hello world", "x") returns true
        // fn:matches("helloworld", "hello[ ]world", "x") returns false
        // fn:matches("hello world", "hello\ sworld", "x") returns true
        // fn:matches("hello world", "hello world", "x") returns false



        private static bool RegexMatches(object[] args)
        {
            // 7.6.2 fn:matches
            // fn:matches($input as xs:string?, $pattern as xs:string) as xs:boolean
            // fn:matches( $input  as xs:string?, 
            // $pattern  as xs:string, 
            // $flags  as xs:string) as xs:boolean 

            // Summary: The function returns true if $input matches the regular expression supplied as $pattern 
            // as influenced by the value of $flags, if present; otherwise, it returns false.

            // The effect of calling the first version of this function (omitting the argument $flags) is the same as 
            // the effect of calling the second version with the $flags argument set to a zero-length string. 
            // Flags are defined in 7.6.1.1 Flags.

            // If $input is the empty sequence, it is interpreted as the zero-length string.

            // Unless the metacharacters ^ and $ are used as anchors, the string is considered to match the pattern if any 
            // substring matches the pattern. But if anchors are used, the anchors must match the start/end of the string 
            // (in string mode), or the start/end of a line (in multiline mode).

            // Note:

            // This is different from the behavior of patterns in [XML Schema Part 2: Datatypes Second Edition], where regular 
            // expressions are implicitly anchored.

            // An error is raised [err:FORX0002] if the value of $pattern is invalid according to the rules described in section 7.6.1 
            // Regular Expression Syntax.

            // An error is raised [err:FORX0001] if the value of $flags is invalid according to the rules described in section 7.6.1 
            // Regular Expression Syntax.

            //7.6.2.1 Examples
            // fn:matches("abracadabra", "bra") returns true
            // fn:matches("abracadabra", "^a.*a$") returns true
            // fn:matches("abracadabra", "^bra") returns false
            // Given the source document:
            // <poem author="Wilhelm Busch"> 
            // Kaum hat dies der Hahn gesehen,
            // Fängt er auch schon an zu krähen:
            // «Kikeriki! Kikikerikih!!»
            // Tak, tak, tak! - da kommen sie.
            // </poem>
            // the following function calls produce the following results, with the poem element as the context node:
            // fn:matches(., "Kaum.*krähen") returns false
            // fn:matches(., "Kaum.*krähen", "s") returns true
            // fn:matches(., "^Kaum.*gesehen,$", "m") returns true
            // fn:matches(., "^Kaum.*gesehen,$") returns false
            // fn:matches(., "kiki", "i") returns true

            // Note:
            // Regular expression matching is defined on the basis of Unicode code points; it takes no account of collations.

            if (args == null || args.Length < 2 || args.Length > 3)
                throw new ArgumentException("'matches' expects 2 or 3 arguments.");

            string op1 = Convert.ToString(args[0], CultureInfo.InvariantCulture);
            string op2 = Convert.ToString(args[1], CultureInfo.InvariantCulture);

            RegexOptions options = RegexOptions.None;
            if (args.Length == 3)
            {
                string flags = Convert.ToString(args[2], CultureInfo.InvariantCulture);
                if (flags.IndexOf('s') > -1)
                    options = options | RegexOptions.Singleline;

                if (flags.IndexOf('m') > -1)
                    options = options | RegexOptions.Multiline;

                if (flags.IndexOf('i') > -1)
                    options = options | RegexOptions.IgnoreCase;

                if (flags.IndexOf('x') > -1)
                    options = options | RegexOptions.IgnorePatternWhitespace;
            }

            Regex rex = new Regex(op2, options);
            return rex.IsMatch(op1);
        }

        private static string RegexMatch(object[] args)
        {
            // 7.6.2 fn:match
            // fn:matches($input as xs:string?, $pattern as xs:string) as xs:boolean
            // fn:matches( $input  as xs:string?, 
            // $pattern  as xs:string, 
            // $flags  as xs:string) as xs:boolean

            if (args == null || args.Length < 2 || args.Length > 3)
                throw new ArgumentException("'match' expects 2 or 3 arguments.");

            string op1 = Convert.ToString(args[0], CultureInfo.InvariantCulture);
            string op2 = Convert.ToString(args[1], CultureInfo.InvariantCulture);

            RegexOptions options = RegexOptions.None;
            if (args.Length == 3)
            {
                string flags = Convert.ToString(args[2], CultureInfo.InvariantCulture);
                if (flags.IndexOf('s') > -1)
                    options = options | RegexOptions.Singleline;

                if (flags.IndexOf('m') > -1)
                    options = options | RegexOptions.Multiline;

                if (flags.IndexOf('i') > -1)
                    options = options | RegexOptions.IgnoreCase;

                if (flags.IndexOf('x') > -1)
                    options = options | RegexOptions.IgnorePatternWhitespace;
            }

            Regex rex = new Regex(op2, options);
            Match match = rex.Match(op1);

            if (match != null)
                return match.Value;
            else
                return string.Empty;
        }

        private static string RegexReplace(object[] args)
        {
            // 7.6.3 fn:replace
            // fn:replace( $input  as xs:string?, 
            // $pattern  as xs:string, 
            // $replacement  as xs:string) as xs:string 

            // fn:replace( $input  as xs:string?, 
            // $pattern  as xs:string, 
            // $replacement  as xs:string, 
            // $flags  as xs:string) as xs:string 

            // Summary: The function returns the xs:string that is obtained by replacing each non-overlapping substring 
            // of $input that matches the given $pattern with an occurrence of the $replacement string.

            // The effect of calling the first version of this function (omitting the argument $flags) is the same as 
            // the effect of calling the second version with the $flags argument set to a zero-length string. 
            // Flags are defined in 7.6.1.1 Flags.

            // The $flags argument is interpreted in the same manner as for the fn:matches() function.

            // If $input is the empty sequence, it is interpreted as the zero-length string.

            // If two overlapping substrings of $input both match the $pattern, then only the first one (that is, the one whose 
            // first character comes first in the $input string) is replaced.

            // Within the $replacement string, a variable $N may be used to refer to the substring captured by the Nth parenthesized 
            // sub-expression in the regular expression. For each match of the pattern, these variables are assigned the value of the 
            // content matched by the relevant sub-expression, and the modified replacement string is then substituted for the characters 
            // in $input that matched the pattern. $0 refers to the substring captured by the regular expression as a whole.

            // More specifically, the rules are as follows, where S is the number of parenthesized sub-expressions in the regular 
            // expression, and N is the decimal number formed by taking all the digits that consecutively follow the $ character:

            // If N=0, then the variable is replaced by the substring matched by the regular expression as a whole.

            // If 1<=N<=S, then the variable is replaced by the substring captured by the Nth parenthesized sub-expression. If the Nth 
            // parenthesized sub-expression was not matched, then the variable is replaced by the zero-length string.

            // If S<N<=9, then the variable is replaced by the zero-length string.

            // Otherwise (if N>S and N>9), the last digit of N is taken to be a literal character to be included "as is" in the replacement 
            // string, and the rules are reapplied using the number N formed by stripping off this last digit.

            // For example, if the replacement string is "$23" and there are 5 substrings, the result contains the value of the substring 
            // that matches the second sub-expression, followed by the digit "3".

            // A literal "$" symbol must be written as "\$".

            // A literal "\" symbol must be written as "\\".

            // If two alternatives within the pattern both match at the same position in the $input, then the match that is chosen is the 
            // one matched by the first alternative. For example:

            // fn:replace("abcd", "(ab)|(a)", "[1=$1][2=$2]") returns "[1=ab][2=]cd"
            // An error is raised [err:FORX0002] if the value of $pattern is invalid according to the rules described in section 7.6.1 Regular 
            // Expression Syntax.

            // An error is raised [err:FORX0001] if the value of $flags is invalid according to the rules described in section 7.6.1 Regular 
            // Expression Syntax.

            // An error is raised [err:FORX0003] if the pattern matches a zero-length string, that is, if the expression 
            // fn:matches("", $pattern, $flags) returns true. It is not an error, however, if a captured substring is zero-length.

            // An error is raised [err:FORX0004] if the value of $replacement contains a "$" character that is not immediately followed by 
            // a digit 0-9 and not immediately preceded by a "\".

            // An error is raised [err:FORX0004] if the value of $replacement contains a "\" character that is not part of a "\\" pair, 
            // unless it is immediately followed by a "$" character.

            //7.6.3.1 Examples
            // replace("abracadabra", "bra", "*") returns "a*cada*"
            // replace("abracadabra", "a.*a", "*") returns "*"
            // replace("abracadabra", "a.*?a", "*") returns "*c*bra"
            // replace("abracadabra", "a", "") returns "brcdbr"
            // replace("abracadabra", "a(.)", "a$1$1") returns "abbraccaddabbra"
            // replace("abracadabra", ".*?", "$1") raises an error, because the pattern matches the zero-length string
            // replace("AAAA", "A+", "b") returns " b "
            // replace("AAAA", "A+?", "b") returns " bbbb "
            // replace("darted", "^(.*?)d(.*)$", "$1c$2") returns " carted ". The first " d " is replaced.

            if (args == null || args.Length < 3 || args.Length > 4)
                throw new ArgumentException("'replace' expects 3 or 4 arguments.");

            string op1 = Convert.ToString(args[0], CultureInfo.InvariantCulture);
            string op2 = Convert.ToString(args[1], CultureInfo.InvariantCulture);
            string op3 = Convert.ToString(args[2], CultureInfo.InvariantCulture);

            RegexOptions options = RegexOptions.None;
            if (args.Length == 4)
            {
                string flags = Convert.ToString(args[3], CultureInfo.InvariantCulture);
                if (flags.IndexOf('s') > -1)
                    options = options | RegexOptions.Singleline;

                if (flags.IndexOf('m') > -1)
                    options = options | RegexOptions.Multiline;

                if (flags.IndexOf('i') > -1)
                    options = options | RegexOptions.IgnoreCase;

                if (flags.IndexOf('x') > -1)
                    options = options | RegexOptions.IgnorePatternWhitespace;
            }

            Regex rex = new Regex(op2, options);

            // An error is raised [err:FORX0003] if the pattern matches a zero-length string, that is, if the expression 
            // fn:matches("", $pattern, $flags) returns true. It is not an error, however, if a captured substring is zero-length.

            if (rex.IsMatch(""))
                throw new XPathException("err:FORX0003 -> fn:replace(): the pattern matches an empty string.");

            return rex.Replace(op1, op3);
        }
        #endregion

        #region Boolean Functions Implementation
        private static bool BoolEquals(object[] args)
        {
            // 9.2.1 op:boolean-equal
            // op:boolean-equal($value1 as xs:boolean, $value2 as xs:boolean) as xs:boolean
            // Summary: Returns true if both arguments are true or if both arguments are false. 
            // Returns false if one of the arguments is true and the other argument is false.

            // This function backs up the "eq" operator on xs:boolean values.

            if (args == null || args.Length != 2)
                throw new ArgumentException("'boolean-equal' expects 2 arguments.");

            bool op1 = Convert.ToBoolean(args[0], CultureInfo.InvariantCulture);
            bool op2 = Convert.ToBoolean(args[1], CultureInfo.InvariantCulture);

            return op1 == op2;
        }

        private static bool BoolLessThan(object[] args)
        {
            // 9.2.2 op:boolean-less-than
            // op:boolean-less-than($arg1 as xs:boolean, $arg2 as xs:boolean) as xs:boolean
            // Summary: Returns true if $arg1 is false and $arg2 is true. Otherwise, returns false.

            // This function backs up the "lt" and "ge" operators on xs:boolean values.

            if (args == null || args.Length != 2)
                throw new ArgumentException("'boolean-less-than' expects 2 arguments.");

            bool op1 = Convert.ToBoolean(args[0], CultureInfo.InvariantCulture);
            bool op2 = Convert.ToBoolean(args[1], CultureInfo.InvariantCulture);

            return !op1 && op2;
        }

        private static bool BoolGreaterThan(object[] args)
        {
            // 9.2.3 op:boolean-greater-than
            // op:boolean-greater-than($arg1 as xs:boolean, $arg2 as xs:boolean) as xs:boolean
            // Summary: Returns true if $arg1 is true and $arg2 is false. Otherwise, returns false.

            // This function backs up the "gt" and "le" operators on xs:boolean values.

            if (args == null || args.Length != 2)
                throw new ArgumentException("'boolean-greater-than' expects 2 arguments.");

            bool op1 = Convert.ToBoolean(args[0], CultureInfo.InvariantCulture);
            bool op2 = Convert.ToBoolean(args[1], CultureInfo.InvariantCulture);

            return op1 && !op2;
        }
        #endregion
    }
}
