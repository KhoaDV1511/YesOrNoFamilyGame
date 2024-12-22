using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class CharLevelChangeSkeleton : CharacterJoinPlay
{
    [SerializeField] private SkeletonDataAsset skeletonWait, skeletonPlay;
    
    [Button]
    public void SetSkeletonWait(int animIndex)
    {
        SetSkeleton(skeletonWait, animIndex, true);
    }
    [Button]
    public void SetSkeletonPlay(int animIndex)
    {
        SetSkeleton(skeletonPlay, animIndex, true);
    }
}