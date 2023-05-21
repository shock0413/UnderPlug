using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PLC
{
    public class PLCManager
    {
        public delegate void OnAsyncTickEventHandler(StructPLCData data);
        public event OnAsyncTickEventHandler OnAsyncTickEvent = delegate { };

        public delegate void OnConnectPlcEventHandler();
        public event OnConnectPlcEventHandler OnConnectPlcEvent = delegate { };

        private readonly ACTMULTILib.ActEasyIF axActEasyIF1 = new ACTMULTILib.ActEasyIF();

        private bool isClose = false;
         
        private bool isConnected = false;
        public bool IsConnected { get { return isConnected; } }

        private string heartbeatAddress = null;
        readonly int tickInterval = 500;

        public PLCManager()
        {
            
        }

        public PLCManager(int stationNum)
        {
            axActEasyIF1.ActLogicalStationNumber = stationNum;
        }

        public bool Start()
        {
            if (PLC_OPEN())
            {

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
                catch(Exception ex)
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
            StructPLCData data = new StructPLCData();

            /*
            int[] tmp_receive = PLC_READ(m_plcAddress.Input_HB, 24);
            if (tmp_receive == null) return null;

            int[] inputDatas = tmp_receive;
            
            tmp_receive = PLC_READ(m_plcAddress.Output_HB, 20);
            int[] outputDatas = tmp_receive;



            data.inputData = inputDatas;
            data.outputData = outputDatas;
                        */
            return data;
        }

        private bool PLC_OPEN()
        {
            bool iret = axActEasyIF1.Open() == 0 ? true : false;

            return iret;

        }

        private bool PLC_CLOSE()
        {
            bool iret = axActEasyIF1.Close() == 0 ? false : true;

            return iret;
        }

        public int[] PLC_READ(string startAdd, int size)
        {
            string szDeviceList = "";
            int[] IData;
            long IRet1;
            char tmp = (char)10;

            IData = new int[size];
            int sp = Int32.Parse(startAdd.ToLower().Replace("d", ""));

            szDeviceList += startAdd;
            for (int i = 1; i < size; i++)
                //szDeviceList += (Strings.Chr(10) + "D" + (sp+i).ToString());
                szDeviceList += (tmp.ToString() + "D" + (sp + i).ToString());

            //영역 읽을 경우
            axActEasyIF1.ReadDeviceRandom(szDeviceList, size, out IData[0]);

            return IData;
        }

        public bool PLC_WRITE(string startAdd, int size, int[] data)
        {
            string szDeviceList = "";
            long IRet1;
            char tmp = (char)10;

            int sp = Int32.Parse(startAdd.ToLower().Replace("d", ""));

            szDeviceList += startAdd;
            for (int i = 1; i < size; i++)
                szDeviceList += (tmp + "D" + (sp + i).ToString());

            IRet1 = axActEasyIF1.WriteDeviceRandom(szDeviceList, size, ref data[0]);
 
            return IRet1 == 0 ? true : false;
        }

        #endregion
    }
}
