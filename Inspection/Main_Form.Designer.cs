namespace Inspection
{
    partial class Main_Form
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_Form));
            this.lb_MoveY = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lb_MoveX = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lb_Result = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lb_Title = new System.Windows.Forms.Label();
            this.lb_Pass = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_CorrectY = new System.Windows.Forms.Label();
            this.tb_CorrectX = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.panel14 = new System.Windows.Forms.Panel();
            this.panel15 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.panel16 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.panel17 = new System.Windows.Forms.Panel();
            this.lb_Kind = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cogDisplay1 = new Cognex.VisionPro.Display.CogDisplay();
            this.btn_Setting = new System.Windows.Forms.Button();
            this.btn_CamSetting = new System.Windows.Forms.Button();
            this.btn_Report = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_Location = new System.Windows.Forms.Label();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel18 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.panel19 = new System.Windows.Forms.Panel();
            this.lb_Direction = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lb_BodyNumber = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel14.SuspendLayout();
            this.panel15.SuspendLayout();
            this.panel16.SuspendLayout();
            this.panel17.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel12.SuspendLayout();
            this.panel18.SuspendLayout();
            this.panel19.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // lb_MoveY
            // 
            this.lb_MoveY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.lb_MoveY.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_MoveY.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.lb_MoveY.ForeColor = System.Drawing.Color.White;
            this.lb_MoveY.Location = new System.Drawing.Point(57, 129);
            this.lb_MoveY.Name = "lb_MoveY";
            this.lb_MoveY.Size = new System.Drawing.Size(157, 32);
            this.lb_MoveY.TabIndex = 0;
            this.lb_MoveY.Text = "0";
            this.lb_MoveY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(14, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "Y :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lb_MoveX
            // 
            this.lb_MoveX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.lb_MoveX.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_MoveX.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.lb_MoveX.ForeColor = System.Drawing.Color.White;
            this.lb_MoveX.Location = new System.Drawing.Point(57, 20);
            this.lb_MoveX.Name = "lb_MoveX";
            this.lb_MoveX.Size = new System.Drawing.Size(157, 32);
            this.lb_MoveX.TabIndex = 0;
            this.lb_MoveX.Text = "0";
            this.lb_MoveX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(14, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "X :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(220, 18);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 34);
            this.label10.TabIndex = 0;
            this.label10.Text = "mm";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(220, 127);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 34);
            this.label8.TabIndex = 0;
            this.label8.Text = "mm";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lb_Result
            // 
            this.lb_Result.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.lb_Result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_Result.Font = new System.Drawing.Font("Times New Roman", 50F, System.Drawing.FontStyle.Bold);
            this.lb_Result.ForeColor = System.Drawing.Color.Green;
            this.lb_Result.Location = new System.Drawing.Point(0, 0);
            this.lb_Result.Name = "lb_Result";
            this.lb_Result.Size = new System.Drawing.Size(270, 99);
            this.lb_Result.TabIndex = 0;
            this.lb_Result.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.panel1.Controls.Add(this.lb_Title);
            this.panel1.Controls.Add(this.lb_Pass);
            this.panel1.Location = new System.Drawing.Point(386, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(763, 145);
            this.panel1.TabIndex = 151;
            // 
            // lb_Title
            // 
            this.lb_Title.BackColor = System.Drawing.Color.Transparent;
            this.lb_Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F, System.Drawing.FontStyle.Bold);
            this.lb_Title.ForeColor = System.Drawing.Color.Gainsboro;
            this.lb_Title.Location = new System.Drawing.Point(3, 4);
            this.lb_Title.Name = "lb_Title";
            this.lb_Title.Size = new System.Drawing.Size(793, 139);
            this.lb_Title.TabIndex = 147;
            this.lb_Title.Text = "플러그 장착 품질 보증 비전 시스템";
            this.lb_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lb_Pass
            // 
            this.lb_Pass.AutoSize = true;
            this.lb_Pass.BackColor = System.Drawing.Color.Black;
            this.lb_Pass.Font = new System.Drawing.Font("Impact", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Pass.ForeColor = System.Drawing.Color.Gold;
            this.lb_Pass.Location = new System.Drawing.Point(1370, 82);
            this.lb_Pass.Name = "lb_Pass";
            this.lb_Pass.Size = new System.Drawing.Size(132, 45);
            this.lb_Pass.TabIndex = 32;
            this.lb_Pass.Text = "ByPASS";
            this.lb_Pass.Visible = false;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel8.Controls.Add(this.label12);
            this.panel8.Controls.Add(this.label11);
            this.panel8.Controls.Add(this.label2);
            this.panel8.Controls.Add(this.lb_MoveY);
            this.panel8.Controls.Add(this.label3);
            this.panel8.Controls.Add(this.label9);
            this.panel8.Controls.Add(this.label10);
            this.panel8.Controls.Add(this.label7);
            this.panel8.Controls.Add(this.label8);
            this.panel8.Controls.Add(this.tb_CorrectY);
            this.panel8.Controls.Add(this.tb_CorrectX);
            this.panel8.Controls.Add(this.lb_MoveX);
            this.panel8.Location = new System.Drawing.Point(1618, 769);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(275, 229);
            this.panel8.TabIndex = 181;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(14, 172);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(38, 25);
            this.label12.TabIndex = 0;
            this.label12.Text = "C :";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(14, 70);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 25);
            this.label11.TabIndex = 0;
            this.label11.Text = "C :";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(220, 61);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 34);
            this.label9.TabIndex = 0;
            this.label9.Text = "mm";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(220, 170);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 34);
            this.label7.TabIndex = 0;
            this.label7.Text = "mm";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tb_CorrectY
            // 
            this.tb_CorrectY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.tb_CorrectY.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tb_CorrectY.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.tb_CorrectY.ForeColor = System.Drawing.Color.White;
            this.tb_CorrectY.Location = new System.Drawing.Point(57, 172);
            this.tb_CorrectY.Name = "tb_CorrectY";
            this.tb_CorrectY.Size = new System.Drawing.Size(157, 32);
            this.tb_CorrectY.TabIndex = 0;
            this.tb_CorrectY.Text = "0";
            this.tb_CorrectY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tb_CorrectX
            // 
            this.tb_CorrectX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.tb_CorrectX.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tb_CorrectX.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.tb_CorrectX.ForeColor = System.Drawing.Color.White;
            this.tb_CorrectX.Location = new System.Drawing.Point(57, 65);
            this.tb_CorrectX.Name = "tb_CorrectX";
            this.tb_CorrectX.Size = new System.Drawing.Size(157, 32);
            this.tb_CorrectX.TabIndex = 0;
            this.tb_CorrectX.Text = "0";
            this.tb_CorrectX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.Gray;
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel9.Controls.Add(this.label6);
            this.panel9.Location = new System.Drawing.Point(1618, 735);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(275, 35);
            this.panel9.TabIndex = 180;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(271, 31);
            this.label6.TabIndex = 0;
            this.label6.Text = "이동 거리";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel14
            // 
            this.panel14.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel14.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel14.Controls.Add(this.lb_Result);
            this.panel14.Location = new System.Drawing.Point(1618, 188);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(274, 103);
            this.panel14.TabIndex = 181;
            // 
            // panel15
            // 
            this.panel15.BackColor = System.Drawing.Color.Gray;
            this.panel15.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel15.Controls.Add(this.label13);
            this.panel15.Location = new System.Drawing.Point(1618, 155);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(274, 35);
            this.panel15.TabIndex = 180;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label13.Location = new System.Drawing.Point(0, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(270, 31);
            this.label13.TabIndex = 0;
            this.label13.Text = "결 과";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel16
            // 
            this.panel16.BackColor = System.Drawing.Color.Gray;
            this.panel16.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel16.Controls.Add(this.label14);
            this.panel16.Location = new System.Drawing.Point(1618, 299);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(275, 35);
            this.panel16.TabIndex = 180;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label14.Location = new System.Drawing.Point(0, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(271, 31);
            this.label14.TabIndex = 0;
            this.label14.Text = "기종";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel17
            // 
            this.panel17.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel17.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel17.Controls.Add(this.lb_Kind);
            this.panel17.Location = new System.Drawing.Point(1618, 333);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(275, 103);
            this.panel17.TabIndex = 181;
            // 
            // lb_Kind
            // 
            this.lb_Kind.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.lb_Kind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_Kind.Font = new System.Drawing.Font("Times New Roman", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Kind.ForeColor = System.Drawing.Color.White;
            this.lb_Kind.Location = new System.Drawing.Point(0, 0);
            this.lb_Kind.Name = "lb_Kind";
            this.lb_Kind.Size = new System.Drawing.Size(271, 99);
            this.lb_Kind.TabIndex = 0;
            this.lb_Kind.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.LightGray;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Controls.Add(this.cogDisplay1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1518, 815);
            this.panel5.TabIndex = 203;
            // 
            // cogDisplay1
            // 
            this.cogDisplay1.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay1.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay1.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay1.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay1.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogDisplay1.DoubleTapZoomCycleLength = 2;
            this.cogDisplay1.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay1.Location = new System.Drawing.Point(0, 0);
            this.cogDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay1.MouseWheelSensitivity = 1D;
            this.cogDisplay1.Name = "cogDisplay1";
            this.cogDisplay1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay1.OcxState")));
            this.cogDisplay1.Size = new System.Drawing.Size(1514, 811);
            this.cogDisplay1.TabIndex = 153;
            // 
            // btn_Setting
            // 
            this.btn_Setting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.btn_Setting.FlatAppearance.BorderSize = 0;
            this.btn_Setting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Setting.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Setting.ForeColor = System.Drawing.Color.White;
            this.btn_Setting.Location = new System.Drawing.Point(1517, 7);
            this.btn_Setting.Name = "btn_Setting";
            this.btn_Setting.Size = new System.Drawing.Size(185, 137);
            this.btn_Setting.TabIndex = 203;
            this.btn_Setting.Text = "설정";
            this.btn_Setting.UseVisualStyleBackColor = false;
            this.btn_Setting.Click += new System.EventHandler(this.btn_Setting_Click_1);
            // 
            // btn_CamSetting
            // 
            this.btn_CamSetting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.btn_CamSetting.FlatAppearance.BorderSize = 0;
            this.btn_CamSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_CamSetting.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_CamSetting.ForeColor = System.Drawing.Color.White;
            this.btn_CamSetting.Location = new System.Drawing.Point(1326, 7);
            this.btn_CamSetting.Name = "btn_CamSetting";
            this.btn_CamSetting.Size = new System.Drawing.Size(185, 137);
            this.btn_CamSetting.TabIndex = 204;
            this.btn_CamSetting.Text = "카메라설정";
            this.btn_CamSetting.UseVisualStyleBackColor = false;
            this.btn_CamSetting.Click += new System.EventHandler(this.btn_CamSetting_Click_1);
            // 
            // btn_Report
            // 
            this.btn_Report.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.btn_Report.FlatAppearance.BorderSize = 0;
            this.btn_Report.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Report.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Report.ForeColor = System.Drawing.Color.White;
            this.btn_Report.Location = new System.Drawing.Point(1708, 7);
            this.btn_Report.Name = "btn_Report";
            this.btn_Report.Size = new System.Drawing.Size(185, 137);
            this.btn_Report.TabIndex = 205;
            this.btn_Report.Text = "이력보기";
            this.btn_Report.UseVisualStyleBackColor = false;
            this.btn_Report.Click += new System.EventHandler(this.btn_Report_Click_1);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(3, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(377, 145);
            this.panel2.TabIndex = 206;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(72, 18);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(233, 91);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 145;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 9F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(88, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 15);
            this.label1.TabIndex = 144;
            this.label1.Text = "Machine Vision for every application";
            // 
            // lb_Location
            // 
            this.lb_Location.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_Location.Font = new System.Drawing.Font("Times New Roman", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Location.ForeColor = System.Drawing.Color.White;
            this.lb_Location.Location = new System.Drawing.Point(0, 0);
            this.lb_Location.Name = "lb_Location";
            this.lb_Location.Size = new System.Drawing.Size(375, 132);
            this.lb_Location.TabIndex = 0;
            this.lb_Location.Text = "LH";
            this.lb_Location.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel12.Controls.Add(this.lb_Location);
            this.panel12.Location = new System.Drawing.Point(269, 1036);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(379, 136);
            this.panel12.TabIndex = 183;
            this.panel12.Visible = false;
            // 
            // panel18
            // 
            this.panel18.BackColor = System.Drawing.Color.Gray;
            this.panel18.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel18.Controls.Add(this.label15);
            this.panel18.Location = new System.Drawing.Point(1618, 590);
            this.panel18.Name = "panel18";
            this.panel18.Size = new System.Drawing.Size(275, 35);
            this.panel18.TabIndex = 207;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label15.Location = new System.Drawing.Point(0, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(271, 31);
            this.label15.TabIndex = 0;
            this.label15.Text = "위치";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel19
            // 
            this.panel19.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel19.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel19.Controls.Add(this.lb_Direction);
            this.panel19.Location = new System.Drawing.Point(1618, 623);
            this.panel19.Name = "panel19";
            this.panel19.Size = new System.Drawing.Size(275, 103);
            this.panel19.TabIndex = 208;
            // 
            // lb_Direction
            // 
            this.lb_Direction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.lb_Direction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_Direction.Font = new System.Drawing.Font("Times New Roman", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_Direction.ForeColor = System.Drawing.Color.White;
            this.lb_Direction.Location = new System.Drawing.Point(0, 0);
            this.lb_Direction.Name = "lb_Direction";
            this.lb_Direction.Size = new System.Drawing.Size(271, 99);
            this.lb_Direction.TabIndex = 0;
            this.lb_Direction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 150);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1532, 847);
            this.tabControl1.TabIndex = 213;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel5);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1524, 821);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "플러그 검사";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.lb_BodyNumber);
            this.panel3.Location = new System.Drawing.Point(1618, 477);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(275, 103);
            this.panel3.TabIndex = 208;
            // 
            // lb_BodyNumber
            // 
            this.lb_BodyNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.lb_BodyNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_BodyNumber.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_BodyNumber.ForeColor = System.Drawing.Color.White;
            this.lb_BodyNumber.Location = new System.Drawing.Point(0, 0);
            this.lb_BodyNumber.Name = "lb_BodyNumber";
            this.lb_BodyNumber.Size = new System.Drawing.Size(271, 99);
            this.lb_BodyNumber.TabIndex = 0;
            this.lb_BodyNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Gray;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.label5);
            this.panel4.Location = new System.Drawing.Point(1618, 444);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(275, 35);
            this.panel4.TabIndex = 207;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(271, 31);
            this.label5.TabIndex = 0;
            this.label5.Text = "차대번호";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(69)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(1155, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(165, 137);
            this.button1.TabIndex = 204;
            this.button1.Text = "높이 리셋";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Main_Form
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.ClientSize = new System.Drawing.Size(1904, 1022);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel16);
            this.Controls.Add(this.panel17);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel18);
            this.Controls.Add(this.panel19);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.btn_Setting);
            this.Controls.Add(this.panel12);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_CamSetting);
            this.Controls.Add(this.btn_Report);
            this.Controls.Add(this.panel15);
            this.Controls.Add(this.panel14);
            this.Controls.Add(this.panel9);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hansero Techno Vision System.";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_Form_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel14.ResumeLayout(false);
            this.panel15.ResumeLayout(false);
            this.panel16.ResumeLayout(false);
            this.panel17.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel12.ResumeLayout(false);
            this.panel18.ResumeLayout(false);
            this.panel19.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lb_MoveY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lb_MoveX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lb_Result;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lb_Title;
        private System.Windows.Forms.Label lb_Pass;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panel17;
        private System.Windows.Forms.Label lb_Kind;
        private System.Windows.Forms.Panel panel5;
        private Cognex.VisionPro.Display.CogDisplay cogDisplay1;
        private System.Windows.Forms.Button btn_Setting;
        private System.Windows.Forms.Button btn_CamSetting;
        private System.Windows.Forms.Button btn_Report;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_Location;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel18;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel panel19;
        private System.Windows.Forms.Label lb_Direction;
        private DisplayPanel displayPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label tb_CorrectY;
        private System.Windows.Forms.Label tb_CorrectX;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lb_BodyNumber;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
    }
}

