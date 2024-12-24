using Newtonsoft.Json;
using System;

[Serializable]
public class AppleWalletInfo : BasePayWalletInfo
{
    public string receipt;

    public AppleWalletInfo(string receipt)
    {
        this.receipt = receipt;
    }
}