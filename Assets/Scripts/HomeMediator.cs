using System;
using com.unity3d.mediation;
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
    private bool _adsAvailable, _adsAvailableInter;
    private GamePlayModle _gamePlayModle = GamePlayModle.Instance;

    private void Start()
    {
        btnTabPlay.onClick.AddListener(TabPlay);
        btnAds.onClick.AddListener(ShowAds);
        btnSetting.onClick.AddListener(() =>
        {
            PopupManager.OpenPopup<SettingPopup>();
        });
    }

    private void OnEnable()
    {
        UpdateHome();
        Signals.Get<UpDateHomeSignals>().AddListener(UpdateHome);
        _showAdsSignal.AddListener(ResultShowAds);
    }

    private void OnDisable()
    {
        Signals.Get<UpDateHomeSignals>().RemoveListener(UpdateHome);
        _showAdsSignal.RemoveListener(ResultShowAds);
    }

    [Button]
    private void ShowToast()
    {
        Toast.Show("hom nay la ngayf toet voi");
    }
    private void ShowAds()
    {
        _adsAvailable = AdsManager.Instance.CanShowAds(LevelPlayAdFormat.REWARDED);
        Debug.Log($"is ads reward available: {_adsAvailable}");
        if (_adsAvailable)
        {
            _adsAvailable = false;
            AdsManager.Instance.ShowAds(LevelPlayAdFormat.REWARDED, (b, placement) =>
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
        Debug.Log($"is ads Inter available: {AdsManager.Instance.CanShowAds(LevelPlayAdFormat.INTERSTITIAL)}");
        AdsManager.Instance.ShowAds(LevelPlayAdFormat.INTERSTITIAL, (b, placement) =>
        {
            if (b)
            {
                Debug.Log("show reward");
            }
        });
    }

    private void ShowBanner()
    {
        AdsManager.Instance.ShowAds(LevelPlayAdFormat.BANNER);
    }
    
    private void HideBanner()
    {
        AdsManager.Instance.HideBanner();
    }
    
    private void ResultShowAds(LevelPlayAdFormat levelPlayAdFormat, bool isSuccess)
    {
        if (isSuccess)
        {
            switch (levelPlayAdFormat)
            {
                case LevelPlayAdFormat.REWARDED:
                    _adsAvailable = true;
                    break;
                case LevelPlayAdFormat.INTERSTITIAL:
                    _adsAvailableInter = true;
                    break;
            }
        }
        else
        {
            OnRetryLoadAdsFail(levelPlayAdFormat);
        }
    }


    private void OnRetryLoadAdsFail(LevelPlayAdFormat levelPlayAdFormat)
    {
        switch (levelPlayAdFormat)
        {
            case LevelPlayAdFormat.REWARDED:
                _adsAvailable = false;
                break;
            case LevelPlayAdFormat.INTERSTITIAL:
                _adsAvailableInter = false;
                break;
        }

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