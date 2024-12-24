using System;
using LuaFramework;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _Instance;
    private static bool isLoadedIns;

    public static SoundManager Instance
    {
        get
        {
            if (!isLoadedIns)
            {
                _Instance = FindObjectOfType<SoundManager>();
                isLoadedIns = true;
            }

            return _Instance;
        }
    }

    [SerializeField] private AudioSource audioSourceGame, audioSourceBgGame;
    
    public void PlayGameBgMusic(string nameBg)
    {
        Sound.LoadSound(nameBg, clip =>
        {
            if (clip == null) return;

            audioSourceBgGame.loop = true;
            audioSourceBgGame.clip = clip;
            audioSourceBgGame.Play();
        });
    }

    public void PlayGameSound(string nameSound)
    {
        Sound.LoadSound(nameSound, c =>
        {
            if (c == null) return;
            Debug.Log(c.name);
            audioSourceGame.mute = false;
            audioSourceGame.loop = false;
            audioSourceGame.clip = c;
            audioSourceGame.Play();
        });
    }
    
    public void ChangeVolumeBg(float v)
    {
        audioSourceBgGame.volume = v;
    }

    public void ChangeVolumeGame(float v)
    {
        audioSourceGame.volume = v;
    }

    public void StopSoundGame()
    {
        audioSourceGame.Stop();
    }
    
    public void StopSoundBgGame()
    {
        audioSourceBgGame.Stop();
    }
}