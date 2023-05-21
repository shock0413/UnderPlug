using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CameraPylon
{
    public class StructNetworkInterface
    {
        public NetworkInterface NetworkInterface { get { return networkInterface; } }
        public NetworkInterface networkInterface;

        public List<StructCameraInterface> ListCameraInterface { get { return listCameraInterface; } set { listCameraInterface = value; } }
        private List<StructCameraInterface> listCameraInterface = new List<StructCameraInterface>();

        public string Name { get { return networkInterface.Name; } }
        public string IpAddress
        {
            get
            {
                UnicastIPAddressInformation[] unicastIPAddressInformation = networkInterface.GetIPProperties().UnicastAddresses.Where(x => x.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToArray();
                if (unicastIPAddressInformation != null && unicastIPAddressInformation.Length > 0)
                {
                    return unicastIPAddressInformation[0].Address.ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        public string Device
        {
            get { return NetworkInterface.Description; }
        }

        public string Status
        {
            get { return NetworkInterface.OperationalStatus.ToString(); }
        }

        public string Speed
        {
            get { return NetworkInterface.Speed.ToString(); }
        }

        public string MACAddress
        {
            get { return networkInterface.GetPhysicalAddress().ToString(); }
        }

        public string DHCPStatus
        {
            get
            {
                if (networkInterface.GetIPProperties().GetIPv4Properties() != null)
                {
                    return networkInterface.GetIPProperties().GetIPv4Properties().IsDhcpEnabled.ToString();
                }
                else
                {
                    return "Error";
                }

            }
        }

        public string SubnetMask
        {
            get
            {
                UnicastIPAddressInformation[] unicastIPAddressInformation = networkInterface.GetIPProperties().UnicastAddresses.Where(x => x.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToArray();
                if (unicastIPAddressInformation != null && unicastIPAddressInformation.Length > 0)
                {
                    return unicastIPAddressInformation[0].IPv4Mask.ToString();
                }
                else
                {
                    return "";
                }
            }
        }


        public string MTU
        {
            get
            {
                return networkInterface.GetIPProperties().GetIPv4Properties().Mtu.ToString();
            }
        }

        public StructNetworkInterface(NetworkInterface networkInterface)
        {
            this.networkInterface = networkInterface;
        }

        public override string ToString()
        {
            return networkInterface.Name;
        }


    }
}
