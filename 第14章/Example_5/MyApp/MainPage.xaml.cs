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
        LightSensor _lightsensor = null;
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            _lightsensor = LightSensor.GetDefault();
            if (_lightsensor != null)
            {
                // 設定資料報告頻率
                _lightsensor.ReportInterval = 60;
                // 處理ReadingChanged事件
                _lightsensor.ReadingChanged += _lightsensor_ReadingChanged;
            }
        }

        private async void _lightsensor_ReadingChanged(LightSensor sender, LightSensorReadingChangedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
             {
                 LightSensorReading reading = args.Reading;
                 // 顯示讀數
                 tbLts.Text = reading.IlluminanceInLux.ToString("0.0000");
             });
        }
    }
}
