using System;
using System.Collections.Generic;
using System.Text;
using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;



namespace Hansero.VisionLib.VisionPro
{
    /// <summary>
    ///  이미지에 관련된 클래스 입니다.
    /// </summary>

    public class ImageFile : ToolBase
    {
        //이미지 들고 다니기
        protected ICogImage m_Image = null;

        public struct RangeHSI
        {
            public int Min_H;
            public int Min_S;
            public int Min_I;
            public int Max_H;
            public int Max_S;
            public int Max_I;
        }
        public struct ReturnRange
        {
            public int Min_H;
            public int Min_S;
            public int Min_I;
            public int Max_H;
            public int Max_S;
            public int Max_I;
            public int Avr_H;
            public int Avr_S;
            public int Avr_I;
        }

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
        private Cognex.VisionPro.CogRectangleAffine m_Area = new CogRectangleAffine();

        private double Weighted_R;
        
        public double Weight_R
        {
            set
            {
                Weighted_R = value;
            }
            get
            {
                return Weighted_R;
            }
        }
        
        private double Weighted_G;
        
        public double Weight_G
        {
            set
            {
                Weighted_G = value;
            }
            get
            {
                return Weighted_G;
            }
        }
        
        private double Weighted_B;
        
        public double Weight_B
        {
            set
            {
                Weighted_B = value;
            }
            get
            {
                return Weighted_B;
            }
        }

        /// <summary>
        /// 설정화면에서 이미지 로드하는 함수
        /// </summary>
        /// <param name="Path">로드할 이미지 경로</param>
        /// <returns>코그넥스 이미지를 반환함</returns>
        public ICogImage Load_Image(string Path)
        {

            CogImageFileTool m_ImageFileTool = new CogImageFileTool();

            try
            {

                m_ImageFileTool.Operator.Open(Path, CogImageFileModeConstants.Read);

                
                m_ImageFileTool.Run();

                m_Image = m_ImageFileTool.OutputImage;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
                m_ImageFileTool = null;
                
            }
            return m_Image;

        }
        /// <summary>
        /// 소벨에지 생성 함수
        /// </summary>
        /// <param name="Source">소벨에지이미지</param>
        /// <returns>변환된 이미지를 리턴합니다.</returns>
        public CogImage8Grey Get_SobelEdge(Cognex.VisionPro.Display.CogDisplay Display)
        {
            Cognex.VisionPro.CogImage8Grey Image;
            try
            {

                Cognex.VisionPro.ImageProcessing.CogSobelEdgeTool mySobel = new Cognex.VisionPro.ImageProcessing.CogSobelEdgeTool();

                mySobel.InputImage = (Cognex.VisionPro.CogImage8Grey)Display.Image;

                mySobel.Run();

                Image = mySobel.Result.FinalMagnitudeImage;

            }
            catch (Exception ex)
            {
                throw ex;
            }



            return Image;

        }
        /// <summary>
        /// 컬러이미지를 변환하는 함수
        /// </summary>
        /// <param name="Source">컬러이미지</param>
        /// <param name="Mode">변환 할 모드를 넣어줌."Hue","Saturation","Intensity","Red","Green","Blue"값을 넣으면 됨.디폴트는 Hue입니다.</param>
        /// <returns>변환된 이미지를 리턴합니다.</returns>
        public CogImage8Grey Get_Plan(CogImage24PlanarColor Source, string Mode)
        {

            Cognex.VisionPro.CogImage24PlanarColor myHSIImage = (Cognex.VisionPro.CogImage24PlanarColor)Cognex.VisionPro.CogImageConvert.GetHSIImage(Source, 0, 0, 0, 0);
            Cognex.VisionPro.CogImage24PlanarColor myRGBImage = (Cognex.VisionPro.CogImage24PlanarColor)Cognex.VisionPro.CogImageConvert.GetRGBImage(Source, 0, 0, 0, 0);
            
            Cognex.VisionPro.CogImage8Grey Image;

            switch (Mode)
            {
                case "Hue":
                    Image = myHSIImage.GetPlane(Cognex.VisionPro.CogImagePlaneConstants.Hue);
                    break;

                case "Saturation":
                    Image = myHSIImage.GetPlane(Cognex.VisionPro.CogImagePlaneConstants.Saturation);
                    break;

                case "Intensity":
                    Image = myHSIImage.GetPlane(Cognex.VisionPro.CogImagePlaneConstants.Intensity);
                    break;

                case "Red":
                    Image = myRGBImage.GetPlane(Cognex.VisionPro.CogImagePlaneConstants.Red);
                    break;

                case "Green":
                    Image = myRGBImage.GetPlane(Cognex.VisionPro.CogImagePlaneConstants.Green);
                    break;

                case "Blue":
                    Image = myRGBImage.GetPlane(Cognex.VisionPro.CogImagePlaneConstants.Blue);
                    break;

                case "WeightedRGB":
                    Image = (Cognex.VisionPro.CogImage8Grey)Cognex.VisionPro.CogImageConvert.GetIntensityImageFromWeightedRGB(Source, 0, 0, 0, 0, Weighted_R, Weighted_G, Weighted_B);
                    break;

                default:
                    Image = myHSIImage.GetPlane(Cognex.VisionPro.CogImagePlaneConstants.Intensity);
                    break;

             
            }

            return Image;
        }
        
        /// <summary>
        /// 화면 사이즈를 지정하여 캡쳐하는 함수
        /// </summary>
        /// <param name="ScreenWidth">캡쳐할 가로 사이즈</param>
        /// <param name="ScreenHeight">캡쳐할 세로 사이즈</param>
        /// <param name="ImageQuality">이미지 퀄리티0~100 지정.작을수록 퀄리티가 떨어짐</param>
        /// <param name="PathFile">이미지가 저장될 경로 및 파일명 지정</param>
        public void CaptureFullScreen(int ScreenWidth,int ScreenHeight,long ImageQuality,string PathFile)
        {

            Size uScreenSize = new Size(ScreenWidth, ScreenHeight); 

            Bitmap bitmap = new Bitmap(uScreenSize.Width, uScreenSize.Height); 
  
            Graphics g = Graphics.FromImage(bitmap); 
   
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), uScreenSize);

            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, ImageQuality);
            myEncoderParameters.Param[0] = myEncoderParameter;

            string[] str = PathFile.Split('\\');

            if (!System.IO.Directory.Exists(PathFile.Substring(0, PathFile.Length - str[str.Length - 1].Length)))
            {
                System.IO.Directory.CreateDirectory(PathFile.Substring(0, PathFile.Length - str[str.Length - 1].Length));
            }

            bitmap.Save(PathFile, jgpEncoder, myEncoderParameters);

            
        }
        /// <summary>
        /// 화면 사이즈를 지정하여 캡쳐하는 함수
        /// </summary>
        /// <param name="ScreenWidth">캡쳐할 가로 사이즈</param>
        /// <param name="ScreenHeight">캡쳐할 세로 사이즈</param>
        /// <param name="ImageQuality">이미지 퀄리티0~100 지정.작을수록 퀄리티가 떨어짐</param>
        /// <param name="PathFile">이미지가 저장될 경로 및 파일명 지정</param>
        /// <param name="CopyPathFile">캡쳐이미지 복사가능하도록....파일명 지정</param>
        public void CaptureFullScreen(int ScreenWidth, int ScreenHeight, long ImageQuality, string PathFile,string CopyPathFile)
        {

            Size uScreenSize = new Size(ScreenWidth, ScreenHeight);

            Bitmap bitmap = new Bitmap(uScreenSize.Width, uScreenSize.Height);

            Graphics g = Graphics.FromImage(bitmap);

            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), uScreenSize);

            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, ImageQuality);
            myEncoderParameters.Param[0] = myEncoderParameter;
            try
            {
                string[] str = PathFile.Split('\\');

                if (!System.IO.Directory.Exists(PathFile.Substring(0, PathFile.Length - str[str.Length - 1].Length)))
                {
                    System.IO.Directory.CreateDirectory(PathFile.Substring(0, PathFile.Length - str[str.Length - 1].Length));
                }

                bitmap.Save(PathFile, jgpEncoder, myEncoderParameters);

                File.Copy(PathFile, CopyPathFile);

            }
            catch
            {
                return;
            }

        }

        
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        /// <summary>
        /// BMP를 JPG로 변환합니다.
        /// </summary>
        /// <param name="SourcePathBMPFile">변환할 BMP파일 경로를 지정합니다.</param>
        /// <param name="ConvertPathJPGFile">BMP를 JPG로 변환후 저장할 경로를 지정합니다.</param>
        /// <param name="ImageQuality">JPG이미지 퀄리티를 지정합니다.1~100</param>
        public void BMPtoJPG(string SourcePathBMPFile,string ConvertPathJPGFile, long ImageQuality)
        {

            Bitmap bitmap = new Bitmap(SourcePathBMPFile);

            //Graphics g = Graphics.FromImage(bitmap);

            Size uScreenSize = new Size(bitmap.Width, bitmap.Height);

            //g.CopyFromScreen(new Point(0, 0), new Point(0, 0), uScreenSize);

            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, ImageQuality);
            myEncoderParameters.Param[0] = myEncoderParameter;

            bitmap.Save(ConvertPathJPGFile, jgpEncoder, myEncoderParameters);
        }
        /// <summary>
        /// 컬러 찾는 함수
        /// </summary>
        /// <param name="Source">컬러이미지</param>
        /// <param name="Range">HSI범위</param>
        /// <returns>OK스코어를 리턴함</returns>
        public double FindColor(CogImage24PlanarColor Source,RangeHSI Range)
        {
            Cognex.VisionPro.CogImage24PlanarColor myHSIImage = (Cognex.VisionPro.CogImage24PlanarColor)Cognex.VisionPro.CogImageConvert.GetHSIImage(Source, 0, 0, 0, 0);
            Cognex.VisionPro.CogImage24PlanarColor myRGBImage = (Cognex.VisionPro.CogImage24PlanarColor)Cognex.VisionPro.CogImageConvert.GetRGBImage(Source, 0, 0, 0, 0);
            //검사영역 로드
            m_Area=(Cognex.VisionPro.CogRectangleAffine) m_Region;

            double Total_H=0,Total_S=0,Total_I=0,Total_Pixel=0;
            double Color_Result = 0;
            byte Hue_Value;
            byte S_Value;
            byte I_Value;
            double OKCount = 0;
            try
            {
                for (int i = (int)(Math.Round(m_Area.CenterX - (m_Area.SideXLength / 2))); i < Math.Round(m_Area.CenterX + (m_Area.SideXLength / 2)); i++)
                    for (int j = (int)(Math.Round(m_Area.CenterY - (m_Area.SideYLength / 2))); j < Math.Round(m_Area.CenterY + (m_Area.SideYLength / 2)); j++)
                    {
                        myHSIImage.GetPixel(Math.Abs(i), Math.Abs(j), out Hue_Value, out S_Value, out I_Value);

                        //Total_H+=(double)Hue_Value;
                        //Total_S+=(double)S_Value;
                        //Total_I+=(double)I_Value;

                        if ((Hue_Value >= Range.Min_H) && (S_Value >= Range.Min_S) && (I_Value >= Range.Min_I) && (Hue_Value <= Range.Max_H) && (S_Value <= Range.Max_S) && (I_Value <= Range.Max_I))
                        {
                            OKCount++;
                        }

                        Total_Pixel++;
                    }
            }
            catch
            {
                Color_Result = 0;
                return Color_Result;
            }

            //OK점수 표시하기
            Color_Result = (OKCount / Total_Pixel) * 100;


            return Color_Result;
        }
        /// <summary>
        /// HSI범위 찾는 함수
        /// </summary>
        /// <param name="Source">컬러 이미지</param>
        /// <returns>RangeHSI 최대 최소값 리턴</returns>
        public ReturnRange Find_Range(CogImage24PlanarColor Source)
        {
            Cognex.VisionPro.CogImage24PlanarColor myHSIImage = (Cognex.VisionPro.CogImage24PlanarColor)Cognex.VisionPro.CogImageConvert.GetHSIImage(Source, 0, 0, 0, 0);
            Cognex.VisionPro.CogImage24PlanarColor myRGBImage = (Cognex.VisionPro.CogImage24PlanarColor)Cognex.VisionPro.CogImageConvert.GetRGBImage(Source, 0, 0, 0, 0);
            //검사영역 로드
            m_Area = (Cognex.VisionPro.CogRectangleAffine)m_Region;

            ReturnRange Color_Result;
            int Min_H = 255, Min_S = 255, Min_I = 255;
            double Total_H = 0, Total_S = 0, Total_I = 0, Total_Pixel = 0;
            int Max_H = 0, Max_S = 0, Max_I = 0;
            byte Hue_Value;
            byte S_Value;
            byte I_Value;
            double OKCount = 0;

            for (int i = (int)(Math.Round(m_Area.CenterX - (m_Area.SideXLength / 2))); i < Math.Round(m_Area.CenterX + (m_Area.SideXLength / 2)); i++)
                for (int j = (int)(Math.Round(m_Area.CenterY - (m_Area.SideYLength / 2))); j < Math.Round(m_Area.CenterY + (m_Area.SideYLength / 2)); j++)
                {
                    myHSIImage.GetPixel(Math.Abs(i), Math.Abs(j), out Hue_Value, out S_Value, out I_Value);

                    
                    if (Min_H > (int)Hue_Value)
                        Min_H = (int)Hue_Value;
                    if (Min_S > (int)S_Value)
                        Min_S = (int)S_Value;
                    if (Min_I > (int)I_Value)
                        Min_I = (int)I_Value;
                    if (Max_H < (int)Hue_Value)
                        Max_H = (int)Hue_Value;
                    if (Max_S < (int)S_Value)
                        Max_S = (int)S_Value;
                    if (Max_I < (int)I_Value)
                        Max_I = (int)I_Value;

                    Total_H += (double)Hue_Value;
                    Total_S += (double)S_Value;
                    Total_I += (double)I_Value;

                        
                    

                    Total_Pixel++;
                }

            Color_Result.Max_H = Max_H;
            Color_Result.Max_I = Max_I;
            Color_Result.Max_S = Max_S;
            Color_Result.Min_H = Min_H;
            Color_Result.Min_I = Min_I;
            Color_Result.Min_S = Min_S;
            Color_Result.Avr_H = (int)(Total_H / Total_Pixel);
            Color_Result.Avr_I = (int)(Total_I / Total_Pixel);
            Color_Result.Avr_S = (int)(Total_S / Total_Pixel);

            return Color_Result;
        }





    }

          
   
}
