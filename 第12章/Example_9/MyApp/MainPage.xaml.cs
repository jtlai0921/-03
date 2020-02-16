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
using Windows.Data.Xml.Dom;

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

        private async void OnBuildXml(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.IsEnabled = false;

            // 建立XmlDocument案例
            XmlDocument xdoc = new XmlDocument();
            // 建立根節點
            XmlElement root = xdoc.CreateElement("books");
            // 建立子元素
            XmlElement book = xdoc.CreateElement("book");
            // 將根節點追加到XML文件中
            xdoc.AppendChild(root);
            // 設定特性值
            book.SetAttribute("ISBM", "100658425");
            // 建立文字節點
            XmlText text = xdoc.CreateTextNode("範例圖書");
            // 將文字節點新增到book節點中
            book.AppendChild(text);
            // 將book節點新增到根節點上
            root.AppendChild(book);
            // 取得本機目錄的參考
            StorageFolder local = ApplicationData.Current.LocalFolder;
            // 建立新檔案
            StorageFile xmlFile = await local.CreateFileAsync("test.xml", CreationCollisionOption.ReplaceExisting);
            // 將新增的XML文件儲存到檔案
            await xdoc.SaveToFileAsync(xmlFile);

            btn.IsEnabled = true;
        }

        private async void OnReadXml(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.IsEnabled = false;

            // 從本機檔案中載入XML
            StorageFolder local = ApplicationData.Current.LocalFolder;
            StorageFile xmlfile = await local.GetFileAsync("test.xml");
            XmlDocument xdoc = await XmlDocument.LoadFromFileAsync(xmlfile);
            // 顯示XML文件
            tbXml.Text = xdoc.GetXml();

            btn.IsEnabled = true;
        }
    }
}
