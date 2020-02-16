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
using Windows.Media.SpeechRecognition;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace MyApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        SpeechRecognizer _recognizer = null;
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.Loaded += Page_Loaded;
            this.Unloaded += Page_Unloaded;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            // 釋放資源
            _recognizer.Dispose();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _recognizer = new SpeechRecognizer();
            // 建立自訂子句約束
            string[] array = { "足球", "排球", "跑步", "羽毛球", "籃球" };
            SpeechRecognitionListConstraint listConstraint = new SpeechRecognitionListConstraint(array);
            // 加入約束案例到集合中
            _recognizer.Constraints.Add(listConstraint);
            // 編譯約束
            await _recognizer.CompileConstraintsAsync();
        }

        private async void OnClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.IsEnabled = false;

            try
            {
                SpeechRecognitionResult res = await _recognizer.RecognizeAsync();
                if (res.Status == SpeechRecognitionResultStatus.Success)
                {
                    // 處理識別結果
                    this.lb.SelectedItem = res.Text;
                }
            }
            catch { /* 忽略例外 */ }

            btn.IsEnabled = true;
        }
    }
}
