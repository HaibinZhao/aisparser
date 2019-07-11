namespace AisParser {
    /// <summary>
    ///     AIS Message 11 class
    ///     UTC/Date response
    /// </summary>
    public sealed class Message11 : Messages {
        public Message11():base(11) {
		}

		public Message11(Sixbit sixbit):this(){
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
        ///     : Lat/Long 1/10000 minute
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

        public Sotdma SotdmaState { get; private set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse(Sixbit sixState) {
            if (sixState.BitLength!= 168) throw new AisMessageException("Message 11 wrong length");

            base.Parse(sixState);

            /* Parse the Message 11 */
            UtcYear = (int) sixState.Get(14);
            UtcMonth = (int) sixState.Get(4);
            UtcDay = (int) sixState.Get(5);
            UtcHour = (int) sixState.Get(5);
            UtcMinute = (int) sixState.Get(6);
            UtcSecond = (int) sixState.Get(6);
            PosAcc = (int) sixState.Get(1);

            Pos = new Position {
                Longitude = sixState.Get(28),
                Latitude = sixState.Get(27)
            };

            PosType = (int) sixState.Get(4);
            Spare = (int) sixState.Get(10);
            Raim = (int) sixState.Get(1);
            SotdmaState = new Sotdma();
            SotdmaState.Parse(sixState);
        }
    }
}