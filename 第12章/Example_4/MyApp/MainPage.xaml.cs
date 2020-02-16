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

        /// <summary>
        /// 在此頁將要在 Frame 中顯示時進行呼叫。
        /// </summary>
        /// <param name="e">描述如何存取此頁的事件資料。
        /// 此參數通常用於組態頁。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 取得各個目錄的參考
            StorageFolder musicLib = KnownFolders.MusicLibrary;
            StorageFolder picLib = KnownFolders.PicturesLibrary;
            StorageFolder docLib = KnownFolders.DocumentsLibrary;
            StorageFolder videoLib = KnownFolders.VideosLibrary;
            // 顯示各個庫的名稱
            tbMusicLibName.Text = musicLib.Name;
            tbVideoLibName.Text = videoLib.Name;
            tbPicLibName.Text = picLib.Name;
            tbDocLibName.Text = docLib.Name;
        }
    }
}
