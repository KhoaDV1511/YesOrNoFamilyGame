using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    private static List<BaseUIPopup> _POPUPS = new();

    private void OnEnable()
    {
        _POPUPS = new List<BaseUIPopup>();
        _POPUPS = transform.GetComponentsInChildren<BaseUIPopup>(true).ToList();
        CloseAllPopup();
    }
    
    private void OnDisable()
    {
        CloseAllPopup();
    }
    
    public static T GetTab<T>() where T : BaseUIPopup
    {
        var popupType = typeof(T).Name;
        return _POPUPS.Find(hq => hq.GetType().Name == popupType) as T;
    }

    private static void CloseAllPopup()
    {
        if(_POPUPS == null) return;
        foreach (var p in _POPUPS)
        {
            p.Hide();
        }
    }

    public static void OpenPopup<T>(Action<T> onLoadComplete = null) where T : BaseUIPopup
    {
        var popupType = typeof(T).Name;
        foreach (var p in _POPUPS)
        {
            p.gameObject.SetActive(p.GetType().Name == popupType);
            if (p.GetType().Name != popupType) continue;
            onLoadComplete?.Invoke(p as T);
        }
    }
}