using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AnimPopup))]
public abstract class BaseUIPopup : MonoBehaviour
{
    protected AnimPopup animPopup;
    [SerializeField] private Button btnClose, btnBgClose;

    protected virtual void Start()
    {
        animPopup = GetComponent<AnimPopup>();
        btnClose!?.onClick.AddListener(OnClose);
        btnBgClose!?.onClick.AddListener(OnClose);
    }

    protected void OnClose()
    {
        animPopup.OnClose();
    }
}