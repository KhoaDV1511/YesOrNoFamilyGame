using System;
using UnityEngine;

[Serializable]
public class CharacterInfo
{
    public CharacterLevelOne character;
    public bool isPlayed;
    public bool isPlaying;
    public RectTransform rect => character.GetComponent<RectTransform>();
}