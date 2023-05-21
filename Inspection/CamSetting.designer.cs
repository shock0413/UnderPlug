namespace Inspection
{
    partial class CamSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CamSetting));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cogDisplay1 = new Cognex.VisionPro.Display.CogDisplay();
            this.btn_Cancle = new System.Windows.Forms.Button();
            this.btn_Modify = new System.Windows.Forms.Button();
            this.panel_CamInfo = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.num_Expose = new System.Windows.Forms.NumericUpDown();
            this.num_Bright = new System.Windows.Forms.NumericUpDown();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btn_Calibration = new System.Windows.Forms.Button();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rb_UnCalibrated = new System.Windows.Forms.RadioButton();
            this.rb_Calibrated = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.rb_Linear = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tb_OriginY = new System.Windows.Forms.TextBox();
            this.tb_OriginX = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chk_SwapHandness = new System.Windows.Forms.CheckBox();
            this.tb_Rotation = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tb_TileSizeY = new System.Windows.Forms.TextBox();
            this.tb_TileSizeX = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_SaveMaster = new System.Windows.Forms.Button();
            this.chk_FiducialMark = new System.Windows.Forms.CheckBox();
            this.btn_SaveImage = new System.Windows.Forms.Button();
            this.btn_OneShot = new System.Windows.Forms.Button();
            this.btn_ContinuousShotStop = new System.Windows.Forms.Button();
            this.btn_ContinuousShot = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cb_kind = new System.Windows.Forms.ComboBox();
            this.cb_direction = new System.Windows.Forms.ComboBox();
            this.tabControl4 = new System.Windows.Forms.TabControl();
            this.btn_CalibImageView = new System.Windows.Forms.Button();
            this.btn_LightOn = new System.Windows.Forms.Button();
            this.btn_LightOff = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).BeginInit();
            this.panel_CamInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_Expose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Bright)).BeginInit();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabControl4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabControl1.ItemSize = new System.Drawing.Size(250, 30);
            this.tabControl1.Location = new System.Drawing.Point(11, 21);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(730, 460);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cogDisplay1);
            this.tabPage1.Font = new System.Drawing.Font("돋움체", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(722, 422);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "카메라";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cogDisplay1
            // 
            this.cogDisplay1.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay1.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay1.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay1.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay1.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay1.DoubleTapZoomCycleLength = 2;
            this.cogDisplay1.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay1.Location = new System.Drawing.Point(4, 4);
            this.cogDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay1.MouseWheelSensitivity = 1D;
            this.cogDisplay1.Name = "cogDisplay1";
            this.cogDisplay1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay1.OcxState")));
            this.cogDisplay1.Size = new System.Drawing.Size(715, 415);
            this.cogDisplay1.TabIndex = 0;
            // 
            // btn_Cancle
            // 
            this.btn_Cancle.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Cancle.Location = new System.Drawing.Point(180, 157);
            this.btn_Cancle.Name = "btn_Cancle";
            this.btn_Cancle.Size = new System.Drawing.Size(135, 36);
            this.btn_Cancle.TabIndex = 1;
            this.btn_Cancle.Text = "취 소";
            this.btn_Cancle.UseVisualStyleBackColor = true;
            this.btn_Cancle.Click += new System.EventHandler(this.btn_Cancle_Click);
            // 
            // btn_Modify
            // 
            this.btn_Modify.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Modify.Location = new System.Drawing.Point(25, 157);
            this.btn_Modify.Name = "btn_Modify";
            this.btn_Modify.Size = new System.Drawing.Size(149, 36);
            this.btn_Modify.TabIndex = 1;
            this.btn_Modify.Text = "수정";
            this.btn_Modify.UseVisualStyleBackColor = true;
            this.btn_Modify.Click += new System.EventHandler(this.btn_Modify_Click);
            // 
            // panel_CamInfo
            // 
            this.panel_CamInfo.Controls.Add(this.label10);
            this.panel_CamInfo.Controls.Add(this.label13);
            this.panel_CamInfo.Controls.Add(this.num_Expose);
            this.panel_CamInfo.Controls.Add(this.num_Bright);
            this.panel_CamInfo.Location = new System.Drawing.Point(6, 15);
            this.panel_CamInfo.Name = "panel_CamInfo";
            this.panel_CamInfo.Size = new System.Drawing.Size(347, 119);
            this.panel_CamInfo.TabIndex = 12;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(58, 28);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 16);
            this.label10.TabIndex = 6;
            this.label10.Text = "밝 기";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(58, 74);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(48, 16);
            this.label13.TabIndex = 8;
            this.label13.Text = "노 출";
            // 
            // num_Expose
            // 
            this.num_Expose.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.num_Expose.Location = new System.Drawing.Point(185, 66);
            this.num_Expose.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.num_Expose.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_Expose.Name = "num_Expose";
            this.num_Expose.Size = new System.Drawing.Size(143, 31);
            this.num_Expose.TabIndex = 5;
            this.num_Expose.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.num_Expose.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // num_Bright
            // 
            this.num_Bright.DecimalPlaces = 1;
            this.num_Bright.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.num_Bright.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.num_Bright.Location = new System.Drawing.Point(185, 20);
            this.num_Bright.Name = "num_Bright";
            this.num_Bright.Size = new System.Drawing.Size(143, 31);
            this.num_Bright.TabIndex = 5;
            this.num_Bright.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.ItemSize = new System.Drawing.Size(250, 30);
            this.tabControl2.Location = new System.Drawing.Point(756, 214);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(367, 251);
            this.tabControl2.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl2.TabIndex = 15;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabPage2.Controls.Add(this.btn_Cancle);
            this.tabPage2.Controls.Add(this.panel_CamInfo);
            this.tabPage2.Controls.Add(this.btn_Modify);
            this.tabPage2.Location = new System.Drawing.Point(4, 34);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(359, 213);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "카메라 세팅";
            // 
            // btn_Calibration
            // 
            this.btn_Calibration.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Calibration.Location = new System.Drawing.Point(518, 42);
            this.btn_Calibration.Name = "btn_Calibration";
            this.btn_Calibration.Size = new System.Drawing.Size(208, 49);
            this.btn_Calibration.TabIndex = 1;
            this.btn_Calibration.Text = "Compute Calibration";
            this.btn_Calibration.UseVisualStyleBackColor = true;
            this.btn_Calibration.Click += new System.EventHandler(this.btn_Calibration_Click);
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage3);
            this.tabControl3.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl3.ItemSize = new System.Drawing.Size(250, 30);
            this.tabControl3.Location = new System.Drawing.Point(11, 521);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(743, 324);
            this.tabControl3.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl3.TabIndex = 15;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabPage3.Controls.Add(this.groupBox4);
            this.tabPage3.Controls.Add(this.groupBox3);
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Controls.Add(this.groupBox5);
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Controls.Add(this.btn_Calibration);
            this.tabPage3.Controls.Add(this.btn_SaveMaster);
            this.tabPage3.Location = new System.Drawing.Point(4, 34);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(735, 286);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "켈리브레이션";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rb_UnCalibrated);
            this.groupBox4.Controls.Add(this.rb_Calibrated);
            this.groupBox4.ForeColor = System.Drawing.Color.Black;
            this.groupBox4.Location = new System.Drawing.Point(46, 139);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(275, 104);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "원점 공간";
            // 
            // rb_UnCalibrated
            // 
            this.rb_UnCalibrated.AutoSize = true;
            this.rb_UnCalibrated.Checked = true;
            this.rb_UnCalibrated.Location = new System.Drawing.Point(7, 34);
            this.rb_UnCalibrated.Name = "rb_UnCalibrated";
            this.rb_UnCalibrated.Size = new System.Drawing.Size(244, 28);
            this.rb_UnCalibrated.TabIndex = 0;
            this.rb_UnCalibrated.TabStop = true;
            this.rb_UnCalibrated.Text = "Uncalibrated Space";
            this.rb_UnCalibrated.UseVisualStyleBackColor = true;
            // 
            // rb_Calibrated
            // 
            this.rb_Calibrated.AutoSize = true;
            this.rb_Calibrated.Location = new System.Drawing.Point(6, 66);
            this.rb_Calibrated.Name = "rb_Calibrated";
            this.rb_Calibrated.Size = new System.Drawing.Size(268, 28);
            this.rb_Calibrated.TabIndex = 0;
            this.rb_Calibrated.Text = "Raw Calibrated Space";
            this.rb_Calibrated.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButton2);
            this.groupBox3.Controls.Add(this.rb_Linear);
            this.groupBox3.ForeColor = System.Drawing.Color.Black;
            this.groupBox3.Location = new System.Drawing.Point(46, 31);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(275, 104);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "켈리브레이션 모드";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(119, 52);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(136, 28);
            this.radioButton2.TabIndex = 0;
            this.radioButton2.Text = "Nonlinear";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // rb_Linear
            // 
            this.rb_Linear.AutoSize = true;
            this.rb_Linear.Checked = true;
            this.rb_Linear.Location = new System.Drawing.Point(7, 52);
            this.rb_Linear.Name = "rb_Linear";
            this.rb_Linear.Size = new System.Drawing.Size(100, 28);
            this.rb_Linear.TabIndex = 0;
            this.rb_Linear.TabStop = true;
            this.rb_Linear.Text = "Linear";
            this.rb_Linear.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tb_OriginY);
            this.groupBox2.Controls.Add(this.tb_OriginX);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox2.ForeColor = System.Drawing.Color.Black;
            this.groupBox2.Location = new System.Drawing.Point(328, 141);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(163, 102);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "켈리브레이션 원점";
            // 
            // tb_OriginY
            // 
            this.tb_OriginY.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_OriginY.Location = new System.Drawing.Point(68, 63);
            this.tb_OriginY.Name = "tb_OriginY";
            this.tb_OriginY.Size = new System.Drawing.Size(76, 31);
            this.tb_OriginY.TabIndex = 1;
            this.tb_OriginY.Text = "0";
            this.tb_OriginY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tb_OriginX
            // 
            this.tb_OriginX.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_OriginX.Location = new System.Drawing.Point(68, 28);
            this.tb_OriginX.Name = "tb_OriginX";
            this.tb_OriginX.Size = new System.Drawing.Size(76, 31);
            this.tb_OriginX.TabIndex = 1;
            this.tb_OriginX.Text = "0";
            this.tb_OriginX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 21);
            this.label6.TabIndex = 0;
            this.label6.Text = "Y";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 21);
            this.label7.TabIndex = 0;
            this.label7.Text = "X";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.chk_SwapHandness);
            this.groupBox5.Controls.Add(this.tb_Rotation);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox5.ForeColor = System.Drawing.Color.Black;
            this.groupBox5.Location = new System.Drawing.Point(497, 142);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(163, 101);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "회전";
            this.groupBox5.Visible = false;
            // 
            // chk_SwapHandness
            // 
            this.chk_SwapHandness.AutoSize = true;
            this.chk_SwapHandness.Location = new System.Drawing.Point(7, 67);
            this.chk_SwapHandness.Name = "chk_SwapHandness";
            this.chk_SwapHandness.Size = new System.Drawing.Size(150, 25);
            this.chk_SwapHandness.TabIndex = 2;
            this.chk_SwapHandness.Text = "Swap Handness";
            this.chk_SwapHandness.UseVisualStyleBackColor = true;
            // 
            // tb_Rotation
            // 
            this.tb_Rotation.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Rotation.Location = new System.Drawing.Point(98, 27);
            this.tb_Rotation.Name = "tb_Rotation";
            this.tb_Rotation.Size = new System.Drawing.Size(46, 31);
            this.tb_Rotation.TabIndex = 1;
            this.tb_Rotation.Text = "0";
            this.tb_Rotation.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(17, 30);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 21);
            this.label11.TabIndex = 0;
            this.label11.Text = "회전값";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tb_TileSizeY);
            this.groupBox1.Controls.Add(this.tb_TileSizeX);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(328, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(163, 101);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tile Size";
            // 
            // tb_TileSizeY
            // 
            this.tb_TileSizeY.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_TileSizeY.Location = new System.Drawing.Point(68, 61);
            this.tb_TileSizeY.Name = "tb_TileSizeY";
            this.tb_TileSizeY.Size = new System.Drawing.Size(76, 31);
            this.tb_TileSizeY.TabIndex = 1;
            this.tb_TileSizeY.Text = "10";
            this.tb_TileSizeY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tb_TileSizeX
            // 
            this.tb_TileSizeX.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_TileSizeX.Location = new System.Drawing.Point(68, 27);
            this.tb_TileSizeX.Name = "tb_TileSizeX";
            this.tb_TileSizeX.Size = new System.Drawing.Size(76, 31);
            this.tb_TileSizeX.TabIndex = 1;
            this.tb_TileSizeX.Text = "10";
            this.tb_TileSizeX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(23, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 21);
            this.label5.TabIndex = 0;
            this.label5.Text = "Y";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(23, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 21);
            this.label4.TabIndex = 0;
            this.label4.Text = "X";
            // 
            // btn_SaveMaster
            // 
            this.btn_SaveMaster.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_SaveMaster.Location = new System.Drawing.Point(523, 194);
            this.btn_SaveMaster.Name = "btn_SaveMaster";
            this.btn_SaveMaster.Size = new System.Drawing.Size(115, 49);
            this.btn_SaveMaster.TabIndex = 19;
            this.btn_SaveMaster.Text = "Save Master";
            this.btn_SaveMaster.UseVisualStyleBackColor = true;
            this.btn_SaveMaster.Visible = false;
            this.btn_SaveMaster.Click += new System.EventHandler(this.btn_SaveMaster_Click);
            // 
            // chk_FiducialMark
            // 
            this.chk_FiducialMark.AutoSize = true;
            this.chk_FiducialMark.Location = new System.Drawing.Point(15, 499);
            this.chk_FiducialMark.Name = "chk_FiducialMark";
            this.chk_FiducialMark.Size = new System.Drawing.Size(117, 18);
            this.chk_FiducialMark.TabIndex = 5;
            this.chk_FiducialMark.Text = "Fiducial Mark";
            this.chk_FiducialMark.UseVisualStyleBackColor = true;
            // 
            // btn_SaveImage
            // 
            this.btn_SaveImage.Font = new System.Drawing.Font("바탕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_SaveImage.Location = new System.Drawing.Point(760, 721);
            this.btn_SaveImage.Name = "btn_SaveImage";
            this.btn_SaveImage.Size = new System.Drawing.Size(363, 59);
            this.btn_SaveImage.TabIndex = 18;
            this.btn_SaveImage.Text = "이미지 저장";
            this.btn_SaveImage.UseVisualStyleBackColor = true;
            this.btn_SaveImage.Click += new System.EventHandler(this.btn_SaveImage_Click);
            // 
            // btn_OneShot
            // 
            this.btn_OneShot.Font = new System.Drawing.Font("바탕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_OneShot.Location = new System.Drawing.Point(760, 467);
            this.btn_OneShot.Name = "btn_OneShot";
            this.btn_OneShot.Size = new System.Drawing.Size(363, 59);
            this.btn_OneShot.TabIndex = 19;
            this.btn_OneShot.Text = "캡 쳐";
            this.btn_OneShot.UseVisualStyleBackColor = true;
            this.btn_OneShot.Click += new System.EventHandler(this.btn_OneShot_Click);
            // 
            // btn_ContinuousShotStop
            // 
            this.btn_ContinuousShotStop.Font = new System.Drawing.Font("바탕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_ContinuousShotStop.Location = new System.Drawing.Point(760, 597);
            this.btn_ContinuousShotStop.Name = "btn_ContinuousShotStop";
            this.btn_ContinuousShotStop.Size = new System.Drawing.Size(363, 59);
            this.btn_ContinuousShotStop.TabIndex = 16;
            this.btn_ContinuousShotStop.Text = "멈 춤";
            this.btn_ContinuousShotStop.UseVisualStyleBackColor = true;
            this.btn_ContinuousShotStop.Click += new System.EventHandler(this.btn_ContinuousShotStop_Click);
            // 
            // btn_ContinuousShot
            // 
            this.btn_ContinuousShot.Font = new System.Drawing.Font("바탕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_ContinuousShot.Location = new System.Drawing.Point(760, 532);
            this.btn_ContinuousShot.Name = "btn_ContinuousShot";
            this.btn_ContinuousShot.Size = new System.Drawing.Size(363, 59);
            this.btn_ContinuousShot.TabIndex = 17;
            this.btn_ContinuousShot.Text = "라이브";
            this.btn_ContinuousShot.UseVisualStyleBackColor = true;
            this.btn_ContinuousShot.Click += new System.EventHandler(this.btn_ContinuousShot_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabPage4.Controls.Add(this.label8);
            this.tabPage4.Controls.Add(this.label9);
            this.tabPage4.Controls.Add(this.cb_kind);
            this.tabPage4.Controls.Add(this.cb_direction);
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(359, 149);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "유 형";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("돋움", 14.25F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(56, 88);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 19);
            this.label8.TabIndex = 22;
            this.label8.Text = "방 향";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("돋움", 14.25F, System.Drawing.FontStyle.Bold);
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(56, 31);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 19);
            this.label9.TabIndex = 22;
            this.label9.Text = "기 종";
            // 
            // cb_kind
            // 
            this.cb_kind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_kind.Font = new System.Drawing.Font("돋움", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cb_kind.FormattingEnabled = true;
            this.cb_kind.Location = new System.Drawing.Point(171, 28);
            this.cb_kind.Name = "cb_kind";
            this.cb_kind.Size = new System.Drawing.Size(144, 27);
            this.cb_kind.TabIndex = 20;
            this.cb_kind.SelectedIndexChanged += new System.EventHandler(this.cb_kind_SelectedIndexChanged);
            // 
            // cb_direction
            // 
            this.cb_direction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_direction.Font = new System.Drawing.Font("돋움", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cb_direction.FormattingEnabled = true;
            this.cb_direction.Location = new System.Drawing.Point(171, 85);
            this.cb_direction.Name = "cb_direction";
            this.cb_direction.Size = new System.Drawing.Size(144, 27);
            this.cb_direction.TabIndex = 21;
            this.cb_direction.SelectedIndexChanged += new System.EventHandler(this.cb_direction_SelectedIndexChanged);
            // 
            // tabControl4
            // 
            this.tabControl4.Controls.Add(this.tabPage4);
            this.tabControl4.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl4.ItemSize = new System.Drawing.Size(250, 30);
            this.tabControl4.Location = new System.Drawing.Point(756, 21);
            this.tabControl4.Name = "tabControl4";
            this.tabControl4.SelectedIndex = 0;
            this.tabControl4.Size = new System.Drawing.Size(367, 187);
            this.tabControl4.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl4.TabIndex = 23;
            // 
            // btn_CalibImageView
            // 
            this.btn_CalibImageView.Font = new System.Drawing.Font("바탕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_CalibImageView.Location = new System.Drawing.Point(760, 786);
            this.btn_CalibImageView.Name = "btn_CalibImageView";
            this.btn_CalibImageView.Size = new System.Drawing.Size(182, 59);
            this.btn_CalibImageView.TabIndex = 24;
            this.btn_CalibImageView.Text = "켈리브레이션 이미지 보기";
            this.btn_CalibImageView.UseVisualStyleBackColor = true;
            this.btn_CalibImageView.Click += new System.EventHandler(this.btn_CalibImageView_Click);
            // 
            // btn_LightOn
            // 
            this.btn_LightOn.Font = new System.Drawing.Font("바탕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_LightOn.Location = new System.Drawing.Point(760, 659);
            this.btn_LightOn.Name = "btn_LightOn";
            this.btn_LightOn.Size = new System.Drawing.Size(182, 59);
            this.btn_LightOn.TabIndex = 13;
            this.btn_LightOn.Text = "조명 ON";
            this.btn_LightOn.UseVisualStyleBackColor = true;
            this.btn_LightOn.Click += new System.EventHandler(this.btn_LightOn_Click);
            // 
            // btn_LightOff
            // 
            this.btn_LightOff.Font = new System.Drawing.Font("바탕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_LightOff.Location = new System.Drawing.Point(948, 659);
            this.btn_LightOff.Name = "btn_LightOff";
            this.btn_LightOff.Size = new System.Drawing.Size(175, 59);
            this.btn_LightOff.TabIndex = 13;
            this.btn_LightOff.Text = "조명 Off";
            this.btn_LightOff.UseVisualStyleBackColor = true;
            this.btn_LightOff.Click += new System.EventHandler(this.btn_LightOff_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("바탕", 15F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(951, 786);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(172, 59);
            this.button1.TabIndex = 24;
            this.button1.Text = "이미지 불러오기";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CamSetting
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1126, 862);
            this.Controls.Add(this.btn_LightOff);
            this.Controls.Add(this.btn_LightOn);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_CalibImageView);
            this.Controls.Add(this.tabControl4);
            this.Controls.Add(this.chk_FiducialMark);
            this.Controls.Add(this.btn_SaveImage);
            this.Controls.Add(this.tabControl3);
            this.Controls.Add(this.btn_OneShot);
            this.Controls.Add(this.btn_ContinuousShotStop);
            this.Controls.Add(this.btn_ContinuousShot);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "CamSetting";
            this.Text = "카메라 세팅";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CamSetting_FormClosing);
            this.Load += new System.EventHandler(this.CamSetting_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).EndInit();
            this.panel_CamInfo.ResumeLayout(false);
            this.panel_CamInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_Expose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Bright)).EndInit();
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabControl4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Cognex.VisionPro.Display.CogDisplay cogDisplay1;
        private System.Windows.Forms.Button btn_Calibration;
        private System.Windows.Forms.Button btn_Cancle;
        private System.Windows.Forms.Button btn_Modify;
        private System.Windows.Forms.Panel panel_CamInfo;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_SaveImage;
        private System.Windows.Forms.Button btn_OneShot;
        private System.Windows.Forms.Button btn_ContinuousShotStop;
        private System.Windows.Forms.Button btn_ContinuousShot;
        private System.Windows.Forms.NumericUpDown num_Expose;
        private System.Windows.Forms.NumericUpDown num_Bright;
        private System.Windows.Forms.TextBox tb_TileSizeY;
        private System.Windows.Forms.TextBox tb_TileSizeX;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tb_OriginY;
        private System.Windows.Forms.TextBox tb_OriginX;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_SaveMaster;
        private System.Windows.Forms.CheckBox chk_FiducialMark;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton rb_Linear;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rb_UnCalibrated;
        private System.Windows.Forms.RadioButton rb_Calibrated;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cb_kind;
        private System.Windows.Forms.ComboBox cb_direction;
        private System.Windows.Forms.TabControl tabControl4;
        private System.Windows.Forms.Button btn_CalibImageView;
        private System.Windows.Forms.Button btn_LightOn;
        private System.Windows.Forms.Button btn_LightOff;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox tb_Rotation;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chk_SwapHandness;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
    }
}