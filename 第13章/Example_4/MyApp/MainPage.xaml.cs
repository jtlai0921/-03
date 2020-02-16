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
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.Web.Http;

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

        private async void OnGetImage(object sender, RoutedEventArgs e)
        {
            if (txtUri.Text.Length == 0) return;

            Button b = sender as Button;
            b.IsEnabled = false;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // 直接向伺服器傳送GET請求
                    HttpResponseMessage response = await client.GetAsync(new Uri(txtUri.Text));
                    // 若果請求成功
                    if (response != null && response.StatusCode == HttpStatusCode.Ok)
                    {
                        // 顯示獲得的圖形
                        BitmapImage bmp = new BitmapImage();
                        bmp.DecodePixelWidth = 400;
                        using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                        {
                            await response.Content.WriteToStreamAsync(stream);
                            stream.Seek(0UL); //將讀取位置設定為流的開始處
                            bmp.SetSource(stream);
                        }
                        this.img.Source = bmp;
                    }
                }
                catch
                {
                    // 忽略例外
                }
            }

            b.IsEnabled = true;
        }
    }
}
