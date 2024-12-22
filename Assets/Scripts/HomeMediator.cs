using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeMediator : MonoBehaviour
{
    [SerializeField] private Button btnTabPlay, btnAds, btnCoin, btnSetting;
    [SerializeField] private TextMeshProUGUI txtLevel, txtMoney;
    [SerializeField] private GameObject bg;

    private void Start()
    {
        btnTabPlay.onClick.AddListener(TabPlay);
    }

    private void OnEnable()
    {
        UpdateHome();
        Signals.Get<UpDateHomeSignals>().AddListener(UpdateHome);
    }

    private void OnDisable()
    {
        Signals.Get<UpDateHomeSignals>().RemoveListener(UpdateHome);
    }

    private void UpdateHome()
    {
        bg.Show();
        txtLevel.SetText(GamePlayModle.Instance.Level.ToString());
        txtMoney.SetText(GamePlayModle.Instance.Coin.ToString());
    }
    private void TabPlay()
    {
        Signals.Get<StartGameSignals>().Dispatch();
        bg.Hide();
    }
}