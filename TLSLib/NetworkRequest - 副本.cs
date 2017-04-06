using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace TLSLib
{
    public class NetworkRequest
    {
        private HttpWebRequest request;

        /// <summary>
        /// 构造函数私有化，不允许通过new关键字实例化
        /// </summary>
        private NetworkRequest(string requestUriString)
        {
            request = WebRequest.CreateHttp(requestUriString);
        }

        #region Property

        /// <summary>
        /// 获取或设置请求的方法。默认值为 GET       
        /// </summary>
        public string Method
        {
            get { return request.Method; }
            set { request.Method = value; }
        }

        /// <summary>
        /// 获取请求的原始统一资源标识符 (URI)。
        /// </summary>
        public Uri RequestUri
        {
            get { return request.RequestUri; }
        }

        /// <summary>
        /// Post网络请求报文体
        /// </summary>
        public string Body { get; set; }

        #endregion

        /// <summary>
        /// 创建HttpRequest实例
        /// </summary>
        /// <param name="requestUriString"></param>
        /// <returns></returns>
        public static NetworkRequest CreatHttp(string requestUriString)
        {
            return new NetworkRequest(requestUriString);
        }

        #region 使用Task封装网络请求，包括get请求的同步和异步方式和post请求的同步和异步方式 

        /// <summary>
        /// 同步Get请求 返回byte[]字节数组
        /// </summary>
        /// <returns></returns>
        public byte[] Get()
        {
            Method = "GET";
            Task<byte[]> task = DoHttpRequestTask();
            byte[] bytes = task.Result;
            return bytes;
        }       

        /// <summary>
        /// 异步Get请求
        /// </summary>
        /// <returns></returns>
        public Task<byte[]> GetAsync()
        {
            Method = "GET";
            Task<byte[]> task = DoHttpRequestTask();
            return task;
        }

        /// <summary>
        /// 同步Post请求
        /// </summary>
        /// <param name="body">网络请求报文体</param>
        /// <returns></returns>
        public byte[] Post(string body = null)
        {
            if (body != null)
                Body = body;
            Method = "POST";
            Task<byte[]> task = DoHttpRequestTask();
            byte[] bytes = task.Result;
            return bytes;
        }

        /// <summary>
        /// 异步Post请求
        /// </summary>
        /// <param name="body">网络请求报文体</param>
        /// <returns></returns>
        public Task<byte[]> PostAsync(string body = null)
        {
            if (body != null)
                Body = body;
            Method = "POST";
            Task<byte[]> task = DoHttpRequestTask();
            return task;
        }

        private Task<byte[]> DoHttpRequestTask()
        {
            byte[] resultBytes = null;
            TaskCompletionSource<byte[]> task = new TaskCompletionSource<byte[]>();
            AsyncCallback getResponseCallBack = (ar) =>
            {
                try
                {
                    var re = (HttpWebRequest)ar.AsyncState;
                    HttpWebResponse response = (HttpWebResponse)re.EndGetResponse(ar);
                    //状态码200
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        resultBytes = GetByteArray(response);
                    }
                    else
                    {
                        //未获取到请求结果
                        System.Diagnostics.Debug.WriteLine("can't get response data:/r/n" + RequestUri.AbsolutePath);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(RequestUri.AbsolutePath + "/r/nweb request error!/r/n" + e.Message);
                }
                finally
                {
                    //设置TaskResult通知调用线程已获取到请求结果
                    task.SetResult(resultBytes);
                }
            };
            AsyncCallback getRequestCallBack = (ar) =>
            {
                try
                {
                    var re = (WebRequest)ar.AsyncState;

                    //TODO:设置request header
                    // Just here
                    //----------------------

                    //TODO:设置要发送的网络请求报文体
                    if (!string.IsNullOrEmpty(Body))
                    {
                        var bytes = Encoding.UTF8.GetBytes(Body);
                        using (var stream = request.EndGetRequestStream(ar))
                        {
                            stream.Write(bytes, 0, bytes.Length);
                        }
                    }
                    //------------------------------

                    re.BeginGetResponse(getResponseCallBack, re);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(RequestUri.AbsolutePath + "/r/nGetRequestStream error:/r/n" + e.Message);
                }
            };
            if (Method.ToUpper().Equals("POST"))
            {
                request.BeginGetRequestStream(getRequestCallBack, request);
            }
            else if (Method.ToUpper().Equals("GET"))
            {
                request.BeginGetResponse(getResponseCallBack, request);
            }
            return task.Task;
            
        }

        #endregion        

        /// <summary>
        /// 获取网络返回数据，读入内存流
        /// </summary>
        /// <param name="responseStream">服务器返回流</param>
        /// <param name="len">返回内容长度</param>
        /// <param name="buffer">内存流</param>
        private static void ReadByByte(Stream responseStream, long len, MemoryStream buffer)
        {
            const int bufferSize = 512;
            var byteInput = new byte[bufferSize];
            int size;
            if (len != -1)
                while (len > 0)
                {
                    if ((size = responseStream.Read(byteInput, 0, (int)Math.Min(len, bufferSize))) == 0)
                        break;

                    len -= size;
                    buffer.Write(byteInput, 0, size);
                }
            else
                while ((size = responseStream.Read(byteInput, 0, bufferSize)) != 0)
                {
                    buffer.Write(byteInput, 0, size);
                }
        }        

        /// <summary>
        /// 从WebResponse获取网络数据流，并将流数据写入字节数组Byte[]
        /// </summary>
        /// <param name="response">Http网络请求返回对象</param>
        /// <returns></returns>
        private static byte[] GetByteArray(WebResponse response)
        {
            byte[] resultBytes = null;
            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    //var encoding = response.Headers["Content-Encoding"];
                    long len = response.ContentLength;
                    var buffer = new MemoryStream(1024);
                    //获取网络返回数据，读入内存流
                    ReadByByte(stream, len, buffer);
                    //转换为字节数组
                    resultBytes = buffer.ToArray();

                    #region 解密逻辑处理
                    //TODO:在这里进行 byte[] 数据解密处理
                    //处理逻辑：

                    //---------------------------
                    #endregion

                    buffer.Dispose();
                }
            }
            return resultBytes;
        }



        #region 仿Jqery网络请求 get和post网络请求方式，使用回调。

        /// <summary>
        /// 发送 Http get请求并使用回调函数监听请求结果 返回byte[]类型
        /// </summary>
        /// <param name="url">网络请求地址</param>
        /// <param name="callBack">请求结束时的回调函数</param>
        public static void Get(string url, Action<byte[], HttpStatusCode> callBack)
        {
            Get(url, null, callBack);
        }

        /// <summary>
        /// 发送 Http get请求并使用回调函数监听请求结果 返回string类型
        /// </summary>
        /// <param name="url">网络请求地址</param>
        /// <param name="callBack">请求结束时的回调函数</param>
        public static void Get(string url, Action<string, HttpStatusCode> callBack)
        {
            Get(url, null, callBack);
        }

        /// <summary>
        /// 发送 Http get请求并使用回调函数监听请求结果 返回byte[]类型
        /// </summary>
        /// <param name="url">网络请求地址</param>
        /// <param name="data">请求参数</param>
        /// <param name="callBack">请求结束时的回调函数</param>
        public static void Get(string url, List<KeyValuePair<string, string>> data, Action<byte[], HttpStatusCode> callBack)
        {
            //拼接请求参数
            if (data != null)
            {
                string para = "";//url请求参数
                foreach (var item in data)
                {
                    para += item.Key + "=" + item.Value + "&"; //稍后优化此逻辑            
                }
                if (!string.IsNullOrEmpty(para))
                {
                    url += "?" + para;
                }
            }
            AsyncCallback getResponseCallBack = (ar) =>
            {
                var re = (HttpWebRequest)ar.AsyncState;
                HttpWebResponse response = (HttpWebResponse)re.EndGetResponse(ar);
                byte[] resultBytes = null;
                try
                {
                    //状态码200
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        resultBytes = GetByteArray(response);
                    }
                    else
                    {
                        //未获取到请求结果
                        System.Diagnostics.Debug.WriteLine("can't get response data:/r/n" + re.RequestUri.AbsolutePath);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(re.RequestUri.AbsolutePath + "/r/nweb request error!/r/n" + e.Message);
                }
                finally
                {
                    if (callBack != null)
                        callBack(resultBytes, response.StatusCode);
                }
            };
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.BeginGetResponse(getResponseCallBack, request);
        }

        /// <summary>
        /// 发送 Http get请求并使用回调函数监听请求结果 返回string类型
        /// </summary>
        /// <param name="url">网络请求地址</param>
        /// <param name="data">请求参数</param>
        /// <param name="callBack">请求结束时的回调函数</param>
        public static void Get(string url, List<KeyValuePair<string, string>> data, Action<string, HttpStatusCode> callBack)
        {
            //拼接请求参数
            if (data != null)
            {
                string para = "";//url请求参数
                foreach (var item in data)
                {
                    para += item.Key + "=" + item.Value + "&"; //稍后优化此逻辑            
                }
                if (!string.IsNullOrEmpty(para))
                {
                    url += "?" + para;
                }
            }
            AsyncCallback getResponseCallBack = (ar) =>
            {
                var re = (HttpWebRequest)ar.AsyncState;
                HttpWebResponse response = (HttpWebResponse)re.EndGetResponse(ar);
                byte[] resultBytes = null;
                try
                {
                    //状态码200
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        resultBytes = GetByteArray(response);
                    }
                    else
                    {
                        //未获取到请求结果
                        System.Diagnostics.Debug.WriteLine("can't get response data:/r/n" + re.RequestUri.AbsolutePath);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(re.RequestUri.AbsolutePath + "/r/nweb request error!/r/n" + e.Message);
                }
                finally
                {
                    if (callBack != null)
                    {
                        if (resultBytes != null)
                        {
                            string content = Encoding.UTF8.GetString(resultBytes, 0, resultBytes.Length);
                            callBack(content, response.StatusCode);
                        }
                        else
                        {
                            callBack(null, response.StatusCode);
                        }
                    }
                }
            };
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.BeginGetResponse(getResponseCallBack, request);
        }

        /// <summary>
        /// 发送 Http post请求并使用回调函数监听请求结果 返回byte[]类型
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="callBack"></param>
        public static void Post(string url, string postData, Action<byte[], HttpStatusCode> callBack)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = "POST";
            byte[] resultBytes = null;

            AsyncCallback getResponseCallBack = (ar) =>
            {
                var re = (HttpWebRequest)ar.AsyncState;
                HttpWebResponse response = (HttpWebResponse)re.EndGetResponse(ar);
                try
                {
                    //状态码200
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        resultBytes = GetByteArray(response);
                    }
                    else
                    {
                        //未获取到请求结果
                        System.Diagnostics.Debug.WriteLine("can't get response data:/r/n" + request.RequestUri.AbsolutePath);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(request.RequestUri.AbsolutePath + "/r/nweb request error!/r/n" + e.Message);
                }
                finally
                {
                    if (callBack != null)
                        callBack(resultBytes, response.StatusCode);
                }
            };

            AsyncCallback getRequestCallBack = (ar) =>
            {
                try
                {
                    var re = (WebRequest)ar.AsyncState;

                    //TODO:设置request header
                    // Just here
                    //----------------------

                    //TODO:设置要发送的网络请求报文体
                    if (!string.IsNullOrEmpty(postData))
                    {
                        var bytes = Encoding.UTF8.GetBytes(postData);
                        using (var stream = request.EndGetRequestStream(ar))
                        {
                            stream.Write(bytes, 0, bytes.Length);
                        }
                    }
                    //------------------------------

                    re.BeginGetResponse(getResponseCallBack, re);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(request.RequestUri.AbsolutePath + "/r/nGetRequestStream error:/r/n" + e.Message);
                }
            };

            request.BeginGetRequestStream(getRequestCallBack, request);
        }

        /// <summary>
        /// 发送 Http post请求并使用回调函数监听请求结果 返回string类型
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="callBack"></param>
        public static void Post(string url, string postData, Action<string, HttpStatusCode> callBack)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = "POST";
            byte[] resultBytes = null;

            AsyncCallback getResponseCallBack = (ar) =>
            {
                var re = (HttpWebRequest)ar.AsyncState;
                HttpWebResponse response = (HttpWebResponse)re.EndGetResponse(ar);
                try
                {
                    //状态码200
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        resultBytes = GetByteArray(response);
                    }
                    else
                    {
                        //未获取到请求结果
                        System.Diagnostics.Debug.WriteLine("can't get response data:/r/n" + request.RequestUri.AbsolutePath);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(request.RequestUri.AbsolutePath + "/r/nweb request error!/r/n" + e.Message);
                }
                finally
                {
                    if (callBack != null)
                    {
                        if (resultBytes != null)
                        {
                            string content = Encoding.UTF8.GetString(resultBytes, 0, resultBytes.Length);
                            callBack(content, response.StatusCode);
                        }
                        else
                        {
                            callBack(null, response.StatusCode);
                        }
                    }
                }
            };

            AsyncCallback getRequestCallBack = (ar) =>
            {
                try
                {
                    var re = (WebRequest)ar.AsyncState;

                    //TODO:设置request header
                    // Just here
                    //----------------------

                    //TODO:设置要发送的网络请求报文体
                    if (!string.IsNullOrEmpty(postData))
                    {
                        var bytes = Encoding.UTF8.GetBytes(postData);
                        using (var stream = request.EndGetRequestStream(ar))
                        {
                            stream.Write(bytes, 0, bytes.Length);
                        }
                    }
                    //------------------------------

                    re.BeginGetResponse(getResponseCallBack, re);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(request.RequestUri.AbsolutePath + "/r/nGetRequestStream error:/r/n" + e.Message);
                }
            };

            request.BeginGetRequestStream(getRequestCallBack, request);
        }

        #endregion
    }
}
