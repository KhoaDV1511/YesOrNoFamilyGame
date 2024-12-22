using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelFiveMediator : BaseLevel, IPointerClickHandler
{
    [SerializeField] private GameObject objTable;
    private LevelFiveData levelFiveData => GlobalDataManager.Ins.levelFiveData;
    private bool _isChoose;
    private int _progress;
    
    [Button]
    public override void StartPlay()
    {
        _progress = 0;
        Signals.Get<UpdateProgressSignals>().Dispatch(_progress, characterInfos.Count);
        objTable.Show();
        InitCharacter();
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
            characterToPlaying.character.Cast<CharLevelChangeSkeleton>().SetSkeletonPlay((int)ItemAnimLevelOne.Idle);
            characterToWait.character.Cast<CharLevelChangeSkeleton>().SetSkeletonWait((int)ItemAnimFive.Idle);  
            characterToWait.character.AnimInitIdleWait(characterToPlaying, (int)ItemAnimLevelOne.Idle);
            characterToPlaying.character.AnimInitIdlePlay((int)ItemAnimFive.Idle);
        }
        _isChoose = true;
    }

    public override void ShowAnimReceiveTypePlay(int typePlay)
    {
        var charPlaying = characterInfos.Find(c => c.isPlaying);
        charPlaying.isPlayed = true;
        Debug.Log($"show anim typeFood: {typePlay}");
        var itemAnimLevelOnes = levelFiveData.animPlayFiveInfos
            .Find(a => a.typePlayFiveChoose == (TypePlayFiveChoose)typePlay).itemAnimFives
            .Select(i => (int)i).ToArray();
        if ((TypePlayFiveChoose)typePlay == TypePlayFiveChoose.NuocOt)
        {
            _progress = characterInfos.Count;
        }
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

        if (characterInfos.Count(c => !c.isPlayed) <= 0)
        {
            foreach (var characterInfo in characterInfos)
            {
                characterInfo.isPlayed = false;
            }

            charToPLay = FindCharPlay();
        }
        charPlaying.isPlaying = false;
        charPlaying.character.Cast<CharLevelChangeSkeleton>().SetSkeletonWait((int)ItemAnimLevelOne.Idle); 
        charPlaying.character.ShowAnimToBackIdle(charToPLay, (int)ItemAnimLevelOne.Idle);
        objTable.Hide();
        charToPLay.isPlaying = true;
        charToPLay.character.ShowAnimMoveToPlayIdle((int)ItemAnimLevelOne.Idle, () =>
        {
            charToPLay.character.Cast<CharLevelChangeSkeleton>().SetSkeletonPlay((int)ItemAnimFive.Idle);
            objTable.Show();
        });
        _isChoose = true;
    }

    public override void UpdateProgress()
    {
        //_progress++;
        Signals.Get<UpdateProgressSignals>().Dispatch(_progress, characterInfos.Count);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!_isChoose) return;
        _isChoose = false;
        var randomIndex = characterInfos.Count(c => c.isPlayed) > 2
            ? levelFiveData.animPlayFiveInfos.Count
            : levelFiveData.animPlayFiveInfos.Count - 1;
        ShowAnimReceiveTypePlay(Random.Range(0, levelFiveData.animPlayFiveInfos.Count));
    }
}