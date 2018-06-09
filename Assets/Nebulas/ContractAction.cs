using System;
using UnityEngine;

public class ContractAction{
    /**
     * Schema 方式启动星云钱包
     * @param context 上下文
     * @param url     schema
     */
    public static void start(String url)
    {
        if (String.IsNullOrEmpty(url))
        {
            return;
        }
        Debug.Log(url);
        Application.OpenURL(url);
    }

}
