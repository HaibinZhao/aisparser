namespace AisParser {
    /// <summary>
    ///     AIS Message 17 class
    ///     DGNSS Broadcast binary message
    /// </summary>
    public sealed class Message17 : Messages {
        public Message17():base(17) {
		}

		public Message17(Sixbit sixbit):this(){
			this.Parse(sixbit);
        }

        /// <summary>
        ///     2 bits      : Spare
        /// </summary>
        public int Spare1 { get; private set; }

        /// <summary>
        ///     : Lat/Long 1/100000 minute
        /// </summary>
        public Position Pos { get; private set; }

        /// <summary>
        ///     5 bits      : Spare
        /// </summary>
        public int Spare2 { get; private set; }

        /// <summary>
        ///     6 bits      : Mesage Type from M.823
        /// </summary>
        public int MsgType { get; private set; }

        /// <summary>
        ///     10 bits     : Station ID from M.823
        /// </summary>
        public int StationId { get; private set; }

        /// <summary>
        ///     13 bits     : Z Count
        /// </summary>
        public int ZCount { get; private set; }

        /// <summary>
        ///     3 bits      : Sequence Number
        /// </summary>
        public int SeqNum { get; private set; }

        /// <summary>
        ///     5 bits      : Number of Data Words
        /// </summary>
        public int NumWords { get; private set; }

        /// <summary>
        ///     3 bits      : Reference Station Health from M.823
        /// </summary>
        public int Health { get; private set; }

        /// <summary>
        ///     0-696 bits  : Data payload
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
            if (length < 80 || length > 816) throw new AisMessageException("Message 17 wrong length");

            base.Parse(sixState);

            Spare1 = (int) sixState.Get(2);

            Pos = new Position();
            Pos.Longitude = sixState.Get(18) * 10;
            Pos.Latitude = sixState.Get(17) * 10;

            Spare2 = (int) sixState.Get(5);
            MsgType = (int) sixState.Get(6);
            StationId = (int) sixState.Get(10);
            ZCount = (int) sixState.Get(13);
            SeqNum = (int) sixState.Get(3);
            NumWords = (int) sixState.Get(5);
            Health = (int) sixState.Get(3);

            Data = sixState;
        }
    }
}