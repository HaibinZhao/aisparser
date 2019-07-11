using System;

namespace AisParser {
    /// <summary>
    ///     6-bit packed ASCII functions
    ///     @author Copyright 2006-2008 by Brian C. Lane
    ///     bcl@ brianlane.com
    ///     All Rights Reserved
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
        private readonly int[] _pow2Mask = {0x00, 0x01, 0x03, 0x07, 0x0F, 0x1F, 0x3F};

        /// <summary>
        ///     !&lt; raw 6- bit ASCII data string
        /// </summary>
        private string _bits;

        /// <summary>
        ///     !&lt; Index of next character
        /// </summary>
        private int _bitsIndex;

        /// <summary>
        ///     !&lt; Number of padding bits at end
        /// </summary>
        private int _padBits;

        /// <summary>
        ///     !&lt; Remainder bits
        /// </summary>
        private int _remainder;

        /// <summary>
        ///     !&lt; Number of remainder bits
        /// </summary>
        private int _remainderLength;

        /// <summary>
        ///     Return the number of bytes in the sixbit string
        /// </summary>
        public int Length => _bits.Length;

        public Sixbit() :this(string.Empty){
        }

        public Sixbit(string bits) {
            _bits = bits;
            _bitsIndex = 0;
            _remainder = 0;
            _remainderLength = 0;
            _padBits = 0;
        }

        /// <summary>
        ///     Initialize a 6-bit datastream structure
        ///     This function initializes the state of the sixbit parser variables
        /// </summary>
        public void Init(string bits) {
            _bits = bits;
            _bitsIndex = 0;
            _remainder = 0;
            _remainderLength = 0;
            _padBits = 0;
        }

        /// <summary>
        ///     Set the bit padding value
        /// </summary>
        public void PadBits(int num) {
            _padBits = num;
        }

        /// <summary>
        ///     Add more bits to the buffer
        /// </summary>
        public void Add(string bits) {
            _bits += bits;
        }

        /// <summary>
        ///     Takes into account the number of padding bits.
        /// </summary>
        /// <returns>Return the number of bits</returns>
        public int BitLength => Length * 6 - _padBits;

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
        /// <exception cref="ArgumentException"></exception>
        public int Binfrom6Bit(int ascii) {
            if (ascii < 0x30 || ascii > 0x77 || ascii > 0x57 && ascii < 0x60)
                throw new SixbitsExhaustedException("Illegal 6-bit ASCII value");
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
        public int BinTo6Bit(int value) {
            if (value > 0x3F) throw new SixbitsExhaustedException("Value is out of range (0-0x3F)");
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
        public int Ais2Ascii(int value) {
            if (value > 0x3F) throw new SixbitsExhaustedException("Value is out of range (0-0x3F)");
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
        public long Get(int numbits) {
            long result = 0;
            var fetchBits = numbits;

            while (fetchBits > 0) {
                /*  Is there anything left over from the last call? */
                if (_remainderLength > 0)
                    if (_remainderLength <= fetchBits) {
                        /* reminder is less than or equal to what is needed */
                        result = (result << 6) + _remainder;
                        fetchBits -= _remainderLength;
                        _remainder = 0;
                        _remainderLength = 0;
                    } else {
                        // remainder is larger than what is needed
                        //Take the bits from the top of remainder
                        result = result << fetchBits;
                        result += _remainder >> (_remainderLength - fetchBits);

                        // Fixup remainder 
                        _remainderLength -= fetchBits;
                        _remainder &= _pow2Mask[_remainderLength];

                        return result;
                    }

                // Get the next block of 6 bits from the ASCII string 
                if (_bitsIndex < _bits.Length) {
                    _remainder = Binfrom6Bit(_bits[_bitsIndex]);
                    _bitsIndex++;
                    if (_bitsIndex == _bits.Length)
                        _remainderLength = 6 - _padBits;
                    else
                        _remainderLength = 6;
                } else if (fetchBits > 0) {
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
        public string GetString(int length) {
            var tmpStr = new char[length];
            int len = 0;
            // Get the 6-bit string, convert to ASCII
            for (var i = 0; i < length; i++){
                try {
                    var c = (char) Ais2Ascii((char) Get(6));
                    if(c == '@'){
                        //skip '@' char
                        continue;
                    }else{
                        tmpStr[i] = c;
                        len++;
                    }
                } catch (SixbitsExhaustedException) {
                    //for (var j = i; j < length; j++) tmpStr[j] = '@';
                    break;
                }
            }

            return new string(tmpStr,0,len);
        }
    }
}