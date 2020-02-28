using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace AisParser.Example {
    class Program {
        static void Main (string[] args) {
            // using (var reader = CreateReader()) {
            //     for(int i=0;i<100;i++){
            //         Console.WriteLine(reader.ReadLine());
            //     }
            // }
            StartParse (GetLines (CreateReader ()).Take(1000));
            //StartAnalysis (GetLines (CreateReader ()));
        }

        private static StreamReader CreateReader () {
            //挪威 开放AIS数据 https://kystverket.no/Maritime-tjenester/Meldings--og-informasjonstjenester/AIS/Brukartilgang-til-AIS-Norge/
            var client = new TcpClient ();
            //client.Connect("153.44.253.27", 5631);
            //client.Connect("localhost", 9050);
            client.Connect ("192.168.1.106", 8040);
            return new StreamReader (client.GetStream ());
        }

        private static IEnumerable<string> GetLines (TextReader reader, int count = int.MaxValue) {
            int i = 0;
            while (i < count) {
                var line = reader.ReadLine ();
                if (line == null) {
                    break;
                }
                yield return line;
                i++;
            }
        }

        private static void StartAnalysis (IEnumerable<string> lines) {
            var vdm = new Vdm ();
            var count = 0;
            var msgTypes = new int[128];
            var userIds = new List<long> ();
            foreach (var line in lines) {
                //Console.Write(".");
                var result = vdm.Add (line);
                switch (result) {
                    case VdmStatus.Complete:
                        //counts[vdm.MsgId]++;
                        count++;
                        try {
                            var msg = vdm.ToMessage ();
                            msgTypes[msg.MsgId]++;
                            if (!userIds.Contains (msg.UserId)) {
                                userIds.Add (msg.UserId);
                            }
                        } catch (Exception ex) {
                            Console.Error.WriteLine (ex.ToString ());
                        }
                        break;
                    case VdmStatus.Incomplete:
                        //Console.WriteLine (" {0} Num:{1} Sequence:{2}", vdm.Total, vdm.Num, vdm.Sequence);
                        break;
                    case VdmStatus.ChecksumFailed:
                    case VdmStatus.NotAisMessage:
                    case VdmStatus.NmeaNextError:
                    case VdmStatus.OutofSequence:
                    default:
                        Console.WriteLine (line);
                        Console.Error.WriteLine (result);
                        break;
                }
                if (count % 1000 == 0) {
                    Console.Write("count:{0}", count);
                    for (int i = 0; i < 25; i++) {
                        if (msgTypes[i] > 0) {
                            Console.Write ("\tMsg {0}:{1}", i, msgTypes[i]);
                        }
                    }
                    Console.WriteLine ();
                }
            }
        }

        public static void StartParse (IEnumerable<string> lines) {
            var counts = new int[30];
            var count = 0;
            var vdm = new Vdm ();
            foreach (var line in lines) {
                //Console.Write(".");
                var result = vdm.Add (line);
                switch (result) {
                    case VdmStatus.Complete:
                        //counts[vdm.MsgId]++;
                        count++;
                        try {
                            var msg = vdm.ToMessage ();
                            FormatMsg (msg);
                        } catch (Exception ex) {
                            Console.Error.WriteLine (ex.ToString ());
                        }

                        //vdm = new Vdm();
                        break;
                    case VdmStatus.Incomplete:
                        Console.WriteLine (" {0} Num:{1} Sequence:{2}", vdm.Total, vdm.Num, vdm.Sequence);
                        break;
                    case VdmStatus.ChecksumFailed:
                    case VdmStatus.NotAisMessage:
                    case VdmStatus.NmeaNextError:
                    case VdmStatus.OutofSequence:
                    default:
                        Console.WriteLine (line);
                        Console.Error.WriteLine (result);
                        break;
                }
                if (count > 100) {
                    Console.WriteLine ("Message Summary {0}", FormatCounts (counts));
                    count = 0;
                    counts = new int[30];
                }
            }
        }

        private static string FormatCounts (int[] counts) {
            var builder = new StringBuilder ();
            for (int i = 0; i < counts.Length; i++) {
                if (counts[i] > 0) {
                    builder.AppendFormat ("Message{0}:{1}\t", i, counts[i]);
                }
            }
            return builder.ToString ();
        }

        private static void FormatMsg (Messages msg) {
            //Console.WriteLine ("Messages {0} UserId:{1}", msg.MsgId, msg.UserId);
            if (msg is Message123 msg1) {
                Console.WriteLine (FormatMsg (msg1));
            } else if (msg is Message5 msg5) {
                //Console.WriteLine (FormatMsg (msg5));
            }
        }

        private static string FormatMsg (Message123 msg) {
            return msg.ToString(); //$"\tMMSI:{msg.UserId} NavStatus:{msg.NavStatus} Cog:{msg.Cog/100f} Pos:{msg.Pos} Reserved for regional {msg.Regional}";
        }

        private static string FormatMsg (Message5 msg) {
            return $"\tMMSI:{msg.UserId} Eta:{msg.Eta:X} {ParseEta(msg.Eta)} Dest:{msg.Dest} Name:{msg.Name}";
        }

        private static string ParseEta (long eta) {
            const string defaultEta = "-";
            if (eta == 0) {
                return defaultEta;
            }

            var min = (int) eta & 0x3F;
            var hour = (int) (eta >> 6) & 0x1F;
            var day = (int) (eta >> 11) & 0x1F;
            var month = (int) (eta >> 16) & 0x0F;

            if (month == 0 || day == 0 || hour == 0) {
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