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
        Altimeter _altimeter = null;
        public MainPage()
        {
            this.InitializeComponent();

            _altimeter = Altimeter.GetDefault();
            if (_altimeter != null)
            {
                _altimeter.ReportInterval = 100;
                _altimeter.ReadingChanged += _altimeter_ReadingChanged;
            }
        }

        private async void _altimeter_ReadingChanged(Altimeter sender, AltimeterReadingChangedEventArgs args)
        {
            // 顯示海拔高度
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
             {
                 tbVal.Text = string.Format("{0:0.000} 米", args.Reading.AltitudeChangeInMeters);
             });
        }
    }
}
