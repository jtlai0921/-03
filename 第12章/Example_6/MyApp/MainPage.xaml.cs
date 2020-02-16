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
// “空白頁”項範本在 http://go.microsoft.com/fwlink/?LinkId=391641 上有介紹

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


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            StorageFolder rmfolder = KnownFolders.RemovableDevices;
            if (rmfolder != null)
            {
                cmbRemovables.ItemsSource = await rmfolder.GetFoldersAsync();
            }
        }

        private async void OnSave(object sender, RoutedEventArgs e)
        {
            // 從下拉清單中取出被選項
            StorageFolder selFolder = cmbRemovables.SelectedItem as StorageFolder;
            if (selFolder == null || txtInput.Text.Length == 0)
            {
                return;
            }

            // 建立新檔案
            StorageFile file = await selFolder.CreateFileAsync("abc.txt", CreationCollisionOption.ReplaceExisting);
            // 向檔案寫入內容
            await FileIO.WriteTextAsync(file, txtInput.Text);
        }

    }


}
