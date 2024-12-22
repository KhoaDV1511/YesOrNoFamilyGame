using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseLevel : MonoBehaviour
{
    [SerializeField] protected List<CharacterInfo> characterInfos;
    public abstract void StartPlay();
    public abstract void InitCharacter();
    public abstract void SwapInitPosCharacter(CharacterInfo characterToPlaying, CharacterInfo characterToWait);
    public abstract void ShowAnimReceiveTypePlay(int typePlay);
    public abstract void SwapPosCharPlaying(CharacterInfo charPlaying, CharacterInfo charToPLay);
    public abstract void UpdateProgress();
    
    protected CharacterInfo FindCharPlay()
    {
        return characterInfos.Count(c => !c.isPlayed) <= 0 ? null 
            : characterInfos.Where(c => !c.isPlayed).ToList()[Random.Range(0, characterInfos.Count(c => !c.isPlayed))];
    }

    protected void InitCharacterPlay()
    {
        characterInfos.ForEach(c =>
        {
            c.isPlayed = false;
            c.isPlaying = false;
        });
    }
}