using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayMediator : MonoBehaviour
{
    [SerializeField] private LevelOne levelOne;
    [SerializeField] private ProgressPlay progressPlay;
    [SerializeField] private TextMeshProUGUI txtLevel;
    [SerializeField] private Button btnSkipAds;
    [SerializeField] private GameObject bottom;
    [SerializeField] private Button btnRepeat, btnNext;

    private void Start()
    {
        btnRepeat.onClick.AddListener(levelOne.StartPlay);
        btnNext.onClick.AddListener(NexLevel);
    }
    private void OnEnable()
    {
        Signals.Get<UpdateProgressSignals>().AddListener(UpdateProgress);
    }
    private void OnDisable()
    {
        Signals.Get<UpdateProgressSignals>().RemoveListener(UpdateProgress);
    }

    private void StartGame()
    {
        bottom.Hide();
    }
    private void NexLevel()
    {
        
    }
    private void UpdateProgress(int progress, int total)
    {
        progressPlay.ShowView(progress, total);
        bottom.SetActive(progress >= total);
    }
}
