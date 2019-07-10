namespace AisParser {
    /// <summary>
    ///     AIS Parser SDK
    ///     AIS Message 16 Class
    ///     Copyright 2008 by Brian C. Lane
    ///     <bcl@ brianlane.com>
    ///         All Rights Reserved
    ///         @author Brian C. Lane
    /// </summary>
    /// <summary>
    ///     AIS Message 16 class
    ///     Assignment mode command
    /// </summary>
    public class Message16 : Messages {
        /// <summary>
        ///     2 bits   : Spare
        /// </summary>
        public int Spare1 { get; private set; }

        /// <summary>
        ///     30 bits  : Destination MMSI A
        /// </summary>
        public long DestIdA { get; private set; }

        /// <summary>
        ///     12 bits  : Slot Offset A
        /// </summary>
        public int OffsetA { get; private set; }

        /// <summary>
        ///     10 bits  : Increment A
        /// </summary>
        public int IncrementA { get; private set; }

        /// <summary>
        ///     30 bits  : Destination MMSI B
        /// </summary>
        public long DestIdB { get; private set; }

        /// <summary>
        ///     12 bits  : Slot Offset B
        /// </summary>
        public int OffsetB { get; private set; }

        /// <summary>
        ///     10 bits  : Increment B
        /// </summary>
        public int IncrementB { get; private set; }

        /// <summary>
        ///     4 bits   : Spare
        /// </summary>
        public int Spare2 { get; private set; }

        /// <summary>
        ///     Number of commands received
        /// </summary>
        public int NumCmds { get; private set; }

        public Message16() : base(16) { }

        /// <summary>
        ///     Subclasses need to override with their own parsing method
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        protected override void Parse(Sixbit sixState) {
            var length = sixState.BitLength();
            if (length < 96 || length > 144) throw new AisMessageException("Message 16 wrong length");

            base.Parse(sixState);

            Spare1 = (int) sixState.Get(2);
            DestIdA = sixState.Get(30);
            OffsetA = (int) sixState.Get(12);
            IncrementA = (int) sixState.Get(10);
            NumCmds = 1;

            if (length == 144) {
                DestIdB = sixState.Get(30);
                OffsetB = (int) sixState.Get(12);
                IncrementB = (int) sixState.Get(10);
                Spare2 = (int) sixState.Get(4);
                NumCmds = 2;
            }
        }
    }
}