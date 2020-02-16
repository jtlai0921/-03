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
using Windows.Storage.Streams;

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

        private async void OnWrite(object sender, RoutedEventArgs e)
        {
            // 取得本機目錄參考
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            // 建立新檔案
            StorageFile file = await localFolder.CreateFileAsync("demo.dat", CreationCollisionOption.ReplaceExisting);
            // 開啟檔案流
            using (IRandomAccessStream stream= await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                // 案例化DataWriter
                DataWriter dw = new DataWriter(stream);
                // 設定預設解碼格式
                dw.UnicodeEncoding = UnicodeEncoding.Utf8;
                // 寫入bool值
                dw.WriteBoolean(true);
                // 寫入日期時間值
                DateTime dt = new DateTime(2010, 8, 21);
                dw.WriteDateTime(dt);
                // 寫入字串
                string str = "測試文字";
                // 計算字串的長度
                uint len = dw.MeasureString(str);
                // 先寫入字串的長度
                dw.WriteUInt32(len);
                // 再寫入字串
                dw.WriteString(str);
                // 以下方法必須呼叫
                await dw.StoreAsync();
                // 解除DataWriter與流的關聯
                dw.DetachStream();
                dw.Dispose();
            }
            Windows.UI.Popups.MessageDialog msgDlg = new Windows.UI.Popups.MessageDialog("儲存成功。");
            await msgDlg.ShowAsync();
        }

        private async void OnRead(object sender, RoutedEventArgs e)
        {
            // 取得本機目錄的參考
            StorageFolder local = ApplicationData.Current.LocalFolder;
            // 取得檔案
            StorageFile file = await local.GetFileAsync("demo.dat");
            if (file != null)
            {
                string strres = "讀到的內容：\n";
                // 開啟檔案流
                using (IRandomAccessStream stream = await file.OpenReadAsync())
                {
                    // 案例化DataReader
                    DataReader dr = new DataReader(stream);
                    // 讀出時的解碼格式要與寫入時使用的解碼格式相同
                    dr.UnicodeEncoding = UnicodeEncoding.Utf8;
                    // 從流中載入所有資料
                    await dr.LoadAsync((uint)stream.Size);
                    // 讀出的順序與寫入的順序相同
                    // 讀取bool值
                    bool b = dr.ReadBoolean();
                    strres += b.ToString() + "\n";
                    // 讀取日期時間值
                    DateTimeOffset dt = dr.ReadDateTime();
                    strres += dt.ToString("yyyy-M-d") + "\n";
                    // 讀取字串
                    // 讀取長度
                    uint len = dr.ReadUInt32();
                    if (len > 0)
                    {
                        strres += dr.ReadString(len);
                    }
                    dr.Dispose();
                }
                tbResult.Text = strres;
            }
        }

    }
}
