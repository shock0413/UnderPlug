using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Hansero.VisionLib.VisionPro;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro;
using Utilities;
using System.IO.Ports;
using Cameras;
using System.Threading;

namespace Inspection
{
    public partial class CamSetting : Form
    {
        IniFile m_Config = new IniFile(Application.StartupPath + "\\Config.ini");
        // 조명 컨트롤러
        Hansero.HSRLEDControl m_LedControler = new Hansero.HSRLEDControl();

        PylonCameraManager cameraManager;

        ILightInterface jtech_LED = new JTECH_LED();
        public CamSetting(PylonCameraManager cameraManager)
        {
            InitializeComponent();

            this.cameraManager = cameraManager;
        }



        private void CamSetting_Load(object sender, EventArgs e)
        {
            panel_CamInfo.Enabled = false;

            // 기종 정보 로드
            cb_kind.Items.Clear();
            foreach (KeyValuePair<string, string> model in m_Config.GetSectionValuesAsList("MODEL"))
            {
                cb_kind.Items.Add(model.Value);
            }

            cb_kind.Text = "RH";

            // 방향 정보 로드
            cb_direction.Items.Clear();
            foreach (KeyValuePair<string, string> model in m_Config.GetSectionValuesAsList("Direction"))
            {
                cb_direction.Items.Add(model.Value);
            }

            // 촬영파라미터 로드
            num_Bright.Minimum = m_Config.GetInt32("Camera", cb_direction.Text + "MinBrightness", 0);
            num_Bright.Maximum = m_Config.GetInt32("Camera", cb_direction.Text + "MaxBrightness", 0);
            num_Bright.Increment = m_Config.GetInt32("Camera", cb_direction.Text + "IncremetBrightness", 1);
            num_Bright.Value = (decimal)m_Config.GetDouble("Camera", cb_direction.Text + "Brightness", 0);
            num_Expose.Minimum = m_Config.GetInt32("Camera", cb_direction.Text + "MinExpose", 0);
            num_Expose.Maximum = m_Config.GetInt32("Camera", cb_direction.Text + "MaxExpose", 0);
            num_Expose.Increment = (decimal)m_Config.GetDouble("Camera", cb_direction.Text + "IncrementExpose", 1);
            num_Expose.Value = (decimal)m_Config.GetDouble("Camera", cb_direction.Text + "Expose", 0);
            
        }

        private void btn_Modify_Click(object sender, EventArgs e)
        {
            if (btn_Modify.Text == "수정")
            {
                panel_CamInfo.Enabled = true;
                btn_Modify.Text = "저장";
            }
            else if (btn_Modify.Text == "저장")
            {
                m_Config.WriteValue("Camera", cb_direction.Text+"Brightness", num_Bright.Value.ToString());
                // m_Config.WriteValue("Camera", cb_direction.Text + "MinBrightness", num_Bright.Minimum.ToString());
                // m_Config.WriteValue("Camera", cb_direction.Text + "MaxBrightness", num_Bright.Maximum.ToString());
                m_Config.WriteValue("Camera", cb_direction.Text + "Expose", (double)num_Expose.Value);
                // m_Config.WriteValue("Camera", cb_direction.Text + "MinExpose", num_Expose.Minimum.ToString());
                // m_Config.WriteValue("Camera", cb_direction.Text + "MaxExpose", num_Expose.Maximum.ToString());

                panel_CamInfo.Enabled = false;
                btn_Modify.Text = "수정";

            }

        }

        private void btn_Calibration_Click(object sender, EventArgs e)
        {
            try
            {
                // 켈리브레이션 툴
                CogCalibCheckerboardTool cali = new CogCalibCheckerboardTool();


                // 켈리브레이션 툴 이미지 로드
                if (cogDisplay1.Image == null)
                {
                    MessageBox.Show("No exist Image.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                cali.InputImage = cogDisplay1.Image.GetType().Name == "CogImage8Grey" ? cogDisplay1.Image : new ImageFile().Get_Plan((CogImage24PlanarColor)cogDisplay1.Image, "Intensity");

                // 보정 모드
                cali.Calibration.ComputationMode = rb_Linear.Checked ? CogCalibFixComputationModeConstants.Linear : CogCalibFixComputationModeConstants.PerspectiveAndRadialWarp;


                // 기준점
                cali.Calibration.FiducialMark = chk_FiducialMark.Checked ? CogCalibCheckerboardFiducialConstants.StandardRectangles : CogCalibCheckerboardFiducialConstants.None;

                // 타일 사이즈
                try
                {
                    cali.Calibration.PhysicalTileSizeX = Int32.Parse(tb_TileSizeX.Text);
                    cali.Calibration.PhysicalTileSizeY = Int32.Parse(tb_TileSizeY.Text);
                }
                catch
                {
                    MessageBox.Show("Input Tile Size.");
                    return;
                }

                // 보정 이미지 그랩
                cali.Calibration.CalibrationImage = cali.InputImage;

                // 보정 원점
                cali.Calibration.CalibratedOriginSpace =
                    rb_Calibrated.Checked ?
                    CogCalibCheckerboardAdjustmentSpaceConstants.RawCalibrated :
                    CogCalibCheckerboardAdjustmentSpaceConstants.Uncalibrated;

                try
                {
                    cali.Calibration.CalibratedOriginX = Int32.Parse(tb_OriginX.Text);
                    cali.Calibration.CalibratedOriginY = Int32.Parse(tb_OriginY.Text);
                }
                catch
                {
                    MessageBox.Show("Input Calibration Origin");
                    return;
                }

                cali.Calibration.CalibratedXAxisRotation = double.Parse(tb_Rotation.Text) * (Math.PI / 180);
                cali.Calibration.SwapCalibratedHandedness = chk_SwapHandness.Checked;

                // 보정 계산
                cali.Calibration.Calibrate();
                // 캘리브레이션 툴 실행

                cali.Run();

                // 결과 저장
                string tmp_Path = Application.StartupPath + "\\Tools\\";
                if (!System.IO.Directory.Exists(tmp_Path))
                    System.IO.Directory.CreateDirectory(tmp_Path);

                Cognex.VisionPro.CogSerializer.SaveObjectToFile(cali, Application.StartupPath + "\\Tools\\" + cb_kind.Text + cb_direction.Text + "Calib.vpp");

                MessageBox.Show("CalibCheckerboard Setting OK");
            }
            catch 
            {
                MessageBox.Show("CalibCheckerboard Setting Error");
                return;
            }
            
        }

        private void btn_OneShot_Click(object sender, EventArgs e)
        {
            //m_Board.Load_Image(Application.StartupPath + "\\Temp\\Cali.bmp", 0);
            try
            {
                jtech_LED.LightON("COM1", 1,10);
            }
                
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                // 영상획득 파라미터
                int brightness = Convert.ToInt32(num_Bright.Value);
                int expose = Convert.ToInt32(num_Expose.Value);
                string cameraSerial = m_Config.GetString("Camera", "Serial", "");

                PylonCameraManager manager = new PylonCameraManager();
                manager.OneShot(cameraSerial, brightness, expose);

                cogDisplay1.Image = new CogImage24PlanarColor(manager.LastCaptureImage);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Thread.Sleep(300);

            try
            {
                jtech_LED.LightOFF("COM1", 1);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void btn_SaveMaster_Click(object sender, EventArgs e)
        {
            // 화면이 동영상 모드일때 영상 정지
            if (cogDisplay1.LiveDisplayRunning)
                cogDisplay1.StopLiveDisplay();

            // 캘리브레이션 실행
            ICogImage calib_image = Run_Calibration(cogDisplay1.Image);
            // 마스터 위치 찾기
            //double[] masterValue = Find_Location(calib_image);

            double[] masterValue = ((Main_Form)this.Owner).Find_Location(calib_image, cb_direction.Text);

            // 마스터 찾기 실패시
            if (masterValue[2] == 180)
            {
                MessageBox.Show("Not fount Master Point", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 마스터 이미지 저장
            Bitmap master = calib_image.ToBitmap();
            if (!System.IO.Directory.Exists(Application.StartupPath + "\\Master")) System.IO.Directory.CreateDirectory(Application.StartupPath + "\\Master");
            master.Save(Application.StartupPath + "\\Master\\Master.mas");

            // 마스터 데이터 저장
            m_Config.WriteValue("Master", cb_kind.Text + cb_direction.Text + "X", masterValue[0]);
            m_Config.WriteValue("Master", cb_kind.Text + cb_direction.Text + "Y", masterValue[1]);
            m_Config.WriteValue("Master", cb_kind.Text + cb_direction.Text + "Angle", masterValue[2]);

        }

        private ICogImage Run_Calibration(ICogImage inputImage)
        {

            try
            {
                // 이미지 변환
                ICogImage monoimage = inputImage.GetType().Name == "CogImage8Grey" ? inputImage : new ImageFile().Get_Plan((CogImage24PlanarColor)inputImage, "Intensity");

                // 캘리브레이션 툴 로드
                CogCalibCheckerboardTool calib = new CogCalibCheckerboardTool();

                string tool_path = Application.StartupPath + "\\Tools\\" + "Calib.vpp";
                if (!System.IO.File.Exists(tool_path))
                {
                    MessageBox.Show("None Calibration Data : " + cb_kind.Text + cb_direction.Text + "Calib.vpp");
                    return null;
                }

                calib = (Cognex.VisionPro.CalibFix.CogCalibCheckerboardTool)Cognex.VisionPro.CogSerializer.LoadObjectFromFile(tool_path);
                calib.InputImage = monoimage;

                calib.Run();

                return calib.OutputImage;
            }
            catch {
                return null;
            }
        }

        private void btn_SaveImage_Click(object sender, EventArgs e)
        {
            if (cogDisplay1.Image == null)
            {
                MessageBox.Show("No Image.");
                return;
            }

            try
            {
                SaveFileDialog savePath = new SaveFileDialog();
                savePath.DefaultExt = "bmp";

                //tmp.Save(savePath.FileName);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Fail : " + ex.Message);
            }
        }

        private bool isStopShot = false;

        private void btn_ContinuousShot_Click(object sender, EventArgs e)
        {
            try
            {
                jtech_LED.LightON("COM1", 1,99);


                // 영상획득 파라미터
                double brightness = m_Config.GetDouble("Camera", cb_direction.Text + "Brightness", 0);
                double contrast = m_Config.GetDouble("Camera", cb_direction.Text + "Contrast", 0);
                double expose = m_Config.GetDouble("Camera", cb_direction.Text + "Expose", 79976);

                string camSerical = m_Config.GetString("Camera", "Serial", "22033290");
                isStopShot = false;
                new Thread(new ThreadStart(() =>
                {
                    while(!isStopShot)
                    {
                        this.Invoke(new Action(() =>
                        {
                            try
                            {
                                cogDisplay1.Image = new CogImage24PlanarColor(cameraManager.OneShot(camSerical, (int)brightness, (int)expose));

                            }
                            catch
                            {

                            }


                            CogLine verticalLine = new CogLine();
                            verticalLine.SetFromStartXYEndXY(cogDisplay1.Image.Width / 2, 0, cogDisplay1.Image.Width / 2, cogDisplay1.Image.Height);

                            CogLine horizentalLine = new CogLine();
                            horizentalLine.SetFromStartXYEndXY(0, cogDisplay1.Image.Height / 2, cogDisplay1.Image.Width, cogDisplay1.Image.Height / 2);

                            cogDisplay1.InteractiveGraphics.Clear();
                            cogDisplay1.InteractiveGraphics.Add(verticalLine, "", false);
                            cogDisplay1.InteractiveGraphics.Add(horizentalLine, "", false);
                        }));

                        Thread.Sleep(300);
                    }
                })).Start();
                 
            }
            catch { }
        }

        private void btn_ContinuousShotStop_Click(object sender, EventArgs e)
        {
            isStopShot = true;
        }

        private void btn_Cancle_Click(object sender, EventArgs e)
        {
            try
            {
                num_Expose.Value = (decimal)m_Config.GetInt32("Camera", cb_direction.Text + "Expose", 0);
                num_Expose.Minimum = (decimal)m_Config.GetInt32("Camera", cb_direction.Text + "MinExpose", 0);
                num_Expose.Maximum = (decimal)m_Config.GetInt32("Camera", cb_direction.Text + "MaxExpose", 0);
                num_Bright.Value = (decimal)m_Config.GetInt32("Camera", cb_direction.Text + "Brightness", 0);
                num_Bright.Minimum = (decimal)m_Config.GetInt32("Camera", cb_direction.Text + "MinBrightness", 0);
                num_Bright.Maximum = (decimal)m_Config.GetInt32("Camera", cb_direction.Text + "MaxBrightness", 0);
            }
            catch
            {
            }

            panel_CamInfo.Enabled = false;
            btn_Modify.Text = "수정";
        }

        private void btn_CalibImageView_Click(object sender, EventArgs e)
        {
            try
            {
                // 캘리브레이션 툴 로드
                CogCalibCheckerboardTool calib = new CogCalibCheckerboardTool();

                string tool_path = Application.StartupPath + "\\Tools\\" + cb_kind.Text + cb_direction.Text + "Calib.vpp";
                if (!System.IO.File.Exists(tool_path))
                {
                    //Write_SystemLog("캘리브레이션데이터가 없습니다. : " + lb_Location.Text + lb_Direction.Text + "Calib.vpp");
                    return;
                }

                calib = (Cognex.VisionPro.CalibFix.CogCalibCheckerboardTool)Cognex.VisionPro.CogSerializer.LoadObjectFromFile(tool_path);


                cogDisplay1.Image = calib.Calibration.CalibrationImage;

                //ToolBase tb = new ToolBase();
                //for (int i = 0; i < calib.Calibration.NumPoints; i++)
                //{
                //    tb.DrawCross(calib.Calibration.GetUncalibratedPointX(i), calib.Calibration.GetUncalibratedPointY(i), 10, 3, cogDisplay1, CogColorConstants.Magenta, CogColorConstants.Black);
                //    tb.DrawCross(calib.Calibration.GetRawCalibratedPointX(i), calib.Calibration.GetRawCalibratedPointY(i), 5, 2, cogDisplay1, CogColorConstants.Cyan, CogColorConstants.Black);

                //}

            }
            catch (Exception ex)
            {
                
                
            }
        }

        private void btn_LightOn_Click(object sender, EventArgs e)
        {
            jtech_LED.LightON("COM1", 1, 10);
        }

        private void btn_LightOff_Click(object sender, EventArgs e)
        {
            jtech_LED.LightOFF("COM1", 1);
        }

        private void CamSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            jtech_LED.LightOFF("COM1", 1);
        }

        private void cb_direction_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 촬영파라미터 로드
            try
            {
                num_Bright.Minimum = (decimal)m_Config.GetDouble("Camera", cb_direction.Text + "MinBrightness", 0);
                num_Bright.Maximum = (decimal)m_Config.GetDouble("Camera", cb_direction.Text + "MaxBrightness", 10);
                num_Bright.Increment = (decimal)m_Config.GetDouble("Camera", cb_direction.Text + "IncrementBrightness", 0);
                num_Bright.Value = (decimal)m_Config.GetDouble("Camera", cb_direction.Text + "Brightness", 0);
            }
            catch
            {

            }

            try
            {
                num_Expose.Minimum = (decimal)m_Config.GetDouble("Camera", cb_direction.Text + "MinExpose", 0);
                num_Expose.Maximum = (decimal)m_Config.GetDouble("Camera", cb_direction.Text + "MaxExpose", 10);
                num_Expose.Increment = (decimal)m_Config.GetDouble("Camera", cb_direction.Text + "IncrementExpose", 0);
                num_Expose.Value = (decimal)m_Config.GetDouble("Camera", cb_direction.Text + "Expose", 0);
            }
            catch
            {
                
            }
        }


        public interface ILightInterface
        {
            void LightON(string port, int channel, int brightness);
            void LightOFF(string port, int channel);

            // 15.11.07 전체조명 ON OFF 테스트 필요. (미완성)
            void LightOnAll(string[] ports);
            void LightOffAll(string[] ports);
        }

        public class HSR_LED : ILightInterface
        {
            Hansero.HSRLEDControl m_LedControler = new Hansero.HSRLEDControl();

            public void LightON(string port, int channel, int brightness)
            {
                m_LedControler.InitControler(port);

                m_LedControler.LED_Brightness(channel, brightness);

                m_LedControler.LED_ONOFF(channel, Hansero.HSRLEDControl.LEDSTATE.ON);

                m_LedControler.ReleaseControler();
            }

            public void LightOFF(string port, int channel)
            {
                m_LedControler.InitControler(port);

                m_LedControler.LED_ONOFF(channel, Hansero.HSRLEDControl.LEDSTATE.OFF);

                m_LedControler.ReleaseControler();
            }

            public void LightOnAll(string[] ports)
            {
                //모든 시리얼포트를 가져올 경우 멈춤 현상 발생
                string[] ctrls = System.IO.Ports.SerialPort.GetPortNames();

                foreach (string port in ports)
                {
                    m_LedControler.InitControler(port);

                    for (int i = 1; i <= 4; i++)
                    {
                        //m_LedControler.LED_Brightness(i, 99);
                        m_LedControler.LED_ONOFF(i, Hansero.HSRLEDControl.LEDSTATE.ON);
                    }

                    m_LedControler.ReleaseControler();
                }
            }

            public void LightOffAll(string[] ports)
            {
                //string[] ctrls = System.IO.Ports.SerialPort.GetPortNames();

                foreach (string port in ports)
                {
                    m_LedControler.InitControler(port);

                    for (int i = 1; i <= 4; i++)
                        m_LedControler.LED_ONOFF(i, Hansero.HSRLEDControl.LEDSTATE.OFF);

                    m_LedControler.ReleaseControler();
                }
            }
        }

        public class JTECH_LED : ILightInterface
        {
            SerialPort serialPort1 = new SerialPort();

            const char CTR = 'L';
            const char OFF = 'E';
            const char CR = (char)0x0D;
            const char CH1 = '1';
            const char CH2 = '2';
            const char CH3 = '3';
            const char CH4 = '4';

            private bool InitControler(string port)
            {
                serialPort1.PortName = port;
                serialPort1.BaudRate = 19200;

                try
                {
                    serialPort1.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            private bool ReleaseControler()
            {
                try
                {
                    serialPort1.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            private void LED_Brightness(int channel, int brightness)
            {
                string tmpBrightness = string.Format("{0:000}", brightness);
                char[] sendMessage = new char[6];

                sendMessage[0] = CTR;
                sendMessage[1] = (char)(channel + '0');
                sendMessage[2] = tmpBrightness.ToCharArray()[0];
                sendMessage[3] = tmpBrightness.ToCharArray()[1];
                sendMessage[4] = tmpBrightness.ToCharArray()[2];
                sendMessage[5] = CR;

                if (serialPort1.IsOpen)
                {
                    serialPort1.Write(sendMessage, 0, 6);
                    //serialPort1.Write(sendMessage);
                }

                //Thread.Sleep(100);

                serialPort1.Close();
            }

            private void LED_Brightness_OFF(int channel)
            {
                char[] sendMessage = new char[3];

                sendMessage[0] = OFF;
                sendMessage[1] = (char)(channel + '0');
                sendMessage[2] = CR;

                //string sendMessage = string.Format("E{0:%c}", m_channel);
                //Console.WriteLine(sendMessage);

                if (serialPort1.IsOpen)
                {
                    serialPort1.Write(sendMessage, 0, 3);
                }

                //Thread.Sleep(100);
            }

            public void LightON(string port, int channel, int brightness)
            {
                InitControler(port);

                LED_Brightness(channel, brightness);

                ReleaseControler();
            }

            public void LightOFF(string port, int channel)
            {
                InitControler(port);

                LED_Brightness_OFF(channel);

                ReleaseControler();
            }

            public void LightOnAll(string[] ports)
            {
                //모든 시리얼포트를 가져올 경우 멈춤 현상 발생
                string[] ctrls = System.IO.Ports.SerialPort.GetPortNames();

                foreach (string port in ports)
                {
                    for (int i = 1; i <= 4; i++)
                    {
                    }
                }
            }

            public void LightOffAll(string[] ports)
            {
                string[] ctrls = System.IO.Ports.SerialPort.GetPortNames();

                foreach (string port in ports)
                {

                    for (int i = 1; i <= 4; i++)
                    {
                    }
                }
            }
        }
        //private void LightON()
        //{

        //    //string[] ctrls = itemINI.GetString(part, "LED_Port", "COM1").Split(',');

        //    string port = m_Config.GetString("LED", "LED_PORT", "COM1");
        //    int ch = m_Config.GetInt32("LED", "LED_ch", 1);
        //    int br = m_Config.GetInt32("LED", "LED_BR", 99);

        //    m_LedControler.InitControler(port);

        //    m_LedControler.LED_Brightness(ch, br);

        //    m_LedControler.LED_ONOFF(Int32.Parse(ch.ToString()), Hansero.HSRLEDControl.LEDSTATE.ON);

        //    m_LedControler.ReleaseControler();

        //}

        //private void LightOff()
        //{
        //    string port = m_Config.GetString("LED", "LED_PORT", "COM1");
        //    int ch = m_Config.GetInt32("LED", "LED_ch", 1);

        //    m_LedControler.InitControler(port);

        //    m_LedControler.LED_ONOFF(Int32.Parse(ch.ToString()), Hansero.HSRLEDControl.LEDSTATE.OFF);

        //    m_LedControler.ReleaseControler();
        //}

        private void cb_kind_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Setting : Load Image Button");

            cogDisplay1.InteractiveGraphics.Clear();
            cogDisplay1.StaticGraphics.Clear();

            OpenFileDialog loadImage = new OpenFileDialog();
            string loadedImgPath;

            loadImage.InitialDirectory = m_Config.GetString("Result", "SavePath", @"D:\Result") + "\\Image";
            loadImage.Filter = "";
            //loadImage.Filter += "|*.bmp|*.bmp|*.jpg|*.jpg";

            if (loadImage.ShowDialog() == DialogResult.OK)
                loadedImgPath = loadImage.FileName;
            else return;

            try
            {
                cogDisplay1.Image = (new ImageFile()).Load_Image(loadedImgPath);
                cogDisplay1.AutoFit = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void num_Cont_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
