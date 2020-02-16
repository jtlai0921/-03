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
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace MyApp
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


        private async void OnPick(object sender, RoutedEventArgs e)
        {
            FolderPicker picker = new FolderPicker();
            picker.FileTypeFilter.Add("*");
            StorageFolder folder = await picker.PickSingleFolderAsync();
            if (folder != null)
            {
                StorageApplicationPermissions.FutureAccessList.Clear();
                // 向存取清單加入項
                StorageApplicationPermissions.FutureAccessList.Add(folder);
                tbMsg.Text = "已加入目錄" + folder.Path + "到存取清單中。";
            }
        }

        private async void OnListFiles(object sender, RoutedEventArgs e)
        {
            if (StorageApplicationPermissions.FutureAccessList.Entries.Count == 0)
            {
                return;
            }
            Button btn = sender as Button;
            btn.IsEnabled = false;

            // 取得存取清單項的標誌
            string token = StorageApplicationPermissions.FutureAccessList.Entries[0].Token;
            // 獲得存取清單中儲存的目錄參考
            StorageFolder fd = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(token);
            if (fd != null)
            {
                // 列出目錄中的子檔案
                IReadOnlyList<StorageFile> files = await fd.GetFilesAsync();
                // 在ListView控制項中顯示檔案清單
                lvFiles.Items.Clear();
                foreach (StorageFile f in files)
                {
                    lvFiles.Items.Add(f.Name);
                }
            }

            btn.IsEnabled = true;
        }
    }
}
