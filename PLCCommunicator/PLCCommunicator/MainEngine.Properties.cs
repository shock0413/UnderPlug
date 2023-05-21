using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using utill;

namespace PLCCommunicator
{
    public partial class MainEngine : ViewModelBase
    {
        public enum ConnectionState { Connected, UnConnected};

        public WindowState WindowState { get { return windowState; }set { windowState = value; NotifyPropertyChanged("WindowState"); } }
        private WindowState windowState = WindowState.Normal;

        public ObservableCollection<string> CommunicateMessageList { get { return communicateMessageList; } set { communicateMessageList = value; NotifyPropertyChanged("CommunicateMessageList"); } }
        private ObservableCollection<string> communicateMessageList = new ObservableCollection<string>();

        public ObservableCollection<StructPLCAddress> InputAddressList { get { return inputAddressList; } set { inputAddressList = value; NotifyPropertyChanged("InputAddressList"); } }
        private ObservableCollection<StructPLCAddress> inputAddressList = new ObservableCollection<StructPLCAddress>();

        public ObservableCollection<StructPLCAddress> OutputAddressList { get { return outputAddressList; } set { outputAddressList = value; NotifyPropertyChanged("OutputAddressList"); } }
        public ObservableCollection<StructPLCAddress> outputAddressList = new ObservableCollection<StructPLCAddress>();

        public StructPLCAddress SelectedInputAddressDataGridIndex { get{ return selectedInputAddressDataGridIndex; } set { selectedInputAddressDataGridIndex = value; NotifyPropertyChanged("SelectedInputAddressDataGridIndex"); MenualWriteDevice = value.Device; } }
        private StructPLCAddress selectedInputAddressDataGridIndex;

        public StructPLCAddress SelectedOutputAddressDataGridIndex { get { return selectedOutputAddressDataGridIndex; } set { selectedOutputAddressDataGridIndex = value; NotifyPropertyChanged("SelectedOutputAddressDataGridIndex"); MenualWriteDevice = value.Device; } }
        private StructPLCAddress selectedOutputAddressDataGridIndex;

        public int CommunicateSelectedIndex { get { return communicateSelectedIndex; } set { communicateSelectedIndex = value; } }
        private int communicateSelectedIndex = 0;

        public SolidColorBrush PlcStateColor
        {
            get { return plcStateColor; }
            set { plcStateColor = value; NotifyPropertyChanged("PlcStateColor"); }
        }
        private SolidColorBrush plcStateColor = Brushes.Red;

        public SolidColorBrush SocketStateColor
        {
            get { return socketStateColor; }
            set { socketStateColor = value; NotifyPropertyChanged("SocketStateColor"); }
        }
        private SolidColorBrush socketStateColor = Brushes.Red;

        public ConnectionState PLCState { 
            set
            {
                if(value == ConnectionState.Connected)
                {
                    PlcStateColor = Brushes.Green;
                }
                else
                {
                    PlcStateColor = Brushes.Red;
                }
            }
        }

        public ConnectionState SocketState
        {
            set
            {
                if (value == ConnectionState.Connected)
                {
                    SocketStateColor = Brushes.Green;
                }
                else
                {
                    SocketStateColor = Brushes.Red;
                }
            }
        }

        public string MenualWriteDevice { get { return manualWriteDevice; } set { manualWriteDevice = value; NotifyPropertyChanged("MenualWriteDevice"); } }
        private string manualWriteDevice;

        public int MenualWriteValue { get { return manualWriteValue; } set { manualWriteValue = value; NotifyPropertyChanged("MenualWriteValue"); } }
        private int manualWriteValue;
    }
}
