using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace JsonHelper
{
    public class JsonAnalysis
    {
        /// <summary>
        /// 将一个object类型的数据结构序列化为json格式的字符串
        /// </summary>
        /// <param name="objectToSerialize">需要格式化的数据结构</param>
        /// <returns>一个json格式的字符串</returns>
        public static string Serialize(object objectToSerialize)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(objectToSerialize.GetType());
                    serializer.WriteObject(ms, objectToSerialize);
                    ms.Position = 0;

                    using (StreamReader reader = new StreamReader(ms))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("JsonHelper Serialize method error!");
                return null;
            }
        }

        /// <summary>
        /// 将一个json格式的字符串反序列化成一个指定类型的数据结构。
        /// 指定类型的数据结构必须与json格式的字符串精确的对应（即属性名与json字段名一致），
        /// 否则将无法取到对应的字段值
        /// </summary>
        /// <typeparam name="T">需要反序列的数据类型</typeparam>
        /// <param name="jsonString">需要反序列化的json字符串</param>
        /// <returns>指定的与json字符串对应的数据实例</returns>
        public static T Deserialize<T>(string jsonString)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                    return (T)serializer.ReadObject(ms);
                }
            }
            catch(Exception ex)
            {                
                return default(T);
            }
        }
    }
}
