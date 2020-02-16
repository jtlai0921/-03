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

        /// <summary>
        /// 在此頁將要在 Frame 中顯示時進行呼叫。
        /// </summary>
        /// <param name="e">描述如何存取此頁的事件資料。
        /// 此參數通常用於組態頁。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: 準備此處顯示的頁面。

            // TODO: 若果您的套用程式包括多個頁面，請確保
            // 透過登錄以下事件來處理硬體“後退”按鈕:
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed 事件。
            // 若果使用由某些範本提供的 NavigationHelper，
            // 則系統會為您處理該事件。
        }

        /// <summary>
        /// 向檔案寫入位元組序列
        /// </summary>
        private async void OnWriteToFile(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.IsEnabled = false;
            // 生產位元組陣列
            Random rand = new Random();
            byte[] data = new byte[8];
            rand.NextBytes(data);
            // 從位元組陣列產生緩沖區物件
            Windows.Storage.Streams.IBuffer buffer = data.AsBuffer();
            // 獲得本機目錄參考
            Windows.Storage.StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            // 在本機目錄中建立新檔案
            Windows.Storage.StorageFile newFile = await local.CreateFileAsync("my.data", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            // 開啟檔案流
            using (Windows.Storage.Streams.IRandomAccessStream stream = await newFile.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
            {
                // 將緩沖區的內容寫入流
                await stream.WriteAsync(buffer);
            }
            // 顯示已寫入的位元組序列
            tbBytes.Text = string.Format("已向檔案寫入：{0}", BitConverter.ToString(data));
            btn.IsEnabled = true;
        }

        /// <summary>
        /// 從檔案讀取位元組序列
        /// </summary>
        private async void OnReadFromFile(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.IsEnabled = false;
            // 取得本機目錄參考
            Windows.Storage.StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            // 取得檔案
            Windows.Storage.StorageFile file = await local.GetFileAsync("my.data");
            if (file != null)
            {
                // 用於存放讀到的資料的緩沖區
                Windows.Storage.Streams.IBuffer buffer = null;
                // 開啟流
                using (Windows.Storage.Streams.IRandomAccessStream streamIn = await file.OpenReadAsync())
                {
                    // 案例化緩沖區物件
                    buffer = System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeBuffer.Create((int)streamIn.Size);
                    // 從流中讀入資料，存放在緩沖區中
                    await streamIn.ReadAsync(buffer, buffer.Capacity, Windows.Storage.Streams.InputStreamOptions.None);
                }
                // 顯示讀到的位元組序列
                tbReadBytes.Text = "讀到的位元組序列：" + BitConverter.ToString(buffer.ToArray());
            }
            btn.IsEnabled = true;
        }

    }
}
