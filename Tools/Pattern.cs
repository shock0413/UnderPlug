using Cognex.VisionPro;
using Cognex.VisionPro.Display;
using Hansero.VisionLib.VisionPro;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Utilities;

using Cognex.VisionPro.PMAlign;
using System.Diagnostics;

namespace Tools
{
    public partial class Pattern : UserControl, ToolInterface, IDisposable
    {
        private IniFile m_Config = new IniFile(Application.StartupPath + "\\Config.ini");

        // 부품 정보   
        private string m_Item, m_Part;
        private IniFile m_PartConfig;

        // 검사 상태
        private enum STATE { RANGE, MODEL, COLOR, NONE }
        private STATE m_Current;

        // 검사 툴
        private CogDisplay m_Display = null;
        private PMAlgin m_Pattern = new PMAlgin();

        // 이미지
        private ICogImage m_LoadedImage;
        private CogImage8Grey m_ConvertedImage;
        private string m_ConvertMethod = "Intencity";
        private ColorExtractor m_ColorEx = new ColorExtractor();

        // 검사결과 화면 표시여부
        private bool _isDisplay = true;

        // 콘트롤 사용여부
        private bool m_IsCreateControl = false;

        // 툴 정보
        private string m_ToolName = "PMTool";
        private string m_PatternPath = "";

        private double m_score;

        private bool _isMaster = false;
        private bool _useMaster = false;
        private bool _isFindNG = false;

        private int m_Count = 0;

        private Point m_MasterPoint;

        private double m_SerchMasterAngle;

        private int m_MasterX = 0;
        private int m_MasterY = 0;

        private bool m_MovingPointCheck = false;

        public double SerchMasterAngle
        {
            get
            {
                return m_SerchMasterAngle;
            }
            set
            {
                m_SerchMasterAngle = value;
            }
        }



        public bool SET_USEMASTER
        {
            set
            {
                _useMaster = value;
            }
        }

        public Point SET_MASTERPOINT
        {
            set
            {
                m_MasterPoint = value;
            }
        }

        // 툴 이름
        public string TOOL_NAME
        {
            get
            {
                return m_ToolName;
            }
            set
            {
                m_ToolName = value;
            }
        }

        // 검사결과 화면 출력 여부 결정
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

        // 패턴 경로
        public string PATTERN_PATH
        {
            get
            {
                return m_PatternPath;
            }
            set
            {
                m_PatternPath = value;
            }
        }
        // 패턴 점수
        public double Score
        {
            get
            {
                return m_score;
            }
        }
        public double CENTER_X
        {
            get
            {
                return m_Pattern.TranslationX;
                //return m_Pattern.CenterX;
            }
        }

        public double CENTER_Y
        {
            get
            {
                return m_Pattern.TranslationY;
                //return m_Pattern.CenterY;
            }
        }
        public Point REGION_CENTER
        {
            get
            {
                //return ((CogRectangleAffine)m_Pattern.Region).CenterY;
                return new Point((int)((CogRectangleAffine)m_Pattern.Region).CornerOppositeX, (int)((CogRectangleAffine)m_Pattern.Region).CenterY);
                //return m_Pattern.CenterY;
            }
        }

        public void DrawRange()
        {
            m_Pattern.DisplaySearchArea(m_Display, false);
        }

        public string GetResult()
        {
            return m_score.ToString();
        }

        #region 생성자 그룹

        public Pattern()
        {
            if (m_IsCreateControl)
                InitializeComponent();

        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="isCreateControl">컨트롤 사용 여부</param>
        public Pattern(bool isCreateControl)
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

        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="tool">툴 이름</param>
        /// <param name="pt">패턴 경로</param>
        public Pattern(string tool, string pt)
        {
            InitializeComponent();

            m_ToolName = tool;
            m_PatternPath = pt;

        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="tool">툴 이름</param>
        /// <param name="master">마스터 여부</param>
        public Pattern(string tool, bool isMaster, bool isCreateControl)
        {
            m_IsCreateControl = isCreateControl;

            if (m_IsCreateControl)
                InitializeComponent();

            m_ToolName = tool;

            _isMaster = isMaster;
        }

        #endregion


        private void Pattern_Load(object sender, EventArgs e)
        {

            
        }

        private void Load_Pattern()
        {
            m_Pattern = null;
            m_Pattern = new PMAlgin();

            m_Pattern.LoadTool(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Tool\\" + m_ToolName + ".vpp");

            // 마스터 사용여부
            //if (_useMaster)
            //{
            //    CogRectangleAffine tmp_region = (CogRectangleAffine)m_Pattern.Region;
            //    tmp_region.CenterX = ((CogRectangleAffine)m_Pattern.Region).CenterX ;
            //    tmp_region.CenterY = ((CogRectangleAffine)m_Pattern.Region).CenterY ;
            //    tmp_region.SideXLength = ((CogRectangleAffine)m_Pattern.Region).SideXLength;
            //    tmp_region.SideYLength = ((CogRectangleAffine)m_Pattern.Region).SideYLength;

            //    m_Pattern.Region = tmp_region;
            //}

            if (m_PatternPath == "")
                m_PatternPath = m_ToolName;
            if (m_IsCreateControl == true)
            {
                //Stopwatch sw = new Stopwatch();
                //sw.Start();
                //CogStopwatch a = new CogStopwatch();
                //a.Start();
                m_Pattern.Load_Pattern(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Pattern\\" + m_PatternPath, cb_Model);
                //sw.Stop();

                //a.Stop();
                //MessageBox.Show(sw.ElapsedMilliseconds.ToString());
            }
            else
            {
                m_Pattern.Load_Pattern(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Pattern\\" + m_PatternPath, true);
            }
        }


        #region ToolBase 멤버
        public void Release()
        {
            m_Pattern = null;
            m_ColorEx = null;
            m_Display = null;
        }

        public void SetViewLabel(bool flag)
        {
            VIEWLABEL = flag;
        }

        public int SetImage(Cognex.VisionPro.Display.CogDisplay display)
        {
            m_Display = display;
            m_MovingPointCheck = false;

            try
            {
                //이미지 변환
                //m_Pattern.Image = m_Display.Image.GetType().Name == "CogImage8Grey" ? m_Display.Image : (new ImageFile()).Get_Plan((CogImage24PlanarColor)m_Display.Image, "Intensity");
                m_LoadedImage = (ICogImage)display.Image;
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        public int SetInfo(string Item, string Part, string toolName)
        {
            m_MovingPointCheck = false;

            m_Part = Part;
            m_Item = Item;
            m_ToolName = toolName;

            // 설정파일
            m_PartConfig = new IniFile(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Item + ".ini");

            // 영상변환 정보 로드
            m_ConvertMethod = m_PartConfig.GetString(m_Part + " " + m_ToolName, "ImageConvert", "Intensity");

            m_MasterX = m_PartConfig.GetInt32(m_ToolName + " Master", "MasterX", 0);
            m_MasterY = m_PartConfig.GetInt32(m_ToolName + " Master", "MasterY", 0);

            // 마스터 사용여부
            _useMaster = m_PartConfig.GetString(m_ToolName + " " + m_ToolName, "UseMaster", "False") == "True";
            _isFindNG = m_PartConfig.GetString(m_ToolName + " " + m_ToolName, "Find", "OK") == "OK" ? false : true;

            // 툴 로드
            Load_Pattern();

            if (m_IsCreateControl)
            {

                num_Score.Value = (decimal)(m_Pattern.OKScore * 100);
                num_Angle.Value = (decimal)m_Pattern.Angle;

                num_ScaleX.Value = (decimal)m_Pattern.XScale;
                num_ScaleY.Value = (decimal)m_Pattern.YScale;

                num_Elasticity.Value = (decimal)m_Pattern.Elasticity;

                lb_CenterX.Text = m_MasterX.ToString();
                lb_CenterY.Text = m_MasterY.ToString();

                cogDisplay_Model.Image = null;
                cb_Model.Text = "";

                 

                chk_UseMaster.Checked = _useMaster;
                cb_ConvertMethod.Text = m_ConvertMethod;
                ckd_Find.Checked = _isFindNG;

                string tmp_ChainArea = m_PartConfig.GetString(m_ToolName + " " + m_ToolName, "InspectionArea", "Rectangle");

                switch (tmp_ChainArea)
                {
                    case "Rectangle":
                        rb_RectAngle.Checked = true;
                        break;
                    case "Circle":
                        rb_Circle.Checked = true;
                        break;
                    case "Ring":
                        rb_Ring.Checked = true;
                        break;
                }

            }

            if (m_IsCreateControl)
            {
                num_VerticalPixelperMM.Value = (decimal)m_PartConfig.GetDouble("Calibration", m_ToolName + " VerticalPixelPerMM", 0.01);
                num_HorizentalPixelPerMM.Value = (decimal)m_PartConfig.GetDouble("Calibration", m_ToolName + " HorizentalPixelPerMM", 0.01);

                if (m_PartConfig.GetString("Calibration", m_ToolName + " ReverseVertical", cb_reverseVertical.Checked.ToString()).ToUpper() == "TRUE")
                {
                    cb_reverseVertical.Checked = true;
                }
                else
                {
                    cb_reverseVertical.Checked = false;
                }
                if (m_PartConfig.GetString("Calibration", m_ToolName + " ReverseHorizental", cb_reverseHorizental.Checked.ToString()) == "TRUE")
                {
                    cb_reverseHorizental.Checked = true;
                }
                else
                {
                    cb_reverseHorizental.Checked = false;
                }
                if (m_PartConfig.GetString("Calibration", m_ToolName + " ReverseXY", cb_reverseVertical.Checked.ToString()).ToUpper() == "TRUE")
                {
                    cb_reverseXY.Checked = true;
                }
                else
                {
                    cb_reverseXY.Checked = false;
                }
            }

            return 0;
        }

        public int Run()
        {
            Console.WriteLine("Pattern.Run");

            //m_Display.InteractiveGraphics.Clear();
            //m_Display.StaticGraphics.Clear();

            int ret = 0;

            //ImageFile imgFile = new ImageFile();

            if (m_Pattern == null)
            {
                MessageBox.Show("패턴검사 툴이 로드 되지 않았습니다.");
                return 1;
            }

            // 패턴 로드
            //Load_Pattern();

            ConvertImage();
            m_Pattern.Image = m_ConvertedImage;

            // 마스터 사용여부
            if (_useMaster)
            {
                if (!m_MovingPointCheck)
                {
                    CogRectangleAffine tmp_region = (CogRectangleAffine)m_Pattern.Region;
                    tmp_region.CenterX = ((CogRectangleAffine)m_Pattern.Region).CenterX + m_PartConfig.GetInt32(m_ToolName + " Master", "MovingX", 0);
                    tmp_region.CenterY = ((CogRectangleAffine)m_Pattern.Region).CenterY + m_PartConfig.GetInt32(m_ToolName + " Master", "MovingY", 0);
                    tmp_region.SideXLength = ((CogRectangleAffine)m_Pattern.Region).SideXLength;
                    tmp_region.SideYLength = ((CogRectangleAffine)m_Pattern.Region).SideYLength;

                    m_Pattern.Region = tmp_region;

                    m_MovingPointCheck = true;
                }
            }

            int X = m_PartConfig.GetInt32(m_ToolName + " Master", "MovingX", 0);
            int Y = m_PartConfig.GetInt32(m_ToolName + " Master", "MovingY", 0);

            m_score = m_Pattern.FindPattern(m_Display, _isDisplay);

            if (m_score >= m_Pattern.OKScore)
            {
                if (_isDisplay)
                {
                    m_Pattern.DrawLabel(m_ToolName + ": OK(" + string.Format("{0:0.00}", m_score) + "), Model: " + m_Pattern.FindPatternIndex, m_Display, m_Pattern.TranslationX + 15, m_Pattern.TranslationY + 5, 10, CogColorConstants.Green, CogColorConstants.Black);
                }

                if (m_IsCreateControl)
                {
                    lb_FindX.Text = Math.Round(m_Pattern.TranslationX).ToString();
                    lb_FindY.Text = Math.Round(m_Pattern.TranslationY).ToString();
                }
                {
                    if (_isMaster)
                        m_PartConfig.WriteValue(m_ToolName + " Master", "MovingX", m_MasterX - m_Pattern.TranslationX);
                    m_PartConfig.WriteValue(m_ToolName + " Master", "MovingY", m_MasterY - m_Pattern.TranslationY);
                }

                if (_isFindNG == true)
                    ret = 1;
            }
            else
            {
                if (_isDisplay)
                {
                    m_Pattern.DrawLabel(m_ToolName + ": NG(" + string.Format("{0:0.00}", m_score) + ")", m_Display, m_Pattern.CenterX + 15, m_Pattern.CenterY + 5, 10, CogColorConstants.Red, CogColorConstants.Black);
                }

                if (m_IsCreateControl)
                {
                    lb_FindX.Text = "0";
                    lb_FindY.Text = "0";
                }
                if (_isFindNG == false)
                {
                    if (!_isMaster)
                        ret = 1;
                }
            }

            return ret;
        }

        /// 시스템 로그 남기는 함수
        /// </summary>
        /// <param name="log">남길 내용</param>
        private void Write_SystemLog(string log)
        {
            FileStream wstream;
            StreamWriter writer;

            Console.WriteLine(DateTime.Now.ToLongTimeString() + "\t" + log);

            try
            {

                if (!Directory.Exists(Application.StartupPath + @"\System Log\"))
                {
                    Directory.CreateDirectory(Application.StartupPath + @"\System Log\");
                }

                if (File.Exists(Application.StartupPath + @"\System Log\" + String.Format("{0:0000}년{1:00}월{2:00}일.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)) == false)
                {
                    wstream = File.Create(Application.StartupPath + @"\System Log\" + String.Format("{0:0000}년{1:00}월{2:00}일.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                    writer = new StreamWriter(wstream);
                }
                else
                {
                    wstream = new FileStream(Application.StartupPath + @"\System Log\" + String.Format("{0:0000}년{1:00}월{2:00}일.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), FileMode.Append, FileAccess.Write);
                    writer = new StreamWriter(wstream);
                }
                writer.WriteLine(DateTime.Now.ToLongTimeString() + "\t" + log);

                writer.Close();
                wstream.Close();
            }
            catch (Exception ex)
            {
                Write_SystemLog(ex.Message);
            }
            finally
            {
                writer = null;
                wstream = null;
            }
        }

        public int Confirm()
        {
            switch (m_Current)
            {
                case STATE.RANGE:
                    if (!System.IO.Directory.Exists(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Tool"))
                        System.IO.Directory.CreateDirectory(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Tool");

                    if (MessageBox.Show("검사영역을 수정하시겠습니까?", "Inspection Area Change", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return 0;
                    }

                    Write_SystemLog("패턴 검사 영역 변경.");

                    m_MovingPointCheck = false;

                    m_Pattern.OKScore = (double)num_Score.Value / 100.0;

                    m_Pattern.Angle = (int)num_Angle.Value;

                    m_Pattern.XScale = (double)num_ScaleX.Value;
                    m_Pattern.YScale = (double)num_ScaleY.Value;

                    m_Pattern.Elasticity = (double)num_Elasticity.Value;

                    m_Pattern.ContrastThresh = 10;
                    //m_Pattern.Zoom = 1;

                    m_PartConfig.WriteValue(m_ToolName + " " + m_ToolName, "InspectionArea", m_Pattern.SearchRegionShape.ToString());

                    // 마스터 사용여부
                    m_PartConfig.WriteValue(m_ToolName + " " + m_ToolName, "UseMaster", _useMaster.ToString());

                    if (ckd_Find.Checked == true)
                    {
                        m_PartConfig.WriteValue(m_ToolName + " " + m_ToolName, "Find", "NG");
                    }
                    else
                    {
                        m_PartConfig.WriteValue(m_ToolName + " " + m_ToolName, "Find", "OK");
                    }

                    //if (ckd_PointCheck.Checked == true)
                    //{
                    //    m_PartConfig.WriteValue(m_ToolName + " " + m_ToolName, "PointCheck", "True");
                    //}
                    //else
                    //{
                    //    m_PartConfig.WriteValue(m_ToolName + " " + m_ToolName, "PointCheck", "False");
                    //}

                    m_Pattern.SaveTool(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Tool\\" + m_ToolName + ".vpp");
                    break;

                case STATE.MODEL:
                    if (MessageBox.Show("패턴을 추가 하시겠습니까 ?", "Add Pattern", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return 0;
                    }

                    Write_SystemLog("패턴 추가.");

                    if (!System.IO.Directory.Exists(Application.StartupPath + "\\PartInfo\\" + m_Item))
                        System.IO.Directory.CreateDirectory(Application.StartupPath + "\\PartInfo\\" + m_Item);

                    if (!System.IO.Directory.Exists(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Pattern"))
                        System.IO.Directory.CreateDirectory(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Pattern");

                    if (!System.IO.Directory.Exists(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Pattern\\" + m_PatternPath))
                        System.IO.Directory.CreateDirectory(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Pattern\\" + m_PatternPath);

                    try
                    {
                        string[] models = System.IO.Directory.GetFiles(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Pattern\\" + m_PatternPath, "*.hsr");

                        m_Pattern.Set_Pattern(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Pattern\\" + m_PatternPath + "\\" + string.Format("{0:000}.hsr", models.Length), chk_useTrainPoint.Checked);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;

                case STATE.COLOR:



                    if (!System.IO.Directory.Exists(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Tool"))
                        System.IO.Directory.CreateDirectory(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Tool");

                    ////검사영역
                    CogRectangleAffine tmp_region1 = new CogRectangleAffine();

                    tmp_region1.CenterX = m_Display.Image.Width / 2;
                    tmp_region1.CenterY = m_Display.Image.Height / 2;
                    tmp_region1.SideXLength = m_Display.Image.Width;
                    tmp_region1.SideYLength = m_Display.Image.Height;

                    m_PartConfig.WriteValue(m_Part, "InspectionArea", m_Pattern.SearchRegionShape.ToString());

                    try
                    {
                        m_ColorEx.Region = tmp_region1;
                        //m_ColorEx.Region = m_Pattern.Region;
                        m_ColorEx.Set_Pattern(new ColorExtractor.ColorParam());
                        m_ColorEx.SaveTool(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Tool\\ColorExToolPattern.vpp");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        m_ColorEx.Load_Pattern(cb_ColorModel);

                    }

                    break;



            }

            m_Display.InteractiveGraphics.Clear();
            m_Display.StaticGraphics.Clear();

            m_Current = STATE.NONE;

            this.SetInfo(m_Item, m_Part, m_ToolName);
            return 0;
        }

        public int Cancle()
        {
            m_Display.InteractiveGraphics.Clear();
            m_Display.StaticGraphics.Clear();

            return 0;
        }

        #endregion





        private void btn_SetRange_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Setting : Pattern Area Set");

            ToolBase.RegionShape setShape = ToolBase.RegionShape.Rectangle;

            if (rb_RectAngle.Checked)
                setShape = ToolBase.RegionShape.Rectangle;

            else if (rb_Ring.Checked)
                setShape = ToolBase.RegionShape.Ring;

            else if (rb_Circle.Checked)
                setShape = ToolBase.RegionShape.Circle;


            m_Pattern.SearchRegionShape = setShape;


            m_Current = STATE.RANGE;

            try
            {
                if (m_Display.Image != null)
                {
                    ConvertImage();
                    m_Pattern.Image = m_ConvertedImage;

                    m_Display.InteractiveGraphics.Clear();
                    m_Display.StaticGraphics.Clear();

                    m_Pattern.DisplaySearchArea(m_Display, true, setShape);

                }
            }
            catch
            {

            }
        }

        private void btn_AddModel_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Setting : Pattern Model Add");

            m_Current = STATE.MODEL;

            m_Display.InteractiveGraphics.Clear();
            m_Display.StaticGraphics.Clear();



            try
            {
                if (m_Display.Image != null)
                {
                    ConvertImage();
                    m_Pattern.Image = m_ConvertedImage;

                    //m_Pattern.Display_PatternArea(m_Display, false);
                    m_Pattern.Display_PatternArea(m_Display, chk_useTrainPoint.Checked);
                }
            }
            catch
            {
            }
        }

        private void btn_DelModel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("패턴을 삭제하시겠습니까 ?", "Delete", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            // 패턴 파일 삭제
            string tmpPath = Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Pattern\\" + m_PatternPath + "\\";
            System.IO.File.Delete(tmpPath + cb_Model.Text + ".hsr");

            //콤보박스 텍스트 지우고...
            cb_Model.Text = "";

            // 경로 지정 후...
            string[] files = System.IO.Directory.GetFiles(tmpPath);

            //for문 돌려서 i랑 파일이름이랑 같으면 계속 그렇지 않으면  
            for (int i = 0; i < files.Length; i++)
            {
                string name = files[i];

                if (int.Parse(name.Substring(name.Length - 7, 3)) == i)
                {
                    continue;
                }
                else
                {
                    //먼저 name을 i이름으로 파일복사하고 
                    System.IO.File.Copy(name, tmpPath + string.Format("{0:000}.hsr", i));
                    //기존name을 삭제한다.
                    System.IO.File.Delete(name);
                }
            }

            //다 끝나면 Master의 패턴을 다시 로드한다.
            m_Pattern.Load_Pattern(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Pattern\\" + m_PatternPath, cb_Model);

            //모델화면 지우기
            cogDisplay_Model.Image = null;
        }

        private void cb_Model_SelectedIndexChanged(object sender, EventArgs e)
        {
            cogDisplay_Model.StaticGraphics.Clear();
            cogDisplay_Model.InteractiveGraphics.Clear();
            cogDisplay_Model.Image = null;

            m_Pattern.Show_Pattern(cb_Model.SelectedIndex, cogDisplay_Model);
        }

        private void btn_Run_Click(object sender, EventArgs e)
        {
            this.Run();
        }



        #region IDisposable 멤버

        void IDisposable.Dispose()
        {
            //throw new NotImplementedException();
            this.Release();

            int i = this.Controls.Count;
            while (i > 0)
            {
                this.Controls[--i].Dispose();
            }
        }

        #endregion


        #region 영상 흑백변환

        /// <summary>
        /// 영상변환 미리보기
        /// </summary>
        private bool _isMono = false;
        private void label1_Click(object sender, EventArgs e)
        {
            if (!_isMono)
            {
                ConvertImage();
                m_Display.Image = m_ConvertedImage;
                _isMono = !_isMono;
            }
            else
            {
                m_Display.Image = m_LoadedImage;
                _isMono = !_isMono;
            }
        }

        /// <summary>
        /// 이미지 변환
        /// </summary>
        private void ConvertImage()
        {
            string ConvertMethod = m_PartConfig.GetString(m_Part + " " + m_ToolName, "ImageConvert", "Intensity");

            if (ConvertMethod == "ColorExtract")
            {
                m_ColorEx.LoadTool(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Tool\\ColorExToolPattern.vpp");
                m_ColorEx.Image = m_LoadedImage;

                m_ColorEx.RunExtractor();

                m_ConvertedImage = (CogImage8Grey)m_ColorEx.ResultImage;

            }
            else if (ConvertMethod == "WeightedRGB")
            {
                ImageFile img = new ImageFile();

                img.Weight_R = m_PartConfig.GetDouble(m_Part + " WeightedRGB", "R", 0.0);
                img.Weight_G = m_PartConfig.GetDouble(m_Part + " WeightedRGB", "G", 0.0);
                img.Weight_B = m_PartConfig.GetDouble(m_Part + " WeightedRGB", "B", 0.0);

                if (m_LoadedImage.GetType() == typeof(CogImage8Grey))
                {
                    m_ConvertedImage = (CogImage8Grey)m_LoadedImage;
                }
                else
                {
                    m_ConvertedImage = img.Get_Plan((CogImage24PlanarColor)m_LoadedImage, ConvertMethod);
                }

            }
            else
            {
                ImageFile img = new ImageFile();

                if (m_LoadedImage.GetType() == typeof(CogImage8Grey))
                {
                    m_ConvertedImage = (CogImage8Grey)m_LoadedImage;
                }
                else
                {
                    m_ConvertedImage = img.Get_Plan((CogImage24PlanarColor)m_LoadedImage, ConvertMethod);
                }
            }
        }

        /// <summary>
        /// 영상변환 방식 수정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ModifyConvertMethod_Click(object sender, EventArgs e)
        {
            if (btn_ModifyConvertMethod.Text == "수정")
            {
                btn_ModifyConvertMethod.Text = "취소";

                cb_ConvertMethod.Enabled = true;
            }
            else
            {
                btn_ModifyConvertMethod.Text = "수정";

                cb_ConvertMethod.Text = m_PartConfig.GetString(m_Part + " " + m_ToolName, "ImageConvert", "Intensity");
                cb_ConvertMethod.Enabled = false;
            }
        }

        /// <summary>
        /// 영상변환 방식 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveConvertMethod_Click(object sender, EventArgs e)
        {
            btn_ModifyConvertMethod.Text = "수정";

            m_PartConfig.WriteValue(m_Part + " " + m_ToolName, "ImageConvert", cb_ConvertMethod.Text);
            cb_ConvertMethod.Enabled = false;
        }

        /// <summary>
        /// 영상변환 세부 설정(색상 추출, WeightedRGB)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ConvertImageSet_Click(object sender, EventArgs e)
        {
            if (cb_ConvertMethod.Text == "ColorExtract")
            {
                panel_Color.Visible = true;
                panel_Color.Location = new Point(5, 10);

                m_ColorEx.LoadTool(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Tool\\ColorExToolPattern.vpp");
                m_ColorEx.Load_Pattern(cb_ColorModel);
                m_ColorEx.Image = m_LoadedImage;

            }
            else if (cb_ConvertMethod.Text == "WeightedRGB")
            {
                panel_WRGB.Show();
                panel_WRGB.Location = new Point(278, 197);

                num_Weight_R.Value = (decimal)m_PartConfig.GetDouble(m_Part + " WeightedRGB", "R", 0.0);
                num_Weight_G.Value = (decimal)m_PartConfig.GetDouble(m_Part + " WeightedRGB", "G", 0.0);
                num_Weight_B.Value = (decimal)m_PartConfig.GetDouble(m_Part + " WeightedRGB", "B", 0.0);
            }
        }

        #region WeightedRGB 변환설정
        private void btn_SaveRGB_Click(object sender, EventArgs e)
        {
            m_PartConfig.WriteValue(m_Part + " WeightedRGB", "R", num_Weight_R.Value.ToString());
            m_PartConfig.WriteValue(m_Part + " WeightedRGB", "G", num_Weight_G.Value.ToString());
            m_PartConfig.WriteValue(m_Part + " WeightedRGB", "B", num_Weight_B.Value.ToString());
        }

        private void btn_CancleRGB_Click(object sender, EventArgs e)
        {
            panel_WRGB.Hide();
        }
        #endregion


        #region 색상 추출 변환 설정
        private void btn_ColorPatternAdd_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Setting : Color Model Add");

            m_Current = STATE.COLOR;

            m_Display.InteractiveGraphics.Clear();
            m_Display.StaticGraphics.Clear();

            try
            {
                if (m_Display.Image != null)
                {

                    m_ColorEx.Display_PatternArea(m_Display);
                }
            }
            catch
            {
            }

        }

        private void btn_ColorPatternDel_Click(object sender, EventArgs e)
        {
            m_ColorEx.DelColorModel(cb_ColorModel.SelectedIndex);

            m_ColorEx.Load_Pattern(cb_ColorModel);
            m_ColorEx.SaveTool(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Tool\\ColorExTool.vpp");
        }

        private void btn_Train_Click(object sender, EventArgs e)
        {
            ColorExtractor.ColorParam param = new ColorExtractor.ColorParam();
            // 파라미터 설정
            param.Dilation = (int)num_Dilation.Value;
            param.Softness = (int)num_Softness.Value;

            param.MinPixelCnt = (int)num_MinPixelCnt.Value;

            param.MatteLineEnabled = chk_MatteLineLimit.Checked;
            param.MatteLineHigh = (double)num_MatteLineHigh.Value;
            param.MatteLineLow = (double)num_MatteLineLow.Value;

            param.HighLightLineEnabled = chk_HighLightLimit.Checked;
            param.HightLightLineLimitValue = (double)num_HighLightLimit.Value;

            // 트레인
            m_ColorEx.TrainColor(cb_ColorModel.SelectedIndex, param);
        }

        private void btn_Result_Click(object sender, EventArgs e)
        {
            m_ColorEx.LoadTool(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Tool\\ColorExToolPattern.vpp");
            m_ColorEx.RunExtractor();

            m_Display.Image = m_ColorEx.ResultImage;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            m_ColorEx.SaveTool(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Tool\\ColorExToolPattern.vpp");
        }

        private void btn_Cancle_Click(object sender, EventArgs e)
        {
            panel_Color.Visible = false;

        }

        private void cb_ColorModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ColorExtractor.ColorParam param = m_ColorEx.ShowColorModel(cb_ColorModel.SelectedIndex, cogDisplay_ColorModel);

            // 파라미터 설정
            num_Dilation.Value = (decimal)param.Dilation;
            num_Softness.Value = (decimal)param.Softness;

            num_MinPixelCnt.Value = (decimal)param.MinPixelCnt;

            chk_MatteLineLimit.Checked = param.MatteLineEnabled;
            num_MatteLineHigh.Value = (decimal)param.MatteLineHigh;
            num_MatteLineLow.Value = (decimal)param.MatteLineLow;

            chk_HighLightLimit.Checked = param.HighLightLineEnabled;
            num_HighLightLimit.Value = (decimal)param.HightLightLineLimitValue;
        }
        #endregion

        private void btn_Run_Click_1(object sender, EventArgs e)
        {
            this.Run();
        }




        #endregion

        private void chk_UseMaster_CheckedChanged(object sender, EventArgs e)
        {
            _useMaster = chk_UseMaster.Checked;
        }

        private void btn_Masking_Click(object sender, EventArgs e)
        {
            if (m_Display == null)
            {

                MessageBox.Show("Image Not Find");
                return;
            }


            if (m_ConvertedImage == null)
            {

                ConvertImage();

            }

            Form_Masking fm = new Form_Masking(m_ConvertedImage);

            if (fm.ShowDialog() == DialogResult.OK)
            {
                m_Pattern.MaskImage = (CogImage8Grey)fm.MASKIMAGE;
            }
        }

        private void btn_MasterSave_Click(object sender, EventArgs e)
        {
            m_PartConfig.WriteValue(m_ToolName + " Master", "MasterX", lb_FindX.Text);
            m_PartConfig.WriteValue(m_ToolName + " Master", "MasterY", lb_FindY.Text);

            lb_CenterX.Text = lb_FindX.Text;
            lb_CenterY.Text = lb_FindY.Text;

            m_MasterX = Convert.ToInt32(lb_CenterX.Text);
            m_MasterY = Convert.ToInt32(lb_CenterY.Text);
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (m_Display != null && m_Display.Image != null)
            {
                int index = m_Display.InteractiveGraphics.FindItem("VerticalPos", CogDisplayZOrderConstants.Front);

                double mmPerPixel = (double)(sender as NumericUpDown).Value;      

                double size = 25;//mm
                double pixel = size * mmPerPixel;

                

                if (index >= 0)
                {
                    CogRectangle rec = (CogRectangle)m_Display.InteractiveGraphics[index];
                    double x;
                    double y;
                    double temp;
                    double range;

                    rec.GetXYWidthHeight(out x, out y, out range, out temp);
                    rec.SetXYWidthHeight(x, y, range, pixel);

                }
                else
                {

                    m_Display.InteractiveGraphics.Clear();


                    CogRectangle rec = new CogRectangle();
                    rec.Interactive = true;
                    rec.GraphicDOFEnable = CogRectangleDOFConstants.Position;

                    rec.SetXYWidthHeight(100, 100, 10, pixel);

                    m_Display.InteractiveGraphics.Add(rec, "VerticalPos", false);
                }
            }
        }

        private void btn_SaveCalib_Click(object sender, EventArgs e)
        {
            m_PartConfig.WriteValue("Calibration", m_ToolName + " VerticalPixelPerMM", (double)num_VerticalPixelperMM.Value);
            m_PartConfig.WriteValue("Calibration", m_ToolName + " HorizentalPixelPerMM", (double)num_HorizentalPixelPerMM.Value);

            m_PartConfig.WriteValue("Calibration", m_ToolName + " ReverseVertical", cb_reverseVertical.Checked.ToString());
            m_PartConfig.WriteValue("Calibration", m_ToolName + " ReverseHorizental", cb_reverseHorizental.Checked.ToString());
            m_PartConfig.WriteValue("Calibration", m_ToolName + " ReverseXY", cb_reverseXY.Checked.ToString());

            MessageBox.Show("저장되었습니다.");
        }

        private void num_HorizentalPixelPerMM_ValueChanged(object sender, EventArgs e)
        {
            if (m_Display != null && m_Display.Image != null)
            {
                int index = m_Display.InteractiveGraphics.FindItem("HorizentalPos", CogDisplayZOrderConstants.Front);

                double mmPerPixel = (double)(sender as NumericUpDown).Value;

                double size = 25;
                double pixel = size * mmPerPixel;



                if (index >= 0)
                {
                    CogRectangle rec = (CogRectangle)m_Display.InteractiveGraphics[index];
                    double x;
                    double y;
                    double temp;
                    double range;

                    rec.GetXYWidthHeight(out x, out y,out temp, out range);
                    rec.SetXYWidthHeight(x, y, pixel, range);

                }
                else
                {

                    m_Display.InteractiveGraphics.Clear();


                    CogRectangle rec = new CogRectangle();
                    rec.Interactive = true;
                    rec.GraphicDOFEnable = CogRectangleDOFConstants.Position;

                    rec.SetXYWidthHeight(100, 100, pixel, 10);

                    m_Display.InteractiveGraphics.Add(rec, "HorizentalPos", false);
                }
            }
        }

        private void cb_reverseXY_CheckedChanged(object sender, EventArgs e)
        {

        }

        public PointF GetMoveValue()
        {
            int masterX = m_PartConfig.GetInt32(m_ToolName + " Master", "MasterX", 0);
            int masterY = m_PartConfig.GetInt32(m_ToolName + " Master", "MasterY", 0);

            PointF moveValue = new PointF((float)(m_Pattern.TranslationX - masterX), (float)(m_Pattern.TranslationY - masterY));

            //좌표 역전

            //세로
            string isVerticalReverse = m_PartConfig.GetString("Calibration", m_ToolName + " ReverseVertical", "False");

            if (isVerticalReverse.ToUpper() == "TRUE")
            {
                moveValue.Y *= -1;
            }

            //가로
            if (m_PartConfig.GetString("Calibration", m_ToolName + " ReverseHorizental", "False").ToUpper() == "TRUE")
            {
                moveValue.X *= -1;
            }

            //X,Y
            if (m_PartConfig.GetString("Calibration", m_ToolName + " ReverseXY", "False").ToUpper() == "TRUE")
            {
                float temp = moveValue.X;
                moveValue.X = moveValue.Y;
                moveValue.Y = temp;
            }
             
            double verticalPixelPerMM = m_PartConfig.GetDouble("Calibration", m_ToolName + " VerticalPixelPerMM", -1);
            double horizentalPixelPerMM = m_PartConfig.GetDouble("Calibration", m_ToolName + " HorizentalPixelPerMM", -1);

            
            if(verticalPixelPerMM != -1 && horizentalPixelPerMM != -1)
            {
                moveValue.Y /= (float)verticalPixelPerMM;
                moveValue.X /= (float)horizentalPixelPerMM;
            }
       
            return moveValue;
        }   
    }
}
