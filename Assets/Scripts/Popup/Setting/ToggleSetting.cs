using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleSetting : MonoBehaviour
{
    [SerializeField] private Image imageToggle, imgHandle;
    [SerializeField] private TextMeshProUGUI txtStatus;
    [SerializeField] private RectTransform rectTxtStatus, rectHandle;
    [SerializeField] private Button btnToggle;
    [SerializeField] private Sprite sprToggleOn, sprToggleOff, sprHandleOn, sprHandleOff;
    public UnityEvent onclick;
    private int posXTxtOff = 35, posXTxtOn = -35;
    private int posXHandleOff = -70, posXHandleOn = 70;

    private void Start()
    {
        btnToggle.onClick.AddListener(onclick.Invoke);
    }
    public void SetToggle(bool isOn)
    {
        imageToggle.sprite = isOn ? sprToggleOn : sprToggleOff;
        imgHandle.sprite = isOn ? sprHandleOn : sprHandleOff;
        var txt = isOn ? "ON" : "OFF";
        txtStatus.SetText(txt);
        rectTxtStatus.anchoredPosition = new Vector2(isOn ? posXTxtOn : posXTxtOff, rectTxtStatus.anchoredPosition.y);
        rectHandle.anchoredPosition = new Vector2(isOn ? posXHandleOn : posXHandleOff, rectHandle.anchoredPosition.y);
    }
}