using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPotato : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtQuantity, txtPriceMoney, txtPriceAds;
    [SerializeField] private Button btnBuy;
    private readonly ShowAdsRewardSignal _showAdsRewardSignal = Signals.Get<ShowAdsRewardSignal>();
    private ProductInfo _productInfo;
    private Action<ProductInfo> _buy;

    private void Start()
    {
        btnBuy.onClick.AddListener(OnClickBuy);
    }
    
    private void OnEnable()
    {
        _showAdsRewardSignal.AddListener(ResultShowAds);
    }

    private void OnDisable()
    {
        _showAdsRewardSignal.RemoveListener(ResultShowAds);
    }
    
    private void ResultShowAds(bool isSuccess)
    {
        if(_productInfo.typePrice != TypePrice.Ads) return;
        if (isSuccess)
        {
            btnBuy.interactable = true;
        }
        else
        {
            OnRetryLoadAdsFail();
        }
    }


    private void OnRetryLoadAdsFail()
    {
        btnBuy.interactable = true;
        if(!gameObject.activeSelf) return;

        Debug.Log("Quảng cáo không khả dụng");
    }

    public void ShowView(ProductInfo productInfo, Action<ProductInfo> buy, int timeWatchAds)
    {
        _buy = buy;
        _productInfo = productInfo;
        gameObject.Show();
        if(_productInfo.typePrice == TypePrice.Ads)
        {
        }
        
        txtPriceAds.gameObject.SetActive(productInfo.typePrice == TypePrice.Ads);
        txtPriceMoney.gameObject.SetActive(productInfo.typePrice == TypePrice.Money);
        txtQuantity.SetText($"x{productInfo.quantity}");
        txtPriceAds.SetText($"{timeWatchAds}/{productInfo.quantity}");
    }

    private void OnClickBuy()
    {
        btnBuy.interactable = _productInfo.typePrice != TypePrice.Ads;
        _buy?.Invoke(_productInfo);
    }
}