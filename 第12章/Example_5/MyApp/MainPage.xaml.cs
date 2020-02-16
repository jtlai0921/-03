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

        /// <summary>
        /// 在此頁將要在 Frame 中顯示時進行呼叫。
        /// </summary>
        /// <param name="e">描述如何存取此頁的事件資料。
        /// 此參數通常用於組態頁。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 從套用設定中讀取訊息
            var rootContainer = ApplicationData.Current.LocalSettings;
            object cmbIndex, text1, text2, toggleVal;
            if (rootContainer.Values.TryGetValue(SETTING_UPDATE_FREQUENCY, out cmbIndex))
            {
                cmb.SelectedIndex = (int)cmbIndex;
            }
            if (rootContainer.Values.TryGetValue(SETTING_TEXT_A, out text1))
            {
                txt1.Text = text1 as string;
            }
            if (rootContainer.Values.TryGetValue(SETTING_TEXT_B, out text2))
            {
                txt2.Text = text2 as string;
            }
            if (rootContainer.Values.TryGetValue(SETTING_TOGGLE_VALUE, out toggleVal))
            {
                tgswitch.IsOn = (bool)toggleVal;
            }
        }

        #region 常數清單
        const string SETTING_UPDATE_FREQUENCY = "update_frequency";
        const string SETTING_TEXT_A = "text_a";
        const string SETTING_TEXT_B = "text_b";
        const string SETTING_TOGGLE_VALUE = "toggle_val";
        #endregion

        private void OnCmbSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cmb.SelectedIndex;
            if (index > -1)
            {
                // 將下拉式選單中勾選項的索引存入套用設定中
                // 取得根容器的參考
                ApplicationDataContainer root = ApplicationData.Current.LocalSettings;
                // 向根容器寫入新設定項
                root.Values[SETTING_UPDATE_FREQUENCY] = index;
            }
        }

        private void OnText1Lostfocus(object sender, RoutedEventArgs e)
        {
            // 將文字框中輸入的文字存入設定中
            if (!string.IsNullOrWhiteSpace(txt1.Text))
            {
                ApplicationData.Current.LocalSettings.Values[SETTING_TEXT_A] = txt1.Text;
            }
        }

        private void OnText2Lostfocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt2.Text))
            {
                ApplicationData.Current.LocalSettings.Values[SETTING_TEXT_B] = txt2.Text;
            }
        }

        private void OnToggled(object sender, RoutedEventArgs e)
        {
            // 將ToggleSwitch控制項的目前狀態存入套用設定
            ApplicationData.Current.LocalSettings.Values[SETTING_TOGGLE_VALUE] = tgswitch.IsOn;
        }
    }
}
