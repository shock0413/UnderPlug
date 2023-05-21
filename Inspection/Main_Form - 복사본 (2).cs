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

namespace Inspection
{
    public partial class Main_Form : Form
    {
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

        PylonCameraManager cameraManager = new PylonCameraManager();

        //// 보정치
        //private double m_mpp_X, m_mpp_Y;

        // 검사정보
        private string m_Model = "";
        private string m_Direction = "";

        // 검사정보(INI 파일)
        private IniFile m_Config = new IniFile(Application.StartupPath + "\\Config.ini");

        // 생산량
        private int m_TotalCnt = 0;
        private int m_OKCnt = 0;

        // 검사 플래그
        private bool isStart = false;
        private bool m_Runstate = false;

        // HDD 클리어 타임
        private DateTime m_HDDClearTime;

        private string m_StartNumber;

        PLCManager plcManager;

        private AsyncSocketServer server;
        private AsyncSocketClient client;
        private int id;

        bool[] m_PositionResult;
        double[][] m_MoveResult;

        private Queue<int> VerifyPositionQueue = new Queue<int>();

        private bool isEndInspection = false;
        private int inspectionIndex = 0;

        private bool isEndVerifyThread = false;

        private PointF[] findPosition;
        private bool[] visionResult;
        private bool?[] verifyResult;
        private bool[] AlreadyInsertedResult;
        private int verifyCount = 0;
        private StructVisionData[] visionDatas;
        private double[] spreadValueX;
        private double[] spreadValueY;
        private int[] reInspectionCount;
        private int reinspectionLimit = 0;
        private bool[] pullForceResult;
        private bool[] isFirstInsert;
        private bool[] servoMoveResult;

        private Queue<int> reinspectionQueue = new Queue<int>();

        private DateTime resultSavaDatatime;

        DBManager.DBManager dbManager = new DBManager.DBManager();


        private AsyncSocketClient sock = null;

        private int currentResultIndex = -1;

        private bool isInspection = false;
        private bool isLightOn = true;

        public Main_Form()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dbManager.CreateAllTable();

            Init_System();
            Init_PLC();
            Init_Camera();

            Init_Server();
        }

        private void Init_System()
        {
            Console.WriteLine("Init System");

            // Label 및 콤포넌트 초기화

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

        }

        #region PLC
        private void Init_PLC()
        {
            //plc 매니저 초기화
            plcManager = new PLCManager();
            //plc 이벤트 추가
            plcManager.OnAsyncTickEvent += PlcManager_OnAsyncTickEvent;
            //PLC 매니저 시작
            plcManager.Start(plcManager.PLCAddress.Output_HB);

            //PLC 주소 초기화
            InitPLCAddr();

            plcManager.PLC_WRITE(plcManager.PLCAddress.Output_ReadyComplete, 1);
        }

        private void InitPLCAddr()
        {
            // Input Address
            //Input_HB = plcManager.PLCAddress.Input_HB;

        }
        #endregion

        private void Init_Server()
        {
            server = new AsyncSocketServer(9988);
            server.OnAccept += new AsyncSocketAcceptEventHandler(OnAccept);
            server.OnError += new AsyncSocketErrorEventHandler(OnError);

            server.Listen(IPAddress.Any);
        }

        private void PlcManager_OnAsyncTickEvent(StructPLCData plcData)
        {
            if (!isInspection && plcData.inputData[2] == 1)
            {
                isInspection = true;
                string kind_number;
                string body_number = "";
                bool result = false;
                kind_number = plcData.inputData[9].ToString();

                for (int i = 0; i < 19; i++)
                {
                    body_number += Convert.ToChar(plcData.inputData[i + 9]);
                }

                plcManager.PLC_WRITE(plcManager.PLCAddress.Output_VisionBusy, 1);

                int position = plcData.inputData[9];

                bool isInspectionEnd = false;

                // 검사
                this.Invoke(new Action(() =>
                {
                    try
                    {
                        DoInspection(position.ToString());
                    }
                    catch(Exception e)
                    {

                    }

                    isInspectionEnd = true;
                }));

                while (!isInspectionEnd)
                {
                    Thread.Sleep(10);
                }

                // 이력저장
                Thread.Sleep(3000);

                int compose = 4030;
                compose += position - 1;
                int compResult = compose;
                plcManager.PLC_WRITE("D" + compose, 1);
                Thread.Sleep(500);

                if (result)
                {
                    compResult += 10;
                    
                    
                }
                else
                {
                    compResult += 20;
                  
                }
                plcManager.PLC_WRITE("D" + compResult, 1);

                Thread.Sleep(3000);
                plcManager.PLC_WRITE("D" + compose, 0);
                plcManager.PLC_WRITE("D" + compResult, 0);
                Thread.Sleep(500);
                plcManager.PLC_WRITE(plcManager.PLCAddress.Output_VisionBusy, 0);
                Thread.Sleep(500);
                isInspection = false;
            }
        }

        #region 서버 관련

            private void OnAccept(object sender, AsyncSocketAcceptEventArgs e)
        {
            AsyncSocketClient worker = new AsyncSocketClient(id++, e.Worker);

            // 데이터 수신을 대기한다.
            worker.Receive();

            worker.OnConnet += new AsyncSocketConnectEventHandler(OnConnet);
            worker.OnClose += new AsyncSocketCloseEventHandler(OnClose);
            worker.OnError += new AsyncSocketErrorEventHandler(OnError);
            worker.OnSend += new AsyncSocketSendEventHandler(OnSend);
            worker.OnReceive += new AsyncSocketReceiveEventHandler(OnReceive);

            // 접속한 클라이언트를 List에 포함한다.
            client = worker;
        }

        private void OnError(object sender, AsyncSocketErrorEventArgs e)
        {
            client = null;
        }

        private void OnReceive(object sender, AsyncSocketReceiveEventArgs e)
        {
            // UpdateTextFunc(txtMessage, "HOST -> PC: Receive ID: " + e.ID.ToString() + " -Bytes received: " + e.ReceiveBytes.ToString() + "\n");
            string receivedString = Encoding.ASCII.GetString(e.ReceiveData, 0, e.ReceiveBytes);

            Write_SocketLog("수신된 내용 : " + receivedString);

            string[] receivedStringParams = receivedString.Split(',');
            string command = receivedStringParams[0].ToUpper();
            if (command == "START")
            {
                Write_SystemLog("검사 시작 신호 수신");
                
                resultSavaDatatime = DateTime.Now;
                Write_SystemLog("검사 시작 시간 : " + resultSavaDatatime.ToString("HH:mm:ss"));
                inspectionIndex++;
                m_Model = receivedStringParams[1];
                Write_SystemLog("검사 모델 : " + m_Model);

                closeVerifyThread = false;

                //서보 ON
                this.Invoke(new Action(() =>
                {
                    isEndInspection = true;
                    VerifyPositionQueue = new Queue<int>();
                    reinspectionQueue = new Queue<int>();
                    ReceiveInspection(receivedStringParams[1]);
                }));

                reinspectionLimit = Convert.ToInt32(receivedStringParams[2]);
            }
            else if (command == "REQUEST")
            {
                int direction = Convert.ToInt32(receivedStringParams[1]) - 1;

                //로봇 보정량 전송
                string shiftData;
                if (m_PositionResult[Convert.ToInt32(direction)] && servoMoveResult[direction])
                {
                    if (AlreadyInsertedResult[direction])
                    {
                        shiftData = "SKIP,";
                    }
                    else
                    {
                        shiftData = "OK,";
                    }

                    Write_VisionLog("전송 검사 위치 : " + direction + " / " + "전송 당시 이동값 : " + visionDatas[direction].moveX + " / " + visionDatas[direction].moveY);


                    shiftData += visionDatas[direction].moveY.ToString() + "," + visionDatas[direction].moveX.ToString() + ",0";
                }
                else
                {
                    double collectionXValue = 0;
                    double collectionYValue = 0;
                    int collectionSumCount = 0;

                    /*
                    for (int i = 0; i < m_PositionResult.Length; i++)
                    {
                        if (m_PositionResult[i] && servoMoveResult[i])
                        {
                            collectionXValue += visionDatas[i].moveX;
                            collectionYValue += visionDatas[i].moveY;
                            collectionSumCount++;
                        }
                    }

                    if (collectionSumCount > 0)
                    {
                        collectionXValue /= collectionSumCount;
                        collectionYValue /= collectionSumCount;
                    }
                    */
                    //shiftData = "OK," + collectionYValue + ", " + collectionXValue + ",0";

                    int leftPibot = -1;
                    int rightPibot = -1;

                    for (int i = 0; i < visionDatas.Length; i++)
                    {
                        if (m_PositionResult[i] && servoMoveResult[i])
                        {
                            if (leftPibot == -1)
                            {
                                leftPibot = i;
                            }
                            rightPibot = i;
                        }
                    }

                    int difference = rightPibot - leftPibot;
                    double xMulti = 0;
                    double yMulti = 0;
                    if (leftPibot != -1 && rightPibot != -1)
                    {
                        xMulti = (visionDatas[rightPibot].moveX - visionDatas[leftPibot].moveX) / difference;
                        yMulti = (visionDatas[rightPibot].moveY - visionDatas[leftPibot].moveY) / difference;

                        double xDestination = (direction - leftPibot) * xMulti + visionDatas[leftPibot].moveX;
                        double yDestination = (direction - leftPibot) * yMulti + visionDatas[leftPibot].moveY;

                        shiftData = "OK," + yDestination + "," + xDestination + ",0";
                        Write_SystemLog("전송 예측 데이터 : " + shiftData);

                        //Write_VisionLog("전송 예측 데이터 : " + direction + " / " + shiftData);
                        Write_VisionLog("전송 예측 위치 : " + direction + " / " + "전송 당시 이동값 : " +  Math.Round(xDestination, 2) + "/" + Math.Round(yDestination, 2));

                    }
                    else
                    {
                        shiftData = "NG,0,0,0";
                        Write_VisionLog("전송 예측 실패 : " + shiftData);
                    }
                }


                Write_SystemLog("전송데이터 : " + shiftData);
                client.Send(Encoding.Default.GetBytes(shiftData));
            }
            else if (command == "VERIFY")
            {
                try
                {
                    int index = Convert.ToInt32(receivedStringParams[1]) - 1;

                    double spreadX = Convert.ToDouble(receivedStringParams[3]);
                    double spreadY = Convert.ToDouble(receivedStringParams[4]);

                    spreadValueX[index] = spreadX;
                    spreadValueY[index] = spreadY;

                    bool pullForceCheck = Convert.ToInt32(receivedStringParams[2]) == 0;
                    pullForceResult[index] = pullForceCheck;

                    VerifyPositionQueue.Enqueue(Convert.ToInt32(receivedStringParams[1]));

                    Write_SystemLog(index + "힘 체크 결과 값 : " + pullForceCheck);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("verify : " + ex.Message);
                }
            }
            else if (command == "REINSPECTION")
            {
                Write_SystemLog("재검사 처리");
                try
                {
                    List<KeyValuePair<string, string>> directions = m_Config.GetSectionValuesAsList(m_Model + " Position");
                    directions = directions.OrderBy(x => Convert.ToInt32(x.Value)).ToList();

                    bool wait = false;

                    for (int i = 0; i < directions.Count; i++)
                    {
                        try
                        {
                            Write_SystemLog(i + "번째 재검사 카운트 : " + reInspectionCount[i] + " 검증 결과 : " + verifyResult[i].Value);
                            if ((!verifyResult[i].Value ) && reInspectionCount[i] < reinspectionLimit && !AlreadyInsertedResult[i])
                            {
                                wait = true;
                            }

                            if (!isFirstInsert[i])
                            {
                                wait = true;
                            }
                        }
                        catch
                        {
                        }
                         
                    }

                    try
                    {
                        if (wait)
                        {
                            if (reinspectionQueue.Count > 0)
                            {
                                string reinspectionPos = reinspectionQueue.Dequeue().ToString();
                                byte[] sendData = Encoding.ASCII.GetBytes("Reinspection," + reinspectionPos + ",");
                                client.Send(sendData);
                                Write_SystemLog("재검사 위치 전송 : " + reinspectionPos);
                            }
                            else
                            {
                                byte[] sendData = Encoding.ASCII.GetBytes("Reinspection,WAIT,");
                                client.Send(sendData);
                                Write_SystemLog("재검사 : 대기 상태 전송");
                            }
                        }

                        else
                        {
                            byte[] sendData = Encoding.ASCII.GetBytes("Reinspection,END,");
                            client.Send(sendData);
                            Write_SystemLog("재검사 : 종료 상태 전송");
                        }
                    }
                    catch
                    {

                    }
                     
                }
                catch(Exception ex)
                {
                    Console.WriteLine("재검사 오류 발생 : " + ex.Message);
                }
            }
            else if (command == "RESULT")
            {
                try
                {
                    List<KeyValuePair<string, string>> directions = m_Config.GetSectionValuesAsList(m_Model + " Position");
                    directions = directions.OrderBy(x => Convert.ToInt32(x.Value)).ToList();

                    string sendData = "Result";

                    directions.ForEach(x =>
                    {
                        int position = Convert.ToInt32(x.Key) - 1;

                        sendData += "," + x.Key.ToString() + ",";
                        if (AlreadyInsertedResult[position])
                        {
                            this.Invoke(new Action(() =>
                            {
                                displayPanel1.SetColor(position, System.Windows.Media.Brushes.Yellow);
                            }));

                            sendData += "Already Inserted";
                        }
                        else if (verifyResult[position].HasValue && verifyResult[position].Value)
                        {
                            sendData += "OK";
                            this.Invoke(new Action(() =>
                            {
                                displayPanel1.SetColor(position, System.Windows.Media.Brushes.Green);
                            }));

                        }
                        else
                        {
                            sendData += "NG";
                            this.Invoke(new Action(() =>
                           {
                               displayPanel1.SetColor(position, System.Windows.Media.Brushes.Red);
                           }));
                        }
                    });

                    client.Send(Encoding.Default.GetBytes(sendData));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("result : " + ex.Message);
                }
            }
            else if (command == "END")
            {
                try
                {
                    isEndInspection = true;
                    closeVerifyThread = true;
                    for (int i = 0; i < 1000 && !isEndVerifyThread; i++)
                    {
                        Thread.Sleep(200);
                    }

                    List<string> receivedData = receivedStringParams.ToList();
                    receivedData.RemoveAt(0);

                    this.Invoke(new Action(() =>
                    {
                        lv_Result.Items.Clear();

                        List<KeyValuePair<string, string>> directions = m_Config.GetSectionValuesAsList(m_Model + " Position");
                        directions = directions.OrderBy(x => Convert.ToInt32(x.Value)).ToList();

                        directions.ForEach(x =>
                        {
                            int position = Convert.ToInt32(x.Key) - 1;

                            ListViewItem item = new ListViewItem(x.Key);


                            item.SubItems.Add(reInspectionCount[position].ToString());

                            //비전 결과
                            if (visionResult[position])
                            {
                                item.SubItems.Add("OK");
                            }
                            else
                            {
                                item.SubItems.Add("NG");
                            }

                            //장착 여부
                            if (AlreadyInsertedResult[position])
                            {
                                item.SubItems.Add("이미 장착됨");
                            }
                            else if (verifyResult[position].HasValue && verifyResult[position].Value)
                            {
                                item.SubItems.Add("OK");
                            }
                            else
                            {
                                item.SubItems.Add("NG");
                            }

                            lv_Result.Items.Add(item);
                        });
                    }));

                    //파일에 쓰기
                    this.Invoke(new Action(() =>
                    {

                        string resultPath = m_Config.GetString("Result", "Path", "D:\\Result") + "\\Result\\Data\\";
                        if (!Directory.Exists(resultPath))
                        {
                            Directory.CreateDirectory(resultPath);
                        }
                        string filePath = resultPath + "\\result_" + resultSavaDatatime.ToString("yyyyMMdd") + ".csv";

                        if (!File.Exists(filePath))
                        {
                            FileStream stream = File.Create(filePath);
                            stream.Close();
                            stream.Dispose();
                            stream = null;
                            StreamWriter writer = File.AppendText(filePath);
                            writer.Write("검사 시간,");
                            writer.Write("Point 1 비전 결과,");
                            writer.Write("point 2 비전 결과,");
                            writer.Write("point 3 비전 결과,");
                            writer.Write("point 4 비전 결과,");
                            writer.Write("Point 1 장착 결과,");
                            writer.Write("point 2 장착 결과,");
                            writer.Write("point 3 장착 결과,");
                            writer.Write("point 4 장착 결과,");
                            writer.WriteLine("");
                            writer.Flush();
                            writer.Close();
                            writer.Dispose();
                            writer = null;
                        }

                        {
                            StreamWriter writer = File.AppendText(filePath);

                            writer.WriteLine("");

                            List<KeyValuePair<string, string>> directions = m_Config.GetSectionValuesAsList(m_Model + " Position");
                            directions = directions.OrderBy(x => Convert.ToInt32(x.Value)).ToList();

                            string str = resultSavaDatatime.ToString("HHmmss") + ",";

                            directions.ForEach(x =>
                            {
                                int position = Convert.ToInt32(x.Key) - 1;

                                //비전 결과
                                if (visionResult[position])
                                {
                                    str += "OK,";
                                }
                                else
                                {
                                    str += "NG,";
                                }
                            });

                            directions.ForEach(x =>
                            {
                                int position = Convert.ToInt32(x.Key) - 1;

                                //장착 여부
                                if (AlreadyInsertedResult[position])
                                {
                                    str += "이미 장착됨,";
                                }
                                else if (verifyResult[position].HasValue && verifyResult[position].Value)
                                {
                                    str += "OK,";
                                }
                                else
                                {
                                    str += "NG,";
                                }
                            });

                            receivedData.ForEach(x =>
                            {
                                if (!string.IsNullOrEmpty(x))
                                {
                                    str += x + ",";
                                }
                            });


                            writer.Write(str);
                            writer.Flush();
                            writer.Close();
                            writer.Dispose();
                            writer = null;
                        }

                    }));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("end : " + ex.Message);
                }
            }
            else if (command == "CT")
            {
               

                this.Invoke(new Action(() =>
                {

                    List<string> receivedData = receivedStringParams.ToList();
                    receivedData.RemoveAt(0);

                    string resultPath = m_Config.GetString("Result", "Path", "D:\\Result") + "\\Result\\Data\\";
                    if (!Directory.Exists(resultPath))
                    {
                        Directory.CreateDirectory(resultPath);
                    }
                    string filePath = resultPath + "\\result_" + resultSavaDatatime.ToString("yyyyMMdd") + ".csv";

                    StreamWriter writer = File.AppendText(filePath);

                    List<KeyValuePair<string, string>> directions = m_Config.GetSectionValuesAsList(m_Model + " Position");
                    directions = directions.OrderBy(x => Convert.ToInt32(x.Value)).ToList();
                    string str = "";
                    receivedData.ForEach(x =>
                    {
                        if (!string.IsNullOrEmpty(x))
                        {
                            str += x + ",";
                        }
                    });

                    writer.Write(str);
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();
                    writer = null;


                }));
            }
            else if (command == "SVM CAPTURE")
            {
                string imagePath = Directory.GetCurrentDirectory() + "\\svm.jpg";
                File.Delete(imagePath);

                bool isCapture = false;


                Process.Start(Directory.GetCurrentDirectory() + "\\SVM_Capture.exe");
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(100);
                    if(File.Exists(imagePath))
                    {
                        isCapture = true;
                        break;
                    }
                }

                if (isCapture)
                {
                    try
                    {

                        this.Invoke(new Action(() =>
                        {
                            try
                            {
                                try
                                {
                                    DoinspectionPlug();
                                }
                                catch (Exception ex)
                                {
                                    Write_SystemLog("검증 DoinspectionPlug Error : " + ex.Message);
                                }
                                if (plugCheckResult[0] == RESULT_VALUE.OK)
                                {
                                    client.Send(Encoding.Default.GetBytes(command + ",OK,"));
                                }
                                else
                                {
                                    client.Send(Encoding.Default.GetBytes(command + ",NG,"));
                                }

                                try
                                {
                                    File.Delete(imagePath);
                                }
                                catch (Exception ex2)
                                {
                                    MessageBox.Show("제거 실패 ex" + ex2.Message);
                                }
                            }
                            catch
                            {
                                client.Send(Encoding.Default.GetBytes(command + ",OK,"));
                            }
                        }));
                    }
                    catch(Exception ex)
                    {
                        client.Send(Encoding.Default.GetBytes(command + ",OK,"));
                        //MessageBox.Show("검증 실패 ex" + ex.Message);
                    }

                }
                else
                {
                    //MessageBox.Show("캡쳐 실패");
                    client.Send(Encoding.Default.GetBytes(command + ",OK,"));
                }
            }

            string[] messages = receivedString.Split('\n');
            for (int i = 1; i < messages.Length; i++)
            {
                OnReceive(sender, new AsyncSocketReceiveEventArgs(e.ID + i, Encoding.Default.GetByteCount(messages[i]), Encoding.Default.GetBytes(messages[i])));
            }
        }

        private void ReceiveInspection(string model)
        {
            Write_SystemLog("검사 시작");

            List<KeyValuePair<string, string>> positions = m_Config.GetSectionValuesAsList(model + " Position");
            Write_SystemLog("총 검사 포지션 : " + positions.Count);
            positions = positions.OrderBy(x => Convert.ToInt32(x.Key)).ToList();

            Write_SystemLog("검사 변수 초기화 : " + positions.Count);

            findPosition = new PointF[positions.Count];
            visionResult = new bool[positions.Count];
            verifyResult = new bool?[positions.Count];
            AlreadyInsertedResult = new bool[positions.Count];
            reInspectionCount = new int[positions.Count];
            visionDatas = new StructVisionData[positions.Count];
            pullForceResult = new bool[positions.Count];
            isFirstInsert = new bool[positions.Count];
            spreadValueX = new double[positions.Count];
            spreadValueY = new double[positions.Count];
            servoMoveResult = new bool[positions.Count];

            for (int i = 0; i < positions.Count; i++)
            {
                AlreadyInsertedResult[i] = false;
                servoMoveResult[i] = false;
                reInspectionCount[i] = 0;
            }

            m_PositionResult = new bool[positions.Count];
            m_MoveResult = new double[positions.Count][];

            positions.ForEach(x =>
            {
                string position = string.Format("{0:00}", Convert.ToInt32(x.Key));
                string direction = x.Key;
                int destination = Convert.ToInt32(x.Value);

                int moveSpeed = m_Config.GetInt32("Servo", "MoveSpeed", 7000);

                //일반 이동
                //servoMove(destination, moveSpeed);
                //포지션 테이블로 이동


                Write_SystemLog("서보 이동 시작 위치 : " + x.Key);

                bool isFinish = false;
               
                for (int i = 0; i < 25 && !isFinish; i++)
                {

                    if (destination < 0)
                    {
                        isFinish = destination - 1 <= (currentServoPos) && (currentServoPos) <= destination + 1;
                    }
                    else
                    {
                        isFinish = destination - 1 <= (currentServoPos) && (currentServoPos) <= destination + 1;
                    }
                    Thread.Sleep(200);
                }

                Thread.Sleep(500);

                if (isFinish)
                {
                    servoMoveResult[Convert.ToInt32(x.Key) - 1] = true;
                    Write_SystemLog("서보 목적지 도착 : " + x.Key);
                }
                else
                {
                    servoMoveResult[Convert.ToInt32(x.Key) - 1] = false;
                    Write_SystemLog("서보 목적지 도착 실패 : " + x.Key);
                }

                //방향 가져오기
                // 검사 수행
                try
                {
                    lb_Kind.Text = m_Model;
                    m_Direction = position.ToString();
                    lb_Direction.Text = m_Direction;

                    Write_SystemLog("해당 위치 검사 시작 : " + x.Key);

                    DoInspection(position.ToString());
                }
                catch(Exception ex)
                {
                    Write_SystemLog("검사 실패 : " + ex);
                }

                /*
                //전송 데이터
                Write_SystemLog("전송 이동값 X : " + m_MoveValue[0]);
                Write_SystemLog("전송 이동값 Y : " + m_MoveValue[1]);
                //Write_SystemLog("전송 이동값 A : " + m_MoveValue[2]);

                //검사 종료 기타 내용 처리
                 *
                 */
                SaveResult();
                SaveImage(direction);
                ScreenCapture();
                HDD_Space_View();

                this.Update();
            });

            currentResultIndex = (int)dbManager.InsertResult(resultSavaDatatime, m_Model, "", "");

            isEndInspection = false;

            DoVerifyPlug(positions.Count);
        }

        bool closeVerifyThread = false;

        private void DoVerifyPlug(int positionCount)
        {
            new Thread(new ThreadStart(() =>
            {
                isEndVerifyThread = false;
                int currentInspectionIndex = inspectionIndex;

                while (inspectionIndex == currentInspectionIndex && !isFormClosing && (!isEndInspection || VerifyPositionQueue.Count > 0))
                {
                    Console.WriteLine("DoVerifyPlug Run");

                    if (closeVerifyThread)
                    {
                        break;
                    }


                    if (VerifyPositionQueue.Count > 0)
                    {
                        verifyCount++;

                        int currentPosition = VerifyPositionQueue.Dequeue();

                        string direction = string.Format("{0:00}", Convert.ToInt32(currentPosition));

                        this.Invoke(new Action(() =>
                        {
                            lb_Direction.Text = direction;
                        }));

                        List<KeyValuePair<string, string>> positions = m_Config.GetSectionValuesAsList(m_Model + " Position");

                        positions = positions.OrderBy(x => Convert.ToInt32(x.Key)).ToList();

                        int survoPos = Convert.ToInt32(positions[currentPosition - 1].Value);
                        int moveSpeed = m_Config.GetInt32("Servo", "MoveSpeed", 7000);
 
                        bool isFinish = false;
                        int destination = Convert.ToInt32(positions[currentPosition - 1].Value);

                        for (int i = 0; i < 25 && !isFinish; i++)
                        {

                            if (destination < 0)
                            {
                                isFinish = destination - 1 <= (currentServoPos) && (currentServoPos) <= destination + 1;
                            }
                            else
                            {
                                isFinish = destination - 1 <= (currentServoPos) && (currentServoPos) <= destination + 1;
                            }
                            Thread.Sleep(200);
                        }


                        Thread.Sleep(500);

                        if (isFinish)
                        {
                            servoMoveResult[currentPosition - 1] = true;
                        }
                        else
                        {
                            servoMoveResult[currentPosition - 1] = false;
                        }

                        Thread.Sleep(1000);

                        bool isEndReinspection = false;

                        this.Invoke(new Action(() =>
                        {
                            int positionIndex = currentPosition - 1;


                            Write_SystemLog("검증 현재 위치 : " + positionIndex);

                            bool result = DoVerifyInspection(direction, true);
                            dbManager.InsertPartResult(currentResultIndex, currentPosition, reInspectionCount[positionIndex], m_MoveResult[positionIndex][0], m_MoveResult[positionIndex][1], spreadValueX[positionIndex], spreadValueY[positionIndex], visionResult[positionIndex], AlreadyInsertedResult[positionIndex], result, pullForceResult[positionIndex]);
                            if (!result)
                            {
                                DoVerifyInspection(direction);
                                if (reInspectionCount[currentPosition - 1] < reinspectionLimit)
                                {
                                    reinspectionQueue.Enqueue(Convert.ToInt32(direction));
                                    reInspectionCount[currentPosition - 1]++;
                                }
                            }
                            isEndReinspection = true;
                        }));

                        while (!isEndReinspection && !isEndInspection)
                        {
                            Thread.Sleep(300);
                        }
                        isFirstInsert[currentPosition - 1] = true;
                    }

                    Thread.Sleep(50);
                }
                closeVerifyThread = false;
                isEndVerifyThread = true;
                Write_SystemLog("검증 쓰레드 종료");
            })).Start();
        }

        private void OnSend(object sender, AsyncSocketSendEventArgs e)
        {
            //UpdateTextFunc(txtMessage, "PC -> HOST: Send ID: " + e.ID.ToString() + " -Bytes sent: " + e.SendBytes.ToString() + "\n");
            Write_SocketLog("Send : " + e.SendBytes.ToString());
        }

        private void OnClose(object sender, AsyncSocketConnectionEventArgs e)
        {
            //UpdateTextFunc(txtMessage, "HOST -> PC: Closed ID: " + e.ID.ToString() + "\n");
            Write_SocketLog("Socket Closed : ");
        }

        private void OnConnet(object sender, AsyncSocketConnectionEventArgs e)
        {
            Write_SocketLog("Socket Connected");
        }
        #endregion

        private void Load_Pattern(string direction)
        {
            try
            {
                m_PMAlgin.LoadTool(Application.StartupPath + "\\Tools\\" + m_Model + string.Format("{0:00}", Convert.ToInt32(direction)) + "Tool.vpp");
                m_PMAlgin.Load_Pattern(Application.StartupPath + "\\Pattern\\" + m_Model+ "\\" + direction, true);
            }
            catch (Exception ex)
            {
                Write_SystemLog("MainPattern: " + ex.Message);
            }

            try
            {
                m_PMAlginLeft.LoadTool(Application.StartupPath + "\\Tools\\" + m_Model + string.Format("{0:00}", Convert.ToInt32(direction)) + "LeftTool.vpp");
                m_PMAlginLeft.Load_Pattern(Application.StartupPath + "\\Pattern\\" + m_Model + "\\" + m_Direction + "\\Left", true);
            }
            catch (Exception ex)
            {
                Write_SystemLog("LeftPattern:" + ex.Message);
            }

            try
            {
                m_PMAlginRight.LoadTool(Application.StartupPath + "\\Tools\\" + m_Model + string.Format("{0:00}", Convert.ToInt32(direction)) + "RightTool.vpp");
                m_PMAlginRight.Load_Pattern(Application.StartupPath + "\\Pattern\\" + m_Model + "\\" + m_Direction + "\\Right", true);
            }
            catch (Exception ex)
            {
                Write_SystemLog("RightPattern:" + ex.Message);
            }
        }

        private void Init_Camera()
        {
            Console.WriteLine("Init Camera");

            string serial = m_Config.GetString("Camera", "Serial", "");
        }

        public int currentServoPos = 0;
        private void OnServoReceive(object sender, AsyncSocketReceiveEventArgs e)
        {
            currentServoPos = Convert.ToInt32(Encoding.ASCII.GetString(e.ReceiveData, 0, e.ReceiveBytes));

            Console.WriteLine(currentServoPos);
        }

        private void DoInspection(string direction)
        {
            // 검사시간 표시
            DateTime cur = DateTime.Now;
            m_ResultDay = cur.ToString("yyyy-MM-dd");
            m_ResultTime = cur.ToString("HHmmss");

            lb_ResultTime.Text = cur.ToString("yyyy-MM-dd hh:mm:ss");

            // 검사화면 초기화
            cogDisplay1.Image = null;
            cogDisplay1.StaticGraphics.Clear();
            cogDisplay1.InteractiveGraphics.Clear();

            // 패턴 찾기 실패시 재검사. 재검사 횟수는 Conifg.ini 에서 불러옴.
            // 2012.11.01 김기택
            int ReTest = m_Config.GetInt32("Limit", "ReTest", 5);

            double[] currentMoveValue = new double[3];

            for (int j = 0; j < ReTest; j++)
            {
                Write_SystemLog((j + 1).ToString() + "번째 검사 시작");

                currentMoveValue[0] = 0;
                currentMoveValue[1] = 0;
                currentMoveValue[2] = 180;
                cogDisplay1.Image = null;
                cogDisplay1.StaticGraphics.Clear();
                cogDisplay1.InteractiveGraphics.Clear();

                jtech_LED.LightON("COM1", 1, 10);
                
                // NewImageCapture 함수 내 이미지 얻을 수 없음
                /*
                for (int i = 0; i < 5 && cogDisplay1.Image == null; i++)
                {
                    Write_SystemLog((i + 1).ToString() + "번째 영상캡춰 시도.");
                    Write_SystemLog("direction 체크 : " + direction + " doinspection");
                    NewImageCapture(j, direction);
                }

                if (cogDisplay1.Image != null)
                {
                    Write_SystemLog("영상캡춰 성공.");
                }
                else
                {
                    Write_SystemLog("영상캡춰 실패.");
                }
                */
                

                // 이미지 켈리브레이션
                cogDisplay1.Image = Run_Calibration(cogDisplay1.Image, direction);

                Write_SystemLog("캘리브레이션 완료.");

                // 패턴 불러오기
                Load_Pattern(direction);

                // 패턴 위치 찾기
                currentMoveValue = Find_Location(cogDisplay1.Image, direction);

                // 결과 OK 이면 재검사 안함.
                if (visionResult[Convert.ToInt32(direction) - 1] == true)
                {
                    Write_SystemLog("검사 OK.");
                    break;
                }

                Write_SystemLog("검사 NG.");
            }

            //방향
            currentMoveValue[0] *= m_Config.GetDouble("Location", "X", 1);
            currentMoveValue[1] *= m_Config.GetDouble("Location", "Y", 1);
            currentMoveValue[2] *= m_Config.GetDouble("Location", "T", 1);


            // double tmp1 = m_Config.GetDouble("Master", lb_Kind.Text + direction + "X", 0);


            // 보정량 계산
            currentMoveValue[0] = (m_Config.GetDouble("Master", lb_Kind.Text + direction + "X", 0) - Math.Round(currentMoveValue[0], 2));
            currentMoveValue[1] = m_Config.GetDouble("Master", lb_Kind.Text + direction + "Y", 0) - Math.Round(currentMoveValue[1], 2);
            currentMoveValue[2] = m_Config.GetDouble("Master", lb_Kind.Text + direction + "Angle", 0) - currentMoveValue[2];

            // LH의 경우 X, Y 역방향
            //if (lb_Location.Text.Contains("LH"))
            //{
            //    m_MoveValue[0] *= -1;
            //    m_MoveValue[1] *= -1;
            //}

            Write_SystemLog(string.Format("보정량 계산 완료.(X:{0}, Y:{1}, Angle:{2:0.00})", currentMoveValue[0], currentMoveValue[1], currentMoveValue[2]));

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

            Write_SystemLog("데이터 화면 표시 완료.");

            // 검사결과
            if ((
                Math.Abs(currentMoveValue[0]) > m_Config.GetDouble("Limit", "X", 100) ||
                Math.Abs(currentMoveValue[1]) > m_Config.GetDouble("Limit", "Y", 100) ||
                Math.Abs(currentMoveValue[2]) > m_Config.GetDouble("Limit", "Angle", 50)
                ))
            {
                //visionResult[Convert.ToInt32(direction) - 1] = false;

                Write_SystemLog("START2 Limit NG");
            }

            if (visionResult[Convert.ToInt32(direction) - 1] == false)
            {

                lb_Result.Text = "NG";
                lb_Result.ForeColor = Color.Red;

                visionResult[Convert.ToInt32(direction) - 1] = false;

                Write_SystemLog("검사 결과 화면 표시 NG and 완료.");

            }
            else
            {
                visionResult[Convert.ToInt32(direction) - 1] = true;

                bool isAlreadyInserted = DoVerifyInspection(direction, false);
                AlreadyInsertedResult[Convert.ToInt32(direction) - 1] = isAlreadyInserted;
                Write_SystemLog("검증 완료");

                Write_SystemLog("검사 결과 화면 표시 OK and 완료.");

                lb_Result.Text = "OK";
                lb_Result.ForeColor = Color.Green;

                m_OKCnt++;
            }

            m_TotalCnt++;

            try
            {

                int idx = Convert.ToInt32(direction) - 1;
                m_PositionResult[idx] = visionResult[Convert.ToInt32(direction) - 1];
                m_MoveResult[idx] = currentMoveValue;

                StructVisionData data = new StructVisionData();
                data.moveX = currentMoveValue[0];
                data.moveY = currentMoveValue[1];

                //Write_VisionLog("일반 검사 위치 : " + idx + " / " + "검사 당시 이동값 : " + data.moveX + " / " + data.moveY);

                visionDatas[idx] = data;

                m_Config.WriteValue("Count", "Total", m_TotalCnt);
                m_Config.WriteValue("Count", "OK", m_OKCnt);
               
                jtech_LED.LightOFF("COM1", 1);
                Write_SystemLog("조명 종료");
           
            }
            catch(Exception ex)
            {
                Write_SystemLog(ex.Message);
            }
        }

        private void DoVerifyInspection(string direction)
        {
            // 검사시간 표시
            DateTime cur = DateTime.Now;
            m_ResultDay = cur.ToString("yyyy-MM-dd");
            m_ResultTime = cur.ToString("HHmmss");

            lb_ResultTime.Text = cur.ToString("yyyy-MM-dd hh:mm:ss");

            // 검사화면 초기화
            cogDisplay1.Image = null;
            cogDisplay1.StaticGraphics.Clear();
            cogDisplay1.InteractiveGraphics.Clear();
             
            // 패턴 찾기 실패시 재검사. 재검사 횟수는 Conifg.ini 에서 불러옴.
            // 2012.11.01 김기택
            int ReTest = m_Config.GetInt32("Limit", "ReTest", 5);


            double[] currentMoveValue = new double[3];

            for (int j = 0; j < ReTest; j++)
            {
                Write_SystemLog((j + 1).ToString() + "번째 검사 시작");

                currentMoveValue[0] = 0;
                currentMoveValue[1] = 0;
                currentMoveValue[2] = 180;
                cogDisplay1.Image = null;
                cogDisplay1.StaticGraphics.Clear();
                cogDisplay1.InteractiveGraphics.Clear();

                jtech_LED.LightON("COM1", 1, 10);

                for (int i = 0; i < 5 && cogDisplay1.Image == null; i++)
                {
                    Write_SystemLog((i + 1).ToString() + "번째 영상캡춰 시도.");
                    Write_SystemLog("direction 체크 : " + direction + " DoVerifyInsepction ");
                    NewImageCapture(j, direction);
                }

                if (cogDisplay1.Image != null)
                {
                    Write_SystemLog("영상캡춰 성공.");
                }
                else
                {
                    Write_SystemLog("영상캡춰 실패.");
                }
                // 이미지 켈리브레이션
                cogDisplay1.Image = Run_Calibration(cogDisplay1.Image, direction);

                Write_SystemLog("캘리브레이션 완료.");

                // 패턴 불러오기
                Load_Pattern(direction);

                // 패턴 위치 찾기
                currentMoveValue = Find_Location(cogDisplay1.Image, direction);

                // 결과 OK 이면 재검사 안함.
                if (visionResult[Convert.ToInt32(direction) - 1] == true)
                {
                    Write_SystemLog("검사 OK.");
                    break;
                }

                Write_SystemLog("검사 NG.");

            }

            //방향
            currentMoveValue[0] *= m_Config.GetDouble("Location", "X", 1);
            currentMoveValue[1] *= m_Config.GetDouble("Location", "Y", 1);
            currentMoveValue[2] *= m_Config.GetDouble("Location", "T", 1);


            double tmp1 = m_Config.GetDouble("Master", lb_Kind.Text + direction + "X", 0);


            // 보정량 계산
            currentMoveValue[0] = (m_Config.GetDouble("Master", lb_Kind.Text + direction + "X", 0) - Math.Round(currentMoveValue[0], 2));
            currentMoveValue[1] = m_Config.GetDouble("Master", lb_Kind.Text + direction + "Y", 0) - Math.Round(currentMoveValue[1], 2);
            currentMoveValue[2] = m_Config.GetDouble("Master", lb_Kind.Text + direction + "Angle", 0) - currentMoveValue[2];

            // LH의 경우 X, Y 역방향
            //if (lb_Location.Text.Contains("LH"))
            //{
            //    m_MoveValue[0] *= -1;
            //    m_MoveValue[1] *= -1;
            //}

            Write_SystemLog(string.Format("보정량 계산 완료.(X:{0}, Y:{1}, Angle:{2:0.00})", currentMoveValue[0], currentMoveValue[1], currentMoveValue[2]));

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

            Write_SystemLog("데이터 화면 표시 완료.");

            // 검사결과
            if ((
                Math.Abs(currentMoveValue[0]) > m_Config.GetDouble("Limit", "X", 100) ||
                Math.Abs(currentMoveValue[1]) > m_Config.GetDouble("Limit", "Y", 100) ||
                Math.Abs(currentMoveValue[2]) > m_Config.GetDouble("Limit", "Angle", 50)
                ))
            {
                visionResult[Convert.ToInt32(direction) - 1] = false;

                Write_SystemLog("START2 Limit NG");

            }

            if (visionResult[Convert.ToInt32(direction) - 1] == false)
            {

                lb_Result.Text = "NG";
                lb_Result.ForeColor = Color.Red;

                visionResult[Convert.ToInt32(direction) - 1] = false;

                Write_SystemLog("검사 결과 화면 표시 NG and 완료.");

            }
            else
            {
                visionResult[Convert.ToInt32(direction) - 1] = true;
                 
                Write_SystemLog("검사 결과 화면 표시 OK and 완료.");

                lb_Result.Text = "OK";
                lb_Result.ForeColor = Color.Green;

                m_OKCnt++;
            }

            m_TotalCnt++;

            try
            {
                int idx = Convert.ToInt32(direction) - 1;
                m_PositionResult[idx] = visionResult[Convert.ToInt32(direction) - 1];
                m_MoveResult[idx] = new double[]{currentMoveValue[0], currentMoveValue[1], currentMoveValue[2]};

                StructVisionData data = new StructVisionData();
                data.moveX = currentMoveValue[0];
                data.moveY = currentMoveValue[1];

                visionDatas[idx] = data;

                m_Config.WriteValue("Count", "Total", m_TotalCnt);
                m_Config.WriteValue("Count", "OK", m_OKCnt);

                jtech_LED.LightOFF("COM1", 1);
                Write_SystemLog("조명 종료");
            }
            catch (Exception ex)
            {
                Write_SystemLog(ex.Message);
            }
        }

        private bool DoVerifyInspection(string direction, bool isShot)
        {
            int positionIndex = Convert.ToInt32(direction) - 1;

            // 검사시간 표시
            DateTime cur = DateTime.Now;
            m_ResultDay = cur.ToString("yyyy-MM-dd");
            m_ResultTime = cur.ToString("HHmmss");

            this.Invoke(new Action(() =>
            {
                lb_ResultTime.Text = cur.ToString("yyyy-MM-dd hh:mm:ss");
                lb_Result.Text = "-";
                lb_Result.ForeColor = Color.White;
            }));

            // 패턴 찾기 실패시 재검사. 재검사 횟수는 Conifg.ini 에서 불러옴.
            // 2012.11.01 김기택
            int ReTest = m_Config.GetInt32("Limit", "ReTest", 5);
            for (int j = 0; j < ReTest; j++)
            {
                Write_SystemLog((j + 1).ToString() + "번째 검사 시작");

                if (isShot)
                {
                    cogDisplay1.Image = null;
                    cogDisplay1.StaticGraphics.Clear();
                    cogDisplay1.InteractiveGraphics.Clear();

                    jtech_LED.LightON("COM1", 1, 10);

                    for (int i = 0; i < 5 && cogDisplay1.Image == null; i++)
                    {
                        Write_SystemLog((i + 1).ToString() + "번째 영상캡춰 시도.");
                        Write_SystemLog("direction 체크 : " + direction + " DoVerifyInsepction isshot");
                        NewImageCapture(j, direction);
                    }


                    // 이미지 켈리브레이션
                    cogDisplay1.Image = Run_Calibration(cogDisplay1.Image, direction);
                }

            }
            /*
            Histogram histogram = new Histogram();
                 
            CogRectangleAffine rec = new CogRectangleAffine();

            rec.SetCenterLengthsRotationSkew(verifyPosX, verifyPosY, 40, 40, 0, 0);

            double histoValue = histogram.Find_Histo(cogDisplay1.Image, rec);

            cogDisplay1.InteractiveGraphics.Add(rec, "", false);

            double histoMin = m_Config.GetInt32("Histogram", m_Model + " " + direction + " Min", 0);
            double histoMax = m_Config.GetInt32("Histogram", m_Model + " " + direction + " Max", 70);


            Write_SystemLog("위치 : " + positionIndex);
            Write_SystemLog("위치 : " + positionIndex + "\t 찾은 블랍 값 : " + histoValue);

            if (histoValue >= histoMin && histoValue <= histoMax)
            {
                verifyResult[positionIndex] = true;
                visionResult[Convert.ToInt32(direction) - 1] = true;
            }
            else
            {
                visionResult[Convert.ToInt32(direction) - 1] = false;
                verifyResult[positionIndex] = false;
            }

            Write_SystemLog("캘리브레이션 완료.");
            */
            //장착여부 패턴 검사


            // 패턴 불러오기
            Load_Pattern("1" + direction);

            //패턴 검사

            m_PMAlgin.Image = cogDisplay1.Image;
            m_PMAlginLeft.Image = cogDisplay1.Image;
            m_PMAlginRight.Image = cogDisplay1.Image;

            ToolBase m_DrawTool = new ToolBase();

            m_DrawTool.Image = cogDisplay1.Image;

            double Left_OKScoere = m_Config.GetDouble("Pattern", "Left Score", 60);
            double Right_OKScoere = m_Config.GetDouble("Pattern", "Right Score", 60);

            // 패턴 검사
            double score = m_PMAlgin.FindPattern(cogDisplay1, true);
            score = (double)((int)(score * 10000 + 0.5)) / 10000.0;     // 소수 세째 자리 반올림            

            bool PatternVerifyResult = score * 100 > m_Config.GetDouble("Pattern", "Score", 60);

            bool visionResult = false;

            // 검사결과
            if (PatternVerifyResult)
            {
                verifyResult[positionIndex] = true;
                visionResult = true;
            }
            else
            {
                visionResult = false;
                verifyResult[positionIndex] = false;
            }

            if (isShot)
            {
                jtech_LED.LightOFF("COM1", 1);
            }

            Console.WriteLine("Save Result");

            string path, destination;
            path = m_Config.GetString("Result", "Path", "D:\\Result");

            try
            {
                // BMP 이미지저장
                path += "\\Image\\";
                path += lb_Result.Text + "\\";
                path += m_ResultDay;

                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                destination = path + "\\";
                destination += m_ResultTime;
                destination += "_" + lb_Kind.Text;
                destination += "_" + m_Direction;
                destination += "_" + lb_Result.Text + "_Verify.bmp";
            }
            catch (Exception ex)
            {
                Write_SystemLog("SaveImage : " + ex.Message);
            }

            Write_SystemLog("이미지 저장 완료");

            return visionResult;
            
        }

        private void NewImageCapture(string direction)
        {
            try
            {
                // 영상획득 파라미터
                double brightness = m_Config.GetDouble("Camera", direction + "Brightness", 0);
                double contrast = m_Config.GetDouble("Camera", direction + "Contrast", 0);
                double expose = m_Config.GetDouble("Camera", direction + "Expose", 8);
            }
            catch (Exception ex)
            {
                Write_SystemLog("영상획득 실패 : " + ex.Message);
            }
        }

        private void NewImageCapture(int Count, string direction)
        {
            try
            {
                Write_SystemLog("촬영 위치 : " + direction);
                 

                // 영상획득 파라미터
                double brightness = m_Config.GetDouble("Camera", direction + "Brightness", 0);
                double contrast = m_Config.GetDouble("Camera", direction + "Contrast", 0);
                double expose = m_Config.GetDouble("Camera", direction + "Expose", 8);


                Write_SystemLog(string.Format("Brightness:{0}, Contrast:{1}, Expose:{2}", brightness, contrast, expose));


                /*
                string camSerical = m_Config.GetString("Camera", "Serial", "22033290");

                cogDisplay1.Image = new CogImage24PlanarColor(cameraManager.OneShot(camSerical, 34, 10208));
                 */

            }
            catch (Exception ex)
            {
                Write_SystemLog("영상획득 실패 : " + ex.Message);
            }
        }

        #region Light Control

        public void LightOn(char ch, int bri)
        {

            string tmpBrightness = string.Format("{0:000}", bri);
            char[] sendMessage = new char[6];



            try { serialPort_Light.Open(); }
            catch { return; }

            sendMessage[0] = 'L';
            sendMessage[1] = ch;
            sendMessage[2] = tmpBrightness.ToCharArray()[0];
            sendMessage[3] = tmpBrightness.ToCharArray()[1];
            sendMessage[4] = tmpBrightness.ToCharArray()[2];
            sendMessage[5] = (char)0x0D;

            //string sendMessage = string.Format("L{0:%c}{1:000}", m_channel, Int32.Parse(tb_Brightness.Text));
            //Console.WriteLine(sendMessage[1]);

            if (serialPort_Light.IsOpen)
            {
                serialPort_Light.Write(sendMessage, 0, 6);
                //serialPort1.Write(sendMessage);
            }

            System.Threading.Thread.Sleep(100);

            serialPort_Light.Close();

        }

        public void LightOff(char ch)
        {
            char[] sendMessage = new char[3];

            try { serialPort_Light.Open(); }
            catch { }

            sendMessage[0] = 'E';
            sendMessage[1] = ch;
            sendMessage[2] = (char)0x0D;

            //string sendMessage = string.Format("E{0:%c}", m_channel);
            //Console.WriteLine(sendMessage);

            if (serialPort_Light.IsOpen)
            {
                serialPort_Light.Write(sendMessage, 0, 3);
                //serialPort1.Write(sendMessage);
            }

            System.Threading.Thread.Sleep(100);

            serialPort_Light.Close();
        }

        #endregion
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
                    Write_SystemLog("캘리브레이션데이터가 없습니다. : " + m_Model + string.Format("{0:00}", Convert.ToInt32(direction)) + "Calib.vpp");
                    return null;
                }

                calib = (CogCalibCheckerboardTool)CogSerializer.LoadObjectFromFile(tool_path);
                calib.InputImage = monoimage;

                calib.Run();

                return calib.OutputImage;
            }
            catch (Exception ex)
            {
                Write_SystemLog("캘리브레이션 실패 : " + ex.Message);
                return null;
            }


        }

        public double[] Find_Location(ICogImage mono, string direction)
        {
            double[] value = new double[3];

            m_PMAlgin.Image = mono;
            m_PMAlginLeft.Image = mono;
            m_PMAlginRight.Image = mono;

            ToolBase m_DrawTool = new ToolBase();

            m_DrawTool.Image = mono;

            double Left_OKScoere = m_Config.GetDouble("Pattern", "Left Score", 60);
            double Right_OKScoere = m_Config.GetDouble("Pattern", "Right Score", 60);

            // 패턴 검사
            double score = m_PMAlgin.FindPattern(cogDisplay1, true);
            score = (double)((int)(score * 10000 + 0.5)) / 10000.0;     // 소수 세째 자리 반올림            

            if (score * 100 > m_Config.GetDouble("Pattern", "Score", 60))
            {
                findPosition[Convert.ToInt32(direction) - 1].X = (float)m_PMAlgin.TranslationX;
                findPosition[Convert.ToInt32(direction) - 1].Y = (float)m_PMAlgin.TranslationY;

                // 찾은 패턴의 위치
                value[0] = m_PMAlgin.TranslationX;
                value[1] = m_PMAlgin.TranslationY;
                //value[2] = (-1 * m_PMAlgin.Rotation) * 180 / Math.PI;
                //value[2] = (m_PMAlgin.Rotation) * 180 / Math.PI;
                //value[2] = (double)((int)(value[2] * 100 + 0.5)) / 100.0; // 소수 세째 자리 반올림

                Write_SystemLog("기준 패턴 찾기 성공.");

                Write_SystemLog("패턴 검증용 검사 시작.");

                m_PMAlginLeft.CenterX = 0;
                m_PMAlginLeft.CenterY = 0;

                Cognex.VisionPro.CogRectangleAffine Leftrec = new CogRectangleAffine();

                Leftrec.SetCenterLengthsRotationSkew(m_PMAlgin.TranslationX - m_Config.GetInt32("Left" + direction, "CenterX", 160), m_PMAlgin.TranslationY - m_Config.GetInt32("Left" + direction, "CenterY", 50), m_Config.GetInt32("Left" + direction, "Width", 90), m_Config.GetInt32("Left" + direction, "Height", 50), 0, 0);
                m_PMAlginLeft.Region = Leftrec;

                double score1 = m_PMAlginLeft.FindPattern(cogDisplay1, true) * 100;

                double x1 = m_PMAlginLeft.TranslationX;
                double y1 = m_PMAlginLeft.TranslationY;
                value[2] = (m_PMAlginLeft.Rotation) * 180 / Math.PI;
                value[2] = (double)((int)(value[2] * 100 + 0.5)) / 100.0; // 소수 세째 자리 반올림

                if (score1 >= Left_OKScoere)
                {
                    m_DrawTool.DrawLabel(string.Format("Sub Pattern(LEFT) : OK {0:0.00}({1:00})", score1, m_PMAlginLeft.FindPatternIndex), cogDisplay1, 7, 40, 20, CogColorConstants.Blue, CogColorConstants.Grey);
                }
                else
                {
                    m_DrawTool.DrawLabel(string.Format("Sub Pattern(LEFT) : NG {0:0.00}({1:00})", score1, m_PMAlginLeft.FindPatternIndex), cogDisplay1, 7, 40, 20, CogColorConstants.Red, CogColorConstants.Grey);
                }

                m_PMAlginRight.CenterX = 0;
                m_PMAlginRight.CenterY = 0;

                Cognex.VisionPro.CogRectangleAffine Rightrec = new CogRectangleAffine();

                Rightrec.SetCenterLengthsRotationSkew(m_PMAlgin.TranslationX - m_Config.GetInt32("Right" + direction, "CenterX", 160), m_PMAlgin.TranslationY - m_Config.GetInt32("Right" + direction, "CenterY", 50), m_Config.GetInt32("Right" + direction, "Width", 90), m_Config.GetInt32("Right" + direction, "Height", 50), 0, 0);
                m_PMAlginRight.Region = Rightrec;

                double score2 = m_PMAlginRight.FindPattern(cogDisplay1, true) * 100;


                if (score2 >= Right_OKScoere)
                {
                    m_DrawTool.DrawLabel(string.Format("Sub Pattern(Right) : OK {0:0.00}({1:00})", score2, m_PMAlginRight.FindPatternIndex), cogDisplay1, 7, 130, 20, CogColorConstants.Blue, CogColorConstants.Grey);
                }
                else
                {
                    m_DrawTool.DrawLabel(string.Format("Sub Pattern(Right) : NG {0:0.00}({1:00})", score2, m_PMAlginRight.FindPatternIndex), cogDisplay1, 7, 130, 20, CogColorConstants.Red, CogColorConstants.Grey);
                }

                Write_SystemLog(string.Format("패턴 검증용 검사 결과. 합격점수(Left:{0:0.00}, Right{1:0.00}), 찾은 점수(Left:{2:0.00}, Right{3:0.00})", Left_OKScoere, Right_OKScoere, score1, score2));

                double x2 = m_PMAlginRight.TranslationX;
                double y2 = m_PMAlginRight.TranslationY;

                if (score1 < Left_OKScoere || score2 < Right_OKScoere)
                {
                    if (m_StartNumber == "START2")
                    {
                        if (m_Config.GetString("Limit", "Score", "CHECK").ToUpper() == "CHECK")
                        {
                            Write_SystemLog("패턴 Score 검사 NG.");

                            value[0] = 0;
                            value[1] = 0;
                            value[2] = 180;

                            //visionResult[Convert.ToInt32(direction) - 1] = false;

                            return value;
                        }
                        else
                        {
                            Write_SystemLog("패턴 Score 검사 패스 모드.");

                            visionResult[Convert.ToInt32(direction) - 1] = true;
                        }
                    }
                }
                else
                {
                    Write_SystemLog("패턴 검증용 검사 OK.");
                    visionResult[Convert.ToInt32(direction) - 1] = true;
                }
            }
            else
            {
                Write_SystemLog("기준 패턴 찾기 실패.");

                value[0] = 0;
                value[1] = 0;
                value[2] = 180;

                visionResult[Convert.ToInt32(direction) - 1] = false;
            }



            // 소수점 2번째 자리 까지만 화면에 출력하도록 수정
            // 2012.11.01 김기택
            if (visionResult[Convert.ToInt32(direction) - 1] == true)
            {
                m_DrawTool.DrawLabel(string.Format("Location Pattern : OK {0:0.00}({1:00})", score * 100.0, m_PMAlgin.FindPatternIndex), cogDisplay1, 7, 230, 20, CogColorConstants.Blue, CogColorConstants.Grey);
            }
            else
            {
                m_DrawTool.DrawLabel(string.Format("Location Pattern : NG {0:0.00}({1:00})", score * 100.0, m_PMAlgin.FindPatternIndex), cogDisplay1, 7, 230, 20, CogColorConstants.Red, CogColorConstants.Grey);
            }
            //m_PMAlgin.DrawCross(0, 0, 10, 20, cogDisplay1, CogColorConstants.Green, CogColorConstants.Black);
            return value;
        }
         

        private void SaveResult()
        {
            // 이력 저장
            Console.WriteLine("SaveResult");

            string result_string = "";
            //검사 날짜
            result_string += m_ResultDay + "\t";
            //검사 시간
            result_string += m_ResultTime + "\t";
            //기종
            result_string += lb_Kind.Text + "\t";
            //검사 결과
            result_string += lb_Result.Text + "\t";

            //보정량
            result_string += lb_MoveX.Text + "\t";
            result_string += lb_MoveY.Text + "\t";

            FileStream stream;
            StreamWriter writer;

            // Result 폴더 생성
            string result_path = m_Config.GetString("Result", "Path", "D:\\Result") + "\\Result";
            if (Directory.Exists(result_path) == false)
            {
                Directory.CreateDirectory(result_path);
            }

            // OK, NG 폴더 생성
            result_path += "\\" + lb_Result.Text;
            if (Directory.Exists(result_path) == false)
            {
                Directory.CreateDirectory(result_path);
            }

            // 날자별 파일 생성
            result_path += "\\" + m_ResultDay + ".txt";
            if (File.Exists(result_path) == false)
            {
                stream = File.Create(result_path);
                writer = new StreamWriter(stream);
            }
            else
            {
                try
                {
                    stream = new FileStream(result_path, FileMode.Append, FileAccess.Write, FileShare.Read);
                }
                catch (Exception ex)
                {
                    Write_SystemLog("파일을 열수없습니다. : " + result_path + "(" + ex.Message + ")");

                    System.Threading.Thread.Sleep(300);
                    stream = new FileStream(result_path, FileMode.Append, FileAccess.Write, FileShare.Read);
                }

                writer = new StreamWriter(stream);
            }

            writer.WriteLine(result_string);

            writer.Close();
            stream.Close();
        }

        private void SaveImage(string direction)
        {
            Console.WriteLine("Save Result");

            string path, destination;
            path = m_Config.GetString("Result", "Path", "D:\\Result");

            try
            {
                // BMP 이미지저장
                path += "\\Image\\";
                path += lb_Result.Text + "\\";
                path += m_ResultDay;

                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                destination = path + "\\";
                destination += m_ResultTime;
                destination += "_" + lb_Kind.Text;
                destination += "_" + direction;
                destination += "_" + lb_Result.Text + ".bmp";
            }
            catch (Exception ex)
            {
                Write_SystemLog("SaveImage : " + ex.Message);
            }

        }

        private void SaveJPGImage()
        {
            Console.WriteLine("SaveImage");

            // 이미지 저장 폴더 생성
            string savePath = m_Config.GetString("Result", "SavePath", "D:\\Result");
            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

            savePath += "\\JPG_IMG";
            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

            //savePath += "\\" + lb_Result.Text;
            //if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

            savePath += "\\" + m_ResultDay.Replace("/", "");
            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

            //savePath += "\\" + lb_Code.Text;
            //if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

            //savePath += "\\" + lb_EngNo.Text;
            //if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

            savePath += "\\" + m_ResultTime;
            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

            for (int i = 0; i < 1; i++)
            {
                if (cogDisplay1.Image == null) continue;

                //Bitmap img = m_Display[i].Image.ToBitmap();
                try
                {

                    Image img = cogDisplay1.CreateContentBitmap(Cognex.VisionPro.Display.CogDisplayContentBitmapConstants.Image, null, 1300);

                    // 저장경로
                    string img_name = savePath + "\\";
                    //num
                    img_name += i + 1 + "_";
                    // time
                    img_name += m_ResultTime + "_";

                    //// kind
                    //img_name += m_CarKind + "_";
                    //// 부품
                    //string Partinfo = inspection_Info[(i + 1).ToString()];
                    //m_ItemName = new IniFile(Application.StartupPath + "\\PartInfo\\" + Partinfo + "\\" + Partinfo + ".ini");

                    //img_name += m_ItemName.GetString("PartInfo", "name", "") + "(" + inspection_Info[(i + 1).ToString()] + ")" + "_";
                    // 검사결과

                    img_name += lb_Result.ToString() + ".jpg";
                    img.Save(img_name, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                catch (Exception ex)
                {
                    Write_SystemLog(ex.Message);
                }
            }

        }

        private void ScreenCapture()
        {
            ImageFile m_ScreenCapture = new ImageFile();

            int Screen_Width = 1920;
            int Screen_Height = 1080;
            int Screen_Quailty = 50;
            Point StartPoint = new Point(0, 0);

            string Jpg_Path = m_Config.GetString("Result", "Path", "D:\\Result");
            if (!Directory.Exists(Jpg_Path)) Directory.CreateDirectory(Jpg_Path);

            Jpg_Path += "\\Image\\JPG_Image\\";
            if (!Directory.Exists(Jpg_Path)) Directory.CreateDirectory(Jpg_Path);

            Jpg_Path += m_ResultDay + "\\";
            if (!Directory.Exists(Jpg_Path)) Directory.CreateDirectory(Jpg_Path);

            Jpg_Path += lb_Result.Text + "\\";
            if (!Directory.Exists(Jpg_Path)) Directory.CreateDirectory(Jpg_Path);

            Jpg_Path += m_ResultTime + "_" + lb_Result.Text + ".jpg";

            try
            {
                // 폼을 최상위로 위치시키기
                /*
                this.TopMost = true;
                this.Location = new Point(0, 0);
                this.WindowState = FormWindowState.Normal;
                */
                Application.DoEvents();

                System.Threading.Thread.Sleep(100);

                m_ScreenCapture.CaptureFullScreen(Screen_Width, Screen_Height, Screen_Quailty, Jpg_Path);
            }
            catch (Exception ex)
            {
                Write_SystemLog("화면캡쳐시 에러가 발생하였습니다 : " + ex.Message);
                return;
            }
            finally
            {
                // 폼 최상위 위치해재
                this.TopMost = false;

                //Application.DoEvents();
            }
        }

        private void tm_IO_Tick(object sender, EventArgs e)
        {
            DateTime cur = DateTime.Now;
            lb_CurTime.Text = cur.ToString("yy-MM-dd hh:mm:ss");

            // HDD 클리어
            if (cur.Hour == 12 && cur.Minute >= 0)
            {
                // HDD 정리

                if ((cur - m_HDDClearTime).Days > 0)
                    hdd_Clear(cur);

            }

            if (cur.ToString("hh:mm:ss") == "00:00:00")
            {
                Write_Countlog(cur.ToString("yy-MM-dd"));

                m_TotalCnt = 0;
                m_OKCnt = 0;
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

        #region log
        /// 시스템 로그 남기는 함수
        /// </summary>
        /// <param name="log">남길 내용</param>
        private void Write_SystemLog(string log)
        {
            FileStream wstream;
            StreamWriter writer;

            Console.WriteLine(DateTime.Now.ToLongTimeString() + "\t" + log);

            try
            {

                if (!Directory.Exists(Application.StartupPath + @"\System Log\"))
                {
                    Directory.CreateDirectory(Application.StartupPath + @"\System Log\");
                }

                if (File.Exists(Application.StartupPath + @"\System Log\" + String.Format("{0:0000}년{1:00}월{2:00}일.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)) == false)
                {
                    wstream = File.Create(Application.StartupPath + @"\System Log\" + String.Format("{0:0000}년{1:00}월{2:00}일.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                    writer = new StreamWriter(wstream);
                }
                else
                {
                    wstream = new FileStream(Application.StartupPath + @"\System Log\" + String.Format("{0:0000}년{1:00}월{2:00}일.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), FileMode.Append, FileAccess.Write);
                    writer = new StreamWriter(wstream);
                }
                writer.WriteLine(DateTime.Now.ToLongTimeString() + "\t" + log);

                writer.Close();
                wstream.Close();
            }
            catch (Exception ex)
            {
                Write_SystemLog(ex.Message);
            }
            finally
            {
                writer = null;
                wstream = null;
            }
        }

        private void Write_VisionLog(string log)
        {
            FileStream wstream;
            StreamWriter writer;

            Console.WriteLine(DateTime.Now.ToLongTimeString() + "\t" + log);

            try
            {

                if (!Directory.Exists(Application.StartupPath + @"\Vision Log\"))
                {
                    Directory.CreateDirectory(Application.StartupPath + @"\Vision Log\");
                }

                if (File.Exists(Application.StartupPath + @"\Vision Log\" + String.Format("{0:0000}년{1:00}월{2:00}일.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)) == false)
                {
                    wstream = File.Create(Application.StartupPath + @"\Vision Log\" + String.Format("{0:0000}년{1:00}월{2:00}일.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                    writer = new StreamWriter(wstream);
                }
                else
                {
                    wstream = new FileStream(Application.StartupPath + @"\Vision Log\" + String.Format("{0:0000}년{1:00}월{2:00}일.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), FileMode.Append, FileAccess.Write);
                    writer = new StreamWriter(wstream);
                }
                writer.WriteLine(DateTime.Now.ToLongTimeString() + "\t" + log);

                writer.Close();
                wstream.Close();
            }
            catch (Exception ex)
            {
                Write_SystemLog(ex.Message);
            }
            finally
            {
                writer = null;
                wstream = null;
            }
        }

        /// 시스템 로그 남기는 함수
        /// </summary>
        /// <param name="log">남길 내용</param>
        private void Write_SocketLog(string log)
        {
            FileStream wstream;
            StreamWriter writer;

            Console.WriteLine(DateTime.Now.ToLongTimeString() + "\t" + log);

            try
            {

                if (!Directory.Exists(Application.StartupPath + @"\Socket Log\"))
                {
                    Directory.CreateDirectory(Application.StartupPath + @"\Socket Log\");
                }

                if (File.Exists(Application.StartupPath + @"\Socket Log\" + String.Format("{0:0000}년{1:00}월{2:00}일.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)) == false)
                {
                    wstream = File.Create(Application.StartupPath + @"\Socket Log\" + String.Format("{0:0000}년{1:00}월{2:00}일.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                    writer = new StreamWriter(wstream);
                }
                else
                {
                    wstream = new FileStream(Application.StartupPath + @"\Socket Log\" + String.Format("{0:0000}년{1:00}월{2:00}일.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), FileMode.Append, FileAccess.Write);
                    writer = new StreamWriter(wstream);
                }
                writer.WriteLine(DateTime.Now.ToLongTimeString() + "\t" + log);

                writer.Close();
                wstream.Close();
            }
            catch (Exception ex)
            {
                Write_SystemLog(ex.Message);
            }
            finally
            {
                writer = null;
                wstream = null;
            }
        }
        #endregion

        #region HDD View

        private void HDD_Space_View()
        {
            DriveInfo drvE = new DriveInfo(@"E:\");
            //long usedSize = drv.TotalSize - drv.AvailableFreeSpace;

            string spacePath = m_Config.GetString("Result", "Path", "");
            if (spacePath != "")
            {
                spacePath = spacePath.Substring(0, 3);
            }

            try
            {
                DriveInfo drvD;
                if (spacePath != "")
                {

                    drvD = new DriveInfo(spacePath);
                }
                else
                {
                    drvD = new DriveInfo(@"D:\");
                }

                //lb_HDDSpace.Text = string.Format("{0:0.00}% Free Space.", hdd_space);


            }
            catch (Exception e)
            {

            }
        }
        #endregion

        private void Write_Countlog(string log)
        {
            FileStream wstream;
            StreamWriter writer;

            Console.WriteLine(DateTime.Now.ToLongDateString() + "\t" + DateTime.Now.ToLongTimeString() + "\t" + log);

            try
            {

                if (!Directory.Exists(Application.StartupPath + @"\Count Log\"))
                {
                    Directory.CreateDirectory(Application.StartupPath + @"\Count Log\");
                }

                if (File.Exists(Application.StartupPath + @"\Count Log\" + String.Format("{0:0000}년.txt", DateTime.Now.Year)) == false)
                {
                    wstream = File.Create(Application.StartupPath + @"\Count Log\" + String.Format("{0:0000}년.txt", DateTime.Now.Year));
                    writer = new StreamWriter(wstream);
                }
                else
                {
                    wstream = new FileStream(Application.StartupPath + @"\Count Log\" + String.Format("{0:0000}년.txt", DateTime.Now.Year), FileMode.Append, FileAccess.Write);
                    writer = new StreamWriter(wstream);
                }
                writer.WriteLine(DateTime.Now.ToLongTimeString() + "\t" + log);

                writer.Close();
                wstream.Close();
            }
            catch (Exception ex)
            {
                Write_SystemLog(ex.Message);
            }
            finally
            {
                writer = null;
                wstream = null;
            }
        }

        private bool isFormClosing = false;

        private void Main_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            isFormClosing = true;

            if (MessageBox.Show("프로그램을 종료하시겠습니까?", "Program Exit", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                e.Cancel = true;
                this.Show();
                return;
            }
            Write_SystemLog("Form Closing");

            plcManager.PLC_WRITE(plcManager.PLCAddress.Output_VisionBusy, 0);

            Thread.Sleep(200);
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
                s.FileName = Application.StartupPath + "\\" + "Report.exe";

                Process.Start(s);
            }
            catch (Exception ex)
            {
                Write_SystemLog("이력 프로그램 실행 실패 : " + ex.Message);
            }

        }

        private void btn_CamSetting_Click_1(object sender, EventArgs e)
        {
            CamSetting camSetting = new CamSetting();
            camSetting.Owner = this;

            camSetting.Show();
        }
        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
 
            btn_CamSetting.Visible = !btn_CamSetting.Visible;
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

                Thread.Sleep(100);

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

                Thread.Sleep(100);
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

        private void DoinspectionPlug()
        {
            if (File.Exists(Application.StartupPath + "\\CarKind\\" + "INDEX" + ".ini"))
                m_CodeINI = new IniFile(Application.StartupPath + "\\CarKind\\" + "INDEX" + ".ini");
            else
                m_CodeINI = new IniFile(Application.StartupPath + "\\CarKind\\default.ini");

            Dictionary<string, string> inspection_Info = m_CodeINI.GetSectionValues("InspectionItem");
            plugCheckResult = new RESULT_VALUE[inspection_Info.Count];


            for (int i = 0; i < plugCheckResult.Length; i++)
            {
                plugCheckResult[i] = RESULT_VALUE.NG;
            }

            for (int i = 0; i < inspection_Info.Count; i++)
            {

                m_CurDisplay = cogDisplay2;

                string item = m_CodeINI.GetString("Inspectionitem", (i + 1).ToString(), "");
                string part = m_CodeINI.GetString("Component", item, "");

                IniFile itemINI = new IniFile(Application.StartupPath + "\\PartInfo\\" + item + "\\" + item + ".ini");
                string itemName = itemINI.GetString("PartInfo", "Name", "");

                //툴 불러와 검사 실행
                plugCheckResult[i] = Part_DoInspection(item, part, i, 1, 0);
                lb_Result.Text = plugCheckResult[i].ToString();

                // 검사결과 화면 출력
                try
                {
                    CogGraphicLabel tmp = new CogGraphicLabel();
                    tmp.Text = plugCheckResult[i].ToString();
                    tmp.BackgroundColor = CogColorConstants.Black;
                    tmp.Color = plugCheckResult[i] == RESULT_VALUE.OK ? CogColorConstants.Green : plugCheckResult[i] == RESULT_VALUE.NG ? CogColorConstants.Red : CogColorConstants.LightGrey;
                    tmp.Font = new Font("Cambria", 35);
                    tmp.Alignment = CogGraphicLabelAlignmentConstants.TopRight;
                    tmp.X = m_CurDisplay.Image.Width - 20;
                    tmp.Y = 20;

                    m_CurDisplay.StaticGraphics.Add(tmp, "Result");
                }
                catch
                {
                }
              

            }

            Console.WriteLine(GC.GetTotalMemory(false));
            GC.Collect();
            Console.WriteLine(GC.GetTotalMemory(false));
        }

        private RESULT_VALUE Part_DoInspection(string item, string part, int cur_pos, int reCnt, int reTotalCnt)
        {


            // 검사결과
            RESULT_VALUE res = RESULT_VALUE.OK;

            // 검사 항목 정보 로드
            IniFile itemINI = new IniFile(Application.StartupPath + "\\PartInfo\\" + item + "\\" + item + ".ini");
            string inspectioName = itemINI.GetString("PartInfo", "Name", "");
            try
            { 
 
                // 검사 툴 로드
                string[] tools = itemINI.GetString(part, "Tool", "").Split(',');
                string toolAlg = "";

                int master_pos = -1;
                string master_tool = "";
                for (int i = 0; i < tools.Length; i++)
                {
                    toolAlg = itemINI.GetString(part + " " + tools[i], "Method", "");

                    if (toolAlg == "Master")
                    {
                        master_pos = i;
                        master_tool = tools[i];
                        break;
                    }
                }
                if (master_pos >= 0)
                {
                    for (int i = master_pos; i > 0; i--)
                    {
                        tools[i] = tools[i - 1];
                    }

                    tools[0] = master_tool;
                }


                //툴 불러와 검사 진행
                for (int i = 0; i < tools.Length; i++)
                {
                    if (tools[i] == "") continue;

                    RESULT_VALUE part_res = RESULT_VALUE.OK;
                    string res_string = "";

                    toolAlg = itemINI.GetString(part + " " + tools[i], "Method", "");

                    ToolInterface wk = null;
                    switch (toolAlg)
                    {
                        case "Pattern":
                            wk = new Tools.Pattern(tools[i], false, false);
                            break;
                        case "Histo":
                            wk = new Tools.HistoTool(tools[i], false);
                            break;
                        case "Master":
                            wk = new Tools.Pattern(tools[i], true, false);
                            break;
                    }

                    if (wk == null) continue;
                     

                    try
                    {
                        wk.SetInfo(item, part, tools[i]);
                        wk.SetViewLabel(true);
                        if (wk.SetImage(m_CurDisplay) == 0)
                        {
                            //검사 시작
                            if (wk.Run() != 0)
                            {
                                part_res = itemINI.GetString(part, "ByPass", "False") == "False" ? RESULT_VALUE.NG : RESULT_VALUE.BYPASS;
                               

                            }
                            else
                            {
                                part_res = RESULT_VALUE.OK;
                                 
                            }
                            res_string = wk.GetResult();
                        }

                        res_string = wk.GetResult();
                    }
                    catch (Exception e)
                    {
                        //검사 진행 중 실패할 시
                        Console.WriteLine(e.Message);

                        part_res = itemINI.GetString(part, "ByPass", "False") == "False" ? RESULT_VALUE.NG : RESULT_VALUE.BYPASS;
                    }

                    if (part_res == RESULT_VALUE.NG)
                    {
                        res = RESULT_VALUE.NG;
                    }
                   

                    try
                    {
                        (wk as IDisposable).Dispose();
                    }
                    catch { }
                }
                 
            }
            catch (Exception ex)
            {
             
                res = RESULT_VALUE.NG;
             
            }

            return res;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            jtech_LED.LightON("COM1", 1, 10);
            string camSerial = m_Config.GetString("Camera", "Serial", "");

            cameraManager.OneShot(camSerial, 0, 35000);
            cogDisplay1.Image = new CogImage24PlanarColor(cameraManager.LastCaptureImage);

            jtech_LED.LightOFF("COM1", 1);
        }
    }
}

