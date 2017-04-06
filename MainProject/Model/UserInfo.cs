using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Model
{
    [DataContract]
    public class GetUserInforRequestParams : Params
    {
        [DataMember(Name = "deviceId")]
        public string deviceId { get; set; }
    }

    [DataContract]
    public class GetUserInforResponseData : ResponseData
    {
        [DataMember(Name = "userName")]
        public string userName { get; set; }
        [DataMember(Name = "realName")]
        public string realName { get; set; }
    }
}
