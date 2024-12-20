using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class AnimPopup : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private CanvasGroup canvasGroup;
    private Sequence mainSequence;
    public float startSize = 0.7f, middleSize = 1.05f, endSize = 1;
    public float firstTime = 0.2f, secondTime = 0.1f;
    
    private void OnValidate()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void OnEnable()
    {
        mainSequence?.Kill();
        canvasGroup.blocksRaycasts = true;
        mainSequence = DOTween.Sequence();
    
        content.localScale = Vector3.one * startSize;
        canvasGroup.alpha = 0;
        mainSequence.Append(content.DOScale(middleSize, firstTime))
            .Join(canvasGroup.DOFade(1, firstTime))
            .Append(content.DOScale(endSize, secondTime));
    }
    
    public void OnClose()
    {
        mainSequence?.Kill();
        content.localScale = Vector3.one * endSize;
        mainSequence = DOTween.Sequence();
        
        canvasGroup.alpha = 1;
        mainSequence.Append(content.DOScale(middleSize, firstTime))
            .Append(content.DOScale(startSize + 0.1f, firstTime))
            .Join(canvasGroup.DOFade(0, firstTime)).AppendCallback(gameObject.Hide);
    }
}
