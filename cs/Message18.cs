namespace AisParser {
    /// <summary>
    ///     AIS Parser SDK
    ///     AIS Message 18 Class
    ///     Copyright 2008 by Brian C. Lane
    ///     <bcl@ brianlane.com>
    ///         All Rights Reserved
    ///         @author Brian C. Lane
    /// </summary>
    /// <summary>
    ///     AIS Message 18 class
    ///     Standard Class B equipment position report
    /// </summary>
    public class Message18 : Messages {
        public Message18() : base(18) {
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
        public int Pos_acc { get; private set; }

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
        public int True_heading { get; private set; }

        /// <summary>
        ///     6 bits   : UTC Seconds
        /// </summary>
        public int Utc_sec { get; private set; }

        /// <summary>
        ///     2 bits   : Regional Bits
        /// </summary>
        public int Regional2 { get; private set; }

        /// <summary>
        ///     1 bit    : Class B CS Flag
        /// </summary>
        public int Unit_flag { get; private set; }

        /// <summary>
        ///     1 bit    : Integrated msg14 Display Flag
        /// </summary>
        public int Display_flag { get; private set; }

        /// <summary>
        ///     1 bit    : DSC Capability flag
        /// </summary>
        public int Dsc_flag { get; private set; }

        /// <summary>
        ///     1 bit    : Marine Band Operation Flag
        /// </summary>
        public int Band_flag { get; private set; }

        /// <summary>
        ///     1 bit    : Msg22 Frequency Management Flag
        /// </summary>
        public int Msg22_flag { get; private set; }

        /// <summary>
        ///     1 bit    : Autonomous Mode Flag
        /// </summary>
        public int Mode_flag { get; private set; }

        /// <summary>
        ///     1 bit    : RAIM Flag
        /// </summary>
        public int Raim { get; private set; }

        /// <summary>
        ///     1 bit    : Comm State Flag
        /// </summary>
        public int Comm_state { get; private set; }

        public Sotdma Sotdma_state { get; private set; }
        public Itdma Itdma_state { get; private set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="msgid"></param>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        protected override void Parse(Sixbit six_state) {
            if (six_state.BitLength() != 168) throw new AisMessageException("Message 18 wrong length");

            base.Parse(six_state);

            Regional1 = (int) six_state.Get(8);
            Sog = (int) six_state.Get(10);
            Pos_acc = (int) six_state.Get(1);

            Pos = new Position();
            Pos.Longitude = six_state.Get(28);
            Pos.Latitude = six_state.Get(27);

            Cog = (int) six_state.Get(12);
            True_heading = (int) six_state.Get(9);
            Utc_sec = (int) six_state.Get(6);
            Regional2 = (int) six_state.Get(2);
            Unit_flag = (int) six_state.Get(1);
            Display_flag = (int) six_state.Get(1);
            Dsc_flag = (int) six_state.Get(1);
            Band_flag = (int) six_state.Get(1);
            Msg22_flag = (int) six_state.Get(1);
            Mode_flag = (int) six_state.Get(1);
            Raim = (int) six_state.Get(1);
            Comm_state = (int) six_state.Get(1);

            if (Comm_state == 0)
                Sotdma_state = new Sotdma(six_state);
            else
                Itdma_state = new Itdma(six_state);
        }
    }
}