using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using AsyncSocket;

namespace AsyncSocketServerApp
{
    public delegate void UpdateText(Control ctrl, string text);

    public partial class FrmServer : Form
    {
        private AsyncSocketServer server;
        private List<AsyncSocketClient> clientList;
        private int id;

        public FrmServer()
        {
            InitializeComponent();

            server = new AsyncSocketServer(15000);
            server.OnAccept += new AsyncSocketAcceptEventHandler(OnAccept);
            server.OnError += new AsyncSocketErrorEventHandler(OnError);

            clientList = new List<AsyncSocketClient>(100);
            id = 0;
        }

        public void UpdateTextFunc(Control ctrl, string text)
        {
            if (ctrl.InvokeRequired)
            {
                ctrl.Invoke(new UpdateText(UpdateTextFunc), new object[] { ctrl, text });
            }
            else
                ctrl.Text = text;
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            server.Listen(IPAddress.Any);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            server.Stop();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string message = "hello\n";            

            for (int i = 0; i < clientList.Count; i++)
            {
                clientList[i].Send(Encoding.Default.GetBytes(message));
            }
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

            UpdateTextFunc(txtMessage, "HOST -> PC: Accepted\n");
        }

        private void OnError(object sender, AsyncSocketErrorEventArgs e)
        {
            UpdateTextFunc(txtMessage, "HOST -> PC: Error ID: " + e.ID.ToString() + "Error Message: " + e.AsyncSocketException.ToString() + "\n");

            for(int i = 0; i < clientList.Count; i ++)
            {
                if(clientList[i].ID == e.ID)
                {
                    clientList.Remove(clientList[i]);

                    break;
                }
            }
        }

        private void OnReceive(object sender, AsyncSocketReceiveEventArgs e)
        {
            UpdateTextFunc(txtMessage, "HOST -> PC: Receive ID: " + e.ID.ToString() + " -Bytes received: " + e.ReceiveBytes.ToString() + "\n");
        }

        private void OnSend(object sender, AsyncSocketSendEventArgs e)
        {
            UpdateTextFunc(txtMessage, "PC -> HOST: Send ID: " + e.ID.ToString() + " -Bytes sent: " + e.SendBytes.ToString() + "\n");
        }

        private void OnClose(object sender, AsyncSocketConnectionEventArgs e)
        {
            UpdateTextFunc(txtMessage, "HOST -> PC: Closed ID: " + e.ID.ToString() + "\n");
        }

        private void OnConnet(object sender, AsyncSocketConnectionEventArgs e)
        {
            UpdateTextFunc(txtMessage, "HOST -> PC: Connected ID: " + e.ID.ToString() + "\n");
        }

        private void FrmServer_Load(object sender, EventArgs e)
        {

        }       

    } // end of class FrmServer
} // end of namespace