using System;
using UnityEngine;
using UnityEngine.UI;

public class HomeMediator : MonoBehaviour
{
    [SerializeField] private Button btnTabPlay, btnAds, btnCoin, btnSetting;

    private void Start()
    {
        btnTabPlay.onClick.AddListener(TabPlay);
    }

    private void TabPlay()
    {
        Signals.Get<StartGameSignals>().Dispatch();
    }
}