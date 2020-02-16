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
using Windows.UI;

// “空白頁”項範本在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介紹

namespace MyApp
{
    /// <summary>
    /// 可用於自己或導覽至 Frame 內定的空白頁。
    /// </summary>
    public sealed partial class ShowColorPage : Page
    {
        public ShowColorPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 在此頁將要在 Frame 中顯示時進行呼叫。
        /// </summary>
        /// <param name="e">描述如何存取此頁的事件資料。
        /// 此參數通常用於組態頁。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            IDictionary<string, string> dic = e.Parameter as IDictionary<string, string>;
            if (dic != null)
            {
                // 讀出各個彩色通道的值
                byte r, g, b;
                if (!byte.TryParse(dic["r"],out r))
                {
                    r = 0;
                }
                if (!byte.TryParse(dic["g"],out g))
                {
                    g = 0;
                }
                if (!byte.TryParse(dic["b"], out b))
                {
                    b = 0;
                }
                // 修改筆刷的彩色
                this.sldbrush.Color = ColorHelper.FromArgb(255, r, g, b);
            }
        }

        private void OnHome(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
