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
    public void ShowView()
    {
        txtReward.SetText($"+{GamePlayModle.coinReward}");
    }
}