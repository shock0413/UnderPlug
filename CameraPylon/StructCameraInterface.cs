using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PylonC.NETSupportLibrary.DeviceEnumerator;

namespace CameraPylon
{
    public class StructCameraInterface
    {
        Device device;
        StructNetworkInterface networkInterface;

        public string Name { get { return device.ModelName; } }

        public string Vendor { get { return device.Vendor; } }

        public string SerialNum { get { return device.SerialNum; } }

        public string Mac { get { return device.MacAddress; } }

        public string HostIP { get { return device.Interface; } }

        public string HostSubnetMask { get { return networkInterface.SubnetMask; } }

        public string IpAddress { get { return device.IpAddress; } }

        public string SubnetMask { get { return device.SubnetMask; } }

        public string SavedIpAddress { get { return savedIpAddress; } set { savedIpAddress = value; } }
        public string savedIpAddress;

        public string SavedSubnetMask { get { return savedSubnetMask; } set { savedSubnetMask = value; } }
        public string savedSubnetMask;

        public StructCameraInterface(Device device, StructNetworkInterface networkInterface)
        {
            this.device = device;
            this.networkInterface = networkInterface;
        }

        public override string ToString()
        {
            return device.ModelName;
        }
    }
}
