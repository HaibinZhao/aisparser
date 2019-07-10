using System;
using System.Linq;

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
        /// <summary>
        ///     Constructor, initialize the state
        /// </summary>
        public Vdm() {
            Total = 0;
            Sequence = 0;
            Num = 0;
        }

        /// <summary>
        ///     !&lt; Message ID 0-31
        /// </summary>
        public int MsgId { get; private set; }

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
        public VdmStatus Add(string str) {
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
                    return (VdmStatus)5;
                    //throw new VDMSentenceException("Out of sequence sentence");
                }

                Num++;
            } else {
                Total = total;
                Num = num;
                Sequence = sequence;
                SixState = new Sixbit();
                SixState.Init("");
            }

            Channel = fields[4].FirstOrDefault();//[0];
            SixState.Add(fields[5]);

            if (total == 0 || Total == num) {
                Total = 0;
                Num = 0;
                Sequence = 0;
                // Get the message id
                try {
                    MsgId = (int) SixState.Get(6);
                } catch (SixbitsExhaustedException) {
                    throw new VDMSentenceException("Not enough bits for msgid");
                }

                // Adjust bit count
                SixState.PadBits(int.Parse(fields[6]));
                // Found a complete packet
                return 0;
            }

            // No complete message yet
            return (VdmStatus)1;
        }
    }

    public enum VdmStatus {
        /// <summary>
        ///     Complete packet
        /// </summary>
        Complete = 0,

        /// <summary>
        ///     Incomplete packet
        /// </summary>
        Incomplete = 1,

        /// <summary>
        ///     NMEA 0183 checksum failed
        /// </summary>
        ChecksumFailed = 2,

        /// <summary>
        ///     Not an AIS message
        /// </summary>
        NotAisMessage = 3,

        /// <summary>
        ///     Error with nmea_next_field
        /// </summary>
        NmeaNextError = 4,

        /// <summary>
        ///     - 5 Out of sequence packet
        /// </summary>
        OutofSequence = 5
    }
}