namespace AisParser {
    /// <summary>
    ///     AIS Position class
    ///     Convert raw unsigned AIS position to signed 1/10000 degree position
    ///     and provide helper methods for other formats
    /// </summary>
    public class Position {
        private long _latitude;
        private long _longitude;

        public Position() { }

        public Position(long longitude, long latitude) {
            this._longitude = longitude;
            this._latitude = latitude;
        }

        public long Longitude {
            get => _longitude;
            set {
                // Convert longitude to signed number 
                if (value >= 0x8000000) {
                    _longitude = 0x10000000 - value;
                    _longitude *= -1;
                } else {
                    _longitude = value;
                }
            }
        }

        public long Latitude {
            get => _latitude;
            set {
                // Convert latitude to signed number 
                if (value >= 0x4000000) {
                    _latitude = 0x8000000 - value;
                    _latitude *= -1;
                } else {
                    _latitude = value;
                }
            }
        }

        #region Overrides of Object

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return $"{{lat:{Latitude/ 1e6},lng:{Longitude/ 1e6}}}";
        }

        #endregion
    }
}