# nebulas-unity-sdk
nebulas  sdk to unity3d

## Interface

#### Call():
    
   
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

#### Pay

>    public static void Pay(GoodsModel goods, String to, String value, String serialNumber)


#### QueryTransferStatus    

>    public static IEnumerator QueryTransferStatus(String serialNumber, Action<string> onSuccess, Action<string> onFail)
  
#### SimulationCall
>    public static IEnumerator SimulationCall(string from, string to, string function, string[] args, Action<string> onSuccess, Action<string> onFail)

## Examples

>    Nebulas>Examples>Simple

## Thanks
#### This project is under test. If you have any questions or suggestions, please correct the fork code and submit it. Thank you for your support and contribution.
#### Project Implementation Reference to Android SDK Project:https://github.com/nebulasio/androidSDK
