
using io.nebulas.model;
using io.nebulas.schema;
using System;
using System.Collections;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace io.nebulas.api
{
    public class SmartContracts
    {
        private const string url = "https://pay.nebulas.io/api/";
        private const string mainnetUrl = "https://pay.nebulas.io/api/mainnet/pay/";
        private const string testnetUrl = "https://pay.nebulas.io/api/pay/";
        private const string baseUrl = "https://mainnet.nebulas.io/";
        private const string baseTestnetUrl = "https://testnet.nebulas.io/";
        public static bool isMainnet = true;

        public static string GetUrl(string path)
        {
            string url = isMainnet ? baseUrl : baseTestnetUrl;
            url += path;
            return url;
        }
        /**
         * pay接口：       星云地址之间的转账
         * @param goods   商品详情
         * @param to      转账目标地址
         * @param value   转账value，单位为wei (1NAS =10^18 wei)
         */
        public static void Pay(GoodsModel goods, String to, String value, String serialNumber)
        {

            OpenAppMode openAppMode = new OpenAppMode();
            openAppMode.category = Constants.CATEGORY;
            openAppMode.des = Constants.DESCRIPTION;

            PageParamsModel pageParamsModel = new PageParamsModel();
            pageParamsModel.serialNumber = serialNumber;
            pageParamsModel.callback = isMainnet ? mainnetUrl : testnetUrl;
            pageParamsModel.goods = goods;

            PayloadModel payloadModel = new PayloadModel();
            payloadModel.type = Constants.PAY_PAYLOAD_TYPE;

            PayModel payModel = new PayModel();
            payModel.currency = Constants.PAY_CURRENCY;
            payModel.payload = payloadModel;
            payModel.value = value;
            payModel.to = to;

            pageParamsModel.pay = payModel;

            openAppMode.pageParams = pageParamsModel;

            String _params = OpenAppMode.getOpenAppModel(openAppMode);

            String url = OpenAppSchema.getSchemaUrl(_params);

            ContractAction.start(url);
        }

        /**
         * call函数：      调用智能合约
         * @param goods   商品详情（*）
         * @param to      转账目标地址
         * @param value   转账value，单位为wei (1NAS =10^18 wei)
         * @param args    函数参数列表
         */
        public static void Call(GoodsModel goods, String functionName, String to, String value, String[] args, String serialNumber)
        {

            OpenAppMode openAppMode = new OpenAppMode();
            openAppMode.category = Constants.CATEGORY;
            openAppMode.des = Constants.DESCRIPTION;

            PageParamsModel pageParamsModel = new PageParamsModel();
            pageParamsModel.serialNumber = serialNumber;
            pageParamsModel.callback = isMainnet ? mainnetUrl : testnetUrl;
            pageParamsModel.goods = goods;

            PayloadModel payloadModel = new PayloadModel();
            payloadModel.type = Constants.CALL_PAYLOAD_TYPE;
            payloadModel.function = functionName;
            payloadModel.args = args;

            PayModel payModel = new PayModel();
            payModel.currency = Constants.PAY_CURRENCY;
            payModel.payload = payloadModel;
            payModel.value = value;
            payModel.to = to;

            pageParamsModel.pay = payModel;

            openAppMode.pageParams = pageParamsModel;

            String _params = OpenAppMode.getOpenAppModel(openAppMode);

            String url = OpenAppSchema.getSchemaUrl(_params);

            ContractAction.start(url);
        }
        /**
         * 查询交易状态
         * @param mainNet 0 测试网    1 主网
         * @param serialNumber
         */
        public static IEnumerator QueryTransferStatus(String serialNumber, Action<string> onSuccess, Action<string> onFail)
        {
            string ENDPOINT = isMainnet ? mainnetUrl : testnetUrl;
            ENDPOINT += "query?payId=" + serialNumber;
            UnityEngine.Debug.Log(ENDPOINT);
            var request = UnityWebRequest.Get(ENDPOINT);
            yield return request.SendWebRequest();
            if (request.isNetworkError)
            {
                if (onFail != null)
                    onFail.Invoke(request.error);
            }
            else
            {
                if (onSuccess != null)
                    onSuccess.Invoke(request.downloadHandler.text);
            }
        }


        public static IEnumerator GetTransactionReceipt(String serialNumber, Action<string> onSuccess, Action<string> onFail)
        {

            String ENDPOINT = GetUrl("/v1/user/getTransactionReceipt");
            JObject jObject = new JObject();
            jObject.Add("hash", serialNumber);
            string jsonString = jObject.ToString();
            yield return HttpPost(ENDPOINT, jsonString, onSuccess, onFail);
        }

        public static IEnumerator SimulationCall(string from, string to, string function, string[] args, Action<string> onSuccess, Action<string> onFail)
        {
            String ENDPOINT = GetUrl("/v1/user/call");
            var jParams = new JObject();
            var jContract = new JObject();
            jParams.Add("from", from);
            jParams.Add("to", to);
            jParams.Add("value", "0");
            jParams.Add("nonce", 3);
            jParams.Add("gasLimit", "2000000");
            jParams.Add("gasPrice", "1000000");
            jContract.Add("function", function);
            if (args != null)
                jContract.Add("args", JsonConvert.SerializeObject(args));
            else
                jContract.Add("args", "");
            jParams.Add("contract", jContract);
            var jsonString = jParams.ToString();
            UnityEngine.Debug.Log(jsonString);
            yield return HttpPost(ENDPOINT, jsonString, onSuccess, onFail);

        }
        public static IEnumerator HttpPost(string ENDPOINT, string jsonString, Action<string> onSuccess, Action<string> onFail)
        {
            byte[] postBytes = System.Text.Encoding.Default.GetBytes(jsonString);
            var request = new UnityWebRequest(ENDPOINT, "POST");
            request.uploadHandler = new UploadHandlerRaw(postBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            if (request.isNetworkError)
            {
                if (onFail != null)
                    onFail.Invoke(request.error);
            }
            else
            {
                if (onSuccess != null)
                    onSuccess.Invoke(request.downloadHandler.text);
            }
        }
    }
}
