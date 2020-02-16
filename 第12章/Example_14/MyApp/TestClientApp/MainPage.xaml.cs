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
using Windows.ApplicationModel.AppService;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestClientApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void OnCompt(object sender, RoutedEventArgs e)
        {
            // 驗證輸入
            if (txtPackName.Text.Length == 0) return;
            int n1, n2;
            if (!int.TryParse(txtNum1.Text, out n1)) return;
            if (!int.TryParse(txtNum2.Text, out n2)) return;
            Button btn = sender as Button;
            btn.IsEnabled = false;

            AppServiceConnection conn = new AppServiceConnection();
            // 設定必備屬性
            conn.PackageFamilyName = txtPackName.Text;
            conn.AppServiceName = ((ComboBoxItem)cmbServiceName.SelectedItem).Content.ToString();
            // 開啟連線
            var state = await conn.OpenAsync();
            if (state == AppServiceConnectionStatus.Success)
            {
                // 收集資料
                ValueSet p = new ValueSet();
                p["num1"] = n1;
                p["num2"] = n2;
                // 傳送請求
                AppServiceResponse response = await conn.SendMessageAsync(p);
                // 取得結果
                if (response.Status == AppServiceResponseStatus.Success)
                {
                    runRes.Text = response.Message["res"].ToString();
                }
            }

            btn.IsEnabled = true;
        }
    }
}
