using System;
using System.Collections;
using System.Collections.Generic;
using LuaFramework;
using UnityEngine;

public class Sound
{
    private static string SoundPath = "Sounds/";

    public const string BgGame = "bg_game";
    public const string Brawler = "brawler_01";
    public const string ButtonClick = "button_click";
    public const string BuyItem = "buy_item";
    public const string Lightning = "lighting";
    public const string PickUpMaterial = "pickup_material_01";
    public const string PistolShoot = "pistol_shot_01";
    public const string PunchKnife = "punch_knife_01";
    public const string SMGShoot = "smg_shot_01";

    public static void LoadSound(string soundName, Action<AudioClip> onComplete)
    {
        Executors.RunOnCoroutineNoReturn(ILoadSound(soundName, onComplete));
    }
    
    private static IEnumerator ILoadSound(string soundName, Action<AudioClip> onComplete)
    {
        var request = (AudioClip)Resources.Load($"{SoundPath}{soundName}", typeof(AudioClip));
        yield return request;
        onComplete.Invoke(request);
    }
}