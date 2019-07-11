using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

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

        public static void StartParse(int length) {
            var counts = new int[30];
            var count = 0;
            var vdm = new Vdm();
            using (var reader = CreateReader()) {
                for (int i = 0; i < length; i++) {
                    Console.Write(".");
                    var line = reader.ReadLine();
                    var result = vdm.Add(line);
                    switch (result) {
                        case VdmStatus.Complete:
                            counts[vdm.MsgId]++;
                            count++;
                            try {
                                var msg = vdm.ToMessage();
                                FormatMsg(vdm, msg);
                            } catch (Exception ex) {
                                Console.Error.WriteLine(ex.ToString());
                            }

                            vdm = new Vdm();
                            break;
                        case VdmStatus.Incomplete:
                            Console.WriteLine(" {0} Num:{1} Sequence:{2}",vdm.Total,vdm.Num,vdm.Sequence);
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
                    if(count>100){
                        Console.WriteLine("Message Summary {0}",FormatCounts(counts));
                        count = 0;
                        counts = new int[30];
                    }
                }
            }
        }

        private static string FormatCounts(int[] counts){
            var builder = new StringBuilder();
            for(int i=0;i<counts.Length;i++){
                if(counts[i]>0){
                    builder.AppendFormat("Message{0}:{1}\t",i,counts[i]);
                }
            }
            return builder.ToString();
        }

        private static void FormatMsg(Vdm vdm,Messages msg) {
            Console.WriteLine("Message_{0} MMSI:{1}", msg.MsgId, msg.UserId);
            if(msg is Message1 msg1){
                Console.WriteLine(FormatMsg(msg1));
            }else if(msg is Message5 msg5){
                Console.WriteLine(FormatMsg(msg5));
            }
        }

        private static  string FormatMsg(Message1 msg) {
            return $"\tMMSI:{msg.UserId} NavStatus:{msg.NavStatus} Cog:{msg.Cog/100f} Pos:{msg.Pos}";
        }

         private static  string FormatMsg(Message5 msg) {
            return $"\tMMSI:{msg.UserId} Eta:{msg.Eta:X} {ParseEta(msg.Eta)} Dest:{msg.Dest} Name:{msg.Name}";
        }

        private static string ParseEta(long eta) {
            const string defaultEta = "-";
            if (eta == 0) {
                return defaultEta;
            }

            var min = (int)eta & 0x3F;
            var hour = (int)(eta >> 6) & 0x1F;
            var day = (int)(eta >> 11) & 0x1F;
            var month = (int)(eta >> 16) & 0x0F;
            
            if (month ==0 || day ==0 || hour==0 ) {
                return defaultEta;
            }

            return $"{month:00}-{day:00} {hour:00}:{min:00}";
            // if (min < 0 || min > 59) {
            //     return defaultEta;
            // }

            // if (hour < 0 | hour > 24) {
            //     return defaultEta;
            // }

            // if (day < 1 || day > 31) {
            //     return defaultEta;
            // }

            // if (month < 1 || month > 12) {
            //     return defaultEta;
            // }

            //var year = DateTime.Today.Year;

            //var date= new DateTime(year, month, day, hour, min, 0);
            //return date.ToString("MM-dd hh:mm");
            //return $"{month:00}-{day} {hour}:{min}";
        }

    }
}
