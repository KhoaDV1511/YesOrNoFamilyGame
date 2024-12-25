using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelThreeMediator : BaseLevel
{
    [SerializeField] private Bubble[] bubbles;
    private int _progress;
    private Tween _twChoose;
    private LevelThreeData LevelThree => GlobalDataManager.Ins.levelThreeData;
    
    [Button]
    public override void StartPlay()
    {
        _progress = 0;
        Signals.Get<UpdateProgressSignals>().Dispatch(_progress, characterInfos.Count);
        InitCharacter();
        InitBubble();
    }

    private void InitBubble()
    {
        foreach (var p in bubbles)                                                                        
        {                                                                                                          
            p.Init(ShowAnimReceiveTypePlay, ShowAnimPoke);                                                                      
        }   
    }

    private void ShowAnimPoke(TypePlayThreeChoose typePlayThreeChoose)
    {
        var charPlaying = characterInfos.Find(c => c.isPlaying);
        charPlaying.character.SetAnimChar((int)LevelThree.animPlayThreeInfos[(int)typePlayThreeChoose].itemAnimPoke, false);
    }
    public override void InitCharacter()
    {
        InitCharacterPlay();
        var charToPlaying = characterInfos[Random.Range(0, characterInfos.Count)];
        charToPlaying.isPlaying = true;
        SwapInitPosCharacter(charToPlaying, characterInfos.Find(c => c.rect.localScale.x > 0.5f));
    }

    public override void SwapInitPosCharacter(CharacterInfo characterToPlaying, CharacterInfo characterToWait)
    {
        characterToWait.isPlaying = false;
        characterToPlaying.isPlaying = true;
        if (characterToPlaying != characterToWait)
        {
            characterToPlaying.character.Cast<CharLevelThree>().SetSkeletonPlay((int)ItemAnimThree.Idle);
            characterToWait.character.Cast<CharLevelThree>().SetSkeletonWait((int)ItemAnimLevelOne.Idle);  
            characterToWait.character.AnimInitIdleWait(characterToPlaying, (int)ItemAnimLevelOne.Idle);
            characterToPlaying.character.AnimInitIdlePlay((int)ItemAnimThree.Idle);
        }
    }

    public override void ShowAnimReceiveTypePlay(int typePlay)
    {
        bubbles.ForEach(b => b.isChoose = false);
        var charPlaying = characterInfos.Find(c => c.isPlaying);
        charPlaying.isPlayed = true;
        Debug.Log($"show anim typeFood: {typePlay}");
        var itemAnimLevelOnes = LevelThree.animPlayThreeInfos
            .Find(a => a.typePlay == (TypePlayThreeChoose)typePlay).itemAnim
            .Select(i => (int)i).ToArray();
        charPlaying.character.ShowAnimReceiveResultChoose(itemAnimLevelOnes , () => 
            SwapPosCharPlaying(charPlaying, FindCharPlay()));
    }

    public override void SwapPosCharPlaying(CharacterInfo charPlaying, CharacterInfo charToPLay)
    {
        UpdateProgress();
        if (_progress >= characterInfos.Count)
        {
            // level complete
            Debug.Log("game complete");
            return;
        }
        if(charToPLay == null) return;
        charPlaying.isPlaying = false;
        charPlaying.character.Cast<CharLevelThree>().SetSkeletonWait((int)ItemAnimLevelOne.Idle); 
        charPlaying.character.ShowAnimToBackIdle(charToPLay, (int)ItemAnimLevelOne.Idle);
        charToPLay.isPlaying = true;
        charToPLay.character.Cast<CharLevelThree>().SetSkeletonPlay((int)ItemAnimThree.Idle); 
        charToPLay.character.ShowAnimMoveToPlayIdle((int)ItemAnimThree.Idle);
        _twChoose?.Kill();
        _twChoose = DOVirtual.DelayedCall(0.5f, () => bubbles.ForEach(p => p.isChoose = true));
    }

    public override void UpdateProgress()
    {
        _progress++;
        Signals.Get<UpdateProgressSignals>().Dispatch(_progress, characterInfos.Count);
    }
}