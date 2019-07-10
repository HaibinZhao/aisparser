using System;

namespace AisParser {
    /// <summary>
    ///     6-bit packed ASCII functions
    ///     @author Copyright 2006-2008 by Brian C. Lane
    ///     <bcl@ brianlane.com
    ///         All Rights Reserved
    /// </summary>
    internal class SixbitsExhaustedException : Exception {
        public SixbitsExhaustedException() {
        }

        public SixbitsExhaustedException(string str) : base(str) {
        }
    }


    /// <summary>
    ///     This class's methods are used to extract data from the 6-bit packed
    ///     ASCII string used by AIVDM/AIVDO AIS messages.
    ///     init() should be called with a sixbit ASCII string.
    ///     Up to 32 bits of data are fetched from the string by calling get()
    ///     Use _padBits() to set the number of padding bits at the end of the message,
    ///     it defaults to 0 if not set.
    /// </summary>
    public class Sixbit {
        private readonly int[] pow2_mask = {0x00, 0x01, 0x03, 0x07, 0x0F, 0x1F, 0x3F};

        /// <summary>
        ///     !< Number of padding bits at end
        /// </summary>
        private int _padBits;

        /// <summary>
        ///     !< raw 6- bit ASCII data string
        /// </summary>
        private string bits;

        /// <summary>
        ///     !< Index of next character
        /// </summary>
        private int bitsIndex;

        /// <summary>
        ///     !< Remainder bits
        /// </summary>
        private int remainder;

        /// <summary>
        ///     !< Number of remainder bits
        /// </summary>
        private int remainderLength;

        /// <summary>
        ///     Return the number of bytes in the sixbit string
        /// </summary>
        public virtual int Length => bits.Length;

        /// <summary>
        ///     Initialize a 6-bit datastream structure
        ///     This function initializes the state of the sixbit parser variables
        /// </summary>
        public virtual void Init(string bits) {
            this.bits = bits;
            bitsIndex = 0;
            remainder = 0;
            remainderLength = 0;
            _padBits = 0;
        }

        /// <summary>
        ///     Set the bit padding value
        /// </summary>
        public virtual void padBits(int num) {
            _padBits = num;
        }

        /// <summary>
        ///     Add more bits to the buffer
        /// </summary>
        public virtual void add(string bits) {
            this.bits += bits;
        }

        /// <summary>
        ///     Takes into account the number of padding bits.
        /// </summary>
        /// <returns>Return the number of bits</returns>
        public int BitLength() {
            return Length * 6 - _padBits;
        }

        /// <summary>
        ///     Convert an ASCII value to a 6-bit binary value
        ///     This function checks the ASCII value to make sure it can be converted.
        ///     If not, it throws an IllegalArgumentException
        ///     Otherwise it returns the 6-bit binary value.
        /// </summary>
        /// <param name="ascii">
        ///     character to convert
        ///     This is used to convert the packed 6-bit value to a binary value. It
        ///     is not used to convert data from fields such as the name and
        ///     destination -- Use ais2ascii() instead.
        /// </param>
        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public int binfrom6bit(int ascii) throws IllegalArgumentException
        public virtual int Binfrom6Bit(int ascii) {
            if (ascii < 0x30 || ascii > 0x77 || ascii > 0x57 && ascii < 0x60)
                throw new ArgumentException("Illegal 6-bit ASCII value");
            if (ascii < 0x60)
                return (ascii - 0x30) & 0x3F;
            return (ascii - 0x38) & 0x3F;
        }


        /// <summary>
        ///     Convert a binary value to a 6-bit ASCII value
        ///     This function checks the binary value to make sure it can be converted.
        ///     If not, it throws an IllegalArgumentException.
        ///     Otherwise it returns the 6-bit ASCII value.
        /// </summary>
        /// <param name="value">
        ///     to convert
        ///     @returns 6-bit ASCII
        /// </param>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public int binto6bit(int value) throws IllegalArgumentException
        public virtual int BinTo6Bit(int value) {
            if (value > 0x3F) throw new ArgumentException("Value is out of range (0-0x3F)");
            if (value < 0x28)
                return value + 0x30;
            return value + 0x38;
        }

        /// <summary>
        ///     Convert a AIS 6-bit character to ASCII
        /// </summary>
        /// <param name="value">
        ///     6-bit value to be converted
        ///     return
        ///     - corresponding ASCII value (0x20-0x5F)     *
        ///     This function is used to convert binary data to ASCII. This is
        ///     different from the 6-bit ASCII to binary conversion for VDM
        ///     messages; it is used for strings within the datastream itself.
        ///     eg. Ship Name, Callsign and Destination.
        /// </param>
        /// <exception cref="ArgumentException">value &gt; 0x3F</exception>
        public virtual int Ais2Ascii(int value) {
            if (value > 0x3F) throw new ArgumentException("Value is out of range (0-0x3F)");
            if (value < 0x20)
                return value + 0x40;
            return value;
        }

        /// <summary>
        ///     Return 0-32 bits from a 6-bit ASCII stream
        /// </summary>
        /// <param name="numbits">
        ///     number of bits to return
        ///     This method returns the requested number of bits to the caller.
        ///     It pulls the bits from the raw 6-bit ASCII as they are needed.
        /// </param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        public virtual long Get(int numbits) {
            long result = 0;
            var fetch_bits = numbits;

            while (fetch_bits > 0) {
                /*  Is there anything left over from the last call? */
                if (remainderLength > 0)
                    if (remainderLength <= fetch_bits) {
                        /* reminder is less than or equal to what is needed */
                        result = (result << 6) + remainder;
                        fetch_bits -= remainderLength;
                        remainder = 0;
                        remainderLength = 0;
                    } else {
                        // remainder is larger than what is needed
                        //Take the bits from the top of remainder
                        result = result << fetch_bits;
                        result += remainder >> (remainderLength - fetch_bits);

                        // Fixup remainder 
                        remainderLength -= fetch_bits;
                        remainder &= pow2_mask[remainderLength];

                        return result;
                    }

                // Get the next block of 6 bits from the ASCII string 
                if (bitsIndex < bits.Length) {
                    remainder = Binfrom6Bit(bits[bitsIndex]);
                    bitsIndex++;
                    if (bitsIndex == bits.Length)
                        remainderLength = 6 - _padBits;
                    else
                        remainderLength = 6;
                } else if (fetch_bits > 0) {
                    // Ran out of bits
                    throw new SixbitsExhaustedException("Ran out of bits");
                } else {
                    return result;
                }
            }

            return result;
        }


        /// <summary>
        ///     Get an ASCII string from the 6-bit data stream
        /// </summary>
        /// <param name="length"> Number of characters to retrieve </param>
        /// <returns> String of the characters</returns>
        public virtual string get_string(int length) {
            var tmp_str = new char[length];

            /* Get the 6-bit string, convert to ASCII */
            for (var i = 0; i < length; i++)
                try {
                    tmp_str[i] = (char) Ais2Ascii((char) Get(6));
                } catch (SixbitsExhaustedException) {
                    for (var j = i; j < length; j++) tmp_str[j] = '@';
                    break;
                }

            return new string(tmp_str);
        }
    }
}