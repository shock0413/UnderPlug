using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCCommunicator
{
    public class StructPLCAddress : ViewModelBase
    {
        public AddressType AddressType { get; set; }
        public string Device { get; set; }

        public int DeviceNumber { get; set; }
        public string Name { get; set; }
        public int Data { get { return data; } set { data = value; NotifyPropertyChanged("Data"); } }
        private int data;
    }
}
