using io.nebulas.api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using io.nebulas.model;
using io.nebulas.utils;

public class Simple : MonoBehaviour
{
    public Toggle toggleMainnet;
    public InputField inputQueryTransfer;
    public Button btnQueryTransfer;

    public Button btnCallTransfer;
    public StringEvent TransferStatusHandler;
    public StringEvent TransferStatusErrorHandler;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        SmartContracts.isMainnet = toggleMainnet.isOn;
    }

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

    public void SimulationCall()
    {
        this.StartCoroutine(
            SmartContracts.SimulationCall("n1NrqHkmuFAHsifysfBh6gombgeg6wJrfnB", "n1j2Q5E9SU1JnpqbyQLVRM8D2jPeefDXKau", "info", null, (success) =>
            {
                Debug.Log(success);
                TransferStatusHandler.Invoke(success);
            }, (fail) =>
            {
                Debug.LogError(fail);
                TransferStatusErrorHandler.Invoke(fail);
            })
        );

    }
    public void QueryTransferStatus()
    {
        this.StartCoroutine(
        SmartContracts.QueryTransferStatus(inputQueryTransfer.text, (success) =>
        {
            Debug.Log(success);
        }, (fail) =>
        {
            Debug.LogError(fail);
        }));
    }
    public void GetTransactionReceipt()
    {
        this.StartCoroutine(
        SmartContracts.GetTransactionReceipt(inputQueryTransfer.text, (success) =>
        {
            Debug.Log(success);
        }, (fail) =>
        {
            Debug.LogError(fail);
        }));
    }
}

[System.Serializable]
public class StringEvent : UnityEngine.Events.UnityEvent<string>
{

}
