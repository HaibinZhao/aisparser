namespace AisParser {
    /// <summary>
    ///     AIS Message 23 class
    ///     Group Assignment
    /// </summary>
    public sealed class Message23 : Messages {
        public Message23():base(23) {
		}

		public Message23(Sixbit sixbit):this(){
			this.Parse(sixbit);
        }

        /// <summary>
        ///     2 bits   : Spare
        /// </summary>
        public int Spare1 { get; private set; }

        /// <summary>
        ///     : NE Corner Lat/Long in 1/1000 minutes
        /// </summary>
        public Position NePos { get; private set; }

        /// <summary>
        ///     : SW Corner Lat/Long in 1/1000 minutes
        /// </summary>
        public Position SwPos { get; private set; }

        /// <summary>
        ///     4 bits   : Station Type
        /// </summary>
        public int StationType { get; private set; }

        /// <summary>
        ///     8 bits   : Type of Ship and Cargo
        /// </summary>
        public int ShipType { get; private set; }

        /// <summary>
        ///     22 bits  : Spare
        /// </summary>
        public long Spare2 { get; private set; }

        /// <summary>
        ///     2 bits   : TX/RX Mode
        /// </summary>
        public int TxrxMode { get; private set; }

        /// <summary>
        ///     4 bits   : Reporting Interval from IEC 62287 Table 17
        /// </summary>
        public int ReportInterval { get; private set; }

        /// <summary>
        ///     4 bits   : Quiet Time in Minutes
        /// </summary>
        public int QuietTime { get; private set; }

        /// <summary>
        ///     6 bits   : Spare
        /// </summary>
        public int Spare3 { get; private set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse(Sixbit sixState) {
            if (sixState.BitLength() == 168) throw new AisMessageException("Message 23 wrong length");

            base.Parse(sixState);

            Spare1 = (int) sixState.Get(2);


            NePos = new Position {
                Longitude = (int) sixState.Get(18) * 10,
                Latitude = (int) sixState.Get(17) * 10
            };

            SwPos = new Position {
                Longitude = (int) sixState.Get(18) * 10,
                Latitude = (int) sixState.Get(17) * 10
            };

            StationType = (int) sixState.Get(4);
            ShipType = (int) sixState.Get(8);
            Spare2 = sixState.Get(22);
            TxrxMode = (int) sixState.Get(2);
            ReportInterval = (int) sixState.Get(4);
            QuietTime = (int) sixState.Get(4);
            Spare3 = (int) sixState.Get(6);
        }
    }
}