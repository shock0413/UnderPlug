using System;
using System.IO;

using Cognex.VisionPro;
using Cognex.VisionPro.Exceptions;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.TwoDSymbol;

namespace Hansero.VisionLib.VisionPro
{
    /// <summary>
    /// Class1�� ���� ��� �����Դϴ�.
    /// </summary>
    public class TowDSymbol : ToolBase
    {
        private Cognex.VisionPro.CogRectangleAffine m_PatternArea = new CogRectangleAffine();

        public TowDSymbol()
        {
            //
            // TODO: ���⿡ ������ ���� �߰��մϴ�.
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
            DrawLabel(String.Format("��ġ��:{0:##0.00}% ({1:##0.00},{2:##0.00})", 100 - mobjLocStats.RunningMean, 100 - mobjLocStats.RunningMaxValue, 100 - mobjLocStats.RunningMinValue), Display, 5, 15, 12, CogColorConstants.Green, CogColorConstants.Black);
        }

        public string Read2DCode(string Name, Cognex.VisionPro.Display.CogDisplay Display)
        {
            Cog2DSymbolTool m_Cog2DSymbolTool = new Cog2DSymbolTool();

            string ReadString;

            try
            {
                // ĸ��� �̹��� ����
                if (m_Image == null)
                {
                    DrawLabel(String.Format("�˻� �� �̹����� �����ϴ�."), Display, 5, 5, 12, CogColorConstants.Red, CogColorConstants.Black);
                    return "Error!! �˻� �� �̹����� �����ϴ�.";
                }

                m_Cog2DSymbolTool.InputImage = (Cognex.VisionPro.CogImage8Grey)m_Image;

                #region �˻� ���� �ε�                
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

                #region �˻� �Ķ���� ����
                #endregion

                #region ���� �ε�
                //if(!Directory.Exists(System.Windows.Forms.Application.StartupPath + "\\" + "2DSymbol\\" + Name))
                //{
                //    DrawLabel(String.Format("���ڵ� �б⸦ ���� �Ͽ����ϴ�."), Display, 5, 5, 12, CogColorConstants.Red, CogColorConstants.Black);
                //    return "Error!! ���ڵ� �б� ����";
                //}

                string[] files = Directory.GetFiles(Name);

                foreach (string filename in files)
                {
                    // ���Ϸκ��� ���� �ε�
                    m_Cog2DSymbolTool.Pattern = (Cognex.VisionPro.TwoDSymbol.Cog2DSymbolPattern)Cognex.VisionPro.CogSerializer.LoadObjectFromFile(filename);
                }
                #endregion

                Display.StaticGraphics.Clear();

                //�˻� ����
                m_Cog2DSymbolTool.Run();

                if (m_Cog2DSymbolTool.Result == null || m_Cog2DSymbolTool.Result.Found == false)
                {
                    return "Error!!";
                }

                #region �˻� ��� ���÷���
                Cognex.VisionPro.ICogGraphic ResultGp;

                #region ���ڵ� ���ռ� üũ
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

                // �˻� ���� ǥ��
                if (m_Cog2DSymbolTool.SearchRegion != null)
                {
                    ResultGp = (Cognex.VisionPro.ICogGraphic)m_Cog2DSymbolTool.SearchRegion;

                    if (m_Cog2DSymbolTool.Result.Found == true)
                        ResultGp.Color = Cognex.VisionPro.CogColorConstants.Blue;
                    else
                        ResultGp.Color = Cognex.VisionPro.CogColorConstants.Red;

                    Display.StaticGraphics.Add(ResultGp, "");
                }

                // ã�� ��� ǥ��
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

            // ĸ��� �̹��� ����
            if (m_Image == null)
                throw new Exception("�˻� �� �̹����� �����ϴ�.");

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
        /// ���� ������ �����ϱ� ���ؼ� ����Ʈ ���� ǥ��
        /// </summary>
        /// <param name="Display">������ ǥ���� ���÷���</param>
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