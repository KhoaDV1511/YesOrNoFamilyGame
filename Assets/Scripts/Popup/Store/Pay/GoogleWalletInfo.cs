using Newtonsoft.Json;

public class GoogleWalletInfo : BasePayWalletInfo
{
    public string productId;
    public string signature;
    public string orderId;
    public string packageName;
    public long purchaseTime;
    public int purchaseState;
    public string purchaseToken;
    public int payPercentId;

    public GoogleWalletInfo(int uid, string productId,
    string signature, string orderId, string packageName, long purchaseTime, int purchaseState,
    string purchaseToken, int payPercentId = 0, int typeForeignCurrency = 0) {
        this.uid = uid;
        this.productId = productId;
        this.signature = signature;
        this.orderId = orderId;
        this.packageName = packageName;
        this.purchaseTime = purchaseTime;
        this.purchaseState = purchaseState;
        this.purchaseToken = purchaseToken;
        this.payPercentId = payPercentId;
        this.typeForeignCurrency = typeForeignCurrency;
    }

    public GoogleWalletInfo()
    {
        
    }
}