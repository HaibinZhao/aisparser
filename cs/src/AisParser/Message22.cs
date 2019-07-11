namespace AisParser {
    /// <summary>
    ///     AIS Message 22 class
    ///     Channel Management
    /// </summary>
    public sealed class Message22 : Messages {
        public Message22():base(22) {
		}

		public Message22(Sixbit sixbit):this(){
			this.Parse(sixbit);
        }

        /// <summary>
        ///     2 bits   : Spare
        /// </summary>
        public int Spare1 { get; private set; }

        /// <summary>
        ///     12 bits  : M.1084 Channel A Frequency
        /// </summary>
        public int ChannelA { get; private set; }

        /// <summary>
        ///     12 bits  : M.1084 Channel B Frequency
        /// </summary>
        public int ChannelB { get; private set; }

        /// <summary>
        ///     4 bits   : TX/RX Mode
        /// </summary>
        public int TxrxMode { get; private set; }

        /// <summary>
        ///     1 bit    : Power Level
        /// </summary>
        public int Power { get; private set; }

        /// <summary>
        ///     : NE Corner Lat/Long in 1/1000 minutes
        /// </summary>
        public Position NePos { get; private set; }

        /// <summary>
        ///     30 bits  : Destination MMSI 1
        /// </summary>
        public long Addressed1 { get; private set; }

        /// <summary>
        ///     : SW Corner Lat/Long in 1/1000 minutes
        /// </summary>
        public Position SwPos { get; private set; }

        /// <summary>
        ///     30 bits  : Destination MMSI 2
        /// </summary>
        public long Addressed2 { get; private set; }

        /// <summary>
        ///     1 bit    : Addressed flag
        /// </summary>
        public int Addressed { get; private set; }

        /// <summary>
        ///     1 bit    : Channel A Bandwidth
        /// </summary>
        public int BwA { get; private set; }

        /// <summary>
        ///     1 bit    : Channel B Bandwidth
        /// </summary>
        public int BwB { get; private set; }

        /// <summary>
        ///     3 bits   : Transitional Zone size
        /// </summary>
        public int TzSize { get; private set; }

        /// <summary>
        ///     23 bits  : Spare
        /// </summary>
        public long Spare2 { get; private set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse(Sixbit sixState) {
            if (sixState.BitLength!= 168) throw new AisMessageException("Message 22 wrong length");

            base.Parse(sixState);

            Spare1 = (int) sixState.Get(1);
            ChannelA = (int) sixState.Get(12);
            ChannelB = (int) sixState.Get(12);
            TxrxMode = (int) sixState.Get(4);
            Power = (int) sixState.Get(1);

            var neLongitude = sixState.Get(18);
            var neLatitude = sixState.Get(17);

            var swLongitude = sixState.Get(18);
            var swLatitude = sixState.Get(17);

            Addressed = (int) sixState.Get(1);
            BwA = (int) sixState.Get(1);
            BwB = (int) sixState.Get(1);
            TzSize = (int) sixState.Get(3);

            // Is the position actually an address?
            if (Addressed == 1) {
                // Convert the positions to addresses 
                Addressed1 = (neLongitude << 12) + (neLatitude >> 5);
                Addressed2 = (swLongitude << 12) + (swLatitude >> 5);
            } else {
                NePos = new Position {
                    Longitude = neLongitude * 10,
                    Latitude = neLatitude * 10
                };

                SwPos = new Position {
                    Longitude = swLongitude * 10,
                    Latitude = swLatitude * 10
                };
            }
        }
    }
}