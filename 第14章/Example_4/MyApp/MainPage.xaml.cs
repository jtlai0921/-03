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
        Gyrometer _gyrometer = null;
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            _gyrometer = Gyrometer.GetDefault();
            if (_gyrometer != null)
            {
                // 設定讀數報告頻率
                _gyrometer.ReportInterval = 50;
                // 處理ReadingChanged事件
                _gyrometer.ReadingChanged += _gyrometer_ReadingChanged;
            }
        }

        private async void _gyrometer_ReadingChanged(Gyrometer sender, GyrometerReadingChangedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                GyrometerReading reading = args.Reading;
                // 顯示讀數
                tbX.Text = reading.AngularVelocityX.ToString("0.00");
                tbY.Text = reading.AngularVelocityY.ToString("0.00");
                tbZ.Text = reading.AngularVelocityZ.ToString("0.00");
            });
        }
    }
}
