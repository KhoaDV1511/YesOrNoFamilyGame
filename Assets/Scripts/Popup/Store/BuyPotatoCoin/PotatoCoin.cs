using System;
using System.Collections.Generic;
using UnityEngine;

public class PotatoCoin : MonoBehaviour
{
    [SerializeField] private ItemPotato item;
    [SerializeField] private Transform content;
    //private readonly GameModel _gameModel = GameModel.Instance;
    private int _timeWatchAds;
    private List<ProductInfo> ProductInfos => GlobalDataManager.Ins.storeData.productInfos;

    public void ShowPotatoStore()
    {
        if (content.childCount <= 1)
        {
            foreach (var p in ProductInfos)
            {
                var objItem = Instantiate(item, content);
                objItem.ShowView(p, OnBuy, _timeWatchAds);
            }
        }
    }

    private void OnBuy(ProductInfo productInfo)
    {
        switch (productInfo.typePrice)
        {
            case TypePrice.Ads:
                break;
            case TypePrice.Money:
                NewMyIAPManager.Instance.BuyProductID(productInfo.id);
                break;
        }
    }
}