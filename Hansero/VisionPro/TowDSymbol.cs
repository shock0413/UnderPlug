using System;
using System.IO;

using Cognex.VisionPro;
using Cognex.VisionPro.Exceptions;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.TwoDSymbol;

namespace Hansero.VisionLib.VisionPro
{
    /// <summary>
    /// Class1에 대한 요약 설명입니다.
    /// </summary>
    public class TowDSymbol : ToolBase
    {
        private Cognex.VisionPro.CogRectangleAffine m_PatternArea = new CogRectangleAffine();

        public TowDSymbol()
        {
            //
            // TODO: 여기에 생성자 논리를 추가합니다.
            //			
        }

        #region Vision Tool Function
        private void ResultIAQGDotShow(Cog2DSymbolVerifyIAQGDotResult objIAQGDot, Cognex.VisionPro.Display.CogDisplay Display)
        {
            CogStatistics mobjLocStats = new CogStatistics();
            CogStatistics mobjSizeStats = new CogStatistics();
            CogStatistics mobjShapeStats = new CogStatistics();

            mobjLocStats.ResetRunningStatistics();
            mobjSizeStats.ResetRunningStatistics();
            mobjShapeStats.ResetRunningStatistics();

            if (objIAQGDot == null)
                return;

            int iRow, iCol;
            int iLastRow, iLastCol;

            iLastRow = objIAQGDot.NumRows - 1;
            iLastCol = objIAQGDot.NumColumns - 1;

            // Loop through all rows and all columns. Wherever a dot is expected and found,
            //' fetch the results and update the statistics.
            for (iRow = 0; iRow < iLastRow; iRow++)
            {
                for (iCol = 0; iCol < iLastCol; iCol++)
                {
                    if (objIAQGDot.GetDotExpected(iCol, iRow))
                    {
                        if (objIAQGDot.GetDotFound(iCol, iRow))
                        {
                            if (objIAQGDot.GetLocationValid(iCol, iRow))
                            {
                                mobjLocStats.AddValue(objIAQGDot.GetLocationDeviationPercent(iCol, iRow));
                            }

                            if (objIAQGDot.GetSizeValid(iCol, iRow))
                            {
                                mobjSizeStats.AddValue(objIAQGDot.GetSizePercent(iCol, iRow));
                            }

                            if (objIAQGDot.GetShapeValid(iCol, iRow))
                            {
                                mobjShapeStats.AddValue(objIAQGDot.GetShapeOvality(iCol, iRow));
                            }
                        }
                    }
                }
            }

            // Generate a mask graphic for combined, and add it to the display. This will
            // map color-coded translucent circles to the combined dot grade per
            // expected dot.
            CogMaskGraphic objLocMap;
            objLocMap = objIAQGDot.CreateResultMaskGraphic(Cog2DSymbolVerifyIAQGDotGraphicConstants.CombinedMap);
            Display.StaticGraphics.Add(objLocMap, "");
            DrawLabel(String.Format("일치도:{0:##0.00}% ({1:##0.00},{2:##0.00})", 100 - mobjLocStats.RunningMean, 100 - mobjLocStats.RunningMaxValue, 100 - mobjLocStats.RunningMinValue), Display, 5, 15, 12, CogColorConstants.Green, CogColorConstants.Black);
        }

        public string Read2DCode(string Name, Cognex.VisionPro.Display.CogDisplay Display)
        {
            Cog2DSymbolTool m_Cog2DSymbolTool = new Cog2DSymbolTool();

            string ReadString;

            try
            {
                // 캡춰된 이미지 설정
                if (m_Image == null)
                {
                    DrawLabel(String.Format("검사 할 이미지가 없습니다."), Display, 5, 5, 12, CogColorConstants.Red, CogColorConstants.Black);
                    return "Error!! 검사 할 이미지가 없습니다.";
                }

                m_Cog2DSymbolTool.InputImage = (Cognex.VisionPro.CogImage8Grey)m_Image;

                #region 검사 영역 로딩                
                if (m_RegionShape == RegionShape.Rectangle)
                {
                    LoadSearchArea(Name, "Rectangle");
                    m_Cog2DSymbolTool.SearchRegion = m_SearchArea;
                }
                else
                {
                    LoadSearchArea(Name, "Ring");
                    m_Cog2DSymbolTool.SearchRegion = m_RingRegion;
                }
                #endregion

                #region 검사 파라미터 설정
                #endregion

                #region 패턴 로딩
                //if(!Directory.Exists(System.Windows.Forms.Application.StartupPath + "\\" + "2DSymbol\\" + Name))
                //{
                //    DrawLabel(String.Format("바코드 읽기를 실패 하였습니다."), Display, 5, 5, 12, CogColorConstants.Red, CogColorConstants.Black);
                //    return "Error!! 바코드 읽기 실패";
                //}

                string[] files = Directory.GetFiles(Name);

                foreach (string filename in files)
                {
                    // 파일로부터 패턴 로드
                    m_Cog2DSymbolTool.Pattern = (Cognex.VisionPro.TwoDSymbol.Cog2DSymbolPattern)Cognex.VisionPro.CogSerializer.LoadObjectFromFile(filename);
                }
                #endregion

                Display.StaticGraphics.Clear();

                //검사 수행
                m_Cog2DSymbolTool.Run();

                if (m_Cog2DSymbolTool.Result == null || m_Cog2DSymbolTool.Result.Found == false)
                {
                    return "Error!!";
                }

                #region 검사 결과 디스플레이
                Cognex.VisionPro.ICogGraphic ResultGp;

                #region 바코드 적합성 체크
                Cog2DSymbolVerifyTool m_Cog2DSymbolVerifyTool = new Cog2DSymbolVerifyTool();

                try
                {
                    m_Cog2DSymbolVerifyTool.InputImage = (Cognex.VisionPro.CogImage8Grey)m_Image;
                    m_Cog2DSymbolVerifyTool.Region = m_Cog2DSymbolTool.Result.ResultGrid;
                    m_Cog2DSymbolVerifyTool.RunParams.Pattern = m_Cog2DSymbolTool.Pattern.TrainResult;

                    m_Cog2DSymbolVerifyTool.RunParams.MetricsAIMEnabled = true;
                    m_Cog2DSymbolVerifyTool.RunParams.MetricsSupplementalEnabled = true;
                    m_Cog2DSymbolVerifyTool.RunParams.MetricsIAQGDotEnabled = true;
                    m_Cog2DSymbolVerifyTool.RunParams.MetricsIAQGDotIncludeShapeInGrades = true;

                    m_Cog2DSymbolVerifyTool.Run();
                }
                catch
                {
                }
                #endregion

                // 검사 영역 표시
                if (m_Cog2DSymbolTool.SearchRegion != null)
                {
                    ResultGp = (Cognex.VisionPro.ICogGraphic)m_Cog2DSymbolTool.SearchRegion;

                    if (m_Cog2DSymbolTool.Result.Found == true)
                        ResultGp.Color = Cognex.VisionPro.CogColorConstants.Blue;
                    else
                        ResultGp.Color = Cognex.VisionPro.CogColorConstants.Red;

                    Display.StaticGraphics.Add(ResultGp, "");
                }

                // 찾은 결과 표시
                if (m_Cog2DSymbolTool.Result.Found == true)
                {
                    ResultGp = (Cognex.VisionPro.ICogGraphic)m_Cog2DSymbolTool.Result.ResultGrid;

                    ResultGp.Color = Cognex.VisionPro.CogColorConstants.Green;

                    Display.StaticGraphics.Add(ResultGp, "");
                    DrawLabel(m_Cog2DSymbolTool.Result.DecodedString, Display, m_Cog2DSymbolTool.Result.ResultGrid.CornerOppositeX, m_Cog2DSymbolTool.Result.ResultGrid.CornerOppositeY, 10, CogColorConstants.Black, CogColorConstants.White);

                    ReadString = m_Cog2DSymbolTool.Result.DecodedString;
                }
                else
                {
                    ReadString = "Read Failed!!";
                }

                //ResultIAQGDotShow(m_Cog2DSymbolVerifyTool.Result.MetricsIAQGDot, Display);
                #endregion

                m_Cog2DSymbolVerifyTool = null;
                m_Cog2DSymbolTool = null;
            }
            catch(Exception ex)
            {
                ReadString = "Error!!";
            }

            return ReadString;
        }

        public void Set_TowDSymbolPattern(string Name)
        {
            Cog2DSymbolTool m_Cog2DSymbolTool = new Cog2DSymbolTool();

            // 캡춰된 이미지 설정
            if (m_Image == null)
                throw new Exception("검사 할 이미지가 없습니다.");

            m_Cog2DSymbolTool.InputImage = (Cognex.VisionPro.CogImage8Grey)m_Image;

            m_Cog2DSymbolTool.Pattern.TrainImage = (Cognex.VisionPro.CogImage8Grey)m_Image;

            //m_Cog2DSymbolTool.Pattern.TrainRegion = (Cognex.VisionPro.ICogRegion) m_PatternArea;			

            m_Cog2DSymbolTool.Pattern.Train();


            if (m_Cog2DSymbolTool.Pattern.Trained == true)
            {
                Cognex.VisionPro.CogSerializer.SaveObjectToFile(m_Cog2DSymbolTool.Pattern, Name + "\\Pattern.vpp");
            }
            else
            {
                throw new Exception("Train Failed.");
            }
        }

        /// <summary>
        /// 패턴 영역을 설정하기 위해서 디폴트 영역 표시
        /// </summary>
        /// <param name="Display">영역을 표시할 디스플레이</param>
        public void Display_PatternArea(Cognex.VisionPro.Display.CogDisplay Display)
        {
            m_PatternArea.CenterX = 320;
            m_PatternArea.CenterY = 240;
            m_PatternArea.SideXLength = 100;
            m_PatternArea.SideYLength = 100;
            m_PatternArea.Rotation = 0;
            m_PatternArea.XDirectionAdornment = Cognex.VisionPro.CogRectangleAffineDirectionAdornmentConstants.Arrow;
            m_PatternArea.YDirectionAdornment = Cognex.VisionPro.CogRectangleAffineDirectionAdornmentConstants.SolidArrow;
            m_PatternArea.GraphicDOFEnable = Cognex.VisionPro.CogRectangleAffineDOFConstants.All;
            m_PatternArea.Interactive = true;

            Display.InteractiveGraphics.Add(m_PatternArea, "", true);
        }

        public void LoadSearchArea(string Name, string Type)
        {
            if (Type == "Rectangle")
            {
                m_SearchArea = (Cognex.VisionPro.CogRectangleAffine)VisionProConfigLoad.DataLoad.Load_Area(Name, Type);
                m_Region = m_SearchArea;

            }
            else
            {
                m_RingRegion = (Cognex.VisionPro.CogCircularAnnulusSection)VisionProConfigLoad.DataLoad.Load_Area(Name, Type);
                m_Region = m_RingRegion;
            }
        }

        public void SaveSearchArea(string Name, string Type)
        {
            if (Type == "Rectangle")
            {
                VisionProConfigLoad.DataLoad.Save_Area(Name, m_SearchArea);
            }
            else
            {
                VisionProConfigLoad.DataLoad.Save_Area(Name, m_RingRegion);
            }
        }
        #endregion
    }
}