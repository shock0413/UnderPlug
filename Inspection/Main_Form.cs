using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Cameras;
using Cognex.VisionPro;
using Cognex.VisionPro.CalibFix;
using Hansero.IOControl;
using Hansero.VisionLib.VisionPro;
using Utilities;
using System.Threading;
using Hansero;
using Microsoft.VisualBasic;
using System.IO.Ports;
using AsyncSocket;
using System.Net;
using Tools;
using PLC;
using static PLC.PLCManager;
using System.Drawing.Imaging;
using Cognex.VisionPro.ImageProcessing;
using System.Security;
using System.Security.Permissions;

namespace Inspection
{
    public partial class Main_Form : Form
    {
        //LogManager
        LogManager logManager = new LogManager(true, true);

        // 조명 컨트롤러
        HSRLEDControl m_LedControler = new HSRLEDControl();
        ILightInterface jtech_LED = new JTECH_LED();

        // 패턴검사 툴
        private Hansero.VisionLib.VisionPro.PMAlgin m_PMAlgin = new Hansero.VisionLib.VisionPro.PMAlgin();
        private Hansero.VisionLib.VisionPro.PMAlgin m_PMAlginLeft = new Hansero.VisionLib.VisionPro.PMAlgin();
        private Hansero.VisionLib.VisionPro.PMAlgin m_PMAlginRight = new Hansero.VisionLib.VisionPro.PMAlgin();

        // 검사결과
        private string m_ResultDay = "";
        private string m_ResultTime = "";

        private int original_direction = 0;

        PylonCameraManager cameraManager = new PylonCameraManager();

        //// 보정치
        //private double m_mpp_X, m_mpp_Y;

        // 검사정보
        private string m_Model = "model1";
        private string m_Direction = "";

        // 검사정보(INI 파일)
        private IniFile m_Config = new IniFile(Application.StartupPath + "\\Config.ini");

        // 비전 보정값(INI 파일)
        private IniFile m_CorrectConfig = new IniFile(Application.StartupPath + "\\Correct.ini");

        // 생산량
        private int m_TotalCnt = 0;
        private int m_OKCnt = 0;

        //결과
        private bool m_result;
        private string m_bodyNumber;
        private string m_hangerNumber;

        Stopwatch mainSW = new Stopwatch();
        Stopwatch robotSW = new Stopwatch();

        // HDD 클리어 타임
        private DateTime m_HDDClearTime;

        PLCManager plcManager;

        private AsyncSocketServer server;
        private AsyncSocketClient client;

        private AsyncSocketServer remoteDBServer;
        private AsyncSocketClient remoteDBClient;


        private AsyncSocketServer remoteImageServer;
        private AsyncSocketClient remoteImageClient;

        private AsyncDetectSocket.AsyncSocketServer detectServer = null;
        private AsyncDetectSocket.AsyncSocketClient detectClient = null;

        private AsyncSocketClient dbSocket;

        private AsyncSocketClient imageSocket;


        private bool isUseRemote;

        private int id;

        private bool[] visionResult = new bool[10];
        private StructVisionData[] visionDatas = new StructVisionData[10];
        private bool[] verifyResult = new bool[10];

        DBManager.DBManager dbManager = new DBManager.DBManager();

        private bool isInspection = false;

        /// <summary>
        /// 이전촬영시의 비전 인덱스
        /// </summary>
        private int beforeCaptureIndex = -1;
        /// <summary>
        /// 비전 Capture Index 0부터 시작
        /// </summary>
        private int visionCaptureIndex = -1;

        bool heartBeat = false;

        int visionReset = 0;

        List<AsyncSocketClient> listClient = new List<AsyncSocketClient>();

        public VisionResultData[] visionResultDatas = new VisionResultData[10];

        public double savedHoleDetectionX;

        public double savedHoleDetectionY;

        private string lastClearDate = "";

        private bool[] isReshotRequest = new bool[10];

        public Main_Form()
        {
            InitializeComponent();

            lastClearDate = m_Config.GetString("HDDClear", "LastClearDate", "");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            Init_System();
            Init_PLC();
            Init_Camera();
            Init_Heartbeat();

            Init_Server();
            Init_Database();
            InitAI();

            Thread detectProcessCheckThread = new Thread(DetectProcessCheckThreadDo);
            detectProcessCheckThread.Start();
        }

        private void InitAI()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = Environment.CurrentDirectory + "\\InspectionDetector\\Detector.exe";
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInfo.CreateNoWindow = true;
            m_Process = Process.Start(processStartInfo);
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

        private void Init_System()
        {
            CogImageConvertTool tool = new CogImageConvertTool();
            tool.InputImage = new CogImage24PlanarColor(new Bitmap(Application.StartupPath + "\\" + "Temp\\" + "PreRunTemp.bmp"));
            tool.Run();

            logManager.Info("프로그램 실행");

            // Label 및 콤포넌트 초기화

            for (int i = 0; i < 10; i++)
            {
                visionResult[i] = false;
                visionDatas[i] = new StructVisionData();
            }

            // Pattern 로드
            //Load_Pattern();
            string spacePath = m_Config.GetString("Result", "Path", "");
            if (spacePath != "")
            {
                spacePath = spacePath.Substring(0, 1);
            }

            // HDD 클리어 타임 로드
            string tmpStr = m_Config.GetString("HDDClear", "LastClearTime", "");
            try
            {
                int[] tmp_datetime = new int[6];

                int i = 0;
                foreach (string tmp in tmpStr.Split(' '))
                    tmp_datetime[i++] = Int32.Parse(tmp);

                m_HDDClearTime = new DateTime(tmp_datetime[0], tmp_datetime[1], tmp_datetime[2], tmp_datetime[3], tmp_datetime[4], tmp_datetime[5]);
            }
            catch
            {
                m_HDDClearTime = DateTime.Now;
            }

            //미리 검사해서 검사시간 축소
            m_Model = "CN7";

            ICogImage tempImage = Run_Calibration(new CogImage24PlanarColor(new Bitmap("Temp\\temp.bmp")), "01");

            cogDisplay1.Image = tempImage;

            logManager.Trace("캘리브레이션 완료.");

            // 패턴 불러오기
            Load_Pattern("01");

            // 패턴 위치 찾기
            Find_Location(cogDisplay1.Image, "01");

            cogDisplay1.Image = null;
            cogDisplay1.InteractiveGraphics.Clear();
            cogDisplay1.StaticGraphics.Clear();

        }


        private void Init_PLC()
        {
            //plc 매니저 초기화
            plcManager = new PLCManager();
            //plc 이벤트 추가
            plcManager.OnAsyncTickEvent += PlcManager_OnAsyncTickEvent;
            //PLC 매니저 시작
            plcManager.Start(plcManager.PLCAddress.Output_HB);

            plcManager.PLC_WRITE(plcManager.PLCAddress.Output_ReadyComplete, 1);
            plcManager.PLC_WRITE(plcManager.PLCAddress.Output_VisionBusy, 0);
        }

        Stopwatch heartbeatSW = new Stopwatch();

        private void Init_Heartbeat()
        {

            new Thread(new ThreadStart(() =>
            {
                heartbeatSW.Start();
                while (!isFormClosing)
                {
                    if (heartbeatSW.ElapsedMilliseconds < 30000)
                    {
                        if (heartBeat)
                        {
                            plcManager.PLC_WRITE(plcManager.heartbeatAddress, 0);
                            heartBeat = false;
                        }
                        else
                        {
                            plcManager.PLC_WRITE(plcManager.heartbeatAddress, 1);
                            heartBeat = true;
                        }
                    }

                    Thread.Sleep(500);
                }
            })).Start();

        }

        private List<string> m_Process_Recv = new List<string>();
        private int detectClientId = 0;

        private Process m_Process = null;


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
        private int sendImageIndex = 0;

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

        private void Init_Server()
        {
            // 인식 엔진 프로그램 소켓 서버
            new Thread(new ThreadStart(() =>
            {
                try
                {
                    detectServer = new AsyncDetectSocket.AsyncSocketServer(9960);
                    detectServer.OnAccept += new AsyncDetectSocket.AsyncSocketAcceptEventHandler(OnAccept);
                    detectServer.OnError += new AsyncDetectSocket.AsyncSocketErrorEventHandler(OnError);
                    detectServer.OnSend += new AsyncDetectSocket.AsyncSocketSendEventHandler(OnSend);
                    detectServer.OnClose += new AsyncDetectSocket.AsyncSocketCloseEventHandler(OnClose);
                    detectServer.Listen(IPAddress.Any);
                }
                catch
                {

                }
            })).Start();

            //로봇 통신 서버
            new Thread(new ThreadStart(() =>
            {
                try
                {
                    server = new AsyncSocketServer(9988);
                    server.OnAccept += new AsyncSocketAcceptEventHandler(OnAccept);
                    server.OnError += new AsyncSocketErrorEventHandler(OnError);

                    server.Listen(IPAddress.Any);
                }
                catch (Exception ex)
                {
                    logManager.Fatal("InitServerError : " + ex.Message);
                    MessageBox.Show("서버 생성에 실패하였습니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            })).Start();

            /*
            //DB 저장 서버
            new Thread(new ThreadStart(() =>
            {
                try
                {
                    remoteDBServer = new AsyncSocketServer(9989);
                    remoteDBServer.OnAccept += new AsyncSocketAcceptEventHandler(OnRemoteDBServerAccept);
                    remoteDBServer.OnError += new AsyncSocketErrorEventHandler(OnError);

                    remoteDBServer.Listen(IPAddress.Any);
                }
                catch (Exception ex)
                {
                    logManager.Fatal("Init Image Remote Server : " + ex.Message);
                    MessageBox.Show("이미지 리모트 서버 생성에 실패하였습니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            })).Start();
            */

            /*
            //이미지 저장 서버
            new Thread(new ThreadStart(() =>
            {
                try
                {
                    remoteImageServer = new AsyncSocketServer(9990);
                    remoteImageServer.OnAccept += new AsyncSocketAcceptEventHandler(OnRemoteImageServerAccept);
                    remoteImageServer.OnError += new AsyncSocketErrorEventHandler(OnError);

                    remoteImageServer.Listen(IPAddress.Any);
                }
                catch (Exception ex)
                {
                    logManager.Fatal("Init Image Remote Server : " + ex.Message);
                    MessageBox.Show("이미지 리모트 서버 생성에 실패하였습니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            })).Start();
            */

            //소켓 종료 쓰레드
            new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    try
                    {
                        for (int i = 0; i < listClient.Count; i++)
                        {
                            AsyncSocketClient client = listClient[i];
                            if (!client.IsAliveSocket())
                            {
                                client.Close();
                                listClient.Remove(client);

                                logManager.Info(client.ID + " : 소켓 종료");
                            }
                        }
                    }
                    catch
                    {

                    }

                    Thread.Sleep(500);
                }
            })).Start();

            // isUseRemote = Convert.ToBoolean(m_Config.GetString("Image Remote", "IsUse", "False"));
            string ip = m_Config.GetString("Image Remote", "IP", "");
            dbSocket = new AsyncSocketClient(0);
            // dbSocket.Connect(ip, 9989);

            imageSocket = new AsyncSocketClient(0);
            // imageSocket.Connect(ip, 9990);
        }

        private void Init_Camera()
        {
            Console.WriteLine("Init Camera");

            string serial = m_Config.GetString("Camera", "Serial", "");

            cameraManager.Open(serial);

        }

        private void Init_Database()
        {
            dbManager.CreateAllTable();
        }

        List<double> heightList = new List<double>();

        private bool isNewBody = false;

        Stopwatch swTotalTime = new Stopwatch();

        StructResultTblData structResultTblData = null;

        int structResultTblDataIndex = 0;

        private void PlcManager_OnAsyncTickEvent(StructPLCData data)
        {
            /*
            try
            {
                if (lastClearDate != DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    if (DateTime.Now.Hour >= 2 && DateTime.Now.Hour < 3)
                    {
                        ProcessStartInfo s = new ProcessStartInfo();
                        s.FileName = Application.StartupPath + "\\" + "HddClear.exe";

                        Process.Start(s);

                        logManager.Info("HDD Clear 실행");

                        ProcessStartInfo info = new ProcessStartInfo();
                        info.CreateNoWindow = true;
                        info.WindowStyle = ProcessWindowStyle.Hidden;
                        info.FileName = AppDomain.CurrentDomain.BaseDirectory + "MySQLDataClear\\MySQLDataClear.exe";

                        Process.Start(info);

                        logManager.Info("MySQL Data Clear 실행");

                        lastClearDate = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                }
            }
            catch (Exception ex)
            {
                logManager.Info("HDD Clear 실패 : " + ex.Message);
            }
            */

            try
            {
                StructPLCData plcData = data;

                int[] inputs = new int[100];

                plcData.inputData.CopyTo(inputs, 0);

                string hanger_number = "";
                string kind_number = inputs[9].ToString();
                string body_number = "";
                string isE = "";

                string str = null;
                for (int i = 10; i < 20; i++)
                {
                    byte data1 = Convert.ToByte(inputs[i] % 256);
                    byte data2 = Convert.ToByte(inputs[i] / 256);
                    byte[] array = new byte[2];
                    array[0] = data1;
                    array[1] = data2;
                    str += Encoding.ASCII.GetString(array);
                }

                hanger_number = str.Substring(0, 4);
                kind_number = str.Substring(4, 2);
                body_number = str.Substring(4, 8);
                isE = str.Substring(12, 2);
                isE = isE.Trim();

                heartbeatSW.Restart();

                string currentModel = "";
                if (kind_number == "0A" && isE.Contains("E"))
                {
                    currentModel = m_Config.GetString("Model", kind_number.ToString() + "E", "CN7E");
                }
                else
                {
                    currentModel = m_Config.GetString("Model", kind_number.ToString(), "CN7");
                }

                if (m_Model != currentModel || m_hangerNumber != hanger_number || m_bodyNumber != body_number)
                {
                    heightList.Clear();
                }

                m_Model = currentModel;
                m_hangerNumber = hanger_number;
                m_bodyNumber = body_number;

                // 이력저장
                bool isVerifyMode = false;
                if (inputs[9] > 10)
                {
                    isVerifyMode = true;
                    inputs[9] -= 10;
                }

                Int32[] outputs = new int[100];

                plcData.outputData.CopyTo(outputs, 0);
                
                visionCaptureIndex = inputs[9] - 1;

                // 검사시간
                DateTime cur = DateTime.Now;
                m_ResultDay = cur.ToString("yyyy-MM-dd");
                m_ResultTime = cur.ToString("HHmmss");

                try
                {
                    if (hanger_number.Replace("\0", "") != "" && inputs[1] == 1)
                    {
                        /*
                        long? index = dbManager.GetResultIndex(cur, hanger_number, body_number);

                        if (index == null)
                        {
                            index = dbManager.InsertResult(cur, m_Model, hanger_number, body_number);
                            logManager.Info("이력 저장 완료 차량 정보 없음 / 신규 등록");
                            isNewBody = true;
                            swTotalTime.Reset();
                            swTotalTime.Start();

                            for(int i = 0; i < isReshotRequest.Length; i++)
                            {
                                isReshotRequest[i] = false;
                            }
                        }
                        */

                        if (structResultTblData == null)
                        {
                            structResultTblData = new StructResultTblData()
                            {
                                Index = structResultTblDataIndex++,
                                DateTime = DateTime.Now,
                                Model = m_Model,
                                Seq = m_hangerNumber,
                                BodyNumber = m_bodyNumber
                            };
                        }
                    }
                }
                catch (Exception err)
                {
                    logManager.Error(err.Message);
                }

                try
                {
                    if (!isNewBody && inputs[1] == 1)
                    {

                        isNewBody = true;
                        swTotalTime.Reset();
                        swTotalTime.Start();
                    }
                }
                catch (Exception err)
                {
                    logManager.Error(err.Message);
                }

                //클램프 풀림 기록
                try
                {
                    if (isNewBody && inputs[1] == 0)
                    {
                        sendImageIndex = 0;

                        /*
                        long? index = dbManager.GetResultIndex(cur, m_hangerNumber, m_bodyNumber);
                        if (index.HasValue)
                        {
                            dbManager.InsertTotalCompleteData(index.Value, DateTime.Now, m_Model, m_hangerNumber, m_bodyNumber);
                            logManager.Info("클램프 풀림 저장 완료");
                        }
                        else
                        {
                            logManager.Info("클램프 풀림 저장 실패");
                        }
                        */

                        if (File.Exists(Environment.CurrentDirectory + "\\database.bak"))
                        {
                            try
                            {
                                List<string> lines = File.ReadAllLines(Environment.CurrentDirectory + "\\database.bak").ToList();

                                for (int i = 0; i < lines.Count; i++)
                                {
                                    List<string> line = lines[i].Split(',').ToList();

                                    string tableName = line[0];
                                    int bak_index = Convert.ToInt32(line[1]);

                                    if (structResultTblDataIndex < bak_index)
                                    {
                                        structResultTblDataIndex = bak_index;
                                    }

                                    if (tableName.ToUpper() == "RESULT_TBL")
                                    {
                                        try
                                        {
                                            DateTime datetime = Convert.ToDateTime(line[2]);
                                            string model = line[3];
                                            string seq = line[4];
                                            string bodyNumber = line[5];

                                            long lastInsertedId = dbManager.InsertResult(bak_index, datetime, model, seq, bodyNumber);

                                            if (lastInsertedId != -1)
                                            {
                                                lines.RemoveAt(i);
                                                i--;
                                            }
                                        }
                                        catch
                                        {

                                        }
                                    }
                                    else if (tableName.ToUpper() == "PART_RESULT_TBL")
                                    {
                                        try
                                        {
                                            // long? result_idx = Convert.ToInt64(line[2]);
                                            int position = Convert.ToInt32(line[3]);
                                            double visionMoveX = Convert.ToDouble(line[4]);
                                            double visionMoveY = Convert.ToDouble(line[5]);
                                            bool visionResult = Convert.ToBoolean(line[6]);
                                            DateTime datetime = Convert.ToDateTime(line[7]);
                                            string model = line[8];
                                            string seq = line[9];
                                            string bodyNumber = line[10];
                                            string visionName = line[11];
                                            string inspectionMode = line[12];
                                            long? result_idx = dbManager.GetResultIndex(datetime, seq, bodyNumber);

                                            long lastInsertedId = dbManager.InsertPartResult(bak_index, result_idx, position, visionMoveX, visionMoveY, visionResult, datetime, model, seq, bodyNumber, visionName, inspectionMode);

                                            if (lastInsertedId != -1)
                                            {
                                                lines.RemoveAt(i);
                                                i--;
                                            }
                                        }
                                        catch
                                        {

                                        }
                                    }
                                    else if (tableName.ToUpper() == "POS_TBL")
                                    {
                                        try
                                        {
                                            // long result_idx = Convert.ToInt64(line[2]);
                                            int position = Convert.ToInt32(line[3]);
                                            double detectX = Convert.ToDouble(line[4]);
                                            double detectY = Convert.ToDouble(line[5]);
                                            double detectZ = Convert.ToDouble(line[6]);
                                            double insertX = Convert.ToDouble(line[7]);
                                            double insertY = Convert.ToDouble(line[8]);
                                            double insertZ = Convert.ToDouble(line[9]);
                                            double detectJ1 = Convert.ToDouble(line[10]);
                                            double detectJ2 = Convert.ToDouble(line[11]);
                                            double detectJ3 = Convert.ToDouble(line[12]);
                                            double detectJ4 = Convert.ToDouble(line[13]);
                                            double detectJ5 = Convert.ToDouble(line[14]);
                                            double detectJ6 = Convert.ToDouble(line[15]);
                                            double insertJ1 = Convert.ToDouble(line[16]);
                                            double insertJ2 = Convert.ToDouble(line[17]);
                                            double insertJ3 = Convert.ToDouble(line[18]);
                                            double insertJ4 = Convert.ToDouble(line[19]);
                                            double insertJ5 = Convert.ToDouble(line[20]);
                                            double insertJ6 = Convert.ToDouble(line[21]);
                                            DateTime datetime = Convert.ToDateTime(line[22]);
                                            string model = line[23];
                                            string seq = line[24];
                                            string bodyNumber = line[25];
                                            string visionName = line[26];
                                            long? result_idx = dbManager.GetResultIndex(datetime, seq, bodyNumber);

                                            long lastInsertedId = dbManager.InsertPosResult(bak_index, result_idx.Value, position, detectX, detectY, detectZ, insertX, insertY, insertZ, detectJ1, detectJ2, detectJ3, detectJ4, detectJ5, detectJ6, insertJ1, insertJ2, insertJ3, insertJ4, insertJ5, insertJ6, datetime, model, seq, bodyNumber, visionName);

                                            if (lastInsertedId != -1)
                                            {
                                                lines.RemoveAt(i);
                                                i--;
                                            }
                                        }
                                        catch
                                        {

                                        }
                                    }
                                    else if (tableName.ToUpper() == "COMPLETE_TBL")
                                    {
                                        try
                                        {
                                            // long result_idx = Convert.ToInt64(line[2]);
                                            DateTime datetime = Convert.ToDateTime(line[3]);
                                            string model = line[4];
                                            string seq = line[5];
                                            string bodyNumber = line[6];
                                            long? result_idx = dbManager.GetResultIndex(datetime, seq, bodyNumber);

                                            long lastInsertedId = dbManager.InsertTotalCompleteData(bak_index, result_idx.Value, datetime, model, seq, bodyNumber);

                                            if (lastInsertedId != -1)
                                            {
                                                lines.RemoveAt(i);
                                                i--;
                                            }
                                        }
                                        catch
                                        {

                                        }
                                    }
                                }

                                File.WriteAllLines(Environment.CurrentDirectory + "\\database.bak", lines.ToArray());

                                structResultTblDataIndex++;
                            }
                            catch
                            {

                            }
                        }

                        // 클램프 풀림 기록 시 검사 정보 데이터베이스 저장
                        if (structResultTblData != null)
                        {
                            structResultTblData.CompelteTblData = new StructCompelteTblData()
                            {
                                DateTime = DateTime.Now,
                                BodyNumber = m_bodyNumber,
                                Model = m_Model,
                                Seq = m_hangerNumber
                            };

                            try
                            {
                                dbManager.InsertResult(structResultTblData.Index, structResultTblData.DateTime, structResultTblData.Model, structResultTblData.Seq, structResultTblData.BodyNumber);

                                long? index = dbManager.GetResultIndex(structResultTblData.DateTime, structResultTblData.Seq, structResultTblData.BodyNumber);

                                for (int i = 0; i < structResultTblData.PartResultTblDataList.Count; i++)
                                {
                                    StructPartResultTblData partResultTblData = structResultTblData.PartResultTblDataList[i];

                                    if (index != null && index.HasValue)
                                    {
                                        dbManager.InsertPartResult(structResultTblData.Index, index, partResultTblData.Position, partResultTblData.VisionMoveX, partResultTblData.VisionMoveY, partResultTblData.VisionResult, partResultTblData.DateTime, partResultTblData.Model, partResultTblData.Seq, partResultTblData.BodyNumber, partResultTblData.VisionName, partResultTblData.InspectionMode);
                                    }
                                    else
                                    {
                                        dbManager.DataBackup("Part_Result_tbl", structResultTblData.Index, "", partResultTblData.Position.ToString(), partResultTblData.VisionMoveX.ToString(), partResultTblData.VisionMoveY.ToString(), partResultTblData.VisionResult.ToString(), partResultTblData.DateTime.ToString("yyyy-MM-dd HH:mm:ss"), partResultTblData.Model, partResultTblData.Seq, partResultTblData.BodyNumber, partResultTblData.VisionName, partResultTblData.InspectionMode);
                                    }
                                }

                                for (int i = 0; i < structResultTblData.PosTblDataList.Count; i++)
                                {
                                    StructPosTblData posTblData = structResultTblData.PosTblDataList[i];

                                    if (index != null && index.HasValue)
                                    {
                                        dbManager.InsertPosResult(structResultTblData.Index, index.Value, posTblData.Position, posTblData.DetectX, posTblData.DetectY, posTblData.DetectZ, posTblData.InsertX, posTblData.InsertY, posTblData.InsertZ, posTblData.DateTime, posTblData.Model, posTblData.Seq, posTblData.BodyNumber, posTblData.VisionName, posTblData.DetectJ1, posTblData.DetectJ2, posTblData.DetectJ3, posTblData.DetectJ4, posTblData.DetectJ5, posTblData.DetectJ6, posTblData.InsertJ1, posTblData.InsertJ2, posTblData.InsertJ3, posTblData.InsertJ4, posTblData.InsertJ5, posTblData.InsertJ6, posTblData.ValidX, posTblData.ValidY, posTblData.ValidZ, posTblData.ValidJ1, posTblData.ValidJ2, posTblData.ValidJ3, posTblData.ValidJ4, posTblData.ValidJ5, posTblData.ValidJ6, posTblData.WaitX, posTblData.WaitY, posTblData.WaitZ, posTblData.WaitJ1, posTblData.WaitJ2, posTblData.WaitJ3, posTblData.WaitJ4, posTblData.WaitJ5, posTblData.WaitJ6);

                                        /*
                                        dbManager.InsertPosResult(structResultTblData.Index, index.Value, posTblData.Position, posTblData.DetectX, posTblData.DetectY, posTblData.DetectZ, posTblData.InsertX, posTblData.InsertY, posTblData.InsertZ, posTblData.DateTime, posTblData.Model, posTblData.Seq, posTblData.BodyNumber, posTblData.VisionName, posTblData.DetectJ1, posTblData.DetectJ2, posTblData.DetectJ3, posTblData.DetectJ4, posTblData.DetectJ5, posTblData.DetectJ6, posTblData.InsertJ1, posTblData.InsertJ2, posTblData.InsertJ3, posTblData.InsertJ4, posTblData.InsertJ5, posTblData.InsertJ6);
                                        */
                                    }
                                    else
                                    {
                                        dbManager.DataBackup("pos_tbl", structResultTblData.Index, "", posTblData.Position.ToString(), posTblData.DetectX.ToString(), posTblData.DetectY.ToString(), posTblData.DetectZ.ToString(), posTblData.InsertX.ToString(), posTblData.InsertY.ToString(), posTblData.InsertZ.ToString(), posTblData.DateTime.ToString("yyyy-MM-dd HH:mm:ss"), posTblData.Model, posTblData.Seq, posTblData.BodyNumber, posTblData.VisionName, posTblData.DetectJ1.ToString(), posTblData.DetectJ2.ToString(), posTblData.DetectJ3.ToString(), posTblData.DetectJ4.ToString(), posTblData.DetectJ5.ToString(), posTblData.DetectJ6.ToString(), posTblData.InsertJ1.ToString(), posTblData.InsertJ2.ToString(), posTblData.InsertJ3.ToString(), posTblData.InsertJ4.ToString(), posTblData.InsertJ5.ToString(), posTblData.InsertJ6.ToString());
                                    }
                                }

                                if (structResultTblData.CompelteTblData != null)
                                {
                                    StructCompelteTblData completeTblData = structResultTblData.CompelteTblData;

                                    if (index != null && index.HasValue)
                                    {
                                        dbManager.InsertTotalCompleteData(structResultTblData.Index, index.Value, completeTblData.DateTime, completeTblData.Model, completeTblData.Seq, completeTblData.BodyNumber);
                                    }
                                    else
                                    {
                                        dbManager.DataBackup("Complete_tbl", structResultTblData.Index, "", completeTblData.DateTime.ToString("yyyy-MM-dd HH:mm:ss"), completeTblData.Model, completeTblData.Seq, completeTblData.BodyNumber);
                                    }
                                }
                            }
                            catch
                            {

                            }
                        }

                        structResultTblData = null;

                        isNewBody = false;
                    }
                }
                catch (Exception ex)
                {
                    logManager.Fatal("행거 완료 데이터 등록 실패" + ex.Message);
                }

                if (isInspection)
                {
                    return;
                }

                //비전 시작 신호가 꺼져있을 경우
                if (inputs[2] != 1)
                {

                    return;
                }
                else
                {
                    isNewBody = true;
                }


                logManager.Trace("차종 여부 문자열 : " + str.Substring(4, 2));
                logManager.Trace("수출 여부 문자열 : " + isE);


                m_Direction = (visionCaptureIndex + 1).ToString();
                original_direction = (visionCaptureIndex + 1);
                if (isVerifyMode)
                {
                    original_direction += 100;
                }

                //////////////////////////////////////검사시작
                logManager.Info("VisionStart=============================================================");
                logManager.Info("행거 번호 : " + hanger_number);
                logManager.Info("차종 : " + kind_number);
                logManager.Info("차대 번호 : " + body_number);
                logManager.Info("수출 여부 번호 : " + isE);


                if (swTotalTime.IsRunning == false)
                {
                    swTotalTime.Start();
                }

                try
                {
                    /*
                    long? index = dbManager.GetResultIndex(cur, hanger_number, body_number);

                    if (index == null)
                    {
                        index = dbManager.InsertResult(cur, m_Model, hanger_number, body_number);
                        logManager.Info("이력 저장 완료 차량 정보 없음 / 신규 등록");
                    }
                    else
                    {
                        logManager.Trace("이력 저장 차량 정보 존재");
                    }
                    */
                    if (structResultTblData == null)
                    {
                        structResultTblData = new StructResultTblData()
                        {
                            Index = structResultTblDataIndex++,
                            DateTime = DateTime.Now,
                            Model = m_Model,
                            Seq = m_hangerNumber,
                            BodyNumber = m_bodyNumber
                        };
                    }
                }
                catch
                {
                    
                }

                isInspection = true;
                bool result = false;

                try
                {
                    plcManager.PLC_WRITE(plcManager.PLCAddress.Output_VisionBusy, 1);
                    logManager.Info("PC to PLC VisionBusy = 1 전송 완료");
                }
                catch (Exception e)
                {
                    logManager.Error("PC to PLC VisionBusy = 1 전송 실패 : " + e.Message);
                }

                bool isInspectionEnd = false;
                bool isVerifyTimeOver = false;

                // 검사
                this.Invoke(new Action(() =>
                {
                    try
                    {
                        logManager.Info("검사");

                        int currentVisionReset = visionReset;
                        visionReset++;
                        //검증 플러그 있을시 true 없으면 false
                        bool verifyResult = false;
                        bool doShot = false;

                        if (!isVerifyMode && visionCaptureIndex == 0)
                        {
                            logManager.Info("isVerifyMode = false, visionCaptureIndex == 0");
                            mainSW.Restart();
                            robotSW.Restart();
                        }

                        //위치 표시
                        lb_Kind.Text = m_Model;
                        lb_Direction.Text = m_Direction;
                        lb_BodyNumber.Text = m_bodyNumber;
                        logManager.Info("위치 표시 완료");

                        //검증시 이미지 크롭을 위한 임시 저장 홀 위치
                        savedHoleDetectionX = 0;
                        savedHoleDetectionY = 0;

                        //장착모드일시 내용 초기화
                        if (!isVerifyMode)
                        {
                            visionResultDatas[visionCaptureIndex] = new VisionResultData();
                            logManager.Info("장착모드 ON 내용 초기화");
                        }

                        Stopwatch visionCaptureSW = new Stopwatch();
                        visionCaptureSW.Start();
                        Stopwatch testSw = new Stopwatch();
                        testSw.Start();
                        logManager.Info("visionCaptureSW 시작");
                        //비전 결과값 

                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        ////검사 시작
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        //CN7일 경우 2 홀 씩 촬영
                        if (kind_number == "0A")
                        {
                            logManager.Info("OA 차종 검증");
                            if (beforeCaptureIndex + 1 != visionCaptureIndex || visionCaptureIndex % 2 == 0 || doShot)
                            {
                                verifyResult = DoInspection((Convert.ToInt32(m_Direction) + 100).ToString(), true);
                                beforeCaptureIndex = visionCaptureIndex;
                                doShot = false;
                            }
                            else
                            {
                                verifyResult = DoInspection((Convert.ToInt32(m_Direction) + 100).ToString(), false);
                            }
                        }
                        else
                        {
                            logManager.Info("OA 외 차종 검증");
                            verifyResult = DoInspection((Convert.ToInt32(m_Direction) + 100).ToString(), true);
                        }
                        visionCaptureSW.Stop();
                        if (!isVerifyMode)
                        {
                            //장착 모드일때 시간 저장
                            visionResultDatas[visionCaptureIndex].visionCaptureTime = (int)visionCaptureSW.ElapsedMilliseconds;
                        }

                        //검증상태에서 가려져서 측정 불가능시
                        if (isVerifyMode && !verifyResult)
                        {

                            bool checkResult = false;
                            DateTime startTime = DateTime.Now;
                            bool isFirstShot = false;
                            bool isFirstVerify = true;

                            while (!checkResult)
                            {
                                bool realNGCheck = false;
                                if (isFirstShot)
                                {
                                    realNGCheck = DoInspection((Convert.ToInt32(m_Direction)).ToString(), true);
                                }
                                else
                                {
                                    realNGCheck = DoInspection((Convert.ToInt32(m_Direction)).ToString(), false);
                                    isFirstShot = true;
                                }

                                logManager.Info("realNGCheck = " + realNGCheck.ToString());

                                savedHoleDetectionX = m_PMAlgin.TranslationX;
                                savedHoleDetectionY = m_PMAlgin.TranslationY;

                                //실제로 NG시가 아닐시
                                if (realNGCheck)
                                {

                                    Stopwatch verifySW = new Stopwatch();
                                    verifySW.Start();

                                    checkResult = DoInspection((Convert.ToInt32(m_Direction) + 100).ToString(), false);


                                    verifySW.Stop();
                                    if (visionResultDatas[visionCaptureIndex].maxVerifyTime > verifySW.ElapsedMilliseconds)
                                    {
                                        visionResultDatas[visionCaptureIndex].maxVerifyTime = (int)verifySW.ElapsedMilliseconds;
                                    }

                                    visionResultDatas[visionCaptureIndex].captureCount++;
                                    break;
                                }
                                else
                                {
                                    verifyResult = false;

                                    checkResult = DoInspection((Convert.ToInt32(m_Direction) + 100).ToString(), false);

                                    int verifyTime = m_Config.GetInt32("INFO", "VerifyTime", 20);
                                    int totalVerifyTime = m_Config.GetInt32("INFO", "TotalVerifyTime", 200);
                                    logManager.Info("검증 시간 : " + verifyTime);
                                    logManager.Info("현재 소요된 시간 / 검증 리미트 토탈 시간 : " + swTotalTime.ElapsedMilliseconds + "ms" + totalVerifyTime);
                                    TimeSpan timeSpan = DateTime.Now - startTime;
                                    logManager.Info("현 시간 - 시작 시간 차이 : " + timeSpan.Seconds + "초");
                                    if ((timeSpan.Seconds >= verifyTime || (swTotalTime.ElapsedMilliseconds / 1000 > totalVerifyTime)))
                                    {
                                        logManager.Fatal("검증 시간 초과 : " + verifyTime + " / " + swTotalTime.ElapsedMilliseconds / 1000);

                                        if (heartBeat)
                                        {
                                            plcManager.PLC_WRITE(plcManager.heartbeatAddress, 0);
                                            heartBeat = false;
                                        }
                                        else
                                        {
                                            plcManager.PLC_WRITE(plcManager.heartbeatAddress, 1);
                                            heartBeat = true;
                                        }

                                        isVerifyTimeOver = true;

                                        break;
                                    }
                                    isFirstVerify = false;

                                    visionResultDatas[visionCaptureIndex].captureCount++;
                                }

                                // checkResult = DoInspection((Convert.ToInt32(m_Direction) + 100).ToString(), true);
                            }

                            verifyResult = checkResult;
                            visionResultDatas[visionCaptureIndex].verifyResult = verifyResult;
                        }

                        if (isVerifyMode)
                        {
                            logManager.Info("direction " + m_Direction + " 검증 결과 : " + verifyResult);
                        }

                        this.verifyResult[visionCaptureIndex] = verifyResult;
                        bool holeResult = false;

                        //장착 모드이고 이미 장착되지 않았을때
                        if (!isVerifyMode && !verifyResult)
                        {

                            Stopwatch swInspection = new Stopwatch();
                            swInspection.Start();
                            int totalVerifyTime = m_Config.GetInt32("INFO", "TotalVerifyTime", 200);
                            bool isUseVerifyTime = m_Config.GetBool("INFO", "IsUseVerifyTime", true);

                            if (isUseVerifyTime)
                            {
                                for (int i = 0; i < 3 && !holeResult && swTotalTime.ElapsedMilliseconds / 1000 < totalVerifyTime; i++)
                                {
                                    if (i > 0)
                                    {
                                        holeResult = DoInspection(m_Direction, true);
                                    }
                                    else
                                    {
                                        holeResult = DoInspection(m_Direction, false);
                                    }

                                    logManager.Info(m_Direction + " 위치, " + (i + 1) + "번째 홀 검사 판정 결과 : " + holeResult.ToString());
                                }
                            }
                            else
                            {
                                for (int i = 0; i < 3 && !holeResult; i++)
                                {
                                    if (i > 0)
                                    {
                                        holeResult = DoInspection(m_Direction, true);
                                    }
                                    else
                                    {
                                        holeResult = DoInspection(m_Direction, false);
                                    }

                                    logManager.Info(m_Direction + " 위치, " + (i + 1) + "번째 홀 검사 판정 결과 : " + holeResult.ToString());
                                }
                            }

                            visionResult[visionCaptureIndex] = holeResult;

                            result = visionResult[visionCaptureIndex];

                            visionResultDatas[visionCaptureIndex].result = result;
                            swInspection.Stop();
                            visionResultDatas[visionCaptureIndex].visionInspectionTime = (int)swInspection.ElapsedMilliseconds;

                        }
                        else
                        {
                            if (isVerifyMode && !verifyResult)
                            {
                                result = false;
                            }
                            else
                            {
                                result = true;
                            }
                        }
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //재검사
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        if (result == false && !isVerifyTimeOver)
                        {
                            logManager.Info("재검사///////////////////////////////////");
                            Thread.Sleep(500);
                            //CN7일 경우 2 홀 씩 촬영
                            if (kind_number == "0A")
                            {
                                verifyResult = DoInspection((Convert.ToInt32(m_Direction) + 100).ToString(), true);
                                beforeCaptureIndex = visionCaptureIndex;
                                doShot = false;
                            }
                            else
                            {
                                verifyResult = DoInspection((Convert.ToInt32(m_Direction) + 100).ToString(), true);
                            }
                            visionCaptureSW.Stop();
                            if (!isVerifyMode)
                            {
                                //장착 모드일때 시간 저장
                                visionResultDatas[visionCaptureIndex].visionCaptureTime = (int)visionCaptureSW.ElapsedMilliseconds;
                            }

                            //검증상태에서 가려져서 측정 불가능시
                            if (isVerifyMode && !verifyResult)
                            {
                                bool checkResult = false;
                                DateTime startTime = DateTime.Now;
                                bool isFirstShot = false;
                                bool isFirstVerify = true;

                                while (!checkResult)
                                {
                                    bool realNGCheck = false;
                                    if (isFirstShot)
                                    {
                                        realNGCheck = DoInspection((Convert.ToInt32(m_Direction)).ToString(), true);
                                    }
                                    else
                                    {
                                        realNGCheck = DoInspection((Convert.ToInt32(m_Direction)).ToString(), false);
                                        isFirstShot = true;
                                    }

                                    savedHoleDetectionX = m_PMAlgin.TranslationX;
                                    savedHoleDetectionY = m_PMAlgin.TranslationY;

                                    //실제로 NG시가 아닐시
                                    if (realNGCheck)
                                    {

                                        Stopwatch verifySW = new Stopwatch();
                                        verifySW.Start();

                                        checkResult = DoInspection((Convert.ToInt32(m_Direction) + 100).ToString(), false);


                                        verifySW.Stop();
                                        if (visionResultDatas[visionCaptureIndex].maxVerifyTime > verifySW.ElapsedMilliseconds)
                                        {
                                            visionResultDatas[visionCaptureIndex].maxVerifyTime = (int)verifySW.ElapsedMilliseconds;
                                        }

                                        visionResultDatas[visionCaptureIndex].captureCount++;
                                        break;
                                    }
                                    else
                                    {
                                        verifyResult = false;

                                        checkResult = DoInspection((Convert.ToInt32(m_Direction) + 100).ToString(), false);

                                        int verifyTime = m_Config.GetInt32("INFO", "VerifyTime", 20);
                                        int totalVerifyTime = m_Config.GetInt32("INFO", "TotalVerifyTime", 200);
                                        logManager.Info("검증 시간 : " + verifyTime);
                                        logManager.Info("현재 소요된 시간 / 검증 리미트 토탈 시간 : " + swTotalTime.ElapsedMilliseconds + "ms" + totalVerifyTime);
                                        TimeSpan timeSpan = DateTime.Now - startTime;
                                        if ((timeSpan.Seconds >= verifyTime ||(swTotalTime.ElapsedMilliseconds / 1000 > totalVerifyTime)))
                                        {
                                            if (heartBeat)
                                            {
                                                plcManager.PLC_WRITE(plcManager.heartbeatAddress, 0);
                                                heartBeat = false;
                                            }
                                            else
                                            {
                                                plcManager.PLC_WRITE(plcManager.heartbeatAddress, 1);
                                                heartBeat = true;
                                            }

                                            break;
                                        }
                                        isFirstVerify = false;

                                        visionResultDatas[visionCaptureIndex].captureCount++;
                                    }

                                    checkResult = DoInspection((Convert.ToInt32(m_Direction) + 100).ToString(), true);
                                }

                                verifyResult = checkResult;
                                visionResultDatas[visionCaptureIndex].verifyResult = verifyResult;
                            }


                            this.verifyResult[visionCaptureIndex] = verifyResult;
                            holeResult = false;

                            //장착 모드이고 이미 장착되지 않았을때
                            if (!isVerifyMode && !verifyResult)
                            {

                                Stopwatch swInspection = new Stopwatch();
                                swInspection.Start();
                                int totalVerifyTime = m_Config.GetInt32("INFO", "TotalVerifyTime", 200);
                                bool isUseVerifyTime = m_Config.GetBool("INFO", "IsUseVerifyTime", true);

                                if (isUseVerifyTime)
                                {
                                    for (int i = 0; i < 3 && !holeResult && swTotalTime.ElapsedMilliseconds / 1000 < totalVerifyTime; i++)
                                    {
                                        if (i > 0)
                                        {
                                            holeResult = DoInspection(m_Direction, true);
                                        }
                                        else
                                        {
                                            holeResult = DoInspection(m_Direction, false);
                                        }

                                        logManager.Info(m_Direction + " 위치, " + (i + 1) + "번째 홀 검사 판정 결과 : " + holeResult.ToString());
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < 3 && !holeResult; i++)
                                    {
                                        if (i > 0)
                                        {
                                            holeResult = DoInspection(m_Direction, true);
                                        }
                                        else
                                        {
                                            holeResult = DoInspection(m_Direction, false);
                                        }

                                        logManager.Info(m_Direction + " 위치, " + (i + 1) + "번째 홀 검사 판정 결과 : " + holeResult.ToString());
                                    }
                                }

                                visionResult[visionCaptureIndex] = holeResult;

                                result = visionResult[visionCaptureIndex];

                                visionResultDatas[visionCaptureIndex].result = result;
                                swInspection.Stop();
                                visionResultDatas[visionCaptureIndex].visionInspectionTime = (int)swInspection.ElapsedMilliseconds;

                            }
                            else
                            {
                                if (isVerifyMode && !verifyResult)
                                {
                                    result = false;
                                }
                                else
                                {
                                    result = true;
                                }
                            }
                        }

                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        ////검사 종료
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        testSw.Stop();
                        logManager.Info("실제 검사 판정 소요 시간 : " + testSw.ElapsedMilliseconds + "ms");

                        if (result)
                        {
                            lb_Result.ForeColor = Color.Green;
                            lb_Result.Text = "OK";

                            m_result = true;
                            result = true;

                            visionResultDatas[visionCaptureIndex].moveX = visionDatas[visionCaptureIndex].moveX;
                            visionResultDatas[visionCaptureIndex].moveY = visionDatas[visionCaptureIndex].moveY;
                            visionResultDatas[visionCaptureIndex].inspectionMode = visionDatas[visionCaptureIndex].inspectionMode;
                        }
                        else
                        {
                            lb_Result.ForeColor = Color.Red;
                            lb_Result.Text = "NG";
                            result = false;
                            m_result = false;
                        }

                        visionResultDatas[visionCaptureIndex].isCompleted = true;

                        try
                        {
                            new Thread(new ThreadStart(() =>
                            {
                                double cropX = m_PMAlgin.TranslationX;
                                double cropY = m_PMAlgin.TranslationY;
                                if (isVerifyMode && m_result == false)
                                {
                                    cropX = savedHoleDetectionX;
                                    cropY = savedHoleDetectionY;
                                }

                                double cropWidth = m_Config.GetDouble("Image Remote", "CropWidth", 100);
                                double cropHeight = m_Config.GetDouble("Image Remote", "CropHeight", 100);

                                CogCopyRegionTool cropTool = new CogCopyRegionTool();
                                cropTool.InputImage = cogDisplay1.Image;
                                CogRectangle rec = new CogRectangle();
                                rec.SetCenterWidthHeight(cropX, cropY, cropWidth, cropHeight);
                                cropTool.Region = rec;
                                cropTool.Run();

                                bool isRemote = Convert.ToBoolean(m_Config.GetString("Image Remote", "IsUse", "False"));
                                if (isRemote)
                                {
                                    logManager.Info("검사 이미지 원격 전송 모드");
                                    SaveJPGImage(cropTool.OutputImage);
                                }
                                else
                                {
                                    logManager.Info("검사 이미지 일반 저장 모드");
                                    SaveJPGImage(cropTool.OutputImage);
                                }
                            })).Start();

                        }
                        catch
                        {
                            logManager.Error("검사 이미지 저장 실패");
                        }

                        //DB 저장
                        try
                        {
                            string visionName = m_Config.GetString("INFO", "Vision Name", "LH");

                            if (isUseRemote)
                            {
                                string command = "DB";
                                string hangerNumber = m_hangerNumber;
                                string bodyNumber = m_bodyNumber;
                                string model = m_Model;
                                string datetime = cur.Ticks.ToString();
                                string originalDirection = original_direction.ToString();
                                string moveX = visionResultDatas[visionCaptureIndex].moveX.ToString();
                                string moveY = visionResultDatas[visionCaptureIndex].moveY.ToString();

                                //내용 전달
                                string sendData = command + ",";
                                sendData += hangerNumber + ",";
                                sendData += bodyNumber + ",";
                                sendData += model + ",";
                                sendData += datetime + ",";
                                sendData += originalDirection + ",";
                                sendData += moveX + ",";
                                sendData += moveY + ",";
                                sendData += m_result + ",";
                                sendData += visionName + ",";

                                if (!dbSocket.IsAliveSocket())
                                {
                                    string ip = m_Config.GetString("Image Remote", "IP", "");
                                    dbSocket = new AsyncSocketClient(0);
                                    dbSocket.Connect(ip, 9989);
                                }

                                dbSocket.Send(Encoding.ASCII.GetBytes(sendData));

                            }
                            else
                            {
                                new Thread(new ThreadStart(() =>
                                {
                                    long? index = null;

                                    try
                                    {
                                        /*
                                        index = dbManager.GetResultIndex(cur, m_hangerNumber, m_bodyNumber);
                                        if (index == null)
                                        {
                                            index = dbManager.InsertResult(cur, m_Model, m_hangerNumber, m_bodyNumber);
                                            logManager.Info("이력 저장 완료 차량 정보 없음 / 신규 등록");
                                        }
                                        else
                                        {

                                            logManager.Trace("이력 저장 차량 정보 존재");
                                        }
                                        */

                                        if (structResultTblData == null)
                                        {
                                            structResultTblData = new StructResultTblData()
                                            {
                                                DateTime = DateTime.Now,
                                                Index = structResultTblDataIndex++,
                                                Model = m_Model,
                                                Seq = m_hangerNumber,
                                                BodyNumber = m_bodyNumber
                                            };
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        logManager.Error("결과 저장 실패 : " + ex.Message);
                                    }

                                    try
                                    {
                                        logManager.Info("부분 결과 저장 성공");
                                    }
                                    catch (Exception ex)
                                    {
                                        logManager.Error("부분 결과 저장 실패 : " + ex.Message);
                                    }
                                })).Start();
                            }
                        }
                        catch (Exception ex)
                        {
                            logManager.Fatal("원격 DB 저장 실패 : " + ex.Message);
                        }

                        new Thread(new ThreadStart(() =>
                        {
                            for (int i = 0; i < 100; i++)
                            {
                                Thread.Sleep(100);
                            }

                            if (currentVisionReset == visionReset)
                            {
                                beforeCaptureIndex = -1;
                            }
                        })).Start();
                    }
                    catch (Exception e)
                    {
                        logManager.Error("비전 검사 실패 " + e.Message);
                    }

                    isInspectionEnd = true;
                }));

                while (!isInspectionEnd)
                {
                    Thread.Sleep(10);
                }

                int compose = m_Config.GetInt32("Info", "CompPose", 4030);
                compose += inputs[9] - 1;
                Console.WriteLine("완료 주소 : " + compose + " 현재 위치 : " + m_Direction);
                logManager.Info("완료 주소 : " + compose + " 현재 위치 : " + m_Direction);
                int compResult = compose;

                if (result)
                {
                    compResult += 10;
                }
                else
                {
                    compResult += 20;
                }

                if (isVerifyMode)
                {
                    compose += 30;
                    compResult += 30;
                }

                try
                {
                    int[] currentServoPosTemp = plcManager.PLC_READ(plcManager.PLCAddress.Input_Position, 1);
                    int currentServoPos = currentServoPosTemp[0] % 10;
                    logManager.Trace("현재 촬영 위치 : " + Convert.ToInt32(m_Direction) % 100);
                    logManager.Trace("현재 서보 위치 : " + currentServoPos);
                    bool isServoPosMatch = false;
                    if (currentServoPos == Convert.ToInt32(m_Direction) % 100)
                    {
                        isServoPosMatch = true;
                    }

                    if (isServoPosMatch)
                    {
                        logManager.Info("서보 위치 및 비전 위치 일치");
                        plcManager.PLC_WRITE("D" + compose, 1);
                        plcManager.PLC_WRITE("D" + compResult, 1);
                    }
                    else
                    {
                        logManager.Error("서보 위치 및 비전 위치 불일치");
                    }
                }
                catch(Exception ex)
                {
                    logManager.Fatal("현재위치 가져오기 실패" + ex.Message);
                }

                plcManager.PLC_WRITE(plcManager.PLCAddress.Output_VisionBusy, 0);

                isInspection = false;

                if (isVerifyMode)
                {
                    visionResultDatas[visionCaptureIndex].verifyTime = (int)mainSW.ElapsedMilliseconds;
                }
                else
                {
                    visionResultDatas[visionCaptureIndex].visionTime = (int)mainSW.ElapsedMilliseconds;
                }

                //DB 저장
                try
                {
                    string visionName = m_Config.GetString("INFO", "Vision Name", "LH");

                    bool isUseRemote = Convert.ToBoolean(m_Config.GetString("Image Remote", "IsUse", "False"));
                    if (isUseRemote)
                    {
                        string command = "DB";
                        string hangerNumber = m_hangerNumber;
                        string bodyNumber = m_bodyNumber;
                        string model = m_Model;
                        string datetime = cur.ToString();
                        string originalDirection = original_direction.ToString();
                        string moveX = visionResultDatas[visionCaptureIndex].moveX.ToString();
                        string moveY = visionResultDatas[visionCaptureIndex].moveX.ToString();

                        //내용 전달
                        string sendData = command + ",";
                        sendData += hangerNumber + ",";
                        sendData += bodyNumber + ",";
                        sendData += model + ",";
                        sendData += datetime + ",";
                        sendData += originalDirection + ",";
                        sendData += moveX + ",";
                        sendData += moveY + ",";
                        sendData += m_result + ",";

                        AsyncSocketClient socket = new AsyncSocketClient(0);
                        socket.Send(Encoding.ASCII.GetBytes(sendData));
                        socket.Close();
                    }
                    else
                    {
                        long? index = null;

                        try
                        {
                            /*
                            index = dbManager.GetResultIndex(cur, m_hangerNumber, m_bodyNumber);
                            if (index == null)
                            {
                                index = dbManager.InsertResult(cur, m_Model, m_hangerNumber, m_bodyNumber);
                                logManager.Info("이력 저장 완료 차량 정보 없음 / 신규 등록");
                            }
                            else
                            {

                                logManager.Trace("이력 저장 차량 정보 존재");
                            }
                            */

                            if (structResultTblData == null)
                            {
                                structResultTblData = new StructResultTblData()
                                {
                                    DateTime = DateTime.Now,
                                    Index = structResultTblDataIndex++,
                                    Model = m_Model,
                                    Seq = m_hangerNumber,
                                    BodyNumber = m_bodyNumber
                                };
                            }
                        }
                        catch (Exception ex)
                        {
                            logManager.Error("결과 저장 실패 : " + ex.Message);
                        }

                        try
                        {
                            /*
                            Stopwatch dbSw = new Stopwatch();
                            dbSw.Start();
                            dbManager.InsertPartResult(index.Value,
                                original_direction,
                                visionResultDatas[visionCaptureIndex].moveX,
                                visionResultDatas[visionCaptureIndex].moveY,
                                m_result,
                                cur,
                                m_Model,
                                m_hangerNumber,
                                m_bodyNumber,
                                visionName
                                );
                            dbSw.Stop();
                            
                            logManager.Info("DB 쓰기 소요 시간 : " + dbSw.ElapsedMilliseconds + "ms");
                            */

                            if (structResultTblData != null)
                            {
                                structResultTblData.PartResultTblDataList.Add(new StructPartResultTblData()
                                {
                                    DateTime = DateTime.Now,
                                    Model = m_Model,
                                    Seq = m_hangerNumber,
                                    BodyNumber = m_bodyNumber,
                                    Position = original_direction,
                                    VisionMoveX = visionResultDatas[visionCaptureIndex].moveX,
                                    VisionMoveY = visionResultDatas[visionCaptureIndex].moveY,
                                    VisionName = visionName,
                                    VisionResult = m_result,
                                    InspectionMode = visionResultDatas[visionCaptureIndex].inspectionMode
                                });
                            }

                            logManager.Info("부분 결과 저장 성공");
                        }
                        catch (Exception ex)
                        {
                            logManager.Error("부분 결과 저장 실패 : " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        SaveImage(m_Direction);
                    }
                    catch (Exception ex)
                    {
                        logManager.Error("이미지 저장 실패 " + ex.Message);
                    }
                })).Start();


                Console.WriteLine("검사 경과 시간 : " + mainSW.ElapsedMilliseconds);
                logManager.Info("검사 경과 시간: " + mainSW.ElapsedMilliseconds);
                mainSW.Restart();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #region 서버 관련

        private void OnAccept(object sender, AsyncSocketAcceptEventArgs e)
        {
            new Thread(new ThreadStart(() =>
            {

                try
                {
                    AsyncSocketClient worker = new AsyncSocketClient(id++, e.Worker);

                    worker.OnConnet += new AsyncSocketConnectEventHandler(OnConnet);
                    worker.OnClose += new AsyncSocketCloseEventHandler(OnClose);
                    worker.OnError += new AsyncSocketErrorEventHandler(OnError);
                    worker.OnSend += new AsyncSocketSendEventHandler(OnSend);
                    worker.OnReceive += new AsyncSocketReceiveEventHandler(OnReceive);

                    // 데이터 수신을 대기한다.
                    worker.Receive();

                    // 접속한 클라이언트를 List에 포함한다.
                    client = worker;

                    listClient.Add(client);
                }
                catch
                {

                }

            })).Start();

        }

        private void OnRemoteDBServerAccept(object sender, AsyncSocketAcceptEventArgs e)
        {
            new Thread(new ThreadStart(() =>
            {
                try
                {
                    AsyncSocketClient worker = new AsyncSocketClient(id++, e.Worker);

                    // 데이터 수신을 대기한다.
                    worker.Receive();

                    worker.OnConnet += new AsyncSocketConnectEventHandler(OnConnet);
                    worker.OnClose += new AsyncSocketCloseEventHandler(OnClose);
                    worker.OnError += new AsyncSocketErrorEventHandler(OnError);
                    worker.OnSend += new AsyncSocketSendEventHandler(OnSend);
                    worker.OnReceive += new AsyncSocketReceiveEventHandler(OnRemoteDBServerReceive);

                    // 접속한 클라이언트를 List에 포함한다.
                    remoteDBClient = worker;

                    listClient.Add(remoteDBClient);
                }
                catch
                {

                }
            })).Start();

        }

        private void OnRemoteImageServerAccept(object sender, AsyncSocketAcceptEventArgs e)
        {
            new Thread(new ThreadStart(() =>
            {
                AsyncSocketClient worker = new AsyncSocketClient(id++, e.Worker);

                // 데이터 수신을 대기한다.
                worker.Receive();

                worker.OnConnet += new AsyncSocketConnectEventHandler(OnConnet);
                worker.OnClose += new AsyncSocketCloseEventHandler(OnClose);
                worker.OnError += new AsyncSocketErrorEventHandler(OnError);
                worker.OnSend += new AsyncSocketSendEventHandler(OnSend);
                worker.OnReceive += new AsyncSocketReceiveEventHandler(OnRemoteImageServerReceive);

                // 접속한 클라이언트를 List에 포함한다.
                remoteImageClient = worker;

                listClient.Add(remoteImageClient);
            })).Start();

        }

        private void OnError(object sender, AsyncSocketErrorEventArgs e)
        {
            client = null;
        }

        private void OnRemoteDBServerReceive(object sender, AsyncSocketReceiveEventArgs e)
        {
            new Thread(new ThreadStart(() =>
            {
                try
                {
                    string header = Encoding.ASCII.GetString(e.ReceiveData, 0, e.ReceiveBytes);
                    logManager.Trace("RemoteDBServer 수신 메시지 : " + header);
                    string[] temp = header.Split(',');

                    if (temp.Length > 1)
                    {
                        string command = temp[0];

                        if (command == "DB")
                        {
                            string hangerNumber = temp[1];
                            string bodyNumber = temp[2];
                            string model = temp[3];
                            string datetimeTick = temp[4];
                            string originalDirection = temp[5];
                            string moveX = temp[6];
                            string moveY = temp[7];
                            string result = temp[8];
                            string visionName = temp[9];

                            // LH 비전 PC에서 RH 시간 받아옴. LH 기준 현재 시각 넣어 사이클 타임 적용 필요.
                            DateTime cur = new DateTime(Convert.ToInt64(datetimeTick));

                            //DB 저장
                            try
                            {
                                long? index = null;

                                try
                                {
                                    /*
                                    index = dbManager.GetResultIndex(cur, hangerNumber, bodyNumber);
                                    if (index == null)
                                    {
                                        index = dbManager.InsertResult(cur, model, hangerNumber, bodyNumber);
                                        logManager.Info("이력 저장 완료 차량 정보 없음 / 신규 등록");
                                    }
                                    else
                                    {

                                        logManager.Trace("이력 저장 차량 정보 존재");
                                    }
                                    */

                                    if (structResultTblData == null)
                                    {
                                        structResultTblData = new StructResultTblData()
                                        {
                                            DateTime = DateTime.Now,
                                            Index = structResultTblDataIndex++,
                                            Model = m_Model,
                                            Seq = m_hangerNumber,
                                            BodyNumber = m_bodyNumber
                                        };
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logManager.Error("결과 저장 실패 : " + ex.Message);
                                }

                                try
                                {
                                    /*
                                    Stopwatch dbSw = new Stopwatch();
                                    dbSw.Start();
                                    dbManager.InsertPartResult(index.Value,
                                        Convert.ToInt32(originalDirection),
                                        Convert.ToDouble(moveX),
                                        Convert.ToDouble(moveY),
                                        Convert.ToBoolean(result),
                                        cur,
                                        model,
                                        hangerNumber,
                                        bodyNumber,
                                        visionName
                                        );
                                    dbSw.Stop();
                                    
                                    logManager.Info("DB 쓰기 소요 시간 : " + dbSw.ElapsedMilliseconds + "ms");
                                    logManager.Info("원격으로 부분 결과 저장 성공");
                                    */

                                    if (structResultTblData != null)
                                    {
                                        structResultTblData.PartResultTblDataList.Add(new StructPartResultTblData()
                                        {
                                            DateTime = cur,
                                            Model = m_Model,
                                            Seq = hangerNumber,
                                            BodyNumber = bodyNumber,
                                            Position = original_direction,
                                            VisionMoveX = Convert.ToDouble(moveX),
                                            VisionMoveY = Convert.ToDouble(moveY),
                                            VisionResult = Convert.ToBoolean(result),
                                            VisionName = visionName
                                        });
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logManager.Error("원격으로 부분 결과 저장 실패 : " + ex.Message);
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                            try
                            {
                                if (temp.Length > 10)
                                {
                                    for (int i = 10; i < temp.Length; i++)
                                    {
                                        if (temp[i] == "DB")
                                        {
                                            hangerNumber = temp[i + 1];
                                            bodyNumber = temp[i + 2];
                                            model = temp[i + 3];
                                            datetimeTick = temp[i + 4];
                                            originalDirection = temp[i + 5];
                                            moveX = temp[i + 6];
                                            moveY = temp[i + 7];
                                            result = temp[i + 8];
                                            visionName = temp[i + 9];
                                            i = i + 9;

                                            // LH 비전 PC에서 RH 시간 받아옴. LH 기준 현재 시각 넣어 사이클 타임 적용 필요.
                                            cur = new DateTime(Convert.ToInt64(datetimeTick));

                                            //DB 저장
                                            try
                                            {
                                                long? index = null;

                                                try
                                                {
                                                    /*
                                                    index = dbManager.GetResultIndex(cur, hangerNumber, bodyNumber);
                                                    if (index == null)
                                                    {
                                                        index = dbManager.InsertResult(cur, model, hangerNumber, bodyNumber);
                                                        logManager.Info("이력 저장 완료 차량 정보 없음 / 신규 등록");
                                                    }
                                                    else
                                                    {

                                                        logManager.Trace("이력 저장 차량 정보 존재");
                                                    }
                                                    */

                                                    if (structResultTblData == null)
                                                    {
                                                        structResultTblData = new StructResultTblData()
                                                        {
                                                            DateTime = DateTime.Now,
                                                            Index = structResultTblDataIndex++,
                                                            Model = m_Model,
                                                            Seq = m_hangerNumber,
                                                            BodyNumber = m_bodyNumber
                                                        };
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    logManager.Error("결과 저장 실패 : " + ex.Message);
                                                }

                                                try
                                                {
                                                    /*
                                                    Stopwatch dbSw = new Stopwatch();
                                                    dbSw.Start();
                                                    dbManager.InsertPartResult(index.Value,
                                                        Convert.ToInt32(originalDirection),
                                                        Convert.ToDouble(moveX),
                                                        Convert.ToDouble(moveY),
                                                        Convert.ToBoolean(result),
                                                        cur,
                                                        model,
                                                        hangerNumber,
                                                        bodyNumber,
                                                        visionName
                                                        );
                                                    dbSw.Stop();

                                                    logManager.Info("DB 쓰기 소요 시간 : " + dbSw.ElapsedMilliseconds + "ms");
                                                    logManager.Info("원격으로 부분 결과 저장 성공");
                                                    */

                                                    if (structResultTblData != null)
                                                    {
                                                        structResultTblData.PartResultTblDataList.Add(new StructPartResultTblData()
                                                        {
                                                            DateTime = cur,
                                                            Model = m_Model,
                                                            Seq = hangerNumber,
                                                            BodyNumber = bodyNumber,
                                                            Position = original_direction,
                                                            VisionMoveX = Convert.ToDouble(moveX),
                                                            VisionMoveY = Convert.ToDouble(moveY),
                                                            VisionResult = Convert.ToBoolean(result),
                                                            VisionName = visionName
                                                        });
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    logManager.Error("원격으로 부분 결과 저장 실패 : " + ex.Message);
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                        else if (temp[i] == "HeightInfo")
                                        {
                                            hangerNumber = temp[i + 1];
                                            bodyNumber = temp[i + 2];
                                            model = temp[i + 3];
                                            datetimeTick = temp[i + 4];
                                            int pos = Convert.ToInt32(temp[5]);
                                            double detectX = Convert.ToDouble(temp[i + 6]);
                                            double detectY = Convert.ToDouble(temp[i + 7]);
                                            double detectZ = Convert.ToDouble(temp[i + 8]);
                                            double insertX = Convert.ToDouble(temp[i + 9]);
                                            double insertY = Convert.ToDouble(temp[i + 10]);
                                            double insertZ = Convert.ToDouble(temp[i + 11]);
                                            visionName = temp[12];

                                            double detect_j1 = Convert.ToDouble(temp[i + 13]);
                                            double detect_j2 = Convert.ToDouble(temp[i + 14]);
                                            double detect_j3 = Convert.ToDouble(temp[i + 15]);
                                            double detect_j4 = Convert.ToDouble(temp[i + 16]);
                                            double detect_j5 = Convert.ToDouble(temp[i + 17]);
                                            double detect_j6 = Convert.ToDouble(temp[i + 18]);
                                            double insert_j1 = Convert.ToDouble(temp[i + 19]);
                                            double insert_j2 = Convert.ToDouble(temp[i + 20]);
                                            double insert_j3 = Convert.ToDouble(temp[i + 21]);
                                            double insert_j4 = Convert.ToDouble(temp[i + 22]);
                                            double insert_j5 = Convert.ToDouble(temp[i + 23]);
                                            double insert_j6 = Convert.ToDouble(temp[i + 24]);

                                            i = i + 24;

                                            cur = new DateTime(Convert.ToInt64(datetimeTick));

                                            //DB 저장
                                            try
                                            {
                                                long? index = null;

                                                try
                                                {
                                                    /*
                                                    index = dbManager.GetResultIndex(cur, hangerNumber, bodyNumber);
                                                    if (index == null)
                                                    {
                                                        index = dbManager.InsertResult(cur, model, hangerNumber, bodyNumber);
                                                        logManager.Info("이력 저장 완료 차량 정보 없음 / 신규 등록");
                                                    }
                                                    else
                                                    {
                                                        logManager.Trace("이력 저장 차량 정보 존재");
                                                    }
                                                    */

                                                    if (structResultTblData == null)
                                                    {
                                                        structResultTblData = new StructResultTblData()
                                                        {
                                                            DateTime = DateTime.Now,
                                                            Index = structResultTblDataIndex++,
                                                            Model = m_Model,
                                                            Seq = m_hangerNumber,
                                                            BodyNumber = m_bodyNumber
                                                        };
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    logManager.Error("결과 저장 실패 : " + ex.Message);
                                                }

                                                try
                                                {
                                                    /*
                                                 dbManager.InsertPosResult(
                                                 index.Value,
                                                 pos,
                                                 detectX,
                                                 detectY,
                                                 detectZ,
                                                 insertX,
                                                 insertY,
                                                 insertZ,
                                                 detect_j1,
                                                 detect_j2,
                                                 detect_j3,
                                                 detect_j4,
                                                 detect_j5,
                                                 detect_j6,
                                                 insert_j1,
                                                 insert_j2,
                                                 insert_j3,
                                                 insert_j4,
                                                 insert_j5,
                                                 insert_j6,
                                                 cur,
                                                 m_Model,
                                                 m_hangerNumber,
                                                 m_bodyNumber,
                                                 visionName
                                                 );

                                                    logManager.Info("원격으로 높이 데이터 저장 성공");
                                                    */

                                                    if (structResultTblData != null)
                                                    {
                                                        /*
                                                        structResultTblData.PosTblDataList.Add(new StructPosTblData()
                                                        {
                                                            DateTime = cur,
                                                            Model = m_Model,
                                                            Seq = m_hangerNumber,
                                                            BodyNumber = m_bodyNumber,
                                                            Position = pos,
                                                            DetectX = detectX,
                                                            DetectY = detectY,
                                                            DetectZ = detectZ,
                                                            InsertX = insertX,
                                                            InsertY = insertY,
                                                            InsertZ = insertZ,
                                                            DetectJ1 = detect_j1,
                                                            DetectJ2 = detect_j2,
                                                            DetectJ3 = detect_j3,
                                                            DetectJ4 = detect_j4,
                                                            DetectJ5 = detect_j5,
                                                            DetectJ6 = detect_j6,
                                                            InsertJ1 = insert_j1,
                                                            InsertJ2 = insert_j2,
                                                            InsertJ3 = insert_j3,
                                                            InsertJ4 = insert_j4,
                                                            InsertJ5 = insert_j5,
                                                            InsertJ6 = insert_j6,
                                                            VisionName = visionName
                                                        });
                                                        */
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    logManager.Error("원격으로 높이 데이터 저장 실패 : " + ex.Message);
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {

                            }
                        }
                        else if (command == "HeightInfo")
                        {
                            string hangerNumber = temp[1];
                            string bodyNumber = temp[2];
                            string model = temp[3];
                            string datetimeTick = temp[4];
                            int pos = Convert.ToInt32(temp[5]);
                            double detectX = Convert.ToDouble(temp[6]);
                            double detectY = Convert.ToDouble(temp[7]);
                            double detectZ = Convert.ToDouble(temp[8]);
                            double insertX = Convert.ToDouble(temp[9]);
                            double insertY = Convert.ToDouble(temp[10]);
                            double insertZ = Convert.ToDouble(temp[11]);
                            string visionName = temp[12];

                            double detect_j1 = Convert.ToDouble(temp[13]);
                            double detect_j2 = Convert.ToDouble(temp[14]);
                            double detect_j3 = Convert.ToDouble(temp[15]);
                            double detect_j4 = Convert.ToDouble(temp[16]);
                            double detect_j5 = Convert.ToDouble(temp[17]);
                            double detect_j6 = Convert.ToDouble(temp[18]);
                            double insert_j1 = Convert.ToDouble(temp[19]);
                            double insert_j2 = Convert.ToDouble(temp[20]);
                            double insert_j3 = Convert.ToDouble(temp[21]);
                            double insert_j4 = Convert.ToDouble(temp[22]);
                            double insert_j5 = Convert.ToDouble(temp[23]);
                            double insert_j6 = Convert.ToDouble(temp[24]);

                            DateTime cur = new DateTime(Convert.ToInt64(datetimeTick));

                            //DB 저장
                            try
                            {
                                long? index = null;

                                try
                                {
                                    /*
                                    index = dbManager.GetResultIndex(cur, hangerNumber, bodyNumber);
                                    if (index == null)
                                    {
                                        index = dbManager.InsertResult(cur, model, hangerNumber, bodyNumber);
                                        logManager.Info("이력 저장 완료 차량 정보 없음 / 신규 등록");
                                    }
                                    else
                                    {
                                        logManager.Trace("이력 저장 차량 정보 존재");
                                    }
                                    */

                                    if (structResultTblData == null)
                                    {
                                        structResultTblData = new StructResultTblData()
                                        {
                                            DateTime = DateTime.Now,
                                            Index = structResultTblDataIndex++,
                                            Model = m_Model,
                                            Seq = m_hangerNumber,
                                            BodyNumber = m_bodyNumber
                                        };
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logManager.Error("결과 저장 실패 : " + ex.Message);
                                }

                                try
                                {
                                    /*
                                    dbManager.InsertPosResult(
                                 index.Value,
                                 pos,
                                 detectX,
                                 detectY,
                                 detectZ,
                                 insertX,
                                 insertY,
                                 insertZ,
                                 detect_j1,
                                 detect_j2,
                                 detect_j3,
                                 detect_j4,
                                 detect_j5,
                                 detect_j6,
                                 insert_j1,
                                 insert_j2,
                                 insert_j3,
                                 insert_j4,
                                 insert_j5,
                                 insert_j6,
                                 cur,
                                 m_Model,
                                 m_hangerNumber,
                                 m_bodyNumber,
                                 visionName
                                 );

                                    logManager.Info("원격으로 높이 데이터 저장 성공");
                                    */

                                    if (structResultTblData != null)
                                    {
                                        /*
                                        structResultTblData.PosTblDataList.Add(new StructPosTblData()
                                        {
                                            DateTime = cur,
                                            Model = m_Model,
                                            Seq = m_hangerNumber,
                                            BodyNumber = m_bodyNumber,
                                            Position = pos,
                                            DetectX = detectX,
                                            DetectY = detectY,
                                            DetectZ = detectZ,
                                            InsertX = insertX,
                                            InsertY = insertY,
                                            InsertZ = insertZ,
                                            DetectJ1 = detect_j1,
                                            DetectJ2 = detect_j2,
                                            DetectJ3 = detect_j3,
                                            DetectJ4 = detect_j4,
                                            DetectJ5 = detect_j5,
                                            DetectJ6 = detect_j6,
                                            InsertJ1 = insert_j1,
                                            InsertJ2 = insert_j2,
                                            InsertJ3 = insert_j3,
                                            InsertJ4 = insert_j4,
                                            InsertJ5 = insert_j5,
                                            InsertJ6 = insert_j6,
                                            VisionName = visionName
                                        });
                                        */
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logManager.Error("원격으로 높이 데이터 저장 실패 : " + ex.Message);
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                            try
                            {
                                if (temp.Length > 25)
                                {
                                    for (int i = 25; i < temp.Length; i++)
                                    {
                                        if (temp[i] == "DB")
                                        {
                                            hangerNumber = temp[i + 1];
                                            bodyNumber = temp[i + 2];
                                            model = temp[i + 3];
                                            datetimeTick = temp[i + 4];
                                            string originalDirection = temp[i + 5];
                                            string moveX = temp[i + 6];
                                            string moveY = temp[i + 7];
                                            string result = temp[i + 8];
                                            visionName = temp[i + 9];
                                            i = i + 9;

                                            // LH 비전 PC에서 RH 시간 받아옴. LH 기준 현재 시각 넣어 사이클 타임 적용 필요.
                                            cur = new DateTime(Convert.ToInt64(datetimeTick));

                                            //DB 저장
                                            try
                                            {
                                                long? index = null;

                                                try
                                                {
                                                    /*
                                                    index = dbManager.GetResultIndex(cur, hangerNumber, bodyNumber);
                                                    if (index == null)
                                                    {
                                                        index = dbManager.InsertResult(cur, model, hangerNumber, bodyNumber);
                                                        logManager.Info("이력 저장 완료 차량 정보 없음 / 신규 등록");
                                                    }
                                                    else
                                                    {

                                                        logManager.Trace("이력 저장 차량 정보 존재");
                                                    }
                                                    */

                                                    if (structResultTblData == null)
                                                    {
                                                        structResultTblData = new StructResultTblData()
                                                        {
                                                            DateTime = DateTime.Now,
                                                            Index = structResultTblDataIndex++,
                                                            Model = m_Model,
                                                            Seq = m_hangerNumber,
                                                            BodyNumber = m_bodyNumber
                                                        };
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    logManager.Error("결과 저장 실패 : " + ex.Message);
                                                }

                                                try
                                                {
                                                    /*
                                                    Stopwatch dbSw = new Stopwatch();
                                                    dbSw.Start();
                                                    dbManager.InsertPartResult(index.Value,
                                                        Convert.ToInt32(originalDirection),
                                                        Convert.ToDouble(moveX),
                                                        Convert.ToDouble(moveY),
                                                        Convert.ToBoolean(result),
                                                        cur,
                                                        model,
                                                        hangerNumber,
                                                        bodyNumber,
                                                        visionName
                                                        );
                                                    dbSw.Stop();

                                                    Thread.Sleep(500);

                                                    logManager.Info("DB 쓰기 소요 시간 : " + dbSw.ElapsedMilliseconds + "ms");
                                                    logManager.Info("원격으로 부분 결과 저장 성공");
                                                    */

                                                    if (structResultTblData != null)
                                                    {
                                                        structResultTblData.PartResultTblDataList.Add(new StructPartResultTblData()
                                                        {
                                                            DateTime = cur,
                                                            Model = m_Model,
                                                            Seq = hangerNumber,
                                                            BodyNumber = bodyNumber,
                                                            Position = original_direction,
                                                            VisionMoveX = Convert.ToDouble(moveX),
                                                            VisionMoveY = Convert.ToDouble(moveY),
                                                            VisionResult = Convert.ToBoolean(result),
                                                            VisionName = visionName
                                                        });
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    logManager.Error("원격으로 부분 결과 저장 실패 : " + ex.Message);
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                        else if (temp[i] == "HeightInfo")
                                        {
                                            hangerNumber = temp[i + 1];
                                            bodyNumber = temp[i + 2];
                                            model = temp[i + 3];
                                            datetimeTick = temp[i + 4];
                                            pos = Convert.ToInt32(temp[5]);
                                            detectX = Convert.ToDouble(temp[i + 6]);
                                            detectY = Convert.ToDouble(temp[i + 7]);
                                            detectZ = Convert.ToDouble(temp[i + 8]);
                                            insertX = Convert.ToDouble(temp[i + 9]);
                                            insertY = Convert.ToDouble(temp[i + 10]);
                                            insertZ = Convert.ToDouble(temp[i + 11]);
                                            visionName = temp[12];

                                            detect_j1 = Convert.ToDouble(temp[i + 13]);
                                            detect_j2 = Convert.ToDouble(temp[i + 14]);
                                            detect_j3 = Convert.ToDouble(temp[i + 15]);
                                            detect_j4 = Convert.ToDouble(temp[i + 16]);
                                            detect_j5 = Convert.ToDouble(temp[i + 17]);
                                            detect_j6 = Convert.ToDouble(temp[i + 18]);
                                            insert_j1 = Convert.ToDouble(temp[i + 19]);
                                            insert_j2 = Convert.ToDouble(temp[i + 20]);
                                            insert_j3 = Convert.ToDouble(temp[i + 21]);
                                            insert_j4 = Convert.ToDouble(temp[i + 22]);
                                            insert_j5 = Convert.ToDouble(temp[i + 23]);
                                            insert_j6 = Convert.ToDouble(temp[i + 24]);

                                            i = i + 24;

                                            cur = new DateTime(Convert.ToInt64(datetimeTick));

                                            //DB 저장
                                            try
                                            {
                                                long? index = null;

                                                try
                                                {
                                                    /*
                                                    index = dbManager.GetResultIndex(cur, hangerNumber, bodyNumber);
                                                    if (index == null)
                                                    {
                                                        index = dbManager.InsertResult(cur, model, hangerNumber, bodyNumber);
                                                        logManager.Info("이력 저장 완료 차량 정보 없음 / 신규 등록");
                                                    }
                                                    else
                                                    {
                                                        logManager.Trace("이력 저장 차량 정보 존재");
                                                    }
                                                    */

                                                    if (structResultTblData == null)
                                                    {
                                                        structResultTblData = new StructResultTblData()
                                                        {
                                                            DateTime = DateTime.Now,
                                                            Index = structResultTblDataIndex++,
                                                            Model = m_Model,
                                                            Seq = m_hangerNumber,
                                                            BodyNumber = m_bodyNumber
                                                        };
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    logManager.Error("결과 저장 실패 : " + ex.Message);
                                                }

                                                try
                                                {
                                                    /*
                                                    dbManager.InsertPosResult(
                                                 index.Value,
                                                 pos,
                                                 detectX,
                                                 detectY,
                                                 detectZ,
                                                 insertX,
                                                 insertY,
                                                 insertZ,
                                                 detect_j1,
                                                 detect_j2,
                                                 detect_j3,
                                                 detect_j4,
                                                 detect_j5,
                                                 detect_j6,
                                                 insert_j1,
                                                 insert_j2,
                                                 insert_j3,
                                                 insert_j4,
                                                 insert_j5,
                                                 insert_j6,
                                                 cur,
                                                 m_Model,
                                                 m_hangerNumber,
                                                 m_bodyNumber,
                                                 visionName
                                                 );

                                                    Thread.Sleep(500);

                                                    logManager.Info("원격으로 높이 데이터 저장 성공");
                                                    */

                                                    if (structResultTblData != null)
                                                    {
                                                        /*
                                                        structResultTblData.PosTblDataList.Add(new StructPosTblData()
                                                        {
                                                            DateTime = cur,
                                                            Model = m_Model,
                                                            Seq = m_hangerNumber,
                                                            BodyNumber = m_bodyNumber,
                                                            Position = pos,
                                                            DetectX = detectX,
                                                            DetectY = detectY,
                                                            DetectZ = detectZ,
                                                            InsertX = insertX,
                                                            InsertY = insertY,
                                                            InsertZ = insertZ,
                                                            DetectJ1 = detect_j1,
                                                            DetectJ2 = detect_j2,
                                                            DetectJ3 = detect_j3,
                                                            DetectJ4 = detect_j4,
                                                            DetectJ5 = detect_j5,
                                                            DetectJ6 = detect_j6,
                                                            InsertJ1 = insert_j1,
                                                            InsertJ2 = insert_j2,
                                                            InsertJ3 = insert_j3,
                                                            InsertJ4 = insert_j4,
                                                            InsertJ5 = insert_j5,
                                                            InsertJ6 = insert_j6,
                                                            VisionName = visionName
                                                        });
                                                        */
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    logManager.Error("원격으로 높이 데이터 저장 실패 : " + ex.Message);
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {

                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    logManager.Error("원격으로 부분 결과 저장 실패 : " + ex.Message);
                }

            })).Start();
        }

        private void OnRemoteImageServerReceive(object sender, AsyncSocketReceiveEventArgs e)
        {
            new Thread(new ThreadStart(() =>
            {
                try
                {
                    string header = Encoding.ASCII.GetString(e.ReceiveData, 0, e.ReceiveBytes);
                    string[] temp = header.Split(',');

                    if (temp.Length > 1)
                    {
                        string command = temp[0];
                        string path = temp[1];

                        int imageStartLength = Encoding.ASCII.GetBytes(command).Length + Encoding.ASCII.GetBytes(path).Length + Encoding.ASCII.GetBytes("\\").Length + Encoding.ASCII.GetBytes("\\").Length;

                        string bitmapStr = header.Substring(command.Length + path.Length + 2);

                        byte[] bitmapData = Encoding.ASCII.GetBytes(temp[2]);

                        if (command == "Image")
                        {
                            Bitmap bitmap = null;
                            using (var ms = new MemoryStream(e.ReceiveData, imageStartLength, e.ReceiveBytes - imageStartLength))
                            {
                                byte[] aa = ms.ToArray();
                                bitmap = new Bitmap(ms);

                                try
                                {
                                    string dir = Path.GetDirectoryName(path);

                                    if (!Directory.Exists(dir))
                                    {
                                        Directory.CreateDirectory(dir);
                                    }

                                    bitmap.Save(path);
                                    logManager.Info("원격 검사 이미지 저장 성공");

                                }
                                catch (Exception ex)
                                {
                                    logManager.Error("원격 검사 이미지 저장 실패 : " + ex.Message);
                                }
                            }


                        }
                    }
                }
                catch (Exception ex)
                {
                    logManager.Error("원격으로 검사 이미지 저장 실패 : " + ex.Message);
                }
            })).Start();
        }

        private void OnReceive(object sender, AsyncSocketReceiveEventArgs e)
        {

            try
            {

                new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        string str = Encoding.ASCII.GetString(e.ReceiveData, 0, e.ReceiveBytes);
                        logManager.Info("수신된 메시지 : " + str);

                        string command = str.Split(',')[0].ToUpper();
                        if (command == "Request_Position".ToUpper())
                        {
                            //위치요구
                            int pos = Convert.ToInt32(str.Split(',')[1]) - 1;

                            bool currentVisionResult = visionResult[pos];
                            StructVisionData currentVisionData = visionDatas[pos];

                            int forceX = m_Config.GetInt32("Force", m_Model + "x" + (pos + 1).ToString(), 0);
                            int forceY = m_Config.GetInt32("Force", m_Model + "y" + (pos + 1).ToString(), 0);
                            int forceZ = m_Config.GetInt32("Force", m_Model + "z" + (pos + 1).ToString(), 0);
                            int secondForceX = m_Config.GetInt32("Second Force", m_Model + "x" + (pos + 1).ToString(), 0);
                            int secondForceY = m_Config.GetInt32("Second Force", m_Model + "y" + (pos + 1).ToString(), 0);
                            int secondForceZ = m_Config.GetInt32("Second Force", m_Model + "z" + (pos + 1).ToString(), 0);
                            int zCorrect = m_Config.GetInt32("ZCorrect", m_Model + (pos + 1).ToString(), 0);

                            int forceRX = m_Config.GetInt32("ForceR", "rx", 0);
                            int forceRY = m_Config.GetInt32("ForceR", "ry", 0);
                            int forceRZ = m_Config.GetInt32("ForceR", "rz", 0);

                            float rev = (float)m_Config.GetDouble("spirial", "rev", 2);
                            float rmax = (float)m_Config.GetDouble("spirial", "rmax", 1);
                            float vel = (float)m_Config.GetDouble("spirial", "vel", 1);
                            float acc = (float)m_Config.GetDouble("spirial", "acc", 0.5);
                            float time = (float)m_Config.GetDouble("spirial", "time", 2);

                            int isUseSpiral = m_Config.GetInt32("Spiral", (pos + 1).ToString() + "_isUse", 1);
                            int isUseRectangle = m_Config.GetInt32("Rectangle", (pos + 1).ToString() + "_IsUse", 0);
                            float rectangleDistance = (float)m_Config.GetDouble("Rectangle", (pos + 1).ToString() + "_Distance", 4.0);


                            if (pos == 2)
                            {
                                rev = (float)m_Config.GetDouble("sprial3", "rev", 2);
                                rmax = (float)m_Config.GetDouble("sprial3", "rmax", 1);
                                vel = (float)m_Config.GetDouble("sprial3", "vel", 1);
                                acc = (float)m_Config.GetDouble("sprial3", "acc", 0.5);
                                time = (float)m_Config.GetDouble("sprial3", "time", 2);
                            }
                            else if (pos == 3)
                            {
                                rev = (float)m_Config.GetDouble("sprial4", "rev", 2);
                                rmax = (float)m_Config.GetDouble("sprial4", "rmax", 1);
                                vel = (float)m_Config.GetDouble("sprial4", "vel", 1);
                                acc = (float)m_Config.GetDouble("sprial4", "acc", 0.5);
                                time = (float)m_Config.GetDouble("sprial4", "time", 2);
                            }
                            else if (pos == 4)
                            {
                                rev = (float)m_Config.GetDouble("sprial5", "rev", 2);
                                rmax = (float)m_Config.GetDouble("sprial5", "rmax", 1);
                                vel = (float)m_Config.GetDouble("sprial5", "vel", 1);
                                acc = (float)m_Config.GetDouble("sprial5", "acc", 0.5);
                                time = (float)m_Config.GetDouble("sprial5", "time", 2);
                            }
                            else if (pos == 7)
                            {
                                rev = (float)m_Config.GetDouble("sprial8", "rev", 2);
                                rmax = (float)m_Config.GetDouble("sprial8", "rmax", 1);
                                vel = (float)m_Config.GetDouble("sprial8", "vel", 1);
                                acc = (float)m_Config.GetDouble("sprial8", "acc", 0.5);
                                time = (float)m_Config.GetDouble("sprial8", "time", 2);
                            }



                            logManager.Info("스파이럴 : " + rev + "/" + rmax + "/" + vel + "/" + acc + "/");


                            string sendStr = "Request_Position,";
                            if (verifyResult[pos])
                            {
                                sendStr += "Skip,";
                                sendStr += 0 + ",";
                                sendStr += 0 + ",";
                                sendStr += forceX + ",";
                                sendStr += forceY + ",";
                                sendStr += forceZ + ",";
                                sendStr += zCorrect + ",";
                            }
                            else if (visionResult[pos])
                            {
                                sendStr += "OK,";
                                sendStr += Math.Round(currentVisionData.moveX, 2) + ",";
                                sendStr += Math.Round(currentVisionData.moveY, 2) + ",";
                                sendStr += forceX + ",";
                                sendStr += forceY + ",";
                                sendStr += forceZ + ",";
                                sendStr += zCorrect + ",";
                            }
                            else
                            {
                                sendStr += "NG,";
                                sendStr += 0 + ",";
                                sendStr += 0 + ",";
                                sendStr += forceX + ",";
                                sendStr += forceY + ",";
                                sendStr += forceZ + ",";
                                sendStr += zCorrect + ",";
                            }

                            double heightAvg = 0;

                            double heightBias = m_Config.GetDouble("INFO", "Height Bias", 50);


                            string doSkipFirstTouch = "0";
                            if (m_Config.GetString("INFO", "Height Use Insert", "False").ToUpper() == "TRUE")
                            {

                                heightBias = m_Config.GetDouble("INFO", "Height Use Insert Bias", 2);
                                if (heightList.Count == 1)
                                {
                                    logManager.Fatal("첫 장착 스킵 높이 보정 Count : " + heightList.Count);
                                    logManager.Fatal("첫 장착 스킵 높이 보정 heightBias : " + heightBias);
                                    heightAvg = (heightList[0] - heightBias);
                                    doSkipFirstTouch = "1";
                                }
                                else if (heightList.Count > 1)
                                {
                                    logManager.Fatal("첫 장착 스킵 높이 보정 Count : " + heightList.Count);
                                    logManager.Fatal("첫 장착 스킵 높이 보정 heightList[heightList.Count - 2] : " + heightList[heightList.Count - 2]);
                                    logManager.Fatal("첫 장착 스킵 높이 보정 heightList[heightList.Count - 1] : " + heightList[heightList.Count - 1]);
                                    logManager.Fatal("첫 장착 스킵 높이 보정 heightBias : " + heightBias);
                                    heightAvg = (heightList[heightList.Count - 2] * 0.1 + heightList[heightList.Count - 1] * 0.9 - heightBias);
                                    doSkipFirstTouch = "1";
                                }
                            }
                            else
                            {
                                if (heightList.Count > 1)
                                {
                                    //heightAvg가 0이면 높이 보정 안함
                                    logManager.Fatal("Count : " + heightList.Count);
                                    logManager.Fatal("heightList[heightList.Count - 2] : " + heightList[heightList.Count - 2]);
                                    logManager.Fatal("heightList[heightList.Count - 1] : " + heightList[heightList.Count - 1]);
                                    logManager.Fatal("heightBias : " + heightBias);
                                    heightAvg = heightList[heightList.Count - 2] * 0.1 + heightList[heightList.Count - 1] * 0.9 - heightBias;
                                }
                            }



                            sendStr += rev + ",";
                            sendStr += rmax + ",";
                            sendStr += vel + ",";
                            sendStr += acc + ",";
                            sendStr += time + ",";

                            sendStr += heightAvg + ",";
                            logManager.Fatal("heightAVG : " + heightAvg);

                            sendStr += forceRX + ",";
                            sendStr += forceRY + ",";
                            sendStr += forceRZ + ",";

                            sendStr += isUseSpiral + ",";
                            sendStr += isUseRectangle + ",";
                            sendStr += rectangleDistance + ",";

                            //직선 이동
                            string moveLineOn = m_Config.GetString("MoveLine" + (pos + 1), "IsUse", "False").ToUpper();
                            double moveLineX = m_Config.GetDouble("MoveLine" + (pos + 1), "X", 0);
                            double moveLineY = m_Config.GetDouble("MoveLine" + (pos + 1), "Y", 0);

                            int powerLimit = m_Config.GetInt32("INFO", "PowerLimit", 110);
                            int torque_j2_limit = m_Config.GetInt32("INFO", "Torque J2 Limit", 60);
                            int torque_j3_limit = m_Config.GetInt32("INFO", "Torque J3 Limit", 60);
                            logManager.Trace("PowerLimit : " + powerLimit);
                            logManager.Trace("Torque J2 Limit : " + torque_j2_limit);
                            logManager.Trace("Torque J3 Limit : " + torque_j3_limit);

                            if (moveLineOn == "TRUE")
                            {
                                sendStr += "1,";
                            }
                            else
                            {
                                sendStr += "0,";
                            }

                            sendStr += moveLineX + ",";
                            sendStr += moveLineY + ",";

                            sendStr += powerLimit + ",";
                            sendStr += torque_j2_limit + ",";
                            sendStr += torque_j3_limit + ",";


                            float reinsert_rev = (float)m_Config.GetDouble("Reinsert spiral" + (pos + 1), "rev", 2);
                            float reinsert_rmax = (float)m_Config.GetDouble("Reinsert spiral" + (pos + 1), "rmax", 1);
                            float reinsert_vel = (float)m_Config.GetDouble("Reinsert spiral" + (pos + 1), "vel", 1);
                            float reinsert_acc = (float)m_Config.GetDouble("Reinsert spiral" + (pos + 1), "acc", 0.5);
                            float reinsert_time = (float)m_Config.GetDouble("Reinsert spiral" + (pos + 1), "time", 2);

                            string reinsert_moveLineOn = m_Config.GetString("Reinsert MoveLine" + (pos + 1), "IsUse", "False").ToUpper();
                            double reinsert_moveLineX = m_Config.GetDouble("Reinsert MoveLine" + (pos + 1), "X", 0);
                            double reinsert_moveLineY = m_Config.GetDouble("Reinsert MoveLine" + (pos + 1), "Y", 0);

                            sendStr += reinsert_rev + ",";
                            sendStr += reinsert_rmax + ",";
                            sendStr += reinsert_vel + ",";
                            sendStr += reinsert_acc + ",";
                            sendStr += reinsert_time + ",";

                            if (reinsert_moveLineOn == "TRUE")
                            {
                                sendStr += "1,";
                            }
                            else
                            {
                                sendStr += "0,";
                            }

                            sendStr += reinsert_moveLineX + ",";
                            sendStr += reinsert_moveLineY + ",";
                            sendStr += doSkipFirstTouch + ",";

                            if (m_Config.GetString("INFO", "Use Three Time Respiral", "False").ToUpper() == "TRUE")
                            {
                                sendStr += "1,";
                            }
                            else
                            {
                                sendStr += "0,";
                            }

                            isReshotRequest[pos] = true;

                            float reSpiral2Rev = (float)m_Config.GetDouble("Reinsert2 spiral" + (pos + 1), "rev", 2);
                            float reSpiral2Rmax = (float)m_Config.GetDouble("Reinsert2 spiral" + (pos + 1), "rmax", 1);
                            float reSpiral2Time = (float)m_Config.GetDouble("Reinsert2 spiral" + (pos + 1), "time", 0.5);

                            float reSpiral3Rev = (float)m_Config.GetDouble("Reinsert3 spiral" + (pos + 1), "rev", 2);
                            float reSpiral3Rmax = (float)m_Config.GetDouble("Reinsert3 spiral" + (pos + 1), "rmax", 1);
                            float reSpiral3Time = (float)m_Config.GetDouble("Reinsert3 spiral" + (pos + 1), "time", 0.5);

                            sendStr += reSpiral2Rev + ",";
                            sendStr += reSpiral2Rmax + ",";
                            sendStr += reSpiral2Time + ",";

                            sendStr += reSpiral3Rev + ",";
                            sendStr += reSpiral3Rmax + ",";
                            sendStr += reSpiral3Time + ",";

                            string reMoveLine2On = m_Config.GetString("Reinsert2 MoveLine" + (pos + 1), "IsUse", "False").ToUpper();
                            float reMoveLine2X = (float)m_Config.GetDouble("Reinsert2 MoveLine" + (pos + 1), "X", 0);
                            float reMoveLine2Y = (float)m_Config.GetDouble("Reinsert2 MoveLine" + (pos + 1), "Y", 0);

                            string reMoveLine3On = m_Config.GetString("Reinsert3 MoveLine" + (pos + 1), "IsUse", "False").ToUpper();
                            float reMoveLine3X = (float)m_Config.GetDouble("Reinsert3 MoveLine" + (pos + 1), "X", 0);
                            float reMoveLine3Y = (float)m_Config.GetDouble("Reinsert3 MoveLine" + (pos + 1), "Y", 0);

                            if (reMoveLine2On == "TRUE")
                            {
                                sendStr += "1,";
                            }
                            else
                            {
                                sendStr += "0,";
                            }

                            sendStr += reMoveLine2X + ",";
                            sendStr += reMoveLine2Y + ",";

                            if (reMoveLine3On == "TRUE")
                            {
                                sendStr += "1,";
                            }
                            else
                            {
                                sendStr += "0,";
                            }

                            sendStr += reMoveLine3X + ",";
                            sendStr += reMoveLine3Y + ",";

                            float reOffset2X = (float)m_Config.GetDouble("Reinsert2 Offset" + (pos + 1), "X", 0);
                            float reOffset2Y = (float)m_Config.GetDouble("Reinsert2 Offset" + (pos + 1), "Y", 0);

                            float reOffset3X = (float)m_Config.GetDouble("Reinsert3 Offset" + (pos + 1), "X", 0);
                            float reOffset3Y = (float)m_Config.GetDouble("Reinsert3 Offset" + (pos + 1), "Y", 0);

                            sendStr += reOffset2X + ",";
                            sendStr += reOffset2Y + ",";

                            sendStr += reOffset3X + ",";
                            sendStr += reOffset3Y + ",";

                            int reinsert_max_count = m_Config.GetInt32("INFO", "Reinsert Max Count", 1);

                            sendStr += reinsert_max_count + ",";

                            int hole_per_reinsert_count = m_Config.GetInt32("INFO", "Hole Per Reinsert Count " + (pos + 1), 1);

                            sendStr += hole_per_reinsert_count + "," + secondForceX + "," + secondForceY + "," + secondForceZ + ",";

                            logManager.Info("Request Position, " + pos + " / " + sendStr);

                            bool sendResult = client.Send(Encoding.ASCII.GetBytes(sendStr));
                            logManager.Trace("socker Send Result : " + sendResult);

                            visionResultDatas[pos].robotRequestTime = (int)robotSW.ElapsedMilliseconds;
                            robotSW.Restart();

                            
                        }
                        else if (command == "insert_info".ToUpper())
                        {
                            int pos = Convert.ToInt32(str.Split(',')[1]);
                            double detectX = Convert.ToDouble(str.Split(',')[2]);
                            double detecty = Convert.ToDouble(str.Split(',')[3]);
                            double detectz = Convert.ToDouble(str.Split(',')[4]);
                            double insertX = Convert.ToDouble(str.Split(',')[5]);
                            double insertY = Convert.ToDouble(str.Split(',')[6]);
                            double insertZ = Convert.ToDouble(str.Split(',')[7]);
                            double detect_j1 = Convert.ToDouble(str.Split(',')[8]);
                            double detect_j2 = Convert.ToDouble(str.Split(',')[9]);
                            double detect_j3 = Convert.ToDouble(str.Split(',')[10]);
                            double detect_j4 = Convert.ToDouble(str.Split(',')[11]);
                            double detect_j5 = Convert.ToDouble(str.Split(',')[12]);
                            double detect_j6 = Convert.ToDouble(str.Split(',')[13]);
                            double insert_j1 = Convert.ToDouble(str.Split(',')[14]);
                            double insert_j2 = Convert.ToDouble(str.Split(',')[15]);
                            double insert_j3 = Convert.ToDouble(str.Split(',')[16]);
                            double insert_j4 = Convert.ToDouble(str.Split(',')[17]);
                            double insert_j5 = Convert.ToDouble(str.Split(',')[18]);
                            double insert_j6 = Convert.ToDouble(str.Split(',')[19]);
                            double validX = Convert.ToDouble(str.Split(',')[20]);
                            double validY = Convert.ToDouble(str.Split(',')[21]);
                            double validZ = Convert.ToDouble(str.Split(',')[22]);
                            double validJ1 = Convert.ToDouble(str.Split(',')[23]);
                            double validJ2 = Convert.ToDouble(str.Split(',')[24]);
                            double validJ3 = Convert.ToDouble(str.Split(',')[25]);
                            double validJ4 = Convert.ToDouble(str.Split(',')[26]);
                            double validJ5 = Convert.ToDouble(str.Split(',')[27]);
                            double validJ6 = Convert.ToDouble(str.Split(',')[28]);
                            double waitX = Convert.ToDouble(str.Split(',')[29]);
                            double waitY = Convert.ToDouble(str.Split(',')[30]);
                            double waitZ = Convert.ToDouble(str.Split(',')[31]);
                            double waitJ1 = Convert.ToDouble(str.Split(',')[32]);
                            double waitJ2 = Convert.ToDouble(str.Split(',')[33]);
                            double waitJ3 = Convert.ToDouble(str.Split(',')[34]);
                            double waitJ4 = Convert.ToDouble(str.Split(',')[35]);
                            double waitJ5 = Convert.ToDouble(str.Split(',')[36]);
                            double waitJ6 = Convert.ToDouble(str.Split(',')[37]);

                            DateTime cur = DateTime.Now;

                            if (m_Config.GetString("INFO", "Height Use Insert", "False").ToUpper() == "TRUE")
                            {
                                heightList.Add(insertZ - 273);
                                logManager.Fatal("Receive Insert Z Value : " + insertZ);
                                logManager.Fatal("save Insert Z Value : " + (insertZ - 273));
                            }
                            else
                            {
                                heightList.Add(detectz - 273);
                                logManager.Fatal("Receive Insert Z Value : " + detectz);
                                logManager.Fatal("save Insert Z Value : " + (detectz - 273));
                            }

                            string visionName = m_Config.GetString("INFO", "Vision Name", "LH");

                            if (structResultTblData != null)
                            {
                                structResultTblData.PosTblDataList.Add(new StructPosTblData()
                                {
                                    DateTime = cur,
                                    Model = m_Model,
                                    Seq = m_hangerNumber,
                                    BodyNumber = m_bodyNumber,
                                    Position = pos,
                                    DetectX = detectX,
                                    DetectY = detecty,
                                    DetectZ = detectz,
                                    InsertX = insertX,
                                    InsertY = insertY,
                                    InsertZ = insertZ,
                                    DetectJ1 = detect_j1,
                                    DetectJ2 = detect_j2,
                                    DetectJ3 = detect_j3,
                                    DetectJ4 = detect_j4,
                                    DetectJ5 = detect_j5,
                                    DetectJ6 = detect_j6,
                                    InsertJ1 = insert_j1,
                                    InsertJ2 = insert_j2,
                                    InsertJ3 = insert_j3,
                                    InsertJ4 = insert_j4,
                                    InsertJ5 = insert_j5,
                                    InsertJ6 = insert_j6,
                                    VisionName = visionName,
                                    ValidX = validX,
                                    ValidY = validY,
                                    ValidZ = validZ,
                                    ValidJ1 = validJ1,
                                    ValidJ2 = validJ2,
                                    ValidJ3 = validJ3,
                                    ValidJ4 = validJ4,
                                    ValidJ5 = validJ5,
                                    ValidJ6 = validJ6,
                                    WaitX = waitX,
                                    WaitY = waitY,
                                    WaitZ = waitZ,
                                    WaitJ1 = waitJ1,
                                    WaitJ2 = waitJ2,
                                    WaitJ3 = waitJ3,
                                    WaitJ4 = waitJ4,
                                    WaitJ5 = waitJ5,
                                    WaitJ6 = waitJ6
                                });

                                /*
                                structResultTblData.PosTblDataList.Add(new StructPosTblData()
                                {
                                    DateTime = cur,
                                    Model = m_Model,
                                    Seq = m_hangerNumber,
                                    BodyNumber = m_bodyNumber,
                                    Position = pos,
                                    DetectX = detectX,
                                    DetectY = detecty,
                                    DetectZ = detectz,
                                    InsertX = insertX,
                                    InsertY = insertY,
                                    InsertZ = insertZ,
                                    DetectJ1 = detect_j1,
                                    DetectJ2 = detect_j2,
                                    DetectJ3 = detect_j3,
                                    DetectJ4 = detect_j4,
                                    DetectJ5 = detect_j5,
                                    DetectJ6 = detect_j6,
                                    InsertJ1 = insert_j1,
                                    InsertJ2 = insert_j2,
                                    InsertJ3 = insert_j3,
                                    InsertJ4 = insert_j4,
                                    InsertJ5 = insert_j5,
                                    InsertJ6 = insert_j6,
                                    VisionName = visionName
                                });
                                */
                            }

                            /*
                            long? index = dbManager.GetResultIndex(cur, m_hangerNumber, m_bodyNumber);

                            dbManager.InsertPosResult(
                                index.Value,
                                pos,
                                detectX,
                                detecty,
                                detectz,
                                insertX,
                                insertY,
                                insertZ,
                                detect_j1,
                                detect_j2,
                                detect_j3,
                                detect_j4,
                                detect_j5,
                                detect_j6,
                                insert_j1,
                                insert_j2,
                                insert_j3,
                                insert_j4,
                                insert_j5,
                                insert_j6,
                                cur,
                                m_Model,
                                m_hangerNumber,
                                m_bodyNumber,
                                visionName
                                );
                            */
                        }
                        else if (command == "HeightCorrect".ToUpper())
                        {
                            double correctX = 0;
                            double correctY = 0;

                            int pos = Convert.ToInt32(str.Split(',')[1]);
                            double requestHeight = Convert.ToDouble(str.Split(',')[2]);

                            List<KeyValuePair<string, string>> configDataList = m_CorrectConfig.GetSectionValuesAsList(m_Model + " " + pos + " " + "X");
                            List<VisionCorrectData> visionCorrectList = new List<VisionCorrectData>();

                            configDataList.ForEach(x =>
                            {
                                visionCorrectList.Add(new VisionCorrectData(Convert.ToDouble(x.Key), Convert.ToDouble(x.Value)));
                            });

                            visionCorrectList.OrderBy(x => x.Height);

                            //내용이 없을 경우
                            if (visionCorrectList.Count == 0)
                            {
                                correctX = 0;
                            }
                            //1개만 존재 할 경우
                            else if (visionCorrectList.Count == 1)
                            {
                                correctX = visionCorrectList.First().Correct;
                            }
                            //다수의 내용이 존재할 경우
                            else
                            {
                                VisionCorrectData start = null;
                                VisionCorrectData end = null;

                                for (int i = 0; i < visionCorrectList.Count; i++)
                                {
                                    if (visionCorrectList[i].Height < requestHeight)
                                    {
                                        start = visionCorrectList[i];
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                for (int i = 0; i < visionCorrectList.Count; i++)
                                {
                                    if (visionCorrectList[i].Height > requestHeight)
                                    {
                                        end = visionCorrectList[i];
                                        break;
                                    }
                                }

                                if (start == null)
                                {
                                    correctX = end.Correct;
                                }
                                else if (end == null)
                                {
                                    correctX = start.Correct;
                                }
                                else
                                {
                                    double distance = end.Height - start.Height;
                                    double tick = (end.Correct - start.Correct) * 1.0 / distance;
                                    correctX = start.Correct + (requestHeight - start.Height) * tick;
                                }
                            }



                            configDataList = m_CorrectConfig.GetSectionValuesAsList(m_Model + " " + pos + " " + "Y");
                            visionCorrectList = new List<VisionCorrectData>();

                            configDataList.ForEach(x =>
                            {
                                visionCorrectList.Add(new VisionCorrectData(Convert.ToDouble(x.Key), Convert.ToDouble(x.Value)));
                            });

                            visionCorrectList.OrderBy(x => x.Height);

                            //내용이 없을 경우
                            if (visionCorrectList.Count == 0)
                            {
                                correctY = 0;
                            }
                            //1개만 존재 할 경우
                            else if (visionCorrectList.Count == 1)
                            {
                                correctY = visionCorrectList.First().Correct;
                            }
                            //다수의 내용이 존재할 경우
                            else
                            {
                                VisionCorrectData start = null;
                                VisionCorrectData end = null;

                                for (int i = 0; i < visionCorrectList.Count; i++)
                                {
                                    if (visionCorrectList[i].Height < requestHeight)
                                    {
                                        start = visionCorrectList[i];
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                for (int i = 0; i < visionCorrectList.Count; i++)
                                {
                                    if (visionCorrectList[i].Height > requestHeight)
                                    {
                                        end = visionCorrectList[i];
                                        break;
                                    }
                                }

                                if (start == null)
                                {
                                    correctY = end.Correct;
                                }
                                else if (end == null)
                                {
                                    correctY = start.Correct;
                                }
                                else
                                {
                                    double distance = end.Height - start.Height;
                                    double tick = (end.Correct - start.Correct) * 1.0 / distance;
                                    correctY = start.Correct + (requestHeight - start.Height) * tick;
                                }
                            }

                            logManager.Fatal(m_Model + " " + pos + " 후 보정 값 : " + requestHeight + " / " + correctX + " / " + correctY);

                            string sendStr = "HeightCorrect," + correctX + "," + correctY;

                            bool sendResult = client.Send(Encoding.ASCII.GetBytes(sendStr));
                            logManager.Trace("socker Send Result : " + sendResult);
                        }
                        else if (command == "force_error".ToUpper())
                        {

                            int pos = Convert.ToInt32(str.Split(',')[1]);

                            logManager.Fatal("Torque Error : " + pos);
                        }
                        else if (command == "torque_error".ToUpper())
                        {

                            int pos = Convert.ToInt32(str.Split(',')[1]);

                            logManager.Fatal("Torque Error : " + pos);
                        }
                        else if (command == "reinsert_mode".ToUpper())
                        {
                            int pos = Convert.ToInt32(str.Split(',')[1]);

                            logManager.Fatal("Reinsert Position : " + pos);
                        }
                        else if (command == "RECEIVE_POS_INFO".ToUpper())
                        {
                            double beforeX = Convert.ToDouble(str.Split(',')[1]);
                            double beforeY = Convert.ToDouble(str.Split(',')[2]);
                            double beforeZ = Convert.ToDouble(str.Split(',')[3]);
                            double afterX = Convert.ToDouble(str.Split(',')[4]);
                            double afterY = Convert.ToDouble(str.Split(',')[5]);
                            double afterZ = Convert.ToDouble(str.Split(',')[6]);
                            double beforeJ1 = Convert.ToDouble(str.Split(',')[7]);
                            double beforeJ2 = Convert.ToDouble(str.Split(',')[8]);
                            double beforeJ3 = Convert.ToDouble(str.Split(',')[9]);
                            double beforeJ4 = Convert.ToDouble(str.Split(',')[10]);
                            double beforeJ5 = Convert.ToDouble(str.Split(',')[11]);
                            double beforeJ6 = Convert.ToDouble(str.Split(',')[12]);
                            double afterJ1 = Convert.ToDouble(str.Split(',')[13]);
                            double afterJ2 = Convert.ToDouble(str.Split(',')[14]);
                            double afterJ3 = Convert.ToDouble(str.Split(',')[15]);
                            double afterJ4 = Convert.ToDouble(str.Split(',')[16]);
                            double afterJ5 = Convert.ToDouble(str.Split(',')[17]);
                            double afterJ6 = Convert.ToDouble(str.Split(',')[18]);

                            DateTime cur = DateTime.Now;

                            string visionName = m_Config.GetString("INFO", "Vision Name", "LH");

                            dbManager.InsertReceivePosResult(0, beforeX, beforeY, beforeZ, afterX, afterY, afterZ, cur, visionName, beforeJ1, beforeJ2, beforeJ3, beforeJ4, beforeJ5, beforeJ6, afterJ1, afterJ2, afterJ3, afterJ4, afterJ5, afterJ6);
                        }
                    }
                    catch(Exception ex)
                    {

                    }
                })).Start();
            }
            catch (Exception ex)
            {
                string sendStr = "Request_Position,";
                sendStr += "NG,";
                sendStr += 0 + ",";
                sendStr += 0 + ",";
                logManager.Error(sendStr + " / " + ex.Message);
                bool sendResult = client.Send(Encoding.ASCII.GetBytes(sendStr));
                logManager.Trace("socker Send Result : " + sendResult);

                Thread.Sleep(200);
                try
                {
                    if (client.IsAliveSocket())
                    {
                        client.Close();
                    }
                }
                catch
                {
                    logManager.Warn("Socket Send Failed");
                }

            }
        }

        private void OnSend(object sender, AsyncSocketSendEventArgs e)
        {
            logManager.Trace("전송 완료 : " + e.SendBytes.ToString());
        }

        private void OnClose(object sender, AsyncSocketConnectionEventArgs e)
        {
            logManager.Warn("소켓 종료 실패");
        }

        private void OnConnet(object sender, AsyncSocketConnectionEventArgs e)
        {
            logManager.Info("소켓 연결 됨");
        }
        #endregion

        private void Load_Pattern(string direction)
        {
            try
            {
                m_PMAlgin.LoadTool(Application.StartupPath + "\\Tools\\" + m_Model + string.Format("{0:00}", Convert.ToInt32(direction)) + "Tool.vpp");
                m_PMAlgin.Load_Pattern(Application.StartupPath + "\\Pattern\\" + m_Model + "\\" + string.Format("{0:00}", Convert.ToInt32(direction)), true);
            }
            catch (Exception ex)
            {
                logManager.Error("패턴 불러오기 실패 : " + ex.Message);
            }
        }

        public int currentServoPos = 0;
        private void OnServoReceive(object sender, AsyncSocketReceiveEventArgs e)
        {
            currentServoPos = Convert.ToInt32(Encoding.ASCII.GetString(e.ReceiveData, 0, e.ReceiveBytes));

            Console.WriteLine(currentServoPos);
        }

        private bool isCapture = false;

        private bool DoInspection(string direction, bool isCapture)
        {
            Stopwatch inspectionSw = new Stopwatch();
            inspectionSw.Start();
            bool result = false;
            this.isCapture = isCapture;

            logManager.Info("캡처모드 : " + isCapture);

            int index = (Convert.ToInt32(direction) - 1) % 100;

            logManager.Info("Index : " + index);

            // 패턴 찾기 실패시 재검사. 재검사 횟수는 Conifg.ini 에서 불러옴.
            // 2012.11.01 김기택
            int ReTest = m_Config.GetInt32("Limit", "ReTest", 5);
            double[] currentMoveValue = new double[3];

            cogDisplay1.StaticGraphics.Clear();
            cogDisplay1.InteractiveGraphics.Clear();

            logManager.Info("디스플레이 클리어");

            if (isCapture)
            {
                // 검사화면 초기화
                cogDisplay1.Image = null;
                logManager.Info("디스플레이 초기화");
                Stopwatch sw = new Stopwatch();
                sw.Start();

                for (int j = 0; j < ReTest; j++)
                {
                    logManager.Info((j + 1).ToString() + " 위치 검사 시작");

                    currentMoveValue[0] = 0;
                    currentMoveValue[1] = 0;
                    currentMoveValue[2] = 180;
                    cogDisplay1.Image = null;
                    cogDisplay1.StaticGraphics.Clear();
                    cogDisplay1.InteractiveGraphics.Clear();

                    logManager.Trace("조명 ON");
                    int light = m_Config.GetInt32("Light", "1", 99);
                    jtech_LED.LightON("COM1", 1, light);
                    Thread.Sleep(50);

                    for (int i = 0; i < 5 && cogDisplay1.Image == null; i++)
                    {
                        logManager.Trace((i + 1).ToString() + "번째 영상캡춰 시도.");
                        logManager.Trace("direction 체크 : " + direction + " doinspection");
                        NewImageCapture(j, direction);
                    }

                    new Thread(new ThreadStart(() =>
                    {
                        jtech_LED.LightOFF("COM1", 1);
                    })).Start();


                    if (cogDisplay1.Image != null)
                    {
                        logManager.Trace("영상캡춰 성공.");
                    }
                    else
                    {
                        logManager.Trace("영상캡춰 실패.");
                    }


                }
                sw.Stop();

                logManager.Info("촬영 완료 소요 시간 : " + sw.ElapsedMilliseconds + "ms");

                // 이미지 켈리브레이션
                if (isCapture)
                {
                    cogDisplay1.Image = Run_Calibration(cogDisplay1.Image, direction);
                    logManager.Trace("캘리브레이션 완료.");
                }
            }

            // 패턴 불러오기
            Load_Pattern(direction);

            // 패턴 위치 찾기
            currentMoveValue = Find_Location(cogDisplay1.Image, direction);

            result = visionResult[index];

            // double tmp1 = m_Config.GetDouble("Master", lb_Kind.Text + direction + "X", 0);


            // 보정량 계산
            double masterX = m_Config.GetDouble("Master", m_Model + string.Format("{0:00}", Convert.ToInt32(direction)) + "X", 0);
            double masterY = m_Config.GetDouble("Master", m_Model + string.Format("{0:00}", Convert.ToInt32(direction)) + "Y", 0);

            double correctX = 0;
            double correctY = 0;

            if (currentMoveValue[3] == 60)
            {
                correctX = m_Config.GetDouble("AI Correction", m_Model + string.Format("{0:00}", Convert.ToInt32(direction)) + "X", 0);
                correctY = m_Config.GetDouble("AI Correction", m_Model + string.Format("{0:00}", Convert.ToInt32(direction)) + "Y", 0);
            }
            else
            {
                correctX = m_Config.GetDouble("Correction", m_Model + string.Format("{0:00}", Convert.ToInt32(direction)) + "X", 0);
                correctY = m_Config.GetDouble("Correction", m_Model + string.Format("{0:00}", Convert.ToInt32(direction)) + "Y", 0);
            }

            currentMoveValue[0] = masterX - Math.Round(currentMoveValue[0], 2) + correctX;
            currentMoveValue[1] = masterY - Math.Round(currentMoveValue[1], 2) + correctY;
            currentMoveValue[2] = 0;

            tb_CorrectX.Text = correctX.ToString();
            tb_CorrectY.Text = correctY.ToString();

            // LH의 경우 X, Y 역방향
            //if (lb_Location.Text.Contains("LH"))
            //{
            //    m_MoveValue[0] *= -1;
            //    m_MoveValue[1] *= -1;
            //}

            logManager.Info(string.Format("보정량 계산 완료.(X:{0}, Y:{1}, Angle:{2:0.00})", currentMoveValue[0], currentMoveValue[1], currentMoveValue[2]));

            if (m_Config.GetString("MoveValue", "MinusX", "False").ToUpper() == "TRUE")
            {
                currentMoveValue[0] = currentMoveValue[0] * -1;
            }

            if (m_Config.GetString("MoveValue", "MinusY", "False").ToUpper() == "TRUE")
            {
                currentMoveValue[1] = currentMoveValue[1] * -1;
            }

            lb_MoveX.Text = string.Format("{0:0.00}", Math.Round(currentMoveValue[0], 2));
            lb_MoveY.Text = string.Format("{0:0.00}", Math.Round(currentMoveValue[1], 2));

            logManager.Trace("데이터 화면 표시 완료.");

            // 검사결과
            if ((
                Math.Abs(currentMoveValue[0]) > m_Config.GetDouble("Limit", "X", 100) ||
                Math.Abs(currentMoveValue[1]) > m_Config.GetDouble("Limit", "Y", 100)
                ))
            {
                //visionResult[Convert.ToInt32(direction) - 1] = false;
                visionResult[index] = false;

                currentMoveValue[0] = 0;
                currentMoveValue[1] = 0;
                currentMoveValue[2] = 0;

                logManager.Error("Limit NG");
            }

            m_TotalCnt++;

            try
            {


                StructVisionData data = new StructVisionData();
                data.moveX = currentMoveValue[0];
                data.moveY = currentMoveValue[1];

                if (currentMoveValue[3] == 60)
                {
                    data.inspectionMode = "AI";
                }
                else
                {
                    data.inspectionMode = "PATTERN";
                }

                logManager.Trace("일반 검사 위치 : " + index + " / " + " 이동값 : " + data.moveX + " , " + data.moveY + " / 보정값 : " + correctX + " , " + correctY);

                visionDatas[index] = data;

            }
            catch (Exception ex)
            {
                logManager.Error("비전 결과 저장 실패" + ex.Message);
            }

            try
            {
                jtech_LED.LightOFF("COM1", 1);
                //Write_SystemLog("조명 종료");
            }
            catch
            {

            }

            inspectionSw.Stop();
            logManager.Info("검사 소요 시간 : " + inspectionSw.ElapsedMilliseconds);

            return result;
        }

        private void NewImageCapture(int Count, string direction)
        {
            try
            {
                logManager.Info("촬영 시작" + direction);

                // 영상획득 파라미터
                double brightness = m_Config.GetDouble("Camera", direction + "Brightness", 0);
                double contrast = m_Config.GetDouble("Camera", direction + "Contrast", 0);
                double expose = m_Config.GetDouble("Camera", direction + "Expose", 79976);

                logManager.Trace(string.Format("Brightness:{0}, Contrast:{1}, Expose:{2}", brightness, contrast, expose));

                string camSerial = m_Config.GetString("Camera", "Serial", "22033290");

                logManager.Trace("Serial Number : " + camSerial);

                cogDisplay1.Image = new CogImage24PlanarColor(cameraManager.OneShot(camSerial, (int)brightness, (int)expose));

                logManager.Info("촬영 종료");
            }
            catch (Exception ex)
            {
                logManager.Fatal("영상획득 실패: " + ex.Message);

                //try
                //{
                //    cameraManager.Close();
                //}
                //catch
                //{

                //}

                //try
                //{
                //    Init_Camera();
                //}
                //catch
                //{

                //}
            }
        }

        private ICogImage Run_Calibration(ICogImage inputImage, string direction)
        {

            try
            {
                // 이미지 변환
                ICogImage monoimage = inputImage.GetType().Name == "CogImage8Grey" ? inputImage : new ImageFile().Get_Plan((CogImage24PlanarColor)inputImage, "Intensity");

                // 캘리브레이션 툴 로드
                CogCalibCheckerboardTool calib = new CogCalibCheckerboardTool();

                string tool_path = Application.StartupPath + "\\Tools\\" + m_Model + string.Format("{0:00}", Convert.ToInt32(direction)) + "Calib.vpp";
                if (!File.Exists(tool_path))
                {
                    logManager.Fatal("캘리브레이션데이터가 없습니다. : " + m_Model + string.Format("{0:00}", Convert.ToInt32(direction)) + "Calib.vpp");
                    return null;
                }

                calib = (CogCalibCheckerboardTool)CogSerializer.LoadObjectFromFile(tool_path);
                calib.InputImage = monoimage;

                calib.Run();

                return calib.OutputImage;
            }
            catch (Exception ex)
            {
                logManager.Fatal("캘리브레이션 실패 : " + ex.Message);
                return null;
            }


        }

        public double[] Find_Location(ICogImage mono, string direction)
        {
            int index = (Convert.ToInt32(direction) - 1) % 100;
            double[] value = new double[4];
            double score = 0;

            ToolBase m_DrawTool = new ToolBase();

            AIResult aiResult = null;

            string inspectionMode = m_Config.GetString("InspectionMode", m_Model + string.Format("{0:00}", Convert.ToInt32(direction)), "ALL").ToUpper();

            logManager.Info("direction = " + direction + ", InspectionMode = " + inspectionMode);

            if (inspectionMode == "ALL" || inspectionMode == "AI")
            {
                aiResult = FindAI(mono, direction);
                value[0] = aiResult.X;
                value[1] = aiResult.Y;
                // AI 검사 체크
                value[3] = 60;

                if (Convert.ToInt32(direction) < 10)
                {
                    if (aiResult.Name != null)
                    {
                        if (aiResult.Name.ToUpper() == "HOLE")
                        {
                            visionResult[index] = true;
                            m_PMAlgin.TranslationX = aiResult.X;
                            m_PMAlgin.TranslationY = aiResult.Y;
                        }
                        else
                        {
                            visionResult[index] = false;
                        }
                    }
                }
                else if (Convert.ToInt32(direction) > 100)
                {
                    if (aiResult.Name != null)
                    {
                        if (aiResult.Name.ToUpper() == "PLUG")
                        {
                            visionResult[index] = true;
                            m_PMAlgin.TranslationX = aiResult.X;
                            m_PMAlgin.TranslationY = aiResult.Y;
                        }
                        else
                        {
                            visionResult[index] = false;
                        }
                    }
                    else
                    {
                        visionResult[index] = false;
                    }
                }
            }

            if (inspectionMode == "ALL" || inspectionMode == "PATTERN")
            {
                if (aiResult == null || !aiResult.Result)
                {
                    m_PMAlgin.Image = mono;
                    m_PMAlginLeft.Image = mono;
                    m_PMAlginRight.Image = mono;

                    m_DrawTool.Image = mono;


                    // 패턴 검사
                    score = m_PMAlgin.FindPattern(cogDisplay1, true);
                    score = (double)((int)(score * 100 + 0.5)) / 100.0;     // 소수 세째 자리 반올림

                    if (score * 100 > m_Config.GetDouble("Pattern", "Score", 60))
                    {

                        // 찾은 패턴의 위치
                        value[0] = m_PMAlgin.TranslationX;
                        value[1] = m_PMAlgin.TranslationY;
                        value[3] = 180;

                        logManager.Info("기준 패턴 찾기 성공.");

                        visionResult[index] = true;
                    }
                    else
                    {
                        logManager.Info("기준 패턴 찾기 실패.");

                        value[0] = 0;
                        value[1] = 0;
                        value[2] = 180;

                        visionResult[index] = false;

                    }

                    if (visionResult[index])
                    {
                        bool circleResult = FindCircle(m_PMAlgin.TranslationX, m_PMAlgin.TranslationY);
                        visionResult[index] = circleResult;
                    }
                }
            }

            if (visionResult[index])
            {
                if (aiResult != null)
                {
                    if (aiResult.Result)
                    {
                        m_DrawTool.DrawLabel(string.Format("Location AI : OK {0:0.00}", aiResult.Score * 100.0), cogDisplay1, 7, 420, 20, CogColorConstants.Blue, CogColorConstants.Grey);

                        m_DrawTool.DrawLabel(string.Format("{0}", aiResult.Name), cogDisplay1, aiResult.OriginX, aiResult.OriginY + aiResult.Height, 12, CogColorConstants.White, CogColorConstants.Purple);
                    }
                    else
                    {
                        m_DrawTool.DrawLabel(string.Format("Location Pattern : OK {0:0.00}({1:00})", score * 100.0, m_PMAlgin.FindPatternIndex), cogDisplay1, 7, 420, 20, CogColorConstants.Blue, CogColorConstants.Grey);
                    }
                }
                else
                {
                    m_DrawTool.DrawLabel(string.Format("Location Pattern : OK {0:0.00}({1:00})", score * 100.0, m_PMAlgin.FindPatternIndex), cogDisplay1, 7, 420, 20, CogColorConstants.Blue, CogColorConstants.Grey);
                }
            }
            else
            {
                m_DrawTool.DrawLabel(string.Format("Location Pattern : NG {0:0.00}({1:00})", score * 100.0, m_PMAlgin.FindPatternIndex), cogDisplay1, 7, 420, 20, CogColorConstants.Red, CogColorConstants.Grey);

                if (aiResult != null)
                {
                    m_DrawTool.DrawLabel(string.Format("{0}", aiResult.Name), cogDisplay1, aiResult.OriginX, aiResult.OriginY + aiResult.Height, 12, CogColorConstants.White, CogColorConstants.Purple);
                }
            }

            return value;
        }


        private void SaveImage(string direction)
        {
            string path, destination;
            path = m_Config.GetString("Result", "RemotePath", "D:\\Result");

            try
            {
                if (!Directory.Exists(path))
                {
                    path = m_Config.GetString("Result", "Path", "D:\\Result");
                }
            }
            catch
            {

            }

            try
            {
                string resultStr = "";
                if (m_result)
                {
                    resultStr = "OK";
                }
                else
                {
                    resultStr = "NG";
                }

                // BMP 이미지저장
                path += "\\Image\\";
                path += m_ResultDay + "\\";

                bool isCreatedPath = false;

                path += m_Config.GetString("INFO", "Vision Name", "LH") + "\\";


                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string[] directories = Directory.GetDirectories(path);
                directories.ToList().ForEach(x =>
                {
                    if (!isCreatedPath)
                    {
                        isCreatedPath = x.Contains("_" + m_hangerNumber + "_" + m_bodyNumber);

                        if (isCreatedPath)
                        {
                            path = x;
                            logManager.Trace("이미 생성된 폴더 존재 / 경로 : " + path);
                        }
                    }
                });

                if (!isCreatedPath)
                {
                    path += m_ResultTime + "_" + m_hangerNumber + "_" + m_bodyNumber + "_" + m_Model;
                }

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                destination = path + "\\";
                destination += m_ResultTime;
                destination += "_" + m_Model;
                destination += "_" + original_direction;
                destination += "_" + resultStr + ".bmp";

                Bitmap bmp = cogDisplay1.Image.ToBitmap();

                bmp.Save(destination, ImageFormat.Jpeg);
                //bmp.Save(destination);
                bmp.Dispose();
                bmp = null;
                logManager.Trace("원본 이미지 저장 완료");
            }
            catch (Exception ex)
            {
                logManager.Error("원본 이미지 저장 실패 : " + ex.Message);
            }
        }

        private void SaveJPGImage(ICogImage image)
        {
            string path, destination;
            path = m_Config.GetString("Result", "RemotePath", "D:\\Result");

            try
            {
                if (!Directory.Exists(path))
                {
                    path = m_Config.GetString("Result", "Path", "D:\\Result");
                }
            }
            catch
            {

            }

            try
            {
                string resultStr = "";
                if (m_result)
                {
                    resultStr = "OK";
                }
                else
                {
                    resultStr = "NG";
                }

                // BMP 이미지저장
                path += "\\InspectionImage\\";
                path += m_ResultDay + "\\";

                bool isCreatedPath = false;

                path += m_Config.GetString("INFO", "Vision Name", "LH") + "\\";


                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string[] directories = Directory.GetDirectories(path);
                directories.ToList().ForEach(x =>
                {
                    if (!isCreatedPath)
                    {
                        isCreatedPath = x.Contains("_" + m_hangerNumber + "_" + m_bodyNumber);

                        if (isCreatedPath)
                        {
                            path = x;
                            logManager.Trace("이미 생성된 폴더 존재 / 경로 : " + path);
                        }
                    }
                });

                if (!isCreatedPath)
                {
                    path += m_ResultTime + "_" + m_hangerNumber + "_" + m_bodyNumber + "_" + m_Model;
                }

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                destination = path + "\\";
                destination += m_ResultTime;
                destination += "_" + m_Model;
                destination += "_" + original_direction;
                destination += "_" + resultStr + ".jpg";

                Bitmap bmp = image.ToBitmap();


                logManager.Trace("검사 이미지 저장 완료");

                /*
                if (isUseRemote)
                {
                    ImageConverter converter = new ImageConverter();
                    if (!imageSocket.IsAliveSocket())
                    {
                        string ip = m_Config.GetString("Image Remote", "IP", "");

                        imageSocket = new AsyncSocketClient(0);
                        imageSocket.Connect(ip, 9990);
                    }

                    string sendData = "Image," + destination + ",";
                    //sendData += Encoding.ASCII.GetString((byte[])converter.ConvertTo(bmp, typeof(byte[])));
                    byte[] sendBytes = Encoding.ASCII.GetBytes(sendData);
                    byte[] sendImageBytes = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
                    byte[] sendAllBytes = new byte[sendBytes.Length + sendImageBytes.Length];

                    Array.Copy(sendBytes, 0, sendAllBytes, 0, sendBytes.Length);
                    Array.Copy(sendImageBytes, 0, sendAllBytes, sendBytes.Length, sendImageBytes.Length);

                    imageSocket.Send(sendAllBytes);

                }
                */

                bmp.Save(destination, ImageFormat.Jpeg);
                bmp.Dispose();
                bmp = null;
            }
            catch (Exception ex)
            {
                logManager.Error("검사 이미지 저장 실패 : " + ex.Message);
            }

        }

        private void hdd_Clear(DateTime current)
        {
            Console.WriteLine("HDD Clear");

            int OKSaveDay = m_Config.GetInt32("HDDClear", "OK", 7);
            int NGSaveDay = m_Config.GetInt32("HDDClear", "NG", 30);
            int ResultSaveDay = m_Config.GetInt32("HDDClear", "Result", 360);

            m_HDDClearTime = current;
            string tmp_time = current.ToString("yyyy MM dd hh mm ss");
            m_Config.WriteValue("HDDClear", "LastClearTime", tmp_time);

            // Result Data 삭제
            current = m_HDDClearTime.AddDays(-1 * ResultSaveDay);
            string day = current.ToString("yyyy-MM-dd");

            string path;
            path = m_Config.GetString("Result", "Path", "D:\\Result");

            #region Result 데이타 삭제 (.txt)
            try
            {
                // Result 데이터 삭제
                //string foldername = Application.StartupPath + "\\Result\\";
                string foldername = path + "\\Result\\";

                string[] files = Directory.GetFiles(foldername + "OK");

                foreach (string filename in files)
                {
                    string[] temp = filename.Split('\\');

                    string compareDay = temp[temp.GetLength(0) - 1];

                    int i;

                    // 저장기간이 초과된 파일인 경우 삭제
                    if ((i = compareDay.CompareTo(day)) < 0)
                    {
                        File.Delete(filename);
                    }
                }

                files = Directory.GetFiles(foldername + "NG");

                foreach (string filename in files)
                {
                    string[] temp = filename.Split('\\');

                    string compareDay = temp[temp.GetLength(0) - 1];

                    int i;

                    // 저장기간이 초과된 파일인 경우 삭제
                    if ((i = compareDay.CompareTo(day)) < 0)
                    {
                        File.Delete(filename);
                    }
                }
            }
            catch
            {
            }
            #endregion
            #region Result 데이타 삭제 (.jpg)
            try
            {
                // Result 이미지 삭제
                string[] directorys = Directory.GetDirectories(path + "\\Image\\JPG_Image");

                foreach (string dirname in directorys)
                {
                    int i;

                    string[] temp = dirname.Split('\\');
                    string compareDirectory = temp[temp.GetLength(0) - 1];

                    // 저장기간이 초과된 파일인 경우 삭제
                    if ((i = compareDirectory.CompareTo(current.Year.ToString())) < 0)
                    {
                        try
                        {
                            Directory.Delete(dirname, true);
                        }
                        catch { }
                    }
                }
            }
            catch
            {
            }
            #endregion

            // NG Data 삭제
            current = m_HDDClearTime.AddDays(-1 * NGSaveDay);
            day = current.ToString("yyyy-MM-dd");

            #region NG 데이타 삭제(.bmp)
            try
            {
                // NG 이미지 삭제
                //string[] directorys = Directory.GetDirectories(Application.StartupPath + "\\Image\\NG");

                //foreach (string dirname in Directory.GetDirectories(Application.StartupPath + "\\Image\\NG"))
                foreach (string dirname in Directory.GetDirectories(path + "\\Image\\NG"))
                {
                    int i;

                    string[] temp = dirname.Split('\\');
                    string compareDirectory = temp[temp.GetLength(0) - 1];

                    // 저장기간이 초과된 파일인 경우 삭제
                    if ((i = compareDirectory.CompareTo(day)) < 0)
                    {
                        try
                        {
                            Directory.Delete(dirname, true);
                        }
                        catch { }
                    }
                }

                // BYPASS 이미지 삭제
                //foreach (string dirname in Directory.GetDirectories(Application.StartupPath + "\\Image\\BYPASS"))
                foreach (string dirname in Directory.GetDirectories(path + "\\Image\\BYPASS"))
                {
                    int i;

                    string[] temp = dirname.Split('\\');
                    string compareDirectory = temp[temp.GetLength(0) - 1];

                    // 저장기간이 초과된 파일인 경우 삭제
                    if ((i = compareDirectory.CompareTo(day)) < 0)
                    {
                        try
                        {
                            Directory.Delete(dirname, true);
                        }
                        catch { }
                    }
                }
            }
            catch
            {
            }
            #endregion

            // OK Data 삭제
            current = m_HDDClearTime.AddDays(-1 * OKSaveDay);
            day = current.ToString("yyyy-MM-dd");

            #region OK 데이타 삭제(.bmp)
            try
            {
                // OK 이미지 삭제
                //string[] directorys = Directory.GetDirectories(Application.StartupPath + "\\Image\\NG");

                //foreach (string dirname in Directory.GetDirectories(Application.StartupPath + "\\Image\\OK"))
                foreach (string dirname in Directory.GetDirectories(path + "\\Image\\OK"))
                {
                    int i;

                    string[] temp = dirname.Split('\\');
                    string compareDirectory = temp[temp.GetLength(0) - 1];

                    // 저장기간이 초과된 파일인 경우 삭제
                    if ((i = compareDirectory.CompareTo(day)) < 0)
                    {
                        try
                        {
                            Directory.Delete(dirname, true);
                        }
                        catch { }
                    }
                }
            }
            catch
            {
            }
            #endregion

        }

        private bool isFormClosing = false;

        private void Main_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("프로그램을 종료하시겠습니까?", "Program Exit", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                e.Cancel = true;
                this.Show();
                return;
            }

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

            isFormClosing = true;
            logManager.Action("프로그램 종료");
            cameraManager.Close();
            plcManager.PLC_WRITE(plcManager.PLCAddress.Output_VisionBusy, 0);

            Thread.Sleep(200);

            Environment.Exit(0);
        }



        private void btn_Setting_Click_1(object sender, EventArgs e)
        {
            ProcessStartInfo s = new ProcessStartInfo();
            s.FileName = Application.StartupPath + "\\" + "Setting.exe";

            Process.Start(s);
        }

        private void btn_Report_Click_1(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo s = new ProcessStartInfo();
                s.FileName = Application.StartupPath + "\\Report System\\" + "Report.exe";

                Process.Start(s);
            }
            catch (Exception ex)
            {
                logManager.Fatal("이력 프로그램 실행 실패: " + ex.Message);
            }

        }

        private void btn_CamSetting_Click_1(object sender, EventArgs e)
        {

            CamSetting camSetting = new CamSetting(cameraManager);
            camSetting.Owner = this;

            camSetting.Show();
        }

        public interface ILightInterface
        {
            void LightON(string port, int channel, int brightness);
            void LightOFF(string port, int channel);

            // 15.11.07 전체조명 ON OFF 테스트 필요. (미완성)
            void LightOnAll(string[] ports);
            void LightOffAll(string[] ports);
        }

        public class HSR_LED : ILightInterface
        {
            HSRLEDControl m_LedControler = new HSRLEDControl();

            public void LightON(string port, int channel, int brightness)
            {
                m_LedControler.InitControler(port);

                m_LedControler.LED_Brightness(channel, brightness);

                m_LedControler.LED_ONOFF(channel, HSRLEDControl.LEDSTATE.ON);

                m_LedControler.ReleaseControler();
            }

            public void LightOFF(string port, int channel)
            {
                m_LedControler.InitControler(port);

                m_LedControler.LED_ONOFF(channel, HSRLEDControl.LEDSTATE.OFF);

                m_LedControler.ReleaseControler();
            }

            public void LightOnAll(string[] ports)
            {
                //모든 시리얼포트를 가져올 경우 멈춤 현상 발생
                string[] ctrls = System.IO.Ports.SerialPort.GetPortNames();

                foreach (string port in ports)
                {
                    m_LedControler.InitControler(port);

                    for (int i = 1; i <= 4; i++)
                    {
                        //m_LedControler.LED_Brightness(i, 99);
                        m_LedControler.LED_ONOFF(i, HSRLEDControl.LEDSTATE.ON);
                    }

                    m_LedControler.ReleaseControler();
                }
            }

            public void LightOffAll(string[] ports)
            {
                //string[] ctrls = System.IO.Ports.SerialPort.GetPortNames();

                foreach (string port in ports)
                {
                    m_LedControler.InitControler(port);

                    for (int i = 1; i <= 4; i++)
                        m_LedControler.LED_ONOFF(i, HSRLEDControl.LEDSTATE.OFF);

                    m_LedControler.ReleaseControler();
                }
            }
        }

        public class JTECH_LED : ILightInterface
        {
            SerialPort serialPort1 = new SerialPort();

            const char CTR = 'L';
            const char OFF = 'E';
            const char CR = (char)0x0D;
            const char CH1 = '1';
            const char CH2 = '2';
            const char CH3 = '3';
            const char CH4 = '4';

            private bool InitControler(string port)
            {
                serialPort1.PortName = port;
                serialPort1.BaudRate = 19200;

                try
                {
                    serialPort1.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            private bool ReleaseControler()
            {
                try
                {
                    serialPort1.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            private void LED_Brightness(int channel, int brightness)
            {
                string tmpBrightness = string.Format("{0:000}", brightness);
                char[] sendMessage = new char[6];

                sendMessage[0] = CTR;
                sendMessage[1] = (char)(channel + '0');
                sendMessage[2] = tmpBrightness.ToCharArray()[0];
                sendMessage[3] = tmpBrightness.ToCharArray()[1];
                sendMessage[4] = tmpBrightness.ToCharArray()[2];
                sendMessage[5] = CR;

                if (serialPort1.IsOpen)
                {
                    serialPort1.Write(sendMessage, 0, 6);
                    //serialPort1.Write(sendMessage);
                }

                Thread.Sleep(50);

                serialPort1.Close();
            }

            private void LED_Brightness_OFF(int channel)
            {
                char[] sendMessage = new char[3];

                sendMessage[0] = OFF;
                sendMessage[1] = (char)(channel + '0');
                sendMessage[2] = CR;

                //string sendMessage = string.Format("E{0:%c}", m_channel);
                //Console.WriteLine(sendMessage);

                if (serialPort1.IsOpen)
                {
                    serialPort1.Write(sendMessage, 0, 3);
                }

                Thread.Sleep(50);
            }

            public void LightON(string port, int channel, int brightness)
            {
                InitControler(port);

                LED_Brightness(channel, brightness);

                ReleaseControler();
            }

            public void LightOFF(string port, int channel)
            {
                InitControler(port);

                LED_Brightness_OFF(channel);

                ReleaseControler();
            }

            public void LightOnAll(string[] ports)
            {
                //모든 시리얼포트를 가져올 경우 멈춤 현상 발생
                string[] ctrls = System.IO.Ports.SerialPort.GetPortNames();

                foreach (string port in ports)
                {
                    for (int i = 1; i <= 4; i++)
                    {
                    }
                }
            }

            public void LightOffAll(string[] ports)
            {
                string[] ctrls = System.IO.Ports.SerialPort.GetPortNames();

                foreach (string port in ports)
                {

                    for (int i = 1; i <= 4; i++)
                    {
                    }
                }
            }
        }


        public enum RESULT_VALUE { OK, NG, BYPASS }
        private IniFile m_CodeINI;
        private RESULT_VALUE[] plugCheckResult = new RESULT_VALUE[1];
        private Cognex.VisionPro.Display.CogDisplay m_CurDisplay;
        private object permisionset;

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

                return true;
            }
            else
            {
                return false;
            }
        }

        public class VisionResultData
        {
            public bool result = false;
            public bool verifyResult = false;
            public int captureCount = 0;
            public double moveX;
            public double moveY;
            public int visionTime;
            public int verifyTime;
            public int robotRequestTime;

            public int visionCaptureTime;
            public int visionInspectionTime;
            public int maxVerifyTime;

            public int robotInsertTime;
            public int robotWaitTime;
            public int robotMoveInTime;
            public string inspectionMode;
            public bool isCompleted = false;
        }

        public class VisionCorrectData
        {
            public double Height { get { return height; } }
            public double Correct { get { return correct; } }

            private double height = 0;
            private double correct = 0;

            public VisionCorrectData(double height, double correct)
            {
                this.height = height;
                this.correct = correct;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            heightList.Clear();
        }

        private List<YoloItem> aiItems = new List<YoloItem>();

        private AIResult FindAI(ICogImage mono, string direction)
        {
            // List<YoloItem> items = new List<YoloItem>();
            double startX = 0;
            double endX = 0;
            double startY = 0;
            double endY = 0;
            bool m_IsFind = false;
            AIResult result = new AIResult();
            result.X = 0;
            result.Y = 0;
            result.Result = false;

            if (isCapture)
            {
                aiItems.Clear();
            }

            if (detectClient != null && isCapture)
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

                    OpenCvSharp.Mat mat = OpenCvSharp.Extensions.BitmapConverter.ToMat(mono.ToBitmap());

                    if (netChannels == 1)
                    {
                        if (mat.Channels() == 3)
                        {
                            mat = mat.CvtColor(OpenCvSharp.ColorConversionCodes.BGR2GRAY);
                        }
                    }
                    else if (netChannels == 3)
                    {
                        if (mat.Channels() == 1)
                        {
                            mat = mat.CvtColor(OpenCvSharp.ColorConversionCodes.GRAY2BGR);
                        }
                    }

                    convertSw.Stop();
                    // Console.WriteLine("변환 속도 : " + convertSw.ElapsedMilliseconds);
                    logManager.Trace("변환 속도 : " + convertSw.ElapsedMilliseconds);

                    GC.Collect();

                    int width = mat.Width;
                    int height = mat.Height;
                    int channels = mat.Channels();
                    byte[] output = new byte[width * height * channels];

                    OpenCvSharp.Cv2.ImEncode(".bmp", mat, out output);

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

                    // Console.WriteLine("멀지 크기 : " + merge.Length);
                    logManager.Trace("멀지 크기 : " + merge.Length);

                    detectClient.Send(merge);

                    yoloSw.Start();

                    // Console.WriteLine("인식 시작 시간 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    logManager.Trace("인식 시작 시간 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

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

                                            aiItems.Add(item);
                                        }
                                    });

                                    m_Process_Recv.RemoveAt(i);

                                    m_IsFind = true;

                                    yoloSw.Stop();

                                    // Console.WriteLine("인식 소요 시간 : " + yoloSw.ElapsedMilliseconds + "ms");
                                    logManager.Trace("인식 소요 시간 : " + yoloSw.ElapsedMilliseconds + "ms");

                                    // Console.WriteLine("인식 종료 시간 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                                    logManager.Trace("인식 종료 시간 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

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

                m_Process_Recv.Clear();
            }

            for (int j = 0; j < aiItems.Count; j++)
            {
                YoloItem item = aiItems[j];

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

            return result;
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

        public class StructResultTblData
        {
            public int Index { get; set; }
            public int TotalHole { get; set; }
            public string Model { get; set; }
            public string Seq { get; set; }
            public string BodyNumber { get; set; }
            public DateTime DateTime { get; set; }
            public List<StructPartResultTblData> PartResultTblDataList = new List<StructPartResultTblData>();
            public StructCompelteTblData CompelteTblData { get; set; }
            public List<StructPosTblData> PosTblDataList = new List<StructPosTblData>();
            public List<StructReceivePosTblData> ReceiveTblDataList = new List<StructReceivePosTblData>();
        }

        public class StructPartResultTblData
        {
            public int Position { get; set; }
            public DateTime DateTime { get; set; }
            public double VisionMoveX { get; set; }
            public double VisionMoveY { get; set; }
            public bool VisionResult { get; set; }
            public string InspectionMode { get; set; }
            public string Model { get; set; }
            public string Seq { get; set; }
            public string BodyNumber { get; set; }
            public string VisionName { get; set; }
        }

        public class StructCompelteTblData
        {
            public DateTime DateTime { get; set; }
            public string Model { get; set; }
            public string Seq { get; set; }
            public string BodyNumber { get; set; }
        }

        public class StructPosTblData
        {
            public DateTime DateTime { get; set; }
            public int Position { get; set; }
            public double DetectX { get; set; }
            public double DetectY { get; set; }
            public double DetectZ { get; set; }
            public double ValidX { get; set; }
            public double ValidY { get; set; }
            public double ValidZ { get; set; }
            public double InsertX { get; set; }
            public double InsertY { get; set; }
            public double InsertZ { get; set; }
            public string Model { get; set; }
            public string Seq { get; set; }
            public string BodyNumber { get; set; }
            public string VisionName { get; set; }
            public double DetectJ1 { get; set; }
            public double DetectJ2 { get; set; }
            public double DetectJ3 { get; set; }
            public double DetectJ4 { get; set; }
            public double DetectJ5 { get; set; }
            public double DetectJ6 { get; set; }
            public double ValidJ1 { get; set; }
            public double ValidJ2 { get; set; }
            public double ValidJ3 { get; set; }
            public double ValidJ4 { get; set; }
            public double ValidJ5 { get; set; }
            public double ValidJ6 { get; set; }
            public double InsertJ1 { get; set; }
            public double InsertJ2 { get; set; }
            public double InsertJ3 { get; set; }
            public double InsertJ4 { get; set; }
            public double InsertJ5 { get; set; }
            public double InsertJ6 { get; set; }
            public double WaitX { get; set; }
            public double WaitY { get; set; }
            public double WaitZ { get; set; }
            public double WaitJ1 { get; set; }
            public double WaitJ2 { get; set; }
            public double WaitJ3 { get; set; }
            public double WaitJ4 { get; set; }
            public double WaitJ5 { get; set; }
            public double WaitJ6 { get; set; }
        }

        public class StructReceivePosTblData
        {
            public DateTime DateTime { get; set; }
            public string VisionName { get; set; }
            public double BeforeX { get; set; }
            public double BeforeY { get; set; }
            public double BeforeZ { get; set; }
            public double AfterX { get; set; }
            public double AfterY { get; set; }
            public double AfterZ { get; set; }
            public double BeforeJ1 { get; set; }
            public double BeforeJ2 { get; set; }
            public double BeforeJ3 { get; set; }
            public double BeforeJ4 { get; set; }
            public double BeforeJ5 { get; set; }
            public double BeforeJ6 { get; set; }
            public double AfterJ1 { get; set; }
            public double AfterJ2 { get; set; }
            public double AfterJ3 { get; set; }
            public double AfterJ4 { get; set; }
            public double AfterJ5 { get; set; }
            public double AfterJ6 { get; set; }
        }
    }
}

