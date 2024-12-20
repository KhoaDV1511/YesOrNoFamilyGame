using UnityEngine;

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
}