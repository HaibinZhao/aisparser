using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Xunit;

namespace AisParser.Tests {
    public class VdmTest : TestBase {

        Vdm _vdmMessage;


        public VdmTest() {
            _vdmMessage = new Vdm();
        }

        [Fact]
        public void TestMsgid() {
            var result = _vdmMessage.Add("!AIVDM,1,1,,B,19NS7Sp02wo?HETKA2K6mUM20<L=,0*27\r\n");
            AssertEquals("Adding message 1 failed", 0, result);
            AssertEquals("Message ID wrong", 1, _vdmMessage.MsgId);

        }

        [Fact]
        public void TestAdd() {
            var result = _vdmMessage.Add("!AIVDM,1,1,,B,19NS7Sp02wo?HETKA2K6mUM20<L=,0*27\r\n");
            AssertEquals("Adding message 1 failed", 0, result);

            _vdmMessage = new Vdm();
            result = _vdmMessage.Add("!AIVDM,2,1,6,B,55ArUT02:nkG<I8GB20nuJ0p5HTu>0hT9860TV16000006420BDi@E53,0*33\r\n");
            AssertEquals("Adding message 5 part 1 failed", 1, result);

            result = _vdmMessage.Add("!AIVDM,2,2,6,B,1KUDhH888888880,2*6A");
            AssertEquals("Adding message 5 part 1 failed", 0, result);

        }
    }


    /*
 * My skeleton test class
 */
}