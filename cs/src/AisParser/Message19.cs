namespace AisParser {
    /// <summary>
    ///     AIS Message 19 class
    ///     Extended Class B Equipment Position Report
    /// </summary>
    public sealed class Message19 : Messages {
        public Message19():base(19) {
		}

		public Message19(Sixbit sixbit):this(){
			this.Parse(sixbit);
        }

        /// <summary>
        ///     8 bits   : Regional Bits
        /// </summary>
        public int Regional1 { get; private set; }

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
        ///     9 bits   : True Heading
        /// </summary>
        public int TrueHeading { get; private set; }

        /// <summary>
        ///     6 bits   : UTC Seconds
        /// </summary>
        public int UtcSec { get; private set; }

        /// <summary>
        ///     4 bits   : Regional Bits
        /// </summary>
        public int Regional2 { get; private set; }

        /// <summary>
        ///     120 bits : Ship Name in ASCII
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     8 bits   : Type of Ship and Cargo
        /// </summary>
        public int ShipType { get; private set; }

        /// <summary>
        ///     9 bits   : GPS Ant. Distance from Bow
        /// </summary>
        public int DimBow { get; private set; }

        /// <summary>
        ///     9 bits   : GPS Ant. Distance from Stern
        /// </summary>
        public int DimStern { get; private set; }

        /// <summary>
        ///     6 bits   : GPS Ant. Distance from Port
        /// </summary>
        public int DimPort { get; private set; }

        /// <summary>
        ///     6 bits   : GPS Ant. Distance from Starboard
        /// </summary>
        public int DimStarboard { get; private set; }

        /// <summary>
        ///     4 bits   : Type of Position Fixing Device
        /// </summary>
        public int PosType { get; private set; }

        /// <summary>
        ///     1 bit    : RAIM Flag
        /// </summary>
        public int Raim { get; private set; }

        /// <summary>
        ///     1 bit    : DTE Flag
        /// </summary>
        public int Dte { get; private set; }

        /// <summary>
        ///     5 bits   : Spare
        /// </summary>
        public int Spare { get; private set; }


        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse(Sixbit sixState) {
            if (sixState.BitLength!= 312) throw new AisMessageException("Message 19 wrong length");

            base.Parse(sixState);

            Regional1 = (int) sixState.Get(8);
            Sog = (int) sixState.Get(10);
            PosAcc = (int) sixState.Get(1);

            Pos = new Position {
                Longitude = sixState.Get(28),
                Latitude = sixState.Get(27)
            };

            Cog = (int) sixState.Get(12);
            TrueHeading = (int) sixState.Get(9);
            UtcSec = (int) sixState.Get(6);
            Regional2 = (int) sixState.Get(4);
            Name = sixState.GetString(20);
            ShipType = (int) sixState.Get(8);
            DimBow = (int) sixState.Get(9);
            DimStern = (int) sixState.Get(9);
            DimPort = (int) sixState.Get(6);
            DimStarboard = (int) sixState.Get(6);
            PosType = (int) sixState.Get(4);
            Raim = (int) sixState.Get(1);
            Dte = (int) sixState.Get(1);
            Spare = (int) sixState.Get(5);
        }
    }
}