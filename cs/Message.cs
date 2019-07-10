namespace AisParser {
    /// <summary>
    ///     AIS Message base class
    ///     All the messages are derived from this class which provides the msgid,
    ///     repeat value and userid
    /// </summary>
    public abstract class Messages {

        protected Messages(int msgId) {
            MsgId = msgId;
        }

        /// <summary>
        ///     6 bits  : Message ID (1)
        /// </summary>
        public int MsgId { get; private set; }

        /// <summary>
        ///     2 bits  : Repeated
        /// </summary>
        public int Repeat { get; private set; }

        /// <summary>
        ///     30 bits : UserID / MMSI
        /// </summary>
        public long UserId { get; private set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        protected virtual void Parse(Sixbit sixState) {
            Repeat = (int) sixState.Get(2);
            UserId = sixState.Get(30);
        }
    }
}