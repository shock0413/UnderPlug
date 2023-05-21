namespace Tools
{
    partial class HistoTool
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
            this.btn_SetRange = new Utilities.VistaButton();
            this.num_HistoMin = new System.Windows.Forms.NumericUpDown();
            this.num_HistoMax = new System.Windows.Forms.NumericUpDown();
            this.btn_Confirm = new Utilities.VistaButton();
            this.btn_Modify = new Utilities.VistaButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Run = new Utilities.VistaButton();
            this.lb_HistoValue = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chk_UseMaster = new System.Windows.Forms.CheckBox();
            this.rb_Ring = new System.Windows.Forms.RadioButton();
            this.rb_Circle = new System.Windows.Forms.RadioButton();
            this.rb_RectAngle = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.num_HistoMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_HistoMax)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_SetRange
            // 
            this.btn_SetRange.BackColor = System.Drawing.Color.Transparent;
            this.btn_SetRange.BaseColor = System.Drawing.Color.Transparent;
            this.btn_SetRange.ButtonColor = System.Drawing.Color.RoyalBlue;
            this.btn_SetRange.ButtonText = "검사 영역";
            this.btn_SetRange.Font = new System.Drawing.Font("Cambria", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SetRange.Location = new System.Drawing.Point(34, 134);
            this.btn_SetRange.Name = "btn_SetRange";
            this.btn_SetRange.Size = new System.Drawing.Size(273, 46);
            this.btn_SetRange.TabIndex = 7;
            this.btn_SetRange.Click += new System.EventHandler(this.btn_SetRange_Click);
            // 
            // num_HistoMin
            // 
            this.num_HistoMin.Font = new System.Drawing.Font("휴먼모음T", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num_HistoMin.Location = new System.Drawing.Point(168, 247);
            this.num_HistoMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.num_HistoMin.Name = "num_HistoMin";
            this.num_HistoMin.Size = new System.Drawing.Size(121, 26);
            this.num_HistoMin.TabIndex = 8;
            // 
            // num_HistoMax
            // 
            this.num_HistoMax.Font = new System.Drawing.Font("휴먼모음T", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num_HistoMax.Location = new System.Drawing.Point(168, 283);
            this.num_HistoMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.num_HistoMax.Name = "num_HistoMax";
            this.num_HistoMax.Size = new System.Drawing.Size(121, 26);
            this.num_HistoMax.TabIndex = 8;
            // 
            // btn_Confirm
            // 
            this.btn_Confirm.BackColor = System.Drawing.Color.Transparent;
            this.btn_Confirm.ButtonText = "Save";
            this.btn_Confirm.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Confirm.Location = new System.Drawing.Point(195, 322);
            this.btn_Confirm.Name = "btn_Confirm";
            this.btn_Confirm.Size = new System.Drawing.Size(94, 38);
            this.btn_Confirm.TabIndex = 10;
            this.btn_Confirm.Click += new System.EventHandler(this.btn_Confirm_Click);
            // 
            // btn_Modify
            // 
            this.btn_Modify.BackColor = System.Drawing.Color.Transparent;
            this.btn_Modify.ButtonText = "Change";
            this.btn_Modify.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Modify.Location = new System.Drawing.Point(83, 322);
            this.btn_Modify.Name = "btn_Modify";
            this.btn_Modify.Size = new System.Drawing.Size(94, 38);
            this.btn_Modify.TabIndex = 9;
            this.btn_Modify.Click += new System.EventHandler(this.btn_Modify_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(78, 247);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 19);
            this.label1.TabIndex = 11;
            this.label1.Text = "Min";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(78, 283);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 19);
            this.label2.TabIndex = 11;
            this.label2.Text = "Max";
            // 
            // btn_Run
            // 
            this.btn_Run.BackColor = System.Drawing.Color.Transparent;
            this.btn_Run.BaseColor = System.Drawing.Color.Transparent;
            this.btn_Run.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btn_Run.ButtonText = "수동 검사";
            this.btn_Run.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Run.GlowColor = System.Drawing.Color.DarkSeaGreen;
            this.btn_Run.Location = new System.Drawing.Point(47, 378);
            this.btn_Run.Name = "btn_Run";
            this.btn_Run.Size = new System.Drawing.Size(273, 46);
            this.btn_Run.TabIndex = 12;
            this.btn_Run.Click += new System.EventHandler(this.btn_Run_Click);
            // 
            // lb_HistoValue
            // 
            this.lb_HistoValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lb_HistoValue.Font = new System.Drawing.Font("휴먼모음T", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lb_HistoValue.Location = new System.Drawing.Point(47, 430);
            this.lb_HistoValue.Name = "lb_HistoValue";
            this.lb_HistoValue.Size = new System.Drawing.Size(273, 40);
            this.lb_HistoValue.TabIndex = 13;
            this.lb_HistoValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chk_UseMaster);
            this.groupBox1.Controls.Add(this.rb_Ring);
            this.groupBox1.Controls.Add(this.rb_Circle);
            this.groupBox1.Controls.Add(this.rb_RectAngle);
            this.groupBox1.Controls.Add(this.btn_SetRange);
            this.groupBox1.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(15, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(341, 210);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "검사 세팅";
            // 
            // chk_UseMaster
            // 
            this.chk_UseMaster.AutoSize = true;
            this.chk_UseMaster.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk_UseMaster.Location = new System.Drawing.Point(32, 93);
            this.chk_UseMaster.Name = "chk_UseMaster";
            this.chk_UseMaster.Size = new System.Drawing.Size(123, 23);
            this.chk_UseMaster.TabIndex = 9;
            this.chk_UseMaster.Text = "Master Point";
            this.chk_UseMaster.UseVisualStyleBackColor = true;
            this.chk_UseMaster.CheckedChanged += new System.EventHandler(this.chk_UseMaster_CheckedChanged);
            // 
            // rb_Ring
            // 
            this.rb_Ring.AutoSize = true;
            this.rb_Ring.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb_Ring.Location = new System.Drawing.Point(236, 43);
            this.rb_Ring.Name = "rb_Ring";
            this.rb_Ring.Size = new System.Drawing.Size(79, 26);
            this.rb_Ring.TabIndex = 8;
            this.rb_Ring.TabStop = true;
            this.rb_Ring.Text = "Circle";
            this.rb_Ring.UseVisualStyleBackColor = true;
            // 
            // rb_Circle
            // 
            this.rb_Circle.AutoSize = true;
            this.rb_Circle.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb_Circle.Location = new System.Drawing.Point(150, 43);
            this.rb_Circle.Name = "rb_Circle";
            this.rb_Circle.Size = new System.Drawing.Size(68, 26);
            this.rb_Circle.TabIndex = 8;
            this.rb_Circle.TabStop = true;
            this.rb_Circle.Text = "Ring";
            this.rb_Circle.UseVisualStyleBackColor = true;
            // 
            // rb_RectAngle
            // 
            this.rb_RectAngle.AutoSize = true;
            this.rb_RectAngle.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb_RectAngle.Location = new System.Drawing.Point(36, 43);
            this.rb_RectAngle.Name = "rb_RectAngle";
            this.rb_RectAngle.Size = new System.Drawing.Size(114, 26);
            this.rb_RectAngle.TabIndex = 8;
            this.rb_RectAngle.TabStop = true;
            this.rb_RectAngle.Text = "Rectangle";
            this.rb_RectAngle.UseVisualStyleBackColor = true;
            // 
            // HistoTool
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lb_HistoValue);
            this.Controls.Add(this.btn_Run);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Confirm);
            this.Controls.Add(this.btn_Modify);
            this.Controls.Add(this.num_HistoMax);
            this.Controls.Add(this.num_HistoMin);
            this.Name = "HistoTool";
            this.Size = new System.Drawing.Size(383, 508);
            this.Load += new System.EventHandler(this.HistoTool_Load);
            ((System.ComponentModel.ISupportInitialize)(this.num_HistoMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_HistoMax)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Utilities.VistaButton btn_SetRange;
        private System.Windows.Forms.NumericUpDown num_HistoMin;
        private System.Windows.Forms.NumericUpDown num_HistoMax;
        private Utilities.VistaButton btn_Confirm;
        private Utilities.VistaButton btn_Modify;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Utilities.VistaButton btn_Run;
        private System.Windows.Forms.Label lb_HistoValue;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chk_UseMaster;
        private System.Windows.Forms.RadioButton rb_Ring;
        private System.Windows.Forms.RadioButton rb_Circle;
        private System.Windows.Forms.RadioButton rb_RectAngle;
    }
}
