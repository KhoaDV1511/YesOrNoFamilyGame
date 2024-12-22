using System;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public static class GameExtension
{
    public static void Hide(this GameObject obj)
    {
        obj.SetActive(false);
    }

    public static void Hide(this Component component)
    {
        component.gameObject.SetActive(false);
    }

    public static void Show(this GameObject obj)
    {
        obj.SetActive(true);
    }

    public static void Show(this Component o)
    {
        o.gameObject.SetActive(true);
    }
    
    public static void ShowFlashWithCallBack(this MonoBehaviour obj, Action callBack)
    {
        FlashPanel.Open().ShowFlashWithCallBack(callBack);
    }
    public static void ChangeAlpha(this Graphic s, float f)
    {
        var c = s.color;
        c.a = f;
        s.color = c;
    }
    public static T Cast<T>(this MonoBehaviour mono) where T : class
    {
        var t = mono as T;
        return t;
    }
    public static void SetAnimSkeleton(this SkeletonGraphic skeleton, int index,  bool loop)
    {
        skeleton.startingAnimation = skeleton.SkeletonData.Animations.Items[index].Name;
        skeleton.startingLoop = loop;
        skeleton.Initialize(true);
    }

    static Sequence _sqCharacter;
    public static void SetArrayAnimSkeleton(this SkeletonGraphic skeleton, int[] itemAnim,  bool loop)
    {
        _sqCharacter?.Kill();
        _sqCharacter = DOTween.Sequence();
        var time = 0f;
        var item = skeleton.SkeletonData.Animations.Items;
        for (int i = 0; i < itemAnim.Length; i++)
        {
            var i1 = i;
            _sqCharacter.AppendInterval(time)
                .AppendCallback(() =>
                {
                    //Debug.Log($"show anim receive food: {item[itemAnim[i1]].Name}");
                    skeleton.SetAnimSkeleton(itemAnim[i1], loop);
                });
            time = item[itemAnim[i1]].Duration;
        }
    }
}