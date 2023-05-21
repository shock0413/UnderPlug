using System;
using System.IO;
using Cognex.VisionPro;
using Cognex.VisionPro.Exceptions;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.FG1394DCAM;

namespace Hansero.VisionLib.VisionPro
{
    
    /// <summary>
    /// Board 클래스
    /// 영상 획득 및 저장을 위한 클래스 입니다.
    /// </summary>
	public class Board
	{
       
		// 설치된 FrameGrabber 리스트를 가지는 변수
		protected CogFrameGrabbers m_FrameGrabbers;
        protected CogFrameGrabber1394DCAMs m_1394Grabbers;
        
		// 캡춰된 이미지 저장.
		protected ICogImage m_Image = null;

        int trignum;
        int numTriger = 0;

		protected ICogAcqFifo[] m_AcqFifo = null;


		/// <summary>
		/// 캡춰한 이미지.
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
		/// 생성자.
		/// 디폴트 비디오 포맷 "Sony XC-ST50 640x480 IntDrv CCF" 사용		
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

        //20개의 보드시리얼 넘버 넣을수 있는 공간 확보
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
            // TODO: 여기에 생성자 논리를 추가합니다.
            //

        }

		/// <summary>
		/// 보드 초기화
		/// </summary>
		/// <param name="VidioFormat">보드사용시 비디오 포맷 넣는 인자</param>
		public void InitializeAcquisition(string VidioFormat)
		{
			// Step 1 - 설치된 FrameGrabber 생성.
			m_FrameGrabbers = new CogFrameGrabbers();
            
			if (m_FrameGrabbers.Count < 1)
				throw new CogAcqNoFrameGrabberException("보드를 찾을 수 없습니다.");

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
                    // 캡춰 컨트롤 생성
                    // 비디오 포맷, 이미지 종류 선택, 카메라 포트 설정, 자동 ?? 설정.
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
		/// 동영상보는 함수
		/// </summary>
		/// <param name="BoardNum">보드넘버</param>
		/// <param name="Channel">포트넘버0~3번까지</param>
		/// <param name="Brightness">밝기</param>
		/// <param name="Contrast">콘트라스트</param>
		/// <param name="Expose">노출</param>
		/// <param name="Display">코그디스플레이 화면</param>
		public void LiveDisplay(int BoardNum, int Channel, double Brightness, double Contrast,double Expose, Cognex.VisionPro.Display.CogDisplay Display)
		{
			
			if(BoardNum < m_FrameGrabbers.Count)
			{
				try
				{
					// 카메라 포트 설정			
					m_AcqFifo[BoardNum].CameraPort = Channel;

					// 밝기, 명암 조절
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
        /// 동영상 멈추는 함수
        /// </summary>
        /// <param name="Display">동영상 멈출 코그디스플레이</param>
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
                            // 카메라 포트 설정			
                            m_AcqFifo[i].CameraPort = 0;

                            // 밝기, 명암 조절
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
		/// 지정된 보드, 채널의 이미지 캡춰.
		/// 먹스 사용시 먼저 먹스 채널 세팅 할 것.
		/// 채널 세팅 후 딜레이 필수.
		/// 현재 함수는 새로운 이미지 촬영시 마다 Fifo 툴을 다시 생성함.
		/// Fifo 툴을 리스트로 만들어서 가질수 있는 방법 연구 필요(2005.09.05)
		/// </summary>
		/// <param name="BoardNum">보드 번호, 0, 1, 2....</param>
		/// <param name="Channel">캡춰할 채널, 0, 1, 2 3, 4</param>
		/// <param name="SavePath">임시 파일 저장할 위치, 파일명</param>		
		/// <param name="Brightness">밝기 값 설정, 0~1</param>
		/// <param name="Contrast">명암 값 설정, 0~1</param>
        public void NewImageCapture(int BoardNum, int Channel, double Brightness, double Contrast, int Rotate)
		{
			if(BoardNum < m_FrameGrabbers.Count)
			{
				try
				{				
					// 카메라 포트 설정			
                    if (Channel < 0) Channel = 0;
                    if (Channel > 3) Channel = 3;
					m_AcqFifo[BoardNum].CameraPort = Channel;

					// 밝기, 명암 조절
					if(Brightness>1) Brightness = 1; 
					if(Brightness<0) Brightness = 0;
					if(Contrast>1) Contrast = 1;
					if(Contrast<0) Contrast = 0;

					m_AcqFifo[BoardNum].OwnedBrightnessParams.Brightness = Brightness;
					m_AcqFifo[BoardNum].OwnedContrastParams.Contrast = Contrast;

					System.Threading.Thread.Sleep(100);

					// 이미지 캡춰
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

                        // 밝기, 명암 조절
                        if (Brightness > 1) Brightness = 1;
                        if (Brightness < 0) Brightness = 0;
                        if (Contrast > 1) Contrast = 1;
                        if (Contrast < 0) Contrast = 0;

                        m_AcqFifo[i].OwnedBrightnessParams.Brightness = Brightness;
                        m_AcqFifo[i].OwnedContrastParams.Contrast = Contrast;

                        //System.Threading.Thread.Sleep(100);

                        // 이미지 캡춰
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
        /// 기가이더넷 컬러카메라 영상 획득시 사용하는 함수
        /// </summary>
        /// <param name="BoardNum">보드번호</param>
        /// <param name="Brightness">밝기</param>
        /// <param name="Contrast">콘트라스트</param>
        /// <param name="Expose">노출</param>
        /// <returns>정상적으로 영상획득시 true값 던져 줌...</returns>
        public bool NewImageCapture(int BoardNum, double Brightness, double Contrast, double Expose)
        {
            try
            {
                m_AcqFifo[BoardNum].CameraPort = 0;

                // 밝기, 명암 조절
                if (Brightness > 1) Brightness = 1;
                if (Brightness < 0) Brightness = 0;
                if (Contrast > 1) Contrast = 1;
                if (Contrast < 0) Contrast = 0;

                m_AcqFifo[BoardNum].OwnedBrightnessParams.Brightness = Brightness;
                m_AcqFifo[BoardNum].OwnedContrastParams.Contrast = Contrast;
                m_AcqFifo[BoardNum].OwnedExposureParams.Exposure = Expose;

                //System.Threading.Thread.Sleep(100);

                // 이미지 캡춰
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
				
					// 카메라 포트 설정			
					m_AcqFifo[BoardNum].CameraPort = Channel;

					// 밝기, 명암 조절
					if(Brightness>1) Brightness = 1;
					if(Brightness<0) Brightness = 0;
					if(Contrast>1) Contrast = 1;
					if(Contrast<0) Contrast = 0;

					m_AcqFifo[BoardNum].OwnedBrightnessParams.Brightness = Brightness;
					m_AcqFifo[BoardNum].OwnedContrastParams.Contrast = Contrast;
					m_AcqFifo[BoardNum].OwnedExposureParams.Exposure = Expose;

					//System.Threading.Thread.Sleep(100);

					// 이미지 캡춰
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

		#region 이미지 불러오기, 저장, 변환 등 관련 함수
		/// <summary>
		/// 이미지 불러오기
		/// </summary>
		/// <param name="Path"></param>
		public void Load_Image(string Path, int Rotate)
		{
			CogImageFileTool m_ImageFileTool = new CogImageFileTool();

			try
			{
				// 임시 이미지 저장하기 위한 파일 Open
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
		/// 현재 이미지 저장
		/// </summary>
		/// <param name="Name">카메라 이름</param>
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
				
                // 임시 이미지 저장하기 위한 파일 Open
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

				throw new Exception("임시 저장 파일 Open Error!!");
			}
			finally
			{
				m_ImageFileTool = null;
			}
		}
		#endregion

	}
}