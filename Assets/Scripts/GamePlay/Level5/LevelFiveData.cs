using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/LevelFiveData", fileName = "LevelFiveData")]
public class LevelFiveData : ScriptableObject
{
    public List<AnimPlayFiveInfo> animPlayFiveInfos;
}

[Serializable]
public class AnimPlayFiveInfo
{
    public bool isLose;
    public TypePlayFiveChoose typePlayFiveChoose;
    public ItemAnimFive[] itemAnimFives;
}
public enum TypePlayFiveChoose
{
    Cam, Dau, Nho, Sua, Socola, NuocOt
}

public enum ItemAnimFive
{
    Idle, Lose, Win, Cam, Dau, Nho, NuocOt, Socola, Sua
}