using DBManager;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Report
{
    public class MainEngine : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        Window window;
        DBManager.DBManager dbManager = new DBManager.DBManager();

        #region Properties

        public DateTime StartDateTime { get { return startDateTime; } set { startDateTime = value; NotifyPropertyChanged("StartDateTime"); } }
        private DateTime startDateTime = DateTime.Now;

        public DateTime EndDateTime { get { return endDateTime; } set { endDateTime = value; NotifyPropertyChanged("EndDateTime"); } }
        private DateTime endDateTime = DateTime.Now;

        public ObservableCollection<StructResult> GridItemSource { get { return gridItemSource; } set { gridItemSource = value; NotifyPropertyChanged("GridItemSource"); } }
        private ObservableCollection<StructResult> gridItemSource = new ObservableCollection<StructResult>();


        private int totalCar;
        private int ngCar;

        public int TotalCar { get { return totalCar; } set { totalCar = value; NotifyPropertyChanged("TotalCar"); NotifyPropertyChanged("NGCar"); NotifyPropertyChanged("OKCar"); NotifyPropertyChanged("PerCar"); } }
        public int NGCar { get { return ngCar; } set { ngCar = value; NotifyPropertyChanged("TotalCar"); NotifyPropertyChanged("NGCar"); NotifyPropertyChanged("OKCar"); NotifyPropertyChanged("PerCar"); } }

        public int OKCar { get { return totalCar - ngCar; } }
        public double PerCar
        {
            get
            {
                try
                {
                    return Math.Round(OKCar * 1.0 / totalCar * 100, 2);
                }
                catch
                {
                    return 100;
                }
            }
        }

        #endregion

        #region Command

        private ICommand searchCommand;
        public ICommand SearchCommand
        {
            get { return (this.searchCommand) ?? (this.searchCommand = new DelegateCommand(Search)); }
        }


        private ICommand exportCommand;
        public ICommand ExportCommand
        {
            get { return (this.exportCommand) ?? (this.exportCommand = new DelegateCommand(Export)); }
        }

        #endregion

        public MainEngine(Window window)
        {
            this.window = window;
        }

        public void Search()
        {
            GridItemSource = new ObservableCollection<StructResult>(dbManager.GetResult(startDateTime, endDateTime));
            TotalCar = dbManager.GetCarTotalCount(startDateTime, endDateTime);

        }

        public void Export()
        {
           
        }
    }
}
