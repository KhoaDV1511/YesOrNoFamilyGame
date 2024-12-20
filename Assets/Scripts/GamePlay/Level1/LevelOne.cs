using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelOne : MonoBehaviour
{
    [SerializeField] private SkeletonGraphic skeleton;
    [SerializeField] private List<CharacterInfo> characterInfos;
    [SerializeField] private PieceMove[] pieceMoves;
    private List<TypeFood> typeFoods;
    private int _indexCharacterPlay, _progress;
    private LevelOneData levelOneData => GlobalDataManager.Ins.levelOneData;
    

    [Button]
    private void SetAnim()
    {
        skeleton.AnimationState.SetAnimation
            (0, skeleton.SkeletonData.Animations.Items[(int)ItemAnimLevelOne.EatCake], true);
        Debug.Log($"{skeleton.SkeletonData.Animations.Items[(int)ItemAnimLevelOne.EatCake].Duration}");
    }

    [Button]
    public void StartPlay()
    {
        InitPlay();
    }

    private void InitPlay()
    {
        InitPiece();
        InitCharacter();
    }

    private void InitCharacter()
    {
        var charToPlaying = characterInfos[Random.Range(0, characterInfos.Count)];
        charToPlaying.isPlaying = true;
        foreach (var c in characterInfos)
        {
            c.character.SetAnimChar((int)ItemAnimLevelOne.Idle, true);
        }
        
        SwapInitPosCharacter(charToPlaying, characterInfos.Find(c => c.rect.localScale.x > 0.4f));
    }

    private CharacterInfo FindCharPlay()
    {
        return characterInfos.Count(c => !c.isPlayed) <= 0 ? null 
            : characterInfos.Where(c => !c.isPlayed).ToList()[Random.Range(0, characterInfos.Count(c => !c.isPlayed))];
    }
    private void SwapInitPosCharacter(CharacterInfo characterToPlaying, CharacterInfo characterToWait)
    {
        characterToWait.character.AnimInitIdleWait(characterToPlaying, (int)ItemAnimLevelOne.Idle);
        characterToPlaying.character.AnimInitIdlePlay((int)ItemAnimLevelOne.Idle);
        characterToPlaying.isPlaying = true;
        characterToWait.isPlaying = false;
    }
    private void InitPiece()
    {
        var typeFoodSet = new List<TypeFood>();                                                              
        typeFoods = new List<TypeFood>();                                                                    
        foreach (var tp in Enum.GetValues(typeof(TypeFood)))                                                 
        {                                                                                                    
            typeFoods.Add((TypeFood)tp);                                                                     
        }                                                                                                    
        foreach (var p in pieceMoves)                                                                        
        {                                                                                                    
            var typeFood = Random.Range(0, typeFoods.Count(t => !typeFoodSet.Contains(t)));        
            p.typeFood = typeFoods.Where(t => !typeFoodSet.Contains(t)).ToList()[typeFood];        
            p.InitAddListener(ShowAnimReceiveTypeFood);
            typeFoodSet.Add(p.typeFood);                                                                       
        }                                                                                                    
    }

    private void ShowAnimReceiveTypeFood(TypeFood typeFood)
    {
        var charPlaying = characterInfos.Find(c => c.isPlaying);
        charPlaying.isPlayed = true;
        Debug.Log($"show anim typeFood: {typeFood}");
        var itemAnimLevelOnes = levelOneData.animFoodInfos
            .Find(a => a.typeFood == typeFood).itemAnimLevelOnes.Select(i => (int)i).ToArray();
        charPlaying.character.ShowAnimReceiveResultChoose(itemAnimLevelOnes , () => 
            SwapPosCharPlaying(charPlaying, FindCharPlay()));
    }

    private void SwapPosCharPlaying(CharacterInfo charPlaying, CharacterInfo charToPLay)
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
        charToPLay.isPlaying = true;
        charPlaying.character.ShowAnimToBackIdle(charToPLay, (int)ItemAnimLevelOne.Idle);
        charToPLay.character.ShowAnimMoveToPlayIdle((int)ItemAnimLevelOne.Idle);
        
    }

    private void UpdateProgress()
    {
        _progress++;
        Signals.Get<UpdateProgressSignals>().Dispatch(_progress, characterInfos.Count);
    }
}