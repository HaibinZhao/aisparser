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
        public override void Parse(Sixbit sixState) {
            if (sixState.BitLength!= 72) throw new AisMessageException("Message 10 wrong length");

            base.Parse(sixState);

            Spare1 = (int) sixState.Get(2);
            Destination = sixState.Get(30);
            Spare2 = (int) sixState.Get(2);
        }
    }
}