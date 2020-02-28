namespace AisParser {
    /// <summary>
    /// 未知的 AIS 消息
    /// </summary>
    public class UnknownMessage : Messages {
        public UnknownMessage (int msgId, Sixbit sixbit) : base (msgId) {
            this.Parse (sixbit);
        }

        public override bool Equals (object obj) {
            return base.Equals (obj);
        }

        public override int GetHashCode () {
            return base.GetHashCode ();
        }

        public override void Parse (Sixbit sixState) {
            base.Parse (sixState);
        }

        public override string ToString () {
            return base.ToString ();
        }
    }
}