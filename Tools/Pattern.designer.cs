namespace Tools
{
    partial class Pattern
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Pattern));
            this.cogDisplay_Model = new Cognex.VisionPro.Display.CogDisplay();
            this.cb_Model = new System.Windows.Forms.ComboBox();
            this.num_Elasticity = new System.Windows.Forms.NumericUpDown();
            this.label86 = new System.Windows.Forms.Label();
            this.num_ScaleY = new System.Windows.Forms.NumericUpDown();
            this.label87 = new System.Windows.Forms.Label();
            this.num_ScaleX = new System.Windows.Forms.NumericUpDown();
            this.label88 = new System.Windows.Forms.Label();
            this.num_Angle = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.num_Score = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_SetRange = new Utilities.VistaButton();
            this.rb_Ring = new System.Windows.Forms.RadioButton();
            this.rb_Circle = new System.Windows.Forms.RadioButton();
            this.rb_RectAngle = new System.Windows.Forms.RadioButton();
            this.chk_UseMaster = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn_Masking = new System.Windows.Forms.Button();
            this.ckd_PointCheck = new System.Windows.Forms.CheckBox();
            this.ckd_Find = new System.Windows.Forms.CheckBox();
            this.chk_useTrainPoint = new System.Windows.Forms.CheckBox();
            this.btn_DelModel = new Utilities.VistaButton();
            this.btn_AddModel = new Utilities.VistaButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_ModifyConvertMethod = new System.Windows.Forms.Button();
            this.btn_SaveConvertMethod = new System.Windows.Forms.Button();
            this.btn_ConvertImageSet = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_ConvertMethod = new System.Windows.Forms.ComboBox();
            this.panel_WRGB = new System.Windows.Forms.Panel();
            this.btn_CancleRGB = new System.Windows.Forms.Button();
            this.btn_SaveRGB = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.num_Weight_B = new System.Windows.Forms.NumericUpDown();
            this.num_Weight_G = new System.Windows.Forms.NumericUpDown();
            this.num_Weight_R = new System.Windows.Forms.NumericUpDown();
            this.panel_Color = new System.Windows.Forms.Panel();
            this.cogDisplay_ColorModel = new Cognex.VisionPro.Display.CogDisplay();
            this.btn_Cancle = new System.Windows.Forms.Button();
            this.btn_ColorPatternDel = new System.Windows.Forms.Button();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_Result = new System.Windows.Forms.Button();
            this.btn_ColorPatternAdd = new System.Windows.Forms.Button();
            this.btn_Train = new System.Windows.Forms.Button();
            this.cb_ColorModel = new System.Windows.Forms.ComboBox();
            this.panel_trainParam = new System.Windows.Forms.Panel();
            this.label109 = new System.Windows.Forms.Label();
            this.label110 = new System.Windows.Forms.Label();
            this.label106 = new System.Windows.Forms.Label();
            this.label105 = new System.Windows.Forms.Label();
            this.label103 = new System.Windows.Forms.Label();
            this.num_Dilation = new System.Windows.Forms.NumericUpDown();
            this.num_MinPixelCnt = new System.Windows.Forms.NumericUpDown();
            this.num_Softness = new System.Windows.Forms.NumericUpDown();
            this.num_MatteLineHigh = new System.Windows.Forms.NumericUpDown();
            this.num_MatteLineLow = new System.Windows.Forms.NumericUpDown();
            this.num_HighLightLimit = new System.Windows.Forms.NumericUpDown();
            this.chk_MatteLineLimit = new System.Windows.Forms.CheckBox();
            this.chk_HighLightLimit = new System.Windows.Forms.CheckBox();
            this.btn_Run = new Utilities.VistaButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lb_CenterY = new System.Windows.Forms.Label();
            this.lb_CenterX = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_MasterSave = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lb_FindY = new System.Windows.Forms.Label();
            this.lb_FindX = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cb_reverseHorizental = new System.Windows.Forms.CheckBox();
            this.btn_SaveCalib = new System.Windows.Forms.Button();
            this.cb_reverseXY = new System.Windows.Forms.CheckBox();
            this.cb_reverseVertical = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.num_HorizentalPixelPerMM = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.num_VerticalPixelperMM = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_Model)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Elasticity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_ScaleY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_ScaleX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Angle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Score)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel_WRGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_Weight_B)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Weight_G)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Weight_R)).BeginInit();
            this.panel_Color.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_ColorModel)).BeginInit();
            this.panel_trainParam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_Dilation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_MinPixelCnt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Softness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_MatteLineHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_MatteLineLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_HighLightLimit)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_HorizentalPixelPerMM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_VerticalPixelperMM)).BeginInit();
            this.SuspendLayout();
            // 
            // cogDisplay_Model
            // 
            this.cogDisplay_Model.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay_Model.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay_Model.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay_Model.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay_Model.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay_Model.Location = new System.Drawing.Point(218, 26);
            this.cogDisplay_Model.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay_Model.MouseWheelSensitivity = 1D;
            this.cogDisplay_Model.Name = "cogDisplay_Model";
            this.cogDisplay_Model.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay_Model.OcxState")));
            this.cogDisplay_Model.Size = new System.Drawing.Size(149, 131);
            this.cogDisplay_Model.TabIndex = 0;
            // 
            // cb_Model
            // 
            this.cb_Model.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.cb_Model.FormattingEnabled = true;
            this.cb_Model.Location = new System.Drawing.Point(37, 31);
            this.cb_Model.Name = "cb_Model";
            this.cb_Model.Size = new System.Drawing.Size(120, 32);
            this.cb_Model.TabIndex = 1;
            this.cb_Model.SelectedIndexChanged += new System.EventHandler(this.cb_Model_SelectedIndexChanged);
            // 
            // num_Elasticity
            // 
            this.num_Elasticity.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num_Elasticity.Location = new System.Drawing.Point(567, 384);
            this.num_Elasticity.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.num_Elasticity.Name = "num_Elasticity";
            this.num_Elasticity.Size = new System.Drawing.Size(82, 26);
            this.num_Elasticity.TabIndex = 440;
            this.num_Elasticity.Visible = false;
            // 
            // label86
            // 
            this.label86.AutoSize = true;
            this.label86.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label86.Location = new System.Drawing.Point(462, 388);
            this.label86.Name = "label86";
            this.label86.Size = new System.Drawing.Size(86, 20);
            this.label86.TabIndex = 439;
            this.label86.Text = "4.탄력주기";
            this.label86.Visible = false;
            // 
            // num_ScaleY
            // 
            this.num_ScaleY.DecimalPlaces = 2;
            this.num_ScaleY.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num_ScaleY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_ScaleY.Location = new System.Drawing.Point(311, 69);
            this.num_ScaleY.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_ScaleY.Name = "num_ScaleY";
            this.num_ScaleY.Size = new System.Drawing.Size(82, 29);
            this.num_ScaleY.TabIndex = 438;
            // 
            // label87
            // 
            this.label87.AutoSize = true;
            this.label87.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label87.Location = new System.Drawing.Point(225, 74);
            this.label87.Name = "label87";
            this.label87.Size = new System.Drawing.Size(79, 24);
            this.label87.TabIndex = 437;
            this.label87.Text = "스케일Y";
            // 
            // num_ScaleX
            // 
            this.num_ScaleX.DecimalPlaces = 2;
            this.num_ScaleX.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num_ScaleX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_ScaleX.Location = new System.Drawing.Point(311, 24);
            this.num_ScaleX.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_ScaleX.Name = "num_ScaleX";
            this.num_ScaleX.Size = new System.Drawing.Size(82, 29);
            this.num_ScaleX.TabIndex = 436;
            // 
            // label88
            // 
            this.label88.AutoSize = true;
            this.label88.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label88.Location = new System.Drawing.Point(199, 28);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(101, 24);
            this.label88.TabIndex = 435;
            this.label88.Text = "3. 스케일X";
            // 
            // num_Angle
            // 
            this.num_Angle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num_Angle.Location = new System.Drawing.Point(92, 69);
            this.num_Angle.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.num_Angle.Name = "num_Angle";
            this.num_Angle.Size = new System.Drawing.Size(82, 29);
            this.num_Angle.TabIndex = 434;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(6, 74);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(68, 24);
            this.label12.TabIndex = 433;
            this.label12.Text = "2. 각도";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(6, 28);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(68, 24);
            this.label10.TabIndex = 432;
            this.label10.Text = "1. 점수";
            // 
            // num_Score
            // 
            this.num_Score.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num_Score.Location = new System.Drawing.Point(92, 24);
            this.num_Score.Name = "num_Score";
            this.num_Score.Size = new System.Drawing.Size(82, 29);
            this.num_Score.TabIndex = 431;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Controls.Add(this.btn_SetRange);
            this.groupBox1.Controls.Add(this.rb_Ring);
            this.groupBox1.Controls.Add(this.rb_Circle);
            this.groupBox1.Controls.Add(this.rb_RectAngle);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.num_Score);
            this.groupBox1.Controls.Add(this.num_ScaleY);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label87);
            this.groupBox1.Controls.Add(this.num_Angle);
            this.groupBox1.Controls.Add(this.num_ScaleX);
            this.groupBox1.Controls.Add(this.label88);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(402, 215);
            this.groupBox1.TabIndex = 441;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "검사영역";
            // 
            // btn_SetRange
            // 
            this.btn_SetRange.BackColor = System.Drawing.Color.Transparent;
            this.btn_SetRange.BaseColor = System.Drawing.Color.Transparent;
            this.btn_SetRange.ButtonColor = System.Drawing.Color.RoyalBlue;
            this.btn_SetRange.ButtonText = "검사 영역 변경";
            this.btn_SetRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btn_SetRange.ForeColor = System.Drawing.Color.Black;
            this.btn_SetRange.Location = new System.Drawing.Point(6, 115);
            this.btn_SetRange.Name = "btn_SetRange";
            this.btn_SetRange.Size = new System.Drawing.Size(384, 42);
            this.btn_SetRange.TabIndex = 3;
            this.btn_SetRange.Click += new System.EventHandler(this.btn_SetRange_Click);
            // 
            // rb_Ring
            // 
            this.rb_Ring.AutoSize = true;
            this.rb_Ring.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.rb_Ring.Location = new System.Drawing.Point(286, 175);
            this.rb_Ring.Name = "rb_Ring";
            this.rb_Ring.Size = new System.Drawing.Size(47, 28);
            this.rb_Ring.TabIndex = 441;
            this.rb_Ring.Text = "링";
            this.rb_Ring.UseVisualStyleBackColor = true;
            // 
            // rb_Circle
            // 
            this.rb_Circle.AutoSize = true;
            this.rb_Circle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.rb_Circle.Location = new System.Drawing.Point(178, 175);
            this.rb_Circle.Name = "rb_Circle";
            this.rb_Circle.Size = new System.Drawing.Size(47, 28);
            this.rb_Circle.TabIndex = 442;
            this.rb_Circle.Text = "원";
            this.rb_Circle.UseVisualStyleBackColor = true;
            // 
            // rb_RectAngle
            // 
            this.rb_RectAngle.AutoSize = true;
            this.rb_RectAngle.Checked = true;
            this.rb_RectAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.rb_RectAngle.Location = new System.Drawing.Point(50, 175);
            this.rb_RectAngle.Name = "rb_RectAngle";
            this.rb_RectAngle.Size = new System.Drawing.Size(85, 28);
            this.rb_RectAngle.TabIndex = 443;
            this.rb_RectAngle.TabStop = true;
            this.rb_RectAngle.Text = "사각형";
            this.rb_RectAngle.UseVisualStyleBackColor = true;
            // 
            // chk_UseMaster
            // 
            this.chk_UseMaster.AutoSize = true;
            this.chk_UseMaster.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.chk_UseMaster.Location = new System.Drawing.Point(10, 205);
            this.chk_UseMaster.Name = "chk_UseMaster";
            this.chk_UseMaster.Size = new System.Drawing.Size(165, 28);
            this.chk_UseMaster.TabIndex = 5;
            this.chk_UseMaster.Text = "Use MasterPoint";
            this.chk_UseMaster.UseVisualStyleBackColor = true;
            this.chk_UseMaster.CheckedChanged += new System.EventHandler(this.chk_UseMaster_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.btn_Masking);
            this.groupBox2.Controls.Add(this.chk_UseMaster);
            this.groupBox2.Controls.Add(this.ckd_PointCheck);
            this.groupBox2.Controls.Add(this.ckd_Find);
            this.groupBox2.Controls.Add(this.chk_useTrainPoint);
            this.groupBox2.Controls.Add(this.cogDisplay_Model);
            this.groupBox2.Controls.Add(this.cb_Model);
            this.groupBox2.Controls.Add(this.btn_DelModel);
            this.groupBox2.Controls.Add(this.btn_AddModel);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.groupBox2.ForeColor = System.Drawing.Color.Black;
            this.groupBox2.Location = new System.Drawing.Point(3, 343);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(402, 268);
            this.groupBox2.TabIndex = 442;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "패턴 등록";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(313, 232);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(77, 29);
            this.textBox1.TabIndex = 447;
            this.textBox1.Text = "0";
            // 
            // btn_Masking
            // 
            this.btn_Masking.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btn_Masking.Location = new System.Drawing.Point(218, 165);
            this.btn_Masking.Name = "btn_Masking";
            this.btn_Masking.Size = new System.Drawing.Size(149, 36);
            this.btn_Masking.TabIndex = 446;
            this.btn_Masking.Text = "Masking";
            this.btn_Masking.UseVisualStyleBackColor = true;
            this.btn_Masking.Click += new System.EventHandler(this.btn_Masking_Click);
            // 
            // ckd_PointCheck
            // 
            this.ckd_PointCheck.AutoSize = true;
            this.ckd_PointCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.ckd_PointCheck.Location = new System.Drawing.Point(135, 234);
            this.ckd_PointCheck.Name = "ckd_PointCheck";
            this.ckd_PointCheck.Size = new System.Drawing.Size(172, 28);
            this.ckd_PointCheck.TabIndex = 6;
            this.ckd_PointCheck.Text = "포인트 오차 체크";
            this.ckd_PointCheck.UseVisualStyleBackColor = true;
            // 
            // ckd_Find
            // 
            this.ckd_Find.AutoSize = true;
            this.ckd_Find.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.ckd_Find.Location = new System.Drawing.Point(10, 235);
            this.ckd_Find.Name = "ckd_Find";
            this.ckd_Find.Size = new System.Drawing.Size(119, 28);
            this.ckd_Find.TabIndex = 6;
            this.ckd_Find.Text = "찾으면 NG";
            this.ckd_Find.UseVisualStyleBackColor = true;
            // 
            // chk_useTrainPoint
            // 
            this.chk_useTrainPoint.AutoSize = true;
            this.chk_useTrainPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.chk_useTrainPoint.Location = new System.Drawing.Point(9, 176);
            this.chk_useTrainPoint.Name = "chk_useTrainPoint";
            this.chk_useTrainPoint.Size = new System.Drawing.Size(165, 28);
            this.chk_useTrainPoint.TabIndex = 5;
            this.chk_useTrainPoint.Text = "Use CenterPoint";
            this.chk_useTrainPoint.UseVisualStyleBackColor = true;
            this.chk_useTrainPoint.Visible = false;
            // 
            // btn_DelModel
            // 
            this.btn_DelModel.BackColor = System.Drawing.Color.Transparent;
            this.btn_DelModel.ButtonText = "삭제";
            this.btn_DelModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btn_DelModel.Location = new System.Drawing.Point(37, 122);
            this.btn_DelModel.Name = "btn_DelModel";
            this.btn_DelModel.Size = new System.Drawing.Size(118, 41);
            this.btn_DelModel.TabIndex = 4;
            this.btn_DelModel.Click += new System.EventHandler(this.btn_DelModel_Click);
            // 
            // btn_AddModel
            // 
            this.btn_AddModel.BackColor = System.Drawing.Color.Transparent;
            this.btn_AddModel.ButtonText = "추가";
            this.btn_AddModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btn_AddModel.Location = new System.Drawing.Point(39, 72);
            this.btn_AddModel.Name = "btn_AddModel";
            this.btn_AddModel.Size = new System.Drawing.Size(118, 41);
            this.btn_AddModel.TabIndex = 4;
            this.btn_AddModel.Click += new System.EventHandler(this.btn_AddModel_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Controls.Add(this.btn_ModifyConvertMethod);
            this.groupBox3.Controls.Add(this.btn_SaveConvertMethod);
            this.groupBox3.Controls.Add(this.btn_ConvertImageSet);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.cb_ConvertMethod);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.groupBox3.ForeColor = System.Drawing.Color.Black;
            this.groupBox3.Location = new System.Drawing.Point(423, 466);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(402, 119);
            this.groupBox3.TabIndex = 443;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "이미지 변환 방법";
            // 
            // btn_ModifyConvertMethod
            // 
            this.btn_ModifyConvertMethod.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btn_ModifyConvertMethod.Location = new System.Drawing.Point(10, 73);
            this.btn_ModifyConvertMethod.Name = "btn_ModifyConvertMethod";
            this.btn_ModifyConvertMethod.Size = new System.Drawing.Size(113, 36);
            this.btn_ModifyConvertMethod.TabIndex = 433;
            this.btn_ModifyConvertMethod.Text = "수정";
            this.btn_ModifyConvertMethod.UseVisualStyleBackColor = true;
            this.btn_ModifyConvertMethod.Click += new System.EventHandler(this.btn_ModifyConvertMethod_Click);
            // 
            // btn_SaveConvertMethod
            // 
            this.btn_SaveConvertMethod.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btn_SaveConvertMethod.Location = new System.Drawing.Point(124, 73);
            this.btn_SaveConvertMethod.Name = "btn_SaveConvertMethod";
            this.btn_SaveConvertMethod.Size = new System.Drawing.Size(113, 36);
            this.btn_SaveConvertMethod.TabIndex = 433;
            this.btn_SaveConvertMethod.Text = "저장";
            this.btn_SaveConvertMethod.UseVisualStyleBackColor = true;
            this.btn_SaveConvertMethod.Click += new System.EventHandler(this.btn_SaveConvertMethod_Click);
            // 
            // btn_ConvertImageSet
            // 
            this.btn_ConvertImageSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btn_ConvertImageSet.Location = new System.Drawing.Point(238, 73);
            this.btn_ConvertImageSet.Name = "btn_ConvertImageSet";
            this.btn_ConvertImageSet.Size = new System.Drawing.Size(159, 36);
            this.btn_ConvertImageSet.TabIndex = 433;
            this.btn_ConvertImageSet.Text = "세부세팅";
            this.btn_ConvertImageSet.UseVisualStyleBackColor = true;
            this.btn_ConvertImageSet.Click += new System.EventHandler(this.btn_ConvertImageSet_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.label1.Location = new System.Drawing.Point(23, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 24);
            this.label1.TabIndex = 432;
            this.label1.Text = "이미지 변환";
            this.label1.DoubleClick += new System.EventHandler(this.label1_Click);
            // 
            // cb_ConvertMethod
            // 
            this.cb_ConvertMethod.Enabled = false;
            this.cb_ConvertMethod.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.cb_ConvertMethod.FormattingEnabled = true;
            this.cb_ConvertMethod.Items.AddRange(new object[] {
            "Hue",
            "Saturation",
            "Intensity",
            "Red",
            "Green",
            "Blue",
            "WeightedRGB",
            "ColorExtract"});
            this.cb_ConvertMethod.Location = new System.Drawing.Point(202, 29);
            this.cb_ConvertMethod.Name = "cb_ConvertMethod";
            this.cb_ConvertMethod.Size = new System.Drawing.Size(181, 32);
            this.cb_ConvertMethod.TabIndex = 0;
            // 
            // panel_WRGB
            // 
            this.panel_WRGB.BackColor = System.Drawing.Color.LightGray;
            this.panel_WRGB.Controls.Add(this.btn_CancleRGB);
            this.panel_WRGB.Controls.Add(this.btn_SaveRGB);
            this.panel_WRGB.Controls.Add(this.label4);
            this.panel_WRGB.Controls.Add(this.label3);
            this.panel_WRGB.Controls.Add(this.label2);
            this.panel_WRGB.Controls.Add(this.num_Weight_B);
            this.panel_WRGB.Controls.Add(this.num_Weight_G);
            this.panel_WRGB.Controls.Add(this.num_Weight_R);
            this.panel_WRGB.Location = new System.Drawing.Point(718, 3);
            this.panel_WRGB.Name = "panel_WRGB";
            this.panel_WRGB.Size = new System.Drawing.Size(132, 125);
            this.panel_WRGB.TabIndex = 433;
            this.panel_WRGB.Visible = false;
            // 
            // btn_CancleRGB
            // 
            this.btn_CancleRGB.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_CancleRGB.Location = new System.Drawing.Point(65, 91);
            this.btn_CancleRGB.Name = "btn_CancleRGB";
            this.btn_CancleRGB.Size = new System.Drawing.Size(62, 26);
            this.btn_CancleRGB.TabIndex = 433;
            this.btn_CancleRGB.Text = "Cancel";
            this.btn_CancleRGB.UseVisualStyleBackColor = true;
            this.btn_CancleRGB.Click += new System.EventHandler(this.btn_CancleRGB_Click);
            // 
            // btn_SaveRGB
            // 
            this.btn_SaveRGB.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_SaveRGB.Location = new System.Drawing.Point(5, 91);
            this.btn_SaveRGB.Name = "btn_SaveRGB";
            this.btn_SaveRGB.Size = new System.Drawing.Size(58, 26);
            this.btn_SaveRGB.TabIndex = 433;
            this.btn_SaveRGB.Text = "Save";
            this.btn_SaveRGB.UseVisualStyleBackColor = true;
            this.btn_SaveRGB.Click += new System.EventHandler(this.btn_SaveRGB_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(12, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 20);
            this.label4.TabIndex = 432;
            this.label4.Text = "B";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(12, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 20);
            this.label3.TabIndex = 432;
            this.label3.Text = "G";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(12, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 20);
            this.label2.TabIndex = 432;
            this.label2.Text = "R";
            // 
            // num_Weight_B
            // 
            this.num_Weight_B.DecimalPlaces = 1;
            this.num_Weight_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.num_Weight_B.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.num_Weight_B.Location = new System.Drawing.Point(38, 59);
            this.num_Weight_B.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.num_Weight_B.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            this.num_Weight_B.Name = "num_Weight_B";
            this.num_Weight_B.Size = new System.Drawing.Size(82, 26);
            this.num_Weight_B.TabIndex = 0;
            // 
            // num_Weight_G
            // 
            this.num_Weight_G.DecimalPlaces = 1;
            this.num_Weight_G.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.num_Weight_G.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.num_Weight_G.Location = new System.Drawing.Point(38, 33);
            this.num_Weight_G.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.num_Weight_G.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            this.num_Weight_G.Name = "num_Weight_G";
            this.num_Weight_G.Size = new System.Drawing.Size(82, 26);
            this.num_Weight_G.TabIndex = 0;
            // 
            // num_Weight_R
            // 
            this.num_Weight_R.DecimalPlaces = 1;
            this.num_Weight_R.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.num_Weight_R.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.num_Weight_R.Location = new System.Drawing.Point(38, 7);
            this.num_Weight_R.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.num_Weight_R.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            this.num_Weight_R.Name = "num_Weight_R";
            this.num_Weight_R.Size = new System.Drawing.Size(82, 26);
            this.num_Weight_R.TabIndex = 0;
            // 
            // panel_Color
            // 
            this.panel_Color.BackColor = System.Drawing.Color.LightGray;
            this.panel_Color.Controls.Add(this.cogDisplay_ColorModel);
            this.panel_Color.Controls.Add(this.btn_Cancle);
            this.panel_Color.Controls.Add(this.btn_ColorPatternDel);
            this.panel_Color.Controls.Add(this.btn_Save);
            this.panel_Color.Controls.Add(this.btn_Result);
            this.panel_Color.Controls.Add(this.btn_ColorPatternAdd);
            this.panel_Color.Controls.Add(this.btn_Train);
            this.panel_Color.Controls.Add(this.cb_ColorModel);
            this.panel_Color.Controls.Add(this.panel_trainParam);
            this.panel_Color.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.panel_Color.Location = new System.Drawing.Point(413, 3);
            this.panel_Color.Name = "panel_Color";
            this.panel_Color.Size = new System.Drawing.Size(302, 327);
            this.panel_Color.TabIndex = 444;
            this.panel_Color.Visible = false;
            // 
            // cogDisplay_ColorModel
            // 
            this.cogDisplay_ColorModel.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.cogDisplay_ColorModel.ColorMapLowerRoiLimit = 0D;
            this.cogDisplay_ColorModel.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.cogDisplay_ColorModel.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.cogDisplay_ColorModel.ColorMapUpperRoiLimit = 1D;
            this.cogDisplay_ColorModel.Location = new System.Drawing.Point(172, 6);
            this.cogDisplay_ColorModel.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.cogDisplay_ColorModel.MouseWheelSensitivity = 1D;
            this.cogDisplay_ColorModel.Name = "cogDisplay_ColorModel";
            this.cogDisplay_ColorModel.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("cogDisplay_ColorModel.OcxState")));
            this.cogDisplay_ColorModel.Size = new System.Drawing.Size(116, 91);
            this.cogDisplay_ColorModel.TabIndex = 382;
            // 
            // btn_Cancle
            // 
            this.btn_Cancle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btn_Cancle.Location = new System.Drawing.Point(120, 290);
            this.btn_Cancle.Name = "btn_Cancle";
            this.btn_Cancle.Size = new System.Drawing.Size(116, 29);
            this.btn_Cancle.TabIndex = 384;
            this.btn_Cancle.Text = "Cancel";
            this.btn_Cancle.UseVisualStyleBackColor = true;
            this.btn_Cancle.Click += new System.EventHandler(this.btn_Cancle_Click);
            // 
            // btn_ColorPatternDel
            // 
            this.btn_ColorPatternDel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btn_ColorPatternDel.Location = new System.Drawing.Point(4, 66);
            this.btn_ColorPatternDel.Name = "btn_ColorPatternDel";
            this.btn_ColorPatternDel.Size = new System.Drawing.Size(110, 31);
            this.btn_ColorPatternDel.TabIndex = 385;
            this.btn_ColorPatternDel.Text = "Delete";
            this.btn_ColorPatternDel.UseVisualStyleBackColor = true;
            this.btn_ColorPatternDel.Click += new System.EventHandler(this.btn_ColorPatternDel_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btn_Save.Location = new System.Drawing.Point(15, 290);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(99, 29);
            this.btn_Save.TabIndex = 385;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Result
            // 
            this.btn_Result.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btn_Result.Location = new System.Drawing.Point(120, 258);
            this.btn_Result.Name = "btn_Result";
            this.btn_Result.Size = new System.Drawing.Size(116, 29);
            this.btn_Result.TabIndex = 386;
            this.btn_Result.Text = "Output Image";
            this.btn_Result.UseVisualStyleBackColor = true;
            this.btn_Result.Click += new System.EventHandler(this.btn_Result_Click);
            // 
            // btn_ColorPatternAdd
            // 
            this.btn_ColorPatternAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btn_ColorPatternAdd.Location = new System.Drawing.Point(4, 35);
            this.btn_ColorPatternAdd.Name = "btn_ColorPatternAdd";
            this.btn_ColorPatternAdd.Size = new System.Drawing.Size(110, 31);
            this.btn_ColorPatternAdd.TabIndex = 387;
            this.btn_ColorPatternAdd.Text = "Add";
            this.btn_ColorPatternAdd.UseVisualStyleBackColor = true;
            this.btn_ColorPatternAdd.Click += new System.EventHandler(this.btn_ColorPatternAdd_Click);
            // 
            // btn_Train
            // 
            this.btn_Train.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btn_Train.Location = new System.Drawing.Point(15, 258);
            this.btn_Train.Name = "btn_Train";
            this.btn_Train.Size = new System.Drawing.Size(99, 29);
            this.btn_Train.TabIndex = 387;
            this.btn_Train.Text = "Train";
            this.btn_Train.UseVisualStyleBackColor = true;
            this.btn_Train.Click += new System.EventHandler(this.btn_Train_Click);
            // 
            // cb_ColorModel
            // 
            this.cb_ColorModel.BackColor = System.Drawing.Color.White;
            this.cb_ColorModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_ColorModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.cb_ColorModel.FormattingEnabled = true;
            this.cb_ColorModel.Location = new System.Drawing.Point(5, 6);
            this.cb_ColorModel.Name = "cb_ColorModel";
            this.cb_ColorModel.Size = new System.Drawing.Size(109, 28);
            this.cb_ColorModel.TabIndex = 383;
            this.cb_ColorModel.SelectedIndexChanged += new System.EventHandler(this.cb_ColorModel_SelectedIndexChanged);
            // 
            // panel_trainParam
            // 
            this.panel_trainParam.Controls.Add(this.label109);
            this.panel_trainParam.Controls.Add(this.label110);
            this.panel_trainParam.Controls.Add(this.label106);
            this.panel_trainParam.Controls.Add(this.label105);
            this.panel_trainParam.Controls.Add(this.label103);
            this.panel_trainParam.Controls.Add(this.num_Dilation);
            this.panel_trainParam.Controls.Add(this.num_MinPixelCnt);
            this.panel_trainParam.Controls.Add(this.num_Softness);
            this.panel_trainParam.Controls.Add(this.num_MatteLineHigh);
            this.panel_trainParam.Controls.Add(this.num_MatteLineLow);
            this.panel_trainParam.Controls.Add(this.num_HighLightLimit);
            this.panel_trainParam.Controls.Add(this.chk_MatteLineLimit);
            this.panel_trainParam.Controls.Add(this.chk_HighLightLimit);
            this.panel_trainParam.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.panel_trainParam.Location = new System.Drawing.Point(3, 101);
            this.panel_trainParam.Name = "panel_trainParam";
            this.panel_trainParam.Size = new System.Drawing.Size(296, 154);
            this.panel_trainParam.TabIndex = 381;
            // 
            // label109
            // 
            this.label109.AutoSize = true;
            this.label109.Location = new System.Drawing.Point(145, 11);
            this.label109.Name = "label109";
            this.label109.Size = new System.Drawing.Size(71, 20);
            this.label109.TabIndex = 23;
            this.label109.Text = "Flexibility";
            // 
            // label110
            // 
            this.label110.AutoSize = true;
            this.label110.Location = new System.Drawing.Point(24, 61);
            this.label110.Name = "label110";
            this.label110.Size = new System.Drawing.Size(155, 20);
            this.label110.TabIndex = 23;
            this.label110.Text = "Minimum Pixel Count";
            // 
            // label106
            // 
            this.label106.AutoSize = true;
            this.label106.Location = new System.Drawing.Point(3, 11);
            this.label106.Name = "label106";
            this.label106.Size = new System.Drawing.Size(83, 20);
            this.label106.TabIndex = 23;
            this.label106.Text = "Expansion";
            // 
            // label105
            // 
            this.label105.AutoSize = true;
            this.label105.Location = new System.Drawing.Point(145, 122);
            this.label105.Name = "label105";
            this.label105.Size = new System.Drawing.Size(38, 20);
            this.label105.TabIndex = 22;
            this.label105.Text = "Low";
            // 
            // label103
            // 
            this.label103.AutoSize = true;
            this.label103.Location = new System.Drawing.Point(145, 91);
            this.label103.Name = "label103";
            this.label103.Size = new System.Drawing.Size(42, 20);
            this.label103.TabIndex = 22;
            this.label103.Text = "Limit";
            // 
            // num_Dilation
            // 
            this.num_Dilation.Location = new System.Drawing.Point(92, 9);
            this.num_Dilation.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.num_Dilation.Name = "num_Dilation";
            this.num_Dilation.Size = new System.Drawing.Size(52, 26);
            this.num_Dilation.TabIndex = 21;
            // 
            // num_MinPixelCnt
            // 
            this.num_MinPixelCnt.Location = new System.Drawing.Point(205, 59);
            this.num_MinPixelCnt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_MinPixelCnt.Name = "num_MinPixelCnt";
            this.num_MinPixelCnt.Size = new System.Drawing.Size(80, 26);
            this.num_MinPixelCnt.TabIndex = 21;
            this.num_MinPixelCnt.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // num_Softness
            // 
            this.num_Softness.Location = new System.Drawing.Point(224, 9);
            this.num_Softness.Maximum = new decimal(new int[] {
            19,
            0,
            0,
            0});
            this.num_Softness.Name = "num_Softness";
            this.num_Softness.Size = new System.Drawing.Size(61, 26);
            this.num_Softness.TabIndex = 21;
            // 
            // num_MatteLineHigh
            // 
            this.num_MatteLineHigh.DecimalPlaces = 1;
            this.num_MatteLineHigh.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.num_MatteLineHigh.Location = new System.Drawing.Point(205, 89);
            this.num_MatteLineHigh.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.num_MatteLineHigh.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_MatteLineHigh.Name = "num_MatteLineHigh";
            this.num_MatteLineHigh.Size = new System.Drawing.Size(80, 26);
            this.num_MatteLineHigh.TabIndex = 21;
            this.num_MatteLineHigh.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            // 
            // num_MatteLineLow
            // 
            this.num_MatteLineLow.DecimalPlaces = 1;
            this.num_MatteLineLow.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.num_MatteLineLow.Location = new System.Drawing.Point(205, 120);
            this.num_MatteLineLow.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_MatteLineLow.Name = "num_MatteLineLow";
            this.num_MatteLineLow.Size = new System.Drawing.Size(80, 26);
            this.num_MatteLineLow.TabIndex = 21;
            this.num_MatteLineLow.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // num_HighLightLimit
            // 
            this.num_HighLightLimit.DecimalPlaces = 1;
            this.num_HighLightLimit.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.num_HighLightLimit.Location = new System.Drawing.Point(288, 160);
            this.num_HighLightLimit.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.num_HighLightLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_HighLightLimit.Name = "num_HighLightLimit";
            this.num_HighLightLimit.Size = new System.Drawing.Size(106, 26);
            this.num_HighLightLimit.TabIndex = 21;
            this.num_HighLightLimit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chk_MatteLineLimit
            // 
            this.chk_MatteLineLimit.AutoSize = true;
            this.chk_MatteLineLimit.Location = new System.Drawing.Point(28, 90);
            this.chk_MatteLineLimit.Name = "chk_MatteLineLimit";
            this.chk_MatteLineLimit.Size = new System.Drawing.Size(103, 24);
            this.chk_MatteLineLimit.TabIndex = 20;
            this.chk_MatteLineLimit.Text = "Matte Line";
            this.chk_MatteLineLimit.UseVisualStyleBackColor = true;
            // 
            // chk_HighLightLimit
            // 
            this.chk_HighLightLimit.AutoSize = true;
            this.chk_HighLightLimit.Location = new System.Drawing.Point(106, 161);
            this.chk_HighLightLimit.Name = "chk_HighLightLimit";
            this.chk_HighLightLimit.Size = new System.Drawing.Size(144, 24);
            this.chk_HighLightLimit.TabIndex = 20;
            this.chk_HighLightLimit.Text = "하이라이트 제한";
            this.chk_HighLightLimit.UseVisualStyleBackColor = true;
            // 
            // btn_Run
            // 
            this.btn_Run.BackColor = System.Drawing.Color.Transparent;
            this.btn_Run.BaseColor = System.Drawing.Color.Transparent;
            this.btn_Run.ButtonColor = System.Drawing.Color.RoyalBlue;
            this.btn_Run.ButtonText = "수동검사";
            this.btn_Run.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Run.ForeColor = System.Drawing.Color.Black;
            this.btn_Run.Location = new System.Drawing.Point(436, 428);
            this.btn_Run.Name = "btn_Run";
            this.btn_Run.Size = new System.Drawing.Size(402, 32);
            this.btn_Run.TabIndex = 3;
            this.btn_Run.Visible = false;
            this.btn_Run.Click += new System.EventHandler(this.btn_Run_Click_1);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox4.Controls.Add(this.lb_CenterY);
            this.groupBox4.Controls.Add(this.lb_CenterX);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.groupBox4.Location = new System.Drawing.Point(3, 617);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(157, 93);
            this.groupBox4.TabIndex = 445;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "마스터 좌표";
            // 
            // lb_CenterY
            // 
            this.lb_CenterY.Location = new System.Drawing.Point(82, 61);
            this.lb_CenterY.Name = "lb_CenterY";
            this.lb_CenterY.Size = new System.Drawing.Size(69, 24);
            this.lb_CenterY.TabIndex = 1;
            this.lb_CenterY.Text = "0";
            // 
            // lb_CenterX
            // 
            this.lb_CenterX.Location = new System.Drawing.Point(82, 34);
            this.lb_CenterX.Name = "lb_CenterX";
            this.lb_CenterX.Size = new System.Drawing.Size(69, 24);
            this.lb_CenterX.TabIndex = 1;
            this.lb_CenterX.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 61);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 24);
            this.label6.TabIndex = 0;
            this.label6.Text = "중심 Y : ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 24);
            this.label5.TabIndex = 0;
            this.label5.Text = "중심 X : ";
            // 
            // btn_MasterSave
            // 
            this.btn_MasterSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_MasterSave.Location = new System.Drawing.Point(168, 28);
            this.btn_MasterSave.Name = "btn_MasterSave";
            this.btn_MasterSave.Size = new System.Drawing.Size(65, 59);
            this.btn_MasterSave.TabIndex = 433;
            this.btn_MasterSave.Text = "마스터 저장";
            this.btn_MasterSave.UseVisualStyleBackColor = true;
            this.btn_MasterSave.Click += new System.EventHandler(this.btn_MasterSave_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox5.Controls.Add(this.btn_MasterSave);
            this.groupBox5.Controls.Add(this.lb_FindY);
            this.groupBox5.Controls.Add(this.lb_FindX);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.groupBox5.Location = new System.Drawing.Point(166, 617);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(239, 93);
            this.groupBox5.TabIndex = 445;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "찾은 중심점";
            // 
            // lb_FindY
            // 
            this.lb_FindY.Location = new System.Drawing.Point(92, 61);
            this.lb_FindY.Name = "lb_FindY";
            this.lb_FindY.Size = new System.Drawing.Size(64, 24);
            this.lb_FindY.TabIndex = 1;
            this.lb_FindY.Text = "0";
            // 
            // lb_FindX
            // 
            this.lb_FindX.Location = new System.Drawing.Point(92, 34);
            this.lb_FindX.Name = "lb_FindX";
            this.lb_FindX.Size = new System.Drawing.Size(64, 24);
            this.lb_FindX.TabIndex = 1;
            this.lb_FindX.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 61);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 24);
            this.label9.TabIndex = 0;
            this.label9.Text = "중심 Y : ";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 34);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(82, 24);
            this.label11.TabIndex = 0;
            this.label11.Text = "중심 X : ";
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox6.Controls.Add(this.cb_reverseHorizental);
            this.groupBox6.Controls.Add(this.btn_SaveCalib);
            this.groupBox6.Controls.Add(this.cb_reverseXY);
            this.groupBox6.Controls.Add(this.cb_reverseVertical);
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Controls.Add(this.num_HorizentalPixelPerMM);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.num_VerticalPixelperMM);
            this.groupBox6.Location = new System.Drawing.Point(0, 221);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(402, 116);
            this.groupBox6.TabIndex = 446;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "켈리브레이션";
            this.groupBox6.Enter += new System.EventHandler(this.groupBox6_Enter);
            // 
            // cb_reverseHorizental
            // 
            this.cb_reverseHorizental.AutoSize = true;
            this.cb_reverseHorizental.Location = new System.Drawing.Point(138, 92);
            this.cb_reverseHorizental.Name = "cb_reverseHorizental";
            this.cb_reverseHorizental.Size = new System.Drawing.Size(76, 16);
            this.cb_reverseHorizental.TabIndex = 435;
            this.cb_reverseHorizental.Text = "좌우 반전";
            this.cb_reverseHorizental.UseVisualStyleBackColor = true;
            // 
            // btn_SaveCalib
            // 
            this.btn_SaveCalib.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btn_SaveCalib.Location = new System.Drawing.Point(244, 72);
            this.btn_SaveCalib.Name = "btn_SaveCalib";
            this.btn_SaveCalib.Size = new System.Drawing.Size(149, 36);
            this.btn_SaveCalib.TabIndex = 446;
            this.btn_SaveCalib.Text = "저장";
            this.btn_SaveCalib.UseVisualStyleBackColor = true;
            this.btn_SaveCalib.Click += new System.EventHandler(this.btn_SaveCalib_Click);
            // 
            // cb_reverseXY
            // 
            this.cb_reverseXY.AutoSize = true;
            this.cb_reverseXY.Location = new System.Drawing.Point(257, 43);
            this.cb_reverseXY.Name = "cb_reverseXY";
            this.cb_reverseXY.Size = new System.Drawing.Size(104, 16);
            this.cb_reverseXY.TabIndex = 435;
            this.cb_reverseXY.Text = "가로 세로 반전";
            this.cb_reverseXY.UseVisualStyleBackColor = true;
            this.cb_reverseXY.CheckedChanged += new System.EventHandler(this.cb_reverseXY_CheckedChanged);
            // 
            // cb_reverseVertical
            // 
            this.cb_reverseVertical.AutoSize = true;
            this.cb_reverseVertical.Location = new System.Drawing.Point(138, 43);
            this.cb_reverseVertical.Name = "cb_reverseVertical";
            this.cb_reverseVertical.Size = new System.Drawing.Size(76, 16);
            this.cb_reverseVertical.TabIndex = 435;
            this.cb_reverseVertical.Text = "상하 반전";
            this.cb_reverseVertical.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label8.Location = new System.Drawing.Point(6, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 17);
            this.label8.TabIndex = 434;
            this.label8.Text = "가로 mmPerPixel";
            // 
            // num_HorizentalPixelPerMM
            // 
            this.num_HorizentalPixelPerMM.DecimalPlaces = 2;
            this.num_HorizentalPixelPerMM.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num_HorizentalPixelPerMM.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_HorizentalPixelPerMM.Location = new System.Drawing.Point(10, 83);
            this.num_HorizentalPixelPerMM.Name = "num_HorizentalPixelPerMM";
            this.num_HorizentalPixelPerMM.Size = new System.Drawing.Size(82, 29);
            this.num_HorizentalPixelPerMM.TabIndex = 433;
            this.num_HorizentalPixelPerMM.ValueChanged += new System.EventHandler(this.num_HorizentalPixelPerMM_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label7.Location = new System.Drawing.Point(6, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 17);
            this.label7.TabIndex = 434;
            this.label7.Text = "세로 mmPerPixel";
            // 
            // num_VerticalPixelperMM
            // 
            this.num_VerticalPixelperMM.DecimalPlaces = 2;
            this.num_VerticalPixelperMM.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num_VerticalPixelperMM.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_VerticalPixelperMM.Location = new System.Drawing.Point(10, 35);
            this.num_VerticalPixelperMM.Name = "num_VerticalPixelperMM";
            this.num_VerticalPixelperMM.Size = new System.Drawing.Size(82, 29);
            this.num_VerticalPixelperMM.TabIndex = 433;
            this.num_VerticalPixelperMM.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_VerticalPixelperMM.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // Pattern
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.panel_WRGB);
            this.Controls.Add(this.panel_Color);
            this.Controls.Add(this.btn_Run);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.num_Elasticity);
            this.Controls.Add(this.label86);
            this.Controls.Add(this.groupBox1);
            this.Name = "Pattern";
            this.Size = new System.Drawing.Size(865, 713);
            this.Load += new System.EventHandler(this.Pattern_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_Model)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Elasticity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_ScaleY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_ScaleX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Angle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Score)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel_WRGB.ResumeLayout(false);
            this.panel_WRGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_Weight_B)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Weight_G)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Weight_R)).EndInit();
            this.panel_Color.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cogDisplay_ColorModel)).EndInit();
            this.panel_trainParam.ResumeLayout(false);
            this.panel_trainParam.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_Dilation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_MinPixelCnt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Softness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_MatteLineHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_MatteLineLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_HighLightLimit)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_HorizentalPixelPerMM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_VerticalPixelperMM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Cognex.VisionPro.Display.CogDisplay cogDisplay_Model;
        private System.Windows.Forms.ComboBox cb_Model;
        private Utilities.VistaButton btn_SetRange;
        private Utilities.VistaButton btn_AddModel;
        private Utilities.VistaButton btn_DelModel;
        private System.Windows.Forms.NumericUpDown num_Elasticity;
        private System.Windows.Forms.Label label86;
        private System.Windows.Forms.NumericUpDown num_ScaleY;
        private System.Windows.Forms.Label label87;
        private System.Windows.Forms.NumericUpDown num_ScaleX;
        private System.Windows.Forms.Label label88;
        private System.Windows.Forms.NumericUpDown num_Angle;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown num_Score;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb_ConvertMethod;
        private System.Windows.Forms.Panel panel_WRGB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown num_Weight_B;
        private System.Windows.Forms.NumericUpDown num_Weight_G;
        private System.Windows.Forms.NumericUpDown num_Weight_R;
        private System.Windows.Forms.Button btn_CancleRGB;
        private System.Windows.Forms.Button btn_SaveRGB;
        private System.Windows.Forms.Button btn_ConvertImageSet;
        private System.Windows.Forms.Panel panel_Color;
        private System.Windows.Forms.Button btn_Cancle;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_Result;
        private System.Windows.Forms.Button btn_Train;
        private Cognex.VisionPro.Display.CogDisplay cogDisplay_ColorModel;
        private System.Windows.Forms.ComboBox cb_ColorModel;
        private System.Windows.Forms.Panel panel_trainParam;
        private System.Windows.Forms.Label label109;
        private System.Windows.Forms.Label label110;
        private System.Windows.Forms.Label label106;
        private System.Windows.Forms.Label label105;
        private System.Windows.Forms.Label label103;
        private System.Windows.Forms.NumericUpDown num_Dilation;
        private System.Windows.Forms.NumericUpDown num_MinPixelCnt;
        private System.Windows.Forms.NumericUpDown num_Softness;
        private System.Windows.Forms.NumericUpDown num_MatteLineHigh;
        private System.Windows.Forms.NumericUpDown num_MatteLineLow;
        private System.Windows.Forms.NumericUpDown num_HighLightLimit;
        private System.Windows.Forms.CheckBox chk_MatteLineLimit;
        private System.Windows.Forms.CheckBox chk_HighLightLimit;
        private System.Windows.Forms.Button btn_ColorPatternDel;
        private System.Windows.Forms.Button btn_ColorPatternAdd;
        private System.Windows.Forms.Button btn_ModifyConvertMethod;
        private System.Windows.Forms.Button btn_SaveConvertMethod;
        private System.Windows.Forms.CheckBox chk_useTrainPoint;
        private Utilities.VistaButton btn_Run;
        private System.Windows.Forms.CheckBox chk_UseMaster;
        private System.Windows.Forms.CheckBox ckd_Find;
        private System.Windows.Forms.RadioButton rb_Ring;
        private System.Windows.Forms.RadioButton rb_Circle;
        private System.Windows.Forms.RadioButton rb_RectAngle;
        private System.Windows.Forms.Button btn_Masking;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lb_CenterY;
        private System.Windows.Forms.Label lb_CenterX;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btn_MasterSave;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lb_FindY;
        private System.Windows.Forms.Label lb_FindX;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox ckd_PointCheck;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown num_VerticalPixelperMM;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown num_HorizentalPixelPerMM;
        private System.Windows.Forms.CheckBox cb_reverseHorizental;
        private System.Windows.Forms.CheckBox cb_reverseVertical;
        private System.Windows.Forms.Button btn_SaveCalib;
        private System.Windows.Forms.CheckBox cb_reverseXY;
    }
}
