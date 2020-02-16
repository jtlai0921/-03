using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using System.Text.RegularExpressions;


namespace MyApp
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

                rootFrame.Language = "zh-CN";

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: 從之前暫停的套用程式載入狀態
                }

                // 將框架放在目前視窗中
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {

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

        protected override void OnActivated(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Protocol)
            {
                ProtocolActivatedEventArgs protoarg = (ProtocolActivatedEventArgs)args;
                // 分析URI
                Uri actUri = protoarg.Uri;
                // 取得查詢字串
                string q = actUri.Query;
                // 去掉查詢字串前面的“?”
                q = q.Replace("?", "");
                // 透過正規表示法來查詢URI參數中的key-value對
                Regex r = new Regex(@"((?<key>[^&=]+)=(?<value>[^&=]+))+");
                MatchCollection matches = r.Matches(q);
                IDictionary<string, string> dic = new Dictionary<string, string>();
                // 將識別出來的各群組key-value對存入字典物件中
                foreach (Match m in matches)
                {
                    string _key = m.Groups["key"].Value;
                    string _value = m.Groups["value"].Value;
                    dic.Add(_key, _value);
                }
                // 導覽到顯示彩色效果的頁面
                Frame rootFrame = Window.Current.Content as Frame;
                if (rootFrame == null)
                {
                    rootFrame = new Frame();
                    Window.Current.Content = rootFrame;
                }
                else
                {
                    rootFrame.BackStack.Clear();
                }
                // 導覽並傳遞參數
                rootFrame.Navigate(typeof(ShowColorPage), dic);
            }

            Window.Current.Activate();
        }
    }
}