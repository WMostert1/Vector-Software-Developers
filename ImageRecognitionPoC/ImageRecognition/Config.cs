using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageRecognition
{
    public class Config
    {
        public int[] layers { get; set; }
        public float dw { get; set; }
        public float moment { get; set; }

        public float alpha { get; set; }
        public float beta { get; set; }

        public float accuracy { get; set; }

        public String toString()
        {
            return "Layers: " + layers[1] + "\n" +
                "DW: " + dw + "\n" +
                "Moment: " + moment + "\n" +
                "A: " + alpha + "\n" +
                "B: " + beta + "\n" +
                "Acc: " + accuracy;
        }
    }

}
