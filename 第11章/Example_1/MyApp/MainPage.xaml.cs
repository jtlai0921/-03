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
using Windows.System;
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


        private async void OnLinkClick(object sender, RoutedEventArgs e)
        {
            HyperlinkButton linkButton = sender as HyperlinkButton;
            string tag = linkButton.Tag as string;
            linkButton.IsEnabled = false;
            switch (tag)
            {
                case "b": //藍芽設定
                    await Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:"));
                    break;
                case "w": //Wlan設定
                    await Launcher.LaunchUriAsync(new Uri("ms-settings-wifi:"));
                    break;
                case "p": //節電模式設定
                    await Launcher.LaunchUriAsync(new Uri("ms-settings-power:"));
                    break;
                case "a": //飛行模式設定
                    await Launcher.LaunchUriAsync(new Uri("ms-settings-airplanemode:"));
                    break;
                case "l": //鎖屏設定
                    await Launcher.LaunchUriAsync(new Uri("ms-settings-lock:"));
                    break;
                case "m": //開啟音樂庫目錄
                    StorageFolder musicLeb = KnownFolders.MusicLibrary;
                    await Launcher.LaunchFolderAsync(musicLeb);
                    break;
            }
            linkButton.IsEnabled = true;
        }
    }
}
