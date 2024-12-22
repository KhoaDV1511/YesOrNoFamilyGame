using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class CharLevelFour : CharacterJoinPlay
{
    [SerializeField] private SkeletonDataAsset skeletonWait, skeletonPlay;
    [SerializeField] private SkeletonGraphic water;
    private Tween _twShowWater;
    
    [Button]
    public void SetSkeletonWait()
    {
        SetSkeleton(skeletonWait, 0, true);
    }
    [Button]
    public void SetSkeletonPlay()
    {
        SetSkeleton(skeletonPlay, 0, true);
    }

    public void HideWater()
    {
        water.Hide();
    }
    public void ShowWater()
    {
        water.Show();
        water.SetAnimSkeleton(0, false);
        _twShowWater?.Kill();
        _twShowWater = DOVirtual.DelayedCall(water.SkeletonData.Animations.Items.First().Duration, (() => water.Hide()));
    }
}