using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PLCCommunicator
{
    public partial class MainEngine : ViewModelBase
    {
        public ICommand WindowLoadedCommand
        {
            get { return (this.windowLoadedCommand) ?? (this.windowLoadedCommand = new DelegateCommand(WindowLoaded)); }
        }
        private ICommand windowLoadedCommand;

        public ICommand WriteManualCommand
        {
            get { return (this.writeManualCommand) ?? (this.writeManualCommand = new DelegateCommand(WriteManual)); }
        }
        private ICommand writeManualCommand;

        public RelayCommand<CancelEventArgs> WindowClosingCommand
        {
            get { return (this.windowClosingCommand) ?? (this.windowClosingCommand = new RelayCommand<CancelEventArgs>(WindowClosing)); }
        }
        private RelayCommand<CancelEventArgs> windowClosingCommand;

       
    }
}
