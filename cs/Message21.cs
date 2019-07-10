namespace AisParser {
    /// <summary>
    ///     AIS Parser SDK
    ///     AIS Message 21 Class
    ///     Copyright 2008 by Brian C. Lane
    ///     <bcl@ brianlane.com>
    ///         All Rights Reserved
    ///         @author Brian C. Lane
    /// </summary>
    /// <summary>
    ///     AIS Message 21 class
    ///     Aids-to-Navigation Report
    /// </summary>
    public class Message21 : Messages {
        public Message21() : base(21) {
        }

        /// <summary>
        ///     5 bits    : Type of AtoN
        /// </summary>
        public int Aton_type { get; private set; }

        /// <summary>
        ///     120 bits  : Name of AtoN in ASCII
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     1 bit     : Position Accuracy
        /// </summary>
        public int Pos_acc { get; private set; }

        /// <summary>
        ///     : Lat/Long 1/100000 minute
        /// </summary>
        public Position Pos { get; private set; }

        /// <summary>
        ///     9 bits    : GPS Ant. Distance from Bow
        /// </summary>
        public int Dim_bow { get; private set; }

        /// <summary>
        ///     9 bits    : GPS Ant. Distance from Stern
        /// </summary>
        public int Dim_stern { get; private set; }

        /// <summary>
        ///     6 bits    : GPS Ant. Distance from Port
        /// </summary>
        public int Dim_port { get; private set; }

        /// <summary>
        ///     6 bits    : GPS Ant. Distance from Starboard
        /// </summary>
        public int Dim_starboard { get; private set; }

        /// <summary>
        ///     4 bits    : Type of Position Fixing Device
        /// </summary>
        public int Pos_type { get; private set; }

        /// <summary>
        ///     6 bits    : UTC Seconds
        /// </summary>
        public int Utc_sec { get; private set; }

        /// <summary>
        ///     1 bit     : Off Position Flag
        /// </summary>
        public int Off_position { get; private set; }

        /// <summary>
        ///     8 bits    : Regional Bits
        /// </summary>
        public int Regional { get; private set; }

        /// <summary>
        ///     1 bit     : RAIM Flag
        /// </summary>
        public int Raim { get; private set; }
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:

        /// <summary>
        ///     1 bit     : Virtual/Pseudo AtoN Flag
        /// </summary>
        public int virtual_Renamed { get; private set; }

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
        public string Name_ext { get; private set; }

        /// <summary>
        ///     0-6 bits  : Spare
        /// </summary>
        public int Spare2 { get; private set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="msgid"></param>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        protected override void Parse(Sixbit six_state) {
            var length = six_state.BitLength();
            if (length < 272 || length > 360) throw new AisMessageException("Message 21 wrong length");

            base.Parse(six_state);

            Aton_type = (int) six_state.Get(5);
            Name = six_state.get_string(20);
            Pos_acc = (int) six_state.Get(1);

            Pos = new Position {
                Longitude = six_state.Get(28),
                Latitude = six_state.Get(27)
            };

            Dim_bow = (int) six_state.Get(9);
            Dim_stern = (int) six_state.Get(9);
            Dim_port = (int) six_state.Get(6);
            Dim_starboard = (int) six_state.Get(6);
            Pos_type = (int) six_state.Get(4);
            Utc_sec = (int) six_state.Get(6);
            Off_position = (int) six_state.Get(1);
            Regional = (int) six_state.Get(8);
            Raim = (int) six_state.Get(1);
            virtual_Renamed = (int) six_state.Get(1);
            Assigned = (int) six_state.Get(1);
            Spare1 = (int) six_state.Get(1);

            if (length > 272) Name_ext = six_state.get_string((length - 272) / 6);
        }
    }
}