namespace AisParser {
    /// <summary>
    ///     AIS Message 21 class
    ///     Aids-to-Navigation Report
    /// </summary>
    public sealed class Message21 : Messages {
        public Message21():base(21) {
		}

		public Message21(Sixbit sixbit):this(){
			this.Parse(sixbit);
        }

        /// <summary>
        ///     5 bits    : Type of AtoN
        /// </summary>
        public int AtonType { get; private set; }

        /// <summary>
        ///     120 bits  : Name of AtoN in ASCII
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     1 bit     : Position Accuracy
        /// </summary>
        public int PosAcc { get; private set; }

        /// <summary>
        ///     : Lat/Long 1/100000 minute
        /// </summary>
        public Position Pos { get; private set; }

        /// <summary>
        ///     9 bits    : GPS Ant. Distance from Bow
        /// </summary>
        public int DimBow { get; private set; }

        /// <summary>
        ///     9 bits    : GPS Ant. Distance from Stern
        /// </summary>
        public int DimStern { get; private set; }

        /// <summary>
        ///     6 bits    : GPS Ant. Distance from Port
        /// </summary>
        public int DimPort { get; private set; }

        /// <summary>
        ///     6 bits    : GPS Ant. Distance from Starboard
        /// </summary>
        public int DimStarboard { get; private set; }

        /// <summary>
        ///     4 bits    : Type of Position Fixing Device
        /// </summary>
        public int PosType { get; private set; }

        /// <summary>
        ///     6 bits    : UTC Seconds
        /// </summary>
        public int UtcSec { get; private set; }

        /// <summary>
        ///     1 bit     : Off Position Flag
        /// </summary>
        public int OffPosition { get; private set; }

        /// <summary>
        ///     8 bits    : Regional Bits
        /// </summary>
        public int Regional { get; private set; }

        /// <summary>
        ///     1 bit     : RAIM Flag
        /// </summary>
        public int Raim { get; private set; }

        /// <summary>
        ///     1 bit     : Virtual/Pseudo AtoN Flag
        /// </summary>
        public int VirtualRenamed { get; private set; }

        /// <summary>
        ///     1 bit     : Assigned Mode Flag
        /// </summary>
        public int Assigned { get; private set; }

        /// <summary>
        ///     1 bit     : Spare
        /// </summary>
        public int Spare1 { get; private set; }

        /// <summary>
        ///     0-84 bits : Extended name in ASCII
        /// </summary>
        public string NameExt { get; private set; }

        /// <summary>
        ///     0-6 bits  : Spare
        /// </summary>
        public int Spare2 { get; private set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse(Sixbit sixState) {
            var length = sixState.BitLength();
            if (length < 272 || length > 360) throw new AisMessageException("Message 21 wrong length");

            base.Parse(sixState);

            AtonType = (int) sixState.Get(5);
            Name = sixState.GetString(20);
            PosAcc = (int) sixState.Get(1);

            Pos = new Position {
                Longitude = sixState.Get(28),
                Latitude = sixState.Get(27)
            };

            DimBow = (int) sixState.Get(9);
            DimStern = (int) sixState.Get(9);
            DimPort = (int) sixState.Get(6);
            DimStarboard = (int) sixState.Get(6);
            PosType = (int) sixState.Get(4);
            UtcSec = (int) sixState.Get(6);
            OffPosition = (int) sixState.Get(1);
            Regional = (int) sixState.Get(8);
            Raim = (int) sixState.Get(1);
            VirtualRenamed = (int) sixState.Get(1);
            Assigned = (int) sixState.Get(1);
            Spare1 = (int) sixState.Get(1);

            if (length > 272) NameExt = sixState.GetString((length - 272) / 6);
        }
    }
}