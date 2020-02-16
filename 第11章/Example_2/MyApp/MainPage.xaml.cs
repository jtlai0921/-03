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
using Windows.Data.Json;
using Windows.UI.Popups;

namespace FileActiveApp
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

        private async void OnClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            MessageDialog msgBox = new MessageDialog("請輸入姓名、城市和年齡。");
            if (txtName.Text =="" || txtCity.Text == "" || txtAge.Text == "")
            {
                await msgBox.ShowAsync();
                return;
            }
            btn.IsEnabled = false;
            // 取得文件庫目錄
            StorageFolder doclib = KnownFolders.DocumentsLibrary;
            // 將輸入的內容轉化為JSON物件
            JsonObject objjson = new JsonObject();
            objjson.Add("name", JsonValue.CreateStringValue(txtName.Text));
            objjson.Add("city", JsonValue.CreateStringValue(txtCity.Text));
            objjson.Add("age", JsonValue.CreateNumberValue(Convert.ToDouble(txtAge.Text)));
            // 分析出JSON字串
            string jsonStr = objjson.Stringify();
            // 在文件庫中建立新檔案
            string fileName = DateTime.Now.ToString("yyyy-M-d-HHmmss") + ".myddc";
            StorageFile newFile = await doclib.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            // 將JSON字串寫入檔案
            await FileIO.WriteTextAsync(newFile, jsonStr);
            btn.IsEnabled = true;
            msgBox.Content = "儲存成功。";
            await msgBox.ShowAsync();
        }

        private void OnFileList(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FilesPage));
        }
    }
}
