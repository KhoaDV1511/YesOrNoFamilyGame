using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class CharLevelTwo : CharacterJoinPlay
{
    [SerializeField] private Image imgBoxLeft, imgBoxRight, imgItemInsideLeft, imgItemInsideRight;
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
    public void ChooseRight(Sprite sprItem)
    {
        imgBoxRight.Show();
        imgItemInsideRight.sprite = sprItem;
        imgItemInsideRight.SetNativeSize();
    }
    public void ChooseLeft(Sprite sprItem)
    {
        imgBoxLeft.Show();
        imgItemInsideLeft.sprite = sprItem;
        imgItemInsideLeft.SetNativeSize();
    }
    public void HideBox()
    {
        imgBoxLeft.Hide();
        imgBoxRight.Hide();
    }
}