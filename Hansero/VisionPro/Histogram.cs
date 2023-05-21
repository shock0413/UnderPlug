using System;
using System.IO;
using Cognex.VisionPro;
using Cognex.VisionPro.Exceptions;
using Cognex.VisionPro.PMAlign;

namespace Hansero.VisionLib.VisionPro
{
    /// <summary>
    /// Class1에 대한 요약 설명입니다.
    /// </summary>
    public class Histogram : ToolBase
    {
        private CogRectangleAffine m_RegionArea = new CogRectangleAffine();
        private Cognex.VisionPro.CogCircle m_RegionAreaCircle = new CogCircle();
        private Cognex.VisionPro.CogPolygon m_RegionAreaPoligon = new CogPolygon();
        public enum REGIONSHAPE { Rectangle, Circle, Poligon, Ring };

        private Cognex.VisionPro.ImageProcessing.CogHistogram m_Histogram = new Cognex.VisionPro.ImageProcessing.CogHistogram();

        private Cognex.VisionPro.ImageProcessing.CogHistogramTool m_HistogramTool = new Cognex.VisionPro.ImageProcessing.CogHistogramTool();

        private System.Drawing.Point m_TranslationPoint = new System.Drawing.Point();

        private REGIONSHAPE m_RegionShape;

        public REGIONSHAPE RegionShape
        {
            get
            {
                return m_RegionShape;
            }
            set
            {
                m_RegionShape = value;
            }
        }

        // 기본 폴더 설정.
        string BasePath;

        public Histogram()
        {
            //
            // TODO: 여기에 생성자 논리를 추가합니다.
            //

            BasePath = System.Windows.Forms.Application.StartupPath + "\\" + "Histogram\\";

            m_RegionShape = REGIONSHAPE.Poligon;
        }

        public double Caculate_Bright()
        {
            Cognex.VisionPro.ImageProcessing.CogHistogramResult Result = m_Histogram.Execute(m_Image, m_RegionAreaPoligon);

            return Result.Mean;
        }
        /// <summary>
        /// 히스토그램 값 찾기
        /// 히스토그램 툴의 경우 검사영역을 따로 저장하는 함수가 없다.따라서패턴툴이든 다른툴을 이용해 vpp파일을 생성하고
        /// 히스토그램실행시 이 클래스를 생성 후 vpp에서 알고 있는 검사영역을 Region인자에 넣어주면 된다.
        /// </summary>
        /// <param name="MonoImage">흑백영상</param>
        /// <param name="Region">검사영역</param>
        /// <returns>히스토그램중 Mean값을 리턴 함</returns>
        public double Find_Histo(ICogImage MonoImage, ICogRegion Region)
        {

            Cognex.VisionPro.ImageProcessing.CogHistogramResult Result;

            try
            {
                Result = m_Histogram.Execute(MonoImage, Region);
            }
            catch
            {
                return -1;
            }

            return Result.Mean;

        }

        public double Find_HistoPixelCount(ICogImage MonoImage, ICogRegion Region, int minHisto, int maxHisto)
        {
            int count = 0;
            Cognex.VisionPro.ImageProcessing.CogHistogramResult Result;

            try
            {


                Result = m_Histogram.Execute(MonoImage, Region);
                int[] histograms = Result.GetHistogram();

                for (int i = minHisto; i <= maxHisto; i++)
                {
                    count += histograms[i];
                }
            }
            catch
            {
                return -1;
            }

            return count;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MonoImage"></param>
        /// <param name="Region"></param>
        /// <returns></returns>
        public double Find_Histo(ICogImage MonoImage, ICogRegion Region, double CenterX, double CenterY)
        {

            Cognex.VisionPro.ImageProcessing.CogHistogramResult Result;


            // 기준 위치로 검사 영역 이동(2012년 5월18일 이기문 수정)
            if (Region.GetType().ToString().IndexOf("Rectangle") >= 0)
            {
                m_RectangleRegion = null;
                m_RectangleRegion = (CogRectangleAffine)Region;

                if (CenterX > 0 && CenterY > 0)
                {
                    m_RectangleRegion.CenterX = CenterX;
                    m_RectangleRegion.CenterY = CenterY;
                }

                Region = m_RectangleRegion;
            }
            else if (Region.GetType().ToString().IndexOf("Circular") >= 0)
            {
                m_RingRegion = (CogCircularAnnulusSection)Region;

                if (CenterX > 0 && CenterY > 0)
                {
                    m_RingRegion.CenterX = CenterX;
                    m_RingRegion.CenterY = CenterY;
                }

                Region = m_RingRegion;
            }
            else if (Region.GetType().ToString().IndexOf("Circle") >= 0)
            {
                m_CircleRegion = null;
                m_CircleRegion = (CogCircle)Region;

                if (CenterX > 0 && CenterY > 0)
                {
                    m_CircleRegion.CenterX = m_CenterX;
                    m_CircleRegion.CenterY = m_CenterY;
                }

                Region = m_CircleRegion;
            }
            else if (Region.GetType().ToString().IndexOf("Polygon") >= 0)
            {
                try
                {
                    m_RingRegion = (CogCircularAnnulusSection)Region;
                }
                catch
                {
                    Region = null;

                    Region = new CogCircularAnnulusSection();

                    m_RingRegion.CenterX = CenterX;
                    m_RingRegion.CenterY = CenterY;
                    m_RingRegion = (CogCircularAnnulusSection)Region;
                }

                if (CenterX > 0 && CenterY > 0)
                {
                    m_RingRegion.CenterX = CenterX;
                    m_RingRegion.CenterY = CenterY;
                }

                Region = m_RingRegion;
            }

            try
            {
                Result = m_Histogram.Execute(MonoImage, Region);
            }
            catch
            {
                return -1;
            }

            return Result.Mean;
        }
        /// <summary>
        /// 히스토그램을 실행하여 검사영역 내에 최소,최대값 사이에 해당하는 픽셀갯수 찾을수 있는 함수
        /// </summary>
        /// <param name="MonoImage">흑백이미지</param>
        /// <param name="Region">검사영역</param>
        /// <param name="CenterX"></param>
        /// <param name="CenterY"></param>
        /// <param name="min">밝기 최소값</param>
        /// <param name="max">밝기 최대값</param>
        /// <returns>검사영역내 밝기 최소값과 최대값사이에 해당하는 픽셀갯수 합</returns>
        public double Find_Histo(ICogImage MonoImage, ICogRegion Region, double CenterX, double CenterY, int min, int max)
        {
            int Sum = 0;

            // 기준 위치로 검사 영역 이동(2012년 5월18일 이기문 수정)
            if (Region.GetType().ToString().IndexOf("Rectangle") >= 0)
            {
                m_RectangleRegion = null;
                m_RectangleRegion = (CogRectangleAffine)Region;

                if (CenterX > 0 && CenterY > 0)
                {
                    m_RectangleRegion.CenterX = CenterX;
                    m_RectangleRegion.CenterY = CenterY;
                }

                Region = m_RectangleRegion;
            }
            else if (Region.GetType().ToString().IndexOf("Circular") >= 0)
            {
                m_RingRegion = (CogCircularAnnulusSection)Region;

                if (CenterX > 0 && CenterY > 0)
                {
                    m_RingRegion.CenterX = CenterX;
                    m_RingRegion.CenterY = CenterY;
                }

                Region = m_RingRegion;
            }
            else if (Region.GetType().ToString().IndexOf("Circle") >= 0)
            {
                m_CircleRegion = null;
                m_CircleRegion = (CogCircle)Region;

                if (CenterX > 0 && CenterY > 0)
                {
                    m_CircleRegion.CenterX = m_CenterX;
                    m_CircleRegion.CenterY = m_CenterY;
                }

                Region = m_CircleRegion;
            }
            else if (Region.GetType().ToString().IndexOf("Polygon") >= 0)
            {
                try
                {
                    m_RingRegion = (CogCircularAnnulusSection)Region;
                }
                catch
                {
                    Region = null;

                    Region = new CogCircularAnnulusSection();

                    m_RingRegion.CenterX = CenterX;
                    m_RingRegion.CenterY = CenterY;
                    m_RingRegion = (CogCircularAnnulusSection)Region;
                }

                if (CenterX > 0 && CenterY > 0)
                {
                    m_RingRegion.CenterX = CenterX;
                    m_RingRegion.CenterY = CenterY;
                }

                Region = m_RingRegion;
            }

            Cognex.VisionPro.ImageProcessing.CogHistogramResult Result = m_Histogram.Execute(MonoImage, Region);


            for (int i = min; i < max; i++)
            {
                //Sum += m_HistogramTool.Result.GetHistogram()[i];

                Sum += Result.GetHistogram()[i];
            }


            return Sum;
        }

        /// <summary>
        /// 히스토그램중 최소 최대값을 넣으면 int형으로 인자결과값을 받을 수 있다.
        /// </summary>
        /// <param name="MonoImage"></param>
        /// <param name="Region"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public double Find_Histo(ICogImage MonoImage, ICogRegion Region, int min, int max)
        {
            Cognex.VisionPro.ImageProcessing.CogHistogramResult Result = m_Histogram.Execute(MonoImage, Region);

            double a;
            int b = 0;
            try
            {

                b = Result.GetMinimumWGVThreshold(min, max, out a);
            }
            catch
            {
                return b = 0;
            }

            return b;
        }



        #region Vision Tool Function
        /// <summary>
        /// 패턴 영역을 설정하기 위해서 디폴트 영역 표시
        /// </summary>
        /// <param name="Display">영역을 표시할 디스플레이</param>
        public void Display_PatternArea(Cognex.VisionPro.Display.CogDisplay Display)
        {
            if (m_RegionShape == REGIONSHAPE.Circle)
            {
                m_RegionAreaCircle.CenterX = 320;
                m_RegionAreaCircle.CenterY = 240;
                m_RegionAreaCircle.Radius = 100;

                m_RegionAreaCircle.GraphicDOFEnable = Cognex.VisionPro.CogCircleDOFConstants.All;
                m_RegionAreaCircle.Interactive = true;

                Display.InteractiveGraphics.Add(m_RegionAreaCircle, "", true);
            }
            else if (m_RegionShape == REGIONSHAPE.Rectangle)
            {
                m_RegionArea.CenterX = 320;
                m_RegionArea.CenterY = 240;
                m_RegionArea.SideXLength = 100;
                m_RegionArea.SideYLength = 100;
                m_RegionArea.Rotation = 0;
                m_RegionArea.GraphicDOFEnable = Cognex.VisionPro.CogRectangleAffineDOFConstants.All;
                m_RegionArea.Interactive = true;

                Display.InteractiveGraphics.Add(m_RegionArea, "", true);
            }
            else if (m_RegionShape == REGIONSHAPE.Poligon)
            {
                for (int i = m_RegionAreaPoligon.NumVertices; i > 0; i--)
                {
                    m_RegionAreaPoligon.RemoveVertex(0);
                }

                m_RegionAreaPoligon.AddVertex(220, 140, 0);
                m_RegionAreaPoligon.AddVertex(420, 140, 1);
                m_RegionAreaPoligon.AddVertex(420, 340, 2);
                m_RegionAreaPoligon.AddVertex(220, 340, 3);

                m_RegionAreaPoligon.GraphicDOFEnable = Cognex.VisionPro.CogPolygonDOFConstants.All;
                m_RegionAreaPoligon.Interactive = true;

                Display.InteractiveGraphics.Add(m_RegionAreaPoligon, "", true);
            }
        }

        public void Add_Vertex(Cognex.VisionPro.Display.CogDisplay Display)
        {
            m_RegionAreaPoligon.AddVertex((m_RegionAreaPoligon.GetVertexX(m_RegionAreaPoligon.NumVertices - 1) + m_RegionAreaPoligon.GetVertexX(0)) / 2, (m_RegionAreaPoligon.GetVertexY(m_RegionAreaPoligon.NumVertices - 1) + m_RegionAreaPoligon.GetVertexY(0)) / 2, m_RegionAreaPoligon.NumVertices);

            m_RegionAreaPoligon.GraphicDOFEnable = Cognex.VisionPro.CogPolygonDOFConstants.All;
            //m_RegionAreaPoligon.Interactive = true;

            //Display.InteractiveGraphics.Add(m_RegionAreaPoligon, "", true);
        }
        #endregion


    }
}