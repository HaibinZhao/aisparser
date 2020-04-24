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
            this.Longitude = longitude;
            this.Latitude = latitude;
        }

        /// <summary>
        /// 1/10000 degree Longitude
        /// </summary>
        /// <value></value>
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

        /// <summary>
        /// 1/10000 degree Latitude
        /// </summary>
        /// <value></value>
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
            return $"{{lng:{Longitude/ 600000d},lat:{Latitude/ 600000d}}}";
        }

        /// <summary>
        /// Get Coordinates [longitude,latitude]
        /// </summary>
        /// <returns></returns>
        public double[] ToCoordinates(){
            return new double[]{Longitude/ 600000d,Latitude/ 600000d};
        }

        #endregion
    }
}