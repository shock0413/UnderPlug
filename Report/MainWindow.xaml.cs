using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Report
{


    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        Dictionary<string, string> headerNames = new Dictionary<string, string>();

        public MainWindow()
        {
            this.DataContext = new MainEngine(this);

            headerNames.Add("model", "검사모델");
            headerNames.Add("dateTime", "검사시간");
            headerNames.Add("position", "검사위치");
            headerNames.Add("reinspectionCount", "재검사횟수");
            headerNames.Add("spreadX", "산포값 X");
            headerNames.Add("spreadY", "산포값 Y");
            headerNames.Add("visionMoveX", "비전 이동값 X");
            headerNames.Add("visionMoveY", "비전 이동값 Y");
            headerNames.Add("visionResult", "비전 결과");
            headerNames.Add("isAlreadyInserted", "이미장착됨");
            headerNames.Add("insertResult", "장착결과");
            headerNames.Add("pullForceError", "당김힘체크결과");

            InitializeComponent();
        }

        private void DataGrid_AutoGeneratingColumn_1(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }

    }
}
