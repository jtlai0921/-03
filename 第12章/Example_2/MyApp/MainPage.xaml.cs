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

        private async void OnWrite(object sender, RoutedEventArgs e)
        {
            if (txtInput.Text.Length < 1) return;

            Button button = sender as Button;
            button.IsEnabled = false;
            // 取得本機目錄的參考
            StorageFolder local = ApplicationData.Current.LocalFolder;
            // 建立新檔案
            StorageFile newFile = await local.CreateFileAsync("data.txt", CreationCollisionOption.ReplaceExisting);
            // 寫入檔案
            await FileIO.WriteTextAsync(newFile, txtInput.Text);
            button.IsEnabled = true;
        }

        private async void OnRead(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.IsEnabled = false;
            try
            {
                // 直接讀取檔案內容
                txtOutput.Text = await PathIO.ReadTextAsync("ms-appdata:///local/data.txt");
            }
            catch (Exception ex)
            {
                txtOutput.Text = ex.Message;
            }
            btn.IsEnabled = true;
        }

    }
}
