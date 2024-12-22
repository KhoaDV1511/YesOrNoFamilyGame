using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Data/LevelFourData", fileName = "LevelFourData")]
public class LevelFourData : ScriptableObject
{
    public List<AnimPlayFourInfo> animPlayFourInfos;
    public List<PokeFourInfo> pokeFourInfos;
}
[Serializable]
public class AnimPlayFourInfo
{
    public TypePlayFourChoose typePlayFourChoose;
    public ItemAnimFour[] itemAnim;
}

[Serializable]
public class PokeFourInfo
{
    public bool isChoose;
    public ItemAnimFour itemAnimPoke;
    public TypeBubbleWater typeBubbleWater;
}

public enum TypeBubbleWater
{
    Red, Violet, Yellow, Blue, Green
}
public enum TypePlayFourChoose
{
    NotMuc, MucDen
}

public enum ItemAnimFour
{
    Idle, IdlePokeRed, IdlePokeViolet, IdlePokeYellow, IdlePokeBlue, IdlePokeGreen,
    WinOne, WinTwo, LoseOne, LoseTwo, LoseIdle, LoseWater
}