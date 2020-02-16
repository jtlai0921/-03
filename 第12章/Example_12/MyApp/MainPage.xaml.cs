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
using Windows.ApplicationModel.DataTransfer;


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

            DataTransferManager transferMgr = DataTransferManager.GetForCurrentView();
            // 處理關聯事件
            transferMgr.DataRequested += TransferMgr_DataRequested;
        }


        private void TransferMgr_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            // 設定要共享的資料內容
            var deferral = args.Request.GetDeferral();
            DataPackage package = args.Request.Data;
            package.SetText(txtInput.Text.Trim());
            // 設定標題屬性
            package.Properties.Title = "分享文字";
            // 報告資料寫入完成
            deferral.Complete();
        }

        private void OnShare(object sender, RoutedEventArgs e)
        {
            if (txtInput.Text.Length == 0) return;

            DataTransferManager.ShowShareUI();
        }
    }
}
