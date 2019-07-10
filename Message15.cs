namespace AisParser {
    /// <summary>
    ///     AIS Message 15 class
    ///     Interrogation
    /// </summary>
    public sealed class Message15 : Messages {
        public Message15():base(15) {
		}

		public Message15(Sixbit sixbit):this(){
			this.Parse(sixbit);
        }

        /// <summary>
        ///     2 bits   : Spare
        /// </summary>
        public int Spare1 { get; private set; }

        /// <summary>
        ///     30 bits  : Destination MMSI 1
        /// </summary>
        public long Destid1 { get; private set; }

        /// <summary>
        ///     6 bits   : MessageID 1.1
        /// </summary>
        public int Msgid1_1 { get; private set; }

        /// <summary>
        ///     12 bits  : Slot Offset 1.1
        /// </summary>
        public int Offset1_1 { get; private set; }

        /// <summary>
        ///     2 bits   : Spare
        /// </summary>
        public int Spare2 { get; private set; }

        /// <summary>
        ///     6 bits   : MessageID 1.2
        /// </summary>
        public int Msgid1_2 { get; private set; }

        /// <summary>
        ///     12 bits  : Slot Offset 1.2
        /// </summary>
        public int Offset1_2 { get; private set; }

        /// <summary>
        ///     2 bits   : Spare
        /// </summary>
        public int Spare3 { get; private set; }

        /// <summary>
        ///     30 bits  : Destination MMSI 2
        /// </summary>
        public long Destid2 { get; private set; }

        /// <summary>
        ///     6 bits   : MessageID 2.1
        /// </summary>
        public int Msgid2_1 { get; private set; }

        /// <summary>
        ///     12 bits  : Slot Offset 2.1
        /// </summary>
        public int Offset2_1 { get; private set; }

        /// <summary>
        ///     2 bits   : Spare
        /// </summary>
        public int Spare4 { get; private set; }

        /// <summary>
        ///     Number of interrogation requests
        /// </summary>
        public int NumReqs { get; private set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse(Sixbit sixState) {
            var length = sixState.BitLength();
            if (length < 88 || length > 162) throw new AisMessageException("Message 15 wrong length");

            Spare1 = (int) sixState.Get(2);
            Destid1 = sixState.Get(30);
            Msgid1_1 = (int) sixState.Get(6);
            Offset1_1 = (int) sixState.Get(12);
            NumReqs = 1;

            if (length > 88) {
                Spare2 = (int) sixState.Get(2);
                Msgid1_2 = (int) sixState.Get(6);
                Offset1_2 = (int) sixState.Get(12);
                NumReqs = 2;
            }

            if (length == 160) {
                Spare3 = (int) sixState.Get(2);
                Destid2 = sixState.Get(30);
                Msgid2_1 = (int) sixState.Get(6);
                Offset2_1 = (int) sixState.Get(12);
                Spare4 = (int) sixState.Get(2);
                NumReqs = 3;
            }
        }
    }
}