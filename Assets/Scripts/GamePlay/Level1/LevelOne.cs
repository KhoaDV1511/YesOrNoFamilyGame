using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class LevelOne : MonoBehaviour
{
    [SerializeField] private SkeletonGraphic skeleton;
    [SerializeField] private string nameAnim;

    [Button]
    private void SetAnim()
    {
        skeleton.AnimationState.SetAnimation
            (0, skeleton.SkeletonData.Animations.Items[(int)ItemAnimLevelOne.EatCake], true);
        Debug.Log($"{skeleton.SkeletonData.Animations.Items[(int)ItemAnimLevelOne.EatCake].Duration}");
    }
}