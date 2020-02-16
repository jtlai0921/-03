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
using Windows.UI.Popups;


namespace MyApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        Compass _compass = null;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.Loaded += MainPage_Loaded;

            Window.Current.VisibilityChanged += OnWindowVisibilityChanged;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // 取得目前裝置上的羅碟感知器
            _compass = Compass.GetDefault();
            if (_compass == null)
            {
                // 裝置不支援羅碟感知器
                MessageDialog msgBox = new MessageDialog("此裝置不支援羅碟。");
                var ac = msgBox.ShowAsync();
                return;
            }
            // 設定讀取頻率
            _compass.ReportInterval = 30;
            // 處理ReadingChanged事件
            _compass.ReadingChanged += _compass_ReadingChanged;
        }

        private void _compass_ReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
        {
            // 讀取資料
            CompassReading reading = args.Reading;
            double trueNorth = reading.HeadingTrueNorth.HasValue ? reading.HeadingTrueNorth.Value : 0d;
            double magNorth = reading.HeadingMagneticNorth;
            // 顯示資料
            var r = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    runGN.Text = trueNorth.ToString("0.000");
                    runMN.Text = magNorth.ToString("0.000");
                });
        }

        private void OnWindowVisibilityChanged(object sender, Windows.UI.Core.VisibilityChangedEventArgs e)
        {
            if (_compass == null)
            {
                return;
            }
            if (e.Visible)
            {
                // 若果目前視窗可見，則讀取資料
                _compass.ReadingChanged += _compass_ReadingChanged;
            }
            else
            {
                // 若果目前視窗不可見，則不再讀取資料
                _compass.ReadingChanged -= _compass_ReadingChanged;
            }
        }
    }
}
