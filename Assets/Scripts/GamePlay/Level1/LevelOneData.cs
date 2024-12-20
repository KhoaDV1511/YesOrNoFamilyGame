using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/LevelOneData", fileName = "LevelOneData")]
public class LevelOneData : ScriptableObject
{
    public List<AnimFoodInfo> animFoodInfos;
}

[Serializable]
public class AnimFoodInfo
{
    public TypeFood typeFood;
    public ItemAnimLevelOne[] itemAnimLevelOnes;
}
public enum TypeFood
{
    StrawBerry,
    Cherry,
    Chilli,
    Onion,
    CupCake
}

public enum ItemAnimLevelOne
{
    EatCake, EatCherry, EatStrawberry, EatUnion, EatChili,
    HoldCake, HoldCherry, HoldStrawberry, HoldUnion, HoldTrayCherry, HoldTrayStrawBerry, HoldChilli,
    Idle, IdleSad, IdleFunny, TrayCherry, TrayStrawBerry
}