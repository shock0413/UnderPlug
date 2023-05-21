using System;
using System.IO;
using System.Collections;

using Cognex.VisionPro;
using Cognex.VisionPro.Exceptions;
using Cognex.VisionPro.Caliper;

namespace Hansero.VisionLib.VisionPro
{
    /// <summary>
    /// Class1에 대한 요약 설명입니다.
    /// </summary>
    public class Caliper : ToolBase
    {

        Cognex.VisionPro.Caliper.CogCaliperTool m_CogCaliperTool = new CogCaliperTool();

        public string EdgeMode
        {
            set
            {
                if (value == "Single")
                {
                    m_CogCaliperTool.RunParams.EdgeMode = CogCaliperEdgeModeConstants.SingleEdge;
                }
                else
                {
                    m_CogCaliperTool.RunParams.EdgeMode = CogCaliperEdgeModeConstants.Pair;
                }                
            }
            get
            {
                if (m_CogCaliperTool.RunParams.EdgeMode == CogCaliperEdgeModeConstants.SingleEdge)
                    return "Single";
                else
                    return "Pair";
            }
        }        

        public string Edge0Polarity
        {
            set
            {
                if (value == "DarkToLight")
                    m_CogCaliperTool.RunParams.Edge0Polarity = CogCaliperPolarityConstants.DarkToLight;
                else if (value == "LightToDark")
                    m_CogCaliperTool.RunParams.Edge0Polarity = CogCaliperPolarityConstants.LightToDark;
                else
                    m_CogCaliperTool.RunParams.Edge0Polarity = CogCaliperPolarityConstants.DontCare;
            }
            get
            {
                if (m_CogCaliperTool.RunParams.Edge0Polarity == CogCaliperPolarityConstants.DarkToLight)
                    return "DarkToLight";
                else if (m_CogCaliperTool.RunParams.Edge0Polarity == CogCaliperPolarityConstants.LightToDark)
                    return "LightToDark";
                else
                    return "DontCare";                
            }
        }

        public string Edge1Polarity
        {
            set
            {
                if (value == "DarkToLight")
                    m_CogCaliperTool.RunParams.Edge1Polarity = CogCaliperPolarityConstants.DarkToLight;
                else if (value == "LightToDark")
                    m_CogCaliperTool.RunParams.Edge1Polarity = CogCaliperPolarityConstants.LightToDark;
                else
                    m_CogCaliperTool.RunParams.Edge1Polarity = CogCaliperPolarityConstants.DontCare;
            }
            get
            {
                if (m_CogCaliperTool.RunParams.Edge1Polarity == CogCaliperPolarityConstants.DarkToLight)
                    return "DarkToLight";
                else if (m_CogCaliperTool.RunParams.Edge1Polarity == CogCaliperPolarityConstants.LightToDark)
                    return "LightToDark";
                else
                    return "DontCare";
            }
        }

        public int MaxResults
        {
            get
            {
                return m_CogCaliperTool.RunParams.MaxResults;
            }
            set
            {
                m_CogCaliperTool.RunParams.MaxResults = value;
            }
        }

        /// <summary>
        /// 선명도 결정하는 값.
        /// 2 정도로 세팅할 경우 선명한 Edge.
        /// 높은값일 수록 흐린 Edge.
        /// </summary>
        public int FilterHalfSizeInPixels
        {            
            get
            {                
                return m_CogCaliperTool.RunParams.FilterHalfSizeInPixels;                
            }
            set
            {
                m_CogCaliperTool.RunParams.FilterHalfSizeInPixels = value;
            }
        }

        /// <summary>
        /// 배경과의 밝기 차 설정.
        /// </summary>
        public double ContrastThreshold
        {
            get
            {
                return m_CogCaliperTool.RunParams.ContrastThreshold;
            }
            set
            {
                m_CogCaliperTool.RunParams.ContrastThreshold = value;
            }
        }

        public double PairWidth
        {
            get
            {
                return Math.Abs(m_CogCaliperTool.RunParams.Edge0Position - m_CogCaliperTool.RunParams.Edge1Position);
            }
            set
            {
                m_CogCaliperTool.RunParams.Edge0Position = 0;
                m_CogCaliperTool.RunParams.Edge1Position = value;
            }
        }

        public Cognex.VisionPro.CogRectangleAffine Region
        {
            get
            {
                return m_CogCaliperTool.Region;
            }
            set
            {
                m_CogCaliperTool.Region = value;
            }
        }

        // 기본 폴더 설정.
        string BasePath;        

        public Caliper()
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
        public double FindEdge(Cognex.VisionPro.Display.CogDisplay Display, bool bViewArea)
        {
            double Width = 0;

            #region 이미지 설정
            if (m_Image == null)
                throw new Exception("검사 할 이미지가 없습니다.");

            m_CogCaliperTool.InputImage = null;
            m_CogCaliperTool.InputImage = (Cognex.VisionPro.CogImage8Grey)m_Image;
            #endregion

            #region 패턴 로딩 후 검사

            m_CogCaliperTool.Region = (CogRectangleAffine) m_Region;

            // 검사 수행
            m_CogCaliperTool.Run();

            #endregion

            #region 검사 결과 디스플레이
            //Display.StaticGraphics.Clear();

            // 검사 영역 표시
            if (bViewArea)
            {
                DisplaySearchArea(Display, false);
            }            

            // 찾은 결과가 있을 경우 찾은 영역, 점수 표시
            if (m_CogCaliperTool.Results != null && m_CogCaliperTool.Results.Count > 0)
            {
                double DifValue = m_CogCaliperTool.Results[0].Width;
                Width = m_CogCaliperTool.Results[0].Width;
                int index = 0;
               
                for(int i = 0; i< m_CogCaliperTool.Results.Count; i++)
                {
                    if (m_CogCaliperTool.Results[i].Width < DifValue)
                    {
                        index = i;
                        Width = m_CogCaliperTool.Results[i].Width;
                        DifValue = Math.Abs(this.PairWidth - Width);
                    }
                }

                double x, y;

                x = (m_CogCaliperTool.Results[index].Edge0.PositionX + m_CogCaliperTool.Results[index].Edge1.PositionX) / 2;
                y = (m_CogCaliperTool.Results[index].Edge0.PositionY + m_CogCaliperTool.Results[index].Edge1.PositionY) / 2;

                // 찾은 패턴 표시
                //DrawCross(m_CogCaliperTool.Results[index].get, m_CogCaliperTool.Results[index].PositionY, 5, 1, Display, CogColorConstants.Green, CogColorConstants.Black);
                DrawLabel(" 두께: " + String.Format("{0:0} ", m_CogCaliperTool.Results[index].Width), Display, x + 2, y + 2, 10, CogColorConstants.White, CogColorConstants.Black);
                Display.StaticGraphics.Add((Cognex.VisionPro.ICogGraphic)m_CogCaliperTool.Results[index].CreateResultGraphics(CogCaliperResultGraphicConstants.All), "");
            }
            #endregion

            m_SearchArea = null;

            return Width;
        }

        public double FindEdge(double Min, double Max, Cognex.VisionPro.Display.CogDisplay Display, bool bViewArea)
        {
            double Width = 0;

            #region 이미지 설정
            if (m_Image == null)
                throw new Exception("검사 할 이미지가 없습니다.");

            m_CogCaliperTool.InputImage = null;
            m_CogCaliperTool.InputImage = (Cognex.VisionPro.CogImage8Grey)m_Image;
            #endregion

            #region 패턴 로딩 후 검사

            //m_CogCaliperTool.Region = (CogRectangleAffine)m_Region;

            // 검사 수행
            m_CogCaliperTool.Run();

            #endregion

            #region 검사 결과 디스플레이
            //Display.StaticGraphics.Clear();

            // 검사 영역 표시
            if (bViewArea)
            {
                DisplaySearchArea(Display, false);
            }

            // 찾은 결과가 있을 경우 찾은 영역, 점수 표시
            if (m_CogCaliperTool.Results != null && m_CogCaliperTool.Results.Count > 0)
            {
                double DifValue = m_CogCaliperTool.Results[0].Width;
                Width = m_CogCaliperTool.Results[0].Width;
                int index = 0;

                for (int i = 0; i < m_CogCaliperTool.Results.Count; i++)
                {
                    if (m_CogCaliperTool.Results[i].Width < DifValue)
                    {
                        index = i;
                        Width = m_CogCaliperTool.Results[i].Width;
                        DifValue = Math.Abs(this.PairWidth - Width);
                    }
                }

                double x, y;

                x = (m_CogCaliperTool.Results[index].Edge0.PositionX + m_CogCaliperTool.Results[index].Edge1.PositionX) / 2;
                y = (m_CogCaliperTool.Results[index].Edge0.PositionY + m_CogCaliperTool.Results[index].Edge1.PositionY) / 2;

                if (Min < Width && Width < Max)
                {
                    DrawLabel(" 두께: " + String.Format("{0:0} ", m_CogCaliperTool.Results[index].Width), Display, x + 2, y + 2, 10, CogColorConstants.White, CogColorConstants.Black);
                    Display.StaticGraphics.Add((Cognex.VisionPro.ICogGraphic)m_CogCaliperTool.Results[index].CreateResultGraphics(CogCaliperResultGraphicConstants.All), "");
                }
                else
                {
                    DrawLabel(" 두께: " + String.Format("{0:0} ", m_CogCaliperTool.Results[index].Width), Display, x + 2, y + 2, 10, CogColorConstants.Red, CogColorConstants.Black);
                    Display.StaticGraphics.Add((Cognex.VisionPro.ICogGraphic)m_CogCaliperTool.Results[index].CreateResultGraphics(CogCaliperResultGraphicConstants.All), "");
                }

                // 찾은 패턴 표시
                //DrawCross(m_CogCaliperTool.Results[index].get, m_CogCaliperTool.Results[index].PositionY, 5, 1, Display, CogColorConstants.Green, CogColorConstants.Black);
                
            }
            #endregion

            m_SearchArea = null;

            return Width;
        }
        
        public void ReleaseTool()
        {
            if (m_CogCaliperTool != null)
            {
                m_CogCaliperTool.Dispose();
                m_CogCaliperTool = null;
            }
        }

        public void Set_SearchArea(Cognex.VisionPro.Display.CogDisplay Display)
        {
            m_CogCaliperTool.Region.Interactive = true;

            m_CogCaliperTool.Region.GraphicDOFEnable = CogRectangleAffineDOFConstants.Position | CogRectangleAffineDOFConstants.Rotation | CogRectangleAffineDOFConstants.Size;

            Display.InteractiveGraphics.Add(m_CogCaliperTool.Region, "", false);
        }

        public void LoadTool(string Path)
        {
            try
            {
                m_CogCaliperTool = (Cognex.VisionPro.Caliper.CogCaliperTool)Cognex.VisionPro.CogSerializer.LoadObjectFromFile(Path);

                m_Region = null;

                m_RegionShape = RegionShape.Rectangle;

                m_Region = m_CogCaliperTool.Region;
            }
            catch
            {
                m_CogCaliperTool = new CogCaliperTool();

                m_Region = null;

                m_RegionShape = RegionShape.Rectangle;

                m_Region = new CogRectangleAffine();

                m_CogCaliperTool.RunParams.EdgeMode = CogCaliperEdgeModeConstants.Pair;
                m_CogCaliperTool.RunParams.Edge0Position = 0;
                m_CogCaliperTool.RunParams.Edge1Position = 20;
                m_CogCaliperTool.RunParams.ContrastThreshold = 50;
                m_CogCaliperTool.RunParams.Edge0Polarity = CogCaliperPolarityConstants.DarkToLight;
                m_CogCaliperTool.RunParams.Edge1Polarity = CogCaliperPolarityConstants.LightToDark;
            }
        }

        public void SaveTool(string Path)
        {
            Cognex.VisionPro.CogSerializer.SaveObjectToFile(m_CogCaliperTool, Path);
        }
        #endregion
    }
}