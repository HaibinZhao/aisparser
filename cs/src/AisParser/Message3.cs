namespace AisParser {
    /// <summary>
    ///     AIS Message 3 class
    ///     Position Report
    /// </summary>
    public sealed class Message3 : Message123 {
        public Message3():base(3) {
		}

		public Message3(Sixbit sixbit):this(){
			this.Parse(sixbit);
        }

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
            SlotIncrement = (int) sixState.Get(13);
            NumSlots = (int) sixState.Get(3);
            Keep = (int) sixState.Get(1);
        }
    }
}