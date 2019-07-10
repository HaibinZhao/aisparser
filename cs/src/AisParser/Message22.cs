namespace AisParser {
    /// <summary>
    ///     AIS Parser SDK
    ///     AIS Message 22 Class
    ///     Copyright 2008 by Brian C. Lane
    ///     <bcl@ brianlane.com>
    ///         All Rights Reserved
    ///         @author Brian C. Lane
    /// </summary>
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
        public int Channel_a { get; private set; }

        /// <summary>
        ///     12 bits  : M.1084 Channel B Frequency
        /// </summary>
        public int Channel_b { get; private set; }

        /// <summary>
        ///     4 bits   : TX/RX Mode
        /// </summary>
        public int Txrx_mode { get; private set; }

        /// <summary>
        ///     1 bit    : Power Level
        /// </summary>
        public int Power { get; private set; }

        /// <summary>
        ///     : NE Corner Lat/Long in 1/1000 minutes
        /// </summary>
        public Position NE_pos { get; private set; }

        /// <summary>
        ///     30 bits  : Destination MMSI 1
        /// </summary>
        public long Addressed_1 { get; private set; }

        /// <summary>
        ///     : SW Corner Lat/Long in 1/1000 minutes
        /// </summary>
        public Position SW_pos { get; private set; }

        /// <summary>
        ///     30 bits  : Destination MMSI 2
        /// </summary>
        public long Addressed_2 { get; private set; }

        /// <summary>
        ///     1 bit    : Addressed flag
        /// </summary>
        public int Addressed { get; private set; }

        /// <summary>
        ///     1 bit    : Channel A Bandwidth
        /// </summary>
        public int Bw_a { get; private set; }

        /// <summary>
        ///     1 bit    : Channel B Bandwidth
        /// </summary>
        public int Bw_b { get; private set; }

        /// <summary>
        ///     3 bits   : Transitional Zone size
        /// </summary>
        public int Tz_size { get; private set; }

        /// <summary>
        ///     23 bits  : Spare
        /// </summary>
        public long Spare2 { get; private set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="msgid"></param>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse(Sixbit six_state) {
            if (six_state.BitLength() != 168) throw new AisMessageException("Message 22 wrong length");

            base.Parse(six_state);

            Spare1 = (int) six_state.Get(1);
            Channel_a = (int) six_state.Get(12);
            Channel_b = (int) six_state.Get(12);
            Txrx_mode = (int) six_state.Get(4);
            Power = (int) six_state.Get(1);

            var NE_longitude = six_state.Get(18);
            var NE_latitude = six_state.Get(17);

            var SW_longitude = six_state.Get(18);
            var SW_latitude = six_state.Get(17);

            Addressed = (int) six_state.Get(1);
            Bw_a = (int) six_state.Get(1);
            Bw_b = (int) six_state.Get(1);
            Tz_size = (int) six_state.Get(3);

            /* Is the position actually an address? */
            if (Addressed == 1) {
                /* Convert the positions to addresses */
                Addressed_1 = (NE_longitude << 12) + (NE_latitude >> 5);
                Addressed_2 = (SW_longitude << 12) + (SW_latitude >> 5);
            } else {
                NE_pos = new Position();
                NE_pos.Longitude = NE_longitude * 10;
                NE_pos.Latitude = NE_latitude * 10;

                SW_pos = new Position();
                SW_pos.Longitude = SW_longitude * 10;
                SW_pos.Latitude = SW_latitude * 10;
            }
        }
    }
}