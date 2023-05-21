using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inspection
{
    /// <summary>
    /// DisplayPanel.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DisplayPanel : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public DisplayPanel()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        public SolidColorBrush Point1Brush { get { return point1Brush; } set { point1Brush = value; NotifyPropertyChanged("Point1Brush"); } }
        public SolidColorBrush Point2Brush { get { return point2Brush; } set { point2Brush = value; NotifyPropertyChanged("Point2Brush"); } }
        public SolidColorBrush Point3Brush { get { return point3Brush; } set { point3Brush = value; NotifyPropertyChanged("Point3Brush"); } }
        public SolidColorBrush Point4Brush { get { return point4Brush; } set { point4Brush = value; NotifyPropertyChanged("Point4Brush"); } }
        public SolidColorBrush Point5Brush { get { return point5Brush; } set { point5Brush = value; NotifyPropertyChanged("Point5Brush"); } }
        public SolidColorBrush Point6Brush { get { return point6Brush; } set { point6Brush = value; NotifyPropertyChanged("Point6Brush"); } }

        private SolidColorBrush point1Brush = Brushes.White;
        private SolidColorBrush point2Brush = Brushes.White;
        private SolidColorBrush point3Brush = Brushes.White;
        private SolidColorBrush point4Brush = Brushes.White;
        private SolidColorBrush point5Brush = Brushes.White;
        private SolidColorBrush point6Brush = Brushes.White;

        public void SetColor(int position, SolidColorBrush color)
        {
            switch (position)
            {
                case 0:
                    Point1Brush = color;
                    break;
                case 1:
                    Point2Brush = color;
                    break;
                case 2:
                    Point3Brush = color;
                    break;
                case 3:
                    Point4Brush = color;
                    break;
                case 4:
                    Point5Brush = color;
                    break;
                case 5:
                    Point6Brush = color;
                    break;
            }
        }
    }
}
