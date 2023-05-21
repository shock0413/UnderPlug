using PylonC.NETSupportLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraPylon
{
    public class CameraCore
    {
        public ImageProvider ImageProvider { get { return imageProvider; } }
        ImageProvider imageProvider = new ImageProvider();

        public CameraCore()
        {

        }
    }
}
