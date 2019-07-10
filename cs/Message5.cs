namespace AisParser {
    /// <summary>
    ///     AIS Message 5 class
    ///     Static and Voyage Related Data
    /// </summary>
    public class Message5 : Messages {
        public Message5() : base(5) {
        }

        /// <summary>
        ///     2 bits          : AIS Version
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        ///     30 bits         : IMO Number
        /// </summary>
        public long Imo { get; private set; }

        /// <summary>
        ///     7x6 (42) bits   : Callsign
        /// </summary>
        public string Callsign { get; private set; }

        /// <summary>
        ///     20x6 (120) bits : Ship Name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     8 bits          : Type of Ship and Cargo
        /// </summary>
        public int ShipType { get; private set; }

        /// <summary>
        ///     9 bits          : GPS Ant. Distance from Bow
        /// </summary>
        public int DimBow { get; private set; }

        /// <summary>
        ///     9 bits          : GPS Ant. Distance from stern
        /// </summary>
        public int DimStern { get; private set; }

        /// <summary>
        ///     6 bits          : GPS Ant. Distance from port
        /// </summary>
        public int DimPort { get; private set; }

        /// <summary>
        ///     6 bits          : GPS Ant. Distance from starboard
        /// </summary>
        public int DimStarboard { get; private set; }

        /// <summary>
        ///     4 bits          : Type of position fixing device
        /// </summary>
        public int PosType { get; private set; }

        /// <summary>
        ///     20 bits         : Estimated Time of Arrival MMDDHHMM
        /// </summary>
        public long Eta { get; private set; }

        /// <summary>
        ///     8 bits          : Maximum present static draught
        /// </summary>
        public int Draught { get; private set; }

        /// <summary>
        ///     6x20 (120) bits : Ship Destination
        /// </summary>
        public string Dest { get; private set; }

        /// <summary>
        ///     1 bit           : DTE flag
        /// </summary>
        public int Dte { get; private set; }

        /// <summary>
        ///     1 bit           : spare
        /// </summary>
        public int Spare { get; private set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="msgid"></param>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        protected override void Parse(Sixbit sixState) {
            if (sixState.BitLength() != 424) throw new AisMessageException("Message 5 wrong length");

            base.Parse(sixState);

            Version = (int) sixState.Get(2);
            Imo = sixState.Get(30);
            Callsign = sixState.get_string(7);
            Name = sixState.get_string(20);
            ShipType = (int) sixState.Get(8);
            DimBow = (int) sixState.Get(9);
            DimStern = (int) sixState.Get(9);
            DimPort = (int) sixState.Get(6);
            DimStarboard = (int) sixState.Get(6);
            PosType = (int) sixState.Get(4);
            Eta = sixState.Get(20);
            Draught = (int) sixState.Get(8);
            Dest = sixState.get_string(20);
        }
    }
}