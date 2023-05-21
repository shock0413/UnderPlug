using System;
using System.IO;

using Cognex.VisionPro;
using Cognex.VisionPro.Exceptions;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.Caliper;

namespace Hansero.VisionLib.VisionPro
{
	/// <summary>
	/// Class1에 대한 요약 설명입니다.
	/// </summary>
	public class FindCircle : ToolBase
	{
        Cognex.VisionPro.Caliper.CogFindCircleTool m_FindCircleTool = new Cognex.VisionPro.Caliper.CogFindCircleTool();

		public FindCircle()
		{
			//
			// TODO: 여기에 생성자 논리를 추가합니다.
			//			
		}

		private double m_TranslationX;
		private double m_TranslationY;
		private double m_Radius;

        //NG카운트개수
        private int m_Count;

        //최소 최대 반지름
        private double m_MinRadius;
        private double m_MaxRadius;

        //검사방식 지정하기 위해 선언
        public enum InspectionMethod { Total, Continuous };
        private InspectionMethod m_InspectionMethod;

        //검사결과를 어레이리스트에 저장
        public System.Collections.ArrayList X_Position = new System.Collections.ArrayList();
        public System.Collections.ArrayList Y_Position = new System.Collections.ArrayList();
        public System.Collections.ArrayList Find_Edge = new System.Collections.ArrayList();

        
        public InspectionMethod Method
        {
            set
            {
                
                m_InspectionMethod = value;
                
            }
            get
            {
                return m_InspectionMethod;
            }
        }
        public int NGCount
        {
            get
            {

                return m_Count;
            }
            set
            {
                
                m_Count = value;
            }
        }
        public string EdgeMode
        {
            set
            {
                if (value == "Single")
                {
                    m_FindCircleTool.RunParams.CaliperRunParams.EdgeMode = Cognex.VisionPro.Caliper.CogCaliperEdgeModeConstants.SingleEdge;
                }
                else
                {
                    m_FindCircleTool.RunParams.CaliperRunParams.EdgeMode = Cognex.VisionPro.Caliper.CogCaliperEdgeModeConstants.Pair;
                }
            }
            get
            {
                if (m_FindCircleTool.RunParams.CaliperRunParams.EdgeMode == Cognex.VisionPro.Caliper.CogCaliperEdgeModeConstants.SingleEdge)
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
                    m_FindCircleTool.RunParams.CaliperRunParams.Edge0Polarity = Cognex.VisionPro.Caliper.CogCaliperPolarityConstants.DarkToLight;
                else if (value == "LightToDark")
                    m_FindCircleTool.RunParams.CaliperRunParams.Edge0Polarity = Cognex.VisionPro.Caliper.CogCaliperPolarityConstants.LightToDark;
                else
                    m_FindCircleTool.RunParams.CaliperRunParams.Edge0Polarity = Cognex.VisionPro.Caliper.CogCaliperPolarityConstants.DontCare;
            }
            get
            {
                if (m_FindCircleTool.RunParams.CaliperRunParams.Edge0Polarity == Cognex.VisionPro.Caliper.CogCaliperPolarityConstants.DarkToLight)
                    return "DarkToLight";
                else if (m_FindCircleTool.RunParams.CaliperRunParams.Edge0Polarity == Cognex.VisionPro.Caliper.CogCaliperPolarityConstants.LightToDark)
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
                    m_FindCircleTool.RunParams.CaliperRunParams.Edge1Polarity = Cognex.VisionPro.Caliper.CogCaliperPolarityConstants.DarkToLight;
                else if (value == "LightToDark")
                    m_FindCircleTool.RunParams.CaliperRunParams.Edge1Polarity = Cognex.VisionPro.Caliper.CogCaliperPolarityConstants.LightToDark;
                else
                    m_FindCircleTool.RunParams.CaliperRunParams.Edge1Polarity = Cognex.VisionPro.Caliper.CogCaliperPolarityConstants.DontCare;
            }
            get
            {
                if (m_FindCircleTool.RunParams.CaliperRunParams.Edge1Polarity == Cognex.VisionPro.Caliper.CogCaliperPolarityConstants.DarkToLight)
                    return "DarkToLight";
                else if (m_FindCircleTool.RunParams.CaliperRunParams.Edge1Polarity == Cognex.VisionPro.Caliper.CogCaliperPolarityConstants.LightToDark)
                    return "LightToDark";
                else
                    return "DontCare";
            }
        }
        //*PairWidth에 대하여
        //PairWidth는 엣지모드가 페어방식일때만 사용하며 페어방식에서 엣지0과 엣지1을 찾았을때 실제 라인을 어디에 긋기를 할지 설정해 주는 부분이다.
        //현재와 같이 value값을 -와 +로 해 놓으면 두개의 엣지를 찾고 그 중간에 엣지라인을 긋게 된다.
        //만약 엣지0포지션을 0,엣지1포지션에 값을 주면 엣지0의 위치에 라인을 긋는다.따라서 엣지0과1의 중간에 라인을 긋지 않게 되는것이다.
        //반듯이 엣지모드가 페어방식일때만 사용하며 싱글일때는 반듯이 Edge0Position의 값을 0으로 만들어 주어야 한다.-SaveTool부분에 프로그램 추가예정
        public double PairWidth
        {
            get
            {
                //return Math.Abs(m_FindCircleTool.RunParams.CaliperRunParams.Edge0Position - m_FindCircleTool.RunParams.CaliperRunParams.Edge1Position);
                return m_FindCircleTool.RunParams.CaliperRunParams.Edge1Position;
            }
            set
            {
                m_FindCircleTool.RunParams.CaliperRunParams.Edge0Position = -value;
                m_FindCircleTool.RunParams.CaliperRunParams.Edge1Position = value;
            }
        }

        public double MaxRadius
        {
            get
            {
                return m_MaxRadius;
            }
            set
            {
                m_MaxRadius = value;
            }
        }

        public double MinRadius
        {
            get
            {
                return m_MinRadius;
            }
            set
            {
                m_MinRadius = value;
            }
        }

		//원래 여기 있었음

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

		public int NumCalipers
		{
			get
			{
                return m_FindCircleTool.RunParams.NumCalipers;
			}
			set
			{
                m_FindCircleTool.RunParams.NumCalipers = value;				
			}
		}

        public int NumIgnores
        {
            get
            {
                return m_FindCircleTool.RunParams.NumToIgnore;
            }
            set
            {
                m_FindCircleTool.RunParams.NumToIgnore = value;                
            }
        }

        public int ContrastThreshold
        {
            get
            {
                return (int) m_FindCircleTool.RunParams.CaliperRunParams.ContrastThreshold;
            }
            set
            {
                m_FindCircleTool.RunParams.CaliperRunParams.ContrastThreshold = value;
            }
        }

		public string Direction
		{
			get
			{
                if (m_FindCircleTool.RunParams.CaliperSearchDirection == Cognex.VisionPro.Caliper.CogFindCircleSearchDirectionConstants.Inward)
					return "InWard";
				else
					return "OutWard";
			}
			set
			{
				if(value=="InWard")
                    m_FindCircleTool.RunParams.CaliperSearchDirection = Cognex.VisionPro.Caliper.CogFindCircleSearchDirectionConstants.Inward;
				else
                    m_FindCircleTool.RunParams.CaliperSearchDirection = Cognex.VisionPro.Caliper.CogFindCircleSearchDirectionConstants.Outward;				
			}
		}

		public int CaliperSearchLength
		{
			get
			{
                return (int) m_FindCircleTool.RunParams.CaliperSearchLength;
			}
			set
			{
                m_FindCircleTool.RunParams.CaliperSearchLength = value;		
			}			
		}

        public int CaliperProjectionLength
        {
            get
            {
                return (int)m_FindCircleTool.RunParams.CaliperProjectionLength;
            }
            set
            {
                m_FindCircleTool.RunParams.CaliperProjectionLength = value;
            }
        }

        public int FilterHalfSizeInPixels
        {
            get
            {
                return m_FindCircleTool.RunParams.CaliperRunParams.FilterHalfSizeInPixels;
            }
            set
            {
                m_FindCircleTool.RunParams.CaliperRunParams.FilterHalfSizeInPixels = value;
            }
        }

        public double Radius
        {
            get
            {
                return m_FindCircleTool.RunParams.ExpectedCircularArc.Radius;
            }
            set
            {
                m_FindCircleTool.RunParams.ExpectedCircularArc.Radius = value;
            }
        }

		#region Vision Tool Function
		public void Display_SettingArea(Cognex.VisionPro.Display.CogDisplay Display)
		{
             m_FindCircleTool.CurrentRecordEnable = CogFindCircleCurrentRecordConstants.All;
			if(m_CenterX!=0 && m_CenterY!=0)
			{
				m_FindCircleTool.RunParams.ExpectedCircularArc.CenterX = m_CenterX;
				m_FindCircleTool.RunParams.ExpectedCircularArc.CenterY = m_CenterY;
			}

            m_FindCircleTool.RunParams.ExpectedCircularArc.DisplayMode = Cognex.VisionPro.CogCircularArcDisplayModeConstants.Mathematical;
            m_FindCircleTool.RunParams.ExpectedCircularArc.GraphicDOFEnable = Cognex.VisionPro.CogCircularArcDOFConstants.All;
			m_FindCircleTool.RunParams.ExpectedCircularArc.Interactive = true;
			Display.InteractiveGraphics.Add(m_FindCircleTool.RunParams.ExpectedCircularArc,"", true);
            
		}
        public void Display_SettingArea(Cognex.VisionPro.Display.CogDisplay Display, bool GraphicEnable)
        {

            
            CogCircularArc myArc;
            CogGraphicCollection myRegions;
            ICogRecord myRec;

            m_FindCircleTool.InputImage = (CogImage8Grey) Display.Image;

            m_FindCircleTool.CurrentRecordEnable = CogFindCircleCurrentRecordConstants.All;

            //m_FindCircleTool.RunParams.ExpectedCircularArc.SelectedSpaceName = "Use Input Image Space";

            myRec = m_FindCircleTool.CreateCurrentRecord();

            if (m_CenterX != 0 && m_CenterY != 0)
            {
                m_FindCircleTool.RunParams.ExpectedCircularArc.CenterX = m_CenterX;
                m_FindCircleTool.RunParams.ExpectedCircularArc.CenterY = m_CenterY;
                m_FindCircleTool.RunParams.ExpectedCircularArc.AngleStart = m_AngleStart;
                m_FindCircleTool.RunParams.ExpectedCircularArc.AngleSpan = m_AngleSpan;
                
            }

            
            
            myArc = (CogCircularArc)myRec.SubRecords["InputImage"].SubRecords["ExpectedShapeSegment"].Content;

            myRegions = (CogGraphicCollection)myRec.SubRecords["InputImage"].SubRecords["CaliperRegions"].Content;
            
            //  Add the graphics to the display
            Display.InteractiveGraphics.Add(myArc, "", false);
            foreach (ICogGraphic g in myRegions)
                Display.InteractiveGraphics.Add((ICogGraphicInteractive)g, "", false);

           
        }

		public double Find_Circle(Cognex.VisionPro.Display.CogDisplay Display, bool bSearchAreaView)
		{	
			try
			{
				// 캡춰된 이미지 설정
				if(m_Image==null)
				{
					DrawLabel(String.Format("검사 할 이미지가 없습니다."), Display, 5, 5, 12, CogColorConstants.Red, CogColorConstants.Black);
					return -1;
				}

				m_FindCircleTool.InputImage= (Cognex.VisionPro.CogImage8Grey) m_Image;

				if(m_CenterX!=0 && m_CenterY!=0)
				{
					m_FindCircleTool.RunParams.ExpectedCircularArc.CenterX = m_CenterX;
					m_FindCircleTool.RunParams.ExpectedCircularArc.CenterY = m_CenterY;
				}
				//검사 수행
				m_FindCircleTool.Run();
				
				#region 검사 결과 디스플레이
                if (m_FindCircleTool.Results.GetCircle() != null)
                {
                    m_TranslationX = m_FindCircleTool.Results.GetCircle().CenterX;
                    m_TranslationY = m_FindCircleTool.Results.GetCircle().CenterY;
                    m_Radius = m_FindCircleTool.Results.GetCircle().Radius;

                    Cognex.VisionPro.ICogGraphic ResultGp;

                    //ResultGp = (Cognex.VisionPro.ICogGraphic)m_FindCircleTool.Results.GetCircle();                    
                    ResultGp = (Cognex.VisionPro.ICogGraphic)m_FindCircleTool.Results.GetCircularArc(); 
                    

                    ResultGp.Color = Cognex.VisionPro.CogColorConstants.Green;

                    Display.StaticGraphics.Add(ResultGp, "");


                    double abc = m_FindCircleTool.Results[25].X;

                }
                else
                {
                    m_Radius = 0;
                }
				#endregion
			}
			catch
			{
			}

			return m_Radius;
		}

        //리턴값은 NG개수
        public int Find_CircleEdge(Cognex.VisionPro.Display.CogDisplay Display, bool bSearchAreaView)
        {
            try
            {
                // 캡춰된 이미지 설정
                if (m_Image == null)
                {
                    DrawLabel(String.Format("검사 할 이미지가 없습니다."), Display, 5, 5, 12, CogColorConstants.Red, CogColorConstants.Black);
                    return -1;
                }

                m_FindCircleTool.InputImage = (Cognex.VisionPro.CogImage8Grey)m_Image;

                if (m_CenterX != 0 && m_CenterY != 0)
                {
                    m_FindCircleTool.RunParams.ExpectedCircularArc.CenterX = m_CenterX;
                    m_FindCircleTool.RunParams.ExpectedCircularArc.CenterY = m_CenterY;
                }
                //검사 수행
                m_FindCircleTool.Run();

                #region 검사 결과 디스플레이
                if (m_FindCircleTool.Results.GetCircle() != null)
                {
                    m_TranslationX = m_FindCircleTool.Results.GetCircle().CenterX;
                    m_TranslationY = m_FindCircleTool.Results.GetCircle().CenterY;
                    m_Radius = m_FindCircleTool.Results.GetCircle().Radius;

                    Cognex.VisionPro.ICogGraphic ResultGp;
                
                    ResultGp = (Cognex.VisionPro.ICogGraphic)m_FindCircleTool.Results.GetCircularArc();

                    ResultGp.Color = Cognex.VisionPro.CogColorConstants.Green;

                    Display.StaticGraphics.Add(ResultGp, "");

                    int Temp_Count = 0;
                    int TempTemp_Count=0;

                    bool Temp_StartOn=false;
                    
                    int a = m_FindCircleTool.Results.Count;

                    for (int i = 0; i<m_FindCircleTool.RunParams.NumCalipers; i++)
                    {

                        if (m_FindCircleTool.Results[i].Used == false)
                        {
                            //만약 컨티뉴스 모드이면
                            if (m_InspectionMethod == InspectionMethod.Continuous)
                            {
                                
                                if (Temp_StartOn == false)
                                {
                                    //앞에도 NG가 발생했으므로 그냥 덧셈을 실시한다.
                                    Temp_Count++;
                                    Temp_StartOn = false;
                                }
                                else
                                {

                                    if (Temp_Count > TempTemp_Count)
                                    {
                                        //앞에서 OK가 발생했으므로 TempTemp_Count에 Temp_Count를 저장시키고
                                        TempTemp_Count = Temp_Count;
                                    }
                                   
                                    //템프카운트를 0으로 만들고
                                    Temp_Count = 0;
                                    //다시 덧셈을 실시한다.
                                    Temp_Count++;
                                    Temp_StartOn = false;
                                }

                            }
                            else
                            {
                                Temp_Count++;
                            }
                        }
                        else
                        {
                            //m_FindCircleTool.Results[i]==true일때만 X,Y값이 있으므로 true일때만 값을 가져 와야 한다.아니면 에러발생
                            //Arraylist에 결과 값들을 넣기
                            X_Position.Add(m_FindCircleTool.Results[i].X);
                            Y_Position.Add(m_FindCircleTool.Results[i].Y);

                            CogPointMarker m_PointPosition = new CogPointMarker();
                            m_PointPosition.X = m_FindCircleTool.Results[i].X;
                            m_PointPosition.Y = m_FindCircleTool.Results[i].Y;
                            m_PointPosition.GraphicDOFEnable = Cognex.VisionPro.CogPointMarkerDOFConstants.All;
                            m_PointPosition.Interactive = true;
                            Display.InteractiveGraphics.Add(m_PointPosition, "", true);

                            Find_Edge.Add(m_FindCircleTool.Results[i].Used);
                            Temp_StartOn = true;
                        }

                        

                    }
                    //만약 컨티뉴스 모드이면
                    if (m_InspectionMethod == InspectionMethod.Continuous)
                    {
                        if (Temp_Count >= TempTemp_Count)
                            NGCount = Temp_Count;
                        else
                            NGCount = TempTemp_Count;

                    }
                    else
                    {
                        NGCount = Temp_Count;
                    }


                }
                else
                {
                    //만약 NG가 발생하면 스캔한 카운트 개수를 리턴하도록 하는 부분
                    NGCount = m_FindCircleTool.RunParams.NumCalipers;
                    return NGCount;
                }
                #endregion
            }
            catch
            {
            }

            return NGCount;
        }
		
		public void ReleaseTool()
		{
			if(m_FindCircleTool!=null)
			{
				m_FindCircleTool.Dispose();
				m_FindCircleTool = null;
			}
		}



		public void LoadTool(string Path)
		{
			try
			{
				m_FindCircleTool = (Cognex.VisionPro.Caliper.CogFindCircleTool) Cognex.VisionPro.CogSerializer.LoadObjectFromFile(Path);				
			}
			catch
			{
				m_FindCircleTool = new Cognex.VisionPro.Caliper.CogFindCircleTool();


                m_FindCircleTool.RunParams.NumCalipers = 50;
                m_FindCircleTool.RunParams.NumToIgnore = 0;
                m_FindCircleTool.CurrentRecordEnable = CogFindCircleCurrentRecordConstants.All;
                m_FindCircleTool.RunParams.CaliperRunParams.ContrastThreshold = 50;
                m_FindCircleTool.RunParams.CaliperRunParams.FilterHalfSizeInPixels = 10;
                m_FindCircleTool.RunParams.CaliperRunParams.EdgeMode = CogCaliperEdgeModeConstants.SingleEdge;
               
				m_FindCircleTool.RunParams.ExpectedCircularArc.SetCenterRadiusAngleStartAngleSpan(320, 240, 200, 0, 180);

                SaveTool(Path);
			}
		}

		public void SaveTool(string Path)
		{
			#region 검사 파라미터 설정
            m_FindCircleTool.RunParams.CaliperRunParams.SingleEdgeScorers.Clear();            
            Cognex.VisionPro.Caliper.CogCaliperScorerPositionNeg ScoreFunction = new Cognex.VisionPro.Caliper.CogCaliperScorerPositionNeg();
            ScoreFunction.Enabled = true;
            Cognex.VisionPro.Caliper.CogCaliperScorerContrast ContrastFunction = new Cognex.VisionPro.Caliper.CogCaliperScorerContrast();

            m_FindCircleTool.CurrentRecordEnable = CogFindCircleCurrentRecordConstants.All;
            
            ContrastFunction.Enabled = true;
            m_FindCircleTool.RunParams.CaliperRunParams.SingleEdgeScorers.Add(ScoreFunction);
            m_FindCircleTool.RunParams.CaliperRunParams.SingleEdgeScorers.Add(ContrastFunction);       

			m_CenterX = m_FindCircleTool.RunParams.ExpectedCircularArc.CenterX;
			m_CenterY = m_FindCircleTool.RunParams.ExpectedCircularArc.CenterY;
            m_AngleSpan = m_FindCircleTool.RunParams.ExpectedCircularArc.AngleSpan;
            m_AngleStart = m_FindCircleTool.RunParams.ExpectedCircularArc.AngleStart;


            if (m_FindCircleTool.RunParams.CaliperRunParams.EdgeMode == CogCaliperEdgeModeConstants.SingleEdge)
            {
                m_FindCircleTool.RunParams.CaliperRunParams.Edge0Position = 0;
            }
			#endregion

            string[] str = Path.Split('\\');

            if (!System.IO.Directory.Exists(Path.Substring(0, Path.Length - str[str.Length - 1].Length)))
            {
                System.IO.Directory.CreateDirectory(Path.Substring(0, Path.Length - str[str.Length - 1].Length));
            }
			
			Cognex.VisionPro.CogSerializer.SaveObjectToFile(m_FindCircleTool, Path);
		}
		#endregion
	}
}