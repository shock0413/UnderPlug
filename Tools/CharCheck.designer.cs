namespace Tools
{
    partial class CharCheck
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_Complete = new System.Windows.Forms.Button();
            this.tb_FilterChars = new System.Windows.Forms.TextBox();
            this.chk_Filter = new System.Windows.Forms.CheckBox();
            this.btn_SaveCharFilter = new System.Windows.Forms.Button();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_DelChar = new System.Windows.Forms.Button();
            this.btn_AddChar = new System.Windows.Forms.Button();
            this.btn_Modify = new System.Windows.Forms.Button();
            this.cb_Char = new System.Windows.Forms.ComboBox();
            this.num_CntInsArea = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.lb_cnt = new System.Windows.Forms.Label();
            this.pm_Char = new Tools.Pattern();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_CntInsArea)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Beige;
            this.groupBox1.Controls.Add(this.btn_Complete);
            this.groupBox1.Controls.Add(this.tb_FilterChars);
            this.groupBox1.Controls.Add(this.chk_Filter);
            this.groupBox1.Controls.Add(this.btn_SaveCharFilter);
            this.groupBox1.Controls.Add(this.btn_Save);
            this.groupBox1.Controls.Add(this.btn_DelChar);
            this.groupBox1.Controls.Add(this.btn_AddChar);
            this.groupBox1.Controls.Add(this.btn_Modify);
            this.groupBox1.Controls.Add(this.cb_Char);
            this.groupBox1.Controls.Add(this.num_CntInsArea);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lb_cnt);
            this.groupBox1.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(430, 105);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "문자 관리";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // btn_Complete
            // 
            this.btn_Complete.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btn_Complete.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Complete.Location = new System.Drawing.Point(365, 61);
            this.btn_Complete.Name = "btn_Complete";
            this.btn_Complete.Size = new System.Drawing.Size(52, 32);
            this.btn_Complete.TabIndex = 6;
            this.btn_Complete.Text = "완료";
            this.btn_Complete.UseVisualStyleBackColor = false;
            this.btn_Complete.Visible = false;
            this.btn_Complete.Click += new System.EventHandler(this.button1_Click);
            // 
            // tb_FilterChars
            // 
            this.tb_FilterChars.Enabled = false;
            this.tb_FilterChars.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.tb_FilterChars.Location = new System.Drawing.Point(292, 213);
            this.tb_FilterChars.Name = "tb_FilterChars";
            this.tb_FilterChars.Size = new System.Drawing.Size(134, 26);
            this.tb_FilterChars.TabIndex = 5;
            this.tb_FilterChars.Visible = false;
            // 
            // chk_Filter
            // 
            this.chk_Filter.AutoSize = true;
            this.chk_Filter.Enabled = false;
            this.chk_Filter.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk_Filter.Location = new System.Drawing.Point(289, 180);
            this.chk_Filter.Name = "chk_Filter";
            this.chk_Filter.Size = new System.Drawing.Size(128, 24);
            this.chk_Filter.TabIndex = 4;
            this.chk_Filter.Text = "특정 문자 검사";
            this.chk_Filter.UseVisualStyleBackColor = true;
            this.chk_Filter.Visible = false;
            this.chk_Filter.CheckedChanged += new System.EventHandler(this.chk_Filter_CheckedChanged);
            // 
            // btn_SaveCharFilter
            // 
            this.btn_SaveCharFilter.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SaveCharFilter.Location = new System.Drawing.Point(432, 212);
            this.btn_SaveCharFilter.Name = "btn_SaveCharFilter";
            this.btn_SaveCharFilter.Size = new System.Drawing.Size(79, 27);
            this.btn_SaveCharFilter.TabIndex = 3;
            this.btn_SaveCharFilter.Text = "저장";
            this.btn_SaveCharFilter.UseVisualStyleBackColor = true;
            this.btn_SaveCharFilter.Visible = false;
            this.btn_SaveCharFilter.Click += new System.EventHandler(this.btn_SaveCharFilter_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Save.Location = new System.Drawing.Point(162, 30);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(55, 27);
            this.btn_Save.TabIndex = 3;
            this.btn_Save.Text = "저장";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Visible = false;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_DelChar
            // 
            this.btn_DelChar.BackColor = System.Drawing.Color.Orange;
            this.btn_DelChar.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_DelChar.Location = new System.Drawing.Point(225, 61);
            this.btn_DelChar.Name = "btn_DelChar";
            this.btn_DelChar.Size = new System.Drawing.Size(58, 32);
            this.btn_DelChar.TabIndex = 3;
            this.btn_DelChar.Text = "삭제";
            this.btn_DelChar.UseVisualStyleBackColor = false;
            this.btn_DelChar.Visible = false;
            this.btn_DelChar.Click += new System.EventHandler(this.btn_DelChar_Click);
            // 
            // btn_AddChar
            // 
            this.btn_AddChar.BackColor = System.Drawing.Color.Orange;
            this.btn_AddChar.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_AddChar.Location = new System.Drawing.Point(161, 61);
            this.btn_AddChar.Name = "btn_AddChar";
            this.btn_AddChar.Size = new System.Drawing.Size(58, 32);
            this.btn_AddChar.TabIndex = 3;
            this.btn_AddChar.Text = "추가";
            this.btn_AddChar.UseVisualStyleBackColor = false;
            this.btn_AddChar.Visible = false;
            this.btn_AddChar.Click += new System.EventHandler(this.btn_AddChar_Click);
            // 
            // btn_Modify
            // 
            this.btn_Modify.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btn_Modify.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Modify.Location = new System.Drawing.Point(307, 61);
            this.btn_Modify.Name = "btn_Modify";
            this.btn_Modify.Size = new System.Drawing.Size(52, 32);
            this.btn_Modify.TabIndex = 3;
            this.btn_Modify.Text = "수정";
            this.btn_Modify.UseVisualStyleBackColor = false;
            this.btn_Modify.Click += new System.EventHandler(this.btn_Modify_Click);
            // 
            // cb_Char
            // 
            this.cb_Char.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cb_Char.FormattingEnabled = true;
            this.cb_Char.Location = new System.Drawing.Point(72, 64);
            this.cb_Char.Name = "cb_Char";
            this.cb_Char.Size = new System.Drawing.Size(82, 28);
            this.cb_Char.TabIndex = 2;
            this.cb_Char.SelectedIndexChanged += new System.EventHandler(this.cb_Char_SelectedIndexChanged);
            // 
            // num_CntInsArea
            // 
            this.num_CntInsArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.num_CntInsArea.Location = new System.Drawing.Point(98, 30);
            this.num_CntInsArea.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_CntInsArea.Name = "num_CntInsArea";
            this.num_CntInsArea.Size = new System.Drawing.Size(58, 26);
            this.num_CntInsArea.TabIndex = 1;
            this.num_CntInsArea.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_CntInsArea.ValueChanged += new System.EventHandler(this.num_CntInsArea_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "* 문자";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_cnt
            // 
            this.lb_cnt.AutoSize = true;
            this.lb_cnt.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_cnt.Location = new System.Drawing.Point(6, 32);
            this.lb_cnt.Name = "lb_cnt";
            this.lb_cnt.Size = new System.Drawing.Size(86, 20);
            this.lb_cnt.TabIndex = 0;
            this.lb_cnt.Text = "* 검사 영역";
            this.lb_cnt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pm_Char
            // 
            this.pm_Char.BackColor = System.Drawing.Color.Transparent;
            this.pm_Char.Location = new System.Drawing.Point(3, 157);
            this.pm_Char.Name = "pm_Char";
            this.pm_Char.PATTERN_PATH = "";
            this.pm_Char.SerchMasterAngle = 0D;
            this.pm_Char.Size = new System.Drawing.Size(528, 632);
            this.pm_Char.TabIndex = 0;
            this.pm_Char.TOOL_NAME = "PMTool";
            this.pm_Char.VIEWLABEL = true;
            // 
            // CharCheck
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.groupBox1);
            this.Name = "CharCheck";
            this.Size = new System.Drawing.Size(436, 840);
            this.Load += new System.EventHandler(this.CharCheck_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_CntInsArea)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Pattern pm_Char;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_Complete;
        private System.Windows.Forms.TextBox tb_FilterChars;
        private System.Windows.Forms.CheckBox chk_Filter;
        private System.Windows.Forms.Button btn_SaveCharFilter;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_DelChar;
        private System.Windows.Forms.Button btn_AddChar;
        private System.Windows.Forms.Button btn_Modify;
        private System.Windows.Forms.ComboBox cb_Char;
        private System.Windows.Forms.NumericUpDown num_CntInsArea;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lb_cnt;


    }
}
