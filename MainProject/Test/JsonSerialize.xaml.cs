using JsonHelper;
using LLBT_SignLib;
using MainProject.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TLSLib;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace MainProject.Test
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class JsonSerialize : Page
    {
        public JsonSerialize()
        {
            this.InitializeComponent();
            //BindDevice();
            //InitData();
            GetUserInfo();
            Sign();
        }

        private async void BindDevice()
        {            
            BindDeviceRequest sParams = new BindDeviceRequest();
            sParams.deviceId = Configuration.GetDeviceUniqueId();
            sParams.userName = "zou.penghui";

            HttpSerializeData<BindDeviceRequest> httpSerializeData = new HttpSerializeData<BindDeviceRequest>("bindDevice.html", sParams);
            string url = httpSerializeData.GetURL();
            NetworkRequest networkRequest = NetworkRequest.CreateHttp(httpSerializeData.GetURL());
            var result = await networkRequest.PostAsync<string>(JsonAnalysis.Serialize(httpSerializeData));            
        }

        private async void GetUserInfo()
        {
            GetUserInforRequestParams sParams = new GetUserInforRequestParams();
            sParams.deviceId = Configuration.GetDeviceUniqueId();
            HttpSerializeData<GetUserInforRequestParams> httpSerializeData = new HttpSerializeData<GetUserInforRequestParams>("getUserInfo.html", sParams);
            NetworkRequest networkRequest = NetworkRequest.CreateHttp(httpSerializeData.GetURL());
            var resultJson = await networkRequest.PostAsync<string>(JsonAnalysis.Serialize(httpSerializeData));       
            HttpDeserializeData<GetUserInforResponseData> sData = JsonAnalysis.Deserialize<HttpDeserializeData<GetUserInforResponseData>>(resultJson);

            if (sData.result.error == "01")   // 未绑定账户
            {
                //todo:跳转到绑定页面，进行用户绑定
                
            }
            else if (sData.status.code.Equals("0000"))
            {
                //todo:跳转到签到页面
            }
            else
            {

            }
        }

        private async void InitData()
        {
            //DoSignRequestParams sParams = new DoSignRequestParams();
            //sParams.deviceId = "111111";
            //sParams.queryOny = "false";
            //HttpSerializeData<DoSignRequestParams> httpSerializeData = new HttpSerializeData<DoSignRequestParams>("doSign.html", sParams);
            //httpSerializeData.GetURL();//

            //NetworkRequest networkRequest = NetworkRequest.CreatHttp(httpSerializeData.GetURL());
            //networkRequest.Body= JsonAnalysis.Serialize(httpSerializeData);
            //var result = await networkRequest.PostAsync<string>();
            string str = "{\"result\": {\"signInTime\": \"2016/10/13\",\"userName\": \"zouph\"}}";

            //result_tbk.Text = JsonAnalysis.Serialize(httpSerializeData);//
            ////var obj = JsonAnalysis.Deserialize<DoSignRequestParams>(result_tbk.Text);


            HttpDeserializeData<DoSignResponseData> sData = JsonAnalysis.Deserialize<HttpDeserializeData<DoSignResponseData>>(str);
        }

        private async void Sign()
        {
            await DoSign("true");
        }


        /// <summary>
        /// 签到，查询
        /// <param name="queryOnly">true:查询 false:签到</param>
        /// </summary>
        /// <returns></returns>
        public async Task<DoSignResponseData> DoSign(string queryOnly)
        {         
            DoSignRequestParams sParams = new DoSignRequestParams();
            sParams.deviceId = Configuration.GetDeviceUniqueId();
            sParams.queryOny = queryOnly;
            HttpSerializeData<DoSignRequestParams> httpSerializeData = new HttpSerializeData<DoSignRequestParams>("doSign.html", sParams);
            NetworkRequest networkRequest = NetworkRequest.CreateHttp(httpSerializeData.GetURL());
            var resultJson = await networkRequest.PostAsync<string>(JsonAnalysis.Serialize(httpSerializeData));
            HttpDeserializeData<DoSignResponseData> sData = JsonAnalysis.Deserialize<HttpDeserializeData<DoSignResponseData>>(resultJson);
            if (queryOnly.Equals("false"))
            {
            }
            if (sData.result.error == "00")// 签到成功
            {
                
                return sData.result;
            }
            else if (sData.status.code.Equals("0000"))
            {
                return sData.result;
            }
            else
            {
                return null;
            }

        }
    }
}
