using System;
using System.IO;
using System.Net.Sockets;

namespace AisParser.Example {
    class Program {
        static void Main(string[] args) {
            StartParse(100000);
        }

        private static StreamReader CreateReader() {
            //挪威 开放AIS数据 https://kystverket.no/Maritime-tjenester/Meldings--og-informasjonstjenester/AIS/Brukartilgang-til-AIS-Norge/
            var client = new TcpClient();
            client.Connect("153.44.253.27", 5631);
            return new StreamReader(client.GetStream());
        }

        public static void StartParse(int count) {
            var vdm = new Vdm();
            using (var reader = CreateReader()) {
                for (int i = 0; i < count; i++) {
                    Console.Write(".");
                    var line = reader.ReadLine();
                    var result = vdm.Add(line);
                    switch (result) {
                        case VdmStatus.Complete:
                            try {
                                var msg = vdm.ToMessage();
                                FormatMsg(msg);
                            } catch (Exception ex) {
                                Console.Error.WriteLine(ex.ToString());
                            }

                            vdm = new Vdm();
                            break;
                        case VdmStatus.Incomplete:
                            Console.Write(".");
                            break;
                        case VdmStatus.ChecksumFailed:
                        case VdmStatus.NotAisMessage:
                        case VdmStatus.NmeaNextError:
                        case VdmStatus.OutofSequence:
                        default:
                            Console.WriteLine(line);
                            Console.Error.WriteLine(result);
                            break;
                    }
                }
            }
        }

        private static void FormatMsg(Messages msg) {
            Console.WriteLine("Message {0} MMSI:{1}", msg.MsgId, msg.UserId);
            if(msg is Message1 msg1){
                Console.WriteLine(FormatMsg(msg1));
            }
        }

        private static  string FormatMsg(Message1 msg) {
            return $"\tMMSI:{msg.UserId} NavStatus:{msg.NavStatus} Cog:{msg.Cog/100f} Pos:{msg.Pos}";
        }

    }
}
