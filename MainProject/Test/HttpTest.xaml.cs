using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using TLSLib;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace MainProject.Test
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HttpTest : Page
    {
        public HttpTest()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            byte[] key = Encoding.UTF8.GetBytes("1");
            byte[] data = Encoding.UTF8.GetBytes("o");
            string hmacSha1 = TLSLib.HMac.HmacSha1(key, data);
            string hmacMd5 = TLSLib.HMac.HmacMd5(key, data);
            string md5str = MD5.GetMd5String("123");
            string md5str2 = ToolsLib.MD5.Creat().ComputeHash(Encoding.UTF8.GetBytes("123"));
        }

        public Task<string> m()
        {
            TaskCompletionSource<string> task = new TaskCompletionSource<string>();
            Task.Run(async () =>
            {
                string str = null;
                NetworkRequest request = NetworkRequest.CreateHttp("http://baike.baidu.com/cms/global/lemma_config.json");
                str = await request.GetAsync<string>();
                task.SetResult(str);
            });            
            return task.Task;
        }

        private async void GetHttpRequest_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://baike.baidu.com/cms/global/lemma_config.json";
            byte[] bytes = null;
            string str = null; 
            
            
            NetworkRequest request = NetworkRequest.CreateHttp("http://baike.baidu.com/cms/global/lemma_config.json?cl=2&rn=20&tn=news");
            bytes = request.Get();
            
            request = NetworkRequest.CreateHttp("http://p5.sinaimg.cn/2776321060/180/73181353913818");
            bytes = await request.GetAsync();
            str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            this.img.Source = await ToolsLib.Utility.GetBitmapImageAsync(bytes);
            
            request = NetworkRequest.CreateHttp(url);
            request.Body = "name=123";
            bytes = request.Post();

            request = NetworkRequest.CreateHttp(url);
            bytes = await request.PostAsync();
            str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            this.result_tbk.Text = str;
        }

        private void GetHttpRequest_Sync_Click(object sender, RoutedEventArgs e)
        {

            //var dd = m().Result;


            //byte[] bytes = null;
            //string str = null;
            //HttpRequest request = HttpRequest.CreatHttp("http://baike.baidu.com/cms/global/lemma_config.json");
            //bytes = request.Get();
            //str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            //this.result_tbk.Text = str;


            NetworkRequest request = NetworkRequest.CreateHttp("http://baike.baidu.com/cms/global/lemma_config.json");
            this.result_tbk.Text = request.Get<string>();

        }

        private async void GetHttpRequest_Async_Click(object sender, RoutedEventArgs e)
        {
            //byte[] bytes = null;
            //string str = null;
            //HttpRequest request = HttpRequest.CreatHttp("http://p5.sinaimg.cn/2776321060/180/73181353913818");
            //bytes = await request.GetAsync();
            //str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            //this.img.Source = await ToolsLib.Utility.GetBitmapImageAsync(bytes);           

            NetworkRequest request = NetworkRequest.CreateHttp("http://p5.sinaimg.cn/2776321060/180/73181353913818");
            //this.img.Source = await request.GetBitmapImageAsync();
            this.img.Source = await request.GetAsync<BitmapImage>();
            
        }

        private async void PostHttpRequest_Sync_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://baike.baidu.com/cms/global/lemma_config.json";
            byte[] bytes = null;
            string str = null;
            NetworkRequest request = NetworkRequest.CreateHttp(url);
            request.Body = "name=123";
            bytes = request.Post();

            request = NetworkRequest.CreateHttp(url);
            bytes = await request.PostAsync();
            str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            this.result_tbk.Text = str;
        }

        private async void PostHttpRequest_Async_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://baike.baidu.com/cms/global/lemma_config.json";
            byte[] bytes = null;
            string str = null;            
            NetworkRequest request = NetworkRequest.CreateHttp(url);
            bytes = await request.PostAsync();
            str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            this.result_tbk.Text = str;
        }

        private void GetHttpCallback_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://baike.baidu.com/cms/global/lemma_config.json";
            //Action<byte[], HttpStatusCode> callBack = (bytes, httpStatusCode) =>
            //   {
            //       string res = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            //       var m = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //       {
            //           var x = new MessageDialog("请求状态码：" + httpStatusCode.ToString()).ShowAsync();
            //           this.result_tbk.Text = res;
            //       });
            //   };
            //HttpRequest.Get(url, null, callBack);

            Action<string, HttpStatusCode> callBack = (content, httpStatusCode) =>
            {
                var m = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    var x = new MessageDialog("请求状态码：" + httpStatusCode.ToString()).ShowAsync();
                    this.result_tbk.Text = content;
                });
            };
            NetworkRequest.Get(url, null, callBack);            
        }

        private void PostHttpCallback_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://baike.baidu.com/cms/global/lemma_config.json";
            Action<byte[], HttpStatusCode> callBack = (bytes, httpStatusCode) =>
            {
                string res = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                var m = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    var x = new MessageDialog("请求状态码：" + httpStatusCode.ToString()).ShowAsync();
                    this.result_tbk.Text = res;
                });
            };
            NetworkRequest.Post(url, "name=123", callBack);
        }
    }
}
