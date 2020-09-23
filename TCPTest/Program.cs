using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace TCPTest
{
    class Program
    {
        const int MF_BYCOMMAND = 0x00000000;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;
        const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        static void Main(string[] args)
        {
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);

            string[] rowMsg = new string[] { @"╔═════Tatami:  ══════════╗═════Tatami:  ══════════╗═════Tatami:  ══════════╗",
                                             @"║     Status: Closed     ║     Status: Closed     ║     Status: Closed     ║",
                                             @"║ Last label:            ║ Last label:            ║ Last label:            ║",
                                             @"║                        ║                        ║                        ║",
                                             @"║                        ║                        ║                        ║",
                                             @"║                        ║                        ║                        ║",
                                             @"║                        ║                        ║                        ║",
                                             @"║                        ║                        ║                        ║",
                                             @"╠═════Tatami:  ══════════╣═════Tatami:  ══════════╣═════Tatami:  ══════════╣",
                                             @"║     Status: Closed     ║     Status: Closed     ║     Status:            ║",
                                             @"║ Last label:            ║ Last label:            ║ Last label:            ║",
                                             @"║                        ║                        ║                        ║",
                                             @"║                        ║                        ║                        ║",
                                             @"║                        ║                        ║                        ║",
                                             @"║                        ║                        ║                        ║",
                                             @"║                        ║                        ║                        ║",
                                             @"╠═════Logging════════════╩════════════════════════╩════════════════════════╣",
                                             @"║                                                                          ║",
                                             @"║                                                                          ║",
                                             @"║                                                                          ║",
                                             @"║                                                                          ║",
                                             @"║                                                                          ║",
                                             @"║                                                                          ║",
                                             @"║                                                                          ║",
                                             @"║                                                                          ║",
                                             @"║                                                                          ║",
                                             @"║                                                                          ║",
                                             @"║                                                                          ║",
                                             @"║                                                                          ║",
                                             @"╠═════Queu═════════════════════════════════════════════════════════════════╣",
                                             @"║ Messages in queue: 0000                                                  ║",
                                             @"║                                                                          ║",
                                             @"║                                                                          ║",
                                             @"║                                                                          ║",
                                             @"╚══════════════════════════════════════════════════════════════════════════╝"
            };

           
            string address = "127.0.0.1";
            int port = 2312;

            bool running = true;

            Console.WindowHeight = 40;
            Console.WindowWidth = 135;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;

            Console.Clear();
            ConcurrentQueue<Message> queue = new ConcurrentQueue<Message>();
            ConcurrentQueue<ScreeMessage> screeMessages = new ConcurrentQueue<ScreeMessage>();

            for (int i = 0; i < 35; i++)
            {
                var row = rowMsg[i];
                screeMessages.Enqueue(new ScreeMessage
                {
                    xPos = 0,
                    yPos = i,
                    message = row,
                    align = ScreeMessage.Align.Left,
                    wAlign = ScreeMessage.WAlign.Top,
                    maxLength = 26
                });

            }
            var client = new TimerClient(address, port, queue, screeMessages);
            
            var text = "Client connecting...";
            screeMessages.Enqueue(new ScreeMessage
            {
                xPos = 1,
                yPos = 5,
                message = text,
                align = ScreeMessage.Align.Left,
                wAlign = ScreeMessage.WAlign.Bottom
            });

            client.ConnectAsync();
            Console.WriteLine("Done!");
            screeMessages.Enqueue(new ScreeMessage
            {
                xPos = 1,
                yPos = 5,
                message = text,
                align = ScreeMessage.Align.Left,
                wAlign = ScreeMessage.WAlign.Bottom
            });

            text = "Press Enter to stop the client or '!' to reconnect the client...";
            screeMessages.Enqueue(new ScreeMessage
            {
                xPos = 1,
                yPos = 3,
                message = text,
                align = ScreeMessage.Align.Left,
                wAlign = ScreeMessage.WAlign.Bottom
            });

            Task t2 = Task.Factory.StartNew(() =>
            {
                while (running)
                {
                    Thread.Sleep(100);

                    var text = $"{queue.Count.ToString().PadLeft(4, '0')}";
                    screeMessages.Enqueue(new ScreeMessage {
                        xPos = 21,
                        yPos = 30,
                        message = text,
                        align = ScreeMessage.Align.Left,
                        wAlign = ScreeMessage.WAlign.Top,
                        maxLength = 0
                    });
                    Message message;
                    if (queue.TryDequeue(out message))
                    {
                        
                        var tatami = message.Sender;
                        screeMessages.Enqueue(new ScreeMessage
                        {
                            xPos = 14,
                            yPos = 0,
                            message = tatami.ToString(),
                            align = ScreeMessage.Align.Left,
                            wAlign = ScreeMessage.WAlign.Top,
                            maxLength = 1
                        });

                        
                        var msgLabel = new ScreeMessage
                        {
                            xPos = 14,
                            yPos = 2,
                            message = message.MsgUpdateLabel.LabelNum.ToString(),
                            align = ScreeMessage.Align.Left,
                            wAlign = ScreeMessage.WAlign.Top,
                            maxLength = 3
                            
                        };
                        switch (message.MsgUpdateLabel.LabelNum)
                        {
                            case (int)Message.Label.START_ADVERTISEMENT:
                                
                                break;
                            case (int)Message.Label.START_BIG:
                                break;
                            case (int)Message.Label.STOP_BIG:
                                break;
                            default:
                                break;
                        }

                        screeMessages.Enqueue(msgLabel);

                    }

                }
                
            });
            Task t3 = Task.Factory.StartNew(() =>
            {
                while (running)
                {
                    ScreeMessage screeMessage;
                    if (screeMessages.TryDequeue(out screeMessage))
                    {
                        var w = Console.WindowWidth;
                        var h = Console.WindowHeight;
                        var x = 0;
                        var y = 0;

                        switch (screeMessage.align)
                        {
                            case ScreeMessage.Align.Left:
                                x = screeMessage.xPos<w ? screeMessage.xPos : w;
                                break;
                            case ScreeMessage.Align.Center:
                                break;
                            case ScreeMessage.Align.Right:
                                x = w - screeMessage.xPos;
                                break;
                            default:
                                break;
                        }

                        switch (screeMessage.wAlign)
                        {
                            case ScreeMessage.WAlign.Top:
                                y = screeMessage.yPos < h ? screeMessage.yPos : h;
                                break;
                            case ScreeMessage.WAlign.Center:
                                break;
                            case ScreeMessage.WAlign.Bottom:
                                y = h - screeMessage.yPos;
                                break;
                            default:
                                break;
                        }
                        Console.SetCursorPosition(x, y);
                        Console.Write(new string(' ', screeMessage.maxLength));
                        Console.SetCursorPosition(x, y);
                        Console.Write(screeMessage.message);
                    }

                }

            });

            // Perform text input
            for (; ; )
            {
                string line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                    break;

                // Disconnect the client
                if (line == "!")
                {
                    Console.Write("Client disconnecting...");
                    client.DisconnectAsync();
                    Console.WriteLine("Done!");
                    Console.Write("Client connecting...");
                    client.ConnectAsync();
                    Console.WriteLine("Done!");
                    continue;
                }

                // Send the entered text to the chat server
                client.SendAsync(line);
            }

            // Disconnect the client
            Console.Write("Client disconnecting...");
            client.DisconnectAndStop();
            running = false;
            try
            {
                Task.WaitAll(t2,t3);
            }
            catch (AggregateException ex) // No exception
            {
                Console.WriteLine(ex.Flatten().Message);
            }
            Console.WriteLine("Done!");

        }
    }
}
