using com.unity3d.mediation;
using DG.Tweening;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardPopup : BaseUIPopup
{
    [SerializeField] private SkeletonGraphic skeletonReward;
    [SerializeField] private Button btnNextLevel, btnCollectReward;
    [SerializeField] private RectTransform rectArrow;
    [SerializeField] private TextMeshProUGUI txtReward;
    private readonly ShowAdsSignal _showAdsSignal = Signals.Get<ShowAdsSignal>();
    private bool _adsAvailable;
    private Sequence _sqArrow;
    private const int rewardIdle = 0, rewardUnBox = 1;
    private int _nReward;

    protected override void Start()
    {
        base.Start();
        btnNextLevel.onClick.AddListener(() =>
        {
            Signals.Get<StartGameSignals>().Dispatch();
            OnClose();
        });
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
        _adsAvailable = AdsManager.Instance.CanShowAds(LevelPlayAdFormat.REWARDED);
        txtReward.SetText($"+{GamePlayModle.coinReward}");
        skeletonReward.SetAnimSkeleton(rewardIdle, true);
    }

    private void CollectReward()
    {
     
        Debug.Log($"is ads reward available: {_adsAvailable}");
        if (_adsAvailable)
        {
            _adsAvailable = false;
            AdsManager.Instance.ShowAds(LevelPlayAdFormat.REWARDED, (b, placement) =>
            {
                if (b)
                {
                    Debug.Log("show reward");
                    skeletonReward.SetAnimSkeleton(rewardUnBox, true);
                    GamePlayModle.Instance.Coin += _nReward * GamePlayModle.coinReward;
                }
            });
        }
        else
        {
            Debug.Log("Quang cao khong co san hoac khong phai level xem ads");
        }
    }
    
    private void ResultShowAds(LevelPlayAdFormat levelPlayAdFormat, bool isSuccess)
    {
        if (isSuccess)
        {
            _adsAvailable = true;
        }
        else
        {
            OnRetryLoadAdsFail(levelPlayAdFormat);
        }
    }
    private void OnRetryLoadAdsFail(LevelPlayAdFormat levelPlayAdFormat)
    {
        _adsAvailable = false;
        if(!gameObject.activeSelf) return;

        Debug.Log("Quảng cáo không khả dụng");
    }
}