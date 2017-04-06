using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Model
{
    [DataContract]
    public class BindDeviceRequest : Params
    {
        [DataMember(Name = "deviceId")]
        public string deviceId { get; set; }
        [DataMember(Name = "userName")]
        public string userName { get; set; }
    }

    [DataContract]
    public class BindDeviceResponseData : ResponseData
    {
        [DataMember(Name = "userName")]
        public string userName { get; set; }
        [DataMember(Name = "realName")]
        public string realName { get; set; }

        public static implicit operator GetUserInforResponseData(BindDeviceResponseData obj)
        {
            GetUserInforResponseData data = new GetUserInforResponseData();
            data.userName = obj.userName;
            data.realName = obj.realName;
            return data;
        }
    }
}
