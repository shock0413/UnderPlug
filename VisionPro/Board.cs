using System;
using System.IO;
using Cognex.VisionPro;
using Cognex.VisionPro.Exceptions;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.FG1394DCAM;

namespace Hansero.VisionLib.VisionPro
{
    
    /// <summary>
    /// Board Ŭ����
    /// ���� ȹ�� �� ������ ���� Ŭ���� �Դϴ�.
    /// </summary>
	public class Board
	{
       
		// ��ġ�� FrameGrabber ����Ʈ�� ������ ����
		protected CogFrameGrabbers m_FrameGrabbers;
        protected CogFrameGrabber1394DCAMs m_1394Grabbers;
        
		// ĸ��� �̹��� ����.
		protected ICogImage m_Image = null;

        int trignum;
        int numTriger = 0;

		protected ICogAcqFifo[] m_AcqFifo = null;


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
				m_Image = value;
			}
		}
		
		/// <summary>
		/// ������.
		/// ����Ʈ ���� ���� "Sony XC-ST50 640x480 IntDrv CCF" ���		
		// Jai CV-A1 1364x1035 IntDrv (rapid-reset, shutter-sw-pulseWidth) CCF
		// Jai CV-A1-14.4 1344x1049 IntDrv (rapid-reset, shutter-sw-pulseWidth) CCF
		// Pulnix TM-6CN 760x574 ExtPLLCo CCF
		// Pulnix TM-7EX 320x240 IntDrv CCF
		// Pulnix TM-7EX 640x240 IntDrv CCF
		// Pulnix TM-7EX 640x480 IntDrv CCF
		// Pulnix TM-9701 640x480 IntDrv (analog-out, rapid-reset, shutter-sw-doublePulse) CCF
		// Sony XC-55 640x480 IntDrv (rapid-reset, shutter-sw-EDONPISHAII) CCF
		// Sony XC-56 640x480 IntDrv (rapid-reset, shutter-sw-EDONPISHAII) CCF
		// Sony XC-75 320x240 ExtPLLCo CCF
		// Sony XC-75 320x240 IntDrv (rapid-reset, shutter-sw-SDONPISHA) CCF
		// Sony XC-75 320x240 IntDrv CCF
		// Sony XC-75 640x240 ExtPLLCo CCF
		// Sony XC-75 640x240 IntDrv CCF
		// Sony XC-75 640x480 ExtPLLCo CCF
		// Sony XC-75 640x480 IntDrv CCF
		// Sony XC-75CE 380x287 ExtPLLCo CCF
		// Sony XC-75CE 380x287 IntDrv (rapid-reset, shutter-sw-SDONPISHA) CCF
		// Sony XC-75CE 380x287 IntDrv CCF
		// Sony XC-75CE 760x287 ExtPLLCo CCF
		// Sony XC-75CE 760x287 IntDrv CCF
		// Sony XC-75CE 760x574 ExtPLLCo CCF
		// Sony XC-75CE 760x574 IntDrv CCF
		// Sony XC-HR50 640x480 IntDrv (rapid-reset, shutter-sw-EDONPISHAII) CCF
		// Sony XC-HR70 1020x768 IntDrv (rapid-reset, shutter-sw-EDONPISHAII) CCF
		// Sony XC-ST50 320x240 ExtPLLCo CCF
		// Sony XC-ST50 320x240 IntDrv (rapid-reset, shutter-sw-SDONPISHA) CCF
		// Sony XC-ST50 320x240 IntDrv CCF
		// Sony XC-ST50 640x240 ExtPLLCo CCF
		// Sony XC-ST50 640x240 IntDrv CCF
		// Sony XC-ST50 640x480 ExtPLLCo CCF
		// Sony XC-ST50 640x480 IntDrv CCF
		// Sony XC-ST50CE 380x287 ExtPLLCo CCF
		// Sony XC-ST50CE 380x287 IntDrv (rapid-reset, shutter-sw-SDONPISHA) CCF
		// Sony XC-ST50CE 380x287 IntDrv CCF
		// Sony XC-ST50CE 760x287 ExtPLLCo CCF
		// Sony XC-ST50CE 760x287 IntDrv CCF
		// Sony XC-ST50CE 760x574 ExtPLLCo CCF
		// Sony XC-ST50CE 760x574 IntDrv CCF
		// Teli CS8531 640x480 IntDrv (shutter-sw-random-trigger, single-tap) CCF
		// Teli CS8541D 640x480 IntDrv (rapid-reset, shutter-sw-random-trigger) CCF
		// Toshiba IK-53V 640x134 IntDrv (rapid-reset, shutter-sw-pulseWidth) CCF
		// Toshiba IK-53V 640x480 IntDrv (rapid-reset, shutter-sw-pulseWidth) CCF		
		/// </summary>
        /// 

        //20���� ����ø��� �ѹ� ������ �ִ� ���� Ȯ��
        private string[] m_BoardSerialNumber = new string[20];
        private int m_BoardCount;

        public string[] BoardSerialNumber
        {
            get
            {

                return m_BoardSerialNumber;
            }
            set
            {

                m_BoardSerialNumber = value;
            }
        }
        public int BoardCount
        {
            get
            {

                return m_BoardCount ;
            }
            set
            {

                m_BoardCount = value;
            }
        }
        
        
        
        public Board()
        {
            //
            // TODO: ���⿡ ������ ���� �߰��մϴ�.
            //

        }

		/// <summary>
		/// ���� �ʱ�ȭ
		/// </summary>
		/// <param name="VidioFormat">������� ���� ���� �ִ� ����</param>
		public void InitializeAcquisition(string VidioFormat)
		{
			// Step 1 - ��ġ�� FrameGrabber ����.
			m_FrameGrabbers = new CogFrameGrabbers();
            
			if (m_FrameGrabbers.Count < 1)
				throw new CogAcqNoFrameGrabberException("���带 ã�� �� �����ϴ�.");

			m_AcqFifo = new ICogAcqFifo[m_FrameGrabbers.Count];

            m_BoardCount = m_FrameGrabbers.Count;

            for (int i = 0, index = 0; i < m_FrameGrabbers.Count; i++)
            {
                if (m_FrameGrabbers[i].ToString().IndexOf("1394") >= 0)
                {
                    m_AcqFifo[index] = m_FrameGrabbers[i].CreateAcqFifo(m_FrameGrabbers[i].AvailableVideoFormats[0], Cognex.VisionPro.CogAcqFifoPixelFormatConstants.Format3Plane, 0, false);
                    m_AcqFifo[index].OwnedExposureParams.Exposure = 100;

                    for (int j = 0; j < m_FrameGrabbers[i].AvailableVideoFormats.Count; j++)
                    {
                        if (m_FrameGrabbers[i].AvailableVideoFormats[j].IndexOf("raw8") >= 0)
                        {
                            m_AcqFifo[index] = m_FrameGrabbers[i].CreateAcqFifo(m_FrameGrabbers[i].AvailableVideoFormats[j], Cognex.VisionPro.CogAcqFifoPixelFormatConstants.Format3Plane, 0, false);
                            m_AcqFifo[index++].OwnedExposureParams.Exposure = 8;

                            m_BoardSerialNumber[i] = m_FrameGrabbers[i].SerialNumber;
                            
                            break;
                        }
                    }            
                }
                else if (m_FrameGrabbers[i].ToString().IndexOf("GigE") >= 0)
                {
                    try
                    {
                        m_AcqFifo[index] = m_FrameGrabbers[i].CreateAcqFifo(m_FrameGrabbers[i].AvailableVideoFormats[0], Cognex.VisionPro.CogAcqFifoPixelFormatConstants.Format3Plane, 0, false);

                        for (int j = 0; j < m_FrameGrabbers[i].AvailableVideoFormats.Count; j++)
                        {
                            if (m_FrameGrabbers[i].AvailableVideoFormats[j].ToUpper().IndexOf("COLOR") >= 0 || m_FrameGrabbers[i].AvailableVideoFormats[j].ToUpper().IndexOf("YUV") >= 0)
                            {
                                m_AcqFifo[index] = m_FrameGrabbers[i].CreateAcqFifo(m_FrameGrabbers[i].AvailableVideoFormats[j], Cognex.VisionPro.CogAcqFifoPixelFormatConstants.Format3Plane, 0, false);
                                m_AcqFifo[index++].OwnedExposureParams.Exposure = 10;
                                break;
                            }
                        }

                        m_BoardSerialNumber[i] = m_FrameGrabbers[i].SerialNumber;
                    }
                    catch
                    {
                        //return;
                        continue;
                    }
                    finally
                    {
                    }
                }
                else
                {
                    // ĸ�� ��Ʈ�� ����
                    // ���� ����, �̹��� ���� ����, ī�޶� ��Ʈ ����, �ڵ� ?? ����.
                    if (VidioFormat == "")
                        continue;

                    m_AcqFifo[index] = m_FrameGrabbers[i].CreateAcqFifo(VidioFormat, CogAcqFifoPixelFormatConstants.Format8Grey, 0, true);
                    //m_AcqFifo[i] = m_FrameGrabbers[i].CreateAcqFifo(m_FrameGrabbers[i].AvailableVideoFormats[0], CogAcqFifoPixelFormatConstants.Format8Grey, Port, true);
                    m_AcqFifo[index].OwnedExposureParams.Exposure = 100;
                    m_AcqFifo[index++].Timeout = 200;

                    m_BoardSerialNumber[i] = m_FrameGrabbers[i].SerialNumber;
                }
            }
		}

        public void Dispose()
        {
            //for (int i = 0; i < m_FrameGrabbers.Count; i++)
            //{
            //    m_AcqFifo[i] = null; 
                
            //}

            //m_AcqFifo = null; 
            //m_FrameGrabbers= null;

            try
            {
                for (int i = 0; i < m_FrameGrabbers.Count; i++)
                {
                    //m_AcqFifo[i] = null;
                    m_FrameGrabbers[i].Disconnect(true);
                    m_AcqFifo[i].FrameGrabber.Disconnect(true);
                }
            }
            catch
            {

            }

            m_AcqFifo = null;
            m_FrameGrabbers = null;

           
        }

        public void Dispose_Image()
        {   

        }
		/// <summary>
		/// �����󺸴� �Լ�
		/// </summary>
		/// <param name="BoardNum">����ѹ�</param>
		/// <param name="Channel">��Ʈ�ѹ�0~3������</param>
		/// <param name="Brightness">���</param>
		/// <param name="Contrast">��Ʈ��Ʈ</param>
		/// <param name="Expose">����</param>
		/// <param name="Display">�ڱ׵��÷��� ȭ��</param>
		public void LiveDisplay(int BoardNum, int Channel, double Brightness, double Contrast,double Expose, Cognex.VisionPro.Display.CogDisplay Display)
		{
			
			if(BoardNum < m_FrameGrabbers.Count)
			{
				try
				{
					// ī�޶� ��Ʈ ����			
					m_AcqFifo[BoardNum].CameraPort = Channel;

					// ���, ��� ����
					if(Brightness>1) Brightness = 1;
					if(Brightness<0) Brightness = 0;
					if(Contrast>1) Contrast = 1;
					if(Contrast<0) Contrast = 0;

                    m_AcqFifo[BoardNum].OwnedExposureParams.Exposure = Expose;
					m_AcqFifo[BoardNum].OwnedBrightnessParams.Brightness = Brightness;
					m_AcqFifo[BoardNum].OwnedContrastParams.Contrast = Contrast;

					Display.StartLiveDisplay(m_AcqFifo[BoardNum],true);
				}
				catch
				{

				}
			}
			else
			{
				Display.StopLiveDisplay();
			}
		}
        /// <summary>
        /// ������ ���ߴ� �Լ�
        /// </summary>
        /// <param name="Display">������ ���� �ڱ׵��÷���</param>
        public void StopDisplay(Cognex.VisionPro.Display.CogDisplay Display)
        {
            Display.StopLiveDisplay();
        }
        [STAThread]
        public void LiveDisplay(string SerialNum, double Brightness, double Contrast, bool bStart, Cognex.VisionPro.Display.CogDisplay Display)
        {
            if (bStart == true)
            {
                for (int i = 0; i < m_FrameGrabbers.Count; i++)
                {
                    if (m_FrameGrabbers[i].SerialNumber.ToUpper().Replace(" ", "") == SerialNum.ToUpper().Replace(" ", ""))
                    {
                        try
                        {
                            // ī�޶� ��Ʈ ����			
                            m_AcqFifo[i].CameraPort = 0;

                            // ���, ��� ����
                            if (Brightness > 1) Brightness = 1;
                            if (Brightness < 0) Brightness = 0;
                            if (Contrast > 1) Contrast = 1;
                            if (Contrast < 0) Contrast = 0;

                            m_AcqFifo[i].OwnedBrightnessParams.Brightness = Brightness;
                            m_AcqFifo[i].OwnedContrastParams.Contrast = Contrast;

                            Display.StartLiveDisplay(m_AcqFifo[i], true);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            else
            {
                Display.StopLiveDisplay();
            }
        }

		/// <summary>
		/// ������ ����, ä���� �̹��� ĸ��.
		/// �Խ� ���� ���� �Խ� ä�� ���� �� ��.
		/// ä�� ���� �� ������ �ʼ�.
		/// ���� �Լ��� ���ο� �̹��� �Կ��� ���� Fifo ���� �ٽ� ������.
		/// Fifo ���� ����Ʈ�� ���� ������ �ִ� ��� ���� �ʿ�(2005.09.05)
		/// </summary>
		/// <param name="BoardNum">���� ��ȣ, 0, 1, 2....</param>
		/// <param name="Channel">ĸ���� ä��, 0, 1, 2 3, 4</param>
		/// <param name="SavePath">�ӽ� ���� ������ ��ġ, ���ϸ�</param>		
		/// <param name="Brightness">��� �� ����, 0~1</param>
		/// <param name="Contrast">��� �� ����, 0~1</param>
        public void NewImageCapture(int BoardNum, int Channel, double Brightness, double Contrast, int Rotate)
		{
			if(BoardNum < m_FrameGrabbers.Count)
			{
				try
				{				
					// ī�޶� ��Ʈ ����			
                    if (Channel < 0) Channel = 0;
                    if (Channel > 3) Channel = 3;
					m_AcqFifo[BoardNum].CameraPort = Channel;

					// ���, ��� ����
					if(Brightness>1) Brightness = 1; 
					if(Brightness<0) Brightness = 0;
					if(Contrast>1) Contrast = 1;
					if(Contrast<0) Contrast = 0;

					m_AcqFifo[BoardNum].OwnedBrightnessParams.Brightness = Brightness;
					m_AcqFifo[BoardNum].OwnedContrastParams.Contrast = Contrast;

					System.Threading.Thread.Sleep(100);

					// �̹��� ĸ��
					m_Image =  m_AcqFifo[BoardNum].Acquire(out trignum);

                    if (trignum > 4)
                    {
                        GC.Collect();
                        trignum = 0;
                    }
				}
                catch (Cognex.VisionPro.Exceptions.CogAcqTimingException e)
                {
                    throw new Cognex.VisionPro.Exceptions.CogAcqTimingException(e.Message, e);
                }
				catch(Exception e)
				{
					throw new  Exception(e.Message);
				}
				finally
				{
				}
			}
		}

        public void NewImageCapture(string SerialNum, double Brightness, double Contrast, int Rotate)
        {
            for (int i = 0; i < m_FrameGrabbers.Count; i++)
            {
                if (m_FrameGrabbers[i].SerialNumber == SerialNum)
                {
                    try
                    {
                        m_AcqFifo[i].CameraPort = 0;

                        // ���, ��� ����
                        if (Brightness > 1) Brightness = 1;
                        if (Brightness < 0) Brightness = 0;
                        if (Contrast > 1) Contrast = 1;
                        if (Contrast < 0) Contrast = 0;

                        m_AcqFifo[i].OwnedBrightnessParams.Brightness = Brightness;
                        m_AcqFifo[i].OwnedContrastParams.Contrast = Contrast;

                        //System.Threading.Thread.Sleep(100);

                        // �̹��� ĸ��
                        m_Image = m_AcqFifo[i].Acquire(out trignum);
                        numTriger++;

                        if (numTriger > 9)
                        {
                            GC.Collect();
                            numTriger = 0;
                        }
                    }

                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                    }

                    break;
                }
            }
        }
        /// <summary>
        /// �Ⱑ�̴��� �÷�ī�޶� ���� ȹ��� ����ϴ� �Լ�
        /// </summary>
        /// <param name="BoardNum">�����ȣ</param>
        /// <param name="Brightness">���</param>
        /// <param name="Contrast">��Ʈ��Ʈ</param>
        /// <param name="Expose">����</param>
        /// <returns>���������� ����ȹ��� true�� ���� ��...</returns>
        public bool NewImageCapture(int BoardNum, double Brightness, double Contrast, double Expose)
        {
            try
            {
                m_AcqFifo[BoardNum].CameraPort = 0;

                // ���, ��� ����
                if (Brightness > 1) Brightness = 1;
                if (Brightness < 0) Brightness = 0;
                if (Contrast > 1) Contrast = 1;
                if (Contrast < 0) Contrast = 0;

                m_AcqFifo[BoardNum].OwnedBrightnessParams.Brightness = Brightness;
                m_AcqFifo[BoardNum].OwnedContrastParams.Contrast = Contrast;
                m_AcqFifo[BoardNum].OwnedExposureParams.Exposure = Expose;

                //System.Threading.Thread.Sleep(100);

                // �̹��� ĸ��
                m_Image = m_AcqFifo[BoardNum].Acquire(out trignum);
                numTriger++;

                if (trignum > 4)
                {
                    GC.Collect();
                    trignum = 0;
                }

                return true;

            }

            catch (Exception e)
            {
                //throw new Exception(e.Message);
                return false;

            }
            finally
            {

            }
        }

		public bool NewImageCapture(int BoardNum, int Channel, double Brightness, double Contrast, int Expose, int Rotate)
		{
			if(BoardNum < m_FrameGrabbers.Count)
			{
				try
				{
					int trignum;
				
					// ī�޶� ��Ʈ ����			
					m_AcqFifo[BoardNum].CameraPort = Channel;

					// ���, ��� ����
					if(Brightness>1) Brightness = 1;
					if(Brightness<0) Brightness = 0;
					if(Contrast>1) Contrast = 1;
					if(Contrast<0) Contrast = 0;

					m_AcqFifo[BoardNum].OwnedBrightnessParams.Brightness = Brightness;
					m_AcqFifo[BoardNum].OwnedContrastParams.Contrast = Contrast;
					m_AcqFifo[BoardNum].OwnedExposureParams.Exposure = Expose;

					//System.Threading.Thread.Sleep(100);

					// �̹��� ĸ��
					m_Image =  m_AcqFifo[BoardNum].Acquire(out trignum);

                    if (trignum > 4)
                    {
                        GC.Collect();
                        trignum = 0;
                        
                    }
                    
                    
				}
				
				catch(Exception e)
				{
                    
					//throw new  Exception(e.Message);
                    return false;
				}
				finally
				{
				}

			}

            return true;

		}

		#region �̹��� �ҷ�����, ����, ��ȯ �� ���� �Լ�
		/// <summary>
		/// �̹��� �ҷ�����
		/// </summary>
		/// <param name="Path"></param>
		public void Load_Image(string Path, int Rotate)
		{
			CogImageFileTool m_ImageFileTool = new CogImageFileTool();

			try
			{
				// �ӽ� �̹��� �����ϱ� ���� ���� Open
				m_ImageFileTool.Operator.Open(Path , CogImageFileModeConstants.Read);

				m_ImageFileTool.Run();			

				m_Image = m_ImageFileTool.OutputImage;

			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				m_ImageFileTool = null;
			}
		}

		/// <summary>
		/// ���� �̹��� ����
		/// </summary>
		/// <param name="Name">ī�޶� �̸�</param>
		public void Save_Image(string Name)
		{
			CogImageFileTool m_ImageFileTool = new CogImageFileTool();

			try
			{
                //if (!System.IO.Directory.Exists(System.Windows.Forms.Application.StartupPath + "\\Temp"))
                //{
                //    System.IO.Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + "\\Temp");
                //}
                
                //Name = System.Windows.Forms.Application.StartupPath + "\\Temp\\" + Name;
				
                // �ӽ� �̹��� �����ϱ� ���� ���� Open
				if(File.Exists(Name)==true)
				{
					File.Delete(Name);
				}
					
				m_ImageFileTool.Operator.Open(Name, CogImageFileModeConstants.Write);

				m_ImageFileTool.InputImage = m_Image;

				m_ImageFileTool.Run();

			}
            catch (Exception ex)
			{
                throw ex;

				throw new Exception("�ӽ� ���� ���� Open Error!!");
			}
			finally
			{
				m_ImageFileTool = null;
			}
		}
		#endregion

	}
}