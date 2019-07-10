using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Xunit;

namespace AisParser.Tests {
    public class test_aisparser : TestBase {

        static void test_sixbit(Sixbit ais_sixbit) {
            int msgid;
            long mmsi;
            msgid = (int)ais_sixbit.Get(6);
            ais_sixbit.Get(2);
            mmsi = ais_sixbit.Get(30);
            /*out.printf("Length = %d\n", ais_sixbit.length());
            out.printf("msgid = %d\n", msgid);
            out.printf("mmsi = %d\n", mmsi);*/

        }


        static void test_nmea() {
            Nmea nmea_message = new Nmea();
            nmea_message.Init("!AIVDM,1,1,,B,19NS7Sp02wo?HETKA2K6mUM20<L=,0*27\r\n");
            Assert.True(nmea_message.CheckChecksum() == 0, "Checksum is BAD");
        }

        [Fact]
        public void test_vdm() {
            Vdm vdm_message = new Vdm();

           var result = vdm_message.Add("!AIVDM,1,1,,B,19NS7Sp02wo?HETKA2K6mUM20<L=,0*27\r\n");
            Assert.Equal(0, (int)result);
            Assert.Equal(1,vdm_message.MsgId);
            Message1 msg = new Message1();
            msg.Parse(vdm_message.SixState);


            result = vdm_message.Add("!AIVDM,2,1,6,B,55ArUT02:nkG<I8GB20nuJ0p5HTu>0hT9860TV16000006420BDi@E53,0*33");
            Assert.Equal(1,(int)result);
            result = vdm_message.Add("!AIVDM,2,2,6,B,1KUDhH888888880,2*6A");
            Assert.Equal(0, (int)result);
            Assert.Equal(5,vdm_message.MsgId);
            var msg5 = new Message5();
            msg5.Parse(vdm_message.SixState);
        }

        private StreamReader CreateReader() {
            //挪威 开放AIS数据 https://kystverket.no/Maritime-tjenester/Meldings--og-informasjonstjenester/AIS/Brukartilgang-til-AIS-Norge/
            var client = new TcpClient();
            client.Connect("153.44.253.27", 5631);
            return new StreamReader(client.GetStream());
        }

        [Fact]
        public void TestParse() {
            var vdm = new Vdm();
            var msgs = new List<Messages>();
            using (var reader = CreateReader()) {
                for (int i = 0; i < 1000; i++) {
                    var line = reader.ReadLine();
                    var result = vdm.Add(line);
                    if (result == 0) {
                        // vdm.MsgId
                        var msg = vdm.ToMessage();
                        Assert.NotNull(msg);
                        msgs.Add(msg);
                        vdm = new Vdm();
                    }
                }
            }
            Assert.Equal(100,msgs.Count);
        }

    }
}