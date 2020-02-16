using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.Data.Json;

namespace FileActiveApp
{
    /// <summary>
    /// 提供特定於套用程式的行為，以補充預設的套用程式類別。
    /// </summary>
    public sealed partial class App : Application
    {
        private TransitionCollection transitions;

        /// <summary>
        /// 起始化單一案例套用程式物件。    這是執行的創作程式碼的第一行，
        /// 邏輯上等同於 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        /// <summary>
        /// 在套用程式由最終使用者標準啟動時進行呼叫。
        /// 當啟動套用程式以開啟特定的檔案或顯示搜尋結果等動作時，
        /// 將使用其他入口點。
        /// </summary>
        /// <param name="e">有關啟動請求和過程的詳細訊息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在視窗已包括內容時重復套用程式起始化，
            // 只需確保視窗處於活動狀態
            if (rootFrame == null)
            {
                // 建立要充當導覽上下文的框架，並導覽到第一頁
                rootFrame = new Frame();

                // TODO: 將此值變更為適合您的套用程式的快取大小
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: 從之前暫停的套用程式載入狀態
                }

                // 將框架放在目前視窗中
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // 移除用於啟動的旋轉門導覽。
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                // 當導覽堆堆疊尚未復原時，導覽到第一頁，
                // 並透過將所需訊息作為導覽參數傳入來組態
                // 新頁面
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // 確保目前視窗處於活動狀態
            Window.Current.Activate();
        }

        /// <summary>
        /// 啟動套用程式後復原內容轉換。
        /// </summary>
        /// <param name="sender">附加了處理程式的物件。</param>
        /// <param name="e">有關導覽事件的詳細訊息。</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// 在將要暫停套用程式執行時呼叫。    在不知道套用程式
        /// 將被終止還是還原的情況下儲存套用程式狀態，
        /// 並讓記憶體內容保持不變。
        /// </summary>
        /// <param name="sender">暫停的請求的源。</param>
        /// <param name="e">有關暫停的請求的詳細訊息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: 儲存套用程式狀態並停止任何背景活動
            deferral.Complete();
        }

        protected async override void OnFileActivated(FileActivatedEventArgs args)
        {
            // 參考傳入的檔案
            StorageFile file = args.Files[0] as StorageFile;
            if (file != null)
            {
                // 讀出JSON資料
                string jsstr = await FileIO.ReadTextAsync(file);
                // 從字串產生JSON物件案例
                JsonObject jsobj = null;
                if (JsonObject.TryParse(jsstr,out jsobj))
                {
                    Frame rootframe = Window.Current.Content as Frame;
                    // 若果用於導覽的Frame為null，則案例化一個
                    if (rootframe == null)
                    {
                        rootframe = new Frame();
                        Window.Current.Content = rootframe;
                    }
                    // 清理頁面歷史堆堆疊
                    rootframe.BackStack.Clear();
                    // 導覽到顯示內容的頁面
                    rootframe.Navigate(typeof(DisplayPage), jsobj);
                }
            }
            Window.Current.Activate();
        }
    }
}