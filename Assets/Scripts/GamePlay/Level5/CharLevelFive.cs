using System;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class CharLevelFive : CharacterJoinPlay
{
    [SerializeField] private SkeletonDataAsset skeletonWait, skeletonPlay;
    [SerializeField] private Button btnCam, btnDau, btnNho, btnSua, btnSocola, btnNuocOt;
    private Action<int> _typeChoose;

    private void Start()
    {
        btnCam.onClick.AddListener(() => _typeChoose?.Invoke((int)TypePlayFiveChoose.Cam));
        btnDau.onClick.AddListener(() => _typeChoose?.Invoke((int)TypePlayFiveChoose.Dau));
        btnNho.onClick.AddListener(() => _typeChoose?.Invoke((int)TypePlayFiveChoose.Nho));
        btnSua.onClick.AddListener(() => _typeChoose?.Invoke((int)TypePlayFiveChoose.Sua));
        btnSocola.onClick.AddListener(() => _typeChoose?.Invoke((int)TypePlayFiveChoose.Socola));
        btnNuocOt.onClick.AddListener(() => _typeChoose?.Invoke((int)TypePlayFiveChoose.NuocOt));
    }

    public void InitChoose(Action<int> typeChoose)
    {
        _typeChoose = typeChoose;
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