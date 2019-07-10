using System;

namespace AisParser {
    internal class AisMessageException : Exception {
        public AisMessageException() {
        }

        public AisMessageException(string str) : base(str) {
        }
    }
}