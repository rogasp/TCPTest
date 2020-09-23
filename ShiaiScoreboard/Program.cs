using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Terminal.Gui;

namespace ShiaiScoreboard
{
    class Program
    {
        static private List<Label> txtServers;
        static private List<Label> txtStatus;
        static private List<Button> btnStart;
        static private List<Button> btnStop;
        static private List<Button> btnReStart;
        static private List<string> logg;

        private enum Status
        {
            NOT_RUNNING = 0,
            TRY_START,
            STARTING,
            RUNNING,
            TRY_RESTART,
            RESTARTING,
            TRY_STOP
        }

        public static Action running = ShiaiAppScoreboardApp;

        static void Main(string[] args)
        {
            txtServers = new List<Label>();
            txtStatus = new List<Label>();
            btnStart = new List<Button>();
            btnStop = new List<Button>();
            btnReStart = new List<Button>();
            logg = new List<string>();

            AddLogg("Starting application");

            AddServer("1", "0.0.0.0", 14, 1, 1, 15);
            AddServer("2", "0.0.0.0", 14, 1, 1, 15);
            AddServer("3", "0.0.0.0", 14, 1, 1, 15);
            AddServer("4", "0.0.0.0", 14, 1, 1, 15);
            AddServer("5", "0.0.0.0", 14, 1, 1, 15);
            AddServer("6", "0.0.0.0", 14, 1, 1, 15);

            AddStatus("1", "Not running", 14, 0, 1, 15);
            AddStatus("2", "Not running", 14, 0, 1, 15);
            AddStatus("3", "Not running", 14, 0, 1, 15);
            AddStatus("4", "Not running", 14, 0, 1, 15);
            AddStatus("5", "Not running", 14, 0, 1, 15);
            AddStatus("6", "Not running", 14, 0, 1, 15);

            AddStartButton("1");
            AddStartButton("2");
            AddStartButton("3");
            AddStartButton("4");
            AddStartButton("5");
            AddStartButton("6");

            AddStopButton("1");
            AddStopButton("2");
            AddStopButton("3");
            AddStopButton("4");
            AddStopButton("5");
            AddStopButton("6");

            AddReStartButton("1");
            AddReStartButton("2");
            AddReStartButton("3");
            AddReStartButton("4");
            AddReStartButton("5");
            AddReStartButton("6");

            while (running != null)
            {
                running.Invoke();
            }
            Application.Shutdown();
        }

        static private void SetStatus(int tatami, Status status)
        {
            int id = tatami - 1;
            switch (status)
            {
                case Status.NOT_RUNNING:
                    txtStatus[id].Text = "Not running";
                    txtStatus[id].ColorScheme = new ColorScheme()
                    {
                        Normal = new Terminal.Gui.Attribute(Color.Red, Color.Blue)
                    };
                    AddLogg($"Status for tatami {tatami} is set to Not running");
                    break;
                case Status.TRY_START:
                    txtStatus[id].Text = "Try to connect with tatami.";
                    txtStatus[id].ColorScheme = new ColorScheme()
                    {
                        Normal = new Terminal.Gui.Attribute(Color.White, Color.Blue)
                    };
                    AddLogg($"Status for tatami {tatami} is set to Try connecting");
                    break;
                case Status.STARTING:
                    txtStatus[id].Text = "Connecting with tatami";
                    txtStatus[id].ColorScheme = new ColorScheme()
                    {
                        Normal = new Terminal.Gui.Attribute(Color.White, Color.Blue)
                    };
                    AddLogg($"Status for tatami {tatami} is set to Connecting");
                    break;
                case Status.RUNNING:
                    txtStatus[id].Text = "Running";
                    txtStatus[id].ColorScheme = new ColorScheme()
                    {
                        Normal = new Terminal.Gui.Attribute(Color.Green, Color.Blue)
                    };
                    AddLogg($"Status for tatami {tatami} is set to Running");
                    break;
                case Status.TRY_RESTART:
                    txtStatus[id].Text = "Not running";
                    txtStatus[id].ColorScheme = new ColorScheme()
                    {
                        Normal = new Terminal.Gui.Attribute(Color.White, Color.Blue)
                    };
                    AddLogg($"Status for tatami {tatami} is SetStatus to Not running");
                    break;
                case Status.RESTARTING:
                    txtStatus[id].Text = "Not running";
                    txtStatus[id].ColorScheme = new ColorScheme()
                    {
                        Normal = new Terminal.Gui.Attribute(Color.White, Color.Blue)
                    };
                    AddLogg($"Status for tatami {tatami} is SetStatus to Not running");
                    break;
                case Status.TRY_STOP:
                    txtStatus[id].Text = "Try to stop";
                    txtStatus[id].ColorScheme = new ColorScheme()
                    {
                        Normal = new Terminal.Gui.Attribute(Color.White, Color.Blue)
                    };
                    AddLogg($"Status for tatami {tatami} is SetStatus to Try stopping");
                    break;
                default:
                    break;
            }
        }
        static private void AddLogg(string text)
        {
            logg.Insert(0, $"{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffff")} :: {text}");
        }
        static private void AddServer(string tatami, string text, int x, int y, int height, int width )
        {
            txtServers.Add(new Label(text)
            {
                Id = tatami,
                LayoutStyle = LayoutStyle.Computed,
                X = x,
                Y = y,
                Height = height,
                Width = width
            });

        }

        static private void AddStatus(string tatami, string text, int x, int y, int height, int width )
        {
            txtStatus.Add(new Label(text)
            {
                Id = tatami,
                LayoutStyle = LayoutStyle.Computed,
                X = x,
                Y = y,
                Height = height,
                Width = width,
                ColorScheme = new ColorScheme() { Normal = new Terminal.Gui.Attribute(Color.Red, Color.Blue) }
        });
        }

        static private void AddStartButton(string tatami)
        {
            int id = Convert.ToInt32(tatami);
            btnStart.Add(new Button(0, 2, "Start") 
            { 
                Clicked = () => { 
                AskIP(id);
                
            } });

        }

        static private void AddStopButton(string tatami)
        {
            int id = Convert.ToInt32(tatami);
            btnStop.Add(new Button(9, 2, "Stop")
            {
                CanFocus = false,
                Clicked = () => {
                    btnStart[id-1].CanFocus = true;
                    btnStop[id-1].CanFocus = false;
                    SetStatus(id, Status.TRY_STOP);

                }
            });

        }

        static private void AddReStartButton(string tatami)
        {
            int id = Convert.ToInt32(tatami);
            btnReStart.Add(new Button(17, 2, "Restart")
            {
                CanFocus = false,
                Clicked = () => {
                    

                }
            });

        }


        static void ShiaiAppScoreboardApp()
        {
            if (Debugger.IsAttached)
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");

            Application.Init();

            var top = Application.Top;

            int margin = 2;
            var win = new Window("Shiai Scoreboard")
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill() - margin,
                Height = Dim.Fill() - margin

            };

            ShowEntries(win);

            var statusBar = new StatusBar(new StatusItem[] {
                new StatusItem(Key.F1, "~F1~ Help", () => Help()),
                new StatusItem(Key.ControlQ, "~^Q~ Quit", () => { if (Quit ()) { running = null; top.Running = false; } })
            });

            top.LayoutComplete += (e) => {

            };

            top.Add(win);
            top.Add(statusBar);

            AddLogg("Application started");
            
            Application.Run(top);

            
        }

        static void Help()
        {
            MessageBox.Query(50, 7, "Help", "This is a small help\nBe kind.", "Ok");
        }

        static bool Quit()
        {
            var n = MessageBox.Query(50, 7, "Quit Shiai Scoreboard", "Are you sure you want to quit?", "Yes", "No");
            return n == 0;
        }

        static void ShowEntries(View container)
        {
            var tatami1StatusLbl = new Label(0, 0, "      Status: ");
            var tatami1ServerLbl = new Label(0, 1, "Connected to: ");
            var tatami2StatusLbl = new Label(0, 0, "      Status: ");
            var tatami2ServerLbl = new Label(0, 1, "Connected to: ");
            var tatami3StatusLbl = new Label(0, 0, "      Status: ");
            var tatami3ServerLbl = new Label(0, 1, "Connected to: ");
            var tatami4StatusLbl = new Label(0, 0, "      Status: ");
            var tatami4ServerLbl = new Label(0, 1, "Connected to: ");
            var tatami5StatusLbl = new Label(0, 0, "      Status: ");
            var tatami5ServerLbl = new Label(0, 1, "Connected to: ");
            var tatami6StatusLbl = new Label(0, 0, "      Status: ");
            var tatami6ServerLbl = new Label(0, 1, "Connected to: ");
            
            var loggView = new ScrollView(new Rect(0, 0, 90, 13))
            {
                ContentSize = new Size(90, 13),
                ShowVerticalScrollIndicator = true,
                ShowHorizontalScrollIndicator = true
            };

            loggView.Add(new ListView(new Rect(0, 0, 90, 13), logg));

            container.Add(
                new FrameView(new Rect(1, 0, 30, 5), "Tatami 1") { tatami1StatusLbl,
                    tatami1ServerLbl,
                    txtStatus[0],
                    txtServers[0],
                    btnStart[0],
                    btnStop[0],
                    btnReStart[0]},
                new FrameView(new Rect(31, 0, 30, 5), "Tatami 2") { tatami2StatusLbl,
                    tatami2ServerLbl,
                    txtStatus[1],
                    txtServers[1],
                    btnStart[1],
                    btnStop[1],
                    btnReStart[1]},
                new FrameView(new Rect(61, 0, 30, 5), "Tatami 3") { tatami3StatusLbl,
                    tatami3ServerLbl,
                    txtStatus[2],
                    txtServers[2],
                    btnStart[2],
                    btnStop[2],
                    btnReStart[2]},
                new FrameView(new Rect(1, 5, 30, 5), "Tatami 4") { tatami4StatusLbl,
                    tatami4ServerLbl,
                    txtStatus[3],
                    txtServers[3],
                    btnStart[3],
                    btnStop[3],
                    btnReStart[3]},
                new FrameView(new Rect(31, 5, 30, 5), "Tatami 5") { tatami5StatusLbl,
                    tatami5ServerLbl,
                    txtStatus[4],
                    txtServers[4],
                    btnStart[4],
                    btnStop[4],
                    btnReStart[4]},
                new FrameView(new Rect(61, 5, 30, 5), "Tatami 6") { tatami6StatusLbl,
                    tatami6ServerLbl,
                    txtStatus[5],
                    txtServers[5],
                    btnStart[5],
                    btnStop[5],
                    btnReStart[5]},
                new FrameView(new Rect(1, 10, 90, 15), "Logging") { loggView },
                new FrameView(new Rect(91, 0, 20, 25), "Queu")

                );
        }

        static string AskIP (int tatami)
        {
            var dialog = new Dialog("IP address", 30, 5);
            var t = new TextField(0, 0, 15, txtServers[tatami-1].Text);
            var oldIp = t.Text;
            dialog.Add(t);
            dialog.AddButton(new Button("OK", true) { Clicked = () => {
                txtServers[tatami-1].Text = t.Text;
                btnStart[tatami - 1].CanFocus = false;
                btnStop[tatami - 1].CanFocus = true;
                AddLogg($"Changed IP from {oldIp} to {t.Text} on tatami {tatami}");
                SetStatus(tatami, Status.TRY_START);
                dialog.Running = false; } });
            dialog.AddButton(new Button("CANCEL", false) { Clicked = () => { 
                dialog.Running = false; } });
            Application.Run(dialog);
           
            return "";
        }
    }
}
