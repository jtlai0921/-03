using System;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace BgServiceTasks
{
    public sealed class ServiceTask : IBackgroundTask
    {
        BackgroundTaskDeferral taskdef = null;
        string serviceName = null;
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            taskdef = taskInstance.GetDeferral();
            taskInstance.Canceled += OnCancel;

            // 取得App Service連線關聯的物件
            AppServiceTriggerDetails details = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            if (details != null)
            {
                // 取得服務名
                serviceName = details.Name;
                // 取得連線物件
                AppServiceConnection connection = details.AppServiceConnection;
                // 處理關聯事件
                connection.RequestReceived += Connection_RequestReceived;
            }
        }

        private void OnCancel(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            taskdef.Complete();
        }

        private async void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var msgdef = args.GetDeferral();
            // 取得參數
            if (args.Request.Message.ContainsKey("num1") == false || args.Request.Message.ContainsKey("num2") == false)
            {
                msgdef.Complete();
                taskdef.Complete();
                return;
            }

            int a = Convert.ToInt32(args.Request.Message["num1"]);
            int b = Convert.ToInt32(args.Request.Message["num2"]);

            int result = default(int);
            // 判斷計算型態
            switch (serviceName)
            {
                case "Add": //加法
                    result = a + b;
                    break;
                case "Sub": //減法
                    result = a - b;
                    break;
                case "Mul": //乘法
                    result = a * b;
                    break;
                case "Div": //除法
                    result = a / b;
                    break;
                default:
                    result = 0;
                    break;
            }
            // 將計算結果發回給用戶端
            ValueSet msg = new ValueSet();
            msg.Add("res", result);
            await args.Request.SendResponseAsync(msg);
            msgdef.Complete();
            taskdef.Complete();
        }
    }
}
