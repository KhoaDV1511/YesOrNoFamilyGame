using System;
using DG.Tweening;
using UnityEngine;

public class RewardAds : MonoBehaviour, BaseAds
{
    private IronSourceADUnitIdConfig ironSourceConfig;

    public Action<bool, string> _onRewardedAds;
    public int _currentReloadAds;
    public bool _isOnLoadAds = false;

    public void Initialize(IronSourceADUnitIdConfig cfg)
    {
        ironSourceConfig = cfg;
        Debug.Log("Ironsource InitEvent");
        InitEvent(); 
    }

    private void InitEvent()
    {
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;
        IronSourceRewardedVideoEvents.onAdLoadFailedEvent += RewardVideoOnLoadFailedEvent;

        Debug.Log("Ironsource InitEvent Done");
    }

    #region Ads Event

        private void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
        ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
        {
            Debug.Log("ironsource: ads opened");
        });
    }

    private void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
        {
            Debug.Log("ironsource: ads closed");
        });
    }

    private void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
        ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
        {
            _currentReloadAds = 0;
            _isOnLoadAds = false;
            Signals.Get<LoadAdsSignal>().Dispatch(TypeAds.REWARDED, true);
            Debug.Log("ironsource: ads available");
        });
    }

    private void RewardedVideoOnAdUnavailable()
    {
        ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
        {
            _isOnLoadAds = false;
            ReloadAds();
            Debug.Log($"ironsource: ads unavailable");
        });
    }

    private void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {
        ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
        {
            _isOnLoadAds = false;
            ReloadAds();
            Debug.Log($"ironsource: show ads failed: {error.getErrorCode()} - {error.getDescription()}");
        });
    }

    private void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        Debug.Log("ironsource: rewared video show complete");
        ExecuteOnMainThread.RunOnMainThread.Enqueue(() =>
        {
            Debug.Log("ironsource: rewared video show complete not receive");
            _onRewardedAds?.Invoke(true, placement.getPlacementName());
        });
        Debug.Log("ironsource: rewared video show complete receive");
    }

    private void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        Debug.Log("ironsource: on placement clicked");
    }

    private void RewardVideoOnLoadFailedEvent(IronSourceError error)
    {
        Debug.Log("ironsource: load ads failed");
        ExecuteOnMainThread.RunOnMainThread.Enqueue(ReloadAds);
    }

    private void SdkInitializationCompletedEvent()
    {
        Debug.Log("ironsource: init sdk complete");
    }

    #endregion

    public TypeAds levelPlayAdFormat => TypeAds.REWARDED;

    public bool CanShowAds()
    {
        return IronSource.Agent.isRewardedVideoAvailable();
    }

    public void LoadAds()
    {
        if (_isOnLoadAds)
        {
            return;
        }
        
        if (!IronSource.Agent.isRewardedVideoAvailable())
        {
            Debug.Log("ironsource: start load ads");
            _isOnLoadAds = true;
            IronSource.Agent.loadRewardedVideo();
        }
    }

    public void HideAds()
    {
        
    }

    public void ShowAds(Action<bool, string> onRewardedAds)
    {
        _onRewardedAds = onRewardedAds;
        if (CanShowAds())
        {
            Debug.Log("ironsource: start show ads");
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            Debug.Log("ironsource: show but ads not available");
            Toast.Show("Quảng cáo hiện không khả dụng");
            if (!_isOnLoadAds && _currentReloadAds < ironSourceConfig.maxCountReload)
            {
                LoadAds();
            }
        }
    }

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
            Signals.Get<LoadAdsSignal>().Dispatch(TypeAds.REWARDED, false);
        }
    }
}