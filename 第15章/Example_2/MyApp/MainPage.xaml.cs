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
using Windows.Storage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace MyApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private async void OnClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.IsEnabled = false;

            using (SpeechRecognizer recognizer = new SpeechRecognizer())
            {
                try
                {
                    // 載入語法檔案
                    StorageFile sgrsFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///sgrs.xml"));
                    // 建立識別約束
                    SpeechRecognitionGrammarFileConstraint grammarfileConstraint = new SpeechRecognitionGrammarFileConstraint(sgrsFile);
                    // 加入到約束集合中
                    recognizer.Constraints.Add(grammarfileConstraint);
                    // 編譯約束
                    SpeechRecognitionCompilationResult compilationResult = await recognizer.CompileConstraintsAsync();
                    if (compilationResult.Status == SpeechRecognitionResultStatus.Success)
                    {
                        // 開始識別
                        SpeechRecognitionResult result = await recognizer.RecognizeAsync();
                        if(result.Status == SpeechRecognitionResultStatus.Success)
                        {
                            tbRes.Text = string.Format("識別結果：{0}。", result.Text);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }

                btn.IsEnabled = true;
        }
    }
}
