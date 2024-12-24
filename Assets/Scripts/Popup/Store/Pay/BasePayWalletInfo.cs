using Newtonsoft.Json;

public class BasePayWalletInfo
{
    public int uid;
    public int typeForeignCurrency;

    public void UpdateTypeCurrency(int type)
    {
        typeForeignCurrency = type;
    }
    
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}
