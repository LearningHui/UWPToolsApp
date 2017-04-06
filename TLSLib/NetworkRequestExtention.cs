using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace TLSLib
{
    /// <summary>
    /// 用于扩展HttpRequest的方法
    /// </summary>
    public static class NetworkRequestExtention
    {
        /// <summary>
        /// 同步Get请求 返回指定的类型 此方法会阻塞调用线程
        /// 支持的返回类型：string,BitmapImage,byte[]
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T Get<T>(this NetworkRequest source) where T:class
        {
            byte[] bytes = source.Get();               
            return TypeConvertAsync<T>(bytes).Result;      
        }

        /// <summary>
        /// 异步get请求，返回指定的类型 
        /// 支持的返回类型：string,BitmapImage,byte[]
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(this NetworkRequest source) where T:class
        {
            byte[] bytes = await source.GetAsync();            
            return await TypeConvertAsync<T>(bytes);
        }

        /// <summary>
        /// 同步post请求，返回指定的类型 此方法会阻塞调用线程
        /// 支持的返回类型：string,BitmapImage,byte[]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="body">网络请求报文体</param>
        /// <returns></returns>
        public static T Post<T>(this NetworkRequest source, string body = null) where T:class
        {
            byte[] bytes = source.Post(body);            
            return TypeConvertAsync<T>(bytes).Result;
        }

        /// <summary>
        /// 异步post请求，返回指定的类型 
        /// 支持的返回类型：string,BitmapImage,byte[]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="body">网络请求报文体</param>
        /// <returns></returns>
        public static async Task<T> PostAsync<T>(this NetworkRequest source, string body = null) where T : class
        {
            byte[] bytes = await source.PostAsync(body);
            return await TypeConvertAsync<T>(bytes);
        }

        /// <summary>
        /// 将网络请求返回的byte[]字节数组类型转换为指定的类型
        /// 目前可转换类型如下：string,BitmapImage
        /// 后续可根据需要添加转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static async Task<T> TypeConvertAsync<T>(byte[] bytes) where T : class
        {
            if (bytes == null)
                return default(T);
            if (typeof(T).Equals(typeof(string)))
            {
                return Encoding.UTF8.GetString(bytes) as T;
            }
            else if (typeof(T).Equals(typeof(BitmapImage)))
            {
                return await ToolsLib.Utility.GetBitmapImageAsync(bytes) as T;
            }
            else if (typeof(T).Equals(typeof(byte[])))
            {
                return bytes as T;
            }
            else
            {
                return default(T);
            }
        }
    }
}
