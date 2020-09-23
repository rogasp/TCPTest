using System;
using System.Collections.Generic;
using System.Text;

namespace TCPTest
{
    class ScreeMessage
    {
        public enum Align
        {
            Left = 0,
            Center = 1,
            Right = 2
        }

        public enum WAlign
        {
            Top = 0,
            Center = 1,
            Bottom = 2
        }

        public string message { get; set; }
        public int xPos { get; set; }
        public int yPos { get; set; }
        public int maxLength { get; set; }

        public Align align { get; set; }

        public WAlign wAlign { get; set; }

        public ScreeMessage()
        {

        }

    }
}
