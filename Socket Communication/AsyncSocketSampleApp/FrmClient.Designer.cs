namespace AsyncSocketSampleApp
{
    partial class FrmClient
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tb_ip = new System.Windows.Forms.TextBox();
            this.tb_port = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(418, 76);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(12, 94);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(418, 76);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(12, 176);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(418, 76);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(12, 431);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(417, 165);
            this.txtMessage.TabIndex = 3;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 358);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(418, 21);
            this.textBox1.TabIndex = 4;
            // 
            // tb_ip
            // 
            this.tb_ip.Location = new System.Drawing.Point(13, 258);
            this.tb_ip.Name = "tb_ip";
            this.tb_ip.Size = new System.Drawing.Size(418, 21);
            this.tb_ip.TabIndex = 4;
            this.tb_ip.Text = "127.0.0.1";
            // 
            // tb_port
            // 
            this.tb_port.Location = new System.Drawing.Point(11, 285);
            this.tb_port.Name = "tb_port";
            this.tb_port.Size = new System.Drawing.Size(418, 21);
            this.tb_port.TabIndex = 4;
            this.tb_port.Text = "9988";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(451, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(418, 76);
            this.button1.TabIndex = 2;
            this.button1.Text = "start,model1,";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(451, 94);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(418, 76);
            this.button2.TabIndex = 2;
            this.button2.Text = "verify,1,1,";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(451, 176);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(418, 76);
            this.button3.TabIndex = 2;
            this.button3.Text = "verify,2,1,";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(451, 258);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(418, 76);
            this.button4.TabIndex = 2;
            this.button4.Text = "verify,3,1,";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(451, 340);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(418, 76);
            this.button5.TabIndex = 2;
            this.button5.Text = "verify,4,1,";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(451, 503);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(418, 76);
            this.button6.TabIndex = 2;
            this.button6.Text = "end,";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(451, 421);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(418, 76);
            this.button7.TabIndex = 2;
            this.button7.Text = "result,";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(891, 12);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(418, 76);
            this.button8.TabIndex = 2;
            this.button8.Text = "start,model1,";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.loop_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(891, 94);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(418, 76);
            this.button9.TabIndex = 2;
            this.button9.Text = "start,model1,";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.stopLoop);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(875, 503);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(418, 76);
            this.button10.TabIndex = 2;
            this.button10.Text = "SVM CAPTURE,";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // FrmClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1360, 634);
            this.Controls.Add(this.tb_port);
            this.Controls.Add(this.tb_ip);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnConnect);
            this.Name = "FrmClient";
            this.Text = "클라이언트";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox tb_ip;
        private System.Windows.Forms.TextBox tb_port;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
    }
}

