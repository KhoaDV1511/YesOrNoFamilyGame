using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    private Button _btn;

    private void Start()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(SoundOnClick);
    }

    private void SoundOnClick()
    {
        SoundManager.Instance.PlayGameSound(Sound.ButtonClick);
    }
}