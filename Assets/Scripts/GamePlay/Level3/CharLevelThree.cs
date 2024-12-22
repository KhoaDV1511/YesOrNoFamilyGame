using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class CharLevelThree : CharacterJoinPlay
{
    [SerializeField] private SkeletonDataAsset skeletonWait, skeletonPlay;
    
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
}