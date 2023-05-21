using AsyncFrameSocket;
using PLC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Hansero;

namespace PLCCommunicator
{
    public partial class MainEngine : ViewModelBase
    {
        public System.Windows.Forms.NotifyIcon notify;
        readonly Config config = new Config();

        PLCManager plc;

        private AsyncSocketServer server;
        private List<AsyncSocketClient> clientList;
        private int id;

        private bool isExit = false;

        private LogManager logManager = new LogManager(true, true);

        public class AddrInfo
        {
            public string StartAddr { get; set; }
            public int Size { get; set; }

            public int[] ReadData { get; set; }
        }

        List<AddrInfo> addrInfoDic = new List<AddrInfo>();

        public MainEngine()
        {
            InitPLC();
            InitPlcAddress();
            InitSocketServer();
        }

        private void InitPLC()
        {
            plc = new PLCManager(config.StationNumber);
            bool plcState = plc.Start();
            if(plcState)
            {
                PLCState = ConnectionState.Connected;
            }
            else
            {
                PLCState = ConnectionState.UnConnected;
            }

            new Thread(new ThreadStart(() =>
            {
                while (!isExit)
                {

                    addrInfoDic.ToList().ForEach(x =>
                    {
                        AddrInfo info = x;

                        int[] readData = plc.PLC_READ(info.StartAddr.ToString(), info.Size);

                        info.ReadData = readData;
                    });

                    Thread.Sleep(200);
                }
            })).Start();

            new Thread(new ThreadStart(() =>
            {
                while (!isExit)
                {
                    try
                    {
                        if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Log"))
                        {
                            DateTime curDateTime = DateTime.Now;
                            int addDays = config.SaveDays * -1;
                            curDateTime = curDateTime.AddDays(addDays);
                            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Log");

                            files.ToList().ForEach(x =>
                            {
                                DateTime result = new DateTime();
                                bool completed = DateTime.TryParse(Path.GetFileNameWithoutExtension(x), out result);

                                if (completed)
                                {
                                    if (result < curDateTime)
                                    {
                                        File.Delete(x);
                                    }
                                }
                            });
                        }

                        Thread.Sleep(5000);
                    }
                    catch
                    {

                    }
                }
            })).Start();
        }

        int beforeValue;

        private void InitPlcAddress()
        {
            InputAddressList.Clear();
            config.InputAddress.ToList().ForEach(x =>
            {
                StructPLCAddress address = new StructPLCAddress
                {
                    AddressType = AddressType.Input,
                    Device = x.Value,
                    DeviceNumber = Convert.ToInt32(x.Value.Replace("D", "")),
                    Name = x.Key
                };
                InputAddressList.Add(address);
            });

            OutputAddressList.Clear();
            config.OutputAddress.ToList().ForEach(x =>
            {
                StructPLCAddress structPLCAddress = new StructPLCAddress
                {
                    AddressType = AddressType.Output,
                    Device = x.Value,
                    DeviceNumber = Convert.ToInt32(x.Value.Replace("D", "")),
                    Name = x.Key
                };
                StructPLCAddress address = structPLCAddress;
                OutputAddressList.Add(address);
            });
        }

        private void InitSocketServer()
        {
            server = new AsyncSocketServer(config.ServerPort);
            server.OnAccept += new AsyncSocketAcceptEventHandler(OnAccept);
            server.OnError += new AsyncSocketErrorEventHandler(OnError);

            clientList = new List<AsyncSocketClient>();
            id = 0;

            SocketState = ConnectionState.UnConnected;

            server.Listen(IPAddress.Any);
        }


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
            clientList.Add(worker);
        }

        private void OnError(object sender, AsyncSocketErrorEventArgs e)
        {

        }

        private void OnReceive(object sender, AsyncSocketReceiveEventArgs e)
        {
            try
            {
                string receivedString = Encoding.Default.GetString(e.ReceiveData, 0, e.ReceiveBytes);
                string[] arg = receivedString.Split(',');

                AddCommunicateMsg("Receive : " + receivedString);

                if (arg.Length > 0)
                {
                    if(arg[0] == config.InitCommand)
                    {
 

                       
                    }
                    else if (arg[0] == config.ReadCommand)
                    {
                        try
                        {
                            int id = Convert.ToInt32(arg[1]);
                            string startAddr = arg[2];
                            int size = Convert.ToInt32(arg[3]);

                            int[] readData = null;

                            bool isDef = false;
                            addrInfoDic.ToList().ForEach(x =>
                            {
                                if (x.StartAddr.ToString() == startAddr && x.Size == size)
                                {
                                    isDef = true;
                                }
                            });

                            if (!isDef)
                            {
                                AddrInfo info = new AddrInfo()
                                {
                                    StartAddr = (startAddr),
                                    Size = size
                                };
                                addrInfoDic.Add(info);
                            }

                            addrInfoDic.ToList().ForEach(x =>
                            {
                                if (x.StartAddr.ToString() == startAddr && x.Size == size)
                                {
                                    readData = x.ReadData;
                                }
                            });

                            string str = arg[0] + "," + id;
                            readData.ToList().ForEach(data => str += "," + data);

                            if (size == 20)
                            {
                                string subStr = startAddr.Substring(1, startAddr.Length - 1);
                                int result = 0;
                                bool completed = Int32.TryParse(subStr, out result);

                                //if (completed)
                                //{
                                //    for (int i = 0; i < size; i++)
                                //    {
                                //        logManager.Info(arg[0] + " : D" + result++ + "," + readData[i]);
                                //    }
                                //}
                            }
                            else if (size == 5)
                            {
                                string subStr = startAddr.Substring(1, startAddr.Length - 1);
                                int result = 0;
                                bool completed = Int32.TryParse(subStr, out result);

                                //if (completed)
                                //{
                                //    for (int i = 0; i < size; i++)
                                //    {
                                //        logManager.Info(arg[0] + " : D" + result++ + "," + readData[i]);
                                //    }
                                //}
                            }

                            SendStr(e.ID, str);
                            Console.WriteLine(str);
                            // AddCommunicateMsg(str);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else if (arg[0] == config.WriteCommand)
                    {
                        try
                        {
                            int id = Convert.ToInt32(arg[1]);
                            string writeAddr = arg[2];
                            int value = Convert.ToInt32(arg[3]);

                             
                            Console.WriteLine(id + " : " + writeAddr + " : " + value);

                            plc.PLC_WRITE(writeAddr, 1, new int[] { value });


                            
                            string str = arg[0] + "," + id + "," + "1";
                            SendStr(e.ID, str);
                            Console.WriteLine(str);
                            // AddCommunicateMsg(str);
                            //logManager.Info(arg[0] + " : " + writeAddr + "," + value);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("PLC 쓰기 실패 : " + ex.Message);
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch
            {

            }
        }

        private void OnSend(object sender, AsyncSocketSendEventArgs e)
        {
            
        }

        private void OnClose(object sender, AsyncSocketConnectionEventArgs e)
        {
            
        }

        private void OnConnet(object sender, AsyncSocketConnectionEventArgs e)
        {
            SocketState = ConnectionState.Connected;
        }

        private void SendStr(int id, string str)
        {
            List<AsyncSocketClient> temp = clientList.Where(client => client.ID == id).ToList();
            if (temp.Count > 0)
            {
                temp[0].Send(Encoding.Default.GetBytes(str));
            }
        }

        private void AddCommunicateMsg(string msg)
        {
            //Application.Current.Dispatcher.Invoke(() =>
            //{
            //    CommunicateMessageList.Insert(0, DateTime.Now.ToString("yyyyMMdd HHmmss") + "\t" + msg);
            //    while(CommunicateMessageList.Count > config.MessageLimitCount)
            //    {
            //        CommunicateMessageList.RemoveAt(config.MessageLimitCount);
            //    }
            //});
        }

        public void WindowLoaded()
        {
            notify = new System.Windows.Forms.NotifyIcon
            {
                Text = "PLC 통신",
                Icon = Properties.Resources.logo1,
                Visible = true
            };
            notify.DoubleClick += Notify_DoubleClick;

            System.Windows.Forms.MenuItem item = new System.Windows.Forms.MenuItem
            {
                Text = "종료"
            };

            item.Click += (s, e) =>
            {
                isExit = true;
                Application.Current.Shutdown();
            };

            System.Windows.Forms.ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
            contextMenu.MenuItems.Add(item);
            notify.ContextMenu = contextMenu;
        }

        public void WriteManual()
        {
            plc.PLC_WRITE(MenualWriteDevice, 1, new int[] { MenualWriteValue }); ;
        }

        private void Notify_DoubleClick(object sender, EventArgs e)
        {
            if(WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Minimized;
            }
            
        }

        public void WindowClosing(CancelEventArgs args)
        {
            WindowState = System.Windows.WindowState.Minimized;
            args.Cancel = true;
            return;
        }
         
    }
}
