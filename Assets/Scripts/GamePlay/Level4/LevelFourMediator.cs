using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class LevelFourMediator : BaseLevel
{
    [SerializeField] private BubbleWater[] bubbleWaters;
    [SerializeField] private SkeletonGraphic muc;
    private int _progress;
    private Tween _twShowWater;
    private LevelFourData levelFourData => GlobalDataManager.Ins.levelFourData;
    
    [Button]
    public override void StartPlay()
    {
        muc.Hide();
        InitCharacter();
        InitBubble();
    }

    private void InitBubble()
    {
        var bubbleMuc = Random.Range(0, bubbleWaters.Length);
        for (int i = 0; i < bubbleWaters.Length; i++)
        {
            bubbleWaters[i].typePlayFourChoose = i == bubbleMuc ? TypePlayFourChoose.MucDen 
                : TypePlayFourChoose.NotMuc;
            bubbleWaters[i].Init(ShowAnimReceiveTypePlay , ShowAnimPoke);
        }
    }
    private void ShowAnimPoke(ItemAnimFour typePoke)
    {
        var charPlaying = characterInfos.Find(c => c.isPlaying);
        charPlaying.character.SetAnimChar((int)typePoke, false);
    }
    public override void InitCharacter()
    {
        InitCharacterPlay();
        var charToPlaying = characterInfos[Random.Range(0, characterInfos.Count)];
        charToPlaying.isPlaying = true;
        foreach (var c in characterInfos)
        {
            c.character.Cast<CharLevelFour>().HideWater();
        }
        SwapInitPosCharacter(charToPlaying, characterInfos.Find(c => c.rect.localScale.x > 0.5f));
    }

    public override void SwapInitPosCharacter(CharacterInfo characterToPlaying, CharacterInfo characterToWait)
    {
        characterToWait.isPlaying = false;
        characterToPlaying.isPlaying = true;
        if (characterToPlaying != characterToWait)
        {
            characterToPlaying.character.Cast<CharLevelFour>().SetSkeletonPlay();
            characterToWait.character.Cast<CharLevelFour>().SetSkeletonWait();  
            characterToWait.character.AnimInitIdleWait(characterToPlaying, (int)ItemAnimLevelOne.Idle);
            characterToPlaying.character.AnimInitIdlePlay((int)ItemAnimFour.Idle);
        }
    }

    public override void ShowAnimReceiveTypePlay(int typePlay)
    {
        var charPlaying = characterInfos.Find(c => c.isPlaying);
        charPlaying.isPlayed = true;
        Debug.Log($"show anim typeFood: {typePlay}");
        var itemAnimLevelOnes = levelFourData.animPlayFourInfos
            .Find(a => a.typePlayFourChoose == (TypePlayFourChoose)typePlay).itemAnim
            .Select(i => (int)i).ToArray();
        ShowWater((TypePlayFourChoose)typePlay);
        charPlaying.character.ShowAnimReceiveResultChoose(itemAnimLevelOnes , () => 
            SwapPosCharPlaying(charPlaying, FindCharPlay()));
    }

    private void ShowWater(TypePlayFourChoose typePlayFourChoose)
    {
        var charPlaying = characterInfos.Find(c => c.isPlaying);
        if (typePlayFourChoose == TypePlayFourChoose.MucDen)
        {
            _progress = characterInfos.Count - 1;
            muc.Show();
            muc.SetArrayAnimSkeleton(new []{0,1}, true);
            _twShowWater?.Kill();
            _twShowWater = DOVirtual.DelayedCall(2f, () => charPlaying.character
                .Cast<CharLevelFour>().ShowWater()); 
        }
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
        charPlaying.character.Cast<CharLevelFour>().SetSkeletonWait(); 
        charPlaying.character.ShowAnimToBackIdle(charToPLay, (int)ItemAnimLevelOne.Idle);
        charToPLay.isPlaying = true;
        charToPLay.character.Cast<CharLevelFour>().SetSkeletonPlay(); 
        charToPLay.character.ShowAnimMoveToPlayIdle((int)ItemAnimFour.Idle);
    }

    public override void UpdateProgress()
    {
        _progress++;
        Signals.Get<UpdateProgressSignals>().Dispatch(_progress, characterInfos.Count);
    }
}