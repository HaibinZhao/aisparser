namespace AisParser {
    /// <summary>
    ///     AIS Message 24 class
    ///     Class B"CS" Static Data Report
    /// </summary>
    public class Message24 : Messages {
        public Message24() : base(24) {
        }

        /// <summary>
        ///     2 bits   : Part Number
        /// </summary>
        public int PartNumber { get; private set; }

        //!< Message 24A 

        /// <summary>
        ///     120 bits : Ship Name in ASCII
        /// </summary>
        public string Name { get; private set; }

        //!< Message 24B 

        /// <summary>
        ///     8 bits   : Type of Ship and Cargo
        /// </summary>
        public int ShipType { get; private set; }

        /// <summary>
        ///     42 bits  : Vendor ID in ASCII
        /// </summary>
        public string VendorId { get; private set; }

        /// <summary>
        ///     42 bits  : Callsign in ASCII
        /// </summary>
        public string Callsign { get; private set; }

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
        ///     6 bits   : Spare
        /// </summary>
        public int Spare { get; private set; }


        /// <summary>
        ///     A/B flags - A = 1  B = 2  Both = 3
        /// </summary>
        public int Flags { get; private set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        protected override void Parse(Sixbit sixState) {
            var length = sixState.BitLength();
            if (length != 160 && length != 168) throw new AisMessageException("Message 24 wrong length");

            base.Parse(sixState);

            PartNumber = (int) sixState.Get(2);

            if (PartNumber == 0) {
                /* Parse 24A */
                /* Get the Ship Name, convert to ASCII */
                Name = sixState.get_string(20);

                /* Indicate reception of part A */
                Flags |= 0x01;
            } else if (PartNumber == 1) {
                /* Parse 24B */
                ShipType = (int) sixState.Get(8);
                VendorId = sixState.get_string(7);
                Callsign = sixState.get_string(7);

                DimBow = (int) sixState.Get(9);
                DimStern = (int) sixState.Get(9);
                DimPort = (int) sixState.Get(6);
                DimStarboard = (int) sixState.Get(6);
                Spare = (int) sixState.Get(6);

                /* Indicate reception of part A */
                Flags |= 0x02;
            } else {
                throw new AisMessageException("Unknown Message 24 Part #");
            }
        }
    }
}