namespace AisParser {
    /// <summary>
    ///     AIS Message 2 class
    ///     Position Report
    /// </summary>
    public sealed class Message2 : Message123 {
        public Message2():base(2) {
		}

		public Message2(Sixbit sixbit):this(){
			this.Parse(sixbit);
        }

        /// <summary>
        ///     3 bits  : SOTDMA Slot Timeout
        /// </summary>
        public int SlotTimeout { get; private set; }

        /// <summary>
        ///     14 bits : SOTDMA sub-message
        /// </summary>
        public int SubMessage { get; private set; }

        //JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
        //ORIGINAL LINE: public void parse(Sixbit six_state) throws SixbitsExhaustedException, AISMessageException
        public override void Parse(Sixbit sixState) {
            if (sixState.BitLength!= 168) throw new AisMessageException("Message 2 wrong length");

            base.Parse(sixState);

            /* Parse the Message 2 */
            SlotTimeout = (int) sixState.Get(3);
            SubMessage = (int) sixState.Get(14);
        }
    }
}