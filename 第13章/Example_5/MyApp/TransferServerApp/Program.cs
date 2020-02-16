using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.ServiceModel.Description;

namespace TransferServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri localUri = new Uri("http://192.168.1.100:98");
            // 開始執行WCF服務
            using (WebServiceHost host = new WebServiceHost(typeof(Service), localUri))
            {
                // 組態緩沖區的最大值
                WebHttpBinding binding = new WebHttpBinding();
                binding.MaxReceivedMessageSize = 500 * 1024 * 1024;
                host.AddServiceEndpoint(typeof(IService), binding, "");

                host.Opened += (a, b) => Console.WriteLine("服務已啟動。\n上傳位址：" + localUri + "upload");
                host.Closed += (a, b) => Console.WriteLine("服務已關閉。");
                try
                {
                    // 開啟服務
                    host.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadKey();
            }
        }
    }

    [ServiceContract]
    public interface IService
    {
        [OperationContract, WebInvoke(UriTemplate = "/upload")]
        void UploadFile(Stream content);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class Service : IService
    {
        public void UploadFile(Stream content)
        {
            string fileName = "";
            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            // 從標頭取得檔名
            fileName = request.Headers["file_name"];
            if (string.IsNullOrEmpty(fileName))
            {
                Guid g = Guid.NewGuid();
                fileName = g.ToString();
            }
            // 開始接收檔案
            try
            {
                // 取得使用者文件庫位置
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string newFilePath = Path.Combine(docPath, fileName);
                // 若果檔案存在，將其移除
                if (File.Exists(newFilePath))
                {
                    File.Delete(newFilePath);
                }
                using (FileStream fileStream = File.OpenWrite(newFilePath))
                {
                    // 從用戶端上傳的流中將資料複製到檔案流中
                    content.CopyTo(fileStream);
                }
                Console.WriteLine(string.Format("在{0}成功接收檔案。", DateTime.Now.ToLongTimeString()));
                // 向用戶端傳送回應訊息
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                // 處理錯誤
                Console.WriteLine(ex.Message);
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                WebOperationContext.Current.OutgoingResponse.StatusDescription = ex.Message;
            }
        }
    }

}
