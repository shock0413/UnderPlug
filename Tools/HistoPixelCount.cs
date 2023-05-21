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
using System.IO;
using System.Drawing.Imaging;

namespace Tools
{
    public partial class HistoPixelCount : UserControl, ToolInterface, IDisposable
    {

        private Histogram m_Histo = new Histogram();

        private string m_Item, m_Part;
        private IniFile m_PartConfig;
        private string m_HistoValue;

        private enum STATE { RANGE, NONE }
        private STATE m_Current;

        private CogDisplay m_Display;

        private int m_MinValue = 0;
        private int m_MaxValue = 0;

        private int m_MinPixelValue = 0;
        private int m_MaxPixelValue = 0;

        // 툴 정보
        private string m_ToolName = "HistoTool";

        // 검사결과 화면 표시여부
        private bool _isDisplay = true;

        // 콘트롤 사용여부
        private bool m_IsCreateControl = false;


        // 마스터 포인트 중심점

        private bool _useMaster = false;

        private int m_Moving_X = 0;
        private int m_Moving_Y = 0;

        private bool m_MovingPointCheck = false;

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

        #region 생성자 그룹
        /// <summary>
        /// 생성자
        /// </summary>
        public HistoPixelCount()
        {
            if (m_IsCreateControl)
                InitializeComponent();


        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="isCreateControl">컨트롤 사용여부</param>
        public HistoPixelCount(bool isCreateControl)
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
        /// <param name="isCreateControl">컨트롤 사용여부</param>
        public HistoPixelCount(string tool, bool isCreateControl)
        {
            m_IsCreateControl = isCreateControl;

            if (m_IsCreateControl)
                InitializeComponent();


            m_ToolName = tool;

        }
        #endregion

        #region ToolBase 멤버
        public void SetViewLabel(bool flag)
        {
            VIEWLABEL = flag;
        }

        public void Release()
        {
            m_Histo = null;
            m_Display = null;
        }

        public string GetResult()
        {
            return m_HistoValue.ToString();
        }

        public int SetImage(Cognex.VisionPro.Display.CogDisplay display)
        {
            m_Display = display;
            m_MovingPointCheck = false;

            try
            {
                if (m_IsCreateControl)
                {

                    string HIstoArea = m_PartConfig.GetString(m_Part + " " + m_ToolName, "HistoShape", "Rectangle");

                    switch (HIstoArea)
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

                using (Bitmap bmp = m_Display.Image.ToBitmap())
                {
                    ImageConverter converter = new ImageConverter();
                    byte[] bmpStream = (byte[])converter.ConvertTo(bmp, typeof(byte[]));

                    //bmp 인경우
                    if (((Image)bmp).RawFormat.Equals(ImageFormat.MemoryBmp))
                    {
                        MemoryStream stream = new MemoryStream();
                        bmp.Save(stream, ImageFormat.Jpeg);

                        using(Bitmap jpgBmp = new Bitmap(stream))
                        {
                            m_Display.Image = new CogImage24PlanarColor(jpgBmp);
                        }
                    }

                    bmp.Dispose();
                }

                m_Histo.Image = m_Display.Image.GetType().Name == "CogImage8Grey" ? m_Display.Image : (new ImageFile()).Get_Plan((CogImage24PlanarColor)m_Display.Image, "Intensity");

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

            m_MinValue = m_PartConfig.GetInt32(m_Part + " " + m_ToolName, "HistoMin", 0);
            m_MaxValue = m_PartConfig.GetInt32(m_Part + " " + m_ToolName, "HistoMax", 0);

            m_MinPixelValue = m_PartConfig.GetInt32(m_Part + " " + m_ToolName, "HistoPixelMin", 0);
            m_MaxPixelValue = m_PartConfig.GetInt32(m_Part + " " + m_ToolName, "HistoPixelMax", 0);

            // 마스터 사용여부
            _useMaster = m_PartConfig.GetString(m_Part + " " + m_ToolName, "UseMaster", "False") == "True";

            // 검사영역 로드
            PMAlgin tmpTool = new PMAlgin();
            tmpTool.LoadTool(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Tool\\" + m_ToolName + ".vpp");
            m_Histo.Region = tmpTool.Region;

            // 마스터 사용여부
            

            if (m_IsCreateControl)
            {
                num_HistoMin.Value = (decimal)m_PartConfig.GetInt32(m_Part + " " + m_ToolName, "HistoMin", 0);
                num_HistoMax.Value = (decimal)m_PartConfig.GetInt32(m_Part + " " + m_ToolName, "HistoMax", 0);

                num_histoPixelMin.Value = (decimal)m_PartConfig.GetInt32(m_Part + " " + m_ToolName, "HistoPixelMin", 0);
                num_histoPixelMax.Value = (decimal)m_PartConfig.GetInt32(m_Part + " " + m_ToolName, "HistoPixelMax", 0);

                chk_UseMaster.Checked = _useMaster;
            }

            return 0;
        }

        public int Run()
        {
            Console.WriteLine("Histogram.Run");
            int ret = 1;

            if (_useMaster)
            {
                if (!m_MovingPointCheck)
                {
                    m_Moving_X = (int)m_PartConfig.GetDouble(m_Part + " Master", "MovingX", 0);
                    m_Moving_Y = (int)m_PartConfig.GetDouble(m_Part + " Master", "MovingY", 0);

                    string HIstoArea = m_PartConfig.GetString(m_Part + " " + m_ToolName, "HistoShape", "Rectangle");

                    ICogRegion tmp_region = m_Histo.Region;

                    switch (HIstoArea)
                    {
                        case "Rectangle":
                            CogRectangleAffine rec = new CogRectangleAffine();
                            rec = (CogRectangleAffine)tmp_region;
                            rec.CenterX = ((CogRectangleAffine)m_Histo.Region).CenterX + m_Moving_X;
                            rec.CenterY = ((CogRectangleAffine)m_Histo.Region).CenterY + m_Moving_Y;
                            m_Histo.Region = rec;
                            break;

                        case "Circle":
                            CogCircle rec1 = new CogCircle();
                            rec1 = (CogCircle)tmp_region;
                            rec1.CenterX = ((CogCircle)m_Histo.Region).CenterX + m_Moving_X;
                            rec1.CenterY = ((CogCircle)m_Histo.Region).CenterY + m_Moving_Y;
                            m_Histo.Region = rec1;
                            break;

                        case "Ring":
                            CogCircularAnnulusSection rec2 = new CogCircularAnnulusSection();
                            rec2 = (CogCircularAnnulusSection)tmp_region;
                            rec2.CenterX = ((CogCircularAnnulusSection)tmp_region).CenterX + m_Moving_X;
                            rec2.CenterY = ((CogCircularAnnulusSection)tmp_region).CenterY + m_Moving_Y;
                            m_Histo.Region = rec2;
                            break;
                    }

                    m_MovingPointCheck = true;
                }
            }

            double histo_value = m_Histo.Find_HistoPixelCount(m_Histo.Image, m_Histo.Region, m_MinValue, m_MaxValue);

            if (m_IsCreateControl == true)
                lb_HistoValue.Text = histo_value.ToString();

            if (histo_value >= (double)m_MinPixelValue && (double)m_MaxPixelValue >= histo_value)
                ret = 0;
            else
                ret = 1;

            m_Histo.DisplaySearchArea(m_Display, false);

            if (_isDisplay)
            {
                m_Histo.DrawLabel("HistoPixelCount Value: " + histo_value, m_Display, m_Histo.CenterX, m_Histo.CenterY, 10, ret == 0 ? CogColorConstants.Green : CogColorConstants.Red, CogColorConstants.Black);
                m_HistoValue = histo_value.ToString();
            }
            else
                m_HistoValue = "0";

            //else
            //m_Histo.DrawLabel(ret == 0 ? "OK" : "NG", m_Display, m_Histo.CenterX, m_Histo.CenterY, 10, ret == 0 ? CogColorConstants.Green : CogColorConstants.Red, CogColorConstants.Black);


            return ret;
        }

        public int Confirm()
        {
            switch (m_Current)
            {
                case STATE.RANGE:
                    if (!System.IO.Directory.Exists(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Tool"))
                        System.IO.Directory.CreateDirectory(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Tool");

                    m_MovingPointCheck = false;

                    PMAlgin tmpTool = new PMAlgin();

                    tmpTool.SearchRegionShape = m_Histo.SearchRegionShape;
                    tmpTool.Region = m_Histo.Region;
                    //tmpTool.SearchRegionShape = ToolBase.RegionShape.Rectangle;

                    // 마스터 사용여부
                    m_PartConfig.WriteValue(m_Part + " " + m_ToolName, "UseMaster", _useMaster.ToString());

                    m_PartConfig.WriteValue(m_Part + " " + m_ToolName, "HistoShape", m_Histo.SearchRegionShape.ToString());

                    //if (chk_UseMaster.Checked)
                    //{
                    //    //CogRectangleAffine tmp_region = (CogRectangleAffine)tmpTool.Region;
                    //    //tmp_region.CenterX -= m_MasterCenterX;
                    //    //tmp_region.CenterY -= m_MasterCenterY;

                    //    //tmpTool.Region = tmp_region;
                    //}

                    tmpTool.SaveTool(Application.StartupPath + "\\PartInfo\\" + m_Item + "\\" + m_Part + "\\Tool\\" + m_ToolName + ".vpp");
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
            if (m_Display.Image == null) return;

            ToolBase.RegionShape setShape = ToolBase.RegionShape.Rectangle;

            if (rb_RectAngle.Checked)
                setShape = ToolBase.RegionShape.Rectangle;

            else if (rb_Ring.Checked)
                setShape = ToolBase.RegionShape.Ring;

            else if (rb_Circle.Checked)
                setShape = ToolBase.RegionShape.Circle;


            m_Histo.SearchRegionShape = setShape;

            //에디트 모드지정
            m_Current = STATE.RANGE;

            //검사영역 보이게 하기
            m_Histo.DisplaySearchArea(m_Display, true, setShape);
        }


        private void btn_Modify_Click(object sender, EventArgs e)
        {
            if (btn_Modify.ButtonText == "Change")
            {
                btn_Modify.ButtonText = "Cancel";

                num_HistoMin.Enabled = true;
                num_HistoMax.Enabled = true;

                num_histoPixelMin.Enabled = true;
                num_histoPixelMax.Enabled = true;

                btn_Confirm.Enabled = true;
            }
            else
            {
                btn_Modify.ButtonText = "Change";

                num_HistoMin.Enabled = false;
                num_HistoMax.Enabled = false;

                num_histoPixelMin.Enabled = false;
                num_histoPixelMax.Enabled = false;

                btn_Confirm.Enabled = false;

                num_HistoMin.Value = (decimal)m_PartConfig.GetInt32(m_Part, "HistoMin", 0);
                num_HistoMax.Value = (decimal)m_PartConfig.GetInt32(m_Part, "HistoMax", 0);

                num_histoPixelMin.Value = (decimal)m_PartConfig.GetInt32(m_Part, "HistoPixelMin", 0);
                num_histoPixelMax.Value = (decimal)m_PartConfig.GetInt32(m_Part, "HistoPixelMax", 0);
            }
        }

        private void btn_Confirm_Click(object sender, EventArgs e)
        {
            m_PartConfig.WriteValue(m_Part + " " + m_ToolName, "HistoMin", (int)num_HistoMin.Value);
            m_PartConfig.WriteValue(m_Part + " " + m_ToolName, "HistoMax", (int)num_HistoMax.Value);

            m_PartConfig.WriteValue(m_Part + " " + m_ToolName, "HistoPixelMin", (int)num_histoPixelMin.Value);
            m_PartConfig.WriteValue(m_Part + " " + m_ToolName, "HistoPixelMax", (int)num_histoPixelMax.Value);

            btn_Modify.ButtonText = "Change";

            num_HistoMin.Enabled = false;
            num_HistoMax.Enabled = false;

            num_histoPixelMin.Enabled = false;
            num_histoPixelMax.Enabled = false;

            btn_Confirm.Enabled = false;

            this.SetInfo(m_Item, m_Part, m_ToolName);
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

        private void chk_UseMaster_CheckedChanged(object sender, EventArgs e)
        {
            _useMaster = chk_UseMaster.Checked;
        }

        private void HistoTool_Load(object sender, EventArgs e)
        {
            num_HistoMin.Enabled = false;
            num_HistoMax.Enabled = false;

            num_histoPixelMin.Enabled = false;
            num_histoPixelMax.Enabled = false;
        }


    }
}
