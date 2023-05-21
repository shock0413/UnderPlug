using System;
using System.IO;
using System.Collections;

using Cognex.VisionPro;
using Cognex.VisionPro.Exceptions;
using Cognex.VisionPro.PMAlign;

using System.Windows.Forms;
using System.Collections.Generic;

namespace Hansero.VisionLib.VisionPro
{
    /// <summary>
    /// Class1에 대한 요약 설명입니다.
    /// </summary>
    public class PMAlgin : ToolBase
    {

        CogPMAlignTool m_CogPMAlignTool = new CogPMAlignTool();
        /// <summary>
        /// 패턴을 어레이리스트에 넣음
        /// </summary>
        public System.Collections.ArrayList m_Pattern = new ArrayList();
        /// <summary>
        /// 패턴폴더명을 이름으로 하여 어레이리스트에 넣음
        /// </summary>
        public System.Collections.ArrayList m_PatternName = new ArrayList();

        private CogRectangleAffine m_PatternArea = new CogRectangleAffine();
        private CogCircle m_CircleArea = new CogCircle();
        private CogPolygon m_PolygonArea = new CogPolygon();

        private CogPointMarker m_PointPosition = new CogPointMarker();
        protected Cognex.VisionPro.CogImage8Grey m_MaskImage = null;

        private int m_FindPatternIndex;
        /// <summary>
        /// 찾은 패턴번호값을 리턴
        /// </summary>
        public int FindPatternIndex
        {
            get
            {
                return m_FindPatternIndex;
            }
        }

        private Cognex.VisionPro.PMAlign.CogPMAlignRunAlgorithmConstants m_Algorithm;
        /// <summary>
        /// PMAlign 알고리즘
        /// </summary>
		public Cognex.VisionPro.PMAlign.CogPMAlignRunAlgorithmConstants Algorithm
        {
            get
            {
                return m_Algorithm;
            }
            set
            {
                m_Algorithm = value;
            }
        }

        public Cognex.VisionPro.CogImage8Grey MaskImage
        {
            get
            {
                return m_MaskImage;
            }
            set
            {
                m_MaskImage = null;
                m_MaskImage = value;
            }
        }

        private double m_AcceptThershold;
        /// <summary>
        /// OK스코어
        /// </summary>
		public double OKScore
        {
            set
            {
                m_AcceptThershold = value;
            }
            get
            {
                return m_AcceptThershold;
            }
        }

        private double m_Zoom;
        /// <summary>
        /// 비율
        /// </summary>
		public double Zoom
        {
            get
            {
                return m_Zoom;
            }
            set
            {
                m_Zoom = value;
            }
        }

        private double m_XScale;
        /// <summary>
        /// XScale
        /// </summary>
        public double XScale
        {
            get
            {
                return m_XScale;
            }
            set
            {
                m_XScale = value;
            }
        }
        private double m_YScale;
        /// <summary>
        /// YScale
        /// </summary>
        public double YScale
        {
            get
            {
                return m_YScale;
            }
            set
            {
                m_YScale = value;
            }
        }
        private double m_Elasticity;
        /// <summary>
        /// 패턴 모델의 탄력성을 가지도록 값 지정하기
        /// </summary>
        public double Elasticity
        {
            get
            {
                return m_Elasticity;
            }
            set
            {
                m_Elasticity = value;
            }
        }
        private double m_Angle;
        /// <summary>
        /// 패턴 각도지정
        /// </summary>
		public double Angle
        {
            get
            {
                return m_Angle * 180 / Math.PI;
            }
            set
            {
                m_Angle = value * Math.PI / 180;
            }
        }

        private double m_Radius;
        /// <summary>
        /// 
        /// </summary>
        public double Radius
        {
            get
            {
                return m_Radius;
            }
            set
            {
                m_Radius = value;
            }
        }

        private double m_TranslationX;
        private double m_TranslationY;
        /// <summary>
        /// 패턴 찾은 X포지션
        /// </summary>
		public double TranslationX
        {
            get
            {
                return m_TranslationX;
            }
            set
            {
                m_TranslationX = value;
            }
        }
        /// <summary>
        /// 패턴 찾은 Y포지션
        /// </summary>
		public double TranslationY
        {
            get
            {
                return m_TranslationY;
            }
            set
            {
                m_TranslationY = value;
            }
        }

        private double m_Rotation;
        /// <summary>
        /// 찾은 패턴 틀어진 정도
        /// </summary>
        public double Rotation
        {
            get
            {
                return m_Rotation;
            }
        }


        public double m_r1X;
        public double m_r1Y;
        public double m_r2X;
        public double m_r2Y;

        /// <summary>
        /// 검사한 패턴의 상위 폴더명 즉 패턴명을 저장한다.
        /// </summary>
        public string ReadChar;

        // 기본 폴더 설정.
        string BasePath;

        private int m_ContrastThresh;
        /// <summary>
        /// ContrastThresh 값
        /// </summary>
        public int ContrastThresh
        {
            get
            {
                return m_ContrastThresh;
            }
            set
            {
                m_ContrastThresh = value;
            }
        }

        /// <summary>
        /// 검사영역모양
        /// </summary>
        public enum PATTERNSHAPE { Rectangle, Circle, Polygon }

        private PATTERNSHAPE m_PatternShape;
        /// <summary>
        /// 검사영역모양
        /// </summary>
		public PATTERNSHAPE PatternShape
        {
            get
            {
                return m_PatternShape;
            }
            set
            {
                m_PatternShape = value;
            }
        }
        /// <summary>
        /// 패턴찾기 툴 생성
        /// </summary>
		public PMAlgin()
        {
            //
            // TODO: 여기에 생성자 논리를 추가합니다.
            //

            BasePath = System.Windows.Forms.Application.StartupPath + "\\" + "PMAlign\\";
        }

        #region Vision Tool Function
        /// <summary>
        /// 패턴 찾기.
        /// 지정된 폴더에서 필요한 정보를 로딩하여 검사를 수행함.
        /// 검사 영역, 합격 점수, 흑백 대비 값, 패턴 폴더 등이 지정되어 있음.
        /// </summary>
        /// <param name="Name">패턴 이름. "차종\카메라 이름\하위 이름" 형식을 취할 것.</param>
        /// <param name="Display">결과를 출력할 디스플레이</param>
        /// 검사 영역 표시까지 작업.
        /// 패턴 정보의 모양. 
        /// 실행파일 폴더\PMAlign\차종\카메라명\패턴명.ini - 환경변수
        /// 실행파일 폴더\PMAlign\차종\카메라명\패턴명\ - 각각에 대한 멀티 패턴 폴더
        public double FindPattern(Cognex.VisionPro.Display.CogDisplay Display, bool bViewArea)
        {
            // 현재 최고 점수
            double Score = 0;

            ReadChar = "?";

            #region 이미지 설정
            if (m_Image == null)
            {
                Write_SystemLog("이미지가 없습니다.-패턴매칭툴");
                return Score = 0;
            }

            m_CogPMAlignTool.InputImage = null;
            m_CogPMAlignTool.InputImage = (Cognex.VisionPro.CogImage8Grey)m_Image;
            #endregion

            #region 패턴 로딩 후 검사
            try
            {


                string a = m_Region.GetType().ToString();
                // 기준 위치로 검사 영역 이동
                if (m_Region.GetType().ToString().IndexOf("Rectangle") >= 0)
                {
                    m_RectangleRegion = null;
                    m_RectangleRegion = (CogRectangleAffine)m_Region;

                    //2015.10.29 modify
                    //if (m_CenterX > 0 && m_CenterY > 0)
                    //{
                    //    m_RectangleRegion.CenterX = m_CenterX;
                    //    m_RectangleRegion.CenterY = m_CenterY;
                    //}


                    m_Region = m_RectangleRegion;
                }
                else if (m_Region.GetType().ToString().IndexOf("Circular") >= 0)
                {
                    m_RingRegion = (CogCircularAnnulusSection)m_Region;

                    if (m_Radius == 0)
                        m_Radius = 1;

                    //m_RingRegion.Radius = m_Radius;
                    //m_Region = m_RingRegion;

                    //m_RingRegion = (CogCircularAnnulusSection)m_Region;
                    m_Radius = m_RingRegion.Radius;
                    m_Region = m_RingRegion;
                }
                else
                {
                    m_CircleRegion = null;
                    m_CircleRegion = (CogCircle)m_Region;

                    if (m_CenterX > 0 && m_CenterY > 0)
                    {
                        m_CircleRegion.CenterX = m_CenterX;
                        m_CircleRegion.CenterY = m_CenterY;
                    }

                    m_Region = m_CircleRegion;
                }
            }
            catch
            {
                Score = 0;
                ReadChar = "?";
                return Score;

            }

            m_CogPMAlignTool.SearchRegion = m_Region;
            m_CogPMAlignTool.RunParams.ContrastThreshold = m_ContrastThresh;

            double x1, x2, y1, y2;

            m_TranslationX = m_TranslationY = m_Rotation = 0;

            int Index = -1;

            try
            {
                for (int i = 0; i < m_Pattern.Count; i++)
                {
                    m_CogPMAlignTool.Pattern = null;
                    m_CogPMAlignTool.Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern)m_Pattern[i];



                    m_Region = null;

                    m_Region = m_CogPMAlignTool.SearchRegion;




                    m_CogPMAlignTool.RunParams.RunAlgorithm = CogPMAlignRunAlgorithmConstants.BestTrained;

                    m_CogPMAlignTool.RunParams.AcceptThreshold = m_AcceptThershold;

                    // 파일로부터 패턴 로드
                    //m_CogPMAlignTool.Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern) Cognex.VisionPro.CogSerializer.LoadObjectFromFile(filename);



                    //이부분 주석처리함...2011년 4월30일 이기문->써클과 사각형 패턴이 있을시 에러 발생
                    //if (m_PatternShape == PATTERNSHAPE.Rectangle)
                    //{
                    //    m_PatternArea = null;
                    //    m_PatternArea = (CogRectangleAffine)m_CogPMAlignTool.Pattern.TrainRegion;
                    //}
                    //else if (m_PatternShape == PATTERNSHAPE.Circle)
                    //{
                    //    m_CircleArea = null;
                    //    m_CircleArea = (CogCircle)m_CogPMAlignTool.Pattern.TrainRegion;
                    //}



                    // 검사 수행
                    m_CogPMAlignTool.Run();

                    // 찾은 결과가 있고
                    if (m_CogPMAlignTool.Results != null && m_CogPMAlignTool.Results.Count > 0)
                    {
                        // 합격 점수 보다 높고, 최고 점수보다 높으면 찾은 패턴 정보 갱신.
                        if (m_CogPMAlignTool.Results[0].Score > Score)
                        {
                            m_FindPatternIndex = i;

                            if (m_PatternShape == PATTERNSHAPE.Rectangle)
                            {
                                //찾은 패턴의 크기
                                x1 = m_CogPMAlignTool.Results[0].GetPose().TranslationX - m_PatternArea.SideXLength / 2;
                                y1 = m_CogPMAlignTool.Results[0].GetPose().TranslationY - m_PatternArea.SideYLength / 2;
                                x2 = m_CogPMAlignTool.Results[0].GetPose().TranslationX + m_PatternArea.SideXLength / 2;
                                y2 = m_CogPMAlignTool.Results[0].GetPose().TranslationY + m_PatternArea.SideYLength / 2;

                                //if (m_RegionShape == RegionShape.Rectangle)
                                if (m_Region.GetType().ToString().IndexOf("Rectangle") >= 0)
                                {
                                    // 검사 영역 벗어나는 경우 체크하는 알고리즘 보류
                                    // 2012.11.01 김기택
                                    //if (!(x1 < m_RectangleRegion.CornerOriginX || y1 < m_RectangleRegion.CornerOriginY || x2 > m_RectangleRegion.CornerOriginX + m_RectangleRegion.SideXLength || y2 > m_RectangleRegion.CornerOriginY + m_RectangleRegion.SideYLength))
                                    //    if (!(x1 < m_RectangleRegion.CenterX - m_RectangleRegion.SideXLength || y1 < m_RectangleRegion.CenterY - m_RectangleRegion.SideYLength || x2 > m_RectangleRegion.CenterX + m_RectangleRegion.SideXLength || y2 > m_RectangleRegion.CenterY + m_RectangleRegion.SideYLength))
                                    //    {
                                    //        Score = m_CogPMAlignTool.Results[0].Score;
                                    //        Index = i;
                                    //        m_TranslationX = ((int)m_CogPMAlignTool.Results[0].GetPose().TranslationX * 100) / 100;
                                    //        m_TranslationY = ((int)m_CogPMAlignTool.Results[0].GetPose().TranslationY * 100) / 100;
                                    //        m_Rotation = m_CogPMAlignTool.Results[0].GetPose().Rotation;
                                    //    }

                                    Score = m_CogPMAlignTool.Results[0].Score;
                                    Index = i;
                                    //m_TranslationX = ((int)m_CogPMAlignTool.Results[0].GetPose().TranslationX * 100) / 100;
                                    //m_TranslationY = ((int)m_CogPMAlignTool.Results[0].GetPose().TranslationY * 100) / 100;
                                    m_TranslationX = (m_CogPMAlignTool.Results[0].GetPose().TranslationX * 100) / 100;
                                    m_TranslationY = (m_CogPMAlignTool.Results[0].GetPose().TranslationY * 100) / 100;
                                    m_Rotation = m_CogPMAlignTool.Results[0].GetPose().Rotation;
                                }
                                else
                                {
                                    Score = m_CogPMAlignTool.Results[0].Score;
                                    Index = i;
                                    m_TranslationX = ((int)m_CogPMAlignTool.Results[0].GetPose().TranslationX * 100) / 100;
                                    m_TranslationY = ((int)m_CogPMAlignTool.Results[0].GetPose().TranslationY * 100) / 100;
                                    m_Rotation = m_CogPMAlignTool.Results[0].GetPose().Rotation;
                                }
                            }
                            else if (m_PatternShape == PATTERNSHAPE.Circle)
                            {
                                // 패턴 영역이 검사 영역을 벗어날 경우 NG로 처리						
                                //x1 = m_CogPMAlignTool.Results[0].GetPose().TranslationX - (m_CogPMAlignTool.Pattern.Origin.TranslationX - m_PatternArea.CenterX) - m_CircleArea.Radius;
                                //y1 = m_CogPMAlignTool.Results[0].GetPose().TranslationY - (m_CogPMAlignTool.Pattern.Origin.TranslationY - m_PatternArea.CenterY) - m_CircleArea.Radius;
                                //x2 = m_CogPMAlignTool.Results[0].GetPose().TranslationX - (m_CogPMAlignTool.Pattern.Origin.TranslationX - m_PatternArea.CenterX) + m_CircleArea.Radius;
                                //y2 = m_CogPMAlignTool.Results[0].GetPose().TranslationY - (m_CogPMAlignTool.Pattern.Origin.TranslationY - m_PatternArea.CenterY) + m_CircleArea.Radius;

                                x1 = m_CogPMAlignTool.Results[0].GetPose().TranslationX - m_CircleArea.Radius;
                                y1 = m_CogPMAlignTool.Results[0].GetPose().TranslationY - m_CircleArea.Radius;
                                x2 = m_CogPMAlignTool.Results[0].GetPose().TranslationX + m_CircleArea.Radius;
                                y2 = m_CogPMAlignTool.Results[0].GetPose().TranslationY + m_CircleArea.Radius;

                                //Score = m_CogPMAlignTool.Results[0].Score;
                                //Index = i;
                                //m_TranslationX = m_CogPMAlignTool.Results[0].GetPose().TranslationX;
                                //m_TranslationY = m_CogPMAlignTool.Results[0].GetPose().TranslationY;

                                if (!(x1 < m_RectangleRegion.CornerOriginX || y1 < m_RectangleRegion.CornerOriginY || x2 > m_RectangleRegion.CornerOriginX + m_RectangleRegion.SideXLength || y2 > m_RectangleRegion.CornerOriginY + m_RectangleRegion.SideYLength))
                                {
                                    Score = m_CogPMAlignTool.Results[0].Score;
                                    Index = i; ;
                                    m_TranslationX = m_CogPMAlignTool.Results[0].GetPose().TranslationX;
                                    m_TranslationY = m_CogPMAlignTool.Results[0].GetPose().TranslationY;
                                    m_Rotation = m_CogPMAlignTool.Results[0].GetPose().Rotation;
                                }
                            }
                            else
                            {
                                Score = m_CogPMAlignTool.Results[0].Score;
                                Index = i;
                                m_TranslationX = m_CogPMAlignTool.Results[0].GetPose().TranslationX;
                                m_TranslationY = m_CogPMAlignTool.Results[0].GetPose().TranslationY;
                                m_Rotation = m_CogPMAlignTool.Results[0].GetPose().Rotation;
                            }
                        }
                    }
                    #endregion
                }

                #region 검사 결과 디스플레이
                //Display.StaticGraphics.Clear();

                // 검사 영역 표시
                if (bViewArea)
                {
                    DisplaySearchArea(Display, false);
                }

                // 찾은 결과가 있을 경우 찾은 영역, 점수 표시
                if (Index >= 0)
                {
                    m_CogPMAlignTool.Pattern = null;

                    // 가장 높은 점수로 찾은 패턴 다시 찾기
                    // 파일로부터 패턴 로드
                    m_CogPMAlignTool.Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern)m_Pattern[Index];

                    ReadChar = m_PatternName[Index].ToString();
                    // 검사 수행
                    m_CogPMAlignTool.Run();

                    if (bViewArea == true)
                    {
                        // 찾은 패턴 표시
                        //DrawCross(m_TranslationX, m_TranslationY, 5, 1, Display, CogColorConstants.Green, CogColorConstants.Black);
                        Display.StaticGraphics.Add((Cognex.VisionPro.ICogGraphic)m_CogPMAlignTool.Results[0].CreateResultGraphics(CogPMAlignResultGraphicConstants.MatchRegion | CogPMAlignResultGraphicConstants.Origin), "");

                    }

                }
                #endregion

                m_SearchArea = null;
            }
            catch (Exception ex)
            {

                Score = 0;
                ReadChar = "?";

                //throw ex;
                return Score;
            }

            return Score;
        }

        public double FindPattern(Cognex.VisionPro.Display.CogDisplay Display, bool bViewArea, string FindChar)
        {
            // 현재 최고 점수
            double Score = 0;

            ReadChar = "?";

            #region 이미지 설정
            if (m_Image == null)
            {
                Write_SystemLog("이미지가 없습니다.-패턴매칭툴");
                return Score = 0;
            }


            m_CogPMAlignTool.InputImage = null;
            m_CogPMAlignTool.InputImage = (Cognex.VisionPro.CogImage8Grey)m_Image;
            #endregion

            #region 패턴 로딩 후 검사
            try
            {
                // 기준 위치로 검사 영역 이동
                if (m_Region.GetType().ToString().IndexOf("Rectangle") >= 0)
                {
                    m_RectangleRegion = null;
                    m_RectangleRegion = (CogRectangleAffine)m_Region;

                    if (m_CenterX > 0 && m_CenterY > 0)
                    {
                        m_RectangleRegion.CenterX = m_CenterX;
                        m_RectangleRegion.CenterY = m_CenterY;
                    }

                    m_Region = m_RectangleRegion;
                }
                else if (m_Region.GetType().ToString().IndexOf("Circular") >= 0)
                {
                    m_RingRegion = (CogCircularAnnulusSection)m_Region;

                    if (m_Radius == 0)
                        m_Radius = 1;

                    m_RingRegion.Radius = m_Radius;
                    m_Region = m_RingRegion;
                }
                else
                {
                    m_CircleRegion = null;
                    m_CircleRegion = (CogCircle)m_Region;

                    if (m_CenterX > 0 && m_CenterY > 0)
                    {
                        m_CircleRegion.CenterX = m_CenterX;
                        m_CircleRegion.CenterY = m_CenterY;
                    }

                    m_Region = m_CircleRegion;
                }
            }
            catch
            {
                Score = 0;
                ReadChar = "?";
                return Score;

            }

            m_CogPMAlignTool.SearchRegion = m_Region;
            m_CogPMAlignTool.RunParams.ContrastThreshold = m_ContrastThresh;

            double x1, x2, y1, y2;

            m_TranslationX = m_TranslationY = m_Rotation = 0;

            int Index = -1;

            try
            {
                for (int i = 0; i < m_Pattern.Count; i++)
                {
                    if (m_PatternName[i].ToString() == FindChar)
                    {
                        m_CogPMAlignTool.Pattern = null;
                        m_CogPMAlignTool.Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern)m_Pattern[i];

                        m_Region = null;

                        m_Region = m_CogPMAlignTool.SearchRegion;
                        //m_CogPMAlignTool.RunParams.AcceptThreshold = m_AcceptThershold / 100;

                        // 파일로부터 패턴 로드
                        //m_CogPMAlignTool.Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern) Cognex.VisionPro.CogSerializer.LoadObjectFromFile(filename);

                        //if (m_PatternShape == PATTERNSHAPE.Rectangle)
                        //{
                        //    m_PatternArea = null;
                        //    m_PatternArea = (CogRectangleAffine)m_CogPMAlignTool.Pattern.TrainRegion;
                        //}
                        //else if (m_PatternShape == PATTERNSHAPE.Circle)
                        //{
                        //    m_CircleArea = null;
                        //    m_CircleArea = (CogCircle)m_CogPMAlignTool.Pattern.TrainRegion;
                        //}

                        // 검사 수행
                        m_CogPMAlignTool.Run();

                        // 찾은 결과가 있고
                        if (m_CogPMAlignTool.Results != null && m_CogPMAlignTool.Results.Count > 0)
                        {
                            // 합격 점수 보다 높고, 최고 점수보다 높으면 찾은 패턴 정보 갱신.
                            if (m_CogPMAlignTool.Results[0].Score > Score)
                            {
                                m_FindPatternIndex = i;

                                if (m_PatternShape == PATTERNSHAPE.Rectangle)
                                {
                                    //찾은 패턴의 크기
                                    x1 = m_CogPMAlignTool.Results[0].GetPose().TranslationX - m_PatternArea.SideXLength / 2;
                                    y1 = m_CogPMAlignTool.Results[0].GetPose().TranslationY - m_PatternArea.SideYLength / 2;
                                    x2 = m_CogPMAlignTool.Results[0].GetPose().TranslationX + m_PatternArea.SideXLength / 2;
                                    y2 = m_CogPMAlignTool.Results[0].GetPose().TranslationY + m_PatternArea.SideYLength / 2;

                                    if (m_Region.GetType().ToString().IndexOf("Rectangle") >= 0)
                                    {
                                        //if (!(x1 < m_RectangleRegion.CornerOriginX || y1 < m_RectangleRegion.CornerOriginY || x2 > m_RectangleRegion.CornerOriginX + m_RectangleRegion.SideXLength || y2 > m_RectangleRegion.CornerOriginY + m_RectangleRegion.SideYLength))
                                        if (!(x1 < m_RectangleRegion.CenterX - m_RectangleRegion.SideXLength || y1 < m_RectangleRegion.CenterY - m_RectangleRegion.SideYLength || x2 > m_RectangleRegion.CenterX + m_RectangleRegion.SideXLength || y2 > m_RectangleRegion.CenterY + m_RectangleRegion.SideYLength))
                                        {
                                            Score = m_CogPMAlignTool.Results[0].Score;
                                            Index = i;
                                            m_TranslationX = ((int)m_CogPMAlignTool.Results[0].GetPose().TranslationX * 100) / 100;
                                            m_TranslationY = ((int)m_CogPMAlignTool.Results[0].GetPose().TranslationY * 100) / 100;
                                            m_Rotation = m_CogPMAlignTool.Results[0].GetPose().Rotation;
                                        }
                                    }
                                    else
                                    {
                                        Score = m_CogPMAlignTool.Results[0].Score;
                                        Index = i;
                                        m_TranslationX = ((int)m_CogPMAlignTool.Results[0].GetPose().TranslationX * 100) / 100;
                                        m_TranslationY = ((int)m_CogPMAlignTool.Results[0].GetPose().TranslationY * 100) / 100;
                                        m_Rotation = m_CogPMAlignTool.Results[0].GetPose().Rotation;
                                    }
                                }
                                else if (m_PatternShape == PATTERNSHAPE.Circle)
                                {
                                    // 패턴 영역이 검사 영역을 벗어날 경우 NG로 처리						
                                    //x1 = m_CogPMAlignTool.Results[0].GetPose().TranslationX - (m_CogPMAlignTool.Pattern.Origin.TranslationX - m_PatternArea.CenterX) - m_CircleArea.Radius;
                                    //y1 = m_CogPMAlignTool.Results[0].GetPose().TranslationY - (m_CogPMAlignTool.Pattern.Origin.TranslationY - m_PatternArea.CenterY) - m_CircleArea.Radius;
                                    //x2 = m_CogPMAlignTool.Results[0].GetPose().TranslationX - (m_CogPMAlignTool.Pattern.Origin.TranslationX - m_PatternArea.CenterX) + m_CircleArea.Radius;
                                    //y2 = m_CogPMAlignTool.Results[0].GetPose().TranslationY - (m_CogPMAlignTool.Pattern.Origin.TranslationY - m_PatternArea.CenterY) + m_CircleArea.Radius;

                                    x1 = m_CogPMAlignTool.Results[0].GetPose().TranslationX - m_CircleArea.Radius;
                                    y1 = m_CogPMAlignTool.Results[0].GetPose().TranslationY - m_CircleArea.Radius;
                                    x2 = m_CogPMAlignTool.Results[0].GetPose().TranslationX + m_CircleArea.Radius;
                                    y2 = m_CogPMAlignTool.Results[0].GetPose().TranslationY + m_CircleArea.Radius;

                                    //Score = m_CogPMAlignTool.Results[0].Score;
                                    //Index = i;
                                    //m_TranslationX = m_CogPMAlignTool.Results[0].GetPose().TranslationX;
                                    //m_TranslationY = m_CogPMAlignTool.Results[0].GetPose().TranslationY;

                                    if (!(x1 < m_RectangleRegion.CornerOriginX || y1 < m_RectangleRegion.CornerOriginY || x2 > m_RectangleRegion.CornerOriginX + m_RectangleRegion.SideXLength || y2 > m_RectangleRegion.CornerOriginY + m_RectangleRegion.SideYLength))
                                    {
                                        Score = m_CogPMAlignTool.Results[0].Score;
                                        Index = i; ;
                                        m_TranslationX = m_CogPMAlignTool.Results[0].GetPose().TranslationX;
                                        m_TranslationY = m_CogPMAlignTool.Results[0].GetPose().TranslationY;
                                        m_Rotation = m_CogPMAlignTool.Results[0].GetPose().Rotation;
                                    }
                                }
                                else
                                {
                                    Score = m_CogPMAlignTool.Results[0].Score;
                                    Index = i;
                                    m_TranslationX = m_CogPMAlignTool.Results[0].GetPose().TranslationX;
                                    m_TranslationY = m_CogPMAlignTool.Results[0].GetPose().TranslationY;
                                    m_Rotation = m_CogPMAlignTool.Results[0].GetPose().Rotation;
                                }
                            }
                        }
                    }
                    #endregion
                }

                #region 검사 결과 디스플레이
                //Display.StaticGraphics.Clear();

                // 검사 영역 표시
                if (bViewArea)
                {
                    DisplaySearchArea(Display, false);
                }

                // 찾은 결과가 있을 경우 찾은 영역, 점수 표시
                if (Index >= 0)
                {
                    m_CogPMAlignTool.Pattern = null;

                    // 가장 높은 점수로 찾은 패턴 다시 찾기
                    // 파일로부터 패턴 로드
                    m_CogPMAlignTool.Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern)m_Pattern[Index];

                    ReadChar = m_PatternName[Index].ToString();
                    // 검사 수행
                    m_CogPMAlignTool.Run();

                    if (bViewArea == true)
                    {
                        // 찾은 패턴 표시
                        //DrawCross(m_TranslationX, m_TranslationY, 5, 1, Display, CogColorConstants.Green, CogColorConstants.Black);
                        Display.StaticGraphics.Add((Cognex.VisionPro.ICogGraphic)m_CogPMAlignTool.Results[0].CreateResultGraphics(CogPMAlignResultGraphicConstants.BoundingBox | CogPMAlignResultGraphicConstants.Origin), "");
                    }

                }
                #endregion

                m_SearchArea = null;
            }
            catch (Exception ex)
            {

                Score = 0;
                ReadChar = "?";
                throw ex;
            }

            return Score;
        }
        /// <summary>
        /// 특정문자만 검사함...2011년9월7일 이기문
        /// </summary>
        /// <param name="Display">사용할 cogDisplay</param>
        /// <param name="bViewArea">검사영역 보이게 할지</param>
        /// <param name="FindChar">지정한 문자만 검사</param>
        /// <param name="InsAreaCount">한 검사영역에서 이 함수를 여러번 사용했을경우(for문을 돌림) 비슷한 패턴이 있으면 찾은 영역이 여러번 표시된다.
        /// 이를 해결 하기 위해 검사영역 번호를 스트링 형식으로 지정하면 Readchar+InsAreaCount형식으로 그룹이름이 지정되어 StaticGraphics를 사용하여 뿌려진다.</param>
        /// <returns>스코어값 리턴</returns>
        public double FindPattern(Cognex.VisionPro.Display.CogDisplay Display, bool bViewArea, string FindChar, string InsAreaCount)
        {
            // 현재 최고 점수
            double Score = 0;

            ReadChar = "?";

            #region 이미지 설정
            if (m_Image == null)
            {
                Write_SystemLog("이미지가 없습니다.-패턴매칭툴");
                return Score = 0;
            }


            m_CogPMAlignTool.InputImage = null;
            m_CogPMAlignTool.InputImage = (Cognex.VisionPro.CogImage8Grey)m_Image;
            #endregion

            #region 패턴 로딩 후 검사
            try
            {
                // 기준 위치로 검사 영역 이동
                if (m_Region.GetType().ToString().IndexOf("Rectangle") >= 0)
                {
                    m_RectangleRegion = null;
                    m_RectangleRegion = (CogRectangleAffine)m_Region;

                    if (m_CenterX > 0 && m_CenterY > 0)
                    {
                        m_RectangleRegion.CenterX = m_CenterX;
                        m_RectangleRegion.CenterY = m_CenterY;
                    }

                    m_Region = m_RectangleRegion;
                }
                else if (m_Region.GetType().ToString().IndexOf("Circular") >= 0)
                {
                    m_RingRegion = (CogCircularAnnulusSection)m_Region;

                    if (m_Radius == 0)
                        m_Radius = 1;

                    m_RingRegion.Radius = m_Radius;
                    m_Region = m_RingRegion;
                }
                else
                {
                    m_CircleRegion = null;
                    m_CircleRegion = (CogCircle)m_Region;

                    if (m_CenterX > 0 && m_CenterY > 0)
                    {
                        m_CircleRegion.CenterX = m_CenterX;
                        m_CircleRegion.CenterY = m_CenterY;
                    }

                    m_Region = m_CircleRegion;
                }
            }
            catch
            {
                Score = 0;
                ReadChar = "?";
                return Score;

            }

            m_CogPMAlignTool.SearchRegion = m_Region;
            m_CogPMAlignTool.RunParams.ContrastThreshold = m_ContrastThresh;

            double x1, x2, y1, y2;

            m_TranslationX = m_TranslationY = m_Rotation = 0;

            int Index = -1;

            try
            {
                for (int i = 0; i < m_Pattern.Count; i++)
                {
                    if (m_PatternName[i].ToString() == FindChar)
                    {
                        m_CogPMAlignTool.Pattern = null;
                        m_CogPMAlignTool.Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern)m_Pattern[i];

                        m_Region = null;

                        m_Region = m_CogPMAlignTool.SearchRegion;
                        //m_CogPMAlignTool.RunParams.AcceptThreshold = m_AcceptThershold / 100;

                        // 파일로부터 패턴 로드
                        //m_CogPMAlignTool.Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern) Cognex.VisionPro.CogSerializer.LoadObjectFromFile(filename);

                        //if (m_PatternShape == PATTERNSHAPE.Rectangle)
                        //{
                        //    m_PatternArea = null;
                        //    m_PatternArea = (CogRectangleAffine)m_CogPMAlignTool.Pattern.TrainRegion;
                        //}
                        //else if (m_PatternShape == PATTERNSHAPE.Circle)
                        //{
                        //    m_CircleArea = null;
                        //    m_CircleArea = (CogCircle)m_CogPMAlignTool.Pattern.TrainRegion;
                        //}

                        // 검사 수행
                        m_CogPMAlignTool.Run();

                        // 찾은 결과가 있고
                        if (m_CogPMAlignTool.Results != null && m_CogPMAlignTool.Results.Count > 0)
                        {
                            // 합격 점수 보다 높고, 최고 점수보다 높으면 찾은 패턴 정보 갱신.
                            if (m_CogPMAlignTool.Results[0].Score > Score)
                            {
                                m_FindPatternIndex = i;

                                if (m_PatternShape == PATTERNSHAPE.Rectangle)
                                {
                                    //찾은 패턴의 크기
                                    x1 = m_CogPMAlignTool.Results[0].GetPose().TranslationX - m_PatternArea.SideXLength / 2;
                                    y1 = m_CogPMAlignTool.Results[0].GetPose().TranslationY - m_PatternArea.SideYLength / 2;
                                    x2 = m_CogPMAlignTool.Results[0].GetPose().TranslationX + m_PatternArea.SideXLength / 2;
                                    y2 = m_CogPMAlignTool.Results[0].GetPose().TranslationY + m_PatternArea.SideYLength / 2;

                                    if (m_Region.GetType().ToString().IndexOf("Rectangle") >= 0)
                                    {
                                        //if (!(x1 < m_RectangleRegion.CornerOriginX || y1 < m_RectangleRegion.CornerOriginY || x2 > m_RectangleRegion.CornerOriginX + m_RectangleRegion.SideXLength || y2 > m_RectangleRegion.CornerOriginY + m_RectangleRegion.SideYLength))
                                        if (!(x1 < m_RectangleRegion.CenterX - m_RectangleRegion.SideXLength || y1 < m_RectangleRegion.CenterY - m_RectangleRegion.SideYLength || x2 > m_RectangleRegion.CenterX + m_RectangleRegion.SideXLength || y2 > m_RectangleRegion.CenterY + m_RectangleRegion.SideYLength))
                                        {
                                            Score = m_CogPMAlignTool.Results[0].Score;
                                            Index = i;
                                            m_TranslationX = ((int)m_CogPMAlignTool.Results[0].GetPose().TranslationX * 100) / 100;
                                            m_TranslationY = ((int)m_CogPMAlignTool.Results[0].GetPose().TranslationY * 100) / 100;
                                            m_Rotation = m_CogPMAlignTool.Results[0].GetPose().Rotation;
                                        }
                                    }
                                    else
                                    {
                                        Score = m_CogPMAlignTool.Results[0].Score;
                                        Index = i;
                                        m_TranslationX = ((int)m_CogPMAlignTool.Results[0].GetPose().TranslationX * 100) / 100;
                                        m_TranslationY = ((int)m_CogPMAlignTool.Results[0].GetPose().TranslationY * 100) / 100;
                                        m_Rotation = m_CogPMAlignTool.Results[0].GetPose().Rotation;
                                    }
                                }
                                else if (m_PatternShape == PATTERNSHAPE.Circle)
                                {
                                    // 패턴 영역이 검사 영역을 벗어날 경우 NG로 처리						
                                    //x1 = m_CogPMAlignTool.Results[0].GetPose().TranslationX - (m_CogPMAlignTool.Pattern.Origin.TranslationX - m_PatternArea.CenterX) - m_CircleArea.Radius;
                                    //y1 = m_CogPMAlignTool.Results[0].GetPose().TranslationY - (m_CogPMAlignTool.Pattern.Origin.TranslationY - m_PatternArea.CenterY) - m_CircleArea.Radius;
                                    //x2 = m_CogPMAlignTool.Results[0].GetPose().TranslationX - (m_CogPMAlignTool.Pattern.Origin.TranslationX - m_PatternArea.CenterX) + m_CircleArea.Radius;
                                    //y2 = m_CogPMAlignTool.Results[0].GetPose().TranslationY - (m_CogPMAlignTool.Pattern.Origin.TranslationY - m_PatternArea.CenterY) + m_CircleArea.Radius;

                                    x1 = m_CogPMAlignTool.Results[0].GetPose().TranslationX - m_CircleArea.Radius;
                                    y1 = m_CogPMAlignTool.Results[0].GetPose().TranslationY - m_CircleArea.Radius;
                                    x2 = m_CogPMAlignTool.Results[0].GetPose().TranslationX + m_CircleArea.Radius;
                                    y2 = m_CogPMAlignTool.Results[0].GetPose().TranslationY + m_CircleArea.Radius;

                                    //Score = m_CogPMAlignTool.Results[0].Score;
                                    //Index = i;
                                    //m_TranslationX = m_CogPMAlignTool.Results[0].GetPose().TranslationX;
                                    //m_TranslationY = m_CogPMAlignTool.Results[0].GetPose().TranslationY;

                                    if (!(x1 < m_RectangleRegion.CornerOriginX || y1 < m_RectangleRegion.CornerOriginY || x2 > m_RectangleRegion.CornerOriginX + m_RectangleRegion.SideXLength || y2 > m_RectangleRegion.CornerOriginY + m_RectangleRegion.SideYLength))
                                    {
                                        Score = m_CogPMAlignTool.Results[0].Score;
                                        Index = i; ;
                                        m_TranslationX = m_CogPMAlignTool.Results[0].GetPose().TranslationX;
                                        m_TranslationY = m_CogPMAlignTool.Results[0].GetPose().TranslationY;
                                        m_Rotation = m_CogPMAlignTool.Results[0].GetPose().Rotation;
                                    }
                                }
                                else
                                {
                                    Score = m_CogPMAlignTool.Results[0].Score;
                                    Index = i;
                                    m_TranslationX = m_CogPMAlignTool.Results[0].GetPose().TranslationX;
                                    m_TranslationY = m_CogPMAlignTool.Results[0].GetPose().TranslationY;
                                    m_Rotation = m_CogPMAlignTool.Results[0].GetPose().Rotation;
                                }
                            }
                        }
                    }
                    #endregion
                }

                #region 검사 결과 디스플레이
                //Display.StaticGraphics.Clear();

                // 검사 영역 표시
                if (bViewArea)
                {
                    DisplaySearchArea(Display, false);
                }

                // 찾은 결과가 있을 경우 찾은 영역, 점수 표시
                if (Index >= 0)
                {
                    m_CogPMAlignTool.Pattern = null;

                    // 가장 높은 점수로 찾은 패턴 다시 찾기
                    // 파일로부터 패턴 로드
                    m_CogPMAlignTool.Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern)m_Pattern[Index];

                    ReadChar = m_PatternName[Index].ToString();
                    // 검사 수행
                    m_CogPMAlignTool.Run();

                    if (bViewArea == true)
                    {
                        // 찾은 패턴 표시
                        //DrawCross(m_TranslationX, m_TranslationY, 5, 1, Display, CogColorConstants.Green, CogColorConstants.Black);
                        Display.StaticGraphics.Add((Cognex.VisionPro.ICogGraphic)m_CogPMAlignTool.Results[0].CreateResultGraphics(CogPMAlignResultGraphicConstants.BoundingBox | CogPMAlignResultGraphicConstants.Origin), ReadChar + InsAreaCount);
                    }

                }
                #endregion

                m_SearchArea = null;
            }
            catch (Exception ex)
            {

                Score = 0;
                ReadChar = "?";
                //throw ex;
            }

            return Score;
        }

        /// <summary>
        /// 시스템 로그 남기는 함수
        /// </summary>
        /// <param name="log">남길 내용</param>
        private void Write_SystemLog(string log)
        {
            FileStream wstream;
            StreamWriter writer;

            if (!Directory.Exists(System.Windows.Forms.Application.StartupPath + @"\System Log\"))
            {
                Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + @"\System Log\");
            }

            if (File.Exists(System.Windows.Forms.Application.StartupPath + @"\System Log\" + String.Format("{0:0000}년{1:00}월{2:00}일.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)) == false)
            {
                wstream = File.Create(System.Windows.Forms.Application.StartupPath + @"\System Log\" + String.Format("{0:0000}년{1:00}월{2:00}일.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                writer = new StreamWriter(wstream);
            }
            else
            {
                wstream = new FileStream(System.Windows.Forms.Application.StartupPath + @"\System Log\" + String.Format("{0:0000}년{1:00}월{2:00}일.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), FileMode.Append, FileAccess.Write);
                writer = new StreamWriter(wstream);
            }

            // 오더링 리스트 다른 곳에 저장...
            writer.WriteLine(DateTime.Now.ToLongTimeString() + "\t" + log);

            writer.Close();
            wstream.Close();

            writer = null;
            wstream = null;
        }

        /// <summary>
        /// 패턴 찾기.
        /// 지정된 폴더에서 필요한 정보를 로딩하여 검사를 수행함.
        /// 검사 영역, 합격 점수, 흑백 대비 값, 패턴 폴더 등이 지정되어 있음.
        /// </summary>
        /// <param name="Name">패턴 이름. "차종\카메라 이름\하위 이름" 형식을 취할 것.</param>
        /// <param name="Display">결과를 출력할 디스플레이</param>
        /// 검사 영역 표시까지 작업.
        /// 패턴 정보의 모양. 
        /// 실행파일 폴더\PMAlign\차종\카메라명\패턴명.ini - 환경변수
        /// 실행파일 폴더\PMAlign\차종\카메라명\패턴명\ - 각각에 대한 멀티 패턴 폴더
        public double FindPattern2(string Name, Cognex.VisionPro.Display.CogDisplay Display, bool bViewArea)
        {
            // 현재 최고 점수
            double Score = 0;
            // 찾은 패턴 이름
            string FindName = "";

            #region 이미지 설정
            if (m_Image == null)
                throw new Exception("검사 할 이미지가 없습니다.");

            m_CogPMAlignTool.InputImage = null;
            m_CogPMAlignTool.InputImage = (Cognex.VisionPro.CogImage8Grey)m_Image;
            #endregion

            #region 패턴 로딩 후 검사
            if (Directory.Exists(Name))
            {
                // 기준 위치로 검사 영역 이동
                if (m_RegionShape == RegionShape.Rectangle)
                {
                    m_RectangleRegion = null;
                    m_RectangleRegion = (CogRectangleAffine)m_SearchArea;

                    if (m_CenterX > 0 && m_CenterY > 0)
                    {
                        m_RectangleRegion.CenterX = m_CenterX;
                        m_RectangleRegion.CenterY = m_CenterY;
                    }

                    m_Region = m_RectangleRegion;
                }
                //else(m_RegionShape==RegionShape.Circle)
                else
                {
                    m_CircleRegion = null;
                    m_CircleRegion = (CogCircle)m_Region;

                    if (m_CenterX > 0 && m_CenterY > 0)
                    {
                        m_CircleRegion.CenterX = m_CenterX;
                        m_CircleRegion.CenterY = m_CenterY;
                    }

                    m_Region = m_CircleRegion;
                }

                m_CogPMAlignTool.SearchRegion = m_Region;

                string[] files = Directory.GetFiles(Name);

                double x1, x2, y1, y2;

                m_TranslationX = m_TranslationY = m_Rotation = 0;

                FindName = "";

                try
                {
                    foreach (string filename in files)
                    {
                        try
                        {
                            //string Pattern = null;

                            //FileStream rstream;
                            //StreamReader reader;

                            //rstream = File.Open(filename, FileMode.Open);
                            //reader = new StreamReader(rstream);

                            //Pattern = reader.ReadToEnd();

                            //reader.Close();
                            //reader.Close();

                            //reader = null;
                            //rstream = null;

                            m_CogPMAlignTool.Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern)Cognex.VisionPro.CogSerializer.LoadObjectFromString(filename);

                            m_Region = null;

                            m_Region = m_CogPMAlignTool.SearchRegion;
                            m_AcceptThershold = m_CogPMAlignTool.RunParams.AcceptThreshold;

                            // 파일로부터 패턴 로드
                            //m_CogPMAlignTool.Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern) Cognex.VisionPro.CogSerializer.LoadObjectFromFile(filename);
                        }
                        catch (Exception ex)
                        {
                            string s = ex.Message;

                            continue;
                        }

                        if (m_PatternShape == PATTERNSHAPE.Rectangle)
                        {
                            m_PatternArea = null;
                            m_PatternArea = (CogRectangleAffine)m_CogPMAlignTool.Pattern.TrainRegion;
                        }
                        else if (m_PatternShape == PATTERNSHAPE.Circle)
                        {
                            m_CircleArea = null;
                            m_CircleArea = (CogCircle)m_CogPMAlignTool.Pattern.TrainRegion;
                        }

                        // 검사 수행
                        m_CogPMAlignTool.Run();

                        // 찾은 결과가 있고
                        if (m_CogPMAlignTool.Results != null && m_CogPMAlignTool.Results.Count > 0)
                        {
                            // 합격 점수 보다 높고, 최고 점수보다 높으면 찾은 패턴 정보 갱신.
                            if (m_CogPMAlignTool.Results[0].Score > Score)
                            {
                                if (m_PatternShape == PATTERNSHAPE.Rectangle)
                                {
                                    m_SearchArea = (CogRectangleAffine)m_Region;

                                    x1 = m_CogPMAlignTool.Results[0].GetPose().TranslationX - m_PatternArea.SideXLength / 2;
                                    y1 = m_CogPMAlignTool.Results[0].GetPose().TranslationY - m_PatternArea.SideYLength / 2;
                                    x2 = m_CogPMAlignTool.Results[0].GetPose().TranslationX + m_PatternArea.SideXLength / 2;
                                    y2 = m_CogPMAlignTool.Results[0].GetPose().TranslationY + m_PatternArea.SideYLength / 2;

                                    if (!(x1 < m_SearchArea.CornerOriginX || y1 < m_SearchArea.CornerOriginY || x2 > m_SearchArea.CornerOriginX + m_SearchArea.SideXLength || y2 > m_SearchArea.CornerOriginY + m_SearchArea.SideYLength))
                                    {
                                        Score = m_CogPMAlignTool.Results[0].Score;
                                        FindName = filename;
                                        m_TranslationX = ((int)m_CogPMAlignTool.Results[0].GetPose().TranslationX * 100) / 100;
                                        m_TranslationY = ((int)m_CogPMAlignTool.Results[0].GetPose().TranslationY * 100) / 100;
                                        m_Rotation = m_CogPMAlignTool.Results[0].GetPose().Rotation;
                                    }
                                }
                                else if (m_PatternShape == PATTERNSHAPE.Circle)
                                {
                                    // 패턴 영역이 검사 영역을 벗어날 경우 NG로 처리						
                                    //							x1 = m_CogPMAlignTool.Results[0].GetPose().TranslationX - (m_CogPMAlignTool.Pattern.Origin.TranslationX - m_PatternArea.CenterX) - m_CircleArea.Radius;
                                    //							y1 = m_CogPMAlignTool.Results[0].GetPose().TranslationY - (m_CogPMAlignTool.Pattern.Origin.TranslationY - m_PatternArea.CenterY) - m_CircleArea.Radius;
                                    //							x2 = m_CogPMAlignTool.Results[0].GetPose().TranslationX - (m_CogPMAlignTool.Pattern.Origin.TranslationX - m_PatternArea.CenterX) + m_CircleArea.Radius;
                                    //							y2 = m_CogPMAlignTool.Results[0].GetPose().TranslationY - (m_CogPMAlignTool.Pattern.Origin.TranslationY - m_PatternArea.CenterY) + m_CircleArea.Radius;

                                    x1 = m_CogPMAlignTool.Results[0].GetPose().TranslationX - m_CircleArea.Radius;
                                    y1 = m_CogPMAlignTool.Results[0].GetPose().TranslationY - m_CircleArea.Radius;
                                    x2 = m_CogPMAlignTool.Results[0].GetPose().TranslationX + m_CircleArea.Radius;
                                    y2 = m_CogPMAlignTool.Results[0].GetPose().TranslationY + m_CircleArea.Radius;

                                    Score = m_CogPMAlignTool.Results[0].Score;
                                    FindName = filename;
                                    m_TranslationX = m_CogPMAlignTool.Results[0].GetPose().TranslationX;
                                    m_TranslationY = m_CogPMAlignTool.Results[0].GetPose().TranslationY;
                                    m_Rotation = m_CogPMAlignTool.Results[0].GetPose().Rotation;

                                    //								if(!(x1 < m_SearchArea.CornerOriginX || y1 < m_SearchArea.CornerOriginY || x2 > m_SearchArea.CornerOriginX + m_SearchArea.SideXLength || y2 > m_SearchArea.CornerOriginY + m_SearchArea.SideYLength))
                                    //								{
                                    //									Score = m_CogPMAlignTool.Results[0].Score;
                                    //									FindName = filename;
                                    //									m_TranslationX =  m_CogPMAlignTool.Results[0].GetPose().TranslationX;
                                    //									m_TranslationY =  m_CogPMAlignTool.Results[0].GetPose().TranslationY;
                                    //								}
                                }
                                else
                                {
                                    Score = m_CogPMAlignTool.Results[0].Score;
                                    FindName = filename;
                                    m_TranslationX = m_CogPMAlignTool.Results[0].GetPose().TranslationX;
                                    m_TranslationY = m_CogPMAlignTool.Results[0].GetPose().TranslationY;
                                    m_Rotation = m_CogPMAlignTool.Results[0].GetPose().Rotation;
                                }
                            }
                        }
                        #endregion
                    }

                    files = null;

                    #region 검사 결과 디스플레이
                    Cognex.VisionPro.ICogGraphic ResultGp;
                    //Display.StaticGraphics.Clear();

                    // 검사 영역 표시
                    if (bViewArea)
                    {
                        DisplaySearchArea(Display, false);
                    }

                    // 찾은 결과가 있을 경우 찾은 영역, 점수 표시
                    if (FindName != "")
                    {
                        m_CogPMAlignTool.Pattern = null;

                        // 가장 높은 점수로 찾은 패턴 다시 찾기
                        // 파일로부터 패턴 로드
                        m_CogPMAlignTool.Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern)Cognex.VisionPro.CogSerializer.LoadObjectFromFile(FindName);
                        // 검사 수행
                        m_CogPMAlignTool.Run();
                        ResultGp = (Cognex.VisionPro.ICogGraphic)m_CogPMAlignTool.Results[0].CreateResultGraphics(CogPMAlignResultGraphicConstants.BoundingBox | CogPMAlignResultGraphicConstants.Origin);
                        ResultGp.Color = Cognex.VisionPro.CogColorConstants.Green;

                        // 찾은 패턴 표시
                        Display.StaticGraphics.Add(ResultGp, "");

                        ResultGp = null;
                    }
                    #endregion

                    files = null;

                    m_SearchArea = null;
                    //m_CogPMAlignTool = null;
                }
                catch (Exception ex)
                {
                    Score = 0;

                    throw ex;
                }
            }

            return Score;
        }

        public void Show_Pattern(string filename, Cognex.VisionPro.Display.CogDisplay Display)
        {
            // PMAlign Tool
            CogPMAlignTool m_CogPMAlignTool = new CogPMAlignTool();

            // 파일로부터 패턴 로드
            m_CogPMAlignTool.Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern)Cognex.VisionPro.CogSerializer.LoadObjectFromFile(filename);

            //Display.Image = m_CogPMAlignTool.Pattern.GetTrainedPatternImage();
            Display.Image = m_CogPMAlignTool.Pattern.GetTrainedPatternImageMask();
        }

        public void Show_Pattern(int Index, Cognex.VisionPro.Display.CogDisplay Display)
        {
            Cognex.VisionPro.PMAlign.CogPMAlignPattern Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern)m_Pattern[Index];

            Display.Image = Pattern.GetTrainedPatternImage();
            Display.StaticGraphics.Add((Cognex.VisionPro.ICogGraphic)Pattern.CreateGraphicsCoarse(CogColorConstants.Yellow)[0], "");
            Display.StaticGraphics.Add((Cognex.VisionPro.ICogGraphic)Pattern.CreateGraphicsFine(CogColorConstants.Green)[0], "");
            //Display.Image = m_CogPMAlignTool.Pattern.GetTrainedPatternImageMask();  




        }

        //폴더 내에 hsr파일 로드 하는 부분->콤보박스로...그리고 ArrayList로...
        public void Load_Pattern(string PathName, ComboBox cb_Name)
        {

            string[] files;
            //ArrayList에 담긴 패턴들을 클리어 시키기
            try
            {

                Clear_Pattern();

                //콤보박스 내용 클리어 시키기
                cb_Name.Items.Clear();

                if (Directory.Exists(PathName))
                {
                    files = Directory.GetFiles(PathName);

                    foreach (string name in files)
                    {
                        Cognex.VisionPro.PMAlign.CogPMAlignPattern Pattern;

                        Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern)Cognex.VisionPro.CogSerializer.LoadObjectFromFile(name);
                        //FileStream fs = new FileStream(name, FileMode.Open);
                        //Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern)Cognex.VisionPro.CogSerializer.LoadObjectFromStream(fs);
                        //fs.Close();

                        string[] str = name.Split('\\');

                        string FDName = str[str.Length - 2];
                        //패턴을 추가하기
                        Add_Pattern(Pattern, FDName);
                        //콤보박스에 이름 추가하기
                        cb_Name.Items.Add(name.Substring(name.Length - 7, 3));


                    }
                }
            }
            catch { }

        }

        //폴더 내에 hsr파일 로드 하는 부분->콤보박스로...그리고 ArrayList로...
        public void Load_Pattern(string PathName, List<string> list)
        {

            string[] files;
            //ArrayList에 담긴 패턴들을 클리어 시키기
            try
            {

                Clear_Pattern();

                //콤보박스 내용 클리어 시키기
                list.Clear();

                if (Directory.Exists(PathName))
                {
                    files = Directory.GetFiles(PathName);

                    foreach (string name in files)
                    {
                        Cognex.VisionPro.PMAlign.CogPMAlignPattern Pattern;

                        Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern)Cognex.VisionPro.CogSerializer.LoadObjectFromFile(name);
                        //FileStream fs = new FileStream(name, FileMode.Open);
                        //Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern)Cognex.VisionPro.CogSerializer.LoadObjectFromStream(fs);
                        //fs.Close();

                        string[] str = name.Split('\\');

                        string FDName = str[str.Length - 2];
                        //패턴을 추가하기
                        Add_Pattern(Pattern, FDName);
                        //콤보박스에 이름 추가하기
                        list.Add(name.Substring(name.Length - 7, 3));


                    }
                }
            }
            catch { }

        }


        //폴더 내에 hsr파일 로드 하는 부분->그리고 ArrayList로...
        public void Load_Pattern(string PathName, bool Clear)
        {
            string[] files;
            if (Clear == true)
            {
                //ArrayList에 담긴 패턴들을 클리어 시키기
                Clear_Pattern();
            }
            string[] str = PathName.Split('\\');

            string FDName = str[str.Length - 2];

            if (Directory.Exists(PathName))
            {
                files = Directory.GetFiles(PathName);

                foreach (string name in files)
                {
                    Cognex.VisionPro.PMAlign.CogPMAlignPattern Pattern;

                    Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern)Cognex.VisionPro.CogSerializer.LoadObjectFromFile(name);
                    //FileStream fs = new FileStream(name, FileMode.Open);
                    //Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern)Cognex.VisionPro.CogSerializer.LoadObjectFromStream(fs);
                    //fs.Close();
                    //패턴을 추가하기
                    Add_Pattern(Pattern, FDName);

                }
            }

        }

        /// <summary>
        /// 패턴 저장하는 함수
        /// </summary>
        /// <param name="Name">000.hsr,001.hsr등등 패턴 파일 이름 지정</param>
        public void Set_Pattern(string Name)
        {
            CogPMAlignTool m_PatternTool = new CogPMAlignTool();
            // 캡춰된 이미지 설정
            if (m_Image == null)
                throw new Exception("검사 할 이미지가 없습니다.");


            m_PatternTool.InputImage = (Cognex.VisionPro.CogImage8Grey)m_Image;

            m_PatternTool.Pattern.TrainImageMask = m_MaskImage;
            m_PatternTool.Pattern.TrainImage = (Cognex.VisionPro.CogImage8Grey)m_Image;

            if (m_PatternShape == PATTERNSHAPE.Rectangle)
            {
                m_PatternTool.Pattern.TrainRegion = (Cognex.VisionPro.ICogRegion)m_PatternArea;
                //2011.1.20 추가해줌
                m_PatternTool.Pattern.TrainRegionMode = CogRegionModeConstants.PixelAlignedBoundingBoxAdjustMask;


                m_PatternTool.Pattern.Origin.TranslationX = m_PatternArea.CenterX;
                m_PatternTool.Pattern.Origin.TranslationY = m_PatternArea.CenterY;
            }
            else if (m_PatternShape == PATTERNSHAPE.Circle)
            {
                m_PatternTool.Pattern.TrainRegion = (Cognex.VisionPro.ICogRegion)m_CircleArea;
                //2012.2.20 추가해줌
                m_PatternTool.Pattern.TrainRegionMode = CogRegionModeConstants.PixelAlignedBoundingBoxAdjustMask;

                m_PatternTool.Pattern.Origin.TranslationX = m_CircleArea.CenterX;
                m_PatternTool.Pattern.Origin.TranslationY = m_CircleArea.CenterY;
            }
            else
            {
                m_PatternTool.Pattern.TrainRegion = (Cognex.VisionPro.ICogRegion)m_PolygonArea;
                //2012.2.20 추가해줌
                m_PatternTool.Pattern.TrainRegionMode = CogRegionModeConstants.PixelAlignedBoundingBoxAdjustMask;
                double x, y;

                m_PolygonArea.ArcCenter(out x, out y);
                m_PatternTool.Pattern.Origin.TranslationX = x;
                m_PatternTool.Pattern.Origin.TranslationY = y;
            }



            m_PatternTool.Pattern.Train();

            if (m_PatternTool.Pattern.Trained == true)
            {
                //m_PatternTool.Pattern.TrainImageMask = m_PatternTool.Pattern.TrainImage;

                //for (int j = (int)m_PatternArea.CornerOriginY + 20; j < m_PatternArea.CornerOppositeY - 20; j++)
                //{
                //    for (int i = (int)m_PatternArea.CornerOriginX + 20; i < m_PatternArea.CornerOppositeX - 20; i++)
                //    {
                //        m_PatternTool.Pattern.TrainImageMask.SetPixel(i, j, 0);
                //    }
                //}
                //m_PatternTool.Pattern.Train();

                string[] str = Name.Split('\\');

                string FDName = str[str.Length - 2];

                Cognex.VisionPro.CogSerializer.SaveObjectToFile(m_PatternTool.Pattern, Name);

                Add_Pattern(m_PatternTool.Pattern, FDName);
            }
            else
            {
                throw new Exception("패턴 설정을 실패 하였습니다.");
            }
        }

        /// <summary>
        /// 패턴 저장하는 함수
        /// </summary>
        /// <param name="Name">000.hsr,001.hsr등등 패턴 파일 이름 지정</param>
        public void Set_Pattern(string Name, int pointX, int pointY)
        {
            CogPMAlignTool m_PatternTool = new CogPMAlignTool();
            // 캡춰된 이미지 설정
            if (m_Image == null)
                throw new Exception("검사 할 이미지가 없습니다.");


            m_PatternTool.InputImage = (Cognex.VisionPro.CogImage8Grey)m_Image;

            m_PatternTool.Pattern.TrainImageMask = m_MaskImage;
            m_PatternTool.Pattern.TrainImage = (Cognex.VisionPro.CogImage8Grey)m_Image;

            if (m_PatternShape == PATTERNSHAPE.Rectangle)
            {
                m_PatternTool.Pattern.TrainRegion = (Cognex.VisionPro.ICogRegion)m_PatternArea;
                //2011.1.20 추가해줌
                m_PatternTool.Pattern.TrainRegionMode = CogRegionModeConstants.PixelAlignedBoundingBoxAdjustMask;


                m_PatternTool.Pattern.Origin.TranslationX = pointX;
                m_PatternTool.Pattern.Origin.TranslationY = pointY;
            }
            else if (m_PatternShape == PATTERNSHAPE.Circle)
            {
                m_PatternTool.Pattern.TrainRegion = (Cognex.VisionPro.ICogRegion)m_CircleArea;
                //2012.2.20 추가해줌
                m_PatternTool.Pattern.TrainRegionMode = CogRegionModeConstants.PixelAlignedBoundingBoxAdjustMask;

                m_PatternTool.Pattern.Origin.TranslationX = m_CircleArea.CenterX;
                m_PatternTool.Pattern.Origin.TranslationY = m_CircleArea.CenterY;
            }
            else
            {
                m_PatternTool.Pattern.TrainRegion = (Cognex.VisionPro.ICogRegion)m_PolygonArea;
                //2012.2.20 추가해줌
                m_PatternTool.Pattern.TrainRegionMode = CogRegionModeConstants.PixelAlignedBoundingBoxAdjustMask;
                double x, y;

                m_PolygonArea.ArcCenter(out x, out y);
                m_PatternTool.Pattern.Origin.TranslationX = x;
                m_PatternTool.Pattern.Origin.TranslationY = y;
            }



            m_PatternTool.Pattern.Train();

            if (m_PatternTool.Pattern.Trained == true)
            {
                //m_PatternTool.Pattern.TrainImageMask = m_PatternTool.Pattern.TrainImage;

                //for (int j = (int)m_PatternArea.CornerOriginY + 20; j < m_PatternArea.CornerOppositeY - 20; j++)
                //{
                //    for (int i = (int)m_PatternArea.CornerOriginX + 20; i < m_PatternArea.CornerOppositeX - 20; i++)
                //    {
                //        m_PatternTool.Pattern.TrainImageMask.SetPixel(i, j, 0);
                //    }
                //}
                //m_PatternTool.Pattern.Train();

                string[] str = Name.Split('\\');

                string FDName = str[str.Length - 2];

                Cognex.VisionPro.CogSerializer.SaveObjectToFile(m_PatternTool.Pattern, Name);

                Add_Pattern(m_PatternTool.Pattern, FDName);
            }
            else
            {
                throw new Exception("패턴 설정을 실패 하였습니다.");
            }
        }
        /// <summary>
        /// 패턴저장하는 함수
        /// </summary>
        /// <param name="Name">패턴 파일 이름</param>
        /// <param name="PointUse">포인트 사용 유무</param>
        public void Set_Pattern(string Name, bool PointUse)
        {
            CogPMAlignTool m_PatternTool = new CogPMAlignTool();
            // 캡춰된 이미지 설정
            if (m_Image == null)
                throw new Exception("검사 할 이미지가 없습니다.");

            m_PatternTool.InputImage = (Cognex.VisionPro.CogImage8Grey)m_Image;

            m_PatternTool.Pattern.TrainImageMask = m_MaskImage;
            m_PatternTool.Pattern.TrainImage = (Cognex.VisionPro.CogImage8Grey)m_Image;

            if (m_PatternShape == PATTERNSHAPE.Rectangle)
            {
                //2011.1.20 추가해줌
                m_PatternTool.Pattern.TrainRegionMode = CogRegionModeConstants.PixelAlignedBoundingBoxAdjustMask;
                m_PatternTool.Pattern.TrainRegion = (Cognex.VisionPro.ICogRegion)m_PatternArea;

                if (PointUse == true)
                {
                    m_PatternTool.Pattern.Origin.TranslationX = m_PointPosition.X;
                    m_PatternTool.Pattern.Origin.TranslationY = m_PointPosition.Y;
                }
                else
                {

                    m_PatternTool.Pattern.Origin.TranslationX = m_PatternArea.CenterX;
                    m_PatternTool.Pattern.Origin.TranslationY = m_PatternArea.CenterY;
                }
            }
            else if (m_PatternShape == PATTERNSHAPE.Circle)
            {
                //2012.2.20 추가해줌
                m_PatternTool.Pattern.TrainRegionMode = CogRegionModeConstants.PixelAlignedBoundingBoxAdjustMask;

                m_PatternTool.Pattern.TrainRegion = (Cognex.VisionPro.ICogRegion)m_CircleArea;

                m_PatternTool.Pattern.Origin.TranslationX = m_CircleArea.CenterX;
                m_PatternTool.Pattern.Origin.TranslationY = m_CircleArea.CenterY;
            }
            else
            {
                //2012.2.20 추가해줌
                m_PatternTool.Pattern.TrainRegionMode = CogRegionModeConstants.PixelAlignedBoundingBoxAdjustMask;

                m_PatternTool.Pattern.TrainRegion = (Cognex.VisionPro.ICogRegion)m_PolygonArea;

                double x, y;

                m_PolygonArea.ArcCenter(out x, out y);
                m_PatternTool.Pattern.Origin.TranslationX = x;
                m_PatternTool.Pattern.Origin.TranslationY = y;
            }


            m_PatternTool.Pattern.Train();

            if (m_PatternTool.Pattern.Trained == true)
            {
                //m_PatternTool.Pattern.TrainImageMask = m_PatternTool.Pattern.TrainImage;

                //for (int j = (int)m_PatternArea.CornerOriginY + 20; j < m_PatternArea.CornerOppositeY - 20; j++)
                //{
                //    for (int i = (int)m_PatternArea.CornerOriginX + 20; i < m_PatternArea.CornerOppositeX - 20; i++)
                //    {
                //        m_PatternTool.Pattern.TrainImageMask.SetPixel(i, j, 0);
                //    }
                //}
                //m_PatternTool.Pattern.Train();

                string[] str = Name.Split('\\');

                string FDName = str[str.Length - 2];

                Cognex.VisionPro.CogSerializer.SaveObjectToFile(m_PatternTool.Pattern, Name);

                Add_Pattern(m_PatternTool.Pattern, FDName);
            }
            else
            {
                throw new Exception("패턴 설정을 실패 하였습니다.");
            }
        }

        /// <summary>
        /// 패턴 영역을 설정하기 위해서 디폴트 영역 표시
        /// </summary>
        /// <param name="Display">영역을 표시할 디스플레이</param>
        public void Display_PatternArea(Cognex.VisionPro.Display.CogDisplay Display)
        {
            //우선 기존 화면 클리어 하고...
            Display.InteractiveGraphics.Clear();
            Display.StaticGraphics.Clear();

            if (this.PatternShape == PATTERNSHAPE.Rectangle)
            {
                m_PatternArea.CenterX = 50;
                m_PatternArea.CenterY = 50;
                m_PatternArea.SideXLength = 50;
                m_PatternArea.SideYLength = 50;
                m_PatternArea.Rotation = 0;
                m_PatternArea.GraphicDOFEnable = Cognex.VisionPro.CogRectangleAffineDOFConstants.All;

                m_PatternArea.Interactive = true;

                Display.InteractiveGraphics.Add(m_PatternArea, "", true);
            }
            else if (this.PatternShape == PATTERNSHAPE.Circle)
            {
                m_CircleArea.CenterX = 320;
                m_CircleArea.CenterY = 240;
                m_CircleArea.Radius = 50;
                m_CircleArea.GraphicDOFEnable = Cognex.VisionPro.CogCircleDOFConstants.All;
                m_CircleArea.Interactive = true;

                Display.InteractiveGraphics.Add(m_CircleArea, "", true);
            }
            else
            {
                int NumVertices = m_PolygonArea.NumVertices;

                for (int i = 0; i < NumVertices; i++)
                {
                    m_PolygonArea.RemoveVertex(0);
                }

                for (int i = 0; i < 5; i++)
                {
                    m_PolygonArea.AddVertex(200, 200 + 20 * i, i);
                }

                for (int i = 0; i < 5; i++)
                {
                    m_PolygonArea.AddVertex(300, 280 - 20 * i, i + 5);
                }

                m_PolygonArea.GraphicDOFEnable = Cognex.VisionPro.CogPolygonDOFConstants.All;
                m_PolygonArea.Interactive = true;

                Display.InteractiveGraphics.Add(m_PolygonArea, "", true);
            }
        }
        //포인트 표시하기...
        public void Display_PatternArea(Cognex.VisionPro.Display.CogDisplay Display, bool Point_Use)
        {
            //우선 기존 화면 클리어 하고...
            Display.InteractiveGraphics.Clear();
            Display.StaticGraphics.Clear();

            if (this.PatternShape == PATTERNSHAPE.Rectangle)
            {
                m_PatternArea.CenterX = 320;
                m_PatternArea.CenterY = 240;
                m_PatternArea.SideXLength = 50;
                m_PatternArea.SideYLength = 50;
                m_PatternArea.Rotation = 0;
                m_PatternArea.GraphicDOFEnable = Cognex.VisionPro.CogRectangleAffineDOFConstants.All;
                m_PatternArea.Interactive = true;

                if (Point_Use == true)
                {
                    m_PointPosition.X = m_PatternArea.CenterX;
                    m_PointPosition.Y = m_PatternArea.CenterY;
                    m_PointPosition.GraphicDOFEnable = Cognex.VisionPro.CogPointMarkerDOFConstants.All;
                    m_PointPosition.Interactive = true;
                    Display.InteractiveGraphics.Add(m_PointPosition, "", true);
                }
                Display.InteractiveGraphics.Add(m_PatternArea, "", true);

            }
            else if (this.PatternShape == PATTERNSHAPE.Circle)
            {
                m_CircleArea.CenterX = 320;
                m_CircleArea.CenterY = 240;
                m_CircleArea.Radius = 50;
                m_CircleArea.GraphicDOFEnable = Cognex.VisionPro.CogCircleDOFConstants.All;
                m_CircleArea.Interactive = true;

                Display.InteractiveGraphics.Add(m_CircleArea, "", true);
            }
            else
            {
                int NumVertices = m_PolygonArea.NumVertices;

                for (int i = 0; i < NumVertices; i++)
                {
                    m_PolygonArea.RemoveVertex(0);
                }

                for (int i = 0; i < 5; i++)
                {
                    m_PolygonArea.AddVertex(200, 200 + 20 * i, i);
                }

                for (int i = 0; i < 5; i++)
                {
                    m_PolygonArea.AddVertex(300, 280 - 20 * i, i + 5);
                }

                m_PolygonArea.GraphicDOFEnable = Cognex.VisionPro.CogPolygonDOFConstants.All;
                m_PolygonArea.Interactive = true;

                Display.InteractiveGraphics.Add(m_PolygonArea, "", true);
            }
        }

        public void Display_PatternArea(Cognex.VisionPro.Display.CogDisplay Display, int SizeX, int SizeY)
        {
            if (this.PatternShape == PATTERNSHAPE.Rectangle)
            {
                m_PatternArea.CenterX = 320;
                m_PatternArea.CenterY = 240;
                m_PatternArea.SideXLength = SizeX;
                m_PatternArea.SideYLength = SizeY;
                m_PatternArea.Rotation = 0;
                m_PatternArea.GraphicDOFEnable = Cognex.VisionPro.CogRectangleAffineDOFConstants.All;
                m_PatternArea.Interactive = true;

                Display.InteractiveGraphics.Add(m_PatternArea, "", true);
            }
            else if (this.PatternShape == PATTERNSHAPE.Circle)
            {
                m_CircleArea.CenterX = 320;
                m_CircleArea.CenterY = 240;
                m_CircleArea.Radius = SizeX;
                m_CircleArea.GraphicDOFEnable = Cognex.VisionPro.CogCircleDOFConstants.All;
                m_CircleArea.Interactive = true;

                Display.InteractiveGraphics.Add(m_CircleArea, "", true);
            }
            else
            {
                int NumVertices = m_PolygonArea.NumVertices;

                for (int i = 0; i < NumVertices; i++)
                {
                    m_PolygonArea.RemoveVertex(0);
                }

                for (int i = 0; i < 5; i++)
                {
                    m_PolygonArea.AddVertex(200, 200 + 20 * i, i);
                }

                for (int i = 0; i < 5; i++)
                {
                    m_PolygonArea.AddVertex(300, 280 - 20 * i, i + 5);
                }

                m_PolygonArea.GraphicDOFEnable = Cognex.VisionPro.CogPolygonDOFConstants.All;
                m_PolygonArea.Interactive = true;

                Display.InteractiveGraphics.Add(m_PolygonArea, "", true);
            }
        }

        public void ReleaseTool()
        {
            if (m_CogPMAlignTool != null)
            {
                m_CogPMAlignTool.Dispose();
                m_CogPMAlignTool = null;
            }
        }

        public void Clear_Pattern()
        {
            //Arraylist에 담긴 패턴 클리어
            m_Pattern.Clear();
            //Arraylist에 담긴 패턴이름 클리어
            m_PatternName.Clear();
        }

        public void Add_Pattern(Cognex.VisionPro.PMAlign.CogPMAlignPattern Pattern)
        {
            m_Pattern.Add(Pattern);
        }
        public void Add_Pattern(Cognex.VisionPro.PMAlign.CogPMAlignPattern Pattern, string PatternName)
        {
            m_Pattern.Add(Pattern);
            //폴더 이름을 Arraylist에 담기...
            m_PatternName.Add(PatternName);
        }


        public void Delete_Pattern(int Index)
        {
            m_Pattern.RemoveAt(Index);
            m_PatternName.RemoveAt(Index);
        }

        //public void LoadTool(string Path,RegionShape Shape)
        //{
        //    try
        //    {
        //        m_CogPMAlignTool = (Cognex.VisionPro.PMAlign.CogPMAlignTool) Cognex.VisionPro.CogSerializer.LoadObjectFromFile(Path);

        //        m_Region = null;

        //        m_Region = m_CogPMAlignTool.SearchRegion;
        //        m_AcceptThershold = m_CogPMAlignTool.RunParams.AcceptThreshold;
        //        m_ContrastThresh = (int) m_CogPMAlignTool.RunParams.ContrastThreshold;
        //        m_Angle = m_CogPMAlignTool.RunParams.ZoneAngle.High;
        //    }
        //    catch
        //    {
        //        m_CogPMAlignTool = new CogPMAlignTool();

        //        m_Region = null;
        //        //로드시 파일이 없을 경우 형태를 지정하여 넣게 하기 위해
        //        m_RegionShape = Shape;

        //        if(m_RegionShape==RegionShape.Rectangle)
        //        {
        //            m_Region = new CogRectangleAffine();
        //        }
        //        else if(m_RegionShape==RegionShape.Circle)
        //        {
        //            m_Region = new CogCircle();
        //        }
        //        else if (m_RegionShape == RegionShape.Polygon)
        //        {
        //            m_Region = (ICogRegion)new CogPolygon();
        //        }
        //        else
        //        {
        //            m_Region = new CogCircularAnnulusSection();
        //        }

        //        m_CogPMAlignTool.SearchRegion = m_Region;

        //        m_AcceptThershold = 0.6;

        //        SaveTool(Path);
        //    }
        //}
        public void LoadTool(string Path)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(Path);

                if (!Directory.Exists(fileInfo.DirectoryName) || !File.Exists(Path))
                {
                    m_CogPMAlignTool = new CogPMAlignTool();

                    m_Region = null;

                    if (m_RegionShape == RegionShape.Rectangle)
                    {
                        m_Region = new CogRectangleAffine();
                    }
                    else if (m_RegionShape == RegionShape.Circle)
                    {
                        m_Region = new CogCircle();
                    }
                    else if (m_RegionShape == RegionShape.Polygon)
                    {
                        m_Region = (ICogRegion)new CogPolygon();
                    }
                    else
                    {
                        m_Region = new CogCircularAnnulusSection();
                    }

                    m_CogPMAlignTool.SearchRegion = m_Region;

                    m_AcceptThershold = 0.6;

                    SaveTool(Path);
                }
                else
                {
                    m_CogPMAlignTool = (Cognex.VisionPro.PMAlign.CogPMAlignTool)LoadSerialzed(Path);

                    m_Region = null;
                    m_Region = m_CogPMAlignTool.SearchRegion;
                    m_AcceptThershold = m_CogPMAlignTool.RunParams.AcceptThreshold;
                    m_ContrastThresh = (int)m_CogPMAlignTool.RunParams.ContrastThreshold;

                    //2012년 1월3일 각도 로드 안되는 부분 수정함
                    if (m_CogPMAlignTool.RunParams.ZoneAngle.Configuration == Cognex.VisionPro.PMAlign.CogPMAlignZoneConstants.LowHigh)
                    {
                        m_Angle = m_CogPMAlignTool.RunParams.ZoneAngle.High;
                    }
                    else
                    {
                        m_Angle = 0;
                    }
                    //2012년 2월16일 XY스케일 로드 안되는 부분 수정함<<------------------------------------------------------------------
                    if (m_CogPMAlignTool.RunParams.ZoneScaleX.Configuration == Cognex.VisionPro.PMAlign.CogPMAlignZoneConstants.LowHigh)
                    {
                        if (m_CogPMAlignTool.RunParams.ZoneScaleX.High > 1)
                        {
                            m_XScale = m_CogPMAlignTool.RunParams.ZoneScaleX.High - 1;
                        }
                        else
                        {
                            m_XScale = 0;
                        }
                    }
                    else
                    {
                        m_XScale = 0;
                    }
                    if (m_CogPMAlignTool.RunParams.ZoneScaleY.Configuration == Cognex.VisionPro.PMAlign.CogPMAlignZoneConstants.LowHigh)
                    {
                        if (m_CogPMAlignTool.RunParams.ZoneScaleY.High > 1)
                        {
                            m_YScale = m_CogPMAlignTool.RunParams.ZoneScaleY.High - 1;
                        }
                        else
                        {
                            m_YScale = 0;
                        }
                    }
                    else
                    {
                        m_YScale = 0;
                    }

                    //2012년 2월16일 이기문 Elassticity추가(패턴 탄력성주기위해 추가)
                    m_Elasticity = m_CogPMAlignTool.Pattern.Elasticity;
                    //<<--------------------------------------------------------------------------------------------------------------------

                }
            }
            catch
            {

            }
        }

        public void SaveTool(string Path)
        {
            m_CogPMAlignTool.SearchRegion = m_Region;

            m_CogPMAlignTool.RunParams.AcceptThreshold = m_AcceptThershold;

            if (m_Zoom == 0)
            {
                m_CogPMAlignTool.RunParams.ZoneScale.Configuration = Cognex.VisionPro.PMAlign.CogPMAlignZoneConstants.Nominal;
            }
            else
            {
                m_CogPMAlignTool.RunParams.ZoneScale.Configuration = Cognex.VisionPro.PMAlign.CogPMAlignZoneConstants.LowHigh;
                m_CogPMAlignTool.RunParams.ZoneScale.Low = 1 - m_Zoom;
                m_CogPMAlignTool.RunParams.ZoneScale.High = 1 + m_Zoom;
            }

            if (m_Angle == 0)
            {
                m_CogPMAlignTool.RunParams.ZoneAngle.Configuration = Cognex.VisionPro.PMAlign.CogPMAlignZoneConstants.Nominal;
                m_CogPMAlignTool.RunParams.ZoneAngle.Nominal = m_Angle;
            }
            else
            {
                m_CogPMAlignTool.RunParams.ZoneAngle.Configuration = Cognex.VisionPro.PMAlign.CogPMAlignZoneConstants.LowHigh;
                //m_CogPMAlignTool.RunParams.OutsideRegionThreshold = 1.0;
                m_CogPMAlignTool.RunParams.ZoneAngle.Low = -m_Angle;
                m_CogPMAlignTool.RunParams.ZoneAngle.High = m_Angle;
            }

            m_CogPMAlignTool.RunParams.ContrastThreshold = m_ContrastThresh;

            //추가 2012.02.16
            if (m_XScale == 0)
            {
                m_CogPMAlignTool.RunParams.ZoneScaleX.Configuration = Cognex.VisionPro.PMAlign.CogPMAlignZoneConstants.Nominal;
            }
            else
            {

                m_CogPMAlignTool.RunParams.ZoneScaleX.Configuration = Cognex.VisionPro.PMAlign.CogPMAlignZoneConstants.LowHigh;

                m_CogPMAlignTool.RunParams.ZoneScaleX.Low = 1 - m_XScale;
                m_CogPMAlignTool.RunParams.ZoneScaleX.High = 1 + m_XScale;

            }
            if (m_YScale == 0)
            {
                m_CogPMAlignTool.RunParams.ZoneScaleY.Configuration = Cognex.VisionPro.PMAlign.CogPMAlignZoneConstants.Nominal;
            }
            else
            {
                m_CogPMAlignTool.RunParams.ZoneScaleY.Configuration = Cognex.VisionPro.PMAlign.CogPMAlignZoneConstants.LowHigh;
                m_CogPMAlignTool.RunParams.ZoneScaleY.Low = 1 - m_YScale;
                m_CogPMAlignTool.RunParams.ZoneScaleY.High = 1 + m_YScale;

            }

            //2012년 2월16일 이기문 Elassticity추가(패턴 탄력성주기위해 추가)
            m_CogPMAlignTool.Pattern.Elasticity = m_Elasticity;


            string[] str = Path.Split('\\');

            if (!System.IO.Directory.Exists(Path.Substring(0, Path.Length - str[str.Length - 1].Length)))
            {
                System.IO.Directory.CreateDirectory(Path.Substring(0, Path.Length - str[str.Length - 1].Length));
            }

            Cognex.VisionPro.CogSerializer.SaveObjectToFile(m_CogPMAlignTool, Path);

            //2012년 5월17일 이기문 ToolBase에 m_CenterX및 m_CenterY에 값이 전달 되지않아 수정
            Reload_CenterRegion();
        }

        private object LoadSerialzed(string path)
        {
            object result = null;

            try
            {
                /*
                FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryFormatter bf = new BinaryFormatter();
                result = bf.Deserialize(stream);

                stream.Close();
                 */
                result = CogSerializer.LoadObjectFromFile(path);
            }
            catch
            {

            }

            return result;
        }


        private void SaveSerialized(object obj, string path)
        {
            /*
            Stream stream = File.Open(Path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, obj);
            stream.Close();
            */

            CogSerializer.SaveObjectToFile(obj, path);
        }

        public void LoadSearchArea(string Path, string Name)
        {
            m_SearchArea = VisionProConfigLoad.DataLoad.Load_Area(Path, Name);
        }

        public void SaveSearchArea(string Path, string Name)
        {
            VisionProConfigLoad.DataLoad.Save_Area(Path, Name, m_SearchArea);
        }

        public void LoadTool(Cognex.VisionPro.PMAlign.CogPMAlignTool CogPMAlignTool)
        {

            m_Region = null;

            m_Region = CogPMAlignTool.SearchRegion;
            m_AcceptThershold = CogPMAlignTool.RunParams.AcceptThreshold;
            m_ContrastThresh = (int)CogPMAlignTool.RunParams.ContrastThreshold;
            m_Zoom = CogPMAlignTool.RunParams.ZoneScale.High - 1;
            m_Angle = CogPMAlignTool.RunParams.ZoneAngle.High - 1;



        }
        public void Find_Pattern_Area(int m_Index, string FolderName, Cognex.VisionPro.Display.CogDisplay Display)
        {

            //m_CogPMAlignTool.Pattern = null;

            // 가장 높은 점수로 찾은 패턴 다시 찾기
            // 파일로부터 패턴 로드
            m_CogPMAlignTool.Pattern = (Cognex.VisionPro.PMAlign.CogPMAlignPattern)m_Pattern[m_Index];
            // 검사 수행
            m_CogPMAlignTool.Run();

            Display.StaticGraphics.Add((Cognex.VisionPro.ICogGraphic)m_CogPMAlignTool.Results[0].CreateResultGraphics(CogPMAlignResultGraphicConstants.BoundingBox | CogPMAlignResultGraphicConstants.Origin), "");



        }
        #endregion


    }
}