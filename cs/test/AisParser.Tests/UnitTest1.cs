using System;
using Xunit;

namespace AisParser.Tests {
    public class Message1Tests: TestBase {
        [Fact]
        public void TestParse() {
            var vdmMessage = new Vdm();
            var msg = new Message1();

            var result = vdmMessage.Add("!AIVDM,1,1,,B,19NS7Sp02wo?HETKA2K6mUM20<L=,0*27\r\n");
                AssertEquals("vdm add failed", 0, (int)result);
                msg.Parse(vdmMessage.SixState);
            

            AssertEquals("msgid", 1, msg.MsgId);
            AssertEquals("repeat", 0, msg.Repeat);
            AssertEquals("userid", 636012431, msg.UserId);
            AssertEquals("nav_status", 8, msg.NavStatus);
            AssertEquals("rot", 0, msg.Rot);
            AssertEquals("sog", 191, msg.Sog);
            AssertEquals("pos_acc", 1, msg.PosAcc);
            AssertEquals("longitude", -73481550, msg.Pos.Longitude);
            AssertEquals("latitude", 28590700, msg.Pos.Latitude);
            AssertEquals("cog", 1750, msg.Cog);
            AssertEquals("true_heading", 174, msg.TrueHeading);
            AssertEquals("utc_sec", 33, msg.UtcSec);
            AssertEquals("regional", 0, msg.Regional);
            AssertEquals("spare", 0, msg.Spare);
            AssertEquals("raim", 0, msg.Raim);
            AssertEquals("sync_state", 0, msg.SyncState);
            AssertEquals("slot_timeout", 3, msg.SlotTimeout);
            AssertEquals("sub_message", 1805, msg.SubMessage);
        }
    }
}
