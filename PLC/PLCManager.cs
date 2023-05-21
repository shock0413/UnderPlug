using AsyncFrameSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Utilities;

namespace PLC
{
    public class PLCManager
    {
        public delegate void OnAsyncTickEventHandler(StructPLCData data);
        public event OnAsyncTickEventHandler OnAsyncTickEvent = delegate { };

        public delegate void OnConnectPlcEventHandler();
        public event OnConnectPlcEventHandler OnConnectPlcEvent = delegate { };

        private bool isClose = false;

        private PLC_Address m_plcAddress;
        public PLC_Address PLCAddress { get { return m_plcAddress; } }

        private bool isConnected = false;
        public bool IsConnected { get { return isConnected; } }

        Thread thread;
        Thread heartbeatThread;

        
        public string heartbeatAddress = null;

        int tickInterval = 300;

        private AsyncSocketClient sock = null;

        Config config = new Config();

        int id = 0;

        List<StructReceiveData> ReceiveDataList = new List<StructReceiveData>();

        public PLCManager()
        {
            m_plcAddress = new PLC_Address();

            InitPlcCommunicator();
            InitSocket();

            new Thread(new ThreadStart(() =>
            {
                try
                {
                    while (!isClose)
                    {
                        InitPlcCommunicator();

                        if (sock == null)
                        {
                            Thread.Sleep(1000);
                            InitSocket();
                        }

                        if(!sock.IsAliveSocket())
                        {
                            InitSocket();
                        }
                        Thread.Sleep(1000);
                    }
                }
                catch
                {

                }
            })).Start();
        }

        public void InitPlcCommunicator()
        {
            Process[] processList = Process.GetProcessesByName("PLCCommunicator");
            if(processList.Length < 1)
            {
                Process.Start("PLCCommunicator\\PLCCommunicator.exe");
            }
        }

        private void InitSocket()
        {
            sock = new AsyncSocketClient(0);

            // 이벤트 핸들러 재정의
            sock.OnConnet += new AsyncSocketConnectEventHandler(OnConnet);
            sock.OnClose += new AsyncSocketCloseEventHandler(OnClose);
            sock.OnSend += new AsyncSocketSendEventHandler(OnSend);
            sock.OnReceive += new AsyncSocketReceiveEventHandler(OnReceive);
            sock.OnError += new AsyncSocketErrorEventHandler(OnError);

            sock.Connect(config.ServerIP, config.ServerPort);
        }

        private void OnConnet(object sender, AsyncSocketConnectionEventArgs e)
        {

        }

        private void OnClose(object sender, AsyncSocketConnectionEventArgs e)
        {

        }

        private void OnSend(object sender, AsyncSocketSendEventArgs e)
        {

        }

        private void OnReceive(object sender, AsyncSocketReceiveEventArgs e)
        {
            string receivedString = Encoding.Default.GetString(e.ReceiveData, 0, e.ReceiveBytes);
            string[] arg = receivedString.Split(',');

            if (arg.Length > 2)
            {
                List<string> list = arg.ToList();

                string command = arg[0];
                int id = Convert.ToInt32(arg[1]);
                string msg = "";

                list.RemoveAt(0);
                list.RemoveAt(0);
                list.ForEach(x =>
                {
                    msg += x;
                    msg += ",";
                });

                StructReceiveData structReceiveData = new StructReceiveData(command, id, msg);
                ReceiveDataList.Add(structReceiveData);
            }
        }

        private void OnError(object sender, AsyncSocketErrorEventArgs e)
        {

        }

        public bool Start(string heartbeatAddress)
        {
            if (isOpen())
            {
                this.heartbeatAddress = heartbeatAddress;

                thread = new Thread(new ThreadStart(ThreadDo));
                thread.Start();

               
               // heartbeatThread = new Thread(new ThreadStart(HeartbeatThreadDo));
               // heartbeatThread.Start();
                
                isConnected = true;

                return true;
            }
            else
            {
                isConnected = false;

                return false;
            }
        }

        public void ThreadDo()
        {
            while (!isClose)
            {
                try
                {
                    Thread.Sleep(tickInterval);

                    StructPLCData data = PLC_SCAN();
                    OnAsyncTickEvent(data);

                }
                catch (Exception ex)
                {

                }
            }
        }

        public void HeartbeatThreadDo()
        {

            while (!isClose)
            {
                try
                {
                    Thread.Sleep(tickInterval);
                    if (PLC_READ(this.heartbeatAddress, 1)[0] == 1)
                    {
                        PLC_WRITE(this.heartbeatAddress, 0);
                    }
                    else
                    {
                        PLC_WRITE(this.heartbeatAddress, 1);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        public void Stop()
        {
            isClose = true;
        }

        public class StructPLCData
        {
            public int[] inputData;
            public int[] outputData;
        }


        #region PLC 통신 관련 메소드
        private StructPLCData PLC_SCAN()
        {
            int[] tmp_receive = PLC_READ(m_plcAddress.Input_HB, 20);
            if (tmp_receive == null) return null;

            int[] inputDatas = tmp_receive;

            tmp_receive = PLC_READ(m_plcAddress.Output_HB, 5);
            int[] outputDatas = tmp_receive;

            StructPLCData data = new StructPLCData();
            data.inputData = inputDatas;
            data.outputData = outputDatas;

            return data;
        }

        public bool isOpen()
        {
            return true;
        }

        public int[] PLC_READ(string startAdd, int size)
        {
            int id = this.id++;
             
            sock.Send(Encoding.Default.GetBytes(config.ReadCommand + "," + id + "," + startAdd + "," + size + "\0"));

            int[] result = GetReceivedData(id);

            return result;
        }

        public bool PLC_WRITE(string addr, int data)
        {
            int id = this.id++;

            sock.Send(Encoding.Default.GetBytes(config.WriteCommand + "," + id + "," + addr + "," + data + "\0"));

            int[] result = GetReceivedData(id);

            if(result == null || result.Length == 0)
            {
                return false;
            }
            else if(result[0] == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        private int[] GetReceivedData(int id)
        {
            StructReceiveData structReceiveData = null;
            for (int i = 0; i < 60 && structReceiveData == null; i++)
            {
                Thread.Sleep(50);
                for (int j = 0; j < ReceiveDataList.Count; j++)
                {
                    try
                    {
                        if (ReceiveDataList[j] == null)
                        {
                            ReceiveDataList.RemoveAt(j);
                        }
                        if (id == ReceiveDataList[j].ID)
                        {
                            try
                            {
                                structReceiveData = ReceiveDataList[j];
                                ReceiveDataList.RemoveAt(j);
                            }
                            catch
                            {
                                j = 0;
                            }
                        }
                    }
                    catch
                    {
                        j = 0;
                    }
                }
            }
            
            if(structReceiveData == null)
            {
                return null;
            }
            else
            {
                List<string> receiveDataList = structReceiveData.Msg.Split(',').ToList();
                List<int> resultList = new List<int>();

                receiveDataList.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x))
                    {
                        resultList.Add(Convert.ToInt32(x));
                    }
                });

                return resultList.ToArray();
            }
        }

        #endregion

        public class StructReceiveData
        {
            public StructReceiveData(string command, int id, string msg)
            {
                this.command = command; 
                this.id = id;
                this.msg = msg;
            }

            public string Command { get { return command; } }
            public int ID { get { return id; } }
            public string Msg { get { return msg; } }
            public DateTime CreateTime { get { return createTime; } }

            private string command;
            private int id;
            private string msg;
            private DateTime createTime = DateTime.Now;
        }


        public class PLC_Address
        {
            // Common
            public string DB;

            // Input
            public string Input_HB;
            public string Input_VisionStart;
            public string Input_Position;
            public string Input_CarKind_Info1;
            public string Input_CarKind_Info2;
            public string Input_CarKind_Info3;
            public string Input_CarKind_Info4;
            public string Input_CarKind_Info5;
            public string Input_CarKind_Info6;
            public string Input_CarKind_Info7;
            public string Input_CarKind_Info8;
            public string Input_CarKind_Info9;
            public string Input_CarKind_Info10;
            public string Input_CarKind_Info11;
            public string Input_CarKind_Info12;
            public string Input_CarKind_Info13;
            public string Input_CarKind_Info14;
            public string Input_CarKind_Info15;
            public string Input_CarKind_Info16;
            public string Input_CarKind_Info17;
            public string Input_CarKind_Info18;
            public string Input_CarKind_Info19;
            public string Input_CarKind_Info20;

            // Output
            public string Output_HB;
            public string Output_ReadyComplete;
            public string Output_VisionBusy;
            public string Output_Vision_1st_CompPos1;
            public string Output_Vision_1st_CompPos2;
            public string Output_Vision_1st_CompPos3;
            public string Output_Vision_1st_CompPos4;
            public string Output_Vision_1st_CompPos5;
            public string Output_Vision_1st_CompPos6;
            public string Output_Vision_1st_CompPos7;
            public string Output_Vision_1st_CompPos8;
            public string Output_Vision_1st_CompPos9;
            public string Output_Vision_1st_CompPos10;
            public string Output_Vision_1st_ResultPos1_OK;
            public string Output_Vision_1st_ResultPos2_OK;
            public string Output_Vision_1st_ResultPos3_OK;
            public string Output_Vision_1st_ResultPos4_OK;
            public string Output_Vision_1st_ResultPos5_OK;
            public string Output_Vision_1st_ResultPos6_OK;
            public string Output_Vision_1st_ResultPos7_OK;
            public string Output_Vision_1st_ResultPos8_OK;
            public string Output_Vision_1st_ResultPos9_OK;
            public string Output_Vision_1st_ResultPos10_OK;
            public string Output_Vision_1st_ResultPos1_NG;
            public string Output_Vision_1st_ResultPos2_NG;
            public string Output_Vision_1st_ResultPos3_NG;
            public string Output_Vision_1st_ResultPos4_NG;
            public string Output_Vision_1st_ResultPos5_NG;
            public string Output_Vision_1st_ResultPos6_NG;
            public string Output_Vision_1st_ResultPos7_NG;
            public string Output_Vision_1st_ResultPos8_NG;
            public string Output_Vision_1st_ResultPos9_NG;
            public string Output_Vision_1st_ResultPos10_NG;
            public string Output_Vision_2nd_CompPos1;
            public string Output_Vision_2nd_CompPos2;
            public string Output_Vision_2nd_CompPos3;
            public string Output_Vision_2nd_CompPos4;
            public string Output_Vision_2nd_CompPos5;
            public string Output_Vision_2nd_CompPos6;
            public string Output_Vision_2nd_CompPos7;
            public string Output_Vision_2nd_CompPos8;
            public string Output_Vision_2nd_CompPos9;
            public string Output_Vision_2nd_CompPos10;
            public string Output_Vision_2nd_ResultPos1_OK;
            public string Output_Vision_2nd_ResultPos2_OK;
            public string Output_Vision_2nd_ResultPos3_OK;
            public string Output_Vision_2nd_ResultPos4_OK;
            public string Output_Vision_2nd_ResultPos5_OK;
            public string Output_Vision_2nd_ResultPos6_OK;
            public string Output_Vision_2nd_ResultPos7_OK;
            public string Output_Vision_2nd_ResultPos8_OK;
            public string Output_Vision_2nd_ResultPos9_OK;
            public string Output_Vision_2nd_ResultPos10_OK;
            public string Output_Vision_2nd_ResultPos1_NG;
            public string Output_Vision_2nd_ResultPos2_NG;
            public string Output_Vision_2nd_ResultPos3_NG;
            public string Output_Vision_2nd_ResultPos4_NG;
            public string Output_Vision_2nd_ResultPos5_NG;
            public string Output_Vision_2nd_ResultPos6_NG;
            public string Output_Vision_2nd_ResultPos7_NG;
            public string Output_Vision_2nd_ResultPos8_NG;
            public string Output_Vision_2nd_ResultPos9_NG;
            public string Output_Vision_2nd_ResultPos10_NG;

            private IniFile m_PlcConfig = new IniFile(Directory.GetCurrentDirectory() + "\\Config.ini");

            public PLC_Address()
            {
                Load_Address();
            }

            public void Load_Address()
            {
                // Input Address
                Input_HB = m_PlcConfig.GetString("Input", "HB", "D4100");
                Input_VisionStart = m_PlcConfig.GetString("Input", "VisionStart", "D4102");
                Input_Position = m_PlcConfig.GetString("Input", "Position", "D4109");
                Input_CarKind_Info1 = m_PlcConfig.GetString("Input", "CarKind_Info1", "D4110");
                Input_CarKind_Info2 = m_PlcConfig.GetString("Input", "CarKind_Info2", "D4111");
                Input_CarKind_Info3 = m_PlcConfig.GetString("Input", "CarKind_Info3", "D4112");
                Input_CarKind_Info4 = m_PlcConfig.GetString("Input", "CarKind_Info4", "D4113");
                Input_CarKind_Info5 = m_PlcConfig.GetString("Input", "CarKind_Info5", "D4114");
                Input_CarKind_Info6 = m_PlcConfig.GetString("Input", "CarKind_Info6", "D4115");
                Input_CarKind_Info7 = m_PlcConfig.GetString("Input", "CarKind_Info7", "D4116");
                Input_CarKind_Info8 = m_PlcConfig.GetString("Input", "CarKind_Info8", "D4117");
                Input_CarKind_Info9 = m_PlcConfig.GetString("Input", "CarKind_Info9", "D4118");
                Input_CarKind_Info10 = m_PlcConfig.GetString("Input", "CarKind_Info10", "D4119");
                Input_CarKind_Info11 = m_PlcConfig.GetString("Input", "CarKind_Info11", "D4120");
                Input_CarKind_Info12 = m_PlcConfig.GetString("Input", "CarKind_Info12", "D4121");
                Input_CarKind_Info13 = m_PlcConfig.GetString("Input", "CarKind_Info13", "D4122");
                Input_CarKind_Info14 = m_PlcConfig.GetString("Input", "CarKind_Info14", "D4123");
                Input_CarKind_Info15 = m_PlcConfig.GetString("Input", "CarKind_Info15", "D4124");
                Input_CarKind_Info16 = m_PlcConfig.GetString("Input", "CarKind_Info16", "D4125");
                Input_CarKind_Info17 = m_PlcConfig.GetString("Input", "CarKind_Info17", "D4126");
                Input_CarKind_Info18 = m_PlcConfig.GetString("Input", "CarKind_Info18", "D4127");
                Input_CarKind_Info19 = m_PlcConfig.GetString("Input", "CarKind_Info19", "D4128");
                Input_CarKind_Info20 = m_PlcConfig.GetString("Input", "CarKind_Info20", "D4129");

                // Output Address
                Output_HB = m_PlcConfig.GetString("Output", "HB", "D4000");
                Output_ReadyComplete = m_PlcConfig.GetString("Output", "ReadyComplete", "D4001");
                Output_VisionBusy = m_PlcConfig.GetString("Output", "VisionBusy", "D4002");
                Output_Vision_1st_CompPos1 = m_PlcConfig.GetString("Output", "Vision_1st_CompPos1", "D4030");
                Output_Vision_1st_CompPos2 = m_PlcConfig.GetString("Output", "Vision_1st_CompPos2", "D4031");
                Output_Vision_1st_CompPos3 = m_PlcConfig.GetString("Output", "Vision_1st_CompPos3", "D4032");
                Output_Vision_1st_CompPos4 = m_PlcConfig.GetString("Output", "Vision_1st_CompPos4", "D4033");
                Output_Vision_1st_CompPos5 = m_PlcConfig.GetString("Output", "Vision_1st_CompPos5", "D4034");
                Output_Vision_1st_CompPos6 = m_PlcConfig.GetString("Output", "Vision_1st_CompPos6", "D4035");
                Output_Vision_1st_CompPos7 = m_PlcConfig.GetString("Output", "Vision_1st_CompPos7", "D4036");
                Output_Vision_1st_CompPos8 = m_PlcConfig.GetString("Output", "Vision_1st_CompPos8", "D4037");
                Output_Vision_1st_CompPos9 = m_PlcConfig.GetString("Output", "Vision_1st_CompPos9", "D4038");
                Output_Vision_1st_CompPos10 = m_PlcConfig.GetString("Output", "Vision_1st_CompPos10", "D4039");
                Output_Vision_1st_ResultPos1_OK = m_PlcConfig.GetString("Output", "Vision_1st_ResultPos1_OK", "D4040");
                Output_Vision_1st_ResultPos2_OK = m_PlcConfig.GetString("Output", "Vision_1st_ResultPos2_OK", "D4041");
                Output_Vision_1st_ResultPos3_OK = m_PlcConfig.GetString("Output", "Vision_1st_ResultPos3_OK", "D4042");
                Output_Vision_1st_ResultPos4_OK = m_PlcConfig.GetString("Output", "Vision_1st_ResultPos4_OK", "D4043");
                Output_Vision_1st_ResultPos5_OK = m_PlcConfig.GetString("Output", "Vision_1st_ResultPos5_OK", "D4044");
                Output_Vision_1st_ResultPos6_OK = m_PlcConfig.GetString("Output", "Vision_1st_ResultPos6_OK", "D4045");
                Output_Vision_1st_ResultPos7_OK = m_PlcConfig.GetString("Output", "Vision_1st_ResultPos7_OK", "D4046");
                Output_Vision_1st_ResultPos8_OK = m_PlcConfig.GetString("Output", "Vision_1st_ResultPos8_OK", "D4047");
                Output_Vision_1st_ResultPos9_OK = m_PlcConfig.GetString("Output", "Vision_1st_ResultPos9_OK", "D4048");
                Output_Vision_1st_ResultPos10_OK = m_PlcConfig.GetString("Output", "Vision_1st_ResultPos10_OK", "D4049");
                Output_Vision_1st_ResultPos1_NG = m_PlcConfig.GetString("Output", "Vision_1st_ResultPos1_NG", "D4050");
                Output_Vision_1st_ResultPos2_NG = m_PlcConfig.GetString("Output", "Vision_1st_ResultPos2_NG", "D4051");
                Output_Vision_1st_ResultPos3_NG = m_PlcConfig.GetString("Output", "Vision_1st_ResultPos3_NG", "D4052");
                Output_Vision_1st_ResultPos4_NG = m_PlcConfig.GetString("Output", "Vision_1st_ResultPos4_NG", "D4053");
                Output_Vision_1st_ResultPos5_NG = m_PlcConfig.GetString("Output", "Vision_1st_ResultPos5_NG", "D4054");
                Output_Vision_1st_ResultPos6_NG = m_PlcConfig.GetString("Output", "Vision_1st_ResultPos6_NG", "D4055");
                Output_Vision_1st_ResultPos7_NG = m_PlcConfig.GetString("Output", "Vision_1st_ResultPos7_NG", "D4056");
                Output_Vision_1st_ResultPos8_NG = m_PlcConfig.GetString("Output", "Vision_1st_ResultPos8_NG", "D4057");
                Output_Vision_1st_ResultPos9_NG = m_PlcConfig.GetString("Output", "Vision_1st_ResultPos9_NG", "D4058");
                Output_Vision_1st_ResultPos10_NG = m_PlcConfig.GetString("Output", "Vision_1st_ResultPos10_NG", "D4059");
                Output_Vision_2nd_CompPos1 = m_PlcConfig.GetString("Output", "Vision_2nd_CompPos1", "D4060");
                Output_Vision_2nd_CompPos2 = m_PlcConfig.GetString("Output", "Vision_2nd_CompPos2", "D4061");
                Output_Vision_2nd_CompPos3 = m_PlcConfig.GetString("Output", "Vision_2nd_CompPos3", "D4062");
                Output_Vision_2nd_CompPos4 = m_PlcConfig.GetString("Output", "Vision_2nd_CompPos4", "D4063");
                Output_Vision_2nd_CompPos5 = m_PlcConfig.GetString("Output", "Vision_2nd_CompPos5", "D4064");
                Output_Vision_2nd_CompPos6 = m_PlcConfig.GetString("Output", "Vision_2nd_CompPos6", "D4065");
                Output_Vision_2nd_CompPos7 = m_PlcConfig.GetString("Output", "Vision_2nd_CompPos7", "D4066");
                Output_Vision_2nd_CompPos8 = m_PlcConfig.GetString("Output", "Vision_2nd_CompPos8", "D4067");
                Output_Vision_2nd_CompPos9 = m_PlcConfig.GetString("Output", "Vision_2nd_CompPos9", "D4068");
                Output_Vision_2nd_CompPos10 = m_PlcConfig.GetString("Output", "Vision_2nd_CompPos10", "D4069");
                Output_Vision_2nd_ResultPos1_OK = m_PlcConfig.GetString("Output", "Vision_2nd_ResultPos1_OK", "D4070");
                Output_Vision_2nd_ResultPos2_OK = m_PlcConfig.GetString("Output", "Vision_2nd_ResultPos2_OK", "D4071");
                Output_Vision_2nd_ResultPos3_OK = m_PlcConfig.GetString("Output", "Vision_2nd_ResultPos3_OK", "D4072");
                Output_Vision_2nd_ResultPos4_OK = m_PlcConfig.GetString("Output", "Vision_2nd_ResultPos4_OK", "D4073");
                Output_Vision_2nd_ResultPos5_OK = m_PlcConfig.GetString("Output", "Vision_2nd_ResultPos5_OK", "D4074");
                Output_Vision_2nd_ResultPos6_OK = m_PlcConfig.GetString("Output", "Vision_2nd_ResultPos6_OK", "D4075");
                Output_Vision_2nd_ResultPos7_OK = m_PlcConfig.GetString("Output", "Vision_2nd_ResultPos7_OK", "D4076");
                Output_Vision_2nd_ResultPos8_OK = m_PlcConfig.GetString("Output", "Vision_2nd_ResultPos8_OK", "D4077");
                Output_Vision_2nd_ResultPos9_OK = m_PlcConfig.GetString("Output", "Vision_2nd_ResultPos9_OK", "D4078");
                Output_Vision_2nd_ResultPos10_OK = m_PlcConfig.GetString("Output", "Vision_2nd_ResultPos10_OK", "D4079");
                Output_Vision_2nd_ResultPos1_NG = m_PlcConfig.GetString("Output", "Vision_2nd_ResultPos1_NG", "D4080");
                Output_Vision_2nd_ResultPos2_NG = m_PlcConfig.GetString("Output", "Vision_2nd_ResultPos2_NG", "D4081");
                Output_Vision_2nd_ResultPos3_NG = m_PlcConfig.GetString("Output", "Vision_2nd_ResultPos3_NG", "D4082");
                Output_Vision_2nd_ResultPos4_NG = m_PlcConfig.GetString("Output", "Vision_2nd_ResultPos4_NG", "D4083");
                Output_Vision_2nd_ResultPos5_NG = m_PlcConfig.GetString("Output", "Vision_2nd_ResultPos5_NG", "D4084");
                Output_Vision_2nd_ResultPos6_NG = m_PlcConfig.GetString("Output", "Vision_2nd_ResultPos6_NG", "D4085");
                Output_Vision_2nd_ResultPos7_NG = m_PlcConfig.GetString("Output", "Vision_2nd_ResultPos7_NG", "D4086");
                Output_Vision_2nd_ResultPos8_NG = m_PlcConfig.GetString("Output", "Vision_2nd_ResultPos8_NG", "D4087");
                Output_Vision_2nd_ResultPos9_NG = m_PlcConfig.GetString("Output", "Vision_2nd_ResultPos9_NG", "D4088");
                Output_Vision_2nd_ResultPos10_NG = m_PlcConfig.GetString("Output", "Vision_2nd_ResultPos10_NG", "D4089");
            }
        }
    }
}
