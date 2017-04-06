using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace TLSLib
{
    public static class HMac
    {

        #region   Static Methods    

        public static string HmacSha1(byte[] secretKey, byte[] data)
        {
            string str_alg_name = MacAlgorithmNames.HmacSha1;
            MacAlgorithmProvider obj_mac_prov = MacAlgorithmProvider.OpenAlgorithm(str_alg_name);
            IBuffer buff_msg = CryptographicBuffer.CreateFromByteArray(data);
            IBuffer buff_key_material = CryptographicBuffer.CreateFromByteArray(secretKey);
            CryptographicKey hmac_key = obj_mac_prov.CreateKey(buff_key_material);
            IBuffer hmac = CryptographicEngine.Sign(hmac_key, buff_msg);
            return CryptographicBuffer.EncodeToBase64String(hmac);
        }

        public static string HmacMd5(byte[] secretKey, byte[] data)
        {
            string str_alg_name = MacAlgorithmNames.HmacMd5;
            MacAlgorithmProvider obj_mac_prov = MacAlgorithmProvider.OpenAlgorithm(str_alg_name);
            IBuffer buff_msg = CryptographicBuffer.CreateFromByteArray(data);
            IBuffer buff_key_material = CryptographicBuffer.CreateFromByteArray(secretKey);
            CryptographicKey hmac_key = obj_mac_prov.CreateKey(buff_key_material);
            IBuffer hmac = CryptographicEngine.Sign(hmac_key, buff_msg);
            return CryptographicBuffer.EncodeToBase64String(hmac);
        }

        #endregion
    }
}
