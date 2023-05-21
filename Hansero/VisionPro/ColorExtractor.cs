using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Cognex.VisionPro;
using Cognex.VisionPro.Exceptions;
using Cognex.VisionPro.ColorExtractor;
using Cognex.VisionPro.Display;

namespace Hansero.VisionLib.VisionPro
{
    /// <summary>
    /// Cognex 색상추출 툴 (2012. 12. 30 서대호 작성)
    /// </summary>
    public class ColorExtractor : ToolBase
    {
        private CogColorExtractorTool m_ColorExtractorTool = new CogColorExtractorTool();
        private CogColorExtractorColorGroup m_ColorGroup = new CogColorExtractorColorGroup();

        private ICogRegion m_ColorRegion;

        /// <summary>
        /// 추출 색상 파라미터
        /// </summary>
        public class ColorParam
        {
            public bool HighLightLineEnabled = false;
            public bool MatteLineEnabled = false;

            public double HightLightLineLimitValue = 1.8;
            public double MatteLineHigh = 1.5;
            public double MatteLineLow = 0.5;

            public int Softness = 0;
            public int Dilation = 0;
            public int MinPixelCnt = 10;

        }


        /// <summary>
        /// Input Image
        /// </summary>
        public ICogImage InputImage
        {
            get
            {
                return m_ColorExtractorTool.InputImage;
            }
            set
            {
                if (value.GetType().ToString().Contains("CogImage24PlanarColor"))
                {
                    m_Image = value;
                }
                else
                    throw new Cognex.VisionPro.Exceptions.CogImageBadTypeException();
            }
        }

        /// <summary>
        /// 추출 후 8bit 흑백 이미지
        /// </summary>
        public ICogImage ResultImage
        {
            get
            {
                if (m_ColorExtractorTool.Results == null)
                    return new CogImage8Grey();

                return m_ColorExtractorTool.Results.OverallResult.GreyscaleImage;
            }
        }

        /// <summary>
        /// 추출된 픽셀 수 
        /// </summary>
        public int ResultPixelCount
        {
            get
            {
                if (m_ColorExtractorTool.Results == null)
                    return 0;

                return m_ColorExtractorTool.Results.OverallResult.PixelCount;
            }
        }

        /// <summary>
        /// 툴 로드
        /// </summary>
        /// <param name="loadPath">로드할 툴이 있는 경로</param>
        public void LoadTool(string loadPath)
        {
            try
            {
                //if (System.IO.File.Exists(loadPath))
                m_ColorExtractorTool = (CogColorExtractorTool)Cognex.VisionPro.CogSerializer.LoadObjectFromFile(loadPath);

                m_Region = m_ColorExtractorTool.Region;
            }
            catch
            {
                m_ColorExtractorTool = new CogColorExtractorTool();


                m_Region = new CogRectangleAffine();

                if (m_RegionShape == RegionShape.Circle)
                {
                    m_Region = new CogCircle();
                }
                else if (m_RegionShape == RegionShape.Polygon)
                {
                    m_Region = (ICogRegion)new CogPolygon();
                }

                m_ColorExtractorTool.Region = m_Region;

                SaveTool(loadPath);
            }
        }

        /// <summary>
        /// 툴 저장 
        /// </summary>
        /// <param name="savePath">툴이 저장될 경로</param>
        public void SaveTool(string savePath)
        {
            m_ColorExtractorTool.Region = m_Region;

            string[] path = savePath.Split('\\');
            string tmp_path = path[0];
            for (int i = 1; i < path.Length - 1; i++)
            {
                tmp_path += "\\" + path[i];
                if (!System.IO.Directory.Exists(tmp_path)) System.IO.Directory.CreateDirectory(tmp_path);
            }

            try
            {
                //if (!System.IO.Directory.Exists(savePath)) System.IO.Directory.CreateDirectory(savePath);
                Cognex.VisionPro.CogSerializer.SaveObjectToFile(m_ColorExtractorTool, savePath);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 생성자
        /// </summary>
        public ColorExtractor()
        {

        }

        /// <summary>
        /// 추출 수행
        /// </summary>
        /// <returns>추출된 픽셀 수</returns>
        public int RunExtractor()
        {
            m_ColorExtractorTool.InputImage = (CogImage24PlanarColor)m_Image;
            m_ColorExtractorTool.Region = m_Region;
            m_ColorExtractorTool.Run();

            if (m_ColorExtractorTool.Results == null)
                return 0;

            return m_ColorExtractorTool.Results.OverallResult.PixelCount;
        }

        /// <summary>
        /// 칼라 패턴 추가
        /// </summary>
        /// <param name="param">추가할 패턴의 파라미터</param>
        public void Set_Pattern(ColorParam param)
        {
            // 캡춰된 이미지 설정
            if (m_Image == null)
                throw new Exception("Not Image.");

            CogColorExtractorColor color = new CogColorExtractorColor((CogImage24PlanarColor)m_Image, m_ColorRegion);

            color.Dilation = param.Dilation;
            color.Softness = param.Softness;

            color.MinimumPixelCount = param.MinPixelCnt;

            color.MatteLineEnabled = param.MatteLineEnabled;
            color.MatteLineLimitHigh = param.MatteLineHigh;
            color.MatteLineLimitLow = param.MatteLineLow;

            color.HighlightLineEnabled = param.HighLightLineEnabled;
            color.HighlightLineLimit = param.HightLightLineLimitValue;

            m_ColorExtractorTool.Pattern.ColorModel[0].Add(color);

            m_ColorExtractorTool.Pattern.Train();


            if (m_ColorExtractorTool.Pattern.Trained == true)
            {

            }
            else
            {
                throw new Exception("Patten Setting Error.");
            }
        }

        /// <summary>
        /// 패턴 로드
        /// </summary>
        /// <param name="cb_Name">로드된 패턴이 연결될 콤보박스</param>
        public void Load_Pattern(ComboBox cb_Name)
        {

            //콤보박스 내용 클리어 시키기
            cb_Name.Items.Clear();

            for (int i = 0; i < m_ColorExtractorTool.Pattern.ColorModel[0].Count; i++)
                cb_Name.Items.Add(i.ToString());

        }

        /// <summary>
        /// 칼라 패턴 영역 보이기
        /// </summary>
        /// <param name="Display">보여줄 cogDisplay</param>
        public void Display_PatternArea(Cognex.VisionPro.Display.CogDisplay Display)
        {
            //우선 기존 화면 클리어 하고...
            Display.InteractiveGraphics.Clear();
            Display.StaticGraphics.Clear();

            m_ColorRegion = new CogRectangleAffine();
            ((CogRectangleAffine)m_ColorRegion).CenterX = 320;
            ((CogRectangleAffine)m_ColorRegion).CenterY = 240;
            ((CogRectangleAffine)m_ColorRegion).SideXLength = 50;
            ((CogRectangleAffine)m_ColorRegion).SideYLength = 50;
            ((CogRectangleAffine)m_ColorRegion).Rotation = 0;
            ((CogRectangleAffine)m_ColorRegion).GraphicDOFEnable = Cognex.VisionPro.CogRectangleAffineDOFConstants.All;

            ((CogRectangleAffine)m_ColorRegion).Interactive = true;

            Display.InteractiveGraphics.Add((CogRectangleAffine)m_ColorRegion, "", true);

        }

        /// <summary>
        /// 칼라 패턴 보여주기
        /// </summary>
        /// <param name="i">보여줄 패턴의 인덱스</param>
        /// <param name="display">패턴을 그릴 cogDisplay</param>
        /// <returns>칼라 패턴 파라미터</returns>
        public ColorParam ShowColorModel(int i, CogDisplay display)
        {
            display.Image = m_ColorExtractorTool.Pattern.ColorModel[0][i].Image;

            ColorExtractor.ColorParam param = new ColorExtractor.ColorParam();

            param.Dilation = m_ColorExtractorTool.Pattern.ColorModel[0][i].Dilation;
            param.Softness = m_ColorExtractorTool.Pattern.ColorModel[0][i].Softness;

            param.MinPixelCnt = m_ColorExtractorTool.Pattern.ColorModel[0][i].MinimumPixelCount;

            param.MatteLineEnabled = m_ColorExtractorTool.Pattern.ColorModel[0][i].MatteLineEnabled;
            param.MatteLineHigh = m_ColorExtractorTool.Pattern.ColorModel[0][i].MatteLineLimitHigh;
            param.MatteLineLow = m_ColorExtractorTool.Pattern.ColorModel[0][i].MatteLineLimitLow;

            param.HighLightLineEnabled = m_ColorExtractorTool.Pattern.ColorModel[0][i].HighlightLineEnabled;
            param.HightLightLineLimitValue = m_ColorExtractorTool.Pattern.ColorModel[0][i].HighlightLineLimit;

            return param;
        }

        /// <summary>
        /// 칼라패턴 삭제
        /// </summary>
        /// <param name="i">삭제할 패턴의 인덱스</param>
        public void DelColorModel(int i)
        {
            m_ColorExtractorTool.Pattern.ColorModel[0].RemoveAt(i);

        }

        /// <summary>
        /// 칼라 패턴 트레인
        /// </summary>
        /// <param name="i">트레인할 패턴의 인덱스</param>
        /// <param name="param">트레인 파라미터</param>
        public void TrainColor(int i, ColorParam param)
        {
            try
            {
                m_ColorExtractorTool.Pattern.ColorModel[0][i].Dilation = param.Dilation;
                m_ColorExtractorTool.Pattern.ColorModel[0][i].Softness = param.Softness;

                m_ColorExtractorTool.Pattern.ColorModel[0][i].MinimumPixelCount = param.MinPixelCnt;

                m_ColorExtractorTool.Pattern.ColorModel[0][i].MatteLineEnabled = param.MatteLineEnabled;
                m_ColorExtractorTool.Pattern.ColorModel[0][i].MatteLineLimitHigh = param.MatteLineHigh;
                m_ColorExtractorTool.Pattern.ColorModel[0][i].MatteLineLimitLow = param.MatteLineLow;

                m_ColorExtractorTool.Pattern.ColorModel[0][i].HighlightLineEnabled = param.HighLightLineEnabled;
                m_ColorExtractorTool.Pattern.ColorModel[0][i].HighlightLineLimit = param.HightLightLineLimitValue;

                m_ColorExtractorTool.Pattern.Train();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
