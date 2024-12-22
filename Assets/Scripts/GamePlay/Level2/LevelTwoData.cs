using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Data/LevelTwoData", fileName = "LevelTwoData")]
public class LevelTwoData : ScriptableObject
{
    public List<AnimPlayTwoInfo> animPlayTwoInfos;
}
[Serializable]
public class AnimPlayTwoInfo
{
    public bool isWin;
    public bool isChoose;
    public TypePlayTwoChoose typePlay;
    public Sprite sprItem;
    [FormerlySerializedAs("itemAnimLevelOnes")] public ItemAnimTwo[] itemAnim;
}
public enum TypePlayTwoChoose
{
    CaiO, CaiThia, LaChuoi, HopKem, CaiGio, CaiChao, ConCa, CaiChan
}

public enum ItemAnimTwo
{
    CamO, CamThia, CamLaChuoi, CamHopKem, CamGio, CamChao, CamConCa, CamChan,
    Idle, IdleNotBoxRight, IdleNotBoxLeft, IdleLose, IdleLoseKem, IdleWin
}