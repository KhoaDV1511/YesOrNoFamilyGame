using System;
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
}