using System;
using Xunit;

namespace AisParser.Tests {
    public class MessagesTests: TestBase {
        [Fact]
        public void TestMessage1() {
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

        public void TestMessage2() {
            var  vdm_message = new Vdm();
            var msg = new Message2();

            //fail("Not yet implemented -- Need an example Message 2 Packet");

            var  result = vdm_message.Add("");
                AssertEquals("vdm add failed", 0, (int)result);
                msg.Parse(vdm_message.SixState);
            

            AssertEquals("msgid", 2, msg.MsgId);
            AssertEquals("repeat", 0, msg.Repeat);
            AssertEquals("userid", 0, msg.UserId);
        }

        [Fact]
        public void TestMessage3() {
            var vdm_message = new Vdm();
            var  msg = new Message3();

           var result = vdm_message.Add("!AIVDM,1,1,,B,35Mk33gOkSG?bLtK?;B2dRO`00`A,0*30");
                AssertEquals("vdm add failed", 0, result);
                msg.Parse(vdm_message.SixState);
           

            AssertEquals("msgid", 3, msg.MsgId);
            AssertEquals("repeat", 0, msg.Repeat);
            AssertEquals("userid", 366789390, msg.UserId);
            AssertEquals("nav_status", 15, msg.NavStatus);
            AssertEquals("rot", 127, msg.Rot);
            AssertEquals("sog", 227, msg.Sog);
            AssertEquals("pos_acc", 0, msg.PosAcc);
            AssertEquals("longitude", -73444450, msg.Pos.Longitude);
            AssertEquals("latitude", 28560200, msg.Pos.Latitude);
            AssertEquals("cog", 690, msg.Cog);
            AssertEquals("true_heading", 79, msg.TrueHeading);
            AssertEquals("utc_sec", 52, msg.UtcSec);
            AssertEquals("regional", 0, msg.Regional);
            AssertEquals("spare", 0, msg.Spare);
            AssertEquals("raim", 0, msg.Raim);
            AssertEquals("sync_state", 0, msg.SyncState);
            AssertEquals("slot_increment", 161, msg.SlotIncrement);
            AssertEquals("num_slots", 0, msg.NumSlots);
            AssertEquals("keep", 1, msg.Keep);
        }

        [Fact]
        public void TestMessage4() {
            var vdm_message = new Vdm();
            var msg = new Message4();


            var result = vdm_message.Add("!AIVDM,1,1,,A,403OwpiuIKl:Ro=sbvK=CG700<3b,0*5E");
            AssertEquals("vdm add failed", 0, result);
            msg.Parse(vdm_message.SixState);


            AssertEquals("msgid", 4, msg.MsgId);
            AssertEquals("repeat", 0, msg.Repeat);
            AssertEquals("userid", 3669987, msg.UserId);
            AssertEquals("utc_year", 2006, msg.UtcYear);
            AssertEquals("utc_month", 5, msg.UtcMonth);
            AssertEquals("utc_day", 23, msg.UtcDay);
            AssertEquals("utc_hour", 20, msg.UtcHour);
            AssertEquals("utc_minute", 10, msg.UtcMinute);
            AssertEquals("utc_second", 34, msg.UtcSecond);
            AssertEquals("pos_acc", 1, msg.PosAcc);
            AssertEquals("longitude", -73671329, msg.Pos.Longitude);
            AssertEquals("latitude", 28529500, msg.Pos.Latitude);
            AssertEquals("pos_type", 7, msg.PosType);
            AssertEquals("spare", 0, msg.Spare);
            AssertEquals("raim", 0, msg.Raim);
            AssertEquals("sync_state", 0, msg.SyncState);
            AssertEquals("slot_timeout", 3, msg.SlotTimeout);
            AssertEquals("sub_message", 234, msg.SubMessage);
        }

        [Fact]
        public void TestMessage5() {
            var vdm_message = new Vdm();
            var msg = new Message5();


            var result = vdm_message.Add("!AIVDM,2,1,9,A,55Mf@6P00001MUS;7GQL4hh61L4hh6222222220t41H,0*49\r\n");
            AssertEquals("vdm add failed", 1, result);

            result = vdm_message.Add("!AIVDM,2,2,9,A,==40HtI4i@E531H1QDTVH51DSCS0,2*16\r\n");
            AssertEquals("vdm add failed", 0, result);

            msg.Parse(vdm_message.SixState);


            AssertEquals("msgid", 5, msg.MsgId);
            AssertEquals("repeat", 0, msg.Repeat);
            AssertEquals("userid", 366710810, msg.UserId);
            AssertEquals("version", 0, msg.Version);
            AssertEquals("imo", 0, msg.Imo);
            AssertEquals("callsign", "WYX2158", msg.Callsign);
            AssertEquals("name", "WALLA WALLA         ", msg.Name);
            AssertEquals("ship_type", 60, msg.ShipType);
            AssertEquals("dim_bow", 32, msg.DimBow);
            AssertEquals("dim_stern", 88, msg.DimStern);
            AssertEquals("dim_starboard", 13, msg.DimStarboard);
            AssertEquals("dim_port", 13, msg.DimPort);
            AssertEquals("pos_type", 1, msg.PosType);
            AssertEquals("eta", 1596, msg.Eta);
            AssertEquals("draught", 100, msg.Draught);
            AssertEquals("dest", "SEATTLE FERRY TERMNL", msg.Dest);
            AssertEquals("dte", 0, msg.Dte);
            AssertEquals("spare", 0, msg.Spare);
        }

        public void TestMessage7() {
            var vdm_message = new Vdm();
            var msg = new Message7();


            var result = vdm_message.Add("!AIVDM,1,1,,A,703Owpi9lmaQ,0*3B");
            AssertEquals("vdm add failed", 0, result);

            msg.Parse(vdm_message.SixState);


            AssertEquals("num_acks", 1, msg.NumAcks);
            AssertEquals("msgid", 7, msg.MsgId);
            AssertEquals("repeat", 0, msg.Repeat);
            AssertEquals("userid", 3669987, msg.UserId);
            AssertEquals("spare", 0, msg.Spare);
            AssertEquals("destid_1", 309647000, msg.Destid1);
            AssertEquals("sequence_1", 1, msg.Sequence1);
        }


        [Fact]
        public void TestMessage8() {
            var vdm_message = new Vdm();
            var msg = new Message8();


           var  result = vdm_message.Add("!AIVDM,3,1,1,A,85MwqciKf@nWshjR1VfGGDssdvT>hncBfTwcsgGKo?t,0*2E\r\n");
            AssertEquals("vdm add failed", 1, result);

            result = vdm_message.Add("!AIVDM,3,2,1,A,u1uBo`7b`1Oa>@cO0f2wr1mwb0=kf<tI2MwS;sVKU07,0*67\r\n");
            AssertEquals("vdm add failed", 1, result);

            result = vdm_message.Add("!AIVDM,3,3,1,A,8fDSaOKeP,2*0C\r\n");
            AssertEquals("vdm add failed", 0, result);

            msg.Parse(vdm_message.SixState);


            AssertEquals("msgid", 8, msg.MsgId);
            AssertEquals("repeat", 0, msg.Repeat);
            AssertEquals("userid", 366999983, msg.UserId);
            AssertEquals("spare", 0, msg.Spare);
            AssertEquals("app_id", 23481, msg.AppId);

            Sixbit data = msg.Data;
            AssertEquals("data length", 568, data.BitLength);

            // Here is where the payload would be parsed, if we knew how...	
        }


        [Fact]
        public void TestMessage9() {
            var vdm_message = new Vdm();
            var msg = new Message9();


            var result = vdm_message.Add("!AIVDM,1,1,,B,900048wwTcJb0mpF16IobRP2086Q,0*48\r\n");
            AssertEquals("vdm add failed", 0, result);

            msg.Parse(vdm_message.SixState);


            AssertEquals("msgid", 9, msg.MsgId);
            AssertEquals("repeat", 0, msg.Repeat);
            AssertEquals("userid", 1059, msg.UserId);
            AssertEquals("altitude", 4094, msg.Altitude);
            AssertEquals("sog", 299, msg.Sog);
            AssertEquals("pos_acc", 0, msg.PosAcc);
            AssertEquals("longitude", -44824900, msg.Pos.Longitude);
            AssertEquals("latitude", 23086695, msg.Pos.Latitude);
            AssertEquals("cog", 1962, msg.Cog);
            AssertEquals("utc_sec", 10, msg.UtcSec);
            AssertEquals("regional", 0, msg.Regional);
            AssertEquals("dte", 1, msg.Dte);
            AssertEquals("spare", 0, msg.Spare);
            AssertEquals("assigned", 0, msg.Assigned);
            AssertEquals("raim", 0, msg.Raim);
            AssertEquals("comm_state", 0, msg.CommState);
            if (msg.CommState == 0) {
                AssertEquals("sotdma.sync_state", 0, msg.SotdmaState.SyncState);
                AssertEquals("sotdma.slot_timeout", 2, msg.SotdmaState.SlotTimeout);
                AssertEquals("sotdma.sub_message", 417, msg.SotdmaState.SubMessage);
            } else {
                Assert.False(msg.CommState==0,"itdma state");
            }

        }

        [Fact]
        public void TestMessage10() {
            var vdm_message = new Vdm();
            var msg = new Message10();


            var result = vdm_message.Add("!AIVDM,1,1,,A,:5D2Lp1Ghfe0,0*4E\r\n");
            AssertEquals("vdm add failed", 0, result);

            msg.Parse(vdm_message.SixState);


            AssertEquals("msgid", 10, msg.MsgId);
            AssertEquals("repeat", 0, msg.Repeat);
            AssertEquals("userid", 356556000, msg.UserId);
            AssertEquals("spare1", 0, msg.Spare1);
            AssertEquals("destination", 368098000, msg.Destination);
            AssertEquals("spare2", 0, msg.Spare2);
        }

        [Fact]
        public void TestMessage11() {
            var vdm_message = new Vdm();
            var msg = new Message11();


            var result = vdm_message.Add("!AIVDM,1,1,,A,;4WOL21uM<jCroP`g8B=NFQ00000,0*37\r\n");
            AssertEquals("vdm add failed", 0, result);

            msg.Parse(vdm_message.SixState);


            AssertEquals("msgid", 11, msg.MsgId);
            AssertEquals("repeat", 0, msg.Repeat);
            AssertEquals("userid", 309845000, msg.UserId);
            AssertEquals("utc_year", 2007, msg.UtcYear);
            AssertEquals("utc_month", 4, msg.UtcMonth);
            AssertEquals("utc_day", 25, msg.UtcDay);
            AssertEquals("utc_hour", 18, msg.UtcHour);
            AssertEquals("utc_minute", 19, msg.UtcMinute);
            AssertEquals("utc_second", 58, msg.UtcSecond);
            AssertEquals("pos_acc", 1, msg.PosAcc);
            AssertEquals("longitude", -71219740, msg.Pos.Longitude);
            AssertEquals("latitude", 19095130, msg.Pos.Latitude);
            AssertEquals("pos_type", 1, msg.PosType);
            AssertEquals("spare", 0, msg.Spare);
            AssertEquals("raim", 0, msg.Raim);
            AssertEquals("sotdma.sync_state",0 ,msg.SotdmaState.SyncState);
            AssertEquals("sotdma.slot_timeout", 0, msg.SotdmaState.SlotTimeout);
            AssertEquals("sotdma.sub_message", 0, msg.SotdmaState.SubMessage);
        }

        [Fact]
        public void TestMessage12() {
            var vdm_message = new Vdm();
            var msg = new Message12();


            var result = vdm_message.Add("!AIVDM,1,1,,A,<03Owph00002QG51D85BP1<5BDQP,0*7D");
            AssertEquals("vdm add failed", 0, result);

            msg.Parse(vdm_message.SixState);


            AssertEquals("msgid", 12, msg.MsgId);
            AssertEquals("repeat", 0, msg.Repeat);
            AssertEquals("userid", 3669987, msg.UserId);
            AssertEquals("sequence", 0, msg.Sequence);
            AssertEquals("destination", 0, msg.Destination);
            AssertEquals("retransmit", 1, msg.Retransmit);
            AssertEquals("message", "!WEATHER ALERT! ", msg.Message);

        }

        [Fact]
        public void TestMessage13() {
            var vdm_message = new Vdm();
            var msg = new Message13();


            var result = vdm_message.Add("!AIVDM,1,1,,A,=03Owpi;Eo7`,0*7F");
            AssertEquals("vdm add failed", 0, result);

            msg.Parse(vdm_message.SixState);


            AssertEquals("num_acks", 1, msg.NumAcks);
            AssertEquals("msgid", 13, msg.MsgId);
            AssertEquals("repeat", 0, msg.Repeat);
            AssertEquals("userid", 3669987, msg.UserId);
            AssertEquals("spare", 0, msg.Spare);
            AssertEquals("destid_1", 316005498, msg.Destid1);
            AssertEquals("sequence_1", 0, msg.Sequence1);

        }

        [Fact]
        public void TestMessage15() {
            var vdm_message = new Vdm();
            var msg = new Message15();


            var result = vdm_message.Add("!AIVDM,1,1,,A,?03OwpiGPmD0000,2*07\r\n");
            AssertEquals("vdm add failed", 0, result);

            msg.Parse(vdm_message.SixState);


            AssertEquals("msgid", 15, msg.MsgId);
            AssertEquals("repeat", 0, msg.Repeat);
            AssertEquals("userid", 3669987, msg.UserId);
            AssertEquals("num_reqs", 1, msg.NumReqs);
            AssertEquals("spare1", 0, msg.Spare1);
            AssertEquals("destid1", 367056192, msg.Destid1);
            AssertEquals("msgid1_1", 0, msg.MsgId1_1);
            AssertEquals("offset1_1", 0, msg.Offset1_1);
        }

        [Fact]
        public void TestMessage18() {
            var vdm_message = new Vdm();
            var msg = new Message18();


            var result = vdm_message.Add("!AIVDM,1,1,,A,B52IRsP005=abWRnlQP03w`UkP06,0*2A\r\n");
            AssertEquals("vdm add failed", 0, result);

            msg.Parse(vdm_message.SixState);


            AssertEquals("msgid", 18, msg.MsgId);
            AssertEquals("repeat", 0, msg.Repeat);
            AssertEquals("userid", 338060014, msg.UserId);
            AssertEquals("regional1", 0, msg.Regional1);
            AssertEquals("sog", 0, msg.Sog);
            AssertEquals("pos_acc", 0, msg.PosAcc);
            AssertEquals("longitude", -93506225, msg.Pos.Longitude);
            AssertEquals("latitude", 11981336, msg.Pos.Latitude);
            AssertEquals("cog", 0, msg.Cog);
            AssertEquals("true_heading", 511, msg.TrueHeading);
            AssertEquals("utc_sec", 17, msg.UtcSec);
            AssertEquals("regional2", 0, msg.Regional2);
            AssertEquals("unit_flag", 1, msg.UnitFlag);
            AssertEquals("display_flag", 0, msg.DisplayFlag);
            AssertEquals("dsc_flag", 1, msg.DscFlag);
            AssertEquals("band_flag", 1, msg.BandFlag);
            AssertEquals("msg22_flag", 1, msg.Msg22Flag);
            AssertEquals("mode_flag", 0, msg.ModeFlag);
            AssertEquals("raim", 0, msg.Raim);
            AssertEquals("comm_state", 1, msg.CommState);

            Assert.False(msg.CommState==0, "sotdma state");
                AssertEquals("itdma.sync_state", 3, msg.ItdmaState.SyncState);
                AssertEquals("itdma.slot_inc", 0, msg.ItdmaState.SlotInc);
                AssertEquals("itdma.num_slots", 3, msg.ItdmaState.NumSlots);
                AssertEquals("itdma.keep_flag", 0, msg.ItdmaState.KeepFlag);

        }

        [Fact]
        public void TestMessage20() {
            var vdm_message = new Vdm();
            var msg = new Message20();


           var  result = vdm_message.Add("!AIVDM,1,1,,A,D03OwphiIN>4,0*25");
            AssertEquals("vdm add failed", 0, result);

            msg.Parse(vdm_message.SixState);


            AssertEquals("num_cmds", 1, msg.NumCmds);
            AssertEquals("msgid", 20, msg.MsgId);
            AssertEquals("repeat", 0, msg.Repeat);
            AssertEquals("userid", 3669987, msg.UserId);
            AssertEquals("spare1", 0, msg.Spare1);
            AssertEquals("offset1", 790, msg.Offset1);
            AssertEquals("slots1", 5, msg.Slots1);
            AssertEquals("timeout1", 7, msg.Timeout1);
            AssertEquals("increment1", 225, msg.Increment1);
        }

        [Fact]
        public void TestMessage24() {
            var vdm_message = new Vdm();
            var msg = new Message24();


            // Part A
            var result = vdm_message.Add("!AIVDM,1,1,,A,H52IRsP518Tj0l59D0000000000,2*45");
            AssertEquals("vdm add failed", 0, result);
            msg.Parse(vdm_message.SixState);

            // Part B
            result = vdm_message.Add("!AIVDM,1,1,,A,H52IRsTU000000000000000@5120,0*76");
            AssertEquals("vdm add failed", 0, result);
            msg.Parse(vdm_message.SixState);


            AssertEquals("msgid", 24, msg.MsgId);
            AssertEquals("repeat", 0, msg.Repeat);
            AssertEquals("userid", 338060014, msg.UserId);
            AssertEquals("name", "APRIL MARU", msg.Name);
            AssertEquals("ship_type", 37, msg.ShipType);
            AssertEquals("vendor_id", "", msg.VendorId);// @@@@@@@ = not available = default
            AssertEquals("callsign", "", msg.Callsign); // @@@@@@@ = not available = default
            AssertEquals("dim_bow", 2, msg.DimBow);
            AssertEquals("dim_stern", 5, msg.DimStern);
            AssertEquals("dim_port", 1, msg.DimPort);
            AssertEquals("dim_starboard", 2, msg.DimStarboard);
            AssertEquals("spare", 0, msg.Spare);
            AssertEquals("flags", 3, msg.Flags);
        }

    }
}
