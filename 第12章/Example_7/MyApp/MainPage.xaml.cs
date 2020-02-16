using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;

namespace MyApp
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

        private async void OnPickFile(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            // 要開啟的檔案型態
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                string info = string.Empty;
                info += string.Format("檔名：{0}\n", file.Name);
                info += string.Format("檔案路徑：{0}\n", file.Path);
                info += string.Format("建立時間：{0:yyyy-M-d HH:mm:ss}\n", file.DateCreated.DateTime);
                ImageProperties imgprop = await file.Properties.GetImagePropertiesAsync();
                info += string.Format("相機廠商：{0}\n", imgprop.CameraManufacturer);
                info += string.Format("相機型號：{0}\n", imgprop.CameraModel);
                info += string.Format("寬度：{0}\n", imgprop.Width);
                info += string.Format("高度：{0}", imgprop.Height);

                // 顯示檔案訊息
                tbFileinfo.Text = info;
            }
        }

        private async void OnFileSave(object sender, RoutedEventArgs e)
        {
            FileSavePicker picker = new FileSavePicker();
            // 指定檔案型態
            picker.FileTypeChoices.Add("自訂檔案", new string[] { ".data" });
            // 預設定位到文件庫中
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            // 預設檔名
            picker.SuggestedFileName = "test.data";
            // 顯示選取器
            StorageFile file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                // 隨機產生200個位元組
                byte[] data = new byte[200];
                Random rand = new Random();
                rand.NextBytes(data);
                // 將位元組陣列寫入檔案
                await FileIO.WriteBytesAsync(file, data);
            }
        }

        private async void OnOpenFolder(object sender, RoutedEventArgs e)
        {
            FolderPicker picker = new FolderPicker();
            picker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            // 指定檔案型態
            picker.FileTypeFilter.Add("*");
            // 顯示選取器
            StorageFolder folder = await picker.PickSingleFolderAsync();
            if (folder != null)
            {
                // 取得目錄下的檔案清單
                var subFiles = await folder.GetFilesAsync();
                // 向ListView中加入檔案
                this.lvFiles.Items.Clear();
                foreach (StorageFile file in subFiles)
                {
                    // 取得檔案圖示
                    StorageItemThumbnail thumb = await file.GetScaledImageAsThumbnailAsync(ThumbnailMode.ListView);
                    BitmapImage bmp = new BitmapImage();
                    bmp.DecodePixelWidth = 40;
                    bmp.SetSource(thumb);
                    this.lvFiles.Items.Add(new { Image = bmp, Name = file.DisplayName });
                }
            }
        }
    }
}
