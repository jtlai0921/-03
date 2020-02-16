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

namespace MyApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Barometer _barometer = null;
        public MainPage()
        {
            this.InitializeComponent();

            _barometer = Barometer.GetDefault();
            if (_barometer != null)
            {
                // 設定報告頻率
                _barometer.ReportInterval = 100;
                // 處理ReadingChanged事件
                _barometer.ReadingChanged += _barometer_ReadingChanged;
            }
        }

        private async void _barometer_ReadingChanged(Barometer sender, BarometerReadingChangedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                BarometerReading reading = args.Reading;
                // 顯示讀數
                tbValue.Text = reading.StationPressureInHectopascals.ToString("0.00");
            });
        }
    }
}
