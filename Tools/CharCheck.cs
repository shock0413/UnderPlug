using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Hansero.VisionLib.VisionPro;
using Cognex.VisionPro.Display;
using Cognex.VisionPro;
using Utilities;

namespace Tools
{
    public partial class CharCheck : UserControl, ToolInterface, IDisposable
    {
        private string m_Item, m_Part, m_ToolName;
        private IniFile m_PartConfig;

        private CogDisplay m_Display = new CogDisplay();
        private string m_ResultStr;

        private IniFile m_Language = new IniFile(Application.StartupPath + "\\Language.ini");

        private bool m_UseMaster = false;

        String charsetCH = "";
        String charsetCA = "";

        private string m_CompStr = "";
        private bool m_IsCreateControl = false;

        private Bitmap m_RotationBitmap;

        // 특정문자 검사
        private bool m_UseFilterChars = false;
        private string m_FilterChars = "";

        private int m_CntInsArea = 0;

        public CharCheck()
        {
            InitializeComponent();
        }

        public CharCheck(string tool, bool isCreateControl)
        {
            if (isCreateControl)
            {
                m_IsCreateControl = true;
            }
            else
            {
                m_IsCreateControl = false;
            }

            if (m_IsCreateControl)
                InitializeComponent();

            m_ToolName = tool;
        }

        //public CharCheck(bool isCreateControl)
        //{
        //    if (isCreateControl)
        //    {
        //        m_IsCreateControl = true;
        //    }
        //    else
        //    {
        //        m_IsCreateControl = false;
        //    }

        //    if (m_IsCreateControl)
        //        InitializeComponent();



        //    //pm_Char = new Pattern(m_IsCreateControl);

        //    if (m_IsCreateControl)
        //    {
        //        this.pm_Char.BackColor = System.Drawing.Color.Transparent;
        //        this.pm_Char.Location = new System.Drawing.Point(0, 157);
        //        this.pm_Char.Name = "pm_Char";
        //        this.pm_Char.PATTERN_PATH = "";
        //        this.pm_Char.Size = new System.Drawing.Size(540, 620);
        //        this.pm_Char.TabIndex = 3;
        //        this.pm_Char.TOOL_NAME = "PMTool";
        //        this.pm_Char.VIEWLABEL = true;


        //        this.Controls.Add(pm_Char);
        //    }

        //    //InitializeComponent();
        //}

        private bool _isDisplay = true;

        public string SET_STRING
        {
            set
            {
                m_CompStr = value;
            }
        }

        public bool VIEWLABEL
        {
            get
            {
                return _isDisplay;
            }
            set
            {
                _isDisplay = value;
            }
        }

        public string RESULT_STRING
        {
            get
            {
                return m_ResultStr;
            }
        }

        #region ToolBase 멤버
        public void Release()
        {
            m_Display = null;
        }

        public void SetViewLabel(bool flag)
        {
            VIEWLABEL = flag;
        }

        public string GetResult()
        {
            return m_ResultStr;
        }

        public Bitmap GetRotationBitmap()
        {
            return m_RotationBitmap;
        }

        public int SetImage(Cognex.VisionPro.Display.CogDisplay display)
        {
            m_Display = display;

            try
            {
                //이미지 변환
                pm_Char.SetImage(m_Display);

                if (m_IsCreateControl)
                {
                    // 패턴 정보 로드
                    pm_Char.TOOL_NAME = m_ToolName + "Char" + string.Format("{0:000}", (int)num_CntInsArea.Value);
                    pm_Char.PATTERN_PATH = m_ToolName + "Char\\" + cb_Char.Text;
                    pm_Char.SetInfo(m_Item, m_Part, m_ToolName + string.Format("Char{0:000}", (int)num_CntInsArea.Value));
                }

                return 0;

            }
            catch
            {
                return 1;
            }
        }

        public int SetInfo(string Item, string Part, string toolName)
        {
            m_Part = Part;
            m_Item = Item;
            m_ToolName = toolName;

            // 설정파일
            m_PartConfig = new IniFile(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Item + ".ini");

            try
            {

                // 패턴툴 생성
                pm_Char = new Pattern(m_IsCreateControl);

                if (m_IsCreateControl)
                {
                    this.pm_Char.BackColor = System.Drawing.Color.Transparent;
                    this.pm_Char.Location = new System.Drawing.Point(0, 106);
                    this.pm_Char.Name = "pm_Char";
                    this.pm_Char.PATTERN_PATH = "";
                    this.pm_Char.Size = new System.Drawing.Size(436, 713);
                    this.pm_Char.TabIndex = 3;
                    this.pm_Char.TOOL_NAME = "PMTool";
                    this.pm_Char.VIEWLABEL = true;

                    this.Controls.Add(pm_Char);
                }

                m_CntInsArea = m_PartConfig.GetInt32(m_Part + " " + m_ToolName, "Inspectionfieldcount", 1);

                if (m_IsCreateControl)
                {

                    btn_Modify.Text = "수정";

                    btn_Save.Visible = false;
                    btn_AddChar.Visible = false;
                    btn_DelChar.Visible = false;

                    if (System.IO.Directory.Exists(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Pattern\\" + m_ToolName + "Char\\Master"))
                    {
                        num_CntInsArea.Minimum = 0;
                        m_UseMaster = true;
                    }
                    else
                    {
                        num_CntInsArea.Minimum = 1;
                        m_UseMaster = false;
                    }

                    num_CntInsArea.Maximum = m_CntInsArea;

                    // 특정문자검사 상태
                    m_UseFilterChars = m_PartConfig.GetString(m_Part + " " + m_ToolName, "UseCharFilter " + num_CntInsArea.Value, "False") == "True";
                    m_FilterChars = m_PartConfig.GetString(m_Part + " " + m_ToolName, "CharFilter " + num_CntInsArea.Value, "");

                    chk_Filter.Checked = m_UseFilterChars;
                    tb_FilterChars.Text = m_FilterChars;

                    // 문자 로드
                    Load_Character();


                }
            }
            catch { }

            return 0;
        }

        public int Run()
        {

            //pattern_chain.SetImage(m_Display);
            //pattern_chain.SetInfo(m_Part, m_Item);
            pm_Char.SetViewLabel(_isDisplay);
            pm_Char.SetImage(m_Display);

            int res = 0;
            Point master_point = new Point(0, 0);

            m_ResultStr = "";

            // 마스터 검사
            if (m_UseMaster)
            {
                pm_Char.SET_USEMASTER = false;
                pm_Char.VIEWLABEL = true;
                pm_Char.TOOL_NAME = m_ToolName + "Master";
                pm_Char.PATTERN_PATH = m_ToolName + "Char\\Master";
                pm_Char.SetInfo(m_Item, m_Part, "Master");

                int tmp = pm_Char.Run();

                if (tmp > 0)
                {
                    master_point = new Point(m_PartConfig.GetInt32(m_Part + " " + m_ToolName, "MasterX", 320), m_PartConfig.GetInt32(m_Part + " " + m_ToolName, "MasterX", 240));
                }
                else
                {

                    master_point = new Point((int)pm_Char.CENTER_X, (int)pm_Char.CENTER_Y);
                }
            }


            for (int i = 1; i <= m_CntInsArea; i++)
            {
                int tmp_res = 1;
                char oneChar = '?';
                double score = 0;
                double pos_x = 0;
                double pos_y = 0;


                if (m_CompStr.Length >= m_CntInsArea)
                {
                    pm_Char.VIEWLABEL = false;
                    pm_Char.TOOL_NAME = m_ToolName + string.Format("Char{0:000}", i);
                    pm_Char.PATTERN_PATH = m_ToolName + "Char\\" + m_CompStr[i - 1];
                    pm_Char.SetInfo(m_Item, m_Part, m_ToolName + string.Format("Char{0:000}", i));

                    int tmp = pm_Char.Run();

                    double sc = pm_Char.Score;

                    if (score < pm_Char.Score)
                    {
                        score = pm_Char.Score;
                        oneChar = m_CompStr[i - 1];
                        tmp_res = 0;
                    }

                    pos_x = pm_Char.REGION_CENTER.X;
                    pos_y = pm_Char.REGION_CENTER.Y;
                }
                else
                {
                    bool useFilterChars = m_PartConfig.GetString(m_Part + " " + m_ToolName, "UseCharFilter " + i, "False") == "True";
                    string filterChars = m_PartConfig.GetString(m_Part + " " + m_ToolName, "CharFilter " + i, "");

                    StringBuilder LoadChars = new StringBuilder();

                    string[] chars = System.IO.Directory.GetDirectories(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Pattern\\" + m_ToolName + "Char");
                    for (int k = 0; k < chars.Length; k++)
                    {
                        string[] tmp = chars[k].Split('\\');
                        if (tmp[tmp.Length - 1].Length > 1) continue;

                        char c = tmp[tmp.Length - 1][0];
                        LoadChars.Append(c);
                    }

                    double[] tmp_score = new double[LoadChars.Length];

                    Console.WriteLine(LoadChars.ToString());
                    // 패턴 정보 로드

                    for (int j = 0; j < LoadChars.Length; j++)
                    {
                        //if (cb_Char.Items[j].ToString() == "Master") continue;

                        if (useFilterChars && filterChars != "")
                            if (!filterChars.Contains(LoadChars[j])) continue;

                        pm_Char.SET_USEMASTER = m_UseMaster;
                        //pm_Char.SET_MASTERPOINT = master_point;
                        pm_Char.VIEWLABEL = false;
                        pm_Char.TOOL_NAME = m_ToolName + "Char" + string.Format("{0:000}", i);
                        //pm_Char.PATTERN_PATH = "Char\\" + cb_Char.Items[j].ToString();
                        pm_Char.PATTERN_PATH = m_ToolName + "Char\\" + LoadChars[j];
                        pm_Char.SetInfo(m_Item, m_Part, m_ToolName + string.Format("Char{0:000}", i));



                        int tmp = pm_Char.Run();

                        tmp_score[j] = pm_Char.Score;

                        if (score < pm_Char.Score)
                        {
                            score = pm_Char.Score;
                            //idx = j;
                            //oneChar = cb_Char.Items[j].ToString();
                            oneChar = LoadChars[j];
                            tmp_res = 0;


                        }

                        pos_x = pm_Char.REGION_CENTER.X;
                        pos_y = pm_Char.REGION_CENTER.Y;

                    }
                }
                string tmp_str = tmp_res == 0 ? oneChar.ToString() : "?";
                m_ResultStr += tmp_str;

                if (_isDisplay)
                {

                    ToolBase tb = new ToolBase();
                    tb.DrawLabel(tmp_str, m_Display, pos_x-50, pos_y + 50, 25, tmp_res == 0 ? CogColorConstants.Green : CogColorConstants.Red, CogColorConstants.Black);
                    pm_Char.DrawRange();
                    //for (int k = 0; k < cb_Char.Items.Count; k++)
                    //{
                    //    //tb.DrawLabel(cb_Char.Items[k].ToString() + " : " + (int)(tmp_score[k] * 100), m_Display, 10 + 100 * i, 10 + 30 * k, 10, k == idx ? CogColorConstants.Orange : CogColorConstants.Grey, CogColorConstants.Black);
                    //}
                }


                res += tmp_res;
            }



            if (m_IsCreateControl)
            {
                // 패턴툴 초기화
                pm_Char.VIEWLABEL = false;
                pm_Char.TOOL_NAME = m_ToolName + "Char" + string.Format("{0:000}", (int)num_CntInsArea.Value);
                pm_Char.PATTERN_PATH = m_ToolName + "Char\\" + cb_Char.Text;
                pm_Char.SetInfo(m_Item, m_Part, m_ToolName + string.Format("Char{0:000}", (int)num_CntInsArea.Value));
            }

            return res;
        }

        public int Confirm()
        {


            m_Display.InteractiveGraphics.Clear();
            m_Display.StaticGraphics.Clear();

            pm_Char.Confirm();

            //cb_Char.Text = "";

            return 0;
        }

        public int Cancle()
        {
            m_Display.InteractiveGraphics.Clear();
            m_Display.StaticGraphics.Clear();

            return 0;
        }

        #endregion



        #region IDisposable 멤버

        void IDisposable.Dispose()
        {
            //throw new NotImplementedException();
            this.Release();

            int i = this.Controls.Count;
            while (i > 0)
            {
                try
                {
                    ((IDisposable)this.Controls[--i]).Dispose();
                }
                catch
                {
                    this.Controls[i].Dispose();
                }
            }
        }

        #endregion


        private void btn_Modify_Click(object sender, EventArgs e)
        {

            if (btn_Modify.Text == "수정")
            {
                btn_Modify.Text = "취소";

                btn_Save.Visible = true;
                btn_AddChar.Visible = true;
                btn_DelChar.Visible = true;

                btn_SaveCharFilter.Visible = true;

                // 검사영역 수 설정
                //lb_cnt.Text = "Inspection area";
                num_CntInsArea.Value = num_CntInsArea.Maximum;
                num_CntInsArea.Maximum = 100;


                // 설정 문자 
                cb_Char.Items.Clear();
                cb_Char.Items.Add("Master");

                for (int i = 0; i < 10; i++)
                    cb_Char.Items.Add(i);
                for (char c = 'A'; c <= 'Z'; c++)
                    cb_Char.Items.Add(c);

                // 특정문자 검사
                chk_Filter.Enabled = true;
                if (chk_Filter.Checked)
                    tb_FilterChars.Enabled = true;
            }
            else
            {
                btn_Modify.Text = "수정";

                btn_Save.Visible = false;
                btn_AddChar.Visible = false;
                btn_DelChar.Visible = false;

                btn_SaveCharFilter.Visible = false;

                // 검사영역 수 설정
                //lb_cnt.Text = "Inspection area";
                num_CntInsArea.Maximum = m_PartConfig.GetInt32(m_Part + " " + m_ToolName, "Inspectionfieldcount", 1);

                // 설정 문자 
                Load_Character();

                // 특정문자 검사
                chk_Filter.Enabled = false;
                tb_FilterChars.Enabled = false;

                m_UseFilterChars = m_PartConfig.GetString(m_Part + " " + m_ToolName, "UseCharFilter " + num_CntInsArea.Value, "False") == "True";
                m_FilterChars = m_PartConfig.GetString(m_Part + " " + m_ToolName, "CharFilter " + num_CntInsArea.Value, "");
            }

        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            //btn_Modify.Text = "수정";

            //btn_Save.Visible = false;
            //btn_AddChar.Visible = false;
            //btn_DelChar.Visible = false;


            // 검사영역 수 설정
            //lb_cnt.Text = "Inspection area";
            m_PartConfig.WriteValue(m_Part + " " + m_ToolName, "Inspectionfieldcount", (int)num_CntInsArea.Value);
            num_CntInsArea.Maximum = m_PartConfig.GetInt32(m_Part + " " + m_ToolName, "Inspectionfieldcount", 1);

            // 문자 로드
            //Load_Character();
        }

        private void Load_Character()
        {

            cb_Char.Items.Clear();

            if (!System.IO.Directory.Exists(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Pattern\\" + m_ToolName + "Char"))
                System.IO.Directory.CreateDirectory(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Pattern\\" + m_ToolName + "Char");

            string[] chars = System.IO.Directory.GetDirectories(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Pattern\\" + m_ToolName + "Char");

            //if(System.IO.Directory.Exists(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Pattern\\Char\\Master"))
            //    cb_Char.Items.Add("Master");

            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i].Contains("Master")) continue;
                string c = chars[i].Substring(chars[i].Length - 1, 1);

                //if (m_UseFilterChars)
                //    if (!m_FilterChars.Contains(c)) continue;

                cb_Char.Items.Add(c);

            }
        }

        private void btn_AddChar_Click(object sender, EventArgs e)
        {
            if (cb_Char.Text != "")
            {
                if (MessageBox.Show("문자 (" + cb_Char.Text + ") 추가하시겠습니까??", "Character add", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    if (!System.IO.Directory.Exists(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Pattern\\" + m_ToolName + "Char\\" + cb_Char.Text))
                        System.IO.Directory.CreateDirectory(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Pattern\\" + m_ToolName + "Char\\" + cb_Char.Text);
            }



        }

        private void btn_DelChar_Click(object sender, EventArgs e)
        {
            if (cb_Char.Text != "")
            {
                if (MessageBox.Show("문자 (" + cb_Char.Text + ") 삭제하시겠습니까?", "Character Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    if (System.IO.Directory.Exists(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Pattern\\" + m_ToolName + "Char\\" + cb_Char.Text))
                        System.IO.Directory.Delete(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Pattern\\" + m_ToolName + "Char\\" + cb_Char.Text);
            }



        }

        private void num_CntInsArea_ValueChanged(object sender, EventArgs e)
        {
            //if (cb_Char.Text == "") return;


            Point master_point = new Point(0, 0);

            if (((int)num_CntInsArea.Minimum == 0) && (m_Display.Image != null))
            {


                pm_Char.SET_USEMASTER = false;
                pm_Char.VIEWLABEL = true;
                pm_Char.TOOL_NAME = "Master";
                pm_Char.PATTERN_PATH = m_ToolName + "Char\\Master";
                pm_Char.SetInfo(m_Item, m_Part, "Master");

                int tmp = pm_Char.Run();

                if (tmp > 0)
                {
                    master_point = new Point(m_PartConfig.GetInt32(m_Part + " " + m_ToolName, "MasterX", 320), m_PartConfig.GetInt32(m_Part + " " + m_ToolName, "MasterY", 240));
                }
                else
                {

                    master_point = new Point((int)pm_Char.CENTER_X, (int)pm_Char.CENTER_Y);
                }

                pm_Char.SET_USEMASTER = m_UseMaster;
                pm_Char.SET_MASTERPOINT = master_point;
                pm_Char.VIEWLABEL = false;
                pm_Char.TOOL_NAME = m_ToolName + "Char" + string.Format("{0:000}", (int)num_CntInsArea.Value);
                //pm_Char.PATTERN_PATH = "Char\\" + cb_Char.Items[0].ToString();
                //pm_Char.SetInfo(m_Item, m_Part);




            }

            // 패턴 정보 로드
            if ((int)num_CntInsArea.Value == 0)
            {
                pm_Char.SET_USEMASTER = false;
                pm_Char.TOOL_NAME = "Master";

                cb_Char.Items.Clear();
                cb_Char.Items.Add("Master");
            }
            else
            {
                pm_Char.TOOL_NAME = m_ToolName + "Char" + string.Format("{0:000}", (int)num_CntInsArea.Value);


                // 특정문자검사 상태
                m_UseFilterChars = m_PartConfig.GetString(m_Part + " " + m_ToolName, "UseCharFilter " + num_CntInsArea.Value, "False") == "True";
                m_FilterChars = m_PartConfig.GetString(m_Part + " " + m_ToolName, "CharFilter " + num_CntInsArea.Value, "");

                chk_Filter.Checked = m_UseFilterChars;
                tb_FilterChars.Text = m_FilterChars;

                Load_Character();
            }

            //pm_Char.PATTERN_PATH = "Char\\" + cb_Char.Text;
            pm_Char.SetInfo(m_Item, m_Part, pm_Char.TOOL_NAME);

            try
            {
                m_Display.StaticGraphics.Clear();
                m_Display.InteractiveGraphics.Clear();
            }
            catch (Exception ex)
            {


            }
            //pm_Char.DrawRange();


        }

        private void cb_Char_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_Char.Text == "") return;

            // 패턴 정보 로드
            if ((int)num_CntInsArea.Value == 0)
                pm_Char.TOOL_NAME = "Master";

            else
            {
                pm_Char.TOOL_NAME = m_ToolName + "Char" + string.Format("{0:000}", (int)num_CntInsArea.Value);
                pm_Char.PATTERN_PATH = m_ToolName + "Char\\" + cb_Char.Text;
                pm_Char.SetInfo(m_Item, m_Part, pm_Char.TOOL_NAME);
            }


        }

        private void pm_Char_Load(object sender, EventArgs e)
        {

        }

        private void btn_SaveCharFilter_Click(object sender, EventArgs e)
        {
            m_UseFilterChars = chk_Filter.Checked;
            m_FilterChars = tb_FilterChars.Text;
            // 특정문자 검사
            m_PartConfig.WriteValue(m_Part + " " + m_ToolName, "UseCharFilter " + num_CntInsArea.Value, m_UseFilterChars.ToString());
            m_PartConfig.WriteValue(m_Part + " " + m_ToolName, "CharFilter " + num_CntInsArea.Value, m_FilterChars);
        }

        private void chk_Filter_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Filter.Enabled)
                tb_FilterChars.Enabled = chk_Filter.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                this.Run();

                CogGraphicLabel tmp = new CogGraphicLabel();
                tmp.Text = "CHAR : " + m_ResultStr;
                tmp.BackgroundColor = CogColorConstants.Black;
                tmp.Color = CogColorConstants.Orange;
                tmp.Font = new Font("맑은 고딕", 10);
                tmp.X = 10;
                tmp.Y = 40;
                tmp.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
                m_Display.StaticGraphics.Add(tmp, "ReadString");

            }
            catch (Exception ex)
            {

                MessageBox.Show("Image Not Find");

            }
        }

        private void CharCheck_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

    }
}
