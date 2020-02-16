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
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;

// “空白頁”項範本在 http://go.microsoft.com/fwlink/?LinkId=391641 上有介紹

namespace ClientApp
{
    /// <summary>
    /// 可用於自己或導覽至 Frame 內定的空白頁。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// 在此頁將要在 Frame 中顯示時進行呼叫。
        /// </summary>
        /// <param name="e">描述如何存取此頁的事件資料。
        /// 此參數通常用於組態頁。</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var uploadTasks = await BackgroundUploader.GetCurrentUploadsAsync();
            if (uploadTasks.Count > 0)
            {
                UploadOperation uploadOpt = uploadTasks[0];
                this.SetUpLoad(uploadOpt, false);
            }
        }

        private async void OnClick(object sender, RoutedEventArgs e)
        {
            int n = (await BackgroundUploader.GetCurrentUploadsAsync()).Count;
            if (n > 0)
            {
                return;
            }

            tbMsg.Text = "";
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            picker.FileTypeFilter.Add("*"); //表示所有檔案型態
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                BackgroundUploader uploader = new BackgroundUploader();
                // 設定檔名
                uploader.SetRequestHeader("file_name", file.Name);
                uploader.Method = "POST";
                // 建立上傳工作
                UploadOperation uploadOpt = uploader.CreateUpload(new Uri(txtUploadUri.Text.Trim()), file);
                SetUpLoad(uploadOpt, true);
            }
        }

        /// <summary>
        /// 執行背景上傳動作
        /// </summary>
        /// <param name="opr">動作背景傳輸工作的物件</param>
        /// <param name="starting">是否為新的上傳工作</param>
        public async void SetUpLoad(UploadOperation opr, bool starting)
        {
            // 當上傳進度更新時能收到報告
            Progress<UploadOperation> progressReporter = new Progress<UploadOperation>(OnProgressHandler);
            // 啟動或附加工作
            try
            {
                if (starting)
                {
                    await opr.StartAsync().AsTask(progressReporter);
                }
                else
                {
                    await opr.AttachAsync().AsTask(progressReporter);
                }
            }
            catch (Exception ex)
            {
                var state = BackgroundTransferError.GetStatus(ex.HResult);
                ShowMessage("錯誤：" + state);
            }
        }

        private void ShowMessage(string msg)
        {
            var res = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    tbMsg.Text = msg;
                });
        }

        private void OnProgressHandler(UploadOperation p)
        {
            BackgroundUploadProgress progress = p.Progress;
            switch (progress.Status)
            {
                case BackgroundTransferStatus.Canceled: //已取消
                    ShowMessage("工作已取消。");
                    pb.Value = 0d;
                    break;
                case BackgroundTransferStatus.Completed: //完成
                    ShowMessage("工作已完成。");
                    pb.Value = 0d;
                    break;
                case BackgroundTransferStatus.Error: //錯誤
                    ShowMessage("發生錯誤。");
                    break;
                case BackgroundTransferStatus.Running: //正在執行
                    double ps = progress.BytesSent * 100 / progress.TotalBytesToSend;
                    pb.Value = ps;
                    ShowMessage(string.Format("已上傳{0}位元組，共{1}位元組。", progress.BytesSent, progress.TotalBytesToSend));
                    break;
            }
        }
    }
}
