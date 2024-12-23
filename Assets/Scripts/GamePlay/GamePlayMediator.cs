using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GamePlayMediator : MonoBehaviour
{
    [SerializeField] private BaseLevel[] baseLevels;
    [SerializeField] private ProgressPlay progressPlay;
    [SerializeField] private TextMeshProUGUI txtLevel;
    [SerializeField] private Button btnSkipAds;
    [SerializeField] private GameObject bottom, bg;
    [SerializeField] private Button btnRepeat, btnNext;

    private GamePlayModle _gamePlayModle = GamePlayModle.Instance;

    private void Start()
    {
        btnRepeat.onClick.AddListener(() =>
        {
            baseLevels[_gamePlayModle.currentLevel - 1].StartPlay();
            this.ShowFlashWithCallBack(() => baseLevels[_gamePlayModle.currentLevel - 1].Show());
        });
        btnNext.onClick.AddListener(NexLevel);
    }
    private void OnEnable()
    {
        Signals.Get<UpdateProgressSignals>().AddListener(UpdateProgress);
        Signals.Get<StartGameSignals>().AddListener(StartGame);
    }
    private void OnDisable()
    {
        Signals.Get<UpdateProgressSignals>().RemoveListener(UpdateProgress);
        Signals.Get<StartGameSignals>().RemoveListener(StartGame);
    }

    private void StartGame()
    {
        txtLevel.SetText($"level {_gamePlayModle.currentLevel}");
        for (int i = 0; i < baseLevels.Length; i++)
        {
            if (i == _gamePlayModle.currentLevel - 1)
            {
                baseLevels[i].StartPlay();
                progressPlay.ShowView();
                var i1 = i;
                this.ShowFlashWithCallBack(() =>
                {
                    bg.Show();
                    baseLevels[i1].Show();
                });
            }
            else
            {
                baseLevels[i].Hide();
            }
        }
        bottom.Hide();
    }
    private void NexLevel()
    {
        bg.Hide();
        this.ShowFlashWithCallBack(() =>
        {
            Signals.Get<UpDateHomeSignals>().Dispatch();
            PopupManager.OpenPopup<RewardPopup>(p =>
            {
                p.ShowView();
                _gamePlayModle.Coin += GamePlayModle.coinReward;
            });
        });
    }
    private void UpdateProgress(int progress, int total)
    {
        bottom.SetActive(progress >= total);
        if(progress >= total)
            _gamePlayModle.Level = _gamePlayModle.currentLevel + 1;
    }
}
