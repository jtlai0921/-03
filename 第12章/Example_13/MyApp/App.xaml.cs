using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

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
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                rootFrame.Language = "zh-cn";

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;


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

        protected async override void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            ShareOperation opt = args.ShareOperation;
            // 讀取資料
            DataPackageView packview = opt.Data;
            if (packview.Contains(StandardDataFormats.StorageItems))
            {
                // 報告開始接收
                opt.ReportStarted();
                try
                {
                    IReadOnlyList<IStorageItem> storageitems = await packview.GetStorageItemsAsync();
                    StorageFolder local = ApplicationData.Current.LocalFolder;
                    foreach (IStorageItem stitem in storageitems)
                    {
                        if (stitem.IsOfType(StorageItemTypes.File))
                        {
                            // 將檔案複製到本機目錄
                            StorageFile thefile = (StorageFile)stitem;
                            await thefile.CopyAsync(local,thefile.Name, NameCollisionOption.ReplaceExisting);
                        }
                    }
                    // 報告接收完成
                    opt.ReportDataRetrieved();
                    // 提示動作成功
                    XmlDocument docx = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
                    var nodes = docx.GetElementsByTagName("text");
                    if (nodes.Count == 2)
                    {
                        XmlElement title = nodes[0] as XmlElement;
                        title.AppendChild(docx.CreateTextNode("共享目的"));
                        XmlElement content = nodes[1] as XmlElement;
                        content.AppendChild(docx.CreateTextNode("內容已成功接收。"));
                    }
                    ToastNotification notifi = new ToastNotification(docx);
                    ToastNotificationManager.CreateToastNotifier().Show(notifi);
                }
                catch (Exception ex)
                {
                    // 報告錯誤
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    opt.ReportError(ex.Message);
                    return;
                }
            }
            // 報告動作完成
            opt.ReportCompleted();
        }
    }
}