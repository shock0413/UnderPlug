
namespace Setting
{
    partial class CarKindAndHoleSettingWindow
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dg_CarKind = new System.Windows.Forms.DataGridView();
            this.save_btn = new System.Windows.Forms.Button();
            this.restore_btn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Model = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Position = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InspectionModeStr = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.CorrectX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CorrectY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AI_CorrectX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AI_CorrectY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.score_numeric = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.ai_score_numeric = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.dg_CarKind)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.score_numeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ai_score_numeric)).BeginInit();
            this.SuspendLayout();
            // 
            // dg_CarKind
            // 
            this.dg_CarKind.AutoGenerateColumns = global::Setting.Properties.Settings.Default.autoGenerateColumns;
            this.dg_CarKind.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_CarKind.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dg_CarKind.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_CarKind.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Model,
            this.Position,
            this.InspectionModeStr,
            this.CorrectX,
            this.CorrectY,
            this.AI_CorrectX,
            this.AI_CorrectY});
            this.dg_CarKind.DataBindings.Add(new System.Windows.Forms.Binding("AutoGenerateColumns", global::Setting.Properties.Settings.Default, "autoGenerateColumns", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dg_CarKind.GridColor = System.Drawing.Color.Silver;
            this.dg_CarKind.Location = new System.Drawing.Point(12, 63);
            this.dg_CarKind.Name = "dg_CarKind";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dg_CarKind.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dg_CarKind.RowTemplate.Height = 23;
            this.dg_CarKind.Size = new System.Drawing.Size(1114, 412);
            this.dg_CarKind.TabIndex = 0;
            // 
            // save_btn
            // 
            this.save_btn.BackColor = System.Drawing.Color.White;
            this.save_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.save_btn.Location = new System.Drawing.Point(12, 12);
            this.save_btn.Name = "save_btn";
            this.save_btn.Size = new System.Drawing.Size(109, 45);
            this.save_btn.TabIndex = 1;
            this.save_btn.Text = "저장";
            this.save_btn.UseVisualStyleBackColor = false;
            this.save_btn.Click += new System.EventHandler(this.save_btn_Click);
            // 
            // restore_btn
            // 
            this.restore_btn.BackColor = System.Drawing.Color.White;
            this.restore_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.restore_btn.Location = new System.Drawing.Point(127, 12);
            this.restore_btn.Name = "restore_btn";
            this.restore_btn.Size = new System.Drawing.Size(109, 45);
            this.restore_btn.TabIndex = 2;
            this.restore_btn.Text = "되돌리기";
            this.restore_btn.UseVisualStyleBackColor = false;
            this.restore_btn.Click += new System.EventHandler(this.restore_btn_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.dg_CarKind);
            this.panel1.Controls.Add(this.save_btn);
            this.panel1.Controls.Add(this.restore_btn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1138, 487);
            this.panel1.TabIndex = 3;
            // 
            // Model
            // 
            this.Model.DataPropertyName = "Model";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Model.DefaultCellStyle = dataGridViewCellStyle2;
            this.Model.HeaderText = "차종명";
            this.Model.Name = "Model";
            this.Model.ReadOnly = true;
            // 
            // Position
            // 
            this.Position.DataPropertyName = "Position";
            dataGridViewCellStyle3.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Position.DefaultCellStyle = dataGridViewCellStyle3;
            this.Position.HeaderText = "위치";
            this.Position.Name = "Position";
            this.Position.ReadOnly = true;
            // 
            // InspectionModeStr
            // 
            this.InspectionModeStr.DataPropertyName = "InspectionModeStr";
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.InspectionModeStr.DefaultCellStyle = dataGridViewCellStyle4;
            this.InspectionModeStr.FillWeight = 130F;
            this.InspectionModeStr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InspectionModeStr.HeaderText = "검사모드";
            this.InspectionModeStr.Items.AddRange(new object[] {
            "AI + 패턴 모드",
            "AI 모드",
            "패턴 모드"});
            this.InspectionModeStr.Name = "InspectionModeStr";
            this.InspectionModeStr.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.InspectionModeStr.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.InspectionModeStr.Width = 130;
            // 
            // CorrectX
            // 
            this.CorrectX.DataPropertyName = "CorrectX";
            dataGridViewCellStyle5.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.CorrectX.DefaultCellStyle = dataGridViewCellStyle5;
            this.CorrectX.HeaderText = "보정값X";
            this.CorrectX.Name = "CorrectX";
            this.CorrectX.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // CorrectY
            // 
            this.CorrectY.DataPropertyName = "CorrectY";
            dataGridViewCellStyle6.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.CorrectY.DefaultCellStyle = dataGridViewCellStyle6;
            this.CorrectY.HeaderText = "보정값Y";
            this.CorrectY.Name = "CorrectY";
            this.CorrectY.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // AI_CorrectX
            // 
            this.AI_CorrectX.DataPropertyName = "AICorrectX";
            dataGridViewCellStyle7.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.AI_CorrectX.DefaultCellStyle = dataGridViewCellStyle7;
            this.AI_CorrectX.FillWeight = 120F;
            this.AI_CorrectX.HeaderText = "AI 보정값X";
            this.AI_CorrectX.Name = "AI_CorrectX";
            this.AI_CorrectX.Width = 120;
            // 
            // AI_CorrectY
            // 
            this.AI_CorrectY.DataPropertyName = "AICorrectY";
            dataGridViewCellStyle8.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.AI_CorrectY.DefaultCellStyle = dataGridViewCellStyle8;
            this.AI_CorrectY.FillWeight = 120F;
            this.AI_CorrectY.HeaderText = "AI 보정값Y";
            this.AI_CorrectY.Name = "AI_CorrectY";
            this.AI_CorrectY.Width = 120;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "패턴 합격 점수";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.ai_score_numeric);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.score_numeric);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(402, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(583, 45);
            this.panel2.TabIndex = 4;
            // 
            // score_numeric
            // 
            this.score_numeric.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.score_numeric.Location = new System.Drawing.Point(134, 11);
            this.score_numeric.Name = "score_numeric";
            this.score_numeric.Size = new System.Drawing.Size(145, 26);
            this.score_numeric.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(316, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "AI 합격 점수";
            // 
            // ai_score_numeric
            // 
            this.ai_score_numeric.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ai_score_numeric.Location = new System.Drawing.Point(417, 11);
            this.ai_score_numeric.Name = "ai_score_numeric";
            this.ai_score_numeric.Size = new System.Drawing.Size(145, 26);
            this.ai_score_numeric.TabIndex = 6;
            // 
            // CarKindAndHoleSettingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.ClientSize = new System.Drawing.Size(1138, 487);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "CarKindAndHoleSettingWindow";
            this.Text = "차종 & 홀 설정";
            ((System.ComponentModel.ISupportInitialize)(this.dg_CarKind)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.score_numeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ai_score_numeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dg_CarKind;
        private System.Windows.Forms.Button save_btn;
        private System.Windows.Forms.Button restore_btn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Model;
        private System.Windows.Forms.DataGridViewTextBoxColumn Position;
        private System.Windows.Forms.DataGridViewComboBoxColumn InspectionModeStr;
        private System.Windows.Forms.DataGridViewTextBoxColumn CorrectX;
        private System.Windows.Forms.DataGridViewTextBoxColumn CorrectY;
        private System.Windows.Forms.DataGridViewTextBoxColumn AI_CorrectX;
        private System.Windows.Forms.DataGridViewTextBoxColumn AI_CorrectY;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown ai_score_numeric;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown score_numeric;
        private System.Windows.Forms.Label label1;
    }
}