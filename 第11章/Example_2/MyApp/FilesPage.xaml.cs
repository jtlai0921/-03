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
using Windows.Storage;

namespace FileActiveApp
{
    /// <summary>
    /// 可用於自己或導覽至 Frame 內定的空白頁。
    /// </summary>
    public sealed partial class FilesPage : Page
    {
        public FilesPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 取得文件庫目錄
            StorageFolder docLib = KnownFolders.DocumentsLibrary;
            // 取得文件庫目錄下的檔案清單
            IReadOnlyList<StorageFile> files = await docLib.GetFilesAsync();
            // 設定資料源
            this.lvFiles.ItemsSource = files;
        }

        private async void OnLvItemClick(object sender, ItemClickEventArgs e)
        {
            // 獲得被點擊的項
            StorageFile theFile = e.ClickedItem as StorageFile;
            if (theFile != null)
            {
                // 透過Launcher元件開啟檔案
                await Windows.System.Launcher.LaunchFileAsync(theFile);
            }
        }

        private void OnHome(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
