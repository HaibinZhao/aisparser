namespace AisParser {
    /// <summary>
    ///     AIS Message 20 class
    ///     Data Link Management Message
    /// </summary>
    public class Message20 : Messages {

        public Message20() : base(20) { }

        /// <summary>
        ///     2 bits   : Spare
        /// </summary>
        public int Spare1 { get; private set; }

        /// <summary>
        ///     12 bits  : Slot Offset 1
        /// </summary>
        public int Offset1 { get; private set; }

        /// <summary>
        ///     4 bits   : Number of Slots 1
        /// </summary>
        public int Slots1 { get; private set; }

        /// <summary>
        ///     3 bits   : Timeout in Minutes 2
        /// </summary>
        public int Timeout1 { get; private set; }

        /// <summary>
        ///     11 bits  : Slot Increment 1
        /// </summary>
        public int Increment1 { get; private set; }

        /// <summary>
        ///     12 bits  : Slot Offset 2
        /// </summary>
        public int Offset2 { get; private set; }

        /// <summary>
        ///     4 bits   : Number of Slots 2
        /// </summary>
        public int Slots2 { get; private set; }

        /// <summary>
        ///     3 bits   : Timeout in Minutes 2
        /// </summary>
        public int Timeout2 { get; private set; }

        /// <summary>
        ///     11 bits  : Slot Increment 2
        /// </summary>
        public int Increment2 { get; private set; }

        /// <summary>
        ///     12 bits  : Slot Offset 3
        /// </summary>
        public int Offset3 { get; private set; }

        /// <summary>
        ///     4 bits   : Number of Slots 3
        /// </summary>
        public int Slots3 { get; private set; }

        /// <summary>
        ///     3 bits   : Timeout in Minutes 3
        /// </summary>
        public int Timeout3 { get; private set; }

        /// <summary>
        ///     11 bits  : Slot Increment 3
        /// </summary>
        public int Increment3 { get; private set; }

        /// <summary>
        ///     12 bits  : Slot Offset 4
        /// </summary>
        public int Offset4 { get; private set; }

        /// <summary>
        ///     4 bits   : Number of Slots 4
        /// </summary>
        public int Slots4 { get; private set; }

        /// <summary>
        ///     3 bits   : Timeout in Minutes 4
        /// </summary>
        public int Timeout4 { get; private set; }

        /// <summary>
        ///     11 bits  : Slot Increment 4
        /// </summary>
        public int Increment4 { get; private set; }

        /// <summary>
        ///     0-6 bits : Spare
        /// </summary>
        public int Spare2 { get; private set; }

        /// <summary>
        ///     Number of commands received
        /// </summary>
        public int NumCmds { get; private set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        protected override void Parse(Sixbit sixState) {
            var length = sixState.BitLength();
            if (length < 72 || length > 162) throw new AisMessageException("Message 20 wrong length");

            base.Parse(sixState);

            Spare1 = (int) sixState.Get(2);
            Offset1 = (int) sixState.Get(12);
            Slots1 = (int) sixState.Get(4);
            Timeout1 = (int) sixState.Get(3);
            Increment1 = (int) sixState.Get(11);
            NumCmds = 1;

            if (length > 72) {
                Offset2 = (int) sixState.Get(12);
                Slots2 = (int) sixState.Get(4);
                Timeout2 = (int) sixState.Get(3);
                Increment2 = (int) sixState.Get(11);
                NumCmds = 2;
            }

            if (length > 104) {
                Offset3 = (int) sixState.Get(12);
                Slots3 = (int) sixState.Get(4);
                Timeout3 = (int) sixState.Get(3);
                Increment3 = (int) sixState.Get(11);
                NumCmds = 3;
            }

            if (length > 136) {
                Offset4 = (int) sixState.Get(12);
                Slots4 = (int) sixState.Get(4);
                Timeout4 = (int) sixState.Get(3);
                Increment4 = (int) sixState.Get(11);
                NumCmds = 4;
            }
        }
    }
}