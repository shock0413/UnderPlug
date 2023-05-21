using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Cognex.VisionPro;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.PMAlign;
using Hansero.VisionLib.VisionPro;
using Utilities;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Threading;

namespace Setting
{
    public partial class Setting_Form : Form
    {
        //AI 검사
        // ClassificationManager classificationManager = new ClassificationManager();

        private AsyncDetectSocket.AsyncSocketServer detectServer = null;
        private AsyncDetectSocket.AsyncSocketClient detectClient = null;
        private int detectClientId = 0;
        private int sendImageIndex = 0;

        // 패턴검사 툴
        // 메인 패턴
        private PMAlgin m_PMAlgin = new PMAlgin();

        // 검사정보(INI 파일)
        private IniFile m_Config = new IniFile(Application.StartupPath + "\\Config.ini");
        // AI 검사정보(INI 파일)
        private IniFile m_InspectionDetectConfig = new IniFile(Environment.CurrentDirectory + "\\InspectionDetector\\Config.ini");
        private IniFile m_SettingDetectConfig = new IniFile(Environment.CurrentDirectory + "\\SettingDetector\\Config.ini");

        private enum CURSTATE { ADD_PATTERN, MODIFY_RANGE, MODIFY_CIRCLE }

        CURSTATE m_CurState = CURSTATE.MODIFY_RANGE;
        private string m_CheckPosition;

        public Setting_Form()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init_System();
            InitAI();

            Thread detectProcessCheckThread = new Thread(DetectProcessCheckThreadDo);
            detectProcessCheckThread.Start();
        }

        private bool detectProcessCheckThreadRunning = true;

        private void DetectProcessCheckThreadDo()
        {
            while (detectProcessCheckThreadRunning)
            {
                try
                {
                    bool isChecked = false;

                    if (m_Process == null)
                    {
                        isChecked = true;
                    }
                    else
                    {
                        if (m_Process.HasExited)
                        {
                            isChecked = true;
                        }
                    }

                    if (isChecked)
                    {
                        if (m_Process != null)
                        {
                            if (!m_Process.HasExited)
                            {
                                m_Process.Kill();
                            }

                            m_Process.Close();
                            m_Process.Dispose();
                            m_Process = null;
                        }

                        InitAI();
                    }
                }
                catch
                {

                }

                Thread.Sleep(100);
            }
        }

        private void OnConnect(object sender, AsyncDetectSocket.AsyncSocketConnectionEventArgs e)
        {

        }

        private void OnReceive(object sender, AsyncDetectSocket.AsyncSocketReceiveEventArgs e)
        {
            byte[] receiveData = e.ReceiveData;

            byte[] bitConvert = new byte[4] { receiveData[0], receiveData[1], receiveData[2], receiveData[3] };

            int len = BitConverter.ToInt32(bitConvert, 0);

            Console.WriteLine("받은 실제 데이터 길이 : " + len);

            byte[] data = new byte[len];

            Buffer.BlockCopy(receiveData, 4, data, 0, len);

            string recvStr = Encoding.Default.GetString(data);

            Console.WriteLine("OnReceive : " + recvStr);

            if (recvStr.StartsWith("Echo,"))
            {

            }
            else
            {
                m_Process_Recv.Add(recvStr);
            }
        }

        private int netWidth = 416;
        private int netHeight = 416;
        private int netChannels = 3;

        private void OnAccept(object sender, AsyncDetectSocket.AsyncSocketAcceptEventArgs e)
        {
            if (detectClient != null)
            {
                detectClient.Close();
                detectClient = null;
            }

            detectClient = new AsyncDetectSocket.AsyncSocketClient(detectClientId++, e.Worker);
            detectClient.OnConnet += new AsyncDetectSocket.AsyncSocketConnectEventHandler(OnConnect);
            detectClient.OnReceive += new AsyncDetectSocket.AsyncSocketReceiveEventHandler(OnReceive);
            detectClient.OnError += new AsyncDetectSocket.AsyncSocketErrorEventHandler(OnError);
            detectClient.OnSend += new AsyncDetectSocket.AsyncSocketSendEventHandler(OnSend);
            detectClient.Receive();

            string m_CfgPath = m_Config.GetString("AI", "CfgPath", "");
            string m_WeightsPath = m_Config.GetString("AI", "WeightsPath", "");
            string m_NamesPath = m_Config.GetString("AI", "NamesPath", "");

            IniFile cfgFile = new IniFile(m_CfgPath);

            netWidth = cfgFile.GetInt32("net", "width", 416);
            netHeight = cfgFile.GetInt32("net", "height", 416);
            netChannels = cfgFile.GetInt32("net", "channels", 3);

            string sendStr = "PARAMS," + m_CfgPath + "," + m_WeightsPath + "," + m_NamesPath + "," + Convert.ToString(netWidth) + "," + Convert.ToString(netHeight) + "," + Convert.ToString(netChannels);

            byte[] buf = Encoding.Default.GetBytes(sendStr);
            int len = buf.Length + 4;
            byte[] sendBytes = new byte[len];

            Buffer.BlockCopy(BitConverter.GetBytes(len - 4), 0, sendBytes, 0, 4);
            Buffer.BlockCopy(buf, 0, sendBytes, 4, buf.Length);

            detectClient.Send(sendBytes);

            Console.WriteLine("OnAccept");
        }

        private void OnError(object sender, AsyncDetectSocket.AsyncSocketErrorEventArgs e)
        {
            Console.WriteLine("OnError : " + e.AsyncSocketException.Message);
        }

        private void OnSend(object sender, AsyncDetectSocket.AsyncSocketSendEventArgs e)
        {

        }

        private void OnClose(object sender, AsyncDetectSocket.AsyncSocketConnectionEventArgs e)
        {

        }

        private void Init_System()
        {
            detectServer = new AsyncDetectSocket.AsyncSocketServer(9963);
            detectServer.OnAccept += new AsyncDetectSocket.AsyncSocketAcceptEventHandler(OnAccept);
            detectServer.OnError += new AsyncDetectSocket.AsyncSocketErrorEventHandler(OnError);
            detectServer.OnSend += new AsyncDetectSocket.AsyncSocketSendEventHandler(OnSend);
            detectServer.OnClose += new AsyncDetectSocket.AsyncSocketCloseEventHandler(OnClose);
            detectServer.Listen(IPAddress.Any);

            // 콤포넌트 초기화
            panel_Confirm.Visible = false;

            // 기종 정보 로드
            cb_kind.Items.Clear();
            foreach (KeyValuePair<string, string> model in m_Config.GetSectionValuesAsList("MODEL"))
            {
                cb_kind.Items.Add(model.Value);
            }
         
            // 방향 정보 로드
            cb_direction.Items.Clear();
            foreach (KeyValuePair<string, string> model in m_Config.GetSectionValuesAsList("Direction"))
            {
                cb_direction.Items.Add(model.Value);
            }

            if (cb_kind.Items.Count > 0)
                cb_kind.SelectedIndex = 0;
            if (cb_direction.Items.Count > 0)
                cb_direction.SelectedIndex = 0;

            // AI 합격점수 로드
            ai_score_numeric.Value = m_InspectionDetectConfig.GetInt32("Info", "Score", 70);

            //// 합격점수 로드
            numericUpDown1.Value = m_Config.GetInt32("Pattern", "Score", 60);

            //// 패턴 로드
            m_PMAlgin.LoadTool(Application.StartupPath + "\\Tools\\" + cb_kind.Text + cb_direction.Text + "Tool.vpp");
            m_PMAlgin.Load_Pattern(Application.StartupPath + "\\Pattern\\" + cb_kind.Text + "\\" + cb_direction.Text, cb_Model);   

            //// 마스터값 로드
            tb_MasterX.Text = m_Config.GetString("Master", cb_kind.Text + cb_direction.Text + "X", "");
            tb_MasterY.Text = m_Config.GetString("Master", cb_kind.Text + cb_direction.Text + "Y", "");

            string inspectionMode = m_Config.GetString("InspectionMode", cb_kind.Text + cb_direction.Text, "ALL").ToUpper();

            if (inspectionMode == "ALL")
            {
                // cb_InspectionMode.SelectedIndex = 0;

                //보정 값 로드
                tb_CorrectionX.Text = m_Config.GetString("AI Correction", cb_kind.Text + cb_direction.Text + "X", "0");
                tb_CorrectionY.Text = m_Config.GetString("AI Correction", cb_kind.Text + cb_direction.Text + "Y", "0");
            }
            else if (inspectionMode == "PATTERN")
            {
                // cb_InspectionMode.SelectedIndex = 2;

                //보정 값 로드
                tb_CorrectionX.Text = m_Config.GetString("Correction", cb_kind.Text + cb_direction.Text + "X", "0");
                tb_CorrectionY.Text = m_Config.GetString("Correction", cb_kind.Text + cb_direction.Text + "Y", "0");
            }
            else if (inspectionMode == "AI")
            {
                // cb_InspectionMode.SelectedIndex = 1;

                //보정 값 로드
                tb_CorrectionX.Text = m_Config.GetString("AI Correction", cb_kind.Text + cb_direction.Text + "X", "0");
                tb_CorrectionY.Text = m_Config.GetString("AI Correction", cb_kind.Text + cb_direction.Text + "Y", "0");
            }

            LoadCircleValue();
            LoadAISetting();
        }

        private List<string> m_Process_Recv = new List<string>();
        private Process m_Process;

        private void InitAI()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = Environment.CurrentDirectory + "\\SettingDetector\\Detector.exe";
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInfo.CreateNoWindow = true;
            m_Process = Process.Start(processStartInfo);
        }

        private void btn_ModelDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete Model", "Guidence Vision", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            // 패턴 파일 삭제
            string tmpPath = Application.StartupPath + "\\Pattern\\" + cb_kind.Text + "\\" + cb_direction.Text + "\\";
            System.IO.File.Delete(tmpPath + cb_Model.Text + ".hsr");

            //콤보박스 텍스트 지우고...
            cb_Model.Text = "";

            // 경로 지정 후...
            string[] files = System.IO.Directory.GetFiles(tmpPath);

            //for문 돌려서 i랑 파일이름이랑 같으면 계속 그렇지 않으면  
            for (int i = 0; i < files.Length; i++)
            {
                string name = files[i];

                if (int.Parse(name.Substring(name.Length - 7, 3)) == i)
                {
                    continue;
                }
                else
                {
                    //먼저 name을 i이름으로 파일복사하고 
                    System.IO.File.Copy(name, tmpPath + string.Format("{0:000}.hsr", i));
                    //기존name을 삭제한다.
                    System.IO.File.Delete(name);
                }
            }

            //다 끝나면 Master의 패턴을 다시 로드한다.
            m_PMAlgin.Load_Pattern(Application.StartupPath + "\\Pattern\\" + cb_kind.Text + "\\" + cb_direction.Text, cb_Model);

            //모델화면 지우기
            cogDisplay2.Image = null;
        }

        // 검사영역 설정
        private void btn_InspectionRange_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Modify Inspection Range");

            if (cogDisplay1.Image == null)
            {
                MessageBox.Show("Not Loaded Image", "Guidence Vision", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            cogDisplay1.InteractiveGraphics.Clear();
            cogDisplay1.StaticGraphics.Clear();

            // 현재 수정 모드
            m_CurState = CURSTATE.MODIFY_RANGE;

            
            // 2012.11.01 김기택
            m_CheckPosition = "";

            // 검사영역 표시
            m_PMAlgin.DisplaySearchArea(cogDisplay1, true);

            // 저장 버튼 활성화
            panel_Confirm.Visible = true;
        }

        private void btn_ModelAdd_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Add Model");

            if (cogDisplay1.Image == null)
            {
                MessageBox.Show("Not Loaded Image", "Guidence Vision", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }           

            cogDisplay1.InteractiveGraphics.Clear();
            cogDisplay1.StaticGraphics.Clear();

            // 현재 수정 모드
            m_CurState = CURSTATE.ADD_PATTERN;

            // 검사영역 표시
            m_PMAlgin.Display_PatternArea(cogDisplay1);

            // 저장 버튼 활성화
            panel_Confirm.Visible = true;

            m_CheckPosition = "";
        }



        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (m_CurState == CURSTATE.MODIFY_RANGE)
            {
                string tmp_Path = Application.StartupPath + "\\Tools\\";
                if (!System.IO.Directory.Exists(tmp_Path))
                    System.IO.Directory.CreateDirectory(tmp_Path);

                m_PMAlgin.Angle = 0;
                m_PMAlgin.Zoom = 0;
                m_PMAlgin.OKScore = (double)numericUpDown1.Value / 100.0;
                
                // 패턴의 정상 여부 확인을 위해 찾은 패턴 기준 왼쪽과 오른쪽에서 특정 모양을 찾음.
                // 그에 따른 세팅값 저장.
                // 2012.11.01 김기택
                if (m_CheckPosition == "")
                {
                    m_PMAlgin.SaveTool(tmp_Path + "\\" + cb_kind.Text + cb_direction.Text + "Tool.vpp");                    
                }

                //m_PMAlgin.LoadTool(tmp_Path + "\\Tool.vpp");
            }
            else if (m_CurState == CURSTATE.ADD_PATTERN)
            {
                string tmp_Path = Application.StartupPath + "\\Pattern\\";
                if (!System.IO.Directory.Exists(tmp_Path))
                    System.IO.Directory.CreateDirectory(tmp_Path);

                tmp_Path += cb_kind.Text + "\\";
                if (!System.IO.Directory.Exists(tmp_Path))
                    System.IO.Directory.CreateDirectory(tmp_Path);

                tmp_Path += cb_direction.Text + "\\";
                if (!System.IO.Directory.Exists(tmp_Path))
                    System.IO.Directory.CreateDirectory(tmp_Path);

                try
                {
                    if (m_CheckPosition == "")
                    {
                        string[] models = System.IO.Directory.GetFiles(tmp_Path);

                        m_PMAlgin.Set_Pattern(tmp_Path + string.Format("{0:000}.hsr", models.Length), cogDisplay1.Image, false);

                        m_PMAlgin.Load_Pattern(tmp_Path, cb_Model);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            // 화면 클리어
            cogDisplay1.InteractiveGraphics.Clear();
            cogDisplay1.StaticGraphics.Clear();
        }

        private void btn_Cancle_Click(object sender, EventArgs e)
        {
            cogDisplay1.InteractiveGraphics.Clear();
            cogDisplay1.StaticGraphics.Clear();
        }

        private string loadedImgPath;
        private Bitmap loadedBitmap;

        private void btn_LoadImage_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Load Image");

            cogDisplay1.InteractiveGraphics.Clear();
            cogDisplay1.StaticGraphics.Clear();

            OpenFileDialog loadImage = new OpenFileDialog();

            loadImage.Filter = "";
            loadImage.Filter += "Image File|*.jpg;*.bmp|all File|*.*";

            if (loadImage.ShowDialog() == DialogResult.OK)
                loadedImgPath = loadImage.FileName;
            else return;
             
            ImageFile imageFile = new ImageFile();

            try
            {
                loadedBitmap = new Bitmap(loadedImgPath);

                cogDisplay1.Image = new CogImage24PlanarColor(loadedBitmap);
                // 이미지 변환
                ICogImage monoimage = cogDisplay1.Image.GetType().Name == "CogImage8Grey" ? cogDisplay1.Image : new ImageFile().Get_Plan((CogImage24PlanarColor)cogDisplay1.Image, "Intensity");

                // 캘리브레이션 툴 로드
                CogCalibCheckerboardTool calib = new CogCalibCheckerboardTool();
                calib = (Cognex.VisionPro.CalibFix.CogCalibCheckerboardTool)Cognex.VisionPro.CogSerializer.LoadObjectFromFile(System.Windows.Forms.Application.StartupPath + "\\Tools\\" + cb_kind.Text + cb_direction.Text + "Calib.vpp");
                calib.InputImage = monoimage;

                calib.Run();
                                
                m_PMAlgin.Image = calib.OutputImage;
                m_PMAlgin.MaskImage = null;
                cogDisplay1.Image = calib.OutputImage;
            }
            catch(Exception ex)
            {
            }
        }

        Cognex.VisionPro.ImageProcessing.CogCopyRegionTool tool;

        private Bitmap CropBitmap(Bitmap bitmap, Rectangle rectangle)
        {
            Bitmap crop = new Bitmap(rectangle.Width, rectangle.Height);

            using (Graphics g = Graphics.FromImage(crop))
            {
                g.DrawImage(bitmap, -rectangle.X, -rectangle.Y);
                return crop;
            }
        }

        private void btn_DoInspection_Click(object sender, EventArgs e)
        {
            ToolBase m_DrawTool = new ToolBase();
            m_DrawTool.Image = cogDisplay1.Image;

            double correctionX;
            double correctionY;

            if (double.TryParse(tb_CorrectionX.Text, out correctionX) == false || double.TryParse(tb_CorrectionY.Text, out correctionY) == false)
            {
                MessageBox.Show("보정 값이 잘못 되었습니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            double patternCT = 0;
            double circleCT = 0;
            double aiCT = 0;
            double totalCT = 0;

            Stopwatch sw = new Stopwatch();
            Stopwatch swTotal = new Stopwatch();

            swTotal.Start();

            cogDisplay1.InteractiveGraphics.Clear();
            cogDisplay1.StaticGraphics.Clear();

            string inspectionMode = m_Config.GetString("InspectionMode", cb_kind.Text + cb_direction.Text, "ALL").ToUpper();

            double x = 0;
            double y = 0;

            AIResult aiResult = null;

            if (inspectionMode == "ALL" || inspectionMode == "AI")
            {
                sw.Start();

                aiResult = FindAI(loadedBitmap);

                sw.Stop();
                aiCT = sw.ElapsedMilliseconds;
                sw.Reset();

                x = aiResult.X;
                y = aiResult.Y;

                if (aiResult != null)
                {
                    m_DrawTool.DrawLabel(string.Format("{0}", aiResult.Name), cogDisplay1, aiResult.OriginX, aiResult.OriginY + aiResult.Height, 12, CogColorConstants.White, CogColorConstants.Purple);
                }
            }

            if (inspectionMode == "ALL" || inspectionMode == "PATTERN")
            {
                m_PMAlgin.LoadTool(Application.StartupPath + "\\Tools\\" + cb_kind.Text + cb_direction.Text + "Tool.vpp");
                m_PMAlgin.Load_Pattern(Application.StartupPath + "\\Pattern\\" + cb_kind.Text + "\\" + cb_direction.Text, cb_Model);

                sw.Start();
                double score = m_PMAlgin.FindPattern(cogDisplay1, true);
                sw.Stop();
                patternCT = sw.ElapsedMilliseconds;
                sw.Reset();
                score = (double)((int)(score * 10000 + 0.5)) / 10000.0;     // 소수 세째 자리 반올림

                // 패턴 중심 위치
                x = m_PMAlgin.TranslationX;
                y = m_PMAlgin.TranslationY;

                if (score * 100.0 > m_Config.GetDouble("Pattern", "Score", 60))
                {

                    sw.Start();
                    bool findCircleResult = FindCircle(m_PMAlgin.TranslationX, m_PMAlgin.TranslationY);
                    sw.Stop();

                    circleCT = sw.ElapsedMilliseconds;
                    sw.Reset();
                }

                m_PMAlgin.DrawLabel("Pattern Score = " + (score * 100.0) + "(" + m_PMAlgin.FindPatternIndex + ")", cogDisplay1, 17, 30, 17, score * 100.0 > m_Config.GetDouble("Pattern", "Score", 60) ? CogColorConstants.White : CogColorConstants.Red, CogColorConstants.Black);
            }

            lb_FindX.Text = Math.Round(x, 2).ToString();
            lb_FindY.Text = Math.Round(y, 2).ToString();

            double moveXValue = Math.Round(((double.Parse(tb_MasterX.Text) + Convert.ToDouble(tb_CorrectionX.Text)) - x), 4);
            double moveYValue = Math.Round(((double.Parse(tb_MasterY.Text) + Convert.ToDouble(tb_CorrectionY.Text)) - y), 4);

            if (m_Config.GetString("MoveValue", "MinusX", "False").ToUpper() == "TRUE")
            {
                moveXValue = moveXValue * -1;
            }

            if (m_Config.GetString("MoveValue", "MinusY", "False").ToUpper() == "TRUE")
            {
                moveYValue = moveYValue * -1;
            }

            lb_MoveX.Text = moveXValue.ToString();
            lb_MoveY.Text = moveYValue.ToString();

            swTotal.Stop();
            totalCT = swTotal.ElapsedMilliseconds;

            //CT 표시

            {
                ICogTransform2D map = cogDisplay1.Image.GetTransform("@", "@\\Checkerboard Calibration");
                double mappedX, mappedY;
                map = cogDisplay1.Image.GetTransform("@\\Checkerboard Calibration", "@");

                //패턴 CT 표시
                map.MapPoint(17, cogDisplay1.Image.Height - (270 * 3) - 10, out mappedX, out mappedY);

                CogGraphicLabel label = new CogGraphicLabel();
                label.Text = "PATTERN 소요 시간 : " + patternCT;
                label.Alignment = CogGraphicLabelAlignmentConstants.BottomLeft;
                label.X = mappedX;
                label.Y = mappedY;
                label.BackgroundColor = CogColorConstants.Black;
                label.Color = CogColorConstants.White;
                label.Font = new Font("돋움", 17);

                cogDisplay1.StaticGraphics.Add(label, "");

                //원 찾기 CT 표시
                map.MapPoint(17, cogDisplay1.Image.Height - (270 * 2) - 10, out mappedX, out mappedY);

                label = new CogGraphicLabel();
                label.Text = "원 찾기 소요시간 : " + circleCT;
                label.Alignment = CogGraphicLabelAlignmentConstants.BottomLeft;
                label.X = mappedX;
                label.Y = mappedY;
                label.BackgroundColor = CogColorConstants.Black;
                label.Color = CogColorConstants.White;
                label.Font = new Font("돋움", 17);

                cogDisplay1.StaticGraphics.Add(label, "");

                //AI CT 표시
                map.MapPoint(17, cogDisplay1.Image.Height - (270 * 1) - 10, out mappedX, out mappedY);

                label = new CogGraphicLabel();
                label.Text = "AI 소요 시간 : " + aiCT;
                label.Alignment = CogGraphicLabelAlignmentConstants.BottomLeft;
                label.X = mappedX;
                label.Y = mappedY;
                label.BackgroundColor = CogColorConstants.Black;
                label.Color = CogColorConstants.White;
                label.Font = new Font("돋움", 17);

                cogDisplay1.StaticGraphics.Add(label, "");

                //전체 CT 표시
                map.MapPoint(17, cogDisplay1.Image.Height - (270 * 0) - 10, out mappedX, out mappedY);

                label = new CogGraphicLabel();
                label.Text = "검사 전체 소요 시간 : " + totalCT;
                label.Alignment = CogGraphicLabelAlignmentConstants.BottomLeft;
                label.X = mappedX;
                label.Y = mappedY;
                label.BackgroundColor = CogColorConstants.Black;
                label.Color = CogColorConstants.White;
                label.Font = new Font("돋움", 17);

                cogDisplay1.StaticGraphics.Add(label, "");
            }
        }

        private AIResult FindAI(Bitmap bitmap)
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();

            List<YoloItem> items = new List<YoloItem>();
            double startX = 0;
            double endX = 0;
            double startY = 0;
            double endY = 0;
            bool m_IsFind = false;
            AIResult result = new AIResult();
            result.X = 0;
            result.Y = 0;
            result.Result = false;

            sw.Stop();
            Console.WriteLine("FindAI 소요 시간 : " + sw.ElapsedMilliseconds + "ms");

            if (detectClient != null)
            {
                bool isConnected = detectClient.Connection.Connected;
                bool isAliveSocket = detectClient.IsAliveSocket();
                int _index = -1;
                Stopwatch yoloSw = new Stopwatch();

                if (detectClient != null && isAliveSocket)
                {
                    CogRectangleAffine m_RectangleRegion = (CogRectangleAffine)m_PMAlgin.Region;

                    double centerX = m_RectangleRegion.CenterX;
                    double centerY = m_RectangleRegion.CenterY;
                    double sideXLength = m_RectangleRegion.SideXLength;
                    double sideYLength = m_RectangleRegion.SideYLength;

                    startX = centerX - sideXLength / 2;
                    endX = centerX + sideXLength / 2;
                    startY = centerY - sideYLength / 2;
                    endY = centerY + sideYLength / 2;

                    ICogTransform2D map = cogDisplay1.Image.GetTransform("@", "@\\Checkerboard Calibration");
                    map.MapPoint(startX, startY, out startX, out startY);
                    map.MapPoint(endX, endY, out endX, out endY);

                    GC.Collect();

                    Stopwatch convertSw = new Stopwatch();
                    convertSw.Start();

                    // Bitmap crop = CropBitmap(cogDisplay1.Image.ToBitmap(), new Rectangle((int)startX, (int)startY, (int)(endX - startX), (int)(endY - startY)));

                    Mat mat = BitmapConverter.ToMat(bitmap);

                    if (netChannels == 1)
                    {
                        if (mat.Channels() == 3)
                        {
                            mat = mat.CvtColor(ColorConversionCodes.BGR2GRAY);
                        }
                    }
                    else if (netChannels == 3)
                    {
                        if (mat.Channels() == 1)
                        {
                            mat = mat.CvtColor(ColorConversionCodes.GRAY2BGR);
                        }
                    }

                    convertSw.Stop();
                    Console.WriteLine("변환 속도 : " + convertSw.ElapsedMilliseconds);

                    GC.Collect();

                    int width = mat.Width;
                    int height = mat.Height;
                    int channels = mat.Channels();
                    byte[] output = new byte[width * height * channels];

                    Cv2.ImEncode(".bmp", mat, out output);

                    // 커맨드 바이트 배열
                    string sendStr = "IMAGE," + sendImageIndex + ",";
                    _index = sendImageIndex;
                    sendImageIndex++;

                    byte[] buf = Encoding.Default.GetBytes(sendStr);
                    int len = buf.Length + output.Length + 16;
                    byte[] merge = new byte[len];

                    Buffer.BlockCopy(BitConverter.GetBytes(len - 4), 0, merge, 0, 4);
                    Buffer.BlockCopy(buf, 0, merge, 4, buf.Length);
                    Buffer.BlockCopy(BitConverter.GetBytes(height), 0, merge, buf.Length + 4, 4);
                    Buffer.BlockCopy(BitConverter.GetBytes(width), 0, merge, buf.Length + 8, 4);
                    Buffer.BlockCopy(BitConverter.GetBytes(channels), 0, merge, buf.Length + 12, 4);
                    Buffer.BlockCopy(output, 0, merge, buf.Length + 16, output.Length);

                    detectClient.Send(merge);

                    yoloSw.Start();

                    Console.WriteLine("인식 시작 시간 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                    // LogManager.Action("인식 엔진 프로그램 경로 전송 완료 (" + path + ")");
                }

                Stopwatch detectSw = new Stopwatch();
                detectSw.Start();

                while (detectSw.ElapsedMilliseconds <= m_Config.GetInt32("AI", "DetectLimitMilliseconds", 200))
                {
                    if (m_Process_Recv != null && m_Process_Recv.Count > 0)
                    {
                        for (int i = 0; i < m_Process_Recv.Count; i++)
                        {
                            string data = m_Process_Recv[i];

                            if (data != null)
                            {
                                List<string> list = data.Split('/').ToList();

                                if (list[0] == Convert.ToString(_index))
                                {
                                    list.GetRange(1, list.Count - 1).ForEach(_x =>
                                    {
                                        if (_x != "")
                                        {
                                            string[] arr = _x.Split(',');

                                            YoloItem item = new YoloItem()
                                            {
                                                Type = arr[0],
                                                Confidence = Convert.ToDouble(arr[1]),
                                                X = Convert.ToDouble(arr[2]),
                                                Y = Convert.ToDouble(arr[3]),
                                                Width = Convert.ToDouble(arr[4]),
                                                Height = Convert.ToDouble(arr[5])
                                            };

                                            item.X = item.X - item.Width / 2;
                                            item.Y = item.Y - item.Height / 2;

                                            items.Add(item);
                                        }
                                    });

                                    m_Process_Recv.RemoveAt(i);

                                    m_IsFind = true;

                                    yoloSw.Stop();

                                    Console.WriteLine("인식 소요 시간 : " + yoloSw.ElapsedMilliseconds + "ms");

                                    Console.WriteLine("인식 종료 시간 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                                    break;
                                }
                            }
                        }
                    }

                    if (m_IsFind)
                    {
                        break;
                    }
                }

                detectSw.Stop();

                for (int j = 0; j < items.Count; j++)
                {
                    YoloItem item = items[j];

                    // item.X += Convert.ToInt32(startX);
                    // item.Y += Convert.ToInt32(startY);

                    CogRectangle rec = new CogRectangle();
                    rec.SelectedSpaceName = "@";
                    rec.SetXYWidthHeight(item.X, item.Y, item.Width, item.Height);
                    rec.Color = CogColorConstants.Purple;
                    rec.Interactive = false;
                    rec.GraphicDOFEnable = CogRectangleDOFConstants.All;
                    rec.LineWidthInScreenPixels = 3;

                    double mappedX = 0, mappedY = 0;
                    ICogTransform2D map = cogDisplay1.Image.GetTransform("@\\Checkerboard Calibration", "@");
                    map.MapPoint(rec.CenterX, rec.CenterY, out mappedX, out mappedY);

                    CogRectangleAffine m_RectangleRegion = (CogRectangleAffine)m_PMAlgin.Region;

                    double centerX = m_RectangleRegion.CenterX;
                    double centerY = m_RectangleRegion.CenterY;
                    double sideXLength = m_RectangleRegion.SideXLength;
                    double sideYLength = m_RectangleRegion.SideYLength;

                    double _startX = centerX - sideXLength / 2;
                    double _endX = centerX + sideXLength / 2;
                    double _startY = centerY - sideYLength / 2;
                    double _endY = centerY + sideYLength / 2;

                    if (mappedX > _startX && mappedX < _endX && mappedY > _startY && mappedY < _endY)
                    {
                        result.Name = item.Type;
                        result.OriginX = item.X;
                        result.OriginY = item.Y;
                        result.X = mappedX;
                        result.Y = mappedY;
                        result.Width = item.Width;
                        result.Height = item.Height;
                        result.Result = true;
                        result.Score = item.Confidence;

                        cogDisplay1.InteractiveGraphics.Add(rec, "", false);
                    }
                }

            }

            return result;
        }

        private void cb_Model_SelectedIndexChanged(object sender, EventArgs e)
        {
            cogDisplay2.InteractiveGraphics.Clear();
            cogDisplay2.StaticGraphics.Clear();
            cogDisplay2.Image = null;

            m_PMAlgin.Show_Pattern(cb_Model.SelectedIndex, cogDisplay2);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 켈리브레이션 툴
            CogCalibCheckerboardTool cali = new CogCalibCheckerboardTool();
            // 켈리브레이션 툴 이미지 로드
            if (cogDisplay1.Image == null)
            {
                MessageBox.Show("이미지가 없습니다.", "A엔진 블럭 투입 로봇비전", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            cali.InputImage = cogDisplay1.Image.GetType().Name == "CogImage8Grey" ? cogDisplay1.Image : new ImageFile().Get_Plan((CogImage24PlanarColor)cogDisplay1.Image, "Intensity");

            // 설정
            cali.Calibration.ComputationMode = CogCalibFixComputationModeConstants.Linear;
            cali.Calibration.FiducialMark = CogCalibCheckerboardFiducialConstants.None;
            cali.Calibration.CalibratedOriginSpace = CogCalibCheckerboardAdjustmentSpaceConstants.Uncalibrated;
            cali.Calibration.CalibratedXAxisRotationSpace = CogCalibCheckerboardAdjustmentSpaceConstants.Uncalibrated;

            // 타일 사이즈
            cali.Calibration.PhysicalTileSizeX = 10;
            cali.Calibration.PhysicalTileSizeY = 10;

            // 보정 이미지 그랩
            cali.Calibration.CalibrationImage = cali.InputImage;

            // 보정 계산
            cali.Calibration.Calibrate();
            // 캘리브레이션 툴 실행
            cali.Run();

            // 결과 저장
            string tmp_Path = Application.StartupPath + "\\Tools\\";
            if (!System.IO.Directory.Exists(tmp_Path))
                System.IO.Directory.CreateDirectory(tmp_Path);

            Cognex.VisionPro.CogSerializer.SaveObjectToFile(cali, Application.StartupPath + "\\Tools\\Calib.vpp");
        }

        private void cb_kind_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  cb_direction.Text = m_Config.GetString("Number", cb_kind.Items.IndexOf, "Error");
            
            // 패턴 로드
            m_PMAlgin.LoadTool(Application.StartupPath + "\\Tools\\" + cb_kind.Text + cb_direction.Text + "Tool.vpp");
            m_PMAlgin.Load_Pattern(Application.StartupPath + "\\Pattern\\" + cb_kind.Text + "\\" + cb_direction.Text, cb_Model);

            // 합격점수 로드
            numericUpDown1.Value = m_Config.GetInt32("Pattern", "Score", 60);
            
            // 마스터값 로드
            tb_MasterX.Text = m_Config.GetString("Master", cb_kind.Text + cb_direction.Text + "X", "");
            tb_MasterY.Text = m_Config.GetString("Master", cb_kind.Text + cb_direction.Text + "Y", "");

            string inspectionMode = m_Config.GetString("InspectionMode", cb_kind.Text + cb_direction.Text, "ALL").ToUpper();
            string inspectionModeStr = "";

            if (inspectionMode == "ALL")
            {
                inspectionModeStr = "AI + 패턴모드";

                tb_CorrectionX.Text = m_Config.GetString("AI Correction ", cb_kind.Text + cb_direction.Text + "X", "0");
                tb_CorrectionY.Text = m_Config.GetString("AI Correction ", cb_kind.Text + cb_direction.Text + "Y", "0");
            }
            else if (inspectionMode == "AI")
            {
                inspectionModeStr = "AI모드";

                tb_CorrectionX.Text = m_Config.GetString("AI Correction", cb_kind.Text + cb_direction.Text + "X", "0");
                tb_CorrectionY.Text = m_Config.GetString("AI Correction", cb_kind.Text + cb_direction.Text + "Y", "0");
            }
            else if (inspectionMode == "PATTERN")
            {
                inspectionModeStr = "패턴모드";

                tb_CorrectionX.Text = m_Config.GetString("Correction", cb_kind.Text + cb_direction.Text + "X", "0");
                tb_CorrectionY.Text = m_Config.GetString("Correction", cb_kind.Text + cb_direction.Text + "Y", "0");
            }

            // cb_InspectionMode.Text = inspectionModeStr;

            bool isPattern = false;
            bool isCircle = false;
            bool isAI = false;

            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                TabPage tabPage = tabControl1.TabPages[i];

                if (tabPage.Text == "패턴")
                {
                    isPattern = true;
                }
                else if (tabPage.Text == "원 찾기")
                {
                    isCircle = true;
                }
                else if (tabPage.Text == "AI")
                {
                    isAI = true;
                }
            }

            if (inspectionMode == "PATTERN")
            {
                if (!isPattern)
                {
                    tabControl1.TabPages.Add(tabPage1);
                }

                if (!isCircle)
                {
                    tabControl1.TabPages.Add(tabPage2);
                }

                if (isAI)
                {
                    tabControl1.TabPages.Remove(tabPage5);
                }
            }
            else if (inspectionMode == "AI")
            {
                if (isPattern)
                {
                    tabControl1.TabPages.Remove(tabPage1);
                }

                if (isCircle)
                {
                    tabControl1.TabPages.Remove(tabPage2);
                }

                if (!isAI)
                {
                    tabControl1.TabPages.Add(tabPage5);
                }
            }
            else if (inspectionMode == "ALL")
            {
                if (!isPattern)
                {
                    tabControl1.TabPages.Add(tabPage1);
                }

                if (!isCircle)
                {
                    tabControl1.TabPages.Add(tabPage2);
                }

                if (!isAI)
                {
                    tabControl1.TabPages.Add(tabPage5);
                }
            }

            if (tabControl1.TabPages.Count >= 3)
            {
                for (int i = 0; i < tabControl1.TabPages.Count; i++)
                {
                    TabPage tabPage = tabControl1.TabPages[i];

                    if (tabPage.Text == "AI" && i != 0)
                    {
                        TabPage temp = tabControl1.TabPages[0];
                        tabControl1.TabPages[0] = tabControl1.TabPages[i];
                        tabControl1.TabPages[i] = temp;
                    }
                    else if (tabPage.Text == "패턴" && i != 1)
                    {
                        TabPage temp = tabControl1.TabPages[1];
                        tabControl1.TabPages[1] = tabControl1.TabPages[i];
                        tabControl1.TabPages[i] = temp;
                    }
                    else if (tabPage.Text == "원 찾기" && i != 2)
                    {
                        TabPage temp = tabControl1.TabPages[2];
                        tabControl1.TabPages[2] = tabControl1.TabPages[i];
                        tabControl1.TabPages[i] = temp;
                    }
                }
            }
        }

        private void btn_SaveScore_Click(object sender, EventArgs e)
        {

            m_PMAlgin.OKScore = (double)numericUpDown1.Value / 100.0;
            m_Config.WriteValue("Pattern", "Score", (int)numericUpDown1.Value);
            // 합격점수 로드
            numericUpDown1.Value = m_Config.GetInt32("Pattern", "Score", 60);
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private FindCircle circle = new FindCircle();
        
        private void btn_MaskImage_Click(object sender, EventArgs e)
        {
            Form_Masking mask = new Form_Masking((CogImage8Grey)cogDisplay1.Image);

            if (mask.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_PMAlgin.MaskImage = (CogImage8Grey)mask.MASKIMAGE;
            }
        }
        
        private void btn_MasterSave_Click(object sender, EventArgs e)
        {
            if (tb_MasterX.Text == "" || tb_MasterY.Text == "")
            {
                MessageBox.Show("Master Not Value");
                return;
            }
            else
            {
                m_Config.WriteValue("Master", cb_kind.Text + cb_direction.Text + "X", tb_MasterX.Text);
                m_Config.WriteValue("Master", cb_kind.Text + cb_direction.Text + "Y", tb_MasterY.Text);
            }
        }

        private void btn_FindToMaster_Click(object sender, EventArgs e)
        {
            if (lb_FindX.Text == "" || lb_FindY.Text == "")
            {
                MessageBox.Show("Find Position Not Value");
                return;
            }
            tb_MasterX.Text = lb_FindX.Text;
            tb_MasterY.Text = lb_FindY.Text;
        }



        private bool FindCircle(double x, double y)
        {
            //#region 중앙원 찾기
            Cognex.VisionPro.Caliper.CogFindCircleTool cogFindCircle = new Cognex.VisionPro.Caliper.CogFindCircleTool();

            cogFindCircle.InputImage = (CogImage8Grey)cogDisplay1.Image;

            cogFindCircle.RunParams.CaliperSearchDirection = m_Config.GetString("Circle", "Direction", "Inward") == "Inward" ? Cognex.VisionPro.Caliper.CogFindCircleSearchDirectionConstants.Inward : Cognex.VisionPro.Caliper.CogFindCircleSearchDirectionConstants.Outward;

            cogFindCircle.RunParams.CaliperRunParams.Edge0Polarity = Cognex.VisionPro.Caliper.CogCaliperPolarityConstants.DontCare;

            cogFindCircle.RunParams.CaliperRunParams.EdgeMode = Cognex.VisionPro.Caliper.CogCaliperEdgeModeConstants.SingleEdge;
            cogFindCircle.RunParams.CaliperRunParams.FilterHalfSizeInPixels = m_Config.GetInt32("Circle", "FilterHalfSizeInPixels", 2);
            cogFindCircle.RunParams.CaliperRunParams.ContrastThreshold = m_Config.GetInt32("Circle", "ContrastThreshold", 5);

            cogFindCircle.RunParams.NumCalipers = m_Config.GetInt32("Circle", "NumCalipers", 40);

            cogFindCircle.RunParams.NumToIgnore = m_Config.GetInt32("Circle", "NumIgnores", 10);


            cogFindCircle.RunParams.CaliperSearchLength = m_Config.GetInt32("Circle", "CaliperSearchLength", 23);

            cogFindCircle.RunParams.CaliperProjectionLength = m_Config.GetDouble("Circle", "CaliperProjectionLength", 0.8);


            // 찾을원 위치
            cogFindCircle.RunParams.ExpectedCircularArc.CenterX = x;
            cogFindCircle.RunParams.ExpectedCircularArc.CenterY = y;
            cogFindCircle.RunParams.ExpectedCircularArc.Radius = m_Config.GetDouble("Circle", "Radius", 12);
            cogFindCircle.RunParams.ExpectedCircularArc.AngleStart = 0;
            cogFindCircle.RunParams.ExpectedCircularArc.AngleSpan = 360;

            cogFindCircle.Run();

            if (cogFindCircle.Results != null && cogFindCircle.Results.GetCircle() != null)
            {
                Cognex.VisionPro.ICogGraphic ResultGp;

                ResultGp = (Cognex.VisionPro.ICogGraphic)cogFindCircle.Results.GetCircularArc();


                ResultGp.Color = Cognex.VisionPro.CogColorConstants.Blue;

                cogDisplay1.StaticGraphics.Add(ResultGp, "");

                m_PMAlgin.DrawLabel("Find Circle : OK", cogDisplay1, 17, 300, 17, CogColorConstants.White, CogColorConstants.Black);

                return true;
            }
            else
            {
                m_PMAlgin.DrawLabel("Find Circle : NG", cogDisplay1, 17, 300, 17, CogColorConstants.Red, CogColorConstants.Black);

                return false;
            }
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {

        }

        private void LoadCircleValue()
        {
            string savedDirectionName = m_Config.GetString("Circle", "Direction", "Inword");
            foreach (object item in cb_Circle_Direction.Items)
            {
                if(item.ToString() == savedDirectionName)
                {
                    cb_Circle_Direction.SelectedItem = item;
                }
            }

            cb_Circle_Direction.SelectedText = m_Config.GetString("Circle", "Direction", "Inword");
            num_Circle_FilterHalfSize.Value = m_Config.GetInt32("Circle", "FilterHalfSizeInPixels", 2);
            num_Circle_Threshold.Value = m_Config.GetInt32("Circle", "ContrastThreshold", 5);
            num_Circle_NumCalipers.Value = m_Config.GetInt32("Circle", "NumCalipers", 40);
            num_Circle_NumTolgnore.Value = m_Config.GetInt32("Circle", "NumIgnores", 10);

            num_Circle_SearchLength.Value = m_Config.GetInt32("Circle", "CaliperSearchLength", 23);
            num_Circle_SearchProgrectionLength.Value = Convert.ToDecimal(m_Config.GetDouble("Circle", "CaliperProjectionLength", 0.8));

            num_Circle_Radius.Value = Convert.ToDecimal(m_Config.GetDouble("Circle", "Radius", 12));
        }

        private void btn_Circle_Save_Click(object sender, EventArgs e)
        {
            m_Config.WriteValue("Circle", "Direction", cb_Circle_Direction.SelectedItem.ToString());
            m_Config.WriteValue("Circle", "FilterHalfSizeInPixels", (int)num_Circle_FilterHalfSize.Value);
            m_Config.WriteValue("Circle", "ContrastThreshold", (int)num_Circle_Threshold.Value);
            m_Config.WriteValue("Circle", "NumCalipers", (int)num_Circle_NumCalipers.Value);
            m_Config.WriteValue("Circle", "NumIgnores", (int)num_Circle_NumTolgnore.Value);

            m_Config.WriteValue("Circle", "CaliperSearchLength", (int)num_Circle_SearchLength.Value);
            m_Config.WriteValue("Circle", "CaliperProjectionLength", (double)num_Circle_SearchProgrectionLength.Value);

            m_Config.WriteValue("Circle", "Radius", (double)num_Circle_Radius.Value);
        }

        private void LoadAISetting()
        {
            // cb_UseAI.Checked = Convert.ToBoolean(m_Config.GetString("AI", "IsUse", "False"));
        }

        private void btn_AI_Save_Click(object sender, EventArgs e)
        {
            // m_Config.WriteValue("AI", "IsUse", cb_UseAI.Checked.ToString());

            m_InspectionDetectConfig.WriteValue("Info", "Score", Convert.ToInt32(ai_score_numeric.Value));
            m_SettingDetectConfig.WriteValue("Info", "Score", Convert.ToInt32(ai_score_numeric.Value));
        }

        private void btn_InspectionRange_Click_1(object sender, EventArgs e)
        {

            if (cogDisplay1.Image == null)
            {
                MessageBox.Show("Not Loaded Image", "Guidence Vision", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            cogDisplay1.InteractiveGraphics.Clear();
            cogDisplay1.StaticGraphics.Clear();

            // 현재 수정 모드
            m_CurState = CURSTATE.MODIFY_RANGE;


            // 2012.11.01 김기택
            m_CheckPosition = "";

            // 검사영역 표시
            m_PMAlgin.DisplaySearchArea(cogDisplay1, true);

            // 저장 버튼 활성화
            panel_Confirm.Visible = true;
        }

        private void btn_SaveCorrection_Click(object sender, EventArgs e)
        {
            /*
            DialogResult result = MessageBox.Show("보정값을 변경하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                double x;
                double y;

                string inspectionMode = m_Config.GetString("InspectionMode", cb_kind.Text + cb_direction.Text, "ALL").ToUpper();

                if (inspectionMode == "ALL")
                {
                    if (double.TryParse(tb_CorrectionX.Text, out x) && double.TryParse(tb_CorrectionY.Text, out y))
                    {
                        m_Config.WriteValue("AI Correction", cb_kind.Text + cb_direction.Text + "X", x.ToString());
                        m_Config.WriteValue("AI Correction", cb_kind.Text + cb_direction.Text + "Y", y.ToString());
                    }
                    else
                    {
                        MessageBox.Show("보정 값이 잘못 되었습니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (inspectionMode == "AI")
                {
                    if (double.TryParse(tb_CorrectionX.Text, out x) && double.TryParse(tb_CorrectionY.Text, out y))
                    {
                        m_Config.WriteValue("AI Correction", cb_kind.Text + cb_direction.Text + "X", x.ToString());
                        m_Config.WriteValue("AI Correction", cb_kind.Text + cb_direction.Text + "Y", y.ToString());
                    }
                    else
                    {
                        MessageBox.Show("보정 값이 잘못 되었습니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (inspectionMode == "PATTERN")
                {
                    if (double.TryParse(tb_CorrectionX.Text, out x) && double.TryParse(tb_CorrectionY.Text, out y))
                    {
                        m_Config.WriteValue("Correction", cb_kind.Text + cb_direction.Text + "X", x.ToString());
                        m_Config.WriteValue("Correction", cb_kind.Text + cb_direction.Text + "Y", y.ToString());
                    }
                    else
                    {
                        MessageBox.Show("보정 값이 잘못 되었습니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            */
        }

        private void lb_MoveX_Click(object sender, EventArgs e)
        {

        }

        private void Setting_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            detectProcessCheckThreadRunning = false;

            if (m_Process != null)
            {
                if (!m_Process.HasExited)
                {
                    m_Process.Kill();
                }

                m_Process.Close();
                m_Process.Dispose();
                m_Process = null;
            }

            Environment.Exit(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog openFileDialog = new CommonOpenFileDialog("폴더 열기");
            openFileDialog.IsFolderPicker = true;
            string content = "일시,시퀸스,바디넘버,차종,홀번호,패턴위치X(mm),패턴위치Y(mm),AI위치X(mm),AI위치Y(mm),X위치오차(mm),Y위치오차(mm),패턴인식속도(ms),AI인식속도(ms),인식속도차(ms),전체CT(ms)" + Environment.NewLine;

            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string dir = openFileDialog.FileName;
                string[] files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);

                new Thread(new ThreadStart(() =>
                {
                    this.Invoke(new Action(delegate()
                    {
                        for (int i = 0; i < files.Length; i++)
                        {
                            string filename = files[i];
                            string[] split = filename.Split('\\');
                            // DateTime dateTime = Convert.ToDateTime(split[3] + " " + split[5].Split('_')[0]);
                            DateTime dateTime = DateTime.ParseExact(split[3] + " " + split[5].Split('_')[0], "yyyy-MM-dd HHmmss", null);
                            string dateTimeStr = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                            string seq = split[5].Split('_')[1];
                            string bodyNumber = split[5].Split('_')[2];
                            string ext = Path.GetExtension(filename);
                            string model = "";
                            string direction = "";
                            double patternX = 0;
                            double patternY = 0;
                            double aiX = 0;
                            double aiY = 0;

                            if (ext == ".bmp" || ext == ".jpg")
                            {
                                loadedImgPath = filename;

                                filename = Path.GetFileName(filename);
                                string[] filenameSplit = filename.Split('_');

                                model = filenameSplit[1];
                                direction = filenameSplit[2];

                                int result = 0;

                                if (int.TryParse(direction, out result))
                                {
                                    if (result < 10)
                                    {
                                        direction = "0" + direction;
                                    }
                                }

                                cogDisplay1.InteractiveGraphics.Clear();
                                cogDisplay1.StaticGraphics.Clear();

                                ImageFile imageFile = new ImageFile();

                                try
                                {
                                    if (loadedBitmap != null)
                                    {
                                        loadedBitmap.Dispose();
                                        loadedBitmap = null;
                                    }

                                    loadedBitmap = new Bitmap(loadedImgPath);
                                    cogDisplay1.Image = new CogImage24PlanarColor(new Bitmap(loadedImgPath));
                                    // 이미지 변환
                                    ICogImage monoimage = cogDisplay1.Image.GetType().Name == "CogImage8Grey" ? cogDisplay1.Image : new ImageFile().Get_Plan((CogImage24PlanarColor)cogDisplay1.Image, "Intensity");

                                    // 캘리브레이션 툴 로드
                                    CogCalibCheckerboardTool calib = new CogCalibCheckerboardTool();
                                    calib = (Cognex.VisionPro.CalibFix.CogCalibCheckerboardTool)Cognex.VisionPro.CogSerializer.LoadObjectFromFile(System.Windows.Forms.Application.StartupPath + "\\Tools\\" + model + direction + "Calib.vpp");
                                    calib.InputImage = monoimage;

                                    calib.Run();

                                    m_PMAlgin.Image = calib.OutputImage;
                                    m_PMAlgin.MaskImage = null;
                                    cogDisplay1.Image = calib.OutputImage;
                                }
                                catch (Exception ex)
                                {
                                }
                            }

                            ToolBase m_DrawTool = new ToolBase();
                            m_DrawTool.Image = cogDisplay1.Image;

                            double correctionX;
                            double correctionY;

                            if (double.TryParse(tb_CorrectionX.Text, out correctionX) == false || double.TryParse(tb_CorrectionY.Text, out correctionY) == false)
                            {
                                MessageBox.Show("보정 값이 잘못 되었습니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            double patternCT = 0;
                            double circleCT = 0;
                            double aiCT = 0;
                            double totalCT = 0;

                            Stopwatch sw = new Stopwatch();
                            Stopwatch swTotal = new Stopwatch();

                            swTotal.Start();

                            cogDisplay1.InteractiveGraphics.Clear();
                            cogDisplay1.StaticGraphics.Clear();

                            string inspectionMode = m_Config.GetString("InspectionMode", cb_kind.Text + cb_direction.Text, "ALL").ToUpper();

                            double x = 0;
                            double y = 0;

                            AIResult aiResult = null;

                            m_PMAlgin.LoadTool(Application.StartupPath + "\\Tools\\" + model + direction + "Tool.vpp");
                            m_PMAlgin.Load_Pattern(Application.StartupPath + "\\Pattern\\" + model + "\\" + direction, cb_Model);

                            if (inspectionMode == "ALL" || inspectionMode == "AI")
                            {
                                sw.Start();

                                aiResult = FindAI(loadedBitmap);

                                aiX = aiResult.X;
                                aiY = aiResult.Y;

                                sw.Stop();
                                aiCT = sw.ElapsedMilliseconds;
                                sw.Reset();

                                x = aiResult.X;
                                y = aiResult.Y;

                                if (aiResult != null)
                                {
                                    m_DrawTool.DrawLabel(string.Format("{0}", aiResult.Name), cogDisplay1, aiResult.OriginX, aiResult.OriginY + aiResult.Height, 12, CogColorConstants.White, CogColorConstants.Purple);
                                }
                            }

                            if (inspectionMode == "ALL" || inspectionMode == "PATTERN")
                            {
                                sw.Start();
                                double score = m_PMAlgin.FindPattern(cogDisplay1, true);
                                sw.Stop();
                                patternCT = sw.ElapsedMilliseconds;
                                sw.Reset();
                                score = (double)((int)(score * 10000 + 0.5)) / 10000.0;     // 소수 세째 자리 반올림

                                // 패턴 중심 위치
                                x = m_PMAlgin.TranslationX;
                                y = m_PMAlgin.TranslationY;

                                patternX = x;
                                patternY = y;

                                if (score * 100.0 > m_Config.GetDouble("Pattern", "Score", 60))
                                {

                                    sw.Start();
                                    bool findCircleResult = FindCircle(m_PMAlgin.TranslationX, m_PMAlgin.TranslationY);
                                    sw.Stop();

                                    circleCT = sw.ElapsedMilliseconds;
                                    sw.Reset();
                                }

                                m_PMAlgin.DrawLabel("Pattern Score = " + (score * 100.0) + "(" + m_PMAlgin.FindPatternIndex + ")", cogDisplay1, 17, 30, 17, score * 100.0 > m_Config.GetDouble("Pattern", "Score", 60) ? CogColorConstants.White : CogColorConstants.Red, CogColorConstants.Black);
                            }

                            lb_FindX.Text = Math.Round(x, 2).ToString();
                            lb_FindY.Text = Math.Round(y, 2).ToString();

                            double moveXValue = Math.Round(((double.Parse(tb_MasterX.Text) + Convert.ToDouble(tb_CorrectionX.Text)) - x), 4);
                            double moveYValue = Math.Round(((double.Parse(tb_MasterY.Text) + Convert.ToDouble(tb_CorrectionY.Text)) - y), 4);

                            if (m_Config.GetString("MoveValue", "MinusX", "False").ToUpper() == "TRUE")
                            {
                                moveXValue = moveXValue * -1;
                            }

                            if (m_Config.GetString("MoveValue", "MinusY", "False").ToUpper() == "TRUE")
                            {
                                moveYValue = moveYValue * -1;
                            }

                            lb_MoveX.Text = moveXValue.ToString();
                            lb_MoveY.Text = moveYValue.ToString();

                            swTotal.Stop();
                            totalCT = swTotal.ElapsedMilliseconds;

                            //CT 표시

                            {
                                ICogTransform2D map = cogDisplay1.Image.GetTransform("@", "@\\Checkerboard Calibration");
                                double mappedX, mappedY;
                                map = cogDisplay1.Image.GetTransform("@\\Checkerboard Calibration", "@");

                                //패턴 CT 표시
                                map.MapPoint(17, cogDisplay1.Image.Height - (270 * 3) - 10, out mappedX, out mappedY);

                                CogGraphicLabel label = new CogGraphicLabel();
                                label.Text = "PATTERN 소요 시간 : " + patternCT;
                                label.Alignment = CogGraphicLabelAlignmentConstants.BottomLeft;
                                label.X = mappedX;
                                label.Y = mappedY;
                                label.BackgroundColor = CogColorConstants.Black;
                                label.Color = CogColorConstants.White;
                                label.Font = new Font("돋움", 17);

                                cogDisplay1.StaticGraphics.Add(label, "");

                                //원 찾기 CT 표시
                                map.MapPoint(17, cogDisplay1.Image.Height - (270 * 2) - 10, out mappedX, out mappedY);

                                label = new CogGraphicLabel();
                                label.Text = "원 찾기 소요시간 : " + circleCT;
                                label.Alignment = CogGraphicLabelAlignmentConstants.BottomLeft;
                                label.X = mappedX;
                                label.Y = mappedY;
                                label.BackgroundColor = CogColorConstants.Black;
                                label.Color = CogColorConstants.White;
                                label.Font = new Font("돋움", 17);

                                cogDisplay1.StaticGraphics.Add(label, "");

                                //AI CT 표시
                                map.MapPoint(17, cogDisplay1.Image.Height - (270 * 1) - 10, out mappedX, out mappedY);

                                label = new CogGraphicLabel();
                                label.Text = "AI 소요 시간 : " + aiCT;
                                label.Alignment = CogGraphicLabelAlignmentConstants.BottomLeft;
                                label.X = mappedX;
                                label.Y = mappedY;
                                label.BackgroundColor = CogColorConstants.Black;
                                label.Color = CogColorConstants.White;
                                label.Font = new Font("돋움", 17);

                                cogDisplay1.StaticGraphics.Add(label, "");

                                //전체 CT 표시
                                map.MapPoint(17, cogDisplay1.Image.Height - (270 * 0) - 10, out mappedX, out mappedY);

                                label = new CogGraphicLabel();
                                label.Text = "검사 전체 소요 시간 : " + totalCT;
                                label.Alignment = CogGraphicLabelAlignmentConstants.BottomLeft;
                                label.X = mappedX;
                                label.Y = mappedY;
                                label.BackgroundColor = CogColorConstants.Black;
                                label.Color = CogColorConstants.White;
                                label.Font = new Font("돋움", 17);

                                cogDisplay1.StaticGraphics.Add(label, "");

                                content += dateTimeStr + "," + seq + "," + bodyNumber + "," + model + "," + direction + "," + Math.Round(patternX, 2) + "," + Math.Round(patternY, 2) + "," + Math.Round(aiX, 2) + "," + Math.Round(aiY, 2) + "," + Math.Round(patternX - aiX, 2) + "," + Math.Round(patternY - aiY, 2) + "," + patternCT + "," + aiCT + "," + (patternCT - aiCT) + "," + totalCT + Environment.NewLine;

                                File.WriteAllText(Environment.CurrentDirectory + "\\test.csv", content);
                            }
                        }
                    }));
                })).Start();
            }
        }

        private void cb_InspectionMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            string selectedItem = (string)cb_InspectionMode.SelectedItem;

            bool isPattern = false;
            bool isCircle = false;
            bool isAI = false;

            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                TabPage tabPage = tabControl1.TabPages[i];

                if (tabPage.Text == "패턴")
                {
                    isPattern = true;
                }
                else if (tabPage.Text == "원 찾기")
                {
                    isCircle = true;
                }
                else if (tabPage.Text == "AI")
                {
                    isAI = true;
                }
            }

            if (selectedItem == "패턴모드")
            {
                if (cb_direction != null && cb_direction.Text != "")
                {
                    m_Config.WriteValue("InspectionMode", cb_kind.Text + cb_direction.Text, "PATTERN");

                    //보정 값 로드
                    tb_CorrectionX.Text = m_Config.GetString("Correction", cb_kind.Text + cb_direction.Text + "X", "0");
                    tb_CorrectionY.Text = m_Config.GetString("Correction", cb_kind.Text + cb_direction.Text + "Y", "0");
                }

                if (!isPattern)
                {
                    tabControl1.TabPages.Add(tabPage1);
                }

                if (!isCircle)
                {
                    tabControl1.TabPages.Add(tabPage2);
                }

                if (isAI)
                {
                    tabControl1.TabPages.Remove(tabPage5);
                }
            }
            else if (selectedItem == "AI모드")
            {
                if (cb_direction != null && cb_direction.Text != "")
                {
                    m_Config.WriteValue("InspectionMode", cb_kind.Text + cb_direction.Text, "AI");

                    //보정 값 로드
                    tb_CorrectionX.Text = m_Config.GetString("AI Correction", cb_kind.Text + cb_direction.Text + "X", "0");
                    tb_CorrectionY.Text = m_Config.GetString("AI Correction", cb_kind.Text + cb_direction.Text + "Y", "0");
                }

                if (isPattern)
                {
                    tabControl1.TabPages.Remove(tabPage1);
                }

                if (isCircle)
                {
                    tabControl1.TabPages.Remove(tabPage2);
                }

                if (!isAI)
                {
                    tabControl1.TabPages.Add(tabPage5);
                }
            }
            else if (selectedItem == "AI + 패턴모드")
            {
                if (cb_direction != null && cb_direction.Text != "")
                {
                    m_Config.WriteValue("InspectionMode", cb_kind.Text + cb_direction.Text, "ALL");

                    //보정 값 로드
                    tb_CorrectionX.Text = m_Config.GetString("AI Correction", cb_kind.Text + cb_direction.Text + "X", "0");
                    tb_CorrectionY.Text = m_Config.GetString("AI Correction", cb_kind.Text + cb_direction.Text + "Y", "0");
                }

                if (!isPattern)
                {
                    tabControl1.TabPages.Add(tabPage1);
                }

                if (!isCircle)
                {
                    tabControl1.TabPages.Add(tabPage2);
                }

                if (!isAI)
                {
                    tabControl1.TabPages.Add(tabPage5);
                }
            }

            if (tabControl1.TabPages.Count >= 3)
            {
                for (int i = 0; i < tabControl1.TabPages.Count; i++)
                {
                    TabPage tabPage = tabControl1.TabPages[i];

                    if (tabPage.Text == "패턴" && i != 1)
                    {
                        TabPage temp = tabControl1.TabPages[0];
                        tabControl1.TabPages[0] = tabControl1.TabPages[i];
                        tabControl1.TabPages[i] = temp;
                    }
                    else if (tabPage.Text == "원 찾기" && i != 2)
                    {
                        TabPage temp = tabControl1.TabPages[1];
                        tabControl1.TabPages[1] = tabControl1.TabPages[i];
                        tabControl1.TabPages[i] = temp;
                    }
                    else if (tabPage.Text == "AI" && i != 0)
                    {
                        TabPage temp = tabControl1.TabPages[2];
                        tabControl1.TabPages[2] = tabControl1.TabPages[i];
                        tabControl1.TabPages[i] = temp;
                    }
                }
            }
            */
        }

        private void CarkindAndHoleSettingButton_Click(object sender, EventArgs e)
        {
            CarKindAndHoleSettingWindow window = new CarKindAndHoleSettingWindow();

            DialogResult result = window.ShowDialog();

            if (result == DialogResult.Cancel)
            {
                ai_score_numeric.Value = m_InspectionDetectConfig.GetInt32("Info", "Score", 70);
                numericUpDown1.Value = m_Config.GetInt32("Pattern", "Score", 60);
            }
        }
    }

    public class YoloItem
    {
        public string Type { get; set; }

        public double Confidence { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public System.Drawing.PointF Center()
        {
            return new System.Drawing.PointF((float)(X + Width / 2), (float)(Y + Height / 2));
        }
    }

    public class AIResult
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double OriginX { get; set; }
        public double OriginY { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public bool Result { get; set; }
        public double Score { get; set; }
        public string Name { get; set; }
    }
}
