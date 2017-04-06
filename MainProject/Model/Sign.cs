using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Model
{

    [DataContract]
    public class Params
    {
        [DataMember(Name = "clientType")]
        public string clientType = "WP";
    }

    [DataContract]
    public class ResponseData
    {
        [DataMember(Name = "sysTime")]
        public string sysTime { get; set; }
        [DataMember(Name = "error")]
        public string error { get; set; }
        [DataMember(Name = "errorMsg")]
        public string errorMsg { get; set; }

    }


    public class DoSignRequestParams : Params
    {
        public string deviceId { get; set; }
        public string queryOny { get; set; }

    }

    [DataContract]
    public class DoSignResponseData : ResponseData
    {
        [DataMember(Name = "signInTime")]
        public string signInTime { get; set; }
        [DataMember(Name = "signOutTime")]
        public string signOutTime { get; set; }
        [DataMember(Name = "userName")]
        public string userName { get; set; }
        [DataMember(Name = "realName")]
        public string realName { get; set; }

    }

}
