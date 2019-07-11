namespace AisParser {
    /// <summary>
    ///     AIS Sotdma class
    /// </summary>
    public class Sotdma {
        public Sotdma() {
        }

        internal Sotdma(Sixbit sixState) {
            Parse(sixState);
        }

        /// <summary>
        ///     !&lt; 2 bits   : SOTDMA Sync State
        /// </summary>
        public int SyncState { get; private set; }

        /// <summary>
        ///     !&lt; 3 bits   : SOTDMA Slot Timeout
        /// </summary>
        public int SlotTimeout { get; private set; }

        /// <summary>
        ///     !&lt; 14 bits  : SOTDMA Sub-Messsage
        /// </summary>
        public int SubMessage { get; private set; }

        /// <summary>
        ///     Parse sixbit message
        /// </summary>
        /// <param name="sixState"></param>
        /// <exception cref="SixbitsExhaustedException"></exception>
        /// <exception cref="AisMessageException"></exception>
        public void Parse(Sixbit sixState) {
            if (sixState.BitLength() < 19) throw new AisMessageException("SOTDMA wrong length");

            SyncState = (char) sixState.Get(2);
            SlotTimeout = (char) sixState.Get(3);
            SubMessage = (int) sixState.Get(14);
        }
    }
}