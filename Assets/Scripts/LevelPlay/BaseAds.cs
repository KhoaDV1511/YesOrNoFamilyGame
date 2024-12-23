using System;
using com.unity3d.mediation;

public interface BaseAds
{
    LevelPlayAdFormat levelPlayAdFormat { get; }
    bool CanShowAds();
    void LoadAds();
    void HideAds();
    void ShowAds(Action<bool, string> onRewardedAds);
    void Initialize(IronSourceADUnitIdConfig cfg);
}