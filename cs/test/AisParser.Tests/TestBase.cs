using Xunit;

namespace AisParser.Tests {
    public class TestBase {
        protected void AssertEquals(string message, int expected, int actual) {
            Assert.True(expected == actual, $"{message} Expected:{expected} But Actual:{actual}");
        }

        protected void AssertEquals(string message, long expected, long actual) {
            Assert.True(expected == actual, $"{message} Expected:{expected} But Actual:{actual}");
        }

        protected void AssertEquals(string message, int expected, VdmStatus actual) {
            Assert.True(expected == (int) actual, message);
        }
        protected void AssertEquals(string message, string expected, string actual) {
            Assert.True(expected == actual,$"{message} Expected:{expected} But Actual:{actual}");
        }

    }
}