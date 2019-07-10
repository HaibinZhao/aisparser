namespace AisParser {
    /// <summary>
    ///     AIS Message 8 class
    ///     Binary Broadcast Message
    /// </summary>
    public sealed class Message8 : Messages {
        public Message8():base(8) {
		}

		public Message8(Sixbit sixbit):this(){
			this.Parse(sixbit);
        }

        /// <summary>
        ///     2 bits   : Spare
        /// </summary>
        public int Spare { get; private set; }

        /// <summary>
        ///     16 bits  : Application ID
        /// </summary>
        public int App_id { get; private set; }

        /// <summary>
        ///     952 bits : Data payload
        /// </summary>
        public Sixbit Data { get; private set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse(Sixbit sixState) {
            var length = sixState.BitLength();
            if (length < 56 || length > 1008) throw new AisMessageException("Message 8 wrong length");

            base.Parse(sixState);

            Spare = (int) sixState.Get(2);
            App_id = (int) sixState.Get(16);

            /* Store the remaining payload of the packet for further processing */
            Data = sixState;
        }
    }
}