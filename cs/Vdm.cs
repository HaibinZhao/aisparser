using System;
using System.Text;
using System.Text.RegularExpressions;

namespace AisParser {
    internal class ChecksumFailedException : Exception {
        public ChecksumFailedException() {
        }

        public ChecksumFailedException(string str) : base(str) {
        }
    }


    internal class VDMSentenceException : Exception {
        public VDMSentenceException() {
        }

        public VDMSentenceException(string str) : base(str) {
        }
    }

    /// <summary>
    ///     VDM Class
    ///     This keeps track partial messages until a complete message has been
    ///     received and it holds the sixbit state for exteacting bits from the
    ///     message.
    /// </summary>
    public class Vdm {
        /*
		 * Constructor, initialize the state
		 */
        public Vdm() {
            Total = 0;
            Sequence = 0;
            Num = 0;
        }

        /// <summary>
        ///     !&lt; Message ID 0-31
        /// </summary>
        public int Msgid { get; private set; }

        /// <summary>
        ///     !&lt; VDM message sequence number
        /// </summary>
        public int Sequence { get; private set; }

        /// <summary>
        ///     !&lt; Total # of parts for the message
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        ///     !&lt; Number of the last part stored
        /// </summary>
        public int Num { get; private set; }

        /// <summary>
        ///     !&lt; AIS Channel character
        /// </summary>
        public char Channel { get; private set; }

        /// <summary>
        ///     !&lt; sixbit parser state
        /// </summary>
        public Sixbit SixState { get; private set; }


        /// <summary>
        ///     Assemble AIVDM/VDO sentences
        ///     This function handles re-assembly and extraction of the 6-bit data
        ///     from AIVDM/AIVDO sentences.
        ///     Because the NMEA standard limits the length of a line to 80 characters
        ///     some AIS messages, such as message 5, are output as a multipart VDM
        ///     messages.
        ///     This routine collects the 6-bit encoded data from these parts and
        ///     returns a 1 when all pieces have been reassembled.
        ///     It expects the sentences to:
        ///     - Be in order, part 1, part 2, etc.
        ///     - Be from a single sequence
        ///     It will return an error if it receives a piece out of order or from
        ///     a new sequence before the previous one is finished.
        /// </summary>
        /// <returns>
        ///     - 0 Complete packet
        ///     - 1 Incomplete packet
        ///     - 2 NMEA 0183 checksum failed
        ///     - 3 Not an AIS message
        ///     - 4 Error with nmea_next_field
        ///     - 5 Out of sequence packet
        /// </returns>
        /// <exception cref="ChecksumFailedException"></exception>
        /// <exception cref="StartNotFoundException"></exception>
        /// <exception cref="VDMSentenceException"></exception>
        public virtual int Add(string str) {
            var nmeaMessage = new Nmea();
            int total;
            int num;
            int sequence;
            nmeaMessage.Init(str);

            if (nmeaMessage.CheckChecksum() != 0) throw new ChecksumFailedException();
            var ptr = nmeaMessage.FindStart();

            // Allow any sender type for VDM and VDO messages
            //if (!str.regionMatches(ptr + 3, "VDM", 0, 3) && !str.regionMatches(ptr + 3, "VDO", 0, 3))
            var tag = str.Substring(ptr + 3, 3);
            if (!tag.Equals("VDM") && !tag.Equals("VDO")) throw new VDMSentenceException("Not a VDM or VDO message");

            var fields = str.Split(",|\\*", true);
            if (fields.Length != 8) throw new VDMSentenceException("Does not have 8 fields");

            // Get the message info for multipart messages
            try {
                total = int.Parse(fields[1]);
                num = int.Parse(fields[2]);
            } catch (FormatException) {
                throw new VDMSentenceException("total or num field is not an integer");
            }

            try {
                sequence = int.Parse(fields[3]);
            } catch (FormatException) {
                // null sequence is not fatal
                sequence = 0;
            }

            // Are we looking for more message parts?
            if (Total > 0) {
                if (Sequence != sequence || Num != num - 1) {
                    Total = 0;
                    Sequence = 0;
                    Num = 0;
                    throw new VDMSentenceException("Out of sequence sentence");
                }

                Num++;
            } else {
                Total = total;
                Num = num;
                Sequence = sequence;
                SixState = new Sixbit();
                SixState.Init("");
            }

            Channel = fields[4][0];
            SixState.add(fields[5]);

            if (total == 0 || Total == num) {
                Total = 0;
                Num = 0;
                Sequence = 0;
                // Get the message id
                try {
                    Msgid = (int) SixState.Get(6);
                } catch (SixbitsExhaustedException) {
                    throw new VDMSentenceException("Not enough bits for msgid");
                }

                // Adjust bit count
                SixState.padBits(int.Parse(fields[6]));
                // Found a complete packet
                return 0;
            }

            // No complete message yet
            return 1;
        }
    }

    //-------------------------------------------------------------------------------------------
    //	Copyright © 2007 - 2017 Tangible Software Solutions Inc.
    //	This class can be used by anyone provided that the copyright notice remains intact.
    //
    //	This class is used to convert some aspects of the Java String class.
    //-------------------------------------------------------------------------------------------
    internal static class StringHelperClass {
        //----------------------------------------------------------------------------------
        //	This method replaces the Java String.substring method when 'start' is a
        //	method call or calculated value to ensure that 'start' is obtained just once.
        //----------------------------------------------------------------------------------
        internal static string SubstringSpecial(this string self, int start, int end) {
            return self.Substring(start, end - start);
        }

        //------------------------------------------------------------------------------------
        //	This method is used to replace calls to the 2-arg Java String.startsWith method.
        //------------------------------------------------------------------------------------
        internal static bool StartsWith(this string self, string prefix, int toffset) {
            return self.IndexOf(prefix, toffset, StringComparison.Ordinal) == toffset;
        }

        //------------------------------------------------------------------------------
        //	This method is used to replace most calls to the Java String.split method.
        //------------------------------------------------------------------------------
        internal static string[] Split(this string self, string regexDelimiter, bool trimTrailingEmptyStrings) {
            var splitArray = Regex.Split(self, regexDelimiter);

            if (trimTrailingEmptyStrings)
                if (splitArray.Length > 1)
                    for (var i = splitArray.Length; i > 0; i--)
                        if (splitArray[i - 1].Length > 0) {
                            if (i < splitArray.Length)
                                Array.Resize(ref splitArray, i);

                            break;
                        }

            return splitArray;
        }

        //-----------------------------------------------------------------------------
        //	These methods are used to replace calls to some Java String constructors.
        //-----------------------------------------------------------------------------
        internal static string NewString(sbyte[] bytes) {
            return NewString(bytes, 0, bytes.Length);
        }

        internal static string NewString(sbyte[] bytes, int index, int count) {
            return Encoding.UTF8.GetString((byte[]) (object) bytes, index, count);
        }

        internal static string NewString(sbyte[] bytes, string encoding) {
            return NewString(bytes, 0, bytes.Length, encoding);
        }

        internal static string NewString(sbyte[] bytes, int index, int count, string encoding) {
            return Encoding.GetEncoding(encoding).GetString((byte[]) (object) bytes, index, count);
        }

        //--------------------------------------------------------------------------------
        //	These methods are used to replace calls to the Java String.getBytes methods.
        //--------------------------------------------------------------------------------
        internal static sbyte[] GetBytes(this string self) {
            return GetSBytesForEncoding(Encoding.UTF8, self);
        }

        internal static sbyte[] GetBytes(this string self, Encoding encoding) {
            return GetSBytesForEncoding(encoding, self);
        }

        internal static sbyte[] GetBytes(this string self, string encoding) {
            return GetSBytesForEncoding(Encoding.GetEncoding(encoding), self);
        }

        private static sbyte[] GetSBytesForEncoding(Encoding encoding, string s) {
            var sbytes = new sbyte[encoding.GetByteCount(s)];
            encoding.GetBytes(s, 0, s.Length, (byte[]) (object) sbytes, 0);
            return sbytes;
        }
    }
}