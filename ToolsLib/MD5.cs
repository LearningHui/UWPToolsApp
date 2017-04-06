using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace ToolsLib
{
    public class MD5
    {
        private CryptographicHash objHash = null;      
         
        public static MD5 Creat()
        {
            return new MD5();
        } 
        //私有化构造函数，禁止通过new创建实例
        private MD5()
        {
            // Create a string that contains the name of the hashing algorithm to use.
            String strAlgName = HashAlgorithmNames.Md5;

            // Create a HashAlgorithmProvider object.
            HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(strAlgName);

            // Create a CryptographicHash object. This object can be reused to continually
            // hash new messages.
            objHash = objAlgProv.CreateHash();
        }
        public string ComputeHash(byte[] data)
        {
            if (objHash == null)
                return null;
            IBuffer buffMsg1 = CryptographicBuffer.CreateFromByteArray(data);
            objHash.Append(buffMsg1);
            IBuffer buffHash1 = objHash.GetValueAndReset();
            return CryptographicBuffer.EncodeToHexString(buffHash1);
        }
    }
}
