using PylonC.NET;
using PylonC.NETSupportLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CameraPylon
{
    public class Camera
    {
        ImageProvider imageProvider = new ImageProvider();
        private DeviceEnumerator.Device device;

        public Bitmap Bitmap{ get { return bitmap; } }
        private Bitmap bitmap= null;

        ImageProvider.Image lastGetImage;

        private bool shotFinish = false;

        public string SerialNum { get { return device.SerialNum; } }

        public int GainMin { get { return GetGainMin(); } }
        public int GainMax { get { return GetGainMax(); } }
        public int GainInterval { get { return GetGainInterval(); } }

        public int ExposureMin { get { return GetExposureMin(); } }
        public int ExposureMax { get { return GetExposureMax(); } }
        public int ExposureInterval { get { return GetExposureInterval(); } }

        public Camera(DeviceEnumerator.Device device, ImageProvider imageProvider)
        {
            this.device = device;
            InitImageProviderEvent();
        }

        private void InitImageProviderEvent()
        {
            imageProvider.ImageReadyEvent += new ImageProvider.ImageReadyEventHandler(OnImageReadyEventCallback);
            imageProvider.GrabErrorEvent += new ImageProvider.GrabErrorEventHandler(OnGrabErrorEventCallback);
            imageProvider.DeviceOpenedEvent += new ImageProvider.DeviceOpenedEventHandler(OnDeviceOpenedEventCallback);
        }

        private void OnImageReadyEventCallback()
        {
            try
            {
                lastGetImage = imageProvider.GetLatestImage();
                shotFinish = true;
            }
            catch (Exception e)
            {
                
            }
        }

        private void OnGrabErrorEventCallback(Exception grabException, string additionalErrorMessage)
        {

        }


        private void OnDeviceOpenedEventCallback()
        {

        }

        private void Open()
        {
            imageProvider.Open(device.Index);
        }

        private void Close()
        {
            imageProvider.Close();
        }

        private void SetGain(int bright)
        {
            NODE_HANDLE hNode = imageProvider.GetNodeFromDevice("GainRaw");
            int min = (int)GenApi.IntegerGetMin(hNode);
            int max = (int)GenApi.IntegerGetMax(hNode);

            if (min > bright)
            {
                bright = min;
            }

            if (max < bright)
            {
                bright = max;
            }

            GenApi.IntegerSetValue(hNode, bright);
        }

        private int GetGainMin()
        {
            Open();
            NODE_HANDLE hNode = imageProvider.GetNodeFromDevice("GainRaw");
            int min = (int)GenApi.IntegerGetMin(hNode);
            Close();
            return min;
        }


        private int GetGainMax()
        {
            Open();
            NODE_HANDLE hNode = imageProvider.GetNodeFromDevice("GainRaw");

            int max = (int)GenApi.IntegerGetMax(hNode);
            Close();
            return max;
        }

        private int GetGainInterval()
        {
            Open();
            NODE_HANDLE hNode = imageProvider.GetNodeFromDevice("GainRaw");

            int inc = (int)GenApi.IntegerGetInc(hNode);
            Close();

            return inc;
        }


        private void SetExposure(int exposure)
        {
            NODE_HANDLE hNode = imageProvider.GetNodeFromDevice("ExposureTimeRaw");
            GenApi.IntegerSetValue(hNode, exposure);
        }

        private int GetExposureMin()
        {
            Open();
            NODE_HANDLE hNode = imageProvider.GetNodeFromDevice("ExposureTimeRaw");
            int min = (int)GenApi.IntegerGetMin(hNode);
            Close();
            return min;
        }


        private int GetExposureMax()
        {
            Open();
            NODE_HANDLE hNode = imageProvider.GetNodeFromDevice("ExposureTimeRaw");

            int max = (int)GenApi.IntegerGetMax(hNode);
            Close();
            return max;
        }

        private int GetExposureInterval()
        {
            Open();
            NODE_HANDLE hNode = imageProvider.GetNodeFromDevice("ExposureTimeRaw");

            int inc = (int)GenApi.IntegerGetInc(hNode);
            Close();

            return inc;
        }


        public Bitmap OneShot(int bright, int exposure)
        {
            try
            {
                shotFinish = false;
                Open();

                SetGain(bright);
                SetExposure(exposure);

                if (Bitmap != null)
                {
                    bitmap.Dispose();
                    bitmap = null;
                }

                imageProvider.OneShot();

                for (int i = 0; i < 1000 && !shotFinish; i++)
                {
                    Thread.Sleep(5);
                }

                Close();

                Bitmap temp;

                BitmapFactory.CreateBitmap(out temp, lastGetImage.Width, lastGetImage.Height, lastGetImage.Color);
                BitmapFactory.UpdateBitmap(temp, lastGetImage.Buffer, lastGetImage.Width, lastGetImage.Height, lastGetImage.Color);

                bitmap = temp;

                if (shotFinish)
                {
                    return temp;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {

                Close();
            }

            return null;
        }


        public BitmapSource ToWpfBitmap(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Bmp);

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }

        public void ContiniusShot()
        {
            imageProvider.ContinuousShot();
        }
    }
}
