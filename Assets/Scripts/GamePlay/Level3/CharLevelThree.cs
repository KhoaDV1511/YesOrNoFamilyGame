using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class CharLevelThree : CharacterJoinPlay
{
    [SerializeField] private SkeletonDataAsset skeletonWait, skeletonPlay;
    
    [Button]
    public void SetSkeletonWait()
    {
        SetSkeleton(skeletonWait, (int)ItemAnimLevelOne.Idle, true);
    }
    [Button]
    public void SetSkeletonPlay()
    {
        SetSkeleton(skeletonPlay, (int)ItemAnimTwo.Idle, true);
    }
}