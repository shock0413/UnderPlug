using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraPylon
{
    public class StructCamera
    {
        public string SerialNum { get; set; }

        public int Gain { get; set; }
        public int Exposure { get; set; }

        public StructCamera(string serialNum, int gain, int exposure)
        {
            this.SerialNum = serialNum;
            this.Gain = gain;
            this.Exposure = exposure;
        }
    }
}
