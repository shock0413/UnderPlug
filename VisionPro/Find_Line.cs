using System;
using System.IO;

using Cognex.VisionPro;
using Cognex.VisionPro.Exceptions;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.Caliper;

using System.Drawing;

namespace Hansero.VisionLib.VisionPro
{
    /// <summary>
    /// Class1�� ���� ��� �����Դϴ�.
    /// </summary>
    public class FindLine : ToolBase
    {
        public FindLine()
        {
            //
            // TODO: ���⿡ ������ ���� �߰��մϴ�.
            //			
        }

        private double m_TranslationX;
        private double m_TranslationY;
        private double m_Angle;
        private Point EndPoint;
        public System.Collections.ArrayList Find_Edge = new System.Collections.ArrayList();

        Cognex.VisionPro.Caliper.CogFindLineTool m_FindLineTool = new Cognex.VisionPro.Caliper.CogFindLineTool();

        public Point Coordinate
        {
            get
            {
                return EndPoint;
            }
            set
            {
                EndPoint = value;
            }
        }

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

        public double Angle
        {
            get
            {
                return m_Angle;
            }
            set
            {
                m_Angle = value;
            }
        }

        public int NumCalipers
        {
            get
            {
                return m_FindLineTool.RunParams.NumCalipers;
            }
            set
            {
                m_FindLineTool.RunParams.NumCalipers = value;
            }
        }

        public int NumToIgnore
        {
            get
            {
                return m_FindLineTool.RunParams.NumToIgnore;
            }
            set
            {
                m_FindLineTool.RunParams.NumToIgnore = value;
            }
        }

        public double Direction
        {
            get
            {
                return m_FindLineTool.RunParams.CaliperSearchDirection;
            }
            set
            {
                m_FindLineTool.RunParams.CaliperSearchDirection = value;
            }
        }

        public double CaliperSearchLength
        {
            get
            {
                return m_FindLineTool.RunParams.CaliperSearchLength;
            }
            set
            {
                m_FindLineTool.RunParams.CaliperSearchLength = value;
            }
        }

        public int FilterHalfSizeInPixels
        {
            get
            {
                return m_FindLineTool.RunParams.CaliperRunParams.FilterHalfSizeInPixels;
            }
            set
            {
                m_FindLineTool.RunParams.CaliperRunParams.FilterHalfSizeInPixels = value;
            }
        }

        public int ContrastThreshold
        {
            get
            {
                return (int)m_FindLineTool.RunParams.CaliperRunParams.ContrastThreshold;
            }
            set
            {
                m_FindLineTool.RunParams.CaliperRunParams.ContrastThreshold = value;
            }
        }

        #region Vision Tool Function
        public void Display_SettingArea(Cognex.VisionPro.Display.CogDisplay Display)
        {
            if (m_CenterX != 0 && m_CenterY != 0)
            {
                m_FindLineTool.RunParams.ExpectedLineSegment.SetStartLengthRotation(m_CenterX - 10, m_CenterY, 40, m_Angle + Math.PI / 2);
            }

            m_FindLineTool.RunParams.ExpectedLineSegment.GraphicDOFEnable = Cognex.VisionPro.CogLineSegmentDOFConstants.All;
            m_FindLineTool.RunParams.ExpectedLineSegment.Interactive = true;
            Display.InteractiveGraphics.Add(m_FindLineTool.RunParams.ExpectedLineSegment, "", true);
        }
        public void Display_SettingArea(Cognex.VisionPro.Display.CogDisplay Display, bool GraphicEnable)
        {
            CogLineSegment myLine;
            CogGraphicCollection myRegions;
            ICogRecord myRec;

            m_FindLineTool.InputImage = (Cognex.VisionPro.CogImage8Grey)m_Image;

            m_FindLineTool.CurrentRecordEnable = CogFindLineCurrentRecordConstants.All;

            myRec = m_FindLineTool.CreateCurrentRecord();

            //if (m_CenterX != 0 && m_CenterY != 0)
            //{
            //    m_FindLineTool.RunParams.ExpectedLineSegment.SetStartLengthRotation(m_CenterX, m_CenterY, CaliperSearchLength, m_Angle);

            //}


            myLine = (CogLineSegment)myRec.SubRecords["InputImage"].SubRecords["ExpectedShapeSegment"].Content;

            myRegions = (CogGraphicCollection)myRec.SubRecords["InputImage"].SubRecords["CaliperRegions"].Content;

            //  Add the graphics to the display
            Display.InteractiveGraphics.Add(myLine, "", false);
            foreach (ICogGraphic g in myRegions)
                Display.InteractiveGraphics.Add((ICogGraphicInteractive)g, "", false);

        }

        public bool Find_Line(Cognex.VisionPro.Display.CogDisplay Display, bool bSearchAreaView)
        {
            try
            {
                // ĸ��� �̹��� ����
                if (m_Image == null)
                {
                    DrawLabel(String.Format("�˻� �� �̹����� �����ϴ�."), Display, 5, 5, 12, CogColorConstants.Red, CogColorConstants.Black);
                    return false;
                }

                m_FindLineTool.InputImage = (Cognex.VisionPro.CogImage8Grey)m_Image;

                //ȭ�鿡 ��ο���̿��ؼ� ǥ���Ҷ� ��ǥ���� m_TranslationX,Y�� ����
                m_TranslationX = m_FindLineTool.RunParams.ExpectedLineSegment.StartX;
                m_TranslationY = m_FindLineTool.RunParams.ExpectedLineSegment.StartY;


                //�˻� ����
                m_FindLineTool.Run();

                #region �˻� ��� ���÷���
                if (m_FindLineTool.Results.GetLineSegment() != null)
                {

                    m_FindLineTool.RunParams.CaliperRunParams.ContrastThreshold = ContrastThreshold;


                    Cognex.VisionPro.ICogGraphic ResultGp;

                    ResultGp = (Cognex.VisionPro.ICogGraphic)m_FindLineTool.Results.GetLineSegment();

                    ResultGp.Color = Cognex.VisionPro.CogColorConstants.Green;

                    Display.StaticGraphics.Add(ResultGp, "");

                    //��Ŭ
                    int Temp_Count = 0;
                    int TempTemp_Count = 0;

                    int Senser_Count = 0;

                    Point TempPoint = new Point();
                    TempPoint.X = -1;
                    TempPoint.Y = -1;
                    EndPoint.X = -1;
                    EndPoint.Y = -1;

                    bool Temp_StartOn = false;

                    //int a = m_FindCircleTool.Results.Count;
                    //Ķ���� ���� ��ŭ ���� ������
                    for (int i = 0; i < m_FindLineTool.RunParams.NumCalipers; i++)
                    {
                        //���� �˻����� false�̸�...
                        if (m_FindLineTool.Results[i].Found == false)
                        {
                            if (Temp_StartOn == false)
                            {
                                //�տ��� NG�� �߻������Ƿ� �׳� ������ �ǽ��Ѵ�.
                                Temp_Count++;
                                Temp_StartOn = false;
                                //���� �˻����� false�� 5������ ũ�� ����ī���Ͱ� 0�ϰ��(ó�� false�� �����ϱ� ����)
                                if ((Temp_Count > 5) && (Senser_Count == 0))
                                {
                                    if ((TempPoint.X != -1) && (TempPoint.Y != -1))
                                    {
                                        //�˻����� true�� ��ǥ�� endpoint�� �ֱ�...
                                        EndPoint = TempPoint;
                                        Senser_Count = 1;
                                    }
                                }

                            }
                            else
                            {
                                //ó�� NG����
                                Temp_StartOn = false;
                                //����ī��Ʈ�ϳ� ������Ű��...
                                Temp_Count++;
                            }


                        }
                        else       //�˻����� true�̸�...
                        {
                            if (i > 5)
                            {
                                if (Math.Abs((int)m_FindLineTool.Results[i].X - TempPoint.X) > 5)
                                {
                                    Senser_Count = 1;
                                    //�˻����� true�� ��ǥ�� endpoint�� �ֱ�...
                                    EndPoint = TempPoint;
                                }
                                else
                                {
                                    TempPoint.X = (int)m_FindLineTool.Results[i].X;
                                    TempPoint.Y = (int)m_FindLineTool.Results[i].Y;
                                }
                            }
                            else
                            {
                                TempPoint.X = (int)m_FindLineTool.Results[i].X;
                                TempPoint.Y = (int)m_FindLineTool.Results[i].Y;
                            }

                            CogPointMarker m_PointPosition = new CogPointMarker();
                            m_PointPosition.X = m_FindLineTool.Results[i].X;
                            m_PointPosition.Y = m_FindLineTool.Results[i].Y;
                            m_PointPosition.GraphicDOFEnable = Cognex.VisionPro.CogPointMarkerDOFConstants.All;
                            m_PointPosition.Interactive = true;
                            Display.InteractiveGraphics.Add(m_PointPosition, "", true);

                            Find_Edge.Add(m_FindLineTool.Results[i].Used);
                            Temp_StartOn = true;
                            //����ī��Ʈ �ʱ�ȭ
                            //Temp_Count = 0;
                        }



                    }
                    return true;
                }
                else
                {
                    EndPoint.X = -1;
                    EndPoint.Y = -1;
                    return false;
                }
                #endregion
            }
            catch
            {
            }

            return false;
        }

        public void LoadTool(string Path)
        {
            try
            {
                m_FindLineTool = (Cognex.VisionPro.Caliper.CogFindLineTool)Cognex.VisionPro.CogSerializer.LoadObjectFromFile(Path);
                //ContrastThreshold = (int)m_FindLineTool.RunParams.CaliperRunParams.ContrastThreshold;
            }
            catch
            {
                m_FindLineTool = new Cognex.VisionPro.Caliper.CogFindLineTool();

                m_FindLineTool.RunParams.NumCalipers = 20;
                m_FindLineTool.RunParams.NumToIgnore = 10;

                m_FindLineTool.RunParams.CaliperSearchDirection = (int)(180 * Math.PI / 2);
                m_FindLineTool.RunParams.CaliperSearchLength = 30;
                m_FindLineTool.RunParams.ExpectedLineSegment.SetStartEnd(320, 240, 360, 240);

                SaveTool(Path);

            }
        }

        public void SaveTool(string Path)
        {
            //m_CenterX = m_FindLineTool.RunParams.ExpectedLineSegment.StartX;
            //m_CenterY = m_FindLineTool.RunParams.ExpectedLineSegment.StartY;
            //m_Angle = m_FindLineTool.RunParams.ExpectedLineSegment.Rotation;

            //������Ȧ�尪 ����
            //m_FindLineTool.RunParams.CaliperRunParams.ContrastThreshold = ContrastThreshold;


            #region �˻� �Ķ���� ����
            m_FindLineTool.RunParams.CaliperRunParams.EdgeMode = Cognex.VisionPro.Caliper.CogCaliperEdgeModeConstants.SingleEdge;
            m_FindLineTool.RunParams.CaliperRunParams.Edge0Polarity = Cognex.VisionPro.Caliper.CogCaliperPolarityConstants.LightToDark;


            #endregion

            string[] str = Path.Split('\\');

            if (!System.IO.Directory.Exists(Path.Substring(0, Path.Length - str[str.Length - 1].Length)))
            {
                System.IO.Directory.CreateDirectory(Path.Substring(0, Path.Length - str[str.Length - 1].Length));
            }

            Cognex.VisionPro.CogSerializer.SaveObjectToFile(m_FindLineTool, Path);

            //2012�� 5��17�� �̱⹮ ToolBase�� m_CenterX�� m_CenterY�� ���� ���� �����ʾ� ����
            //Reload_CenterRegion();
        }
        #endregion
    }
}