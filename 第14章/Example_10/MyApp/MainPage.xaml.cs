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
using Windows.Devices.Geolocation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MyApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Geolocator _geo = null;
        public MainPage()
        {
            this.InitializeComponent();
            // 案例化Geolocator物件
            _geo = new Geolocator();
            // 設定報告讀數頻率
            _geo.ReportInterval = 100;
            // 響應PositionChanged事件
            _geo.PositionChanged += _geo_PositionChanged;
        }

        private async void _geo_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
             {
                 Geoposition position = args.Position;
                 Geocoordinate coordinate = position.Coordinate;
                 // 顯示位置訊息
                 string msg = "";
                 msg += "經度：" + coordinate.Point.Position.Longitude.ToString("0.00") + "\n";
                 msg += "緯度：" + coordinate.Point.Position.Latitude.ToString("0.00") + "\n";
                 msg += "來源：" + coordinate.PositionSource.ToString();
                 tbLocInfo.Text = msg;
             });
        }
    }
}
