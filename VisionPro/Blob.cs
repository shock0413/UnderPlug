using System;
using System.IO;

using Cognex.VisionPro;
using Cognex.VisionPro.Exceptions;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.Blob;

namespace Hansero.VisionLib.VisionPro
{
	/// <summary>
	/// Class1에 대한 요약 설명입니다.
	/// </summary>
	public class Blob : ToolBase
	{
		Cognex.VisionPro.Blob.CogBlobTool m_CogBlobTool = new CogBlobTool();

		// 영역의 최소, 최대값 설정
		private double m_MinArea;
		private double m_MaxArea;

		public double MinArea
		{
			get
			{
				return m_MinArea;
			}		
			set
			{
				m_MinArea = value;
			}
		}

		public double MaxArea
		{
			get
			{
				return m_MaxArea;
			}		
			set
			{
				m_MaxArea = value;
			}
		}

		private int m_MinThresh;
		private int m_MaxThresh;

		public int MinThresh
		{
			get
			{
				return m_MinThresh;
			}
			set
			{
				m_MinThresh = value;
			}
		}

		public int MaxThresh
		{
			get
			{
				return m_MaxThresh;
			}
			set
			{
				m_MaxThresh = value;
			}
		}

		private string m_BlobKind;

		public string BlobKind
		{
			set
			{
				m_BlobKind = value;
			}
			get
			{
				return m_BlobKind;
			}
		}

		private int m_Clipped;

		public int Clipped
		{
			get
			{
				return m_Clipped;
			}
			set
			{
				m_Clipped = value;
			}
		}

        private double m_Acircularity;

        public double Acircularity
        {
            get
            {
                return m_Acircularity;
            }
        }
        
		public Blob()
		{
			//
			// TODO: 여기에 생성자 논리를 추가합니다.
			//			
		}

		private double m_TranslationX;
		private double m_TranslationY;

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

		#region Vision Tool Function	
		public int Find_Blob(Cognex.VisionPro.Display.CogDisplay Display, bool bViewArea)
		{
			try
			{
				// 캡춰된 이미지 설정
				if(m_Image==null)
				{
					DrawLabel(String.Format("검사 할 이미지가 없습니다."), Display, 5, 5, 12, CogColorConstants.Red, CogColorConstants.Black);
					return -1;
				}

				m_CogBlobTool.InputImage = (Cognex.VisionPro.CogImage8Grey) m_Image;

                m_CogBlobTool.RunParams.SegmentationParams.HardFixedThreshold = m_MinThresh;

				if(m_CenterX!=0 && m_CenterY!=0)
				{
                    // 기준 위치로 검사 영역 이동
                    if (m_RegionShape == RegionShape.Rectangle)
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
                    else if (m_RegionShape == RegionShape.Ring)
                    {
                        m_RingRegion = (CogCircularAnnulusSection)m_Region;                         
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

					m_CogBlobTool.Region = m_Region;
				}

				//검사 수행
				m_CogBlobTool.Run();
				
				#region 검사 결과 디스플레이
				Cognex.VisionPro.ICogGraphic ResultGp;

				// 검사 영역 표시
				if(bViewArea==true)
				{
					if(m_CogBlobTool.Region!=null)
					{
						ResultGp = (Cognex.VisionPro.ICogGraphic) m_CogBlobTool.Region;
					
						ResultGp.Color = Cognex.VisionPro.CogColorConstants.Blue;					
										
						Display.StaticGraphics.Add(ResultGp, "");

                        ResultGp = null;
					}
				}

                double s = 10;
                m_TranslationX = 0;
				m_TranslationY = 0;
                m_Acircularity = 0;

                for (int i = 0; i < m_CogBlobTool.Results.GetBlobs().Count; i++)
                {
                    if(Math.Abs(m_CogBlobTool.Results.GetBlobs()[i].Acircularity - 1) < s)
                    {
                        s = m_CogBlobTool.Results.GetBlobs()[i].Acircularity;
                        m_Acircularity = m_CogBlobTool.Results.GetBlobs()[i].Acircularity;
                        m_TranslationX = m_CogBlobTool.Results.GetBlobs()[0].CenterOfMassX;
					    m_TranslationY = m_CogBlobTool.Results.GetBlobs()[0].CenterOfMassY;
                        
                    }
                }				
				
				for(int i=0; i<m_CogBlobTool.Results.GetBlobs().Count; i++)
				{
					ResultGp = m_CogBlobTool.Results.GetBlobs()[i].CreateResultGraphics(Cognex.VisionPro.Blob.CogBlobResultGraphicConstants.Boundary | Cognex.VisionPro.Blob.CogBlobResultGraphicConstants.CenterOfMass);

					ResultGp.Color = Cognex.VisionPro.CogColorConstants.Green;

					Display.StaticGraphics.Add( ResultGp, "");

                    DrawLabel(String.Format("{0:0}", m_CogBlobTool.Results.GetBlobs()[i].Area), Display, m_TranslationX, m_TranslationY, 8, CogColorConstants.Green, CogColorConstants.Black);

                    ResultGp = null;
				}
				
				#endregion
			}
			catch
			{
			}			
			
			return m_CogBlobTool.Results.GetBlobs().Count;
		}

		public void ReleaseTool()
		{
			if(m_CogBlobTool!=null)
			{
				m_CogBlobTool.Dispose();
				m_CogBlobTool = null;
			}
		}

		public void LoadTool(string Path)
		{
			try
			{
				m_CogBlobTool = (Cognex.VisionPro.Blob.CogBlobTool) Cognex.VisionPro.CogSerializer.LoadObjectFromFile(Path);

				m_Region = m_CogBlobTool.Region;

				if(m_CogBlobTool.RunParams.SegmentationParams.Polarity==Cognex.VisionPro.Blob.CogBlobSegmentationPolarityConstants.LightBlobs)					
					m_BlobKind = "WHITE";
				else
					m_BlobKind = "BLACK";

				m_MinThresh = m_CogBlobTool.RunParams.SegmentationParams.SoftRelativeThresholdLow;
                m_MinThresh = m_CogBlobTool.RunParams.SegmentationParams.HardFixedThreshold;
				m_MaxThresh = m_CogBlobTool.RunParams.SegmentationParams.SoftRelativeThresholdHigh;

				m_Clipped = 0;

				for(int i=0; i<m_CogBlobTool.RunParams.RunTimeMeasures.Count; i++)
				{
					if(m_CogBlobTool.RunParams.RunTimeMeasures[i].Measure==Cognex.VisionPro.Blob.CogBlobMeasureConstants.Area)
					{
						m_MinArea = m_CogBlobTool.RunParams.RunTimeMeasures[i].FilterRangeLow;
						m_MaxArea = m_CogBlobTool.RunParams.RunTimeMeasures[i].FilterRangeHigh;
					}

					if(m_CogBlobTool.RunParams.RunTimeMeasures[i].Measure==Cognex.VisionPro.Blob.CogBlobMeasureConstants.NotClipped)
					{
						m_Clipped = 1;
					}
				}
			}
			catch
			{
				m_CogBlobTool = new CogBlobTool();

				if(m_RegionShape==RegionShape.Rectangle)
				{
					m_Region = new CogRectangleAffine();
				}
				else if(m_RegionShape==RegionShape.Circle)
				{
					m_Region = new CogCircle();
				}
				else if(m_RegionShape==RegionShape.Polygon)
				{
					m_Region = (ICogRegion) new CogPolygon();
				}
                else if (m_RegionShape == RegionShape.Ring)
                {
                    m_Region = new CogCircularAnnulusSection();
                }

                //m_MinThresh = 40;
                //m_MaxThresh = 60;
				m_Clipped = 1;
				m_MinArea = 100;
				m_MaxArea = 1000;
                m_BlobKind = "White";

				m_CogBlobTool.RunParams.SegmentationParams.Mode = Cognex.VisionPro.Blob.CogBlobSegmentationModeConstants.HardFixedThreshold;
                m_CogBlobTool.RunParams.SegmentationParams.HardFixedThreshold = 40;
				//m_CogBlobTool.RunParams.SegmentationParams.Mode = Cognex.VisionPro.Blob.CogBlobSegmentationModeConstants.SoftFixedThreshold;
				
				m_CogBlobTool.RunParams.SegmentationParams.Polarity = Cognex.VisionPro.Blob.CogBlobSegmentationPolarityConstants.LightBlobs;
				
				//m_CogBlobTool.RunParams.RunTimeMeasures.Clear();

                //// 블랍 영역 크기 설정
                //CogBlobMeasure myMeasure = new CogBlobMeasure();
                //myMeasure.Measure = Cognex.VisionPro.Blob.CogBlobMeasureConstants.Area;
                //myMeasure.FilterMode = Cognex.VisionPro.Blob.CogBlobFilterModeConstants.IncludeBlobsInRange;
                //myMeasure.FilterRangeLow = 100;
                //myMeasure.FilterRangeHigh = 1000;
                //myMeasure.Mode = Cognex.VisionPro.Blob.CogBlobMeasureModeConstants.Filter;

                //m_CogBlobTool.RunParams.RunTimeMeasures.Add(myMeasure);

                //// 가로 세로 크기 비슷한것 찾기
                //myMeasure = new CogBlobMeasure();
                //myMeasure.Measure = Cognex.VisionPro.Blob.CogBlobMeasureConstants.Acircularity;
                //myMeasure.FilterMode = Cognex.VisionPro.Blob.CogBlobFilterModeConstants.IncludeBlobsInRange;
                //myMeasure.FilterRangeLow = 0.9;
                //myMeasure.FilterRangeHigh = 1.1;
                //myMeasure.Mode = Cognex.VisionPro.Blob.CogBlobMeasureModeConstants.Filter;

                //m_CogBlobTool.RunParams.RunTimeMeasures.Add(myMeasure);
				
                //myMeasure = new CogBlobMeasure();

                //myMeasure.Measure = Cognex.VisionPro.Blob.CogBlobMeasureConstants.NotClipped;
                //myMeasure.FilterMode = Cognex.VisionPro.Blob.CogBlobFilterModeConstants.IncludeBlobsInRange;
                //myMeasure.FilterRangeLow = 1;
                //myMeasure.FilterRangeHigh = 1;
                //myMeasure.Mode = Cognex.VisionPro.Blob.CogBlobMeasureModeConstants.Filter;

				//m_CogBlobTool.RunParams.RunTimeMeasures.Add(myMeasure);				
			}
		}

		public void SaveTool(string Path)
		{
			m_CogBlobTool.RunParams.SegmentationParams.Mode = Cognex.VisionPro.Blob.CogBlobSegmentationModeConstants.HardFixedThreshold;
            m_CogBlobTool.RunParams.SegmentationParams.HardFixedThreshold = m_MinThresh;
            //m_CogBlobTool.RunParams.SegmentationParams.Mode = Cognex.VisionPro.Blob.CogBlobSegmentationModeConstants.SoftFixedThreshold;
            //m_CogBlobTool.RunParams.SegmentationParams.SoftFixedThresholdLow = m_MinThresh;
            //m_CogBlobTool.RunParams.SegmentationParams.SoftFixedThresholdHigh = m_MaxThresh;

			if(m_BlobKind.ToUpper()=="WHITE")
				m_CogBlobTool.RunParams.SegmentationParams.Polarity = Cognex.VisionPro.Blob.CogBlobSegmentationPolarityConstants.LightBlobs;
			else
				m_CogBlobTool.RunParams.SegmentationParams.Polarity = Cognex.VisionPro.Blob.CogBlobSegmentationPolarityConstants.DarkBlobs;

			m_CogBlobTool.Region = m_Region;

			m_CogBlobTool.RunParams.RunTimeMeasures.Clear();

			// 블랍 영역 크기 설정
			CogBlobMeasure myMeasure = new CogBlobMeasure();
			myMeasure.Measure = Cognex.VisionPro.Blob.CogBlobMeasureConstants.Area;
			myMeasure.FilterMode = Cognex.VisionPro.Blob.CogBlobFilterModeConstants.IncludeBlobsInRange;
			myMeasure.FilterRangeLow = m_MinArea;
			myMeasure.FilterRangeHigh = m_MaxArea;
			myMeasure.Mode = Cognex.VisionPro.Blob.CogBlobMeasureModeConstants.Filter;

			m_CogBlobTool.RunParams.RunTimeMeasures.Add(myMeasure);

			// 원에 가까운 정도 제한
            //myMeasure = new CogBlobMeasure();
            //myMeasure.Measure = Cognex.VisionPro.Blob.CogBlobMeasureConstants.Acircularity;
            //myMeasure.FilterMode = Cognex.VisionPro.Blob.CogBlobFilterModeConstants.IncludeBlobsInRange;
            //myMeasure.FilterRangeLow = 0.5;
            //myMeasure.FilterRangeHigh = 1.5;
            //myMeasure.Mode = Cognex.VisionPro.Blob.CogBlobMeasureModeConstants.Filter;

            //m_CogBlobTool.RunParams.RunTimeMeasures.Add(myMeasure);

            // 블랍만 찾기
            myMeasure = new CogBlobMeasure();
            myMeasure.Measure = Cognex.VisionPro.Blob.CogBlobMeasureConstants.Label;
            myMeasure.FilterMode = Cognex.VisionPro.Blob.CogBlobFilterModeConstants.IncludeBlobsInRange;
            myMeasure.FilterRangeLow = 1;
            myMeasure.FilterRangeHigh = 1;
            myMeasure.Mode = Cognex.VisionPro.Blob.CogBlobMeasureModeConstants.Filter;

            m_CogBlobTool.RunParams.RunTimeMeasures.Add(myMeasure);


            //// 닫힌 영역만 찾기
            //if (m_Clipped == 1)
            //{
            //    myMeasure = new CogBlobMeasure();
            //    myMeasure.Measure = Cognex.VisionPro.Blob.CogBlobMeasureConstants.NotClipped;
            //    myMeasure.FilterMode = Cognex.VisionPro.Blob.CogBlobFilterModeConstants.IncludeBlobsInRange;
            //    myMeasure.FilterRangeLow = 1;
            //    myMeasure.FilterRangeHigh = 1;
            //    myMeasure.Mode = Cognex.VisionPro.Blob.CogBlobMeasureModeConstants.Filter;

            //    m_CogBlobTool.RunParams.RunTimeMeasures.Add(myMeasure);
            //}

			Cognex.VisionPro.CogSerializer.SaveObjectToFile(m_CogBlobTool, Path);
		}
		
		public void LoadSearchArea(string Path, string Name)
		{
			m_SearchArea = VisionProConfigLoad.DataLoad.Load_Area(Path, Name);		
		}

		public void SaveSearchArea(string Path, string Name)
		{
			VisionProConfigLoad.DataLoad.Save_Area(Path, Name, m_SearchArea);
		}
		#endregion
	}
}