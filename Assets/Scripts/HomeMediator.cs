using System;

using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HomeMediator : MonoBehaviour
{
    [SerializeField] private Button btnTabPlay, btnAds, btnCoin, btnSetting;
    [SerializeField] private TextMeshProUGUI txtLevel, txtMoney;
    [SerializeField] private GameObject bg;
    private readonly ShowAdsSignal _showAdsSignal = Signals.Get<ShowAdsSignal>();
    private bool _adsAvailable;
    private GamePlayModle _gamePlayModle = GamePlayModle.Instance;

    private void Start()
    {
        btnTabPlay.onClick.AddListener(TabPlay);
        btnAds.onClick.AddListener(ShowAds);
        btnSetting.onClick.AddListener(() =>
        {
            PopupManager.OpenPopup<SettingPopup>();
        });
        btnCoin.onClick.AddListener(() =>
        {
            PopupManager.OpenPopup<ShopPopup>();
        });
    }

    private void OnEnable()
    {
        UpdateHome();
        Signals.Get<UpDateHomeSignals>().AddListener(UpdateHome);
        Signals.Get<UpdateCoinSignal>().AddListener(UpdateCoin);
        _showAdsSignal.AddListener(ResultShowAds);
    }

    private void OnDisable()
    {
        Signals.Get<UpDateHomeSignals>().RemoveListener(UpdateHome);
        Signals.Get<UpdateCoinSignal>().RemoveListener(UpdateCoin);
        _showAdsSignal.RemoveListener(ResultShowAds);
    }

    [Button]
    private void ShowToast()
    {
        Toast.Show("hom nay la ngayf toet voi");
    }
    [Button]
    private void ShowUnlockReward()
    {
        PopupManager.OpenPopup<RewardUnlock>(p =>
        {
            p.ShowView();
        });
    }

    private void UpdateCoin()
    {
        txtMoney.SetText(_gamePlayModle.Coin.ToString());
    }
    private void ShowAds()
    {
        _adsAvailable = AdsManager.Instance.CanShowAds(TypeAds.REWARDED);
        Debug.Log($"is ads reward available: {_adsAvailable}");
        if (_adsAvailable)
        {
            _adsAvailable = false;
            AdsManager.Instance.ShowAds(TypeAds.REWARDED, (b, placement) =>
            {
                if (b)
                {
                    Debug.Log("show reward");
                }
            });
        }
        else
        {
            Debug.Log("Quang cao khong co san hoac khong phai level xem ads");
        }
    }
    private void ShowAdsInter()
    {
        Debug.Log($"is ads Inter available: {AdsManager.Instance.CanShowAds(TypeAds.INTERSTITIAL)}");
        AdsManager.Instance.ShowAds(TypeAds.INTERSTITIAL, (b, placement) =>
        {
            if (b)
            {
                Debug.Log("show reward");
            }
        });
    }

    private void ShowBanner()
    {
        AdsManager.Instance.ShowAds(TypeAds.BANNER);
    }
    
    private void HideBanner()
    {
        AdsManager.Instance.HideBanner();
    }
    
    private void ResultShowAds(bool isSuccess)
    {
        if (isSuccess)
        {
            _adsAvailable = true;
        }
        else
        {
            OnRetryLoadAdsFail();
        }
    }


    private void OnRetryLoadAdsFail()
    {
        _adsAvailable = false;

        if(!gameObject.activeSelf) return;

        Debug.Log("Quảng cáo không khả dụng");
    }
    private void UpdateHome()
    {
        bg.Show();
        var level = _gamePlayModle.IsMaxLevel() ? Random.Range(1, _gamePlayModle.Level) : _gamePlayModle.Level;
        _gamePlayModle.currentLevel = level;
        txtLevel.SetText($"Level: {level}");
        txtMoney.SetText(_gamePlayModle.Coin.ToString());
        
        ShowBanner();
    }
    private void TabPlay()
    {
        Signals.Get<StartGameSignals>().Dispatch();
        bg.Hide();
    }
}