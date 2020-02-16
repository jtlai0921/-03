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
        Inclinometer _inclinometer = null;
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            _inclinometer = Inclinometer.GetDefault();
            if (_inclinometer != null)
            {
                // 設定讀數報告頻率
                _inclinometer.ReportInterval = 50;
                // 處理事件
                _inclinometer.ReadingChanged += _inclinometer_ReadingChanged;
            }
        }

        private async void _inclinometer_ReadingChanged(Inclinometer sender, InclinometerReadingChangedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
             {
                 InclinometerReading reading = args.Reading;
                 // 顯示讀數
                 tbPitch.Text = reading.PitchDegrees.ToString("0.000");
                 tbRoll.Text = reading.RollDegrees.ToString("0.000");
                 tbYaw.Text = reading.YawDegrees.ToString("0.000");
             });
        }
    }
}
