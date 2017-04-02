using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nikon;
using System.Drawing;
using System.Windows;

namespace SMoOnion
{
    class Camera
    {
        private string _device = "Type0003.md3";
        private string _path;
        
        private Session _session;
        public Session Session
        {
            get { return _session; }
            set { _session = value; }
        }

        public NikonDevice cam;
        public NikonManager manager;

        private MainWindow _mainWindow;

        public Camera()
        {
            _mainWindow = (MainWindow)Application.Current.MainWindow;

            _path = AppDomain.CurrentDomain.BaseDirectory + "/pictures/";

            System.IO.Directory.CreateDirectory(_path + "/temp");

            manager = new NikonManager(_device);

            manager.DeviceAdded += new DeviceAddedDelegate(onDeviceAdded);
        }

        void onDeviceAdded(NikonManager sender, NikonDevice device)
        {
            cam = device;
            device.ImageReady += Device_ImageReady;
            device.CaptureComplete += Device_CaptureComplete;

            cam.LiveViewEnabled = false;
        }

        private void Device_CaptureComplete(NikonDevice sender, int data)
        {
            _mainWindow.SetLastFrameOnOnion();
        }

        private void Device_ImageReady(NikonDevice sender, NikonImage image)
        {
            byte[] _img = image.Buffer;

            string pathToWrite = _path + _session.Name;
            System.IO.Directory.CreateDirectory(pathToWrite);

            System.IO.File.WriteAllBytes(_path + _session.Name + "/" + _session.PictureCount + ".jpeg", _img);
            System.IO.File.WriteAllBytes(_path + "/temp/lastframe.jpeg", _img);

            _session.PictureCount++;
        }

        public void Snap()
        {
            cam.Capture();
        }
    }
}
