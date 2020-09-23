using System;
using System.Collections.Generic;
using System.Text;

namespace TCPTest.Messages
{
    class UpdateLabel
    {
        public string Expose { get; set; }
        public string Text { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }

        public string CatA { get; set; }
        public string Comp1A { get; set; }
        public string Comp2A { get; set; }
        public string CatB { get; set; }
        public string Comp1B { get; set; }
        public string Comp2B { get; set; }

        public int Pts1 { get; set; }
        public int Pts2 { get; set; }

        public int I1 { get; set; }
        public int I2 { get; set; }
        public int I3 { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public double W { get; set; }
        public double H { get; set; }
        public double FgR { get; set; }
        public double FgG { get; set; }
        public double FgB { get; set; }

        public double BgR { get; set; }
        public double BgG { get; set; }
        public double BgB { get; set; }
        public double Size { get; set; }
        /* special label nums */
        public int LabelNum { get; set; }
        public int Xalign { get; set; }
        public int Round { get; set; }
    }
}
