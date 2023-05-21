using System;
using System.IO;

using Cognex.VisionPro;
using Cognex.VisionPro.Exceptions;
using Cognex.VisionPro.ImageFile;

namespace Hansero.VisionLib.VisionPro
{
	/// <summary>
	/// Class1�� ���� ��� �����Դϴ�.
	/// </summary>
	public class ToolBase
	{	
		// ĸ��� �̹��� ����.
		protected ICogImage m_Image = null;
 
        protected Cognex.VisionPro.ICogRegion m_Region;
		protected Cognex.VisionPro.CogRectangleAffine m_RectangleRegion = new CogRectangleAffine();
		protected Cognex.VisionPro.CogCircle m_CircleRegion = new CogCircle();
		protected Cognex.VisionPro.CogPolygon m_PolygonRegion = new CogPolygon();
        protected Cognex.VisionPro.CogCircularAnnulusSection m_RingRegion = new CogCircularAnnulusSection();

        public ICogRegion Region
        {
            get
            {
                return m_Region;
            }
            set
            {
                m_Region = value;
            }
        }

        private string m_Name;

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

		public enum RegionShape {Rectangle, Circle, Polygon, Ring};

		protected RegionShape m_RegionShape;

		public RegionShape SearchRegionShape
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

		protected double m_CenterX;
		protected double m_CenterY;
        protected double m_AngleStart;
        protected double m_AngleSpan;
        protected double m_SideX;
        protected double m_SideY;

		public double CenterX
		{
			get
			{
				return m_CenterX;
			}
			set
			{
				m_CenterX = value;
			}
		}

		public double CenterY
		{
			get
			{
				return m_CenterY;
			}
			set
			{
				m_CenterY = value;
			}
		}
        public double SideX
        {
            get
            {
                return m_SideX;
            }
            set
            {
                m_SideX = value;
            }
        }

        public double SideY
        {
            get
            {
                return m_SideY;
            }
            set
            {
                m_SideY = value;
            }
        }
        public double AngleStart
        {
            get
            {
                return m_AngleStart;
            }
            set
            {
                m_AngleStart = value;
            }
        }
        public double AngleSpan
        {
            get
            {
                return m_AngleSpan;
            }
            set
            {
                m_AngleSpan = value;
            }
        }

		// �˻翵��
		protected CogRectangleAffine m_SearchArea = new CogRectangleAffine();

		/// <summary>
		/// ĸ���� �̹���.
		/// </summary>
		public ICogImage Image
		{
			get
			{
				return m_Image;
			}
			set
			{
                m_Image = null;
				m_Image = value;
			}
		}	
		
		public ToolBase()
		{
			//
			// TODO: ���⿡ ������ ���� �߰��մϴ�.
			//
		}		
       
		public void DisplaySearchArea(Cognex.VisionPro.Display.CogDisplay Display, bool bEdit)
		{
            //2012�� 5��17�� ��ü������ ������  
            if (m_Region.GetType().ToString().IndexOf("Rectangle") >= 0 )
            {
                try
                {
                    m_RectangleRegion = (CogRectangleAffine)m_Region;

                    m_CenterX = m_RectangleRegion.CenterX;
                    m_CenterY = m_RectangleRegion.CenterY;

                    if (bEdit)
                        m_RectangleRegion.GraphicDOFEnable = CogRectangleAffineDOFConstants.Position | CogRectangleAffineDOFConstants.Rotation | CogRectangleAffineDOFConstants.Size | CogRectangleAffineDOFConstants.Scale;
                    else
                        m_RectangleRegion.GraphicDOFEnable = CogRectangleAffineDOFConstants.None;

                    m_RectangleRegion.Interactive = bEdit;
                    //m_RectangleRegion.Color = CogColorConstants.Red;
                    Display.InteractiveGraphics.Add(m_RectangleRegion, "", bEdit);
                }
                catch
                {
                    return;
                }
            }
            else if (m_Region.GetType().ToString().IndexOf("Circle") >= 0)
            {
                try
                {
                    m_CircleRegion = (CogCircle)m_Region;

                    m_CenterX = m_CircleRegion.CenterX;
                    m_CenterY = m_CircleRegion.CenterY;
                    if (bEdit)
                        m_CircleRegion.GraphicDOFEnable = CogCircleDOFConstants.All;
                    else
                        m_CircleRegion.GraphicDOFEnable = CogCircleDOFConstants.None;

                    m_CircleRegion.Interactive = bEdit;

                    Display.InteractiveGraphics.Add(m_CircleRegion, "", bEdit);
                }
                catch
                {
                    return;
                }

                
            }
            else if (m_Region.GetType().ToString().IndexOf("Polygon") >= 0)
            {
                try
                {
                    m_PolygonRegion = (CogPolygon)m_Region;


                    int NumVertices = m_PolygonRegion.NumVertices;

                    for (int i = 0; i < NumVertices; i++)
                    {
                        m_PolygonRegion.RemoveVertex(0);
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        m_PolygonRegion.AddVertex(200, 200 + 20 * i, i);
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        m_PolygonRegion.AddVertex(300, 280 - 20 * i, i + 5);
                    }
                    if (bEdit)
                        m_PolygonRegion.GraphicDOFEnable = Cognex.VisionPro.CogPolygonDOFConstants.All;
                    else
                        m_PolygonRegion.GraphicDOFEnable = Cognex.VisionPro.CogPolygonDOFConstants.None;

                    m_PolygonRegion.Interactive = bEdit;

                    Display.InteractiveGraphics.Add(m_PolygonRegion, "", bEdit);
                }
                catch
                {
                    return;
                }
            }
            else
            {
                try
                {
                    m_RingRegion = (CogCircularAnnulusSection)m_Region;

                    m_CenterX = m_RingRegion.CenterX;
                    m_CenterY = m_RingRegion.CenterY;
                    if (bEdit)
                        m_RingRegion.GraphicDOFEnable = CogCircularAnnulusSectionDOFConstants.All;
                    else
                        m_RingRegion.GraphicDOFEnable = CogCircularAnnulusSectionDOFConstants.None;

                    m_RingRegion.Interactive = bEdit;

                    Display.InteractiveGraphics.Add(m_RingRegion, "", bEdit);
                }
                catch
                {
                    return;

                }
            }
            
 
		}

        // �˻�����
        public void DisplaySearchArea(Cognex.VisionPro.Display.CogDisplay Display, bool bEdit, RegionShape SelectRegionShape)
        {

            Display.InteractiveGraphics.Clear();
            Display.StaticGraphics.Clear();
            try
            {
                if (m_RegionShape == SelectRegionShape)
                {
                    if (bEdit)
                    {
                        if (m_RegionShape == RegionShape.Rectangle)
                        {
                            m_RectangleRegion = (CogRectangleAffine)m_Region;

                            //if (m_CenterX != 0 && m_CenterY != 0)
                            //{
                            //    m_RectangleRegion.CenterX = m_CenterX;
                            //    m_RectangleRegion.CenterY = m_CenterY;
                            //}

                            ////2012��5��2�� �߰� �̱⹮
                            //m_CenterX = m_RectangleRegion.CenterX;
                            //m_CenterY = m_RectangleRegion.CenterY;

                            m_RectangleRegion.GraphicDOFEnable = CogRectangleAffineDOFConstants.Position | CogRectangleAffineDOFConstants.Rotation | CogRectangleAffineDOFConstants.Size | CogRectangleAffineDOFConstants.Scale;
                            m_RectangleRegion.Interactive = true;

                            Display.InteractiveGraphics.Add(m_RectangleRegion, "", false);
                        }
                        else if (m_RegionShape == RegionShape.Circle)
                        {
                            m_CircleRegion = (CogCircle)m_Region;

                            if (m_CenterX != 0 && m_CenterY != 0)
                            {
                                m_CircleRegion.CenterX = m_CenterX;
                                m_CircleRegion.CenterY = m_CenterY;
                            }

                            m_CircleRegion.GraphicDOFEnable = CogCircleDOFConstants.All;
                            m_CircleRegion.Interactive = true;

                            Display.InteractiveGraphics.Add(m_CircleRegion, "", true);
                        }
                        else if (m_RegionShape == RegionShape.Polygon)
                        {
                            m_PolygonRegion = (CogPolygon)m_Region;


                            int NumVertices = m_PolygonRegion.NumVertices;

                            for (int i = 0; i < NumVertices; i++)
                            {
                                m_PolygonRegion.RemoveVertex(0);
                            }

                            for (int i = 0; i < 5; i++)
                            {
                                m_PolygonRegion.AddVertex(200, 200 + 20 * i, i);
                            }

                            for (int i = 0; i < 5; i++)
                            {
                                m_PolygonRegion.AddVertex(300, 280 - 20 * i, i + 5);
                            }

                            m_PolygonRegion.GraphicDOFEnable = Cognex.VisionPro.CogPolygonDOFConstants.All;
                            m_PolygonRegion.Interactive = true;

                            Display.InteractiveGraphics.Add(m_PolygonRegion, "", true);
                        }
                        else
                        {
                            m_RingRegion = (CogCircularAnnulusSection)m_Region;

                            if (m_CenterX != 0 && m_CenterY != 0)
                            {
                                m_RingRegion.CenterX = m_CenterX;
                                m_RingRegion.CenterY = m_CenterY;
                            }

                            m_RingRegion.GraphicDOFEnable = CogCircularAnnulusSectionDOFConstants.All;
                            m_RingRegion.Interactive = true;

                            Display.InteractiveGraphics.Add(m_RingRegion, "", true);
                        }
                    }
                    else
                    {
                        if (m_RegionShape == RegionShape.Rectangle)
                        {
                            m_RectangleRegion = null;
                            m_RectangleRegion = (CogRectangleAffine)m_Region;

                            if (m_CenterX != 0 && m_CenterY != 0)
                            {
                                m_RectangleRegion.CenterX = m_CenterX;
                                m_RectangleRegion.CenterY = m_CenterY;
                            }

                            m_RectangleRegion.GraphicDOFEnable = CogRectangleAffineDOFConstants.None;
                            m_RectangleRegion.Interactive = false;

                            Display.InteractiveGraphics.Add(m_RectangleRegion, "", true);
                        }
                        else if (m_RegionShape == RegionShape.Circle)
                        {
                            m_CircleRegion = null;
                            m_CircleRegion = (CogCircle)m_Region;

                            if (m_CenterX != 0 && m_CenterY != 0)
                            {
                                m_CircleRegion.CenterX = m_CenterX;
                                m_CircleRegion.CenterY = m_CenterY;
                            }

                            m_CircleRegion.GraphicDOFEnable = CogCircleDOFConstants.None;
                            m_CircleRegion.Interactive = false;

                            Display.InteractiveGraphics.Add(m_CircleRegion, "", true);
                        }
                        else if (m_RegionShape == RegionShape.Polygon)
                        {
                            m_PolygonRegion = (CogPolygon)m_Region;


                            int NumVertices = m_PolygonRegion.NumVertices;

                            for (int i = 0; i < NumVertices; i++)
                            {
                                m_PolygonRegion.RemoveVertex(0);
                            }

                            for (int i = 0; i < 5; i++)
                            {
                                m_PolygonRegion.AddVertex(200, 200 + 20 * i, i);
                            }

                            for (int i = 0; i < 5; i++)
                            {
                                m_PolygonRegion.AddVertex(300, 280 - 20 * i, i + 5);
                            }

                            m_PolygonRegion.GraphicDOFEnable = Cognex.VisionPro.CogPolygonDOFConstants.None;
                            m_PolygonRegion.Interactive = true;

                            Display.InteractiveGraphics.Add(m_PolygonRegion, "", true);
                        }
                        else
                        {

                            m_RingRegion = (CogCircularAnnulusSection)m_Region;

                            if (m_CenterX != 0 && m_CenterY != 0)
                            {
                                m_RingRegion.CenterX = m_CenterX;
                                m_RingRegion.CenterY = m_CenterY;
                            }

                            m_RingRegion.GraphicDOFEnable = CogCircularAnnulusSectionDOFConstants.None;
                            m_RingRegion.Interactive = true;

                            Display.InteractiveGraphics.Add(m_RingRegion, "", true);

                        }

                    }
                }
                else            //���� ���� �˻翵���� Ʋ�� ���...���ο� �˻翵�� ���¸� �����ֱ�
                {

                    //���ο� �˻翵�� ����
                    if (SelectRegionShape == RegionShape.Rectangle)
                    {
                        //���ο� �˻翵�� ������ Ŭ������ �����ϰ�...
                        m_Region = (ICogRegion)new CogRectangleAffine();
                        //���� �˻翵�� �η� �����...
                        m_RectangleRegion = null;
                        //���ο� �˻����¸� �簢�� �������� �ѱ��.
                        m_RectangleRegion = (CogRectangleAffine)m_Region;

                        if (m_CenterX != 0 && m_CenterY != 0)
                        {
                            m_RectangleRegion.CenterX = m_CenterX;
                            m_RectangleRegion.CenterY = m_CenterY;
                        }

                        m_RectangleRegion.GraphicDOFEnable = CogRectangleAffineDOFConstants.All;
                        m_RectangleRegion.Interactive = true;

                        Display.InteractiveGraphics.Add(m_RectangleRegion, "", true);



                    }
                    else if (SelectRegionShape == RegionShape.Circle)
                    {
                        m_Region = (ICogRegion)new CogCircle();
                        m_CircleRegion = null;
                        m_CircleRegion = (CogCircle)m_Region;

                        if (m_CenterX != 0 && m_CenterY != 0)
                        {
                            m_CircleRegion.CenterX = m_CenterX;
                            m_CircleRegion.CenterY = m_CenterY;
                        }

                        m_CircleRegion.GraphicDOFEnable = CogCircleDOFConstants.All;
                        m_CircleRegion.Interactive = true;

                        Display.InteractiveGraphics.Add(m_CircleRegion, "", true);


                    }
                    else if (m_RegionShape == RegionShape.Polygon)
                    {
                        m_PolygonRegion = (CogPolygon)m_Region;


                        int NumVertices = m_PolygonRegion.NumVertices;

                        for (int i = 0; i < NumVertices; i++)
                        {
                            m_PolygonRegion.RemoveVertex(0);
                        }

                        for (int i = 0; i < 5; i++)
                        {
                            m_PolygonRegion.AddVertex(200, 200 + 20 * i, i);
                        }

                        for (int i = 0; i < 5; i++)
                        {
                            m_PolygonRegion.AddVertex(300, 280 - 20 * i, i + 5);
                        }

                        m_PolygonRegion.GraphicDOFEnable = Cognex.VisionPro.CogPolygonDOFConstants.All;
                        m_PolygonRegion.Interactive = true;

                        Display.InteractiveGraphics.Add(m_PolygonRegion, "", true);


                    }
                    else
                    {
                        m_Region = (ICogRegion)new CogCircularAnnulusSection();
                        m_RingRegion = null;
                        m_RingRegion = (CogCircularAnnulusSection)m_Region;

                        if (m_CenterX != 0 && m_CenterY != 0)
                        {
                            m_RingRegion.CenterX = m_CenterX;
                            m_RingRegion.CenterY = m_CenterY;
                        }

                        m_RingRegion.Radius = 200;
                        m_RingRegion.AngleStart = 0;
                        m_RingRegion.AngleSpan = 2 * Math.PI;
                        m_RingRegion.RadialScale = 0.5;

                        m_RingRegion.GraphicDOFEnable = CogCircularAnnulusSectionDOFConstants.All;
                        m_RingRegion.Interactive = true;

                        Display.InteractiveGraphics.Add(m_RingRegion, "", true);


                    }


                }
            }
            catch
            {
                //���ο� �˻翵�� ����
                if (SelectRegionShape == RegionShape.Rectangle)
                {
                    //���ο� �˻翵�� ������ Ŭ������ �����ϰ�...
                    m_Region = (ICogRegion)new CogRectangleAffine();
                    //���� �˻翵�� �η� �����...
                    m_RectangleRegion = null;
                    //���ο� �˻����¸� �簢�� �������� �ѱ��.
                    m_RectangleRegion = (CogRectangleAffine)m_Region;

                    if (m_CenterX != 0 && m_CenterY != 0)
                    {
                        m_RectangleRegion.CenterX = m_CenterX;
                        m_RectangleRegion.CenterY = m_CenterY;
                    }

                    m_RectangleRegion.GraphicDOFEnable = CogRectangleAffineDOFConstants.All;
                    m_RectangleRegion.Interactive = true;

                    Display.InteractiveGraphics.Add(m_RectangleRegion, "", true);



                }
                else if (SelectRegionShape == RegionShape.Circle)
                {
                    m_Region = (ICogRegion)new CogCircle();
                    m_CircleRegion = null;
                    m_CircleRegion = (CogCircle)m_Region;

                    if (m_CenterX != 0 && m_CenterY != 0)
                    {
                        m_CircleRegion.CenterX = m_CenterX;
                        m_CircleRegion.CenterY = m_CenterY;
                    }

                    m_CircleRegion.GraphicDOFEnable = CogCircleDOFConstants.All;
                    m_CircleRegion.Interactive = true;

                    Display.InteractiveGraphics.Add(m_CircleRegion, "", true);


                }
                else if (m_RegionShape == RegionShape.Polygon)
                {
                    m_PolygonRegion = (CogPolygon)m_Region;


                    int NumVertices = m_PolygonRegion.NumVertices;

                    for (int i = 0; i < NumVertices; i++)
                    {
                        m_PolygonRegion.RemoveVertex(0);
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        m_PolygonRegion.AddVertex(200, 200 + 20 * i, i);
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        m_PolygonRegion.AddVertex(300, 280 - 20 * i, i + 5);
                    }

                    m_PolygonRegion.GraphicDOFEnable = Cognex.VisionPro.CogPolygonDOFConstants.All;
                    m_PolygonRegion.Interactive = true;

                    Display.InteractiveGraphics.Add(m_PolygonRegion, "", true);


                }
                else
                {
                    m_Region = (ICogRegion)new CogCircularAnnulusSection();
                    m_RingRegion = null;
                    m_RingRegion = (CogCircularAnnulusSection)m_Region;

                    if (m_CenterX != 0 && m_CenterY != 0)
                    {
                        m_RingRegion.CenterX = m_CenterX;
                        m_RingRegion.CenterY = m_CenterY;
                    }

                    m_RingRegion.Radius = 200;
                    m_RingRegion.AngleStart = 0;
                    m_RingRegion.AngleSpan = 2 * Math.PI;
                    m_RingRegion.RadialScale = 0.5;

                    m_RingRegion.GraphicDOFEnable = CogCircularAnnulusSectionDOFConstants.All;
                    m_RingRegion.Interactive = true;

                    Display.InteractiveGraphics.Add(m_RingRegion, "", true);


                }
            }

        }

        public void Reload_CenterRegion()
        {
            //2012�� 5��17�� ��ü������ ������  
            if (m_Region.GetType().ToString().IndexOf("Rectangle") >= 0)
            {
                try
                {
                    m_RectangleRegion = (CogRectangleAffine)m_Region;

                    m_CenterX = m_RectangleRegion.CenterX;
                    m_CenterY = m_RectangleRegion.CenterY;

                    
                }
                catch
                {
                    return;
                }
            }
            else if (m_Region.GetType().ToString().IndexOf("Circle") >= 0)
            {
                try
                {
                    m_CircleRegion = (CogCircle)m_Region;

                    m_CenterX = m_CircleRegion.CenterX;
                    m_CenterY = m_CircleRegion.CenterY;

                    
                }
                catch
                {
                    return;
                }


            }
            else if (m_Region.GetType().ToString().IndexOf("Polygon") >= 0)
            {
                
            }
            else
            {
                try
                {
                    m_RingRegion = (CogCircularAnnulusSection)m_Region;

                    m_CenterX = m_RingRegion.CenterX;
                    m_CenterY = m_RingRegion.CenterY;

                    
                }
                catch
                {
                    return;

                }
            }


        }


		#region ȭ�� ǥ�ÿ� Function
		public void DrawLabel(string text, Cognex.VisionPro.Display.CogDisplay Display, double sx, double sy, float size, Cognex.VisionPro.CogColorConstants ForeColor, Cognex.VisionPro.CogColorConstants BackColor, int Align)
		{
			CogGraphicLabel label = new CogGraphicLabel();
			label.Font = new System.Drawing.Font("����ü", size, System.Drawing.FontStyle.Bold);

			label.SelectedSpaceName = "#";

			switch(Align)
			{
				case 0:
					label.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
					break;
				case 1:
					label.Alignment = CogGraphicLabelAlignmentConstants.TopCenter;
					break;
				case 2:
					label.Alignment = CogGraphicLabelAlignmentConstants.TopRight;
					break;
				case 3:
					label.Alignment = CogGraphicLabelAlignmentConstants.BaselineLeft;
					break;
				case 4:
					label.Alignment = CogGraphicLabelAlignmentConstants.BaselineCenter;
					break;
				case 5:
					label.Alignment = CogGraphicLabelAlignmentConstants.BaselineRight;
					break;
				default:
					label.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;
					break;					
			}
			
			label.BackgroundColor = BackColor;
			label.Color = ForeColor;
			label.SetXYText(sx, sy, text);
			Display.StaticGraphics.Add(label, "");
		}

		public void DrawLabel(string text, Cognex.VisionPro.Display.CogDisplay Display, double sx, double sy, float size, Cognex.VisionPro.CogColorConstants ForeColor, Cognex.VisionPro.CogColorConstants BackColor)
		{
            CogGraphicLabel label = new CogGraphicLabel();
			
			label.SelectedSpaceName = "#";

            System.Drawing.Font f = new System.Drawing.Font("����", size);
			
			label.Alignment = CogGraphicLabelAlignmentConstants.TopLeft;

            label.Font = f;
			
			label.BackgroundColor = BackColor;
			label.Color = ForeColor;            
			label.SetXYText(sx, sy, text);
			Display.StaticGraphics.Add(label, "");

            f.Dispose();
            f = null;
            label = null;
		}

		public void DrawRectangle(double sx, double sy, double ex, double ey, Cognex.VisionPro.Display.CogDisplay Display, Cognex.VisionPro.CogColorConstants ForeColor, Cognex.VisionPro.CogColorConstants BackColor)
		{
			Cognex.VisionPro.CogRectangle rec = new CogRectangle();

			rec.X = sx;
			rec.Y = sy;
			rec.Width = ex - sx;
			rec.Height = ey - sy;
			rec.Color = ForeColor;

			Display.StaticGraphics.Add(rec, "");
		}

		public void DrawRectangle(string name, float size, double sx, double sy, double ex, double ey, Cognex.VisionPro.Display.CogDisplay Display, Cognex.VisionPro.CogColorConstants ForeColor, Cognex.VisionPro.CogColorConstants BackColor)
		{
			Cognex.VisionPro.CogRectangle rec = new CogRectangle();

			try
			{
				rec.SelectedSpaceName = "#";
				rec.X = sx;
				rec.Y = sy;
				rec.Width = ex - sx;
				rec.Height = ey - sy;			
				rec.Color = ForeColor;

				Display.StaticGraphics.Add(rec, "");

				DrawLabel(name, Display,sx, ey+1,  size, ForeColor, BackColor);
			}
			catch
			{
			}
		}

		public void DrawTowPointLine(double sx, double sy, double ex, double ey, int width, Cognex.VisionPro.Display.CogDisplay Display, Cognex.VisionPro.CogColorConstants ForeColor, Cognex.VisionPro.CogColorConstants BackColor)
		{
			Cognex.VisionPro.CogLineSegment line = new CogLineSegment();

			line.SelectedSpaceName = "#";
			line.StartX = sx;
			line.StartY = sy;
			line.EndX = ex;
			line.EndY = ey;
			line.Color = ForeColor;
			line.LineWidthInScreenPixels = width;

			Display.StaticGraphics.Add(line, "");
		}

		public void DrawCross(double x, double y, int size, int width, Cognex.VisionPro.Display.CogDisplay Display, Cognex.VisionPro.CogColorConstants ForeColor, Cognex.VisionPro.CogColorConstants BackColor)
		{
			Cognex.VisionPro.CogLineSegment line = new CogLineSegment();

			line.SelectedSpaceName = "#";
			line.StartX = x-size/2;
			line.StartY = y;
			line.EndX = x+size/2;
			line.EndY = y;
			line.Color = ForeColor;
			line.LineWidthInScreenPixels = width;

			Display.StaticGraphics.Add(line, "");

			line.StartX = x;
			line.StartY = y-size/2;
			line.EndX = x;
			line.EndY = y+size/2;
			line.Color = ForeColor;
			line.LineWidthInScreenPixels = width;

			Display.StaticGraphics.Add(line, "");

            line = null;
		}
        /// <summary>
        /// �˻翵�� ��������Ʈ�� ���̸� �˱� ���� ���� 2011.1.15
        /// </summary>
        public void Location_Rectangle()
        {
            if (m_RegionShape == RegionShape.Rectangle)
            {
                m_RectangleRegion = (CogRectangleAffine)m_Region;
                CenterX = m_RectangleRegion.CenterX;
                CenterY=m_RectangleRegion.CenterY;
                SideX=m_RectangleRegion.SideXLength;
                SideY=m_RectangleRegion.SideYLength;
                
            }
        }

		/// <summary>
		/// ȭ�鿡 ��ǥ�踦 �׸��� �Լ�
		/// </summary>
		/// <param name="Position">���� ��ġ</param>
		/// <param name="OriginX">���� ��ǥ ��</param>
		/// <param name="OriginY">���� ��ǥ ��</param>
		/// <param name="size">����</param>
		/// <param name="Xname">X ��ǥ�� �̸�</param>
		/// <param name="YName">Y ��ǥ�� �̸�</param>
		public void DrawCoordinate(string Position, int OriginX, int OriginY, int size, string Xname, string YName, int width, Cognex.VisionPro.Display.CogDisplay Display, Cognex.VisionPro.CogColorConstants ForeColor, Cognex.VisionPro.CogColorConstants BackColor)
		{
			Cognex.VisionPro.CogLineSegment line = new CogLineSegment();

			line.SelectedSpaceName = "#";
			
			if(Position=="LeftTop")
			{				
				// ���μ�
				line.StartX = 20;
				line.StartY = 20;
				line.EndX = 20+size;
				line.EndY = 20;
				line.Color = ForeColor;
				line.LineWidthInScreenPixels = width;
				Display.StaticGraphics.Add(line, "");

				line.StartX = 20+size-5;
				line.StartY = 20-3;
				line.EndX = 20+size;
				line.EndY = 20;
				line.Color = ForeColor;
				line.LineWidthInScreenPixels = width;
				Display.StaticGraphics.Add(line, "");

				line.StartX = 20+size-5;
				line.StartY = 20+3;
				line.EndX = 20+size;
				line.EndY = 20;
				line.Color = ForeColor;
				line.LineWidthInScreenPixels = width;
				Display.StaticGraphics.Add(line, "");

				DrawLabel(Xname, Display, 20+size-10, 5,  12, ForeColor, BackColor);

				// ���μ�
				line.StartX = 20;
				line.StartY = 20;
				line.EndX = 20;
				line.EndY = 20+size;
				line.Color = ForeColor;
				line.LineWidthInScreenPixels = width;
				Display.StaticGraphics.Add(line, "");

				line.StartX = 20-3;
				line.StartY = 20+size-5;
				line.EndX = 20;
				line.EndY = 20+size;
				line.Color = ForeColor;
				line.LineWidthInScreenPixels = width;
				Display.StaticGraphics.Add(line, "");

				line.StartX = 20+3;
				line.StartY = 20+size-5;
				line.EndX = 20;
				line.EndY = 20+size;
				line.Color = ForeColor;
				line.LineWidthInScreenPixels = width;
				Display.StaticGraphics.Add(line, "");

				DrawLabel(YName, Display, 5, 20+size-10,  12, ForeColor, BackColor);

				// ���� ��ǥ ����
				DrawLabel(String.Format("({0:0},{1:0})", OriginX, OriginY), Display, 3, 10,  8, ForeColor, BackColor);
			}
			else if(Position=="LeftBottom")
			{
				// ���μ�
				line.StartX = 20;
				line.StartY = 460;
				line.EndX = 20+size;
				line.EndY = 460;
				line.Color = ForeColor;
				line.LineWidthInScreenPixels = width;
				Display.StaticGraphics.Add(line, "");

				line.StartX = 20+size-5;
				line.StartY = 460-3;
				line.EndX = 20+size;
				line.EndY = 460;
				line.Color = ForeColor;
				line.LineWidthInScreenPixels = width;
				Display.StaticGraphics.Add(line, "");

				line.StartX = 20+size-5;
				line.StartY = 460+3;
				line.EndX = 20+size;
				line.EndY = 460;
				line.Color = ForeColor;
				line.LineWidthInScreenPixels = width;
				Display.StaticGraphics.Add(line, "");

				DrawLabel(Xname, Display, 28+size, 457,  12, ForeColor, BackColor);

				// ���μ�
				line.StartX = 20;
				line.StartY = 460;
				line.EndX = 20;
				line.EndY = 460-size;
				line.Color = ForeColor;
				line.LineWidthInScreenPixels = width;
				Display.StaticGraphics.Add(line, "");

				line.StartX = 20-3;
				line.StartY = 465-size;
				line.EndX = 20;
				line.EndY = 465-size-5;
				line.Color = ForeColor;
				line.LineWidthInScreenPixels = width;
				Display.StaticGraphics.Add(line, "");

				line.StartX = 20+3;
				line.StartY = 465-size;
				line.EndX = 20;
				line.EndY = 465-size-5;
				line.Color = ForeColor;
				line.LineWidthInScreenPixels = width;
				Display.StaticGraphics.Add(line, "");

				DrawLabel(YName, Display, 17, 447-size,  12, ForeColor, BackColor);

				// ���� ��ǥ ����
				DrawLabel(String.Format("({0:0},{1:0})", OriginX, OriginY), Display, 3, 465,  8, ForeColor, BackColor);
			}
			else if(Position=="RightBottom")
			{				
				// ���μ�
				line.StartX = 620;
				line.StartY = 460;
				line.EndX = 620-size;
				line.EndY = 460;
				line.Color = ForeColor;
				line.LineWidthInScreenPixels = width;
				Display.StaticGraphics.Add(line, "");

				line.StartX = 620-size+5;
				line.StartY = 460-3;
				line.EndX = 620-size;
				line.EndY = 460;
				line.Color = ForeColor;
				line.LineWidthInScreenPixels = width;
				Display.StaticGraphics.Add(line, "");

				line.StartX = 620-size+5;
				line.StartY = 460+3;
				line.EndX = 620-size;
				line.EndY = 460;
				line.Color = ForeColor;
				line.LineWidthInScreenPixels = width;
				Display.StaticGraphics.Add(line, "");

				DrawLabel(Xname, Display, 610-size, 455,  12, ForeColor, BackColor);

				// ���μ�
				line.StartX = 620;
				line.StartY = 460;
				line.EndX = 620;
				line.EndY = 460-size;
				line.Color = ForeColor;
				line.LineWidthInScreenPixels = width;
				Display.StaticGraphics.Add(line, "");

				line.StartX = 620+3;
				line.StartY = 465-size;
				line.EndX = 620;
				line.EndY = 465-size-5;
				line.Color = ForeColor;
				line.LineWidthInScreenPixels = width;
				Display.StaticGraphics.Add(line, "");

				line.StartX = 620-3;
				line.StartY = 465-size;
				line.EndX = 620;
				line.EndY = 465-size-5;
				line.Color = ForeColor;
				line.LineWidthInScreenPixels = width;
				Display.StaticGraphics.Add(line, "");

				DrawLabel(YName, Display, 615, 447-size,  12, ForeColor, BackColor);

				// ���� ��ǥ ����
				DrawLabel(String.Format("({0:0},{1:0})", OriginX, OriginY), Display, 600, 465,  8, ForeColor, BackColor);
			}
		}
		#endregion
	}
}