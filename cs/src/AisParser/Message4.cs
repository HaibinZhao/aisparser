namespace AisParser {
    /// <summary>
    ///     AIS Message 4 class
    ///     Base Station Report
    /// </summary>
    public sealed class Message4 : Messages {
        public Message4():base(4) {
		}

		public Message4(Sixbit sixbit):this(){
			this.Parse(sixbit);
        }

        /// <summary>
        ///     14 bits : UTC Year
        /// </summary>
        public int UtcYear { get; private set; }

        /// <summary>
        ///     4 bits  : UTC Month
        /// </summary>
        public int UtcMonth { get; private set; }

        /// <summary>
        ///     5 bits  : UTC Day
        /// </summary>
        public int UtcDay { get; private set; }

        /// <summary>
        ///     5 bits  : UTC Hour
        /// </summary>
        public int UtcHour { get; private set; }

        /// <summary>
        ///     6 bits  : UTC Minute
        /// </summary>
        public int UtcMinute { get; private set; }

        /// <summary>
        ///     6 bits  : UTC Second
        /// </summary>
        public int UtcSecond { get; private set; }

        /// <summary>
        ///     1 bit   : Position Accuracy
        /// </summary>
        public int PosAcc { get; private set; }

        /// <summary>
        ///     : Lat/Long 1/100000 minute
        /// </summary>
        public Position Pos { get; private set; }

        /// <summary>
        ///     4 bits  : Type of position fixing device
        /// </summary>
        public int PosType { get; private set; }

        /// <summary>
        ///     10 bits : Spare
        /// </summary>
        public int Spare { get; private set; }

        /// <summary>
        ///     1 bit   : RAIM flag
        /// </summary>
        public int Raim { get; private set; }

        /// <summary>
        ///     2 bits  : SOTDMA sync state
        /// </summary>
        public int SyncState { get; private set; }

        /// <summary>
        ///     3 bits  : SOTDMA Slot Timeout
        /// </summary>
        public int SlotTimeout { get; private set; }

        /// <summary>
        ///     14 bits : SOTDMA sub-message
        /// </summary>
        public int SubMessage { get; private set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse(Sixbit sixState) {
            if (sixState.BitLength() != 168) throw new AisMessageException("Message 5 wrong length");

            base.Parse(sixState);

            UtcYear = (int) sixState.Get(14);
            UtcMonth = (int) sixState.Get(4);
            UtcDay = (int) sixState.Get(5);
            UtcHour = (int) sixState.Get(5);
            UtcMinute = (int) sixState.Get(6);
            UtcSecond = (int) sixState.Get(6);
            PosAcc = (int) sixState.Get(1);

            Pos = new Position();
            Pos.Longitude = sixState.Get(28);
            Pos.Latitude = sixState.Get(27);

            PosType = (int) sixState.Get(4);
            Spare = (int) sixState.Get(10);
            Raim = (int) sixState.Get(1);
            SyncState = (int) sixState.Get(2);
            SlotTimeout = (int) sixState.Get(3);
            SubMessage = (int) sixState.Get(14);
        }
    }
}