using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Sensors;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace MyApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Accelerometer _accelerometer = null;
        Random rand = null;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            rand = new Random();
            _accelerometer = Accelerometer.GetDefault();
            if (_accelerometer != null)
            {
                _accelerometer.ReportInterval = 30;
                _accelerometer.ReadingChanged += _accelerometer_ReadingChanged;
            }
        }

        private async void _accelerometer_ReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
             {
                 AccelerometerReading reading = args.Reading;
                 // 取得每個座標軸上的值的絕對值
                 double ax = Math.Abs(reading.AccelerationX);
                 double ay = Math.Abs(reading.AccelerationY);
                 double az = Math.Abs(reading.AccelerationZ);
                 // 判斷
                 if (ax >= 1.8d || ay >= 1.5d || az >= 1.8d)
                 {
                     // 產生隨機值
                     int v = rand.Next(0, 100);
                     tbNum.Text = v.ToString();
                 }
             });
        }
    }
}
