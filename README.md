# nebulas-unity-sdk
nebulas  sdk to unity3d

## 调用接口

#### 调用接口 call():
    
   
>    public static void Call(GoodsModel goods, String functionName, String to, String value, String[] args, String serialNumber)

```
        public void CallTransfer()
        {
            var gmol = new GoodsModel();
            var serialNumber = Util.getRandomCode(32);
            SmartContracts.Call(gmol, "vote", inputQueryTransfer.text, (Util.OnNAS * 0.000001).ToString("F0"), new string[] { "霸王别姬" }, serialNumber);
            StartCoroutine(TransferStatusCoroutine(serialNumber));
        }

        public IEnumerator TransferStatusCoroutine(string serialNumber)
        {
            yield return new WaitForSeconds(1);
            yield return SmartContracts.QueryTransferStatus(serialNumber, (success) =>
            {
                Debug.Log(success);
                TransferStatusHandler.Invoke(success);
            }, (fail) =>
            {
                Debug.LogError(fail);
                TransferStatusErrorHandler.Invoke(fail);
            });
        }
```

#### 调用接口 pay

>    public static void Pay(GoodsModel goods, String to, String value, String serialNumber)


#### 调用接口 queryTransferStatus() :    

>    public static IEnumerator QueryTransferStatus(String serialNumber, Action<string> onSuccess, Action<string> onFail)
  

## Examples

>    Nebulas>Examples>Simple

