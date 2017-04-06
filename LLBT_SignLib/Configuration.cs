using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace LLBT_SignLib
{
    public class Configuration
    {
        /// <summary>
        /// 默认广源地址
        /// </summary>
        public const string DefaultNetIp = "http://22.18.61.188:9194/sign-manager/";
        /// <summary>
        /// 网络地址
        /// </summary>
        public static string NetIp = "http://22.18.61.188:9194/sign-manager/";

        public static string GetDeviceUniqueId()
        {
            return "0421460710001350";

            //EasClientDeviceInformation deviceInfo = new EasClientDeviceInformation();
            //byte[] bytes = deviceInfo.Id.ToByteArray();
            //string strTemp = "";
            //string strDeviceUniqueID = "";
            //foreach (byte b in bytes)
            //{
            //    strTemp = b.ToString();
            //    if (1 == strTemp.Length)
            //    {
            //        strTemp = "00" + strTemp;
            //    }
            //    else if (2 == strTemp.Length)
            //    {
            //        strTemp = "0" + strTemp;
            //    }
            //    strDeviceUniqueID += strTemp;
            //}
            //return strDeviceUniqueID.Substring(0, 16);
        }
    }
}
