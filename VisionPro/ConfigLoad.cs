using System;
using System.Collections;

namespace Hansero.VisionProConfigLoad
{
	public class CameraInfo : INIManager
	{
		private int m_BoardNum;
		private int m_Mux;
		private int m_Channel;		
		private double m_Brightness;
		private double m_Contrast;
		private double m_MMperPixelX;
		private double m_MMperPixelY;
		private string m_CoodinateType;
		private string m_CoodinateValue;

		public int BoardNum
		{
			get
			{
				return m_BoardNum;
			}
			set
			{
				m_BoardNum = value;
			}
		}

		public int MuxNum
		{
			get
			{
				return m_Mux;
			}
			set
			{
				m_Mux = value;
			}
		}

		public int Channel
		{
			get
			{
				return m_Channel;
			}
			set
			{
				m_Channel = value;
			}
		}

		public double Brightness
		{
			get
			{
				return m_Brightness;
			}
			set
			{
				m_Brightness = value;
			}
		}

		public double Contrast
		{
			get
			{
				return m_Contrast;
			}
			set
			{
				m_Contrast = value;
			}
		}

		public double MMperPixelX
		{
			get
			{
				return m_MMperPixelX;
			}
			set
			{
				m_MMperPixelX = value;
			}
		}

		public double MMperPixelY
		{
			get
			{
				return m_MMperPixelY;
			}
			set
			{
				m_MMperPixelY = value;
			}
		}

		public string CoodinateType
		{
			get
			{
				return m_CoodinateType;
			}
			set
			{
				m_CoodinateType = value;
			}
		}

		public string CoodinateValue
		{
			get
			{
				return m_CoodinateValue;
			}
			set
			{
				m_CoodinateValue = value;
			}
		}

		public CameraInfo()
		{
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected virtual void Dispose( )
		{	
		}
	}

	/// <summary>
	/// Summary description for CarINIManager.
	/// </summary>
	public class DataLoad : INIManager
	{
		public DataLoad()
		{
			//
			// TODO: Add constructor logic here
			//
		}		

		/// <summary>
		/// 검사 영역을 불러오는 함수
		/// </summary>
		/// <param name="ToolType">툴의 종류</param>
		/// <param name="Name">툴 네임. "차종\이름\하위이름" 형식. </param>
		/// <returns></returns>
		public static Cognex.VisionPro.CogRectangleAffine Load_Area(string Path)
		{
			Cognex.VisionPro.CogRectangleAffine rec = new Cognex.VisionPro.CogRectangleAffine();

			try
			{
				rec.CenterX = double.Parse(IniReadValue("Search Area", "CenterX", Path + ".ini"));				
				rec.CenterY = double.Parse(IniReadValue("Search Area", "CenterY", Path + ".ini"));
				rec.SideXLength = double.Parse(IniReadValue("Search Area", "Width", Path + ".ini"));
				rec.SideYLength = double.Parse(IniReadValue("Search Area", "Height", Path + ".ini"));				
				rec.Rotation = double.Parse(IniReadValue("Search Area", "Rotation", Path + ".ini"));
			}
			catch
			{
				rec.CenterX = 320;
				rec.CenterY = 240;
				rec.SideXLength = 100;
				rec.SideYLength = 100;
				rec.Rotation = 0;
			}

			return rec;
		}

		public static Cognex.VisionPro.CogRectangleAffine Load_Area(string Path, string Name)
		{
			Cognex.VisionPro.CogRectangleAffine rec = new Cognex.VisionPro.CogRectangleAffine();

			try
			{
				rec.CenterX = double.Parse(IniReadValue(Name, "CenterX", Path));				
				rec.CenterY = double.Parse(IniReadValue(Name, "CenterY", Path));
				rec.SideXLength = double.Parse(IniReadValue(Name, "Width", Path));
				rec.SideYLength = double.Parse(IniReadValue(Name, "Height", Path));				
				rec.Rotation = double.Parse(IniReadValue(Name, "Rotation", Path));
			}
			catch
			{
				rec.CenterX = 320;
				rec.CenterY = 240;
				rec.SideXLength = 100;
				rec.SideYLength = 100;
				rec.Rotation = 0;
			}

			return rec;
		}

		public static void Save_Area(string Path, Cognex.VisionPro.CogRectangleAffine rec)
		{			
			IniWriteValue("Search Area", "CenterX", rec.CenterX.ToString(), Path + ".ini");
			IniWriteValue("Search Area", "CenterY", rec.CenterY.ToString(), Path + ".ini");
			IniWriteValue("Search Area", "Width", rec.SideXLength.ToString(), Path + ".ini");
			IniWriteValue("Search Area", "Height", rec.SideYLength.ToString(), Path + ".ini");
			IniWriteValue("Search Area", "Rotation", rec.Rotation.ToString(), Path + ".ini");
		}

		public static void Save_Area(string Path, string Name, Cognex.VisionPro.CogRectangleAffine rec)
		{			
			IniWriteValue(Name, "CenterX", rec.CenterX.ToString(), Path);
			IniWriteValue(Name, "CenterY", rec.CenterY.ToString(), Path);
			IniWriteValue(Name, "Width", rec.SideXLength.ToString(), Path);
			IniWriteValue(Name, "Height", rec.SideYLength.ToString(), Path);
			IniWriteValue(Name, "Rotation", rec.Rotation.ToString(), Path);
		}
		
		/// <summary>
		/// 합격 점수 불러 오는 함수
		/// </summary>
		/// <param name="Name">툴 네임. "기종\카메라이름\하위이름" 형식. </param></param>
		/// <returns></returns>
		public static double Load_PassScore(string Path)
		{
			double score;

			try
			{
				// 모든 경로 대문자로 치환
				score =	double.Parse(IniReadValue("Run Param", "PassScore", Path));					
			}
			catch
			{
				score = 0.7;
			}

			return score;
		}

		/// <summary>
		/// 합격 점수 불러 오는 함수
		/// </summary>
		/// <param name="Name">툴 네임. "기종\카메라이름\하위이름" 형식. </param></param>
		/// <returns></returns>
		public static void Save_PassScore(string Path, double Score)
		{
			IniWriteValue("Run Param", "PassScore", Score.ToString(), Path);
		}
	}
}