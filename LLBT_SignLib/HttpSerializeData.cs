using LLBT_SignLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LLBT_SignLib
{
    #region Header
    [DataContract]
    public class Header
    {
        [DataMember(Name = "locale")]
        public string locale = "ZH_CN";
    }
    #endregion

    [DataContract]
    public class HttpSerializeData<T>
    {
        [DataMember(Name = "header")]
        public Header Header = new Header();

        [DataMember(Name = "method")]
        public string Method { get; set; }

        [DataMember(Name = "request")]
        public HttpSerializeRequestData<T> request;

        public HttpSerializeData(string _method, T obj)
        {
            Method = _method;
            request = new HttpSerializeRequestData<T>();
            request.setParams(obj);
        }

        public string GetURL()
        {
            return Configuration.NetIp + Method;
        }
    }

    [DataContract]
    public class HttpSerializeRequestData<T>
    {
        [DataMember(Name = "params")]
        public T _x0070_arams;
        public void setParams(T obj)
        {
            _x0070_arams = obj;
        }
    }



    [DataContract]
    public class HttpReturnStatus
    {
        [DataMember(Name = "code")]
        public string code { get; set; }
        [DataMember(Name = "expMsg")]
        public string expMsg { get; set; }
        [DataMember(Name = "arguments")]
        public string arguments { get; set; }
        [DataMember(Name = "msg")]
        public string msg { get; set; }
    }

    [DataContract]
    public class HttpDeserializeData<T>
    {
        [DataMember(Name = "status")]
        public HttpReturnStatus status { get; set; }

        [DataMember(Name = "result")]
        public T result { get; set; }
        public T getDeserializeData()
        {
            return result;
        }
    }

}
