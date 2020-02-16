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
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Networking.Connectivity;

// “空白頁”項範本在 http://go.microsoft.com/fwlink/?LinkId=391641 上有介紹

namespace MyApp
{
    /// <summary>
    /// 可用於自己或導覽至 Frame 內定的空白頁。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// 用於接收資料的通訊埠
        /// </summary>
        const string SERVICE_PORT = "795";

        /// <summary>
        /// 用於伺服器的Socket
        /// </summary>
        DatagramSocket svrSocket = null;
        /// <summary>
        /// 用於用戶端的Socket
        /// </summary>
        DatagramSocket clientSocket = null;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.Loaded += async (s1, e1) =>
            {
                svrSocket = new DatagramSocket();
                // 加入接收訊息事件處理
                svrSocket.MessageReceived += sersocket_Received;
                // 綁定到本機通訊埠
                await svrSocket.BindServiceNameAsync(SERVICE_PORT);
                clientSocket = new DatagramSocket();
            };

            this.Unloaded += (s2, e2) =>
                {
                    // 釋放Socket案例
                    svrSocket.Dispose();
                    clientSocket.Dispose();
                };
        }

        async void sersocket_Received(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            // 取得關聯的DataReader物件
            DataReader reader = args.GetDataReader();
            // 讀取訊息內容
            uint len = reader.UnconsumedBufferLength;
            string msg = reader.ReadString(len);
            // 遠端主電腦
            string remoteHost = args.RemoteAddress.DisplayName;
            reader.Dispose();

            // 顯示接收到的訊息
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    lvMsg.Items.Add(new { FromIP = remoteHost, Message = msg });
                });
        }

        /// <summary>
        /// 在此頁將要在 Frame 中顯示時進行呼叫。
        /// </summary>
        /// <param name="e">描述如何存取此頁的事件資料。
        /// 此參數通常用於組態頁。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 顯示伺服器的IP
            var hosts = NetworkInformation.GetHostNames();
            HostName svName = hosts.FirstOrDefault(h => h.Type == HostNameType.Ipv4 && h.IPInformation.NetworkAdapter.IanaInterfaceType == 71);
            if (svName != null)
            {
                tbIp.Text = svName.DisplayName;
            }
        }

        private async void OnSend(object sender, RoutedEventArgs e)
        {
            if (txtServer.Text.Length == 0 || txtMessage.Text.Length == 0) return;

            Button b = sender as Button;
            b.IsEnabled = false;
            // 取得輸出流
            IOutputStream outStream = await clientSocket.GetOutputStreamAsync(new HostName(txtServer.Text), SERVICE_PORT);
            using (DataWriter writer = new DataWriter(outStream))
            {
                // 寫入訊息
                writer.WriteString(txtMessage.Text);
                // 傳送寫入的內容
                await writer.StoreAsync();
            }
            b.IsEnabled = true;
            txtMessage.Text = "";
        }
    }
}
