using Nikon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SMoOnion
{
    public partial class MainWindow : Window
    {
        Camera camera;
        System.Timers.Timer _timer;

        private BatteryChecker _batteryChecker;

        private byte[] _lastFrameBytes;

        private string _path;

        public MainWindow()
        {
            InitializeComponent();

            camera = new Camera();

            _timer = new System.Timers.Timer();

            _batteryChecker = new BatteryChecker(camera);

            _path = AppDomain.CurrentDomain.BaseDirectory + "/pictures/";
        }

        private void Application_Closing(object sender, CancelEventArgs e)
        {
            camera.manager.Shutdown();
        }

        private void setSessionButton_Click(object sender, RoutedEventArgs e)
        {
            if (sessionNameTextbox.Text != "")
            {
                bool sessionAlreadyExists = System.IO.Directory.Exists(_path + sessionNameTextbox.Text);

                if (!sessionAlreadyExists)
                {
                    Session newSession = new Session();
                    newSession.Name = sessionNameTextbox.Text;
                    camera.Session = newSession;

                    sessionLabel.Content = "working on session: " + camera.Session.Name;
                    sessionLabel.Visibility = Visibility.Visible;
                    
                    setSessionButton.Visibility = Visibility.Hidden;
                    sessionNameTextbox.Visibility = Visibility.Hidden;
                }
                else
                {
                    MessageBox.Show("Session folder already exists, type another session name", "Error");
                }
            }
            else
            {
                MessageBox.Show("Session name cannot be blank", "Error");
            }
        }



        private void captureButton_Click(object sender, RoutedEventArgs e)
        {
        
            if (camera.cam != null &&
                camera.cam.LiveViewEnabled &&
                camera.Session != null)
            {

                StopLiveView();

                camera.Snap();

                if (!batteryBar.IsVisible)
                    batteryBar.Visibility = Visibility.Visible;

                if (!batteryLevelLabel.IsVisible)
                    batteryLevelLabel.Visibility = Visibility.Visible;

                batteryBar.Value = _batteryChecker.ReturnBatteryLevel();
            }
            else
            {
                MessageBox.Show("Verify that camera is turned on, properly connected through USB cable, LiveView is activated and at least 10% remaining battery", "Unable to snap");
            }
            
        }
             
        public void SetLastFrameOnOnion()
        {

            StartLiveView();


            _lastFrameBytes = System.IO.File.ReadAllBytes(_path + "/temp/" + "lastframe.jpeg");

            this.Dispatcher.Invoke(() =>
            {
                lastFrameBox.Source = LoadImage(_lastFrameBytes);
            });
        }
        
        private void SetLiveViewTimer()
        {
            try
            {
                camera.cam.LiveViewEnabled = true;
                _timer.Elapsed += Timer_Elapsed;
                _timer.Interval = 41;
                _timer.Enabled = true;

            }
            catch { }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                NikonLiveViewImage img = camera.cam.GetLiveViewImage();

                this.Dispatcher.Invoke(() =>
                {
                    liveFeedDisplay.Source = LoadImage(img.JpegBuffer);
                });
            }
            catch { }

        }

        private BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        private void startLiveViewButton_Click(object sender, RoutedEventArgs e)
        {
            StartLiveView();
        }
        private void stopLiveViewButton_Click(object sender, RoutedEventArgs e)
        {
            StopLiveView();
        }

        private void StartLiveView()
        {
            if (camera.cam != null)
            {
                camera.cam.LiveViewEnabled = false;
                SetLiveViewTimer();

                Dispatcher.Invoke(() =>
                {
                    liveViewFeedLabel.Content = "LiveView feed: on";

                    startLiveViewButton.IsEnabled = false;
                    stopLiveViewButton.IsEnabled = true;
                });
            }
            else
            {
                MessageBox.Show("Verify that camera is turned on, properly connected through USB cable and at least 10% remaining battery", "Unable to find camera");
            }
        }

        private void StopLiveView()
        {
            camera.cam.LiveViewEnabled = false;
            _timer.Enabled = false;

            liveViewFeedLabel.Content = "LiveView feed: off";

            startLiveViewButton.IsEnabled = true;
            stopLiveViewButton.IsEnabled = false;
        }

        private void onionSkinSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double truncated = Math.Round(onionSkinSlider.Value, 1);
            
            onionSliderLabel.Content = "Onion skin opacity: " + (truncated * 10).ToString() + "%";

            lastFrameBox.Opacity = onionSkinSlider.Value / 10;
        }
    }
}
