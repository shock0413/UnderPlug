namespace Setting
{
    partial class Setting_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Setting_Form));
            this.cogDisplay1 = new Cognex.VisionPro.Display.CogDisplay();
            this.btn_DoInspection = new System.Windows.Forms.Button();
            this.btn_LoadImage = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel_Confirm = new System.Windows.Forms.Panel();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_Cancle = new System.Windows.Forms.Button();
            this.label29 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lb_MoveY = new System.Windows.Forms.Label();
            this.lb_FindY = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lb_MoveX = new System.Windows.Forms.Label();
            this.lb_FindX = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tb_MasterX = new System.Windows.Forms.TextBox();
            this.tb_MasterY = new System.Windows.Forms.TextBox();
            this.btn_MaskImage = new System.Windows.Forms.Button();
            this.btn_MasterSave = new System.Windows.Forms.Button();
            this.btn_FindToMaster = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.ai_score_label = new System.Windows.Forms.Label();
            this.ai_score_numeric = new System.Windows.Forms.NumericUpDown();
            this.button2 = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btn_SaveScore = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cb_Model = new System.Windows.Forms.ComboBox();
            this.btn_ModelDel = new System.Windows.Forms.Button();
            this.btn_ModelAdd = new System.Windows.Forms.Button();
            this.btn_InspectionRange = new System.Windows.Forms.Button();
            this.cogDisplay2 = new Cognex.VisionPro.Display.CogDisplay();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btn_Circle_Save = new System.Windows.Forms.Button();
            this.num_Circle_Radius = new System.Windows.Forms.NumericUpDown();
            this.num_Circle_SearchProgrectionLength = new System.Windows.Forms.NumericUpDown();
            this.num_Circle_SearchLength = new System.Windows.Forms.NumericUpDown();
            this.num_Circle_NumTolgnore = new System.Windows.Forms.NumericUpDown();
            this.num_Circle_NumCalipers = new System.Windows.Forms.NumericUpDown();
            this.num_Circle_Threshold = new System.Windows.Forms.NumericUpDown();
            this.num_Circle_FilterHalfSize = new System.Windows.Forms.NumericUpDown();
            this.cb_Circle_Direction = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tb_CorrectionX = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.tb_CorrectionY = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.lb_AIerror = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button4 = new System.Windows.Forms.Button();
            this.label42 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_kind = new System.Windows.Forms.ComboBox();
            this.cb_direction = new System.Windows.Forms.ComboBox();
            this.tabControl4 = new System.Windows.Forms.TabControl();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).BeginInit();
            this.tabControl3.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel_Confirm.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ai_score_numeric)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay2)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_Circle_Radius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Circle_SearchProgrectionLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Circle_SearchLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Circle_NumTolgnore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Circle_NumCalipers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Circle_Threshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Circle_FilterHalfSize)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabControl4.SuspendLayout();
            this.SuspendLayout();
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
            this.cogDisplay1.Location = new System.Drawing.Point(3, 3);
            this.cogDisplay1.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay1.MouseWheelSensitivity = 1D;
            this.cogDisplay1.Name = "cogDisplay1";
            this.cogDisplay1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay1.OcxState")));
            this.cogDisplay1.Size = new System.Drawing.Size(955, 606);
            this.cogDisplay1.TabIndex = 0;
            // 
            // btn_DoInspection
            // 
            this.btn_DoInspection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_DoInspection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.btn_DoInspection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_DoInspection.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_DoInspection.ForeColor = System.Drawing.Color.White;
            this.btn_DoInspection.Location = new System.Drawing.Point(1151, 667);
            this.btn_DoInspection.Name = "btn_DoInspection";
            this.btn_DoInspection.Size = new System.Drawing.Size(111, 61);
            this.btn_DoInspection.TabIndex = 0;
            this.btn_DoInspection.Text = "검 사";
            this.btn_DoInspection.UseVisualStyleBackColor = false;
            this.btn_DoInspection.Click += new System.EventHandler(this.btn_DoInspection_Click);
            // 
            // btn_LoadImage
            // 
            this.btn_LoadImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_LoadImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.btn_LoadImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_LoadImage.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_LoadImage.ForeColor = System.Drawing.Color.White;
            this.btn_LoadImage.Location = new System.Drawing.Point(998, 667);
            this.btn_LoadImage.Name = "btn_LoadImage";
            this.btn_LoadImage.Size = new System.Drawing.Size(145, 61);
            this.btn_LoadImage.TabIndex = 0;
            this.btn_LoadImage.Text = "이미지 불러오기";
            this.btn_LoadImage.UseVisualStyleBackColor = false;
            this.btn_LoadImage.Click += new System.EventHandler(this.btn_LoadImage_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(12, 667);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(162, 61);
            this.button1.TabIndex = 8;
            this.button1.Text = "캘리브레이션";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControl3
            // 
            this.tabControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl3.Controls.Add(this.tabPage3);
            this.tabControl3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabControl3.ItemSize = new System.Drawing.Size(400, 24);
            this.tabControl3.Location = new System.Drawing.Point(12, 16);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(969, 644);
            this.tabControl3.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl3.TabIndex = 2;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.cogDisplay1);
            this.tabPage3.Font = new System.Drawing.Font("Consolas", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabPage3.Location = new System.Drawing.Point(4, 28);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(961, 612);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "이미지";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel_Confirm
            // 
            this.panel_Confirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_Confirm.Controls.Add(this.btn_Save);
            this.panel_Confirm.Controls.Add(this.btn_Cancle);
            this.panel_Confirm.Location = new System.Drawing.Point(751, 668);
            this.panel_Confirm.Name = "panel_Confirm";
            this.panel_Confirm.Size = new System.Drawing.Size(230, 63);
            this.panel_Confirm.TabIndex = 7;
            // 
            // btn_Save
            // 
            this.btn_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.btn_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Save.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Save.ForeColor = System.Drawing.Color.White;
            this.btn_Save.Location = new System.Drawing.Point(3, 0);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(111, 63);
            this.btn_Save.TabIndex = 5;
            this.btn_Save.Text = "저 장";
            this.btn_Save.UseVisualStyleBackColor = false;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Cancle
            // 
            this.btn_Cancle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Cancle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.btn_Cancle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancle.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Cancle.ForeColor = System.Drawing.Color.White;
            this.btn_Cancle.Location = new System.Drawing.Point(117, 0);
            this.btn_Cancle.Name = "btn_Cancle";
            this.btn_Cancle.Size = new System.Drawing.Size(111, 63);
            this.btn_Cancle.TabIndex = 5;
            this.btn_Cancle.Text = "취 소";
            this.btn_Cancle.UseVisualStyleBackColor = false;
            this.btn_Cancle.Click += new System.EventHandler(this.btn_Cancle_Click);
            // 
            // label29
            // 
            this.label29.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label29.ForeColor = System.Drawing.Color.White;
            this.label29.Location = new System.Drawing.Point(234, 111);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(41, 19);
            this.label29.TabIndex = 37;
            this.label29.Text = "mm";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(194, 112);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(41, 19);
            this.label20.TabIndex = 36;
            this.label20.Text = "mm";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(201, 111);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 19);
            this.label8.TabIndex = 35;
            this.label8.Text = "mm";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("돋움", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label19.ForeColor = System.Drawing.Color.White;
            this.label19.Location = new System.Drawing.Point(35, 111);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(23, 20);
            this.label19.TabIndex = 33;
            this.label19.Text = "Y";
            // 
            // lb_MoveY
            // 
            this.lb_MoveY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lb_MoveY.BackColor = System.Drawing.Color.White;
            this.lb_MoveY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_MoveY.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lb_MoveY.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lb_MoveY.Location = new System.Drawing.Point(111, 104);
            this.lb_MoveY.Name = "lb_MoveY";
            this.lb_MoveY.Size = new System.Drawing.Size(121, 34);
            this.lb_MoveY.TabIndex = 13;
            this.lb_MoveY.Text = "0";
            this.lb_MoveY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lb_FindY
            // 
            this.lb_FindY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lb_FindY.BackColor = System.Drawing.Color.White;
            this.lb_FindY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_FindY.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lb_FindY.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lb_FindY.Location = new System.Drawing.Point(79, 104);
            this.lb_FindY.Name = "lb_FindY";
            this.lb_FindY.Size = new System.Drawing.Size(121, 34);
            this.lb_FindY.TabIndex = 11;
            this.lb_FindY.Text = "0";
            this.lb_FindY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label25.ForeColor = System.Drawing.Color.White;
            this.label25.Location = new System.Drawing.Point(234, 66);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(41, 19);
            this.label25.TabIndex = 22;
            this.label25.Text = "mm";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label16.ForeColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(194, 67);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(41, 19);
            this.label16.TabIndex = 21;
            this.label16.Text = "mm";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(201, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 19);
            this.label7.TabIndex = 20;
            this.label7.Text = "mm";
            // 
            // lb_MoveX
            // 
            this.lb_MoveX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lb_MoveX.BackColor = System.Drawing.Color.White;
            this.lb_MoveX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_MoveX.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lb_MoveX.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lb_MoveX.Location = new System.Drawing.Point(111, 58);
            this.lb_MoveX.Name = "lb_MoveX";
            this.lb_MoveX.Size = new System.Drawing.Size(121, 34);
            this.lb_MoveX.TabIndex = 19;
            this.lb_MoveX.Text = "0";
            this.lb_MoveX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lb_MoveX.Click += new System.EventHandler(this.lb_MoveX_Click);
            // 
            // lb_FindX
            // 
            this.lb_FindX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lb_FindX.BackColor = System.Drawing.Color.White;
            this.lb_FindX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_FindX.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lb_FindX.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lb_FindX.Location = new System.Drawing.Point(79, 58);
            this.lb_FindX.Name = "lb_FindX";
            this.lb_FindX.Size = new System.Drawing.Size(121, 34);
            this.lb_FindX.TabIndex = 17;
            this.lb_FindX.Text = "0";
            this.lb_FindX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("돋움", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(35, 67);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(23, 20);
            this.label14.TabIndex = 16;
            this.label14.Text = "X";
            // 
            // tb_MasterX
            // 
            this.tb_MasterX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_MasterX.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this.tb_MasterX.Location = new System.Drawing.Point(67, 60);
            this.tb_MasterX.Name = "tb_MasterX";
            this.tb_MasterX.ReadOnly = true;
            this.tb_MasterX.Size = new System.Drawing.Size(121, 31);
            this.tb_MasterX.TabIndex = 38;
            // 
            // tb_MasterY
            // 
            this.tb_MasterY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_MasterY.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this.tb_MasterY.Location = new System.Drawing.Point(67, 106);
            this.tb_MasterY.Name = "tb_MasterY";
            this.tb_MasterY.ReadOnly = true;
            this.tb_MasterY.Size = new System.Drawing.Size(121, 31);
            this.tb_MasterY.TabIndex = 38;
            // 
            // btn_MaskImage
            // 
            this.btn_MaskImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_MaskImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.btn_MaskImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_MaskImage.Font = new System.Drawing.Font("돋움", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_MaskImage.ForeColor = System.Drawing.Color.White;
            this.btn_MaskImage.Location = new System.Drawing.Point(567, 668);
            this.btn_MaskImage.Name = "btn_MaskImage";
            this.btn_MaskImage.Size = new System.Drawing.Size(178, 64);
            this.btn_MaskImage.TabIndex = 39;
            this.btn_MaskImage.Text = "이미지 마스킹";
            this.btn_MaskImage.UseVisualStyleBackColor = false;
            this.btn_MaskImage.Click += new System.EventHandler(this.btn_MaskImage_Click);
            // 
            // btn_MasterSave
            // 
            this.btn_MasterSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_MasterSave.Font = new System.Drawing.Font("돋움", 8.25F);
            this.btn_MasterSave.Location = new System.Drawing.Point(39, 170);
            this.btn_MasterSave.Name = "btn_MasterSave";
            this.btn_MasterSave.Size = new System.Drawing.Size(180, 53);
            this.btn_MasterSave.TabIndex = 8;
            this.btn_MasterSave.Text = "마스터 위치 저장";
            this.btn_MasterSave.UseVisualStyleBackColor = true;
            this.btn_MasterSave.Click += new System.EventHandler(this.btn_MasterSave_Click);
            // 
            // btn_FindToMaster
            // 
            this.btn_FindToMaster.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_FindToMaster.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_FindToMaster.Font = new System.Drawing.Font("돋움", 8.25F);
            this.btn_FindToMaster.Location = new System.Drawing.Point(33, 169);
            this.btn_FindToMaster.Name = "btn_FindToMaster";
            this.btn_FindToMaster.Size = new System.Drawing.Size(213, 53);
            this.btn_FindToMaster.TabIndex = 8;
            this.btn_FindToMaster.Text = "찾은 위치 마스터 값으로 넣기";
            this.btn_FindToMaster.UseVisualStyleBackColor = true;
            this.btn_FindToMaster.Click += new System.EventHandler(this.btn_FindToMaster_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(988, 165);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(285, 495);
            this.tabControl1.TabIndex = 43;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.ai_score_label);
            this.tabPage5.Controls.Add(this.ai_score_numeric);
            this.tabPage5.Controls.Add(this.button2);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(277, 469);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "AI";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // ai_score_label
            // 
            this.ai_score_label.AutoSize = true;
            this.ai_score_label.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ai_score_label.Location = new System.Drawing.Point(14, 23);
            this.ai_score_label.Name = "ai_score_label";
            this.ai_score_label.Size = new System.Drawing.Size(33, 13);
            this.ai_score_label.TabIndex = 19;
            this.ai_score_label.Text = "점수";
            // 
            // ai_score_numeric
            // 
            this.ai_score_numeric.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ai_score_numeric.Location = new System.Drawing.Point(84, 19);
            this.ai_score_numeric.Name = "ai_score_numeric";
            this.ai_score_numeric.Size = new System.Drawing.Size(182, 22);
            this.ai_score_numeric.TabIndex = 18;
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(17, 413);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(249, 38);
            this.button2.TabIndex = 17;
            this.button2.Text = "저장";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btn_AI_Save_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btn_SaveScore);
            this.tabPage1.Controls.Add(this.numericUpDown1);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.cb_Model);
            this.tabPage1.Controls.Add(this.btn_ModelDel);
            this.tabPage1.Controls.Add(this.btn_ModelAdd);
            this.tabPage1.Controls.Add(this.btn_InspectionRange);
            this.tabPage1.Controls.Add(this.cogDisplay2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(277, 469);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "패턴";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btn_SaveScore
            // 
            this.btn_SaveScore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_SaveScore.Font = new System.Drawing.Font("바탕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SaveScore.Location = new System.Drawing.Point(168, 26);
            this.btn_SaveScore.Name = "btn_SaveScore";
            this.btn_SaveScore.Size = new System.Drawing.Size(103, 34);
            this.btn_SaveScore.TabIndex = 14;
            this.btn_SaveScore.Text = "저 장";
            this.btn_SaveScore.UseVisualStyleBackColor = true;
            this.btn_SaveScore.Click += new System.EventHandler(this.btn_SaveScore_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown1.Location = new System.Drawing.Point(108, 29);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(57, 26);
            this.numericUpDown1.TabIndex = 13;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("돋움", 8.25F);
            this.label6.Location = new System.Drawing.Point(36, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 11);
            this.label6.TabIndex = 12;
            this.label6.Text = "점 수";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("돋움", 8.25F);
            this.label10.Location = new System.Drawing.Point(36, 137);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 11);
            this.label10.TabIndex = 11;
            this.label10.Text = "패 턴";
            // 
            // cb_Model
            // 
            this.cb_Model.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_Model.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Model.FormattingEnabled = true;
            this.cb_Model.Location = new System.Drawing.Point(130, 132);
            this.cb_Model.Name = "cb_Model";
            this.cb_Model.Size = new System.Drawing.Size(142, 27);
            this.cb_Model.TabIndex = 10;
            this.cb_Model.SelectedIndexChanged += new System.EventHandler(this.cb_Model_SelectedIndexChanged);
            // 
            // btn_ModelDel
            // 
            this.btn_ModelDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_ModelDel.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.btn_ModelDel.Location = new System.Drawing.Point(144, 169);
            this.btn_ModelDel.Name = "btn_ModelDel";
            this.btn_ModelDel.Size = new System.Drawing.Size(127, 50);
            this.btn_ModelDel.TabIndex = 6;
            this.btn_ModelDel.Text = "삭 제";
            this.btn_ModelDel.UseVisualStyleBackColor = true;
            this.btn_ModelDel.Click += new System.EventHandler(this.btn_ModelDel_Click);
            // 
            // btn_ModelAdd
            // 
            this.btn_ModelAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_ModelAdd.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.btn_ModelAdd.Location = new System.Drawing.Point(17, 169);
            this.btn_ModelAdd.Name = "btn_ModelAdd";
            this.btn_ModelAdd.Size = new System.Drawing.Size(127, 50);
            this.btn_ModelAdd.TabIndex = 7;
            this.btn_ModelAdd.Text = "패턴 추가";
            this.btn_ModelAdd.UseVisualStyleBackColor = true;
            this.btn_ModelAdd.Click += new System.EventHandler(this.btn_ModelAdd_Click);
            // 
            // btn_InspectionRange
            // 
            this.btn_InspectionRange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_InspectionRange.Font = new System.Drawing.Font("바탕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_InspectionRange.Location = new System.Drawing.Point(17, 78);
            this.btn_InspectionRange.Name = "btn_InspectionRange";
            this.btn_InspectionRange.Size = new System.Drawing.Size(255, 40);
            this.btn_InspectionRange.TabIndex = 8;
            this.btn_InspectionRange.Text = "검사 영역";
            this.btn_InspectionRange.UseVisualStyleBackColor = true;
            this.btn_InspectionRange.Click += new System.EventHandler(this.btn_InspectionRange_Click_1);
            // 
            // cogDisplay2
            // 
            this.cogDisplay2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cogDisplay2.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay2.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay2.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay2.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay2.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay2.DoubleTapZoomCycleLength = 2;
            this.cogDisplay2.DoubleTapZoomSensitivity = 2.5D;
            this.cogDisplay2.Location = new System.Drawing.Point(17, 254);
            this.cogDisplay2.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay2.MouseWheelSensitivity = 1D;
            this.cogDisplay2.Name = "cogDisplay2";
            this.cogDisplay2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay2.OcxState")));
            this.cogDisplay2.Size = new System.Drawing.Size(251, 208);
            this.cogDisplay2.TabIndex = 9;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btn_Circle_Save);
            this.tabPage2.Controls.Add(this.num_Circle_Radius);
            this.tabPage2.Controls.Add(this.num_Circle_SearchProgrectionLength);
            this.tabPage2.Controls.Add(this.num_Circle_SearchLength);
            this.tabPage2.Controls.Add(this.num_Circle_NumTolgnore);
            this.tabPage2.Controls.Add(this.num_Circle_NumCalipers);
            this.tabPage2.Controls.Add(this.num_Circle_Threshold);
            this.tabPage2.Controls.Add(this.num_Circle_FilterHalfSize);
            this.tabPage2.Controls.Add(this.cb_Circle_Direction);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.label24);
            this.tabPage2.Controls.Add(this.label18);
            this.tabPage2.Controls.Add(this.label17);
            this.tabPage2.Controls.Add(this.label15);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.label26);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(277, 469);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "원 찾기";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btn_Circle_Save
            // 
            this.btn_Circle_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Circle_Save.Location = new System.Drawing.Point(12, 444);
            this.btn_Circle_Save.Name = "btn_Circle_Save";
            this.btn_Circle_Save.Size = new System.Drawing.Size(249, 38);
            this.btn_Circle_Save.TabIndex = 16;
            this.btn_Circle_Save.Text = "저장";
            this.btn_Circle_Save.UseVisualStyleBackColor = true;
            this.btn_Circle_Save.Click += new System.EventHandler(this.btn_Circle_Save_Click);
            // 
            // num_Circle_Radius
            // 
            this.num_Circle_Radius.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_Circle_Radius.Location = new System.Drawing.Point(158, 382);
            this.num_Circle_Radius.Name = "num_Circle_Radius";
            this.num_Circle_Radius.Size = new System.Drawing.Size(103, 20);
            this.num_Circle_Radius.TabIndex = 15;
            this.num_Circle_Radius.ValueChanged += new System.EventHandler(this.numericUpDown5_ValueChanged);
            // 
            // num_Circle_SearchProgrectionLength
            // 
            this.num_Circle_SearchProgrectionLength.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_Circle_SearchProgrectionLength.Location = new System.Drawing.Point(158, 335);
            this.num_Circle_SearchProgrectionLength.Name = "num_Circle_SearchProgrectionLength";
            this.num_Circle_SearchProgrectionLength.Size = new System.Drawing.Size(103, 20);
            this.num_Circle_SearchProgrectionLength.TabIndex = 15;
            this.num_Circle_SearchProgrectionLength.ValueChanged += new System.EventHandler(this.numericUpDown5_ValueChanged);
            // 
            // num_Circle_SearchLength
            // 
            this.num_Circle_SearchLength.Location = new System.Drawing.Point(158, 287);
            this.num_Circle_SearchLength.Name = "num_Circle_SearchLength";
            this.num_Circle_SearchLength.Size = new System.Drawing.Size(103, 20);
            this.num_Circle_SearchLength.TabIndex = 15;
            this.num_Circle_SearchLength.ValueChanged += new System.EventHandler(this.numericUpDown5_ValueChanged);
            // 
            // num_Circle_NumTolgnore
            // 
            this.num_Circle_NumTolgnore.Location = new System.Drawing.Point(158, 236);
            this.num_Circle_NumTolgnore.Name = "num_Circle_NumTolgnore";
            this.num_Circle_NumTolgnore.Size = new System.Drawing.Size(103, 20);
            this.num_Circle_NumTolgnore.TabIndex = 15;
            this.num_Circle_NumTolgnore.ValueChanged += new System.EventHandler(this.numericUpDown5_ValueChanged);
            // 
            // num_Circle_NumCalipers
            // 
            this.num_Circle_NumCalipers.Location = new System.Drawing.Point(158, 189);
            this.num_Circle_NumCalipers.Name = "num_Circle_NumCalipers";
            this.num_Circle_NumCalipers.Size = new System.Drawing.Size(103, 20);
            this.num_Circle_NumCalipers.TabIndex = 15;
            // 
            // num_Circle_Threshold
            // 
            this.num_Circle_Threshold.Location = new System.Drawing.Point(158, 135);
            this.num_Circle_Threshold.Name = "num_Circle_Threshold";
            this.num_Circle_Threshold.Size = new System.Drawing.Size(103, 20);
            this.num_Circle_Threshold.TabIndex = 15;
            // 
            // num_Circle_FilterHalfSize
            // 
            this.num_Circle_FilterHalfSize.Location = new System.Drawing.Point(158, 87);
            this.num_Circle_FilterHalfSize.Name = "num_Circle_FilterHalfSize";
            this.num_Circle_FilterHalfSize.Size = new System.Drawing.Size(103, 20);
            this.num_Circle_FilterHalfSize.TabIndex = 15;
            // 
            // cb_Circle_Direction
            // 
            this.cb_Circle_Direction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Circle_Direction.FormattingEnabled = true;
            this.cb_Circle_Direction.Items.AddRange(new object[] {
            "Inward",
            "Outward"});
            this.cb_Circle_Direction.Location = new System.Drawing.Point(158, 40);
            this.cb_Circle_Direction.Name = "cb_Circle_Direction";
            this.cb_Circle_Direction.Size = new System.Drawing.Size(103, 21);
            this.cb_Circle_Direction.TabIndex = 14;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.label12.Location = new System.Drawing.Point(24, 67);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(126, 16);
            this.label12.TabIndex = 13;
            this.label12.Text = "Filter Half Size";
            // 
            // label24
            // 
            this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.label24.Location = new System.Drawing.Point(23, 363);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(63, 16);
            this.label24.TabIndex = 13;
            this.label24.Text = "Radius";
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.label18.Location = new System.Drawing.Point(23, 314);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(217, 16);
            this.label18.TabIndex = 13;
            this.label18.Text = "Search Projection Length";
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.label17.Location = new System.Drawing.Point(23, 265);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(127, 16);
            this.label17.TabIndex = 13;
            this.label17.Text = "Search Length";
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.label15.Location = new System.Drawing.Point(23, 215);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(122, 16);
            this.label15.TabIndex = 13;
            this.label15.Text = "Num Tolgnore";
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.label13.Location = new System.Drawing.Point(23, 168);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(116, 16);
            this.label13.TabIndex = 13;
            this.label13.Text = "Num Calipers";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(24, 115);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 16);
            this.label11.TabIndex = 13;
            this.label11.Text = "Threshold";
            // 
            // label26
            // 
            this.label26.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("돋움", 10F, System.Drawing.FontStyle.Bold);
            this.label26.Location = new System.Drawing.Point(13, 427);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(253, 14);
            this.label26.TabIndex = 13;
            this.label26.Text = "※ 전체 검사항목 설정 값 공통 적용";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(24, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 16);
            this.label5.TabIndex = 13;
            this.label5.Text = "Direction";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.tb_CorrectionX);
            this.groupBox1.Controls.Add(this.tb_MasterX);
            this.groupBox1.Controls.Add(this.btn_MasterSave);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label37);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label38);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.tb_CorrectionY);
            this.groupBox1.Controls.Add(this.tb_MasterY);
            this.groupBox1.Font = new System.Drawing.Font("돋움", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 740);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(473, 239);
            this.groupBox1.TabIndex = 44;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "마스터 위치";
            // 
            // tb_CorrectionX
            // 
            this.tb_CorrectionX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_CorrectionX.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this.tb_CorrectionX.Location = new System.Drawing.Point(282, 60);
            this.tb_CorrectionX.Name = "tb_CorrectionX";
            this.tb_CorrectionX.ReadOnly = true;
            this.tb_CorrectionX.Size = new System.Drawing.Size(121, 31);
            this.tb_CorrectionX.TabIndex = 38;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("돋움", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(247, 111);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 20);
            this.label9.TabIndex = 16;
            this.label9.Text = "+";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("돋움", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(247, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 20);
            this.label3.TabIndex = 16;
            this.label3.Text = "+";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("돋움", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label22.ForeColor = System.Drawing.Color.White;
            this.label22.Location = new System.Drawing.Point(266, 31);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(80, 20);
            this.label22.TabIndex = 16;
            this.label22.Text = "보정 값";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("돋움", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(35, 31);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(101, 20);
            this.label21.TabIndex = 16;
            this.label21.Text = "마스터 값";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label37.ForeColor = System.Drawing.Color.White;
            this.label37.Location = new System.Drawing.Point(409, 68);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(41, 19);
            this.label37.TabIndex = 21;
            this.label37.Text = "mm";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label38.ForeColor = System.Drawing.Color.White;
            this.label38.Location = new System.Drawing.Point(409, 112);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(41, 19);
            this.label38.TabIndex = 36;
            this.label38.Text = "mm";
            // 
            // tb_CorrectionY
            // 
            this.tb_CorrectionY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_CorrectionY.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this.tb_CorrectionY.Location = new System.Drawing.Point(282, 106);
            this.tb_CorrectionY.Name = "tb_CorrectionY";
            this.tb_CorrectionY.ReadOnly = true;
            this.tb_CorrectionY.Size = new System.Drawing.Size(121, 31);
            this.tb_CorrectionY.TabIndex = 38;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.lb_FindX);
            this.groupBox2.Controls.Add(this.btn_FindToMaster);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.lb_FindY);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Font = new System.Drawing.Font("돋움", 15F, System.Drawing.FontStyle.Bold);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(567, 741);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(279, 238);
            this.groupBox2.TabIndex = 45;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "찾은 위치";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("돋움", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(33, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 20);
            this.label1.TabIndex = 36;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("돋움", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(33, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 20);
            this.label2.TabIndex = 37;
            this.label2.Text = "Y";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.label23);
            this.groupBox3.Controls.Add(this.label28);
            this.groupBox3.Controls.Add(this.lb_MoveX);
            this.groupBox3.Controls.Add(this.label25);
            this.groupBox3.Controls.Add(this.lb_MoveY);
            this.groupBox3.Controls.Add(this.label29);
            this.groupBox3.Font = new System.Drawing.Font("돋움", 15F, System.Drawing.FontStyle.Bold);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(926, 741);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(336, 238);
            this.groupBox3.TabIndex = 46;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "이동 값";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("돋움", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label23.ForeColor = System.Drawing.Color.White;
            this.label23.Location = new System.Drawing.Point(65, 65);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(23, 20);
            this.label23.TabIndex = 38;
            this.label23.Text = "X";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("돋움", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label28.ForeColor = System.Drawing.Color.White;
            this.label28.Location = new System.Drawing.Point(65, 109);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(23, 20);
            this.label28.TabIndex = 39;
            this.label28.Text = "Y";
            // 
            // label35
            // 
            this.label35.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("돋움", 40.25F, System.Drawing.FontStyle.Bold);
            this.label35.ForeColor = System.Drawing.Color.White;
            this.label35.Location = new System.Drawing.Point(499, 828);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(55, 54);
            this.label35.TabIndex = 47;
            this.label35.Text = "-";
            // 
            // label36
            // 
            this.label36.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label36.AutoSize = true;
            this.label36.Font = new System.Drawing.Font("돋움", 40.25F, System.Drawing.FontStyle.Bold);
            this.label36.ForeColor = System.Drawing.Color.White;
            this.label36.Location = new System.Drawing.Point(860, 833);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(55, 54);
            this.label36.TabIndex = 47;
            this.label36.Text = "=";
            // 
            // lb_AIerror
            // 
            this.lb_AIerror.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lb_AIerror.AutoSize = true;
            this.lb_AIerror.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.lb_AIerror.ForeColor = System.Drawing.Color.Red;
            this.lb_AIerror.Location = new System.Drawing.Point(186, 685);
            this.lb_AIerror.Name = "lb_AIerror";
            this.lb_AIerror.Size = new System.Drawing.Size(368, 25);
            this.lb_AIerror.TabIndex = 48;
            this.lb_AIerror.Text = "AI 작동 불가 - 설정 후 프로그램을 재시작 해주세요.";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.button4);
            this.tabPage4.Controls.Add(this.label42);
            this.tabPage4.Controls.Add(this.label4);
            this.tabPage4.Controls.Add(this.cb_kind);
            this.tabPage4.Controls.Add(this.cb_direction);
            this.tabPage4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage4.Location = new System.Drawing.Point(4, 28);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(282, 111);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "유 형";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("돋움", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button4.Location = new System.Drawing.Point(18, 74);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(249, 34);
            this.button4.TabIndex = 29;
            this.button4.Text = "차종 및 홀 설정";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.CarkindAndHoleSettingButton_Click);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Font = new System.Drawing.Font("돋움", 10F);
            this.label42.Location = new System.Drawing.Point(15, 51);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(40, 14);
            this.label42.TabIndex = 28;
            this.label42.Text = "방 향";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("돋움", 10F);
            this.label4.Location = new System.Drawing.Point(15, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 14);
            this.label4.TabIndex = 27;
            this.label4.Text = "기 종";
            // 
            // cb_kind
            // 
            this.cb_kind.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cb_kind.FormattingEnabled = true;
            this.cb_kind.Location = new System.Drawing.Point(124, 14);
            this.cb_kind.Name = "cb_kind";
            this.cb_kind.Size = new System.Drawing.Size(143, 24);
            this.cb_kind.TabIndex = 23;
            this.cb_kind.SelectedIndexChanged += new System.EventHandler(this.cb_kind_SelectedIndexChanged);
            // 
            // cb_direction
            // 
            this.cb_direction.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.cb_direction.FormattingEnabled = true;
            this.cb_direction.Location = new System.Drawing.Point(124, 44);
            this.cb_direction.Name = "cb_direction";
            this.cb_direction.Size = new System.Drawing.Size(143, 24);
            this.cb_direction.TabIndex = 24;
            this.cb_direction.SelectedIndexChanged += new System.EventHandler(this.cb_kind_SelectedIndexChanged);
            // 
            // tabControl4
            // 
            this.tabControl4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl4.Controls.Add(this.tabPage4);
            this.tabControl4.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabControl4.ItemSize = new System.Drawing.Size(140, 24);
            this.tabControl4.Location = new System.Drawing.Point(987, 16);
            this.tabControl4.Name = "tabControl4";
            this.tabControl4.SelectedIndex = 0;
            this.tabControl4.Size = new System.Drawing.Size(290, 143);
            this.tabControl4.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl4.TabIndex = 9;
            // 
            // Setting_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.ClientSize = new System.Drawing.Size(1278, 986);
            this.Controls.Add(this.lb_AIerror);
            this.Controls.Add(this.label36);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel_Confirm);
            this.Controls.Add(this.btn_MaskImage);
            this.Controls.Add(this.tabControl4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl3);
            this.Controls.Add(this.btn_LoadImage);
            this.Controls.Add(this.btn_DoInspection);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Setting_Form";
            this.Text = "설정";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Setting_Form_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay1)).EndInit();
            this.tabControl3.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panel_Confirm.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ai_score_numeric)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay2)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_Circle_Radius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Circle_SearchProgrectionLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Circle_SearchLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Circle_NumTolgnore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Circle_NumCalipers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Circle_Threshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Circle_FilterHalfSize)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabControl4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Cognex.VisionPro.Display.CogDisplay cogDisplay1;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel_Confirm;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_Cancle;
        private System.Windows.Forms.Button btn_LoadImage;
        private System.Windows.Forms.Button btn_DoInspection;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lb_MoveY;
        private System.Windows.Forms.Label lb_FindY;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lb_MoveX;
        private System.Windows.Forms.Label lb_FindX;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tb_MasterX;
        private System.Windows.Forms.TextBox tb_MasterY;
        private System.Windows.Forms.Button btn_MaskImage;
        private System.Windows.Forms.Button btn_MasterSave;
        private System.Windows.Forms.Button btn_FindToMaster;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btn_SaveScore;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cb_Model;
        private System.Windows.Forms.Button btn_ModelDel;
        private System.Windows.Forms.Button btn_ModelAdd;
        private System.Windows.Forms.Button btn_InspectionRange;
        private Cognex.VisionPro.Display.CogDisplay cogDisplay2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cb_Circle_Direction;
        private System.Windows.Forms.NumericUpDown num_Circle_NumTolgnore;
        private System.Windows.Forms.NumericUpDown num_Circle_NumCalipers;
        private System.Windows.Forms.NumericUpDown num_Circle_Threshold;
        private System.Windows.Forms.NumericUpDown num_Circle_FilterHalfSize;
        private System.Windows.Forms.NumericUpDown num_Circle_Radius;
        private System.Windows.Forms.NumericUpDown num_Circle_SearchProgrectionLength;
        private System.Windows.Forms.NumericUpDown num_Circle_SearchLength;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Button btn_Circle_Save;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.TextBox tb_CorrectionX;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.TextBox tb_CorrectionY;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label lb_AIerror;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_kind;
        private System.Windows.Forms.ComboBox cb_direction;
        private System.Windows.Forms.TabControl tabControl4;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label ai_score_label;
        private System.Windows.Forms.NumericUpDown ai_score_numeric;
    }
}

