namespace AisParser {
    /// <summary>
    ///     AIS Message 7 class
    ///     Binary Acknowledgement
    /// </summary>
    public class Message7 : Messages {
        public Message7() : base(7) {
        }

        /// <summary>
        ///     2 bits   : Spare
        /// </summary>
        public int Spare { get; private set; }

        /// <summary>
        ///     30 bits  : Destination MMSI 1
        /// </summary>
        public long Destid1 { get; private set; }

        /// <summary>
        ///     2 bits   : Sequence Number 1
        /// </summary>
        public int Sequence1 { get; private set; }

        /// <summary>
        ///     30 bits  : Destination MMSI 2
        /// </summary>
        public long Destid2 { get; private set; }

        /// <summary>
        ///     2 bits   : Sequence Number 2
        /// </summary>
        public int Sequence2 { get; private set; }

        /// <summary>
        ///     30 bits  : Destination MMSI 3
        /// </summary>
        public long Destid3 { get; private set; }

        /// <summary>
        ///     2 bits   : Sequence Number 3
        /// </summary>
        public int Sequence3 { get; private set; }

        /// <summary>
        ///     30 bits  : Destination MMSI 4
        /// </summary>
        public long Destid4 { get; private set; }

        /// <summary>
        ///     2 bits   : Sequence Number 4
        /// </summary>
        public int Sequence4 { get; private set; }

        /// <summary>
        ///     Number of acks
        /// </summary>
        public int NumAcks { get; private set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="msgid"></param>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        protected override void Parse(Sixbit sixState) {
            var length = sixState.BitLength();
            if (length < 72 || length > 168) throw new AisMessageException("Message 7 wrong length");

            base.Parse(sixState);

            Spare = (int) sixState.Get(2);
            Destid1 = sixState.Get(30);
            Sequence1 = (int) sixState.Get(2);
            NumAcks = 1;

            if (length > 72) {
                Destid2 = sixState.Get(30);
                Sequence2 = (int) sixState.Get(2);
                NumAcks++;
            }

            if (length > 104) {
                Destid3 = sixState.Get(30);
                Sequence3 = (int) sixState.Get(2);
                NumAcks++;
            }

            if (length > 136) {
                Destid4 = sixState.Get(30);
                Sequence4 = (int) sixState.Get(2);
                NumAcks++;
            }
        }
    }
}