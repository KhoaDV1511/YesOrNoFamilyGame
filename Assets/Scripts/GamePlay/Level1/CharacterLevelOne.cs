using System;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class CharacterLevelOne : MonoBehaviour
{
    [SerializeField] private SkeletonGraphic skeleton;
    [SerializeField] private Vector3 posPlay;
    private RectTransform rect => skeleton.GetComponent<RectTransform>();
    private float _scalePlay = 0.6f, _scaleIdle = 0.4f;
    private Sequence _sqCharacter;

    [Button]
    private void SetAnim(int index)
    {
        int[] itemAnimLevelOnes = GlobalDataManager.Ins.levelOneData.animFoodInfos[index].itemAnimLevelOnes
            .Select(i => (int)i).ToArray();
        _sqCharacter?.Kill();
        _sqCharacter = DOTween.Sequence();
        var time = 0f;
        var item = skeleton.SkeletonData.Animations.Items;
        for (int i = 0; i < itemAnimLevelOnes.Length; i++)
        {
            var i1 = i;
            var time1 = time;
            _sqCharacter.AppendInterval(time)
                .AppendCallback(() =>
                {
                    Debug.Log($"show anim receive food: {item[itemAnimLevelOnes[i1]].Duration}-{time1}-{item[(int)itemAnimLevelOnes[i1]].Name}");
                    SetAnimChar(itemAnimLevelOnes[i1], false);
                });
            time = item[(int)itemAnimLevelOnes[i1]].Duration;
        }
    }
    public void ShowAnimReceiveResultChoose(int[] itemAnim, Action complete)
    {
        _sqCharacter?.Kill();
        _sqCharacter = DOTween.Sequence();
        var time = 0f;
        var item = skeleton.SkeletonData.Animations.Items;
        for (int i = 0; i < itemAnim.Length; i++)
        {
            var i1 = i;
            _sqCharacter.AppendInterval(time)
                .AppendCallback(() =>
                {
                    Debug.Log($"show anim receive food: {item[itemAnim[i1]].Name}");
                    SetAnimChar(itemAnim[i1], false);
                    if (i1 >= itemAnim.Length - 1)
                    {
                        DOVirtual.DelayedCall(item[itemAnim[i1]].Duration, complete.Invoke);
                    }
                });
            time = item[itemAnim[i1]].Duration;
        }
    }

    public void ShowAnimMoveToPlayIdle(int indexAnim)
    {
        _sqCharacter?.Kill();
        rect.SetAsLastSibling();
        _sqCharacter = DOTween.Sequence().Append(rect.DOAnchorPos(posPlay, 0.5f).From(rect.anchoredPosition))
            .Join(rect.DOScale(_scalePlay, 0.5f).From(_scaleIdle))
            .AppendCallback(() =>
            {
                SetAnimChar(indexAnim, true);
            });
    }
    public void ShowAnimToBackIdle(CharacterInfo characterInfo, int indexAnim)
    {
        _sqCharacter?.Kill();
        var posBackHalf = characterInfo.rect.anchoredPosition.x > 0 ? new Vector2(700, characterInfo.rect.anchoredPosition.y / 2) 
            : new Vector2(-700, characterInfo.rect.anchoredPosition.y / 2);
        _sqCharacter = DOTween.Sequence()
            .AppendCallback(() =>
            {
                SetAnimChar(indexAnim, true);
            })
            .Append(rect.DOAnchorPos(posBackHalf, 0.5f).From(rect.anchoredPosition))
            .Join(rect.DOScale(_scaleIdle, 1f).From(_scalePlay))
            .Append(rect.DOAnchorPos(characterInfo.rect.anchoredPosition, 0.5f));
    }
    
    public void AnimInitIdleWait(CharacterInfo characterInfo, int indexAnim)
    {
        rect.localScale = Vector3.one * _scaleIdle;
        rect.anchoredPosition = characterInfo.rect.anchoredPosition;
        rect.SetSiblingIndex(characterInfo.character.rect.GetSiblingIndex());
        SetAnimChar(indexAnim, true);
    }
    public void AnimInitIdlePlay(int indexAnim)
    {
        rect.localScale = Vector3.one * _scalePlay;
        rect.anchoredPosition = posPlay;
        SetAnimChar(indexAnim, true);
        rect.SetAsLastSibling();
    }

    public void SetAnimChar(int indexAnim, bool loop)
    {
        skeleton.startingAnimation = skeleton.SkeletonData.Animations.Items[indexAnim].Name;
        skeleton.startingLoop = loop;
        skeleton.Initialize(true);
    }
}