using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Data/LevelThreeData", fileName = "LevelThreeData")]
public class LevelThreeData : ScriptableObject
{
    public List<AnimPlayThreeInfo> animPlayThreeInfos;
}
[Serializable]
public class AnimPlayThreeInfo
{
    public bool isChoose;
    public TypePlayThreeChoose typePlay;
    public Sprite sprItem;
    public ItemAnimThree[] itemAnim;
    public ItemAnimThree itemAnimPoke;
}
public enum TypePlayThreeChoose
{
    BinhXit, BanhKem, CaiBuaHoi, Money, VuongMien
}

public enum ItemAnimThree
{
    EatCake, UseBinhXit, UseBuaHoi, TakeMoney, UseVuongMien, Idle,
    EffectCake, EffectBinhXit, EffectBuaHoi, EffectMoney, EffectVuongMien,
    PokeBuaHoi, PokeCake, PokeBinhXit, PokeVuongMien, PokeMoney
}