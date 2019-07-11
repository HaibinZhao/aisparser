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
        public int Utc_year { get; private set; }

        /// <summary>
        ///     4 bits  : UTC Month
        /// </summary>
        public int Utc_month { get; private set; }

        /// <summary>
        ///     5 bits  : UTC Day
        /// </summary>
        public int Utc_day { get; private set; }

        /// <summary>
        ///     5 bits  : UTC Hour
        /// </summary>
        public int Utc_hour { get; private set; }

        /// <summary>
        ///     6 bits  : UTC Minute
        /// </summary>
        public int Utc_minute { get; private set; }

        /// <summary>
        ///     6 bits  : UTC Second
        /// </summary>
        public int Utc_second { get; private set; }

        /// <summary>
        ///     1 bit   : Position Accuracy
        /// </summary>
        public int Pos_acc { get; private set; }

        /// <summary>
        ///     : Lat/Long 1/10000 minute
        /// </summary>
        public Position Pos { get; private set; }

        /// <summary>
        ///     4 bits  : Type of position fixing device
        /// </summary>
        public int Pos_type { get; private set; }

        /// <summary>
        ///     10 bits : Spare
        /// </summary>
        public int Spare { get; private set; }

        /// <summary>
        ///     1 bit   : RAIM flag
        /// </summary>
        public int Raim { get; private set; }

        public Sotdma Sotdma_state { get; private set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse(Sixbit sixState) {
            if (sixState.BitLength() != 168) throw new AisMessageException("Message 11 wrong length");

            base.Parse(sixState);

            /* Parse the Message 11 */
            Utc_year = (int) sixState.Get(14);
            Utc_month = (int) sixState.Get(4);
            Utc_day = (int) sixState.Get(5);
            Utc_hour = (int) sixState.Get(5);
            Utc_minute = (int) sixState.Get(6);
            Utc_second = (int) sixState.Get(6);
            Pos_acc = (int) sixState.Get(1);

            Pos = new Position {
                Longitude = sixState.Get(28),
                Latitude = sixState.Get(27)
            };

            Pos_type = (int) sixState.Get(4);
            Spare = (int) sixState.Get(10);
            Raim = (int) sixState.Get(1);
            Sotdma_state = new Sotdma();
            Sotdma_state.Parse(sixState);
        }
    }
}