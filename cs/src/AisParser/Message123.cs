using System;

namespace AisParser {
    /// <summary>
    ///     AIS Message 1/2/3 class
    ///     Position Report
    /// </summary>
    public abstract class Message123 : Messages {
        protected Message123(int msgId) : base(msgId) {
            if (msgId != 1 && msgId != 2 && msgId != 3) {
                throw new ArgumentException("message Id must be [1,2,3]", nameof(msgId));
            }
        }

        /// <summary>
        ///     4 bits  : Navigational Status
        /// </summary>
        public int NavStatus { get; private set; }

        /// <summary>
        ///     8 bits  : Rate of Turn
        /// </summary>
        public int Rot { get; private set; }

        /// <summary>
        ///     10 bits : Speed Over Ground
        /// </summary>
        public int Sog { get; private set; }

        /// <summary>
        ///     1 bit   : Position Accuracy
        /// </summary>
        public int PosAcc { get; private set; }

        /// <summary>
        ///     : Lat/Long 1/10000 minute
        /// </summary>
        public Position Pos { get; private set; }

        /// <summary>
        ///     12 bits : Course over Ground
        /// </summary>
        public int Cog { get; private set; }

        /// <summary>
        ///     9 bits  : True heading
        /// </summary>
        public int TrueHeading { get; private set; }

        /// <summary>
        ///     6 bits  : UTC Seconds
        /// </summary>
        public int UtcSec { get; private set; }

        /// <summary>
        ///     4 bits  : Reserved for regional
        /// </summary>
        /// <remarks>
        /// manoeuvre indicator 
        ///     0 = not available = default 
        ///     1 = not engaged in special manoeuvre 
        ///     2 = engaged in special manoeuvre (i.e. regional passing arrangement on Inland Waterway) 
        /// </remarks>
        public int Regional { get; private set; }

        /// <summary>
        ///     1 bit   : Spare
        /// </summary>
        public int Spare { get; private set; }

        /// <summary>
        ///     1 bit   : RAIM flag
        /// </summary>
        public int Raim { get; private set; }

        /// <summary>
        ///     2 bits  : SOTDMA sync state
        /// </summary>
        public int SyncState { get; private set; }


        /// <summary>
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public override void Parse(Sixbit sixState) {
            if (sixState.BitLength != 168) throw new AisMessageException($"Message {MsgId} wrong length");

            base.Parse(sixState);

            /* Parse the Message 1 */
            NavStatus = (int) sixState.Get(4);
            Rot = (int) sixState.Get(8);
            Sog = (int) sixState.Get(10);
            PosAcc = (int) sixState.Get(1);

            Pos = new Position {
                Longitude = sixState.Get(28),
                Latitude = sixState.Get(27)
            };

            Cog = (int) sixState.Get(12);
            TrueHeading = (int) sixState.Get(9);
            UtcSec = (int) sixState.Get(6);
            Regional = (int) sixState.Get(4);
            Spare = (int) sixState.Get(1);
            Raim = (int) sixState.Get(1);
            SyncState = (int) sixState.Get(2);
        }
    }
}