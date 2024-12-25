using System;
using System.Collections.Generic;
using System.Linq;
using com.unity3d.mediation;
using UnityEngine;

public class AdsManager : MonoSingleton<AdsManager>
{
    [SerializeField] private IronSourceADUnitIdConfig[] adUnitIdConfig = { };
    private List<BaseAds> _listAds = new();

    protected override void Start()
    {
        base.Start();
        _listAds = GetComponentsInChildren<BaseAds>().ToList();
        Initialize();
    }
    
    private void OnEnable()
    {
        Signals.Get<LoadAdsRewardSignal>().AddListener(ResultLoadAds);
    }

    private void OnDisable()
    {
        Signals.Get<LoadAdsRewardSignal>().RemoveListener(ResultLoadAds);
    }

    private void ResultLoadAds( bool loadSuccess)
    {
        if (loadSuccess)
        {
            Signals.Get<ShowAdsRewardSignal>().Dispatch(true);
        }
        else
        {
            if (!CanShowAds(TypeAds.REWARDED))
            {
                Debug.Log("All ads not available");
                Signals.Get<ShowAdsRewardSignal>().Dispatch(false);
            }
            else
            {
                Signals.Get<ShowAdsRewardSignal>().Dispatch(true);
            }
        }
    }

    public bool CanShowAds(TypeAds levelPlayAdFormat)
    {
        var ads = _listAds.Find(a => a.levelPlayAdFormat == levelPlayAdFormat);
        return ads?.CanShowAds() ?? false;
    }

    public void LoadAds(TypeAds levelPlayAdFormat)
    {
        var ads = _listAds.Find(a => a.levelPlayAdFormat == levelPlayAdFormat);
        if(ads != null)
            _listAds.Find(a => a.levelPlayAdFormat == levelPlayAdFormat).LoadAds();
    }

    public void ShowAds(TypeAds levelPlayAdFormat, Action<bool, string> onRewardedAds = null)
    {
        var adsShow = _listAds.Find(a => a.levelPlayAdFormat == levelPlayAdFormat);
        if(adsShow == null) return;
        if (adsShow.CanShowAds())
        {
            adsShow.ShowAds(onRewardedAds);
            return;
        }
        if(levelPlayAdFormat != TypeAds.BANNER)
            LoadAndShowAds(levelPlayAdFormat);
    }

    public void HideBanner()
    {
        var adsShow = _listAds.Find(a => a.levelPlayAdFormat == TypeAds.BANNER);
        adsShow?.HideAds();
    }
    private void LoadAndShowAds(TypeAds levelPlayAdFormat)
    {
        if (!CanShowAds(levelPlayAdFormat))
        {
            LoadAds(levelPlayAdFormat);
        }
    }

    private void Initialize()
    {
        if (GameUtils.IsAndroid() || GameUtils.IsIOS())
        {
            Debug.Log("unity-script: IronSource.Agent.validateIntegration");
            IronSource.Agent.validateIntegration();

            Debug.Log("unity-script: unity version" + IronSource.unityVersion());

            // SDK init
            Debug.Log("unity-script: LevelPlay SDK initialization");
            LevelPlay.Init(adUnitIdConfig[(int)LevelPlayAdFormat.REWARDED].ADUnitId ,adFormats:new []{LevelPlayAdFormat.REWARDED});
        
            LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
            LevelPlay.OnInitFailed += SdkInitializationFailedEvent;
        }
    }
    void SdkInitializationCompletedEvent(LevelPlayConfiguration config)
    {
        Debug.Log("unity-script: I got SdkInitializationCompletedEvent with config: "+ config);
        EnableAds();
    }
    
    void SdkInitializationFailedEvent(LevelPlayInitError error)
    {
        Debug.Log("unity-script: I got SdkInitializationFailedEvent with error: "+ error);
    }
    void EnableAds()
    {
        //Add ImpressionSuccess Event
        IronSourceEvents.onImpressionDataReadyEvent += ImpressionDataReadyEvent;

        //Add AdInfo Rewarded Video Events
        _listAds.Find(a => a.levelPlayAdFormat == TypeAds.REWARDED)
            .Initialize(adUnitIdConfig.ToList().Find(a => a.levelPlayAdFormat == TypeAds.REWARDED));

        // Register to Banner events
        _listAds.Find(a => a.levelPlayAdFormat == TypeAds.BANNER)
            .Initialize(adUnitIdConfig.ToList().Find(a => a.levelPlayAdFormat ==TypeAds.BANNER));

        // Register to Interstitial events
        _listAds.Find(a => a.levelPlayAdFormat == TypeAds.INTERSTITIAL)
            .Initialize(adUnitIdConfig.ToList().Find(a => a.levelPlayAdFormat ==TypeAds.INTERSTITIAL));
    }
    void ImpressionDataReadyEvent(IronSourceImpressionData impressionData)
    {
        Debug.Log("unity - script: I got ImpressionDataReadyEvent ToString(): " + impressionData.ToString());
        Debug.Log("unity - script: I got ImpressionDataReadyEvent allData: " + impressionData.allData);
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        IronSource.Agent.onApplicationPause(pauseStatus);
    }
}

[Serializable]
public class IronSourceADUnitIdConfig
{
    public TypeAds levelPlayAdFormat;
    [SerializeField] private bool testMode = false;
    [SerializeField] private string androidId = "";
    [SerializeField] private string iosId = "";
    [SerializeField] private string androidIdTest = "85460dcd";
    [SerializeField] private string iOSIdTest = "8545d445";
    
    public string ADUnitId
    {
        get
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return testMode ? androidIdTest : androidId;
                case RuntimePlatform.IPhonePlayer:
                    return testMode ? iOSIdTest : iosId;
            }

            return "unexpected_platform";
        }
    }

    public int maxCountReload = 3;
    public float timeReloadAds = 5f;
}
public enum TypeAds
{
    BANNER,
    INTERSTITIAL,
    REWARDED
}