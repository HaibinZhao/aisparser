namespace AisParser {
    /// <summary>
    ///     AIS Message 10 class
    ///     UTC/date inquiry
    /// </summary>
    public sealed class Message10 : Messages {
        public Message10():base(10) {
		}

		public Message10(Sixbit sixbit):this(){
			this.Parse(sixbit);
        }

        /// <summary>
        ///     30 bits  : Destination MMSI
        /// </summary>
        public long Destination { get; private set; }

        /// <summary>
        ///     2 bits   : Spare
        /// </summary>
        public int Spare1 { get; private set; }

        /// <summary>
        ///     2 bits   : Spare
        /// </summary>
        public int Spare2 { get; private set; }


        /// <summary>
        ///     Parse sixbit message
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse(Sixbit six_state) {
            if (six_state.BitLength() != 72) throw new AisMessageException("Message 10 wrong length");

            base.Parse(six_state);

            Spare1 = (int) six_state.Get(2);
            Destination = six_state.Get(30);
            Spare2 = (int) six_state.Get(2);
        }
    }
}