using DG.Tweening;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardUnlock : BaseUIPopup
{
    [SerializeField] private SkeletonGraphic skeletonReward;
    [SerializeField] private Button btnCollectReward;
    [SerializeField] private TextMeshProUGUI txtReward;
    private readonly ShowAdsSignal _showAdsSignal = Signals.Get<ShowAdsSignal>();
    private bool _adsAvailable;
    private Sequence _sqArrow;
    private const int rewardIdle = 0, rewardUnBox = 1;
    private int _nReward = 3;

    protected override void Start()
    {
        base.Start();
        btnCollectReward.onClick.AddListener(CollectReward);
    }
    private void OnEnable()
    {
        _showAdsSignal.AddListener(ResultShowAds);
    }

    private void OnDisable()
    {
        _showAdsSignal.RemoveListener(ResultShowAds);
    }
    public void ShowView()
    {
        _adsAvailable = AdsManager.Instance.CanShowAds(TypeAds.REWARDED);
        txtReward.SetText($"+{GamePlayModle.coinRewardUnlock}");
        skeletonReward.SetAnimSkeleton(rewardIdle, true);
        GamePlayModle.Instance.Coin += GamePlayModle.coinRewardUnlock;
    }

    private void CollectReward()
    {
     
        Debug.Log($"is ads reward available: {_adsAvailable}");
        if (_adsAvailable)
        {
            _adsAvailable = false;
            AdsManager.Instance.ShowAds(TypeAds.REWARDED, (b, placement) =>
            {
                if (b)
                {
                    Debug.Log("show reward");
                    skeletonReward.SetAnimSkeleton(rewardUnBox, false);
                    GamePlayModle.Instance.Coin += _nReward * GamePlayModle.coinRewardUnlock;
                }
            });
        }
        else
        {
            Debug.Log("Quang cao khong co san hoac khong phai level xem ads");
        }
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
}