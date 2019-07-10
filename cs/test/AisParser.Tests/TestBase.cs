using Xunit;

namespace AisParser.Tests {
    public class TestBase {
        protected void AssertEquals(string message, int expected, int actual) {
            Assert.True(expected == actual, message);
        }

        protected void AssertEquals(string message, long expected, long actual) {
            Assert.True(expected == actual, message);
        }
    }
}