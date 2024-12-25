using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class LevelTwo : BaseLevel, IPointerClickHandler
{
    [SerializeField] private SkeletonGraphic girlPouringWater, water;
    [SerializeField] private CharacterInfo charInPlay;
    private LevelTwoData levelTwoData => GlobalDataManager.Ins.levelTwoData;
    private float _posClick;
    private int _progress;
    private bool _isChoose;
    private AnimPlayTwoInfo _itemLeft, _itemRight;
    private const int girlPouring = 0, girlIdle = 1, girlNotWater = 2;
    private Sequence _sqGirl;
    private Tween _twChoose;

    public void OnPointerClick(PointerEventData eventData)
    {
        var pos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(pos);
        _posClick = pos.x;
        if(!_isChoose) return;
        var charPlaying = characterInfos.Find(c => c.isPlaying);
        var indexAnim = _posClick > 0 ? (int)ItemAnimTwo.IdleNotBoxRight : (int)ItemAnimTwo.IdleNotBoxLeft;
        var itemPlay = _posClick > 0 ? _itemRight : _itemLeft;
        charPlaying.character.SetAnimChar(indexAnim, true);
        if (_posClick > 0)
        {
            charPlaying.character.Cast<CharLevelTwo>().ChooseRight(itemPlay.sprItem);
            charPlaying.character.SetAnimChar((int)ItemAnimTwo.IdleNotBoxRight, true);
        }
        else
        {
            charPlaying.character.Cast<CharLevelTwo>().ChooseLeft(itemPlay.sprItem);
            charPlaying.character.SetAnimChar((int)ItemAnimTwo.IdleNotBoxLeft, true);
        }
        Debug.Log($"item choose: {itemPlay.typePlay}");
        foreach (var a in itemPlay.itemAnim)
        {
            Debug.Log($"item choose play: {a}");
        }
        _isChoose = false;
        _twChoose?.Kill();
        _twChoose = DOVirtual.DelayedCall(1.33f, () => ShowAnimReceiveTypePlay((int)itemPlay.typePlay));
    }
    
    [Button]
    public override void StartPlay()
    {
        _progress = 0;
        Signals.Get<UpdateProgressSignals>().Dispatch(_progress, characterInfos.Count);
        levelTwoData.animPlayTwoInfos.ForEach(t => t.isChoose = false);
        InitCharacter();
    }
    public override void InitCharacter()
    {
        InitCharacterPlay();
        var charToPlaying = characterInfos[Random.Range(0, characterInfos.Count)];
        charToPlaying.isPlaying = true;
        foreach (var c in characterInfos)
        {
            c.character.Cast<CharLevelTwo>().HideBox();
        }
        
        SwapInitPosCharacter(charToPlaying, characterInfos.Find(c => c.rect.localScale.x > 0.5f));
    }
    
    public override void SwapInitPosCharacter(CharacterInfo characterToPlaying, CharacterInfo characterToWait)
    {
        characterToWait.isPlaying = false;
        characterToPlaying.isPlaying = true;
        if (characterToPlaying != characterToWait)
        {
            characterToPlaying.character.Cast<CharLevelTwo>().SetSkeletonPlay();
            characterToWait.character.Cast<CharLevelTwo>().SetSkeletonWait();  
            characterToWait.character.AnimInitIdleWait(characterToPlaying, (int)ItemAnimLevelOne.Idle);
            characterToPlaying.character.AnimInitIdlePlay((int)ItemAnimTwo.Idle);
        }
        
        girlPouringWater.transform.SetSiblingIndex(characterInfos.Count - 1);
        _isChoose = true;
        SetBoxItem();
    }
    public override void ShowAnimReceiveTypePlay(int typePlay)
    {
        var charPlaying = characterInfos.Find(c => c.isPlaying);
        charPlaying.character.Cast<CharLevelTwo>().HideBox(); 
        charPlaying.isPlayed = true;
        Debug.Log($"show anim typeFood: {typePlay}");
        var itemAnimLevelOnes = levelTwoData.animPlayTwoInfos
            .Find(a => a.typePlay == (TypePlayTwoChoose)typePlay).itemAnim.Select(i => (int)i).ToArray();
        GirlPouringWater();
        charPlaying.character.ShowAnimReceiveResultChoose(itemAnimLevelOnes , () => 
            SwapPosCharPlaying(charPlaying, FindCharPlay()));
    }
    private void GirlPouringWater()
    {
        int[] itemAnimLevelOnes = { girlPouring, girlNotWater, girlIdle };
        _sqGirl?.Kill();
        _sqGirl = DOTween.Sequence();
        var time = 0f;
        var item = girlPouringWater.SkeletonData.Animations.Items;
        for (int i = 0; i < itemAnimLevelOnes.Length; i++)
        {
            var i1 = i;
            var time1 = time;
            _sqGirl.AppendInterval(time)
                .AppendCallback(() =>
                {
                    Debug.Log($"show anim receive food: {item[itemAnimLevelOnes[i1]].Duration}-{time1}-{item[(int)itemAnimLevelOnes[i1]].Name}");
                    SetAnimChar(itemAnimLevelOnes[i1], false);
                    water.gameObject.SetActive(i1 == girlPouring);
                    if (i1 == girlPouring)
                    {
                        water.transform.SetAsLastSibling();
                        water.SetAnimSkeleton(0, true);
                    }
                });
            time = item[(int)itemAnimLevelOnes[i1]].Duration;
        }
    }
    public void SetAnimChar(int indexAnim, bool loop)
    {
        girlPouringWater.startingAnimation = girlPouringWater.SkeletonData.Animations.Items[indexAnim].Name;
        girlPouringWater.startingLoop = loop;
        girlPouringWater.Initialize(true);
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
        charPlaying.character.Cast<CharLevelTwo>().SetSkeletonWait();
        charPlaying.character.ShowAnimToBackIdle(charToPLay, (int)ItemAnimLevelOne.Idle);
        charToPLay.isPlaying = true;
        charToPLay.character.Cast<CharLevelTwo>().HideBox(); 
        charToPLay.character.ShowAnimMoveToPlayIdle((int)ItemAnimLevelOne.Idle, () => charToPLay.character.Cast<CharLevelTwo>().SetSkeletonPlay());
          
        girlPouringWater.transform.SetSiblingIndex(characterInfos.Count - 1);
        _isChoose = true;
        SetBoxItem();
    }

    private void SetBoxItem()
    {
        _itemLeft = levelTwoData.animPlayTwoInfos.Where(c => !c.isChoose).ToList()[
            Random.Range(0, levelTwoData.animPlayTwoInfos.Count(c => !c.isChoose))];
        _itemLeft.isChoose = true;
        if (_itemLeft.isWin)
        {
            _itemRight = levelTwoData.animPlayTwoInfos.Where(c => !c.isChoose && !c.isWin).ToList()[
                Random.Range(0, levelTwoData.animPlayTwoInfos.Count(c => !c.isChoose && !c.isWin))];
        }
        else
        {
            _itemRight = levelTwoData.animPlayTwoInfos.Where(c => !c.isChoose && c.isWin).ToList()[
                Random.Range(0, levelTwoData.animPlayTwoInfos.Count(c => !c.isChoose && c.isWin))];
        }

        _itemRight.isChoose = true;
    }
    public override void UpdateProgress()
    {
        _progress++;
        Signals.Get<UpdateProgressSignals>().Dispatch(_progress, characterInfos.Count);
    }
}