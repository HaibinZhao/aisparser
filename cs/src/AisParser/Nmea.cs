using System;

namespace AisParser {
    internal class StartNotFoundException : Exception {
        public StartNotFoundException() {
        }

        public StartNotFoundException(string str) : base(str) {
        }
    }

    internal class IllegalNmeaCharacterException : Exception {
        public IllegalNmeaCharacterException() {
        }

        public IllegalNmeaCharacterException(string str) : base(str) {
        }
    }


    /// <summary>
    ///     NMEA message class
    ///     Provides methods for finding the start of a NMEA message,
    ///     and calculating the checksum of the message.
    /// </summary>
    public class Nmea {
        private int _checksum;
        private string _msg;

        /// <summary>
        ///     Initialize it with a NMEA message string
        /// </summary>
        /// <param name="msg"></param>
        public void Init(string msg) {
            _msg = msg;
        }

        #region Static methods

        /// <summary>
        /// find next Nmea filed
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="offset">field start pos</param>
        /// <returns></returns>
        public static string NextField(string msg,int offset = 0) {
            for (var i = offset; i < msg.Length; i++) {
                var c = msg[i];
                if (c == ',' || c == '*') {
                    return msg.Substring(offset, i - offset);
                }
            }

            return null;
        }

        /// <summary>
        ///     Find the start of a NMEA sentence '!' or '$' character
        ///     Returns:
        ///     - index of the start character
        ///     The NMEA sentence may not always start at the beginning of the buffer,
        ///     use this routine to make sure the start of the sentence has been found.
        /// </summary>
        /// <returns>index of the start character</returns>
        /// <exception cref="StartNotFoundException"></exception>
        public static int FindStart(ref string msg) {
            var i = 0;
            foreach (var x in msg) {
                if (x == '!' || x == '$') return i;
                i++;
            }

            throw new StartNotFoundException("NMEA Start Not Found");
        }

        /// <summary>
        ///     Calculate the checksum of a NMEA 0183 sentence
        ///     This method will calculate the checksum of the string by skipping the
        ///     ! or $ character and stopping at the * character or end of string.
        ///     The checksum is stored in this.checksum
        /// </summary>
        /// <exception cref="StartNotFoundException"></exception>
        /// <exception cref="IllegalNmeaCharacterException"></exception>
        public static int CalculateChecksum(string msg) {
            // Find start of sentence, after a '!' or '$'
            var ptr = FindStart(ref msg) + 1;

            var msgArray = msg.Substring(ptr);

            var checksum = 0;
            foreach (var c in msgArray) {
                if (c == '!' || c == '$')
                    throw new IllegalNmeaCharacterException("Start Character Found before Checksum");

                if (c == '*') break;

                checksum ^= c;
            }

            return checksum;
        }
        /// <summary>
        ///     Check and return the checksum of a NMEA 0183 sentence
        ///     This method calls calculateChecksum() and updates this.checksum
        /// </summary>
        /// <returns>
        ///     - 0 if it does match
        ///     - 1 if it does not match
        ///     - 2 if there was an error
        /// </returns>
        public static int CheckChecksum(string msg) {
            string msgChecksum;
            int checksum;
            try {
                checksum = CalculateChecksum(msg);
            } catch (StartNotFoundException) {
                return 2;
            } catch (IllegalNmeaCharacterException) {
                return 2;
            }

            var ptr = msg.IndexOf('*');
            if (ptr < 0) return 2;

            try {
                msgChecksum = msg.Substring(ptr + 1, ptr + 3 - (ptr + 1));
            } catch (IndexOutOfRangeException) {
                return 2;
            }

            if (Convert.ToInt32(msgChecksum, 16) != checksum) return 1;
            return 0;
        }

        #endregion

        /// <summary>
        ///     Find the start of a NMEA sentence '!' or '$' character
        ///     Returns:
        ///     - index of the start character
        ///     The NMEA sentence may not always start at the beginning of the buffer,
        ///     use this routine to make sure the start of the sentence has been found.
        /// </summary>
        /// <returns>index of the start character</returns>
        /// <exception cref="StartNotFoundException"></exception>
        public int FindStart() {
            var i = 0;
            foreach (var x in _msg) {
                if (x == '!' || x == '$') return i;
                i++;
            }

            throw new StartNotFoundException("NMEA Start Not Found");
        }


        /// <summary>
        ///     Calculate the checksum of a NMEA 0183 sentence
        ///     This method will calculate the checksum of the string by skipping the
        ///     ! or $ character and stopping at the * character or end of string.
        ///     The checksum is stored in this.checksum
        /// </summary>
        /// <exception cref="StartNotFoundException"></exception>
        /// <exception cref="IllegalNmeaCharacterException"></exception>
        public void CalculateChecksum() {
            // Find start of sentence, after a '!' or '$'
            var ptr = FindStart() + 1;

            var msgArray = _msg.Substring(ptr).ToCharArray();

            _checksum = 0;
            foreach (var c in msgArray) {
                if (c == '!' || c == '$')
                    throw new IllegalNmeaCharacterException("Start Character Found before Checksum");

                if (c == '*') break;

                _checksum ^= c;
            }
        }


        /// <summary>
        ///     Check and return the checksum of a NMEA 0183 sentence
        ///     This method calls calculateChecksum() and updates this.checksum
        /// </summary>
        /// <returns>
        ///     - 0 if it does match
        ///     - 1 if it does not match
        ///     - 2 if there was an error
        /// </returns>
        public int CheckChecksum() {
            string msgChecksum;

            try {
                CalculateChecksum();
            } catch (StartNotFoundException) {
                return 2;
            } catch (IllegalNmeaCharacterException) {
                return 2;
            }

            var ptr = _msg.IndexOf('*');
            if (ptr < 0) return 2;

            try {
                msgChecksum = _msg.Substring(ptr + 1, ptr + 3 - (ptr + 1));
            } catch (IndexOutOfRangeException) {
                return 2;
            }

            if (Convert.ToInt32(msgChecksum, 16) != _checksum) return 1;
            return 0;
        }
    }
}