using System;
using System.Collections.Generic;
using System.Text;

namespace Acrux.Html
{
    /// <summary>
    /// In accordance with Extensible Markup Language (XML) 1.0 (Fourth Edition) W3C Recommendation 16 August 2006
    /// </summary>
    internal sealed class CharacterClasses
    {
        internal static char[] WHITE_SPACES = new char[] { '\x09', '\x20', '\xD', '\xA' };

        internal static bool IsWhiteSpaceChar(char test)
        {
            //[2]            S    ::=    (#x20 | #x9 | #xD | #xA)+ 
            if (test == '\x20' || test == '\x09' || test == '\xD' || test == '\xA')
                return true;

            return false;
        }

        internal static unsafe bool IsWhiteSpaceChar(char* test)
        {
            //[2]            S    ::=    (#x20 | #x9 | #xD | #xA)+ 
            if (*test == '\x20' || *test == '\x09' || *test == '\xD' || *test == '\xA')
                return true;

            return false;
        }

        internal static bool IsBaseChar(char test)
        {
            //[85]    BaseChar    ::=    [#x0041-#x005A] | [#x0061-#x007A] | [#x00C0-#x00D6] | [#x00D8-#x00F6] | 
            //                           [#x00F8-#x00FF] | [#x0100-#x0131] | [#x0134-#x013E] | [#x0141-#x0148] | 
            //                           [#x014A-#x017E] | [#x0180-#x01C3] | [#x01CD-#x01F0] | [#x01F4-#x01F5] | 
            //                           [#x01FA-#x0217] | [#x0250-#x02A8] | [#x02BB-#x02C1] | #x0386 | 
            //                           [#x0388-#x038A] | #x038C | [#x038E-#x03A1] | [#x03A3-#x03CE] | 
            //                           [#x03D0-#x03D6] | #x03DA | #x03DC | #x03DE | #x03E0 | [#x03E2-#x03F3] |
            //                           [#x0401-#x040C] | [#x040E-#x044F] | [#x0451-#x045C] | [#x045E-#x0481] | 
            //                           [#x0490-#x04C4] | [#x04C7-#x04C8] | [#x04CB-#x04CC] | [#x04D0-#x04EB] | 
            //                           [#x04EE-#x04F5] | [#x04F8-#x04F9] | [#x0531-#x0556] | #x0559 | 
            //                           [#x0561-#x0586] | [#x05D0-#x05EA] | [#x05F0-#x05F2] | [#x0621-#x063A] | 
            //                           [#x0641-#x064A] | [#x0671-#x06B7] | [#x06BA-#x06BE] | [#x06C0-#x06CE] | 
            //                           [#x06D0-#x06D3] | #x06D5 | [#x06E5-#x06E6] | [#x0905-#x0939] | #x093D | 
            //                           [#x0958-#x0961] | [#x0985-#x098C] | [#x098F-#x0990] | [#x0993-#x09A8] | 
            //                           [#x09AA-#x09B0] | #x09B2 | [#x09B6-#x09B9] | [#x09DC-#x09DD] | 
            //                           [#x09DF-#x09E1] | [#x09F0-#x09F1] | [#x0A05-#x0A0A] | [#x0A0F-#x0A10] | 
            //                           [#x0A13-#x0A28] | [#x0A2A-#x0A30] | [#x0A32-#x0A33] | [#x0A35-#x0A36] | 
            //                           [#x0A38-#x0A39] | [#x0A59-#x0A5C] | #x0A5E | [#x0A72-#x0A74] | 
            //                           [#x0A85-#x0A8B] | #x0A8D | [#x0A8F-#x0A91] | [#x0A93-#x0AA8] | 
            //                           [#x0AAA-#x0AB0] | [#x0AB2-#x0AB3] | [#x0AB5-#x0AB9] | #x0ABD | 
            //                           #x0AE0 | [#x0B05-#x0B0C] | [#x0B0F-#x0B10] | [#x0B13-#x0B28] | 
            //                           [#x0B2A-#x0B30] | [#x0B32-#x0B33] | [#x0B36-#x0B39] | #x0B3D | 
            //                           [#x0B5C-#x0B5D] | [#x0B5F-#x0B61] | [#x0B85-#x0B8A] | [#x0B8E-#x0B90] | 
            //                           [#x0B92-#x0B95] | [#x0B99-#x0B9A] | #x0B9C | [#x0B9E-#x0B9F] | 
            //                           [#x0BA3-#x0BA4] | [#x0BA8-#x0BAA] | [#x0BAE-#x0BB5] | [#x0BB7-#x0BB9] | 
            //                           [#x0C05-#x0C0C] | [#x0C0E-#x0C10] | [#x0C12-#x0C28] | [#x0C2A-#x0C33] | 
            //                           [#x0C35-#x0C39] | [#x0C60-#x0C61] | [#x0C85-#x0C8C] | [#x0C8E-#x0C90] | 
            //                           [#x0C92-#x0CA8] | [#x0CAA-#x0CB3] | [#x0CB5-#x0CB9] | #x0CDE | 
            //                           [#x0CE0-#x0CE1] | [#x0D05-#x0D0C] | [#x0D0E-#x0D10] | [#x0D12-#x0D28] | 
            //                           [#x0D2A-#x0D39] | [#x0D60-#x0D61] | [#x0E01-#x0E2E] | #x0E30 | 
            //                           [#x0E32-#x0E33] | [#x0E40-#x0E45] | [#x0E81-#x0E82] | #x0E84 | 
            //                           [#x0E87-#x0E88] | #x0E8A | #x0E8D | [#x0E94-#x0E97] | [#x0E99-#x0E9F] | 
            //                           [#x0EA1-#x0EA3] | #x0EA5 | #x0EA7 | [#x0EAA-#x0EAB] | [#x0EAD-#x0EAE] | 
            //                           #x0EB0 | [#x0EB2-#x0EB3] | #x0EBD | [#x0EC0-#x0EC4] | [#x0F40-#x0F47] | 
            //                           [#x0F49-#x0F69] | [#x10A0-#x10C5] | [#x10D0-#x10F6] | #x1100 | 
            //                           [#x1102-#x1103] | [#x1105-#x1107] | #x1109 | [#x110B-#x110C] | 
            //                           [#x110E-#x1112] | #x113C | #x113E | #x1140 | #x114C | #x114E | #x1150 | 
            //                           [#x1154-#x1155] | #x1159 | [#x115F-#x1161] | #x1163 | #x1165 | #x1167 | 
            //                           #x1169 | [#x116D-#x116E] | [#x1172-#x1173] | #x1175 | #x119E | #x11A8 | 
            //                           #x11AB | [#x11AE-#x11AF] | [#x11B7-#x11B8] | #x11BA | [#x11BC-#x11C2] | 
            //                           #x11EB | #x11F0 | #x11F9 | [#x1E00-#x1E9B] | [#x1EA0-#x1EF9] | 
            //                           [#x1F00-#x1F15] | [#x1F18-#x1F1D] | [#x1F20-#x1F45] | [#x1F48-#x1F4D] | 
            //                           [#x1F50-#x1F57] | #x1F59 | #x1F5B | #x1F5D | [#x1F5F-#x1F7D] | 
            //                           [#x1F80-#x1FB4] | [#x1FB6-#x1FBC] | #x1FBE | [#x1FC2-#x1FC4] | 
            //                           [#x1FC6-#x1FCC] | [#x1FD0-#x1FD3] | [#x1FD6-#x1FDB] | [#x1FE0-#x1FEC] | 
            //                           [#x1FF2-#x1FF4] | [#x1FF6-#x1FFC] | #x2126 | [#x212A-#x212B] | #x212E | 
            //                           [#x2180-#x2182] | [#x3041-#x3094] | [#x30A1-#x30FA] | [#x3105-#x312C] | 
            //                           [#xAC00-#xD7A3] 
            if (
                (test >= '\x0041' && test <= '\x005A') ||
                (test >= '\x0061' && test <= '\x007A') ||
                (test >= '\x00C0' && test <= '\x00D6') ||
                (test >= '\x00D8' && test <= '\x00F6') || 
                (test >= '\x00F8' && test <= '\x00FF') ||
                (test >= '\x0100' && test <= '\x0131') ||
                (test >= '\x0134' && test <= '\x013E') ||
                (test >= '\x0141' && test <= '\x0148') ||
                (test >= '\x014A' && test <= '\x017E') ||
                (test >= '\x0180' && test <= '\x01C3') ||
                (test >= '\x01CD' && test <= '\x01F0') ||
                (test >= '\x01F4' && test <= '\x01F5') ||
                (test >= '\x01FA' && test <= '\x0217') ||
                (test >= '\x0250' && test <= '\x02A8') ||
                (test >= '\x02BB' && test <= '\x02C1') ||
                (test == '\x0386') ||
                (test >= '\x0388' && test <= '\x038A') ||
                (test == '\x038C') ||
                (test >= '\x038E' && test <= '\x03A1') ||
                (test >= '\x03A3' && test <= '\x03CE') ||
                (test >= '\x03D0' && test <= '\x03D6') ||
                (test >= '\x03DA' && test <= '\x03DC') ||
                (test >= '\x03DE' && test <= '\x03E0') ||
                (test >= '\x03E2' && test <= '\x03F3') ||
                (test >= '\x0401' && test <= '\x040C') ||
                (test >= '\x040E' && test <= '\x044F') ||
                (test >= '\x0451' && test <= '\x045C') ||
                (test >= '\x045E' && test <= '\x0481') ||
                (test >= '\x0490' && test <= '\x04C4') ||
                (test >= '\x04C7' && test <= '\x04C8') ||
                (test >= '\x04CB' && test <= '\x04CC') ||
                (test >= '\x04D0' && test <= '\x04EB') ||
                (test >= '\x04EE' && test <= '\x04F5') || 
                (test >= '\x04F8' && test <= '\x04F9') ||
                (test >= '\x0531' && test <= '\x0556') ||
                (test == '\x0559') ||
                (test >= '\x0561' && test <= '\x0586') ||
                (test >= '\x05D0' && test <= '\x05EA') ||
                (test >= '\x05F0' && test <= '\x05F2') ||
                (test >= '\x0621' && test <= '\x063A') || 
                (test >= '\x0641' && test <= '\x064A') ||
                (test >= '\x0671' && test <= '\x06B7') ||
                (test >= '\x06BA' && test <= '\x06BE') ||
                (test >= '\x06C0' && test <= '\x06CE') ||
                (test >= '\x06D0' && test <= '\x06D3') ||
                (test == '\x06D5') ||
                (test >= '\x06E5' && test <= '\x06E6') ||
                (test >= '\x0905' && test <= '\x0939') ||
                (test == '\x093D') ||
                (test >= '\x0958' && test <= '\x0961') ||
                (test >= '\x0985' && test <= '\x098C') ||
                (test >= '\x098F' && test <= '\x0990') ||
                (test >= '\x0993' && test <= '\x09A8') ||
                (test >= '\x09AA' && test <= '\x09B0') ||
                (test == '\x09B2') ||
                (test >= '\x09B6' && test <= '\x09B9') ||
                (test >= '\x09DC' && test <= '\x09DD') ||
                (test >= '\x09DF' && test <= '\x09E1') ||
                (test >= '\x09F0' && test <= '\x09F1') ||
                (test >= '\x0A05' && test <= '\x0A0A') ||
                (test >= '\x0A0F' && test <= '\x0A10') ||
                (test >= '\x0A13' && test <= '\x0A28') ||
                (test >= '\x0A2A' && test <= '\x0A30') ||
                (test >= '\x0A32' && test <= '\x0A33') ||
                (test >= '\x0A35' && test <= '\x0A36') ||
                (test >= '\x0A38' && test <= '\x0A39') || 
                (test >= '\x0A59' && test <= '\x0A5C') ||
                (test == '\x0A5E') ||
                (test >= '\x0A72' && test <= '\x0A74') ||
                (test >= '\x0A85' && test <= '\x0A8B') ||
                (test == '\x0A8D') ||
                (test >= '\x0A8F' && test <= '\x0A91') ||
                (test >= '\x0A93' && test <= '\x0AA8') ||
                (test >= '\x0AAA' && test <= '\x0AB0') ||
                (test >= '\x0AB2' && test <= '\x0AB3') ||
                (test >= '\x0AB5' && test <= '\x0AB9') ||
                (test == '\x0ABD') ||
                (test == '\x0AE0') ||
                (test >= '\x0B05' && test <= '\x0B0C') ||
                (test >= '\x0B0F' && test <= '\x0B10') ||
                (test >= '\x0B13' && test <= '\x0B28') ||
                (test >= '\x0B2A' && test <= '\x0B30') ||
                (test >= '\x0B32' && test <= '\x0B33') ||
                (test >= '\x0B36' && test <= '\x0B39') ||
                (test == '\x0B3D') ||
                (test >= '\x0B5C' && test <= '\x0B5D') ||
                (test >= '\x0B5F' && test <= '\x0B61') ||
                (test >= '\x0B85' && test <= '\x0B8A') ||
                (test >= '\x0B8E' && test <= '\x0B90') ||
                (test >= '\x0B92' && test <= '\x0B95') ||
                (test >= '\x0B99' && test <= '\x0B9A') ||
                (test == '\x0B9C') ||
                (test >= '\x0B9E' && test <= '\x0B9F') ||
                (test >= '\x0BA3' && test <= '\x0BA4') ||
                (test >= '\x0BA8' && test <= '\x0BAA') ||
                (test >= '\x0BAE' && test <= '\x0BB5') ||
                (test >= '\x0BB7' && test <= '\x0BB9') ||
                (test >= '\x0C05' && test <= '\x0C0C') ||
                (test >= '\x0C0E' && test <= '\x0C10') ||
                (test >= '\x0C12' && test <= '\x0C28') ||
                (test >= '\x0C2A' && test <= '\x0C33') ||
                (test >= '\x0C35' && test <= '\x0C39') ||
                (test >= '\x0C60' && test <= '\x0C61') ||
                (test >= '\x0C85' && test <= '\x0C8C') ||
                (test >= '\x0C8E' && test <= '\x0C90') ||
                (test >= '\x0C92' && test <= '\x0CA8') ||
                (test >= '\x0CAA' && test <= '\x0CB3') ||
                (test >= '\x0CB5' && test <= '\x0CB9') ||
                (test == '\x0CDE') ||
                (test >= '\x0CE0' && test <= '\x0CE1') ||
                (test >= '\x0D05' && test <= '\x0D0C') ||
                (test >= '\x0D0E' && test <= '\x0D10') ||
                (test >= '\x0D12' && test <= '\x0D28') ||
                (test >= '\x0D2A' && test <= '\x0D39') ||
                (test >= '\x0D60' && test <= '\x0D61') ||
                (test >= '\x0E01' && test <= '\x0E2E') ||
                (test == '\x0E30') ||
                (test >= '\x0E32' && test <= '\x0E33') ||
                (test >= '\x0E40' && test <= '\x0E45') ||
                (test >= '\x0E81' && test <= '\x0E82') ||
                (test == '\x0E84') ||
                (test >= '\x0E87' && test <= '\x0E88') ||
                (test == '\x0E8A') ||
                (test == '\x0E8D') ||
                (test >= '\x0E94' && test <= '\x0E97') ||
                (test >= '\x0E99' && test <= '\x0E9F') ||
                (test >= '\x0EA1' && test <= '\x0EA3') ||
                (test == '\x0EA5') ||
                (test == '\x0EA7') ||
                (test >= '\x0EAA' && test <= '\x0EAB') ||
                (test >= '\x0EAD' && test <= '\x0EAE') ||
                (test == '\x0EB0') ||
                (test >= '\x0EB2' && test <= '\x0EB3') ||
                (test == '\x0EBD') ||
                (test >= '\x0EC0' && test <= '\x0EC4') ||
                (test >= '\x0F40' && test <= '\x0F47') ||
                (test >= '\x0F49' && test <= '\x0F69') ||
                (test >= '\x10A0' && test <= '\x10C5') ||
                (test >= '\x10D0' && test <= '\x10F6') ||
                (test == '\x1100') ||
                (test >= '\x1102' && test <= '\x1103') ||
                (test >= '\x1105' && test <= '\x1107') ||
                (test == '\x1109') ||
                (test >= '\x110B' && test <= '\x110C') ||
                (test >= '\x110E' && test <= '\x1112') ||
                (test == '\x113C') ||
                (test == '\x113E') ||
                (test == '\x1140') ||
                (test == '\x114C') ||
                (test == '\x114E') ||
                (test == '\x1150') ||
                (test >= '\x1154' && test <= '\x1155') ||
                (test == '\x1159') ||
                (test >= '\x115F' && test <= '\x1161') ||
                (test == '\x1163') ||
                (test == '\x1165') ||
                (test == '\x1167') ||
                (test == '\x1169') ||
                (test >= '\x116D' && test <= '\x116E') ||
                (test >= '\x1172' && test <= '\x1173') ||
                (test == '\x1175') ||
                (test == '\x119E') ||
                (test == '\x11A8') ||
                (test == '\x11AB') ||
                (test >= '\x11AE' && test <= '\x11AF') ||
                (test >= '\x11B7' && test <= '\x11B8') ||
                (test == '\x11BA') ||
                (test >= '\x11BC' && test <= '\x11C2') ||
                (test == '\x11EB') ||
                (test == '\x11F0') ||
                (test == '\x11F9') ||
                (test >= '\x1E00' && test <= '\x1E9B') ||
                (test >= '\x1EA0' && test <= '\x1EF9') ||
                (test >= '\x1F00' && test <= '\x1F15') ||
                (test >= '\x1F18' && test <= '\x1F1D') ||
                (test >= '\x1F20' && test <= '\x1F45') ||
                (test >= '\x1F48' && test <= '\x1F4D') ||
                (test >= '\x1F50' && test <= '\x1F57') ||
                (test == '\x1F59') ||
                (test == '\x1F5B') ||
                (test == '\x1F5D') ||
                (test >= '\x1F5F' && test <= '\x1F7D') ||
                (test >= '\x1F80' && test <= '\x1FB4') ||
                (test >= '\x1FB6' && test <= '\x1FBC') ||
                (test == '\x1FBE') ||
                (test >= '\x1FC2' && test <= '\x1FC4') ||
                (test >= '\x1FC6' && test <= '\x1FCC') ||
                (test >= '\x1FD0' && test <= '\x1FD3') ||
                (test >= '\x1FD6' && test <= '\x1FDB') ||
                (test >= '\x1FE0' && test <= '\x1FEC') ||
                (test >= '\x1FF2' && test <= '\x1FF4') || 
                (test >= '\x1FF6' && test <= '\x1FFC') ||
                (test == '\x2126') ||
                (test >= '\x212A' && test <= '\x212B') ||
                (test == '\x212E') ||
                (test >= '\x2180' && test <= '\x2182') ||
                (test >= '\x3041' && test <= '\x3094') ||
                (test >= '\x30A1' && test <= '\x30FA') ||
                (test >= '\x3105' && test <= '\x312C') ||
                (test >= '\xAC00' && test <= '\xD7A3')
               )
                return true;

            return false;
        }

        internal static unsafe bool IsBaseChar(char*test)
        {
            //[85]    BaseChar    ::=    [#x0041-#x005A] | [#x0061-#x007A] | [#x00C0-#x00D6] | [#x00D8-#x00F6] | 
            //                           [#x00F8-#x00FF] | [#x0100-#x0131] | [#x0134-#x013E] | [#x0141-#x0148] | 
            //                           [#x014A-#x017E] | [#x0180-#x01C3] | [#x01CD-#x01F0] | [#x01F4-#x01F5] | 
            //                           [#x01FA-#x0217] | [#x0250-#x02A8] | [#x02BB-#x02C1] | #x0386 | 
            //                           [#x0388-#x038A] | #x038C | [#x038E-#x03A1] | [#x03A3-#x03CE] | 
            //                           [#x03D0-#x03D6] | #x03DA | #x03DC | #x03DE | #x03E0 | [#x03E2-#x03F3] |
            //                           [#x0401-#x040C] | [#x040E-#x044F] | [#x0451-#x045C] | [#x045E-#x0481] | 
            //                           [#x0490-#x04C4] | [#x04C7-#x04C8] | [#x04CB-#x04CC] | [#x04D0-#x04EB] | 
            //                           [#x04EE-#x04F5] | [#x04F8-#x04F9] | [#x0531-#x0556] | #x0559 | 
            //                           [#x0561-#x0586] | [#x05D0-#x05EA] | [#x05F0-#x05F2] | [#x0621-#x063A] | 
            //                           [#x0641-#x064A] | [#x0671-#x06B7] | [#x06BA-#x06BE] | [#x06C0-#x06CE] | 
            //                           [#x06D0-#x06D3] | #x06D5 | [#x06E5-#x06E6] | [#x0905-#x0939] | #x093D | 
            //                           [#x0958-#x0961] | [#x0985-#x098C] | [#x098F-#x0990] | [#x0993-#x09A8] | 
            //                           [#x09AA-#x09B0] | #x09B2 | [#x09B6-#x09B9] | [#x09DC-#x09DD] | 
            //                           [#x09DF-#x09E1] | [#x09F0-#x09F1] | [#x0A05-#x0A0A] | [#x0A0F-#x0A10] | 
            //                           [#x0A13-#x0A28] | [#x0A2A-#x0A30] | [#x0A32-#x0A33] | [#x0A35-#x0A36] | 
            //                           [#x0A38-#x0A39] | [#x0A59-#x0A5C] | #x0A5E | [#x0A72-#x0A74] | 
            //                           [#x0A85-#x0A8B] | #x0A8D | [#x0A8F-#x0A91] | [#x0A93-#x0AA8] | 
            //                           [#x0AAA-#x0AB0] | [#x0AB2-#x0AB3] | [#x0AB5-#x0AB9] | #x0ABD | 
            //                           #x0AE0 | [#x0B05-#x0B0C] | [#x0B0F-#x0B10] | [#x0B13-#x0B28] | 
            //                           [#x0B2A-#x0B30] | [#x0B32-#x0B33] | [#x0B36-#x0B39] | #x0B3D | 
            //                           [#x0B5C-#x0B5D] | [#x0B5F-#x0B61] | [#x0B85-#x0B8A] | [#x0B8E-#x0B90] | 
            //                           [#x0B92-#x0B95] | [#x0B99-#x0B9A] | #x0B9C | [#x0B9E-#x0B9F] | 
            //                           [#x0BA3-#x0BA4] | [#x0BA8-#x0BAA] | [#x0BAE-#x0BB5] | [#x0BB7-#x0BB9] | 
            //                           [#x0C05-#x0C0C] | [#x0C0E-#x0C10] | [#x0C12-#x0C28] | [#x0C2A-#x0C33] | 
            //                           [#x0C35-#x0C39] | [#x0C60-#x0C61] | [#x0C85-#x0C8C] | [#x0C8E-#x0C90] | 
            //                           [#x0C92-#x0CA8] | [#x0CAA-#x0CB3] | [#x0CB5-#x0CB9] | #x0CDE | 
            //                           [#x0CE0-#x0CE1] | [#x0D05-#x0D0C] | [#x0D0E-#x0D10] | [#x0D12-#x0D28] | 
            //                           [#x0D2A-#x0D39] | [#x0D60-#x0D61] | [#x0E01-#x0E2E] | #x0E30 | 
            //                           [#x0E32-#x0E33] | [#x0E40-#x0E45] | [#x0E81-#x0E82] | #x0E84 | 
            //                           [#x0E87-#x0E88] | #x0E8A | #x0E8D | [#x0E94-#x0E97] | [#x0E99-#x0E9F] | 
            //                           [#x0EA1-#x0EA3] | #x0EA5 | #x0EA7 | [#x0EAA-#x0EAB] | [#x0EAD-#x0EAE] | 
            //                           #x0EB0 | [#x0EB2-#x0EB3] | #x0EBD | [#x0EC0-#x0EC4] | [#x0F40-#x0F47] | 
            //                           [#x0F49-#x0F69] | [#x10A0-#x10C5] | [#x10D0-#x10F6] | #x1100 | 
            //                           [#x1102-#x1103] | [#x1105-#x1107] | #x1109 | [#x110B-#x110C] | 
            //                           [#x110E-#x1112] | #x113C | #x113E | #x1140 | #x114C | #x114E | #x1150 | 
            //                           [#x1154-#x1155] | #x1159 | [#x115F-#x1161] | #x1163 | #x1165 | #x1167 | 
            //                           #x1169 | [#x116D-#x116E] | [#x1172-#x1173] | #x1175 | #x119E | #x11A8 | 
            //                           #x11AB | [#x11AE-#x11AF] | [#x11B7-#x11B8] | #x11BA | [#x11BC-#x11C2] | 
            //                           #x11EB | #x11F0 | #x11F9 | [#x1E00-#x1E9B] | [#x1EA0-#x1EF9] | 
            //                           [#x1F00-#x1F15] | [#x1F18-#x1F1D] | [#x1F20-#x1F45] | [#x1F48-#x1F4D] | 
            //                           [#x1F50-#x1F57] | #x1F59 | #x1F5B | #x1F5D | [#x1F5F-#x1F7D] | 
            //                           [#x1F80-#x1FB4] | [#x1FB6-#x1FBC] | #x1FBE | [#x1FC2-#x1FC4] | 
            //                           [#x1FC6-#x1FCC] | [#x1FD0-#x1FD3] | [#x1FD6-#x1FDB] | [#x1FE0-#x1FEC] | 
            //                           [#x1FF2-#x1FF4] | [#x1FF6-#x1FFC] | #x2126 | [#x212A-#x212B] | #x212E | 
            //                           [#x2180-#x2182] | [#x3041-#x3094] | [#x30A1-#x30FA] | [#x3105-#x312C] | 
            //                           [#xAC00-#xD7A3] 
            if (
                (*test >= '\x0041' && *test <= '\x005A') ||
                (*test >= '\x0061' && *test <= '\x007A') ||
                (*test >= '\x00C0' && *test <= '\x00D6') ||
                (*test >= '\x00D8' && *test <= '\x00F6') ||
                (*test >= '\x00F8' && *test <= '\x00FF') ||
                (*test >= '\x0100' && *test <= '\x0131') ||
                (*test >= '\x0134' && *test <= '\x013E') ||
                (*test >= '\x0141' && *test <= '\x0148') ||
                (*test >= '\x014A' && *test <= '\x017E') ||
                (*test >= '\x0180' && *test <= '\x01C3') ||
                (*test >= '\x01CD' && *test <= '\x01F0') ||
                (*test >= '\x01F4' && *test <= '\x01F5') ||
                (*test >= '\x01FA' && *test <= '\x0217') ||
                (*test >= '\x0250' && *test <= '\x02A8') ||
                (*test >= '\x02BB' && *test <= '\x02C1') ||
                (*test == '\x0386') ||
                (*test >= '\x0388' && *test <= '\x038A') ||
                (*test == '\x038C') ||
                (*test >= '\x038E' && *test <= '\x03A1') ||
                (*test >= '\x03A3' && *test <= '\x03CE') ||
                (*test >= '\x03D0' && *test <= '\x03D6') ||
                (*test >= '\x03DA' && *test <= '\x03DC') ||
                (*test >= '\x03DE' && *test <= '\x03E0') ||
                (*test >= '\x03E2' && *test <= '\x03F3') ||
                (*test >= '\x0401' && *test <= '\x040C') ||
                (*test >= '\x040E' && *test <= '\x044F') ||
                (*test >= '\x0451' && *test <= '\x045C') ||
                (*test >= '\x045E' && *test <= '\x0481') ||
                (*test >= '\x0490' && *test <= '\x04C4') ||
                (*test >= '\x04C7' && *test <= '\x04C8') ||
                (*test >= '\x04CB' && *test <= '\x04CC') ||
                (*test >= '\x04D0' && *test <= '\x04EB') ||
                (*test >= '\x04EE' && *test <= '\x04F5') ||
                (*test >= '\x04F8' && *test <= '\x04F9') ||
                (*test >= '\x0531' && *test <= '\x0556') ||
                (*test == '\x0559') ||
                (*test >= '\x0561' && *test <= '\x0586') ||
                (*test >= '\x05D0' && *test <= '\x05EA') ||
                (*test >= '\x05F0' && *test <= '\x05F2') ||
                (*test >= '\x0621' && *test <= '\x063A') ||
                (*test >= '\x0641' && *test <= '\x064A') ||
                (*test >= '\x0671' && *test <= '\x06B7') ||
                (*test >= '\x06BA' && *test <= '\x06BE') ||
                (*test >= '\x06C0' && *test <= '\x06CE') ||
                (*test >= '\x06D0' && *test <= '\x06D3') ||
                (*test == '\x06D5') ||
                (*test >= '\x06E5' && *test <= '\x06E6') ||
                (*test >= '\x0905' && *test <= '\x0939') ||
                (*test == '\x093D') ||
                (*test >= '\x0958' && *test <= '\x0961') ||
                (*test >= '\x0985' && *test <= '\x098C') ||
                (*test >= '\x098F' && *test <= '\x0990') ||
                (*test >= '\x0993' && *test <= '\x09A8') ||
                (*test >= '\x09AA' && *test <= '\x09B0') ||
                (*test == '\x09B2') ||
                (*test >= '\x09B6' && *test <= '\x09B9') ||
                (*test >= '\x09DC' && *test <= '\x09DD') ||
                (*test >= '\x09DF' && *test <= '\x09E1') ||
                (*test >= '\x09F0' && *test <= '\x09F1') ||
                (*test >= '\x0A05' && *test <= '\x0A0A') ||
                (*test >= '\x0A0F' && *test <= '\x0A10') ||
                (*test >= '\x0A13' && *test <= '\x0A28') ||
                (*test >= '\x0A2A' && *test <= '\x0A30') ||
                (*test >= '\x0A32' && *test <= '\x0A33') ||
                (*test >= '\x0A35' && *test <= '\x0A36') ||
                (*test >= '\x0A38' && *test <= '\x0A39') ||
                (*test >= '\x0A59' && *test <= '\x0A5C') ||
                (*test == '\x0A5E') ||
                (*test >= '\x0A72' && *test <= '\x0A74') ||
                (*test >= '\x0A85' && *test <= '\x0A8B') ||
                (*test == '\x0A8D') ||
                (*test >= '\x0A8F' && *test <= '\x0A91') ||
                (*test >= '\x0A93' && *test <= '\x0AA8') ||
                (*test >= '\x0AAA' && *test <= '\x0AB0') ||
                (*test >= '\x0AB2' && *test <= '\x0AB3') ||
                (*test >= '\x0AB5' && *test <= '\x0AB9') ||
                (*test == '\x0ABD') ||
                (*test == '\x0AE0') ||
                (*test >= '\x0B05' && *test <= '\x0B0C') ||
                (*test >= '\x0B0F' && *test <= '\x0B10') ||
                (*test >= '\x0B13' && *test <= '\x0B28') ||
                (*test >= '\x0B2A' && *test <= '\x0B30') ||
                (*test >= '\x0B32' && *test <= '\x0B33') ||
                (*test >= '\x0B36' && *test <= '\x0B39') ||
                (*test == '\x0B3D') ||
                (*test >= '\x0B5C' && *test <= '\x0B5D') ||
                (*test >= '\x0B5F' && *test <= '\x0B61') ||
                (*test >= '\x0B85' && *test <= '\x0B8A') ||
                (*test >= '\x0B8E' && *test <= '\x0B90') ||
                (*test >= '\x0B92' && *test <= '\x0B95') ||
                (*test >= '\x0B99' && *test <= '\x0B9A') ||
                (*test == '\x0B9C') ||
                (*test >= '\x0B9E' && *test <= '\x0B9F') ||
                (*test >= '\x0BA3' && *test <= '\x0BA4') ||
                (*test >= '\x0BA8' && *test <= '\x0BAA') ||
                (*test >= '\x0BAE' && *test <= '\x0BB5') ||
                (*test >= '\x0BB7' && *test <= '\x0BB9') ||
                (*test >= '\x0C05' && *test <= '\x0C0C') ||
                (*test >= '\x0C0E' && *test <= '\x0C10') ||
                (*test >= '\x0C12' && *test <= '\x0C28') ||
                (*test >= '\x0C2A' && *test <= '\x0C33') ||
                (*test >= '\x0C35' && *test <= '\x0C39') ||
                (*test >= '\x0C60' && *test <= '\x0C61') ||
                (*test >= '\x0C85' && *test <= '\x0C8C') ||
                (*test >= '\x0C8E' && *test <= '\x0C90') ||
                (*test >= '\x0C92' && *test <= '\x0CA8') ||
                (*test >= '\x0CAA' && *test <= '\x0CB3') ||
                (*test >= '\x0CB5' && *test <= '\x0CB9') ||
                (*test == '\x0CDE') ||
                (*test >= '\x0CE0' && *test <= '\x0CE1') ||
                (*test >= '\x0D05' && *test <= '\x0D0C') ||
                (*test >= '\x0D0E' && *test <= '\x0D10') ||
                (*test >= '\x0D12' && *test <= '\x0D28') ||
                (*test >= '\x0D2A' && *test <= '\x0D39') ||
                (*test >= '\x0D60' && *test <= '\x0D61') ||
                (*test >= '\x0E01' && *test <= '\x0E2E') ||
                (*test == '\x0E30') ||
                (*test >= '\x0E32' && *test <= '\x0E33') ||
                (*test >= '\x0E40' && *test <= '\x0E45') ||
                (*test >= '\x0E81' && *test <= '\x0E82') ||
                (*test == '\x0E84') ||
                (*test >= '\x0E87' && *test <= '\x0E88') ||
                (*test == '\x0E8A') ||
                (*test == '\x0E8D') ||
                (*test >= '\x0E94' && *test <= '\x0E97') ||
                (*test >= '\x0E99' && *test <= '\x0E9F') ||
                (*test >= '\x0EA1' && *test <= '\x0EA3') ||
                (*test == '\x0EA5') ||
                (*test == '\x0EA7') ||
                (*test >= '\x0EAA' && *test <= '\x0EAB') ||
                (*test >= '\x0EAD' && *test <= '\x0EAE') ||
                (*test == '\x0EB0') ||
                (*test >= '\x0EB2' && *test <= '\x0EB3') ||
                (*test == '\x0EBD') ||
                (*test >= '\x0EC0' && *test <= '\x0EC4') ||
                (*test >= '\x0F40' && *test <= '\x0F47') ||
                (*test >= '\x0F49' && *test <= '\x0F69') ||
                (*test >= '\x10A0' && *test <= '\x10C5') ||
                (*test >= '\x10D0' && *test <= '\x10F6') ||
                (*test == '\x1100') ||
                (*test >= '\x1102' && *test <= '\x1103') ||
                (*test >= '\x1105' && *test <= '\x1107') ||
                (*test == '\x1109') ||
                (*test >= '\x110B' && *test <= '\x110C') ||
                (*test >= '\x110E' && *test <= '\x1112') ||
                (*test == '\x113C') ||
                (*test == '\x113E') ||
                (*test == '\x1140') ||
                (*test == '\x114C') ||
                (*test == '\x114E') ||
                (*test == '\x1150') ||
                (*test >= '\x1154' && *test <= '\x1155') ||
                (*test == '\x1159') ||
                (*test >= '\x115F' && *test <= '\x1161') ||
                (*test == '\x1163') ||
                (*test == '\x1165') ||
                (*test == '\x1167') ||
                (*test == '\x1169') ||
                (*test >= '\x116D' && *test <= '\x116E') ||
                (*test >= '\x1172' && *test <= '\x1173') ||
                (*test == '\x1175') ||
                (*test == '\x119E') ||
                (*test == '\x11A8') ||
                (*test == '\x11AB') ||
                (*test >= '\x11AE' && *test <= '\x11AF') ||
                (*test >= '\x11B7' && *test <= '\x11B8') ||
                (*test == '\x11BA') ||
                (*test >= '\x11BC' && *test <= '\x11C2') ||
                (*test == '\x11EB') ||
                (*test == '\x11F0') ||
                (*test == '\x11F9') ||
                (*test >= '\x1E00' && *test <= '\x1E9B') ||
                (*test >= '\x1EA0' && *test <= '\x1EF9') ||
                (*test >= '\x1F00' && *test <= '\x1F15') ||
                (*test >= '\x1F18' && *test <= '\x1F1D') ||
                (*test >= '\x1F20' && *test <= '\x1F45') ||
                (*test >= '\x1F48' && *test <= '\x1F4D') ||
                (*test >= '\x1F50' && *test <= '\x1F57') ||
                (*test == '\x1F59') ||
                (*test == '\x1F5B') ||
                (*test == '\x1F5D') ||
                (*test >= '\x1F5F' && *test <= '\x1F7D') ||
                (*test >= '\x1F80' && *test <= '\x1FB4') ||
                (*test >= '\x1FB6' && *test <= '\x1FBC') ||
                (*test == '\x1FBE') ||
                (*test >= '\x1FC2' && *test <= '\x1FC4') ||
                (*test >= '\x1FC6' && *test <= '\x1FCC') ||
                (*test >= '\x1FD0' && *test <= '\x1FD3') ||
                (*test >= '\x1FD6' && *test <= '\x1FDB') ||
                (*test >= '\x1FE0' && *test <= '\x1FEC') ||
                (*test >= '\x1FF2' && *test <= '\x1FF4') ||
                (*test >= '\x1FF6' && *test <= '\x1FFC') ||
                (*test == '\x2126') ||
                (*test >= '\x212A' && *test <= '\x212B') ||
                (*test == '\x212E') ||
                (*test >= '\x2180' && *test <= '\x2182') ||
                (*test >= '\x3041' && *test <= '\x3094') ||
                (*test >= '\x30A1' && *test <= '\x30FA') ||
                (*test >= '\x3105' && *test <= '\x312C') ||
                (*test >= '\xAC00' && *test <= '\xD7A3')
               )
                return true;

            return false;
        }

        internal static bool IsIdeographic(char test)
        {
            //[86]    Ideographic    ::=    [#x4E00-#x9FA5] | #x3007 | [#x3021-#x3029]  

            if (
                (test >= '\x4E00' && test <= '\x9FA5') ||
                (test == '\x3007') ||
                (test >= '\x3021' && test <= '\x3029')
               )
                return true;

            return false;
        }

        internal static unsafe bool IsIdeographic(char* test)
        {
            //[86]    Ideographic    ::=    [#x4E00-#x9FA5] | #x3007 | [#x3021-#x3029]  

            if (
                (*test >= '\x4E00' && *test <= '\x9FA5') ||
                (*test == '\x3007') ||
                (*test >= '\x3021' && *test <= '\x3029')
               )
                return true;

            return false;
        }

        internal static bool IsLetter(char test)
        {
            //[84]    Letter    ::=    BaseChar | Ideographic  
            return IsBaseChar(test) || IsIdeographic(test);
        }

        internal static unsafe bool IsLetter(char* test)
        {
            //[84]    Letter    ::=    BaseChar | Ideographic  
            return IsBaseChar(test) || IsIdeographic(test);
        }

        internal static bool IsChar(char test)
        {
            //[2]    Char    ::=    #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]  
            if (
                (test == '\x9') ||
                (test == '\xA') ||
                (test == '\xD') ||
                (test >= '\x20' && test <= '\xD7FF') ||
                (test >= '\xE000' && test <= '\xFFFD')
               )
                return true;

            return false;
        }

        internal static unsafe bool IsChar(char* test)
        {
            //[2]    Char    ::=    #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]  
            if (
                (*test >= '\x20' && *test <= '\xD7FF') ||
                (*test >= '\xE000' && *test <= '\xFFFD') ||
                (*test == '\x9') ||
                (*test == '\xA') ||
                (*test == '\xD')
               )
                return true;

            return false;
        }

        internal static bool IsPubidChar(char test)
        {
            //[13]    PubidChar         ::=    #x20 | #xD | #xA | [a-zA-Z0-9] | [-'()+,./:=?;!*#@$_%] 

            if (
                (test == '\x20') ||
                (test == '\xA') ||
                (test == '\xD') ||
                (test >= 'a' && test <= 'z') ||
                (test >= 'A' && test <= 'Z') ||
                (test >= '0' && test <= '9') ||
                (test == '-') ||
                (test == '\'') ||
                (test == '(') ||
                (test == ')') || 
                (test == '+') ||
                (test == ',') ||
                (test == '.') ||
                (test == '/') ||
                (test == ':') ||
                (test == '=') ||
                (test == '?') ||
                (test == ';') ||
                (test == '!') ||
                (test == '*') ||
                (test == '#') ||
                (test == '@') ||
                (test == '$') ||
                (test == '_') ||
                (test == '%')
               )
                return true;

            return false;
        }

        internal static unsafe bool IsPubidChar(char* test)
        {
            //[13]    PubidChar         ::=    #x20 | #xD | #xA | [a-zA-Z0-9] | [-'()+,./:=?;!*#@$_%] 

            if (
                (*test == '\x20') ||
                (*test == '\xA') ||
                (*test == '\xD') ||
                (*test >= 'a' && *test <= 'z') ||
                (*test >= 'A' && *test <= 'Z') ||
                (*test >= '0' && *test <= '9') ||
                (*test == '-') ||
                (*test == '\'') ||
                (*test == '(') ||
                (*test == ')') ||
                (*test == '+') ||
                (*test == ',') ||
                (*test == '.') ||
                (*test == '/') ||
                (*test == ':') ||
                (*test == '=') ||
                (*test == '?') ||
                (*test == ';') ||
                (*test == '!') ||
                (*test == '*') ||
                (*test == '#') ||
                (*test == '@') ||
                (*test == '$') ||
                (*test == '_') ||
                (*test == '%')
               )
                return true;

            return false;
        }

        internal static bool IsNameChar(char test)
        {
            //[4]    NameChar    ::=    Letter | Digit | '.' | '-' | '_' | ':' | CombiningChar | Extender  
            return IsLetter(test) || 
                   IsDigit(test) ||
                   (test == '.') ||
                   (test == '-') ||
                   (test == '_') ||
                   (test == ':') ||
                   IsCombiningChar(test) || 
                   IsExtender(test);
        }

        internal static unsafe bool IsNameChar(char* test)
        {
            //[4]    NameChar    ::=    Letter | Digit | '.' | '-' | '_' | ':' | CombiningChar | Extender  
            return IsLetter(test) ||
                   IsDigit(test) ||
                   (*test == '.') ||
                   (*test == '-') ||
                   (*test == '_') ||
                   (*test == ':') ||
                   IsCombiningChar(test) ||
                   IsExtender(test);
        }


        internal static bool IsDigit(char test)
        {
            //[88]    Digit    ::=    [#x0030-#x0039] | [#x0660-#x0669] | [#x06F0-#x06F9] | [#x0966-#x096F] | 
            //                        [#x09E6-#x09EF] | [#x0A66-#x0A6F] | [#x0AE6-#x0AEF] | [#x0B66-#x0B6F] | 
            //                        [#x0BE7-#x0BEF] | [#x0C66-#x0C6F] | [#x0CE6-#x0CEF] | [#x0D66-#x0D6F] | 
            //                        [#x0E50-#x0E59] | [#x0ED0-#x0ED9] | [#x0F20-#x0F29]   

            if (
                (test >= '\x0030' && test <= '\x0039') ||
                (test >= '\x0660' && test <= '\x0669') ||
                (test >= '\x06F0' && test <= '\x06F9') ||
                (test >= '\x0966' && test <= '\x096F') ||
                (test >= '\x09E6' && test <= '\x09EF') ||
                (test >= '\x0A66' && test <= '\x0A6F') ||
                (test >= '\x0AE6' && test <= '\x0AEF') ||
                (test >= '\x0B66' && test <= '\x0B6F') ||
                (test >= '\x0BE7' && test <= '\x0BEF') ||
                (test >= '\x0C66' && test <= '\x0C6F') ||
                (test >= '\x0CE6' && test <= '\x0CEF') ||
                (test >= '\x0D66' && test <= '\x0D6F') ||
                (test >= '\x0E50' && test <= '\x0E59') ||
                (test >= '\x0ED0' && test <= '\x0ED9') ||
                (test >= '\x0F20' && test <= '\x0F29')
               )
                return true;

            return false;
        }

        internal static unsafe bool IsDigit(char* test)
        {
            //[88]    Digit    ::=    [#x0030-#x0039] | [#x0660-#x0669] | [#x06F0-#x06F9] | [#x0966-#x096F] | 
            //                        [#x09E6-#x09EF] | [#x0A66-#x0A6F] | [#x0AE6-#x0AEF] | [#x0B66-#x0B6F] | 
            //                        [#x0BE7-#x0BEF] | [#x0C66-#x0C6F] | [#x0CE6-#x0CEF] | [#x0D66-#x0D6F] | 
            //                        [#x0E50-#x0E59] | [#x0ED0-#x0ED9] | [#x0F20-#x0F29]   

            if (
                (*test >= '\x0030' && *test <= '\x0039') ||
                (*test >= '\x0660' && *test <= '\x0669') ||
                (*test >= '\x06F0' && *test <= '\x06F9') ||
                (*test >= '\x0966' && *test <= '\x096F') ||
                (*test >= '\x09E6' && *test <= '\x09EF') ||
                (*test >= '\x0A66' && *test <= '\x0A6F') ||
                (*test >= '\x0AE6' && *test <= '\x0AEF') ||
                (*test >= '\x0B66' && *test <= '\x0B6F') ||
                (*test >= '\x0BE7' && *test <= '\x0BEF') ||
                (*test >= '\x0C66' && *test <= '\x0C6F') ||
                (*test >= '\x0CE6' && *test <= '\x0CEF') ||
                (*test >= '\x0D66' && *test <= '\x0D6F') ||
                (*test >= '\x0E50' && *test <= '\x0E59') ||
                (*test >= '\x0ED0' && *test <= '\x0ED9') ||
                (*test >= '\x0F20' && *test <= '\x0F29')
               )
                return true;

            return false;
        }

        internal static bool IsExtender(char test)
        {
            //[89]    Extender    ::=    #x00B7 | #x02D0 | #x02D1 | #x0387 | #x0640 | #x0E46 | #x0EC6 | 
            //                           #x3005 | [#x3031-#x3035] | [#x309D-#x309E] | [#x30FC-#x30FE] 

            if (
                (test == '\x00B7') ||
                (test == '\x02D0') ||
                (test == '\x02D1') ||
                (test == '\x0387') ||
                (test == '\x0640') ||
                (test == '\x0E46') ||
                (test == '\x0EC6') ||
                (test == '\x3005') ||
                (test >= '\x3031' && test <= '\x3035') ||
                (test >= '\x309D' && test <= '\x309E') ||
                (test >= '\x30FC' && test <= '\x30FE')
               )
                return true;

            return false;
        }

        internal static unsafe bool IsExtender(char* test)
        {
            //[89]    Extender    ::=    #x00B7 | #x02D0 | #x02D1 | #x0387 | #x0640 | #x0E46 | #x0EC6 | 
            //                           #x3005 | [#x3031-#x3035] | [#x309D-#x309E] | [#x30FC-#x30FE] 

            if (
                (*test == '\x00B7') ||
                (*test == '\x02D0') ||
                (*test == '\x02D1') ||
                (*test == '\x0387') ||
                (*test == '\x0640') ||
                (*test == '\x0E46') ||
                (*test == '\x0EC6') ||
                (*test == '\x3005') ||
                (*test >= '\x3031' && *test <= '\x3035') ||
                (*test >= '\x309D' && *test <= '\x309E') ||
                (*test >= '\x30FC' && *test <= '\x30FE')
               )
                return true;

            return false;
        }


        internal static bool IsCombiningChar(char test)
        {
            //[87]    CombiningChar    ::=    [#x0300-#x0345] | [#x0360-#x0361] | [#x0483-#x0486] | [#x0591-#x05A1] | 
            //                                [#x05A3-#x05B9] | [#x05BB-#x05BD] | #x05BF | [#x05C1-#x05C2] | #x05C4 | 
            //                                [#x064B-#x0652] | #x0670 | [#x06D6-#x06DC] | [#x06DD-#x06DF] | 
            //                                [#x06E0-#x06E4] | [#x06E7-#x06E8] | [#x06EA-#x06ED] | [#x0901-#x0903] | 
            //                                #x093C | [#x093E-#x094C] | #x094D | [#x0951-#x0954] | [#x0962-#x0963] | 
            //                                [#x0981-#x0983] | #x09BC | #x09BE | #x09BF | [#x09C0-#x09C4] | 
            //                                [#x09C7-#x09C8] | [#x09CB-#x09CD] | #x09D7 | [#x09E2-#x09E3] | #x0A02 | 
            //                                #x0A3C | #x0A3E | #x0A3F | [#x0A40-#x0A42] | [#x0A47-#x0A48] | 
            //                                [#x0A4B-#x0A4D] | [#x0A70-#x0A71] | [#x0A81-#x0A83] | #x0ABC | 
            //                                [#x0ABE-#x0AC5] | [#x0AC7-#x0AC9] | [#x0ACB-#x0ACD] | [#x0B01-#x0B03] | 
            //                                #x0B3C | [#x0B3E-#x0B43] | [#x0B47-#x0B48] | [#x0B4B-#x0B4D] | 
            //                                [#x0B56-#x0B57] | [#x0B82-#x0B83] | [#x0BBE-#x0BC2] | [#x0BC6-#x0BC8] | 
            //                                [#x0BCA-#x0BCD] | #x0BD7 | [#x0C01-#x0C03] | [#x0C3E-#x0C44] | 
            //                                [#x0C46-#x0C48] | [#x0C4A-#x0C4D] | [#x0C55-#x0C56] | [#x0C82-#x0C83] | 
            //                                [#x0CBE-#x0CC4] | [#x0CC6-#x0CC8] | [#x0CCA-#x0CCD] | [#x0CD5-#x0CD6] | 
            //                                [#x0D02-#x0D03] | [#x0D3E-#x0D43] | [#x0D46-#x0D48] | [#x0D4A-#x0D4D] | 
            //                                #x0D57 | #x0E31 | [#x0E34-#x0E3A] | [#x0E47-#x0E4E] | #x0EB1 | 
            //                                [#x0EB4-#x0EB9] | [#x0EBB-#x0EBC] | [#x0EC8-#x0ECD] | [#x0F18-#x0F19] | 
            //                                #x0F35 | #x0F37 | #x0F39 | #x0F3E | #x0F3F | [#x0F71-#x0F84] | 
            //                                [#x0F86-#x0F8B] | [#x0F90-#x0F95] | #x0F97 | [#x0F99-#x0FAD] | 
            //                                [#x0FB1-#x0FB7] | #x0FB9 | [#x20D0-#x20DC] | #x20E1 | [#x302A-#x302F] |
            //                                #x3099 | #x309A  

            if (
                (test >= '\x0300' && test <= '\x0345') ||
                (test >= '\x0360' && test <= '\x0361') ||
                (test >= '\x0483' && test <= '\x0486') ||
                (test >= '\x0591' && test <= '\x05A1') ||
                (test >= '\x05A3' && test <= '\x05B9') ||
                (test >= '\x05BB' && test <= '\x05BD') ||
                (test == '\x05BF') ||
                (test >= '\x05C1' && test <= '\x05C2') ||
                (test == '\x05C4') ||
                (test >= '\x064B' && test <= '\x0652') ||
                (test == '\x0670') ||
                (test >= '\x06D6' && test <= '\x06DC') ||
                (test >= '\x06DD' && test <= '\x06DF') ||
                (test >= '\x06E0' && test <= '\x06E4') ||
                (test >= '\x06E7' && test <= '\x06E8') ||
                (test >= '\x06EA' && test <= '\x06ED') ||
                (test >= '\x0901' && test <= '\x0903') ||
                (test == '\x093C') || 
                (test >= '\x093E' && test <= '\x094C') ||
                (test == '\x094D') ||
                (test >= '\x0951' && test <= '\x0954') ||
                (test >= '\x0962' && test <= '\x0963') ||
                (test >= '\x0981' && test <= '\x0983') ||
                (test == '\x09BC') ||
                (test == '\x09BE') ||
                (test == '\x09BF') ||
                (test >= '\x09C0' && test <= '\x09C4') ||
                (test >= '\x09C7' && test <= '\x09C8') ||
                (test >= '\x09CB' && test <= '\x09CD') ||
                (test == '\x09D7') ||
                (test >= '\x09E2' && test <= '\x09E3') ||
                (test == '\x0A02') ||
                (test == '\x0A3C') ||
                (test == '\x0A3E') ||
                (test == '\x0A3F') ||
                (test >= '\x0A40' && test <= '\x0A42') ||
                (test >= '\x0A47' && test <= '\x0A48') ||
                (test >= '\x0A4B' && test <= '\x0A4D') ||
                (test >= '\x0A70' && test <= '\x0A71') ||
                (test >= '\x0A81' && test <= '\x0A83') ||
                (test == '\x0ABC') ||
                (test >= '\x0ABE' && test <= '\x0AC5') ||
                (test >= '\x0AC7' && test <= '\x0AC9') ||
                (test >= '\x0ACB' && test <= '\x0ACD') ||
                (test >= '\x0B01' && test <= '\x0B03') ||
                (test == '\x0B3C') ||
                (test >= '\x0B3E' && test <= '\x0B43') ||
                (test >= '\x0B47' && test <= '\x0B48') ||
                (test >= '\x0B4B' && test <= '\x0B4D') ||
                (test >= '\x0B56' && test <= '\x0B57') ||
                (test >= '\x0B82' && test <= '\x0B83') ||
                (test >= '\x0BBE' && test <= '\x0BC2') ||
                (test >= '\x0BC6' && test <= '\x0BC8') ||
                (test >= '\x0BCA' && test <= '\x0BCD') ||
                (test == '\x0BD7') ||
                (test >= '\x0C01' && test <= '\x0C03') ||
                (test >= '\x0C3E' && test <= '\x0C44') ||
                (test >= '\x0C46' && test <= '\x0C48') ||
                (test >= '\x0C4A' && test <= '\x0C4D') ||
                (test >= '\x0C55' && test <= '\x0C56') ||
                (test >= '\x0C82' && test <= '\x0C83') ||
                (test >= '\x0CBE' && test <= '\x0CC4') ||
                (test >= '\x0CC6' && test <= '\x0CC8') ||
                (test >= '\x0CCA' && test <= '\x0CCD') ||
                (test >= '\x0CD5' && test <= '\x0CD6') ||
                (test >= '\x0D02' && test <= '\x0D03') ||
                (test >= '\x0D3E' && test <= '\x0D43') ||
                (test >= '\x0D46' && test <= '\x0D48') ||
                (test >= '\x0D4A' && test <= '\x0D4D') ||
                (test == '\x0D57') ||
                (test == '\x0E31') ||
                (test >= '\x0E34' && test <= '\x0E3A') ||
                (test >= '\x0E47' && test <= '\x0E4E') ||
                (test == '\x0EB1') ||
                (test >= '\x0EB4' && test <= '\x0EB9') ||
                (test >= '\x0EBB' && test <= '\x0EBC') ||
                (test >= '\x0EC8' && test <= '\x0ECD') ||
                (test >= '\x0F18' && test <= '\x0F19') ||
                (test == '\x0F35') ||
                (test == '\x0F37') ||
                (test == '\x0F39') ||
                (test == '\x0F3E') ||
                (test == '\x0F3F') ||
                (test >= '\x0F71' && test <= '\x0F84') ||
                (test >= '\x0F86' && test <= '\x0F8B') ||
                (test >= '\x0F90' && test <= '\x0F95') ||
                (test == '\x0F97') ||
                (test >= '\x0F99' && test <= '\x0FAD') ||
                (test >= '\x0FB1' && test <= '\x0FB7') ||
                (test == '\x0FB9') ||
                (test >= '\x20D0' && test <= '\x20DC') ||
                (test == '\x20E1') ||
                (test >= '\x302A' && test <= '\x302F') ||
                (test == '\x3099') ||
                (test == '\x309A')
               )
                return true;

            return false;
        }

        internal static unsafe bool IsCombiningChar(char* test)
        {
            //[87]    CombiningChar    ::=    [#x0300-#x0345] | [#x0360-#x0361] | [#x0483-#x0486] | [#x0591-#x05A1] | 
            //                                [#x05A3-#x05B9] | [#x05BB-#x05BD] | #x05BF | [#x05C1-#x05C2] | #x05C4 | 
            //                                [#x064B-#x0652] | #x0670 | [#x06D6-#x06DC] | [#x06DD-#x06DF] | 
            //                                [#x06E0-#x06E4] | [#x06E7-#x06E8] | [#x06EA-#x06ED] | [#x0901-#x0903] | 
            //                                #x093C | [#x093E-#x094C] | #x094D | [#x0951-#x0954] | [#x0962-#x0963] | 
            //                                [#x0981-#x0983] | #x09BC | #x09BE | #x09BF | [#x09C0-#x09C4] | 
            //                                [#x09C7-#x09C8] | [#x09CB-#x09CD] | #x09D7 | [#x09E2-#x09E3] | #x0A02 | 
            //                                #x0A3C | #x0A3E | #x0A3F | [#x0A40-#x0A42] | [#x0A47-#x0A48] | 
            //                                [#x0A4B-#x0A4D] | [#x0A70-#x0A71] | [#x0A81-#x0A83] | #x0ABC | 
            //                                [#x0ABE-#x0AC5] | [#x0AC7-#x0AC9] | [#x0ACB-#x0ACD] | [#x0B01-#x0B03] | 
            //                                #x0B3C | [#x0B3E-#x0B43] | [#x0B47-#x0B48] | [#x0B4B-#x0B4D] | 
            //                                [#x0B56-#x0B57] | [#x0B82-#x0B83] | [#x0BBE-#x0BC2] | [#x0BC6-#x0BC8] | 
            //                                [#x0BCA-#x0BCD] | #x0BD7 | [#x0C01-#x0C03] | [#x0C3E-#x0C44] | 
            //                                [#x0C46-#x0C48] | [#x0C4A-#x0C4D] | [#x0C55-#x0C56] | [#x0C82-#x0C83] | 
            //                                [#x0CBE-#x0CC4] | [#x0CC6-#x0CC8] | [#x0CCA-#x0CCD] | [#x0CD5-#x0CD6] | 
            //                                [#x0D02-#x0D03] | [#x0D3E-#x0D43] | [#x0D46-#x0D48] | [#x0D4A-#x0D4D] | 
            //                                #x0D57 | #x0E31 | [#x0E34-#x0E3A] | [#x0E47-#x0E4E] | #x0EB1 | 
            //                                [#x0EB4-#x0EB9] | [#x0EBB-#x0EBC] | [#x0EC8-#x0ECD] | [#x0F18-#x0F19] | 
            //                                #x0F35 | #x0F37 | #x0F39 | #x0F3E | #x0F3F | [#x0F71-#x0F84] | 
            //                                [#x0F86-#x0F8B] | [#x0F90-#x0F95] | #x0F97 | [#x0F99-#x0FAD] | 
            //                                [#x0FB1-#x0FB7] | #x0FB9 | [#x20D0-#x20DC] | #x20E1 | [#x302A-#x302F] |
            //                                #x3099 | #x309A  

            if (
                (*test >= '\x0300' && *test <= '\x0345') ||
                (*test >= '\x0360' && *test <= '\x0361') ||
                (*test >= '\x0483' && *test <= '\x0486') ||
                (*test >= '\x0591' && *test <= '\x05A1') ||
                (*test >= '\x05A3' && *test <= '\x05B9') ||
                (*test >= '\x05BB' && *test <= '\x05BD') ||
                (*test == '\x05BF') ||
                (*test >= '\x05C1' && *test <= '\x05C2') ||
                (*test == '\x05C4') ||
                (*test >= '\x064B' && *test <= '\x0652') ||
                (*test == '\x0670') ||
                (*test >= '\x06D6' && *test <= '\x06DC') ||
                (*test >= '\x06DD' && *test <= '\x06DF') ||
                (*test >= '\x06E0' && *test <= '\x06E4') ||
                (*test >= '\x06E7' && *test <= '\x06E8') ||
                (*test >= '\x06EA' && *test <= '\x06ED') ||
                (*test >= '\x0901' && *test <= '\x0903') ||
                (*test == '\x093C') ||
                (*test >= '\x093E' && *test <= '\x094C') ||
                (*test == '\x094D') ||
                (*test >= '\x0951' && *test <= '\x0954') ||
                (*test >= '\x0962' && *test <= '\x0963') ||
                (*test >= '\x0981' && *test <= '\x0983') ||
                (*test == '\x09BC') ||
                (*test == '\x09BE') ||
                (*test == '\x09BF') ||
                (*test >= '\x09C0' && *test <= '\x09C4') ||
                (*test >= '\x09C7' && *test <= '\x09C8') ||
                (*test >= '\x09CB' && *test <= '\x09CD') ||
                (*test == '\x09D7') ||
                (*test >= '\x09E2' && *test <= '\x09E3') ||
                (*test == '\x0A02') ||
                (*test == '\x0A3C') ||
                (*test == '\x0A3E') ||
                (*test == '\x0A3F') ||
                (*test >= '\x0A40' && *test <= '\x0A42') ||
                (*test >= '\x0A47' && *test <= '\x0A48') ||
                (*test >= '\x0A4B' && *test <= '\x0A4D') ||
                (*test >= '\x0A70' && *test <= '\x0A71') ||
                (*test >= '\x0A81' && *test <= '\x0A83') ||
                (*test == '\x0ABC') ||
                (*test >= '\x0ABE' && *test <= '\x0AC5') ||
                (*test >= '\x0AC7' && *test <= '\x0AC9') ||
                (*test >= '\x0ACB' && *test <= '\x0ACD') ||
                (*test >= '\x0B01' && *test <= '\x0B03') ||
                (*test == '\x0B3C') ||
                (*test >= '\x0B3E' && *test <= '\x0B43') ||
                (*test >= '\x0B47' && *test <= '\x0B48') ||
                (*test >= '\x0B4B' && *test <= '\x0B4D') ||
                (*test >= '\x0B56' && *test <= '\x0B57') ||
                (*test >= '\x0B82' && *test <= '\x0B83') ||
                (*test >= '\x0BBE' && *test <= '\x0BC2') ||
                (*test >= '\x0BC6' && *test <= '\x0BC8') ||
                (*test >= '\x0BCA' && *test <= '\x0BCD') ||
                (*test == '\x0BD7') ||
                (*test >= '\x0C01' && *test <= '\x0C03') ||
                (*test >= '\x0C3E' && *test <= '\x0C44') ||
                (*test >= '\x0C46' && *test <= '\x0C48') ||
                (*test >= '\x0C4A' && *test <= '\x0C4D') ||
                (*test >= '\x0C55' && *test <= '\x0C56') ||
                (*test >= '\x0C82' && *test <= '\x0C83') ||
                (*test >= '\x0CBE' && *test <= '\x0CC4') ||
                (*test >= '\x0CC6' && *test <= '\x0CC8') ||
                (*test >= '\x0CCA' && *test <= '\x0CCD') ||
                (*test >= '\x0CD5' && *test <= '\x0CD6') ||
                (*test >= '\x0D02' && *test <= '\x0D03') ||
                (*test >= '\x0D3E' && *test <= '\x0D43') ||
                (*test >= '\x0D46' && *test <= '\x0D48') ||
                (*test >= '\x0D4A' && *test <= '\x0D4D') ||
                (*test == '\x0D57') ||
                (*test == '\x0E31') ||
                (*test >= '\x0E34' && *test <= '\x0E3A') ||
                (*test >= '\x0E47' && *test <= '\x0E4E') ||
                (*test == '\x0EB1') ||
                (*test >= '\x0EB4' && *test <= '\x0EB9') ||
                (*test >= '\x0EBB' && *test <= '\x0EBC') ||
                (*test >= '\x0EC8' && *test <= '\x0ECD') ||
                (*test >= '\x0F18' && *test <= '\x0F19') ||
                (*test == '\x0F35') ||
                (*test == '\x0F37') ||
                (*test == '\x0F39') ||
                (*test == '\x0F3E') ||
                (*test == '\x0F3F') ||
                (*test >= '\x0F71' && *test <= '\x0F84') ||
                (*test >= '\x0F86' && *test <= '\x0F8B') ||
                (*test >= '\x0F90' && *test <= '\x0F95') ||
                (*test == '\x0F97') ||
                (*test >= '\x0F99' && *test <= '\x0FAD') ||
                (*test >= '\x0FB1' && *test <= '\x0FB7') ||
                (*test == '\x0FB9') ||
                (*test >= '\x20D0' && *test <= '\x20DC') ||
                (*test == '\x20E1') ||
                (*test >= '\x302A' && *test <= '\x302F') ||
                (*test == '\x3099') ||
                (*test == '\x309A')
               )
                return true;

            return false;
        }
    }

}
