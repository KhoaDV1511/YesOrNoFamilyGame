using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class CharLevelFive : CharacterJoinPlay
{
    [SerializeField] private SkeletonDataAsset skeletonWait, skeletonPlay;
    [SerializeField] private Button btnCam, btnDau, btnNho, btnSua, btnSocola, btnNuocOt;
    private Action<int> _typeChoose;
    public bool isChoose;

    private void Start()
    {
        btnCam.onClick.AddListener(ChooseCam);
        btnDau.onClick.AddListener(ChooseDau);
        btnNho.onClick.AddListener(ChooseNho);
        btnSua.onClick.AddListener(ChooseSua);
        btnSocola.onClick.AddListener(ChooseSocola);
        btnNuocOt.onClick.AddListener(ChooseNuocOt);
    }

    private void ChooseCam()
    {
        if (isChoose)
        {
            _typeChoose?.Invoke((int)TypePlayFiveChoose.Cam);
        }
    }
    private void ChooseDau()
    {
        if (isChoose)
        {
            _typeChoose?.Invoke((int)TypePlayFiveChoose.Dau);
        }
    }
    private void ChooseNho()
    {
        if (isChoose)
        {
            _typeChoose?.Invoke((int)TypePlayFiveChoose.Nho);
        }
    }
    private void ChooseSua()
    {
        if (isChoose)
        {
            _typeChoose?.Invoke((int)TypePlayFiveChoose.Sua);
        }
    }
    private void ChooseSocola()
    {
        if (isChoose)
        {
            _typeChoose?.Invoke((int)TypePlayFiveChoose.Socola);
        }
    }
    private void ChooseNuocOt()
    {
        if (isChoose)
        {
            _typeChoose?.Invoke((int)TypePlayFiveChoose.NuocOt);
        }
    }
    public void InitChoose(Action<int> typeChoose)
    {
        _typeChoose = typeChoose;
        isChoose = true;
    }
    [Button]
    public void SetSkeletonWait(int animIndex)
    {
        SetSkeleton(skeletonWait, animIndex, true);
    }
    [Button]
    public void SetSkeletonPlay(int animIndex)
    {
        SetSkeleton(skeletonPlay, animIndex, true);
    }
}