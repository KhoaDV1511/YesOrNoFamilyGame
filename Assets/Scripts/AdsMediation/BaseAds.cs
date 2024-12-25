using System;

public interface BaseAds
{
    TypeAds levelPlayAdFormat { get; }
    bool CanShowAds();
    void LoadAds();
    void HideAds();
    void ShowAds(Action<bool, string> onRewardedAds);
    void Initialize(IronSourceADUnitIdConfig cfg);
}