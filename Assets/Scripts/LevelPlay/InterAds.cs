using System;
using com.unity3d.mediation;
using DG.Tweening;
using UnityEngine;

public class InterAds : MonoBehaviour, BaseAds
{
    private IronSourceADUnitIdConfig ironSourceConfig;
    private LevelPlayInterstitialAd interstitialAd;
    public int _currentReloadAds;
    public Action<bool, string> _onDoneAds;

    public LevelPlayAdFormat levelPlayAdFormat => LevelPlayAdFormat.INTERSTITIAL;
    public bool CanShowAds()
    {
        return interstitialAd.IsAdReady();
    }

    public void LoadAds()
    {
        if (!interstitialAd.IsAdReady())
        {
            Debug.Log("ironsource: start load ads");
            interstitialAd.LoadAd();
        }
    }

    public void HideAds()
    {
        
    }

    public void ShowAds(Action<bool, string> onRewardedAds)
    {
        _onDoneAds = onRewardedAds;
        if (CanShowAds())
        {
            Debug.Log("ironsource: start show ads");
            interstitialAd.ShowAd();
        }
        else
        {
            Debug.Log("ironsource: show but ads not available");
            LoadAds();
        }
    }

    public void Initialize(IronSourceADUnitIdConfig cfg)
    {
        ironSourceConfig = cfg;
        InitEvent();
    }

    private void InitEvent()
    {
        // Create Interstitial object
        interstitialAd = new LevelPlayInterstitialAd(ironSourceConfig.ADUnitId);
        interstitialAd.OnAdLoaded += InterstitialOnAdLoadedEvent;
        interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
        interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
        interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
        interstitialAd.OnAdClicked += InterstitialOnAdClickedEvent;
        interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
        interstitialAd.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;
    }
    #region AdInfo Interstitial

    void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got InterstitialOnAdLoadedEvent With AdInfo " + adInfo);
        Signals.Get<LoadAdsSignal>().Dispatch(LevelPlayAdFormat.INTERSTITIAL, true);
    }

    void InterstitialOnAdLoadFailedEvent(LevelPlayAdError error)
    {
        ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
        {
            ReloadAds();
            Debug.Log("unity-script: I got InterstitialOnAdLoadFailedEvent With Error " + error);
        });
    }
	
    void InterstitialOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got InterstitialOnAdDisplayedEvent With AdInfo " + adInfo);
    }
	
    void InterstitialOnAdDisplayFailedEvent(LevelPlayAdDisplayInfoError infoError)
    {
        Debug.Log("unity-script: I got InterstitialOnAdDisplayFailedEvent With InfoError " + infoError);
    }
	
    void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got InterstitialOnAdClickedEvent With AdInfo " + adInfo);
    }

    void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got InterstitialOnAdClosedEvent With AdInfo " + adInfo);
        ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
        {
            _onDoneAds?.Invoke(true, "");
        });
    }
	
    void InterstitialOnAdInfoChangedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("unity-script: I got InterstitialOnAdInfoChangedEvent With AdInfo " + adInfo);
    }

    #endregion
    private void ReloadAds()
    {
        _currentReloadAds++;
        if (_currentReloadAds < ironSourceConfig.maxCountReload)
        {
            DOVirtual.DelayedCall(ironSourceConfig.timeReloadAds, LoadAds);
        }
        else
        {
            Debug.Log("ironsource: try to retry load ads but ads not available");
            Signals.Get<LoadAdsSignal>().Dispatch(LevelPlayAdFormat.INTERSTITIAL, false);
        }
    }
}