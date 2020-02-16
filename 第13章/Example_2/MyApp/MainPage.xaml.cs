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
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;
using Windows.UI.Core;

// “空白頁”項範本在 http://go.microsoft.com/fwlink/?LinkId=391641 上有介紹

namespace MyApp
{
    /// <summary>
    /// 可用於自己或導覽至 Frame 內定的空白頁。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// 伺服器監聽通訊埠
        /// </summary>
        const string LISTEN_PORT = "2100";
        /// <summary>
        /// 監聽元件
        /// </summary>
        StreamSocketListener listener = null;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.Loaded += async (s, e) =>
                {
                    // 案例化StreamSocketListener物件
                    listener = new StreamSocketListener();
                    // 加入ConnectionReceived事件處理程式
                    listener.ConnectionReceived += listener_ConnectionReceived;
                    // 綁定本機通訊埠
                    await listener.BindServiceNameAsync(LISTEN_PORT);
                };
            this.Unloaded += (s, e) =>
                {
                    // 釋放資源
                    if (listener != null)
                    {
                        listener.Dispose();
                        listener = null;
                    }
                };
        }

        async void listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            string text = string.Empty;
            IRandomAccessStream imgStream = new InMemoryRandomAccessStream();
            // 處理接收到的連線
            using (StreamSocket socket = args.Socket)
            {
                using (DataReader reader = new DataReader(socket.InputStream))
                {
                    try
                    {
                        // 讀出第一個整數，表示檔案長度
                        await reader.LoadAsync(sizeof(uint));
                        uint len = reader.ReadUInt32();
                        await reader.LoadAsync(len);
                        IBuffer buffer = reader.ReadBuffer(len);
                        // 寫入記憶體流
                        await imgStream.WriteAsync(buffer);
                        await reader.LoadAsync(sizeof(uint));
                        // 讀入字串的長度
                        len = reader.ReadUInt32();
                        // 讀出字串內容
                        if (len > 0)
                        {
                            await reader.LoadAsync(len);
                            text = reader.ReadString(len);
                        }
                    }
                    catch (Exception ex)
                    {
                        DisplayMessage(ex.Message);
                    }
                }
            }

            // 顯示接收到的內容
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    BitmapImage bmp = new BitmapImage();
                    bmp.DecodePixelWidth = 50;
                    imgStream.Seek(0UL);
                    bmp.SetSource(imgStream);
                    imgStream.Dispose();
                    lbItems.Items.Add(new { Image = bmp, Text = text });
                });
        }

        private async void DisplayMessage(string msg)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    Windows.UI.Popups.MessageDialog d = new Windows.UI.Popups.MessageDialog(msg);
                    await d.ShowAsync();
                });
        }

        /// <summary>
        /// 在此頁將要在 Frame 中顯示時進行呼叫。
        /// </summary>
        /// <param name="e">描述如何存取此頁的事件資料。
        /// 此參數通常用於組態頁。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 取得目前主電腦的IP位址
            var hosts = NetworkInformation.GetHostNames();
            // 篩選出IPv4版本的位址
            HostName server = hosts.FirstOrDefault(h => h.Type == HostNameType.Ipv4 && h.IPInformation.NetworkAdapter.IanaInterfaceType == 71);
            // 顯示IP位址
            tbSvIP.Text = server.DisplayName;
        }

        private async void OnPickImagFile(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                using (IRandomAccessStream stream = await file.OpenReadAsync())
                {
                    BitmapImage bmp = new BitmapImage();
                    bmp.DecodePixelHeight = 200;
                    bmp.SetSource(stream);
                    this.img.Source = bmp; //顯示圖形
                    using (DataReader reader = new DataReader(stream.GetInputStreamAt(0UL)))
                    {
                        uint len = (uint)stream.Size;
                        // 載入資料
                        await reader.LoadAsync(len);
                        IBuffer buffer = reader.ReadBuffer(len);
                        // 暫存在Tag屬性中，稍後用到
                        this.img.Tag = buffer;
                    }
                }
            }
        }

        private async void OnSend(object sender, RoutedEventArgs e)
        {
            if (txtServerIp.Text.Length == 0)
            {
                DisplayMessage("請輸入伺服器IP。");
                return;
            }
            IBuffer bufferImg = img.Tag as IBuffer;
            if (bufferImg == null)
            {
                DisplayMessage("請選取圖形。");
                return;
            }
            Button b = sender as Button;
            b.IsEnabled = false;

            using (StreamSocket socket = new StreamSocket())
            {
                try
                {
                    // 發起連線
                    await socket.ConnectAsync(new HostName(txtServerIp.Text), LISTEN_PORT);
                    // 準備傳送資料
                    using (DataWriter writer = new DataWriter(socket.OutputStream))
                    {
                        // 首先寫入圖形資料
                        uint len = bufferImg.Length;
                        writer.WriteUInt32(len); //長度
                        writer.WriteBuffer(bufferImg);
                        // 接著寫入文字
                        if (txtContent.Text.Length == 0)
                        {
                            writer.WriteUInt32(0);
                        }
                        else
                        {
                            len = writer.MeasureString(txtContent.Text);
                            writer.WriteUInt32(len); //長度
                            writer.WriteString(txtContent.Text);
                        }
                        // 傳送資料到流
                        await writer.StoreAsync();
                    }
                    txtContent.Text = "";
                }
                catch (Exception ex)
                {
                    DisplayMessage(ex.Message);
                }
            }
            b.IsEnabled = true;
        }
    }
}
