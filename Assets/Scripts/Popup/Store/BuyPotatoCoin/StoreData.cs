using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

[CreateAssetMenu(menuName = "Data/StoreData", fileName = "StoreData")]
public class StoreData : ScriptableObject
{
    public List<ProductInfo> productInfos;

    public void OnPayWalletSuccess(string id)
    {
        var p = productInfos.Find(p => p.id == id);
    }
}

[Serializable]
public class ProductInfo
{
    public string id;
    public int quantity;
    public TypePrice typePrice;
    public int price;
    public ProductType productType = ProductType.Consumable;
}

public enum TypePrice
{
    Ads,
    Money
}