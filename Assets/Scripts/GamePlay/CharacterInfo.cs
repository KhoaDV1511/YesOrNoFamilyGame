using System;
using UnityEngine;

[Serializable]
public class CharacterInfo
{
    public CharacterJoinPlay character;
    public bool isPlayed;
    public bool isPlaying;
    public RectTransform rect => character.GetComponent<RectTransform>();
}