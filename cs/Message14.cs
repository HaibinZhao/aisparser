namespace AisParser {
    /// <summary>
    ///     AIS Parser SDK
    ///     AIS Message 14 Class
    ///     Copyright 2008 by Brian C. Lane
    ///     <bcl@ brianlane.com>
    ///         All Rights Reserved
    ///         @author Brian C. Lane
    /// </summary>
    /// <summary>
    ///     AIS Message 14 class
    ///     Safety Related Broadcast
    /// </summary>
    public class Message14 : Messages {
        public Message14() : base(14) {
        }

        /// <summary>
        ///     2 bits   : Spare
        /// </summary>
        public int Spare { get; private set; }

        /// <summary>
        ///     968 bits : Message in ASCII
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="msgid"></param>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        protected override void Parse(Sixbit six_state) {
            var length = six_state.BitLength();
            if (length < 40 || length > 1008) throw new AisMessageException("Message 14 wrong length");

            base.Parse(six_state);

            Spare = (int) six_state.Get(2);
            Message = six_state.get_string((length - 40) / 6);
        }
    }
}