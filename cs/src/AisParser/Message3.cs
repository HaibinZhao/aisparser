namespace AisParser {
    /// <summary>
    ///     AIS Message 3 class
    ///     Position Report
    /// </summary>
    public sealed class Message3 : Messages {
        public Message3():base(3) {
		}

		public Message3(Sixbit sixbit):this(){
			this.Parse(sixbit);
        }

        /// <summary>
        ///     4 bits  : Navigational Status
        /// </summary>
        public int NavStatus { get; private set; }

        /// <summary>
        ///     8 bits  : Rate of Turn
        /// </summary>
        public int Rot { get; private set; }

        /// <summary>
        ///     10 bits : Speed Over Ground
        /// </summary>
        public int Sog { get; private set; }

        /// <summary>
        ///     1 bit   : Position Accuracy
        /// </summary>
        public int PosAcc { get; private set; }

        /// <summary>
        ///     : Lat/Long 1/10000 minute
        /// </summary>
        public Position Pos { get; private set; }

        /// <summary>
        ///     12 bits : Course over Ground
        /// </summary>
        public int Cog { get; private set; }

        /// <summary>
        ///     9 bits  : True heading
        /// </summary>
        public int TrueHeading { get; private set; }

        /// <summary>
        ///     6 bits  : UTC Seconds
        /// </summary>
        public int UtcSec { get; private set; }

        /// <summary>
        ///     4 bits  : Regional bits
        /// </summary>
        public int Regional { get; private set; }

        /// <summary>
        ///     1 bit   : Spare
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
        ///     13 bits : ITDMA Slot Increment
        /// </summary>
        public int SlotIncrement { get; private set; }

        /// <summary>
        ///     3 bits  : ITDMA Number of Slots
        /// </summary>
        public int NumSlots { get; private set; }

        /// <summary>
        ///     1 bit   : ITDMA Keep Flag
        /// </summary>
        public int Keep { get; private set; }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public void parse(Sixbit six_state) throws SixbitsExhaustedException, AISMessageException
        public override void Parse(Sixbit sixState) {
            if (sixState.BitLength!= 168) throw new AisMessageException("Message 3 wrong length");

            base.Parse(sixState);

            // Parse the Message 3 
            NavStatus = (int) sixState.Get(4);
            Rot = (int) sixState.Get(8);
            Sog = (int) sixState.Get(10);
            PosAcc = (int) sixState.Get(1);

            Pos = new Position {
                Longitude = sixState.Get(28),
                Latitude = sixState.Get(27)
            };

            Cog = (int) sixState.Get(12);
            TrueHeading = (int) sixState.Get(9);
            UtcSec = (int) sixState.Get(6);
            Regional = (int) sixState.Get(4);
            Spare = (int) sixState.Get(1);
            Raim = (int) sixState.Get(1);
            SyncState = (int) sixState.Get(2);
            SlotIncrement = (int) sixState.Get(13);
            NumSlots = (int) sixState.Get(3);
            Keep = (int) sixState.Get(1);
        }
    }
}