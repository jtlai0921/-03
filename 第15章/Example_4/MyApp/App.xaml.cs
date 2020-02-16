using System;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Media.SpeechRecognition;
using Windows.ApplicationModel.VoiceCommands;

namespace MyApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {


            StorageFile vcdFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///vcd.xml"));
            // 安裝VCD檔案
            await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcdFile);

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                // Set the default language
                rootFrame.Language = "zh-CN";

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {


                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.VoiceCommand)
            {
                VoiceCommandActivatedEventArgs varg = (VoiceCommandActivatedEventArgs)args;
                // 處理識別結果
                SpeechRecognitionResult res = varg.Result;
                // 取得已識別的指令名字
                string cmdName = res.RulePath[0];
                if (cmdName == "open")
                {
                    // 取得PhraseList中被識別出來的項
                    var interpretation = res.SemanticInterpretation;
                    if (interpretation != null)
                    {
                        // 透過PhraseList的Label屬性可以查詢出被識別的Item
                        string item = interpretation.Properties["pages"].FirstOrDefault();
                        if (!string.IsNullOrEmpty(item))
                        {
                            // 導覽到對應頁面
                            Frame root = Window.Current.Content as Frame;
                            if (root == null)
                            {
                                root = new Frame();
                                Window.Current.Content = root;
                            }
                            switch (item)
                            {
                                case "我的音樂":
                                    root.Navigate(typeof(MyMusicPage));
                                    break;
                                case "我的視訊":
                                    root.Navigate(typeof(MyVedioPage));
                                    break;
                                case "我的照片":
                                    root.Navigate(typeof(MyPhotoPage));
                                    break;
                                case "主頁":
                                    root.Navigate(typeof(MainPage));
                                    break;
                                default:
                                    root.Navigate(typeof(MainPage));
                                    break;
                            }
                        }
                    }
                }
            }
            Window.Current.Activate();
        }
    }
}