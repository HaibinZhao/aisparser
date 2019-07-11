namespace AisParser {
    /// <summary>
    ///     AIS Message 9 class
    ///     Standard SAR Aircraft Position Report
    /// </summary>
    public sealed class Message9 : Messages {
        public Message9():base(9) {
		}

		public Message9(Sixbit sixbit):this(){
			this.Parse(sixbit);
        }

        /// <summary>
        ///     12 bits  : Altitude
        /// </summary>
        public int Altitude { get; private set; }

        /// <summary>
        ///     10 bits  : Speed Over Ground
        /// </summary>
        public int Sog { get; private set; }

        /// <summary>
        ///     1 bit    : Position Accuracy
        /// </summary>
        public int PosAcc { get; private set; }

        /// <summary>
        ///     : Lat/Long 1/100000 minute
        /// </summary>
        public Position Pos { get; private set; }

        /// <summary>
        ///     12 bits  : Course Over Ground
        /// </summary>
        public int Cog { get; private set; }

        /// <summary>
        ///     6 bits   : UTC Seconds
        /// </summary>
        public int UtcSec { get; private set; }

        /// <summary>
        ///     8 bits   : Regional bits
        /// </summary>
        public int Regional { get; private set; }

        /// <summary>
        ///     1 bit    : DTE flag
        /// </summary>
        public int Dte { get; private set; }

        /// <summary>
        ///     3 bits   : Spare
        /// </summary>
        public int Spare { get; private set; }

        /// <summary>
        ///     1 bit    : Assigned mode flag
        /// </summary>
        public int Assigned { get; private set; }

        /// <summary>
        ///     1 bit    : RAIM flag
        /// </summary>
        public int Raim { get; private set; }

        /// <summary>
        ///     1 bit    : Comm State Flag
        /// </summary>
        public int CommState { get; private set; }

        public Sotdma SotdmaState { get; private set; }
        public Itdma ItdmaState { get; private set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse(Sixbit sixState) {
            if (sixState.BitLength() != 168) throw new AisMessageException("Message 9 wrong length");

            base.Parse(sixState);

            Altitude = (int) sixState.Get(12);
            Sog = (int) sixState.Get(10);
            PosAcc = (int) sixState.Get(1);

            Pos = new Position {
                Longitude = sixState.Get(28),
                Latitude = sixState.Get(27)
            };

            Cog = (int) sixState.Get(12);
            UtcSec = (int) sixState.Get(6);
            Regional = (char) sixState.Get(8);
            Dte = (char) sixState.Get(1);
            Spare = (char) sixState.Get(3);
            Assigned = (char) sixState.Get(1);
            Raim = (char) sixState.Get(1);
            CommState = (char) sixState.Get(1);

            if (CommState == 0)
                SotdmaState = new Sotdma(sixState);
            else
                ItdmaState = new Itdma(sixState);
        }
    }
}