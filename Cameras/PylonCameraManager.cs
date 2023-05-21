 
using PylonC.NET;
using PylonC.NETSupportLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cameras
{
    public class PylonCameraManager
    {
        private ImageProvider m_imageProvider = new ImageProvider(); /* Create one image provider. */

        List<DeviceEnumerator.Device> list;
        private Bitmap m_bitmap = null;

        public Bitmap LastCaptureImage { get { return m_bitmap; } }

        bool isOpen = false;
        bool isShot = false;
         
 
        DeviceEnumerator.Device device;

        public PylonCameraManager()
        {
            Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "1000");

            Pylon.Initialize();

            list = DeviceEnumerator.EnumerateDevices();
            m_imageProvider.ImageReadyEvent += new ImageProvider.ImageReadyEventHandler(OnImageReadyEventCallback);
            m_imageProvider.DeviceOpenedEvent += new ImageProvider.DeviceOpenedEventHandler(OnDeviceOpenedEventCallback);
            m_imageProvider.GrabbingStartedEvent += new ImageProvider.GrabbingStartedEventHandler(M_imageProvider_GrabbingStartedEvent);
            m_imageProvider.GrabbingStoppedEvent += new ImageProvider.GrabbingStoppedEventHandler(M_imageProvider_GrabbingStoppedEvent);
            m_imageProvider.GrabErrorEvent += new ImageProvider.GrabErrorEventHandler(M_imageProvider_GrabErrorEvent);
        }

        ~PylonCameraManager()
        {
            Pylon.Terminate();
        }

        private void M_imageProvider_GrabErrorEvent(Exception grabException, string additionalErrorMessage)
        {

        }

        private void M_imageProvider_GrabbingStoppedEvent()
        {

        }

        private void M_imageProvider_GrabbingStartedEvent()
        {

        }

        private void SetParams(int bright, int Expose)
        {
            try
            {
                NODE_HANDLE hNode = m_imageProvider.GetNodeFromDevice("GainRaw");
                int min = (int)GenApi.IntegerGetMin(hNode);
                int max = (int)GenApi.IntegerGetMax(hNode);
                int inc = (int)GenApi.IntegerGetInc(hNode);

                GenApi.IntegerSetValue(hNode, bright);
            }
            catch
            {

            }

            try
            {
                NODE_HANDLE hNode = m_imageProvider.GetNodeFromDevice("ExposureTimeRaw");
                GenApi.IntegerSetValue(hNode, Expose);
            }
            catch
            {

            }
        }

        public Bitmap OneShot(string cameraSerial, int bright, int expose)
        {
            try
            {
                // Open(cameraSerial);

                SetParams(bright, expose);

                m_bitmap = null;

                isShot = false;

                for (int i = 0; i < 4 && !isShot; i++)
                {
                    ImageProvider.Image img = m_imageProvider.OneShot();
                    if (img != null)
                    {
                        try
                        {
                            /* Check if the image is compatible with the currently used bitmap. */
                            if (BitmapFactory.IsCompatible(m_bitmap, img.Width, img.Height, img.Color))
                            {
                                /* Update the bitmap with the image data. */
                                BitmapFactory.UpdateBitmap(m_bitmap, img.Buffer, img.Width, img.Height, img.Color);
                                /* To show the new image, request the display control to update itself. */

                            }
                            else /* A new bitmap is required. */
                            {
                                BitmapFactory.CreateBitmap(out m_bitmap, img.Width, img.Height, img.Color);
                                BitmapFactory.UpdateBitmap(m_bitmap, img.Buffer, img.Width, img.Height, img.Color);

                                /* We have to dispose the bitmap after assigning the new one to the display control. */
                            }

                            /* The processing of the image is done. Release the image buffer. */
                            try
                            {
                                m_imageProvider.ReleaseImage();
                            }
                            catch
                            {

                            }
                            /* The buffer can be used for the next image grabs. */

                            isShot = true;
                        }
                        catch
                        {
                            throw new Exception("Failed to grap pylonViewer");
                        }
                    }

                    if (isShot == false)
                    {
                        try
                        {
                            Close();
                        }
                        catch
                        {
                            throw new Exception("Failed to Close Camera");
                        }

                        try
                        {
                            Open(cameraSerial);
                        }
                        catch
                        {
                            throw new Exception("Failed to Open Camera");
                        }
                    }
                }

                // Close();

                if (isShot)
                {
                    return m_bitmap;
                }
                else
                {
                    throw new Exception("image receive timeout");

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("카메라 취득 실패 원인 : " + e.Message);

                throw e;

                return null;
            }

        }

        public void Open(string camSerial)
        {
            isOpen = false;

            DeviceEnumerator.Device device = null;

            list.ForEach(x =>
            {
                if (x.SerialNum == camSerial)
                {
                    device = x;
                }
            });

            try
            {
                for (int i = 0; i < 300 && !isOpen; i++)
                {
                    if (device != null)
                    {
                        m_imageProvider.Open(device.Index);
                    }
                    
                    Thread.Sleep(20);
                }

                if (isOpen)
                {
                    this.device = device;
                }
            }
            catch (Exception e)
            {

            }
        }
         
        private void OnDeviceOpenedEventCallback()
        {
            isOpen = true;
        }

        private void OnImageReadyEventCallback()
        {
            try
            {
                /* Acquire the image from the image provider. Only show the latest image. The camera may acquire images faster than images can be displayed*/
                ImageProvider.Image image = m_imageProvider.GetLatestImage();

                /* Check if the image has been removed in the meantime. */
                if (image != null)
                {
                    /* Check if the image is compatible with the currently used bitmap. */
                    if (BitmapFactory.IsCompatible(m_bitmap, image.Width, image.Height, image.Color))
                    {
                        /* Update the bitmap with the image data. */
                        BitmapFactory.UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);
                        /* To show the new image, request the display control to update itself. */

                    }
                    else /* A new bitmap is required. */
                    {
                        BitmapFactory.CreateBitmap(out m_bitmap, image.Width, image.Height, image.Color);
                        BitmapFactory.UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);

                        /* We have to dispose the bitmap after assigning the new one to the display control. */
                    }
                 
                    /* The processing of the image is done. Release the image buffer. */
                    try
                    {
                        m_imageProvider.ReleaseImage();
                    }
                    catch
                    {

                    }
                    /* The buffer can be used for the next image grabs. */

                    isShot = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Close()
        {
            try
            {
                m_imageProvider.Close();
            }
            catch
            {

            }
        }
    }
}