using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AsyncSocket;
using System.Threading;

namespace AsyncSocketSampleApp
{
    public delegate void UpdateText(Control ctrl, string text);

    public partial class FrmClient : Form
    {
        private AsyncSocketClient sock = null;

        public FrmClient()
        {
            InitializeComponent();

            sock = new AsyncSocketClient(0);

            // 이벤트 핸들러 재정의
            sock.OnConnet += new AsyncSocketConnectEventHandler(OnConnet);
            sock.OnClose += new AsyncSocketCloseEventHandler(OnClose);
            sock.OnSend += new AsyncSocketSendEventHandler(OnSend);
            sock.OnReceive += new AsyncSocketReceiveEventHandler(OnReceive);
            sock.OnError += new AsyncSocketErrorEventHandler(OnError);
        }

        private void OnConnet(object sender, AsyncSocketConnectionEventArgs e)
        {
            UpdateTextFunc(txtMessage, "HOST -> PC: Connected ID: " + e.ID.ToString() + "\n");
        }

        private void OnClose(object sender, AsyncSocketConnectionEventArgs e)
        {
            UpdateTextFunc(txtMessage, "HOST -> PC: Closed ID: " + e.ID.ToString() + "\n");
        }

        private void OnSend(object sender, AsyncSocketSendEventArgs e)
        {
            UpdateTextFunc(txtMessage, "PC -> HOST: Send ID: " + e.ID.ToString() + " Bytes sent: " + e.SendBytes.ToString() + "\n");
        }

        private void OnReceive(object sender, AsyncSocketReceiveEventArgs e)
        {
            UpdateTextFunc(txtMessage, "HOST -> PC: Receive ID: " + e.ID.ToString() + " Bytes received: " + e.ReceiveBytes.ToString() +
                " Data: " + new string(Encoding.Default.GetChars(e.ReceiveData)) + "\n");
        }

        private void OnError(object sender, AsyncSocketErrorEventArgs e)
        {
            UpdateTextFunc(txtMessage, "HOST -> PC: Error ID: " + e.ID.ToString() + " Error Message: " + e.AsyncSocketException.ToString() + "\n");
        }       

        private void btnConnect_Click(object sender, EventArgs e)
        {
            sock.Connect(tb_ip.Text, Convert.ToInt32(tb_port.Text));
            //sock.Connect("127.0.0.1", 15000);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            sock.Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            string message = textBox1.Text + "\n";

            if (btn.Text != "Send")
            {
                message = btn.Text + "\n";
            }

            sock.Send(Encoding.Default.GetBytes(message));
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

        bool isLoop = false;

        private void loop_Click(object sender, EventArgs e)
        {
            isLoop = true;
            while (isLoop)
            {
                try
                {
                    sock.Send(Encoding.Default.GetBytes("start,model1,1,"));
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                try
                {
                    Thread.Sleep(12000);
                    sock.Send(Encoding.Default.GetBytes("end,"));
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
        }

        private void stopLoop(object sender, EventArgs e)
        {
            isLoop = false;
        }

    } // end of class
} // end of namespace