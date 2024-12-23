using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using  Sirenix.OdinInspector;

public class ToastItem : MonoBehaviour
{
    [SerializeField] private TMP_Text txtMessage;
    private Action _onToastClick;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Sequence TweenSequence { get; set; }

    private Sequence OpenTween => DOTween.Sequence()
        .Append(_rectTransform.DOSizeDelta(new Vector2(_rectTransform.sizeDelta.x, 50), 0.3f).From(new Vector2(_rectTransform.sizeDelta.x, 0))
            .OnUpdate(RebuildLayout))
        .Join(_canvasGroup.DOFade(1f, 0.3f).From(0f));

    private Sequence CloseTween => DOTween.Sequence()
        .Append(_rectTransform.DOSizeDelta(Vector2.zero, 0.3f).OnUpdate(RebuildLayout))
        .Join(_canvasGroup.DOFade(0f, 0.3f))
        .OnComplete(() => LeanPool.Despawn(this));

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Set(string text, float time = 2f, Action onToastClick = null)
    {
        gameObject.Show();
        txtMessage.SetText(text);
        _rectTransform.SetAsFirstSibling();
        _rectTransform.sizeDelta = new Vector2((LayoutUtility.GetPreferredWidth(txtMessage.rectTransform) + 25),
        _rectTransform.sizeDelta.y);
        TweenSequence?.Kill();
        TweenSequence = OpenTween.AppendInterval(time).Append(CloseTween).Play();
        _onToastClick = onToastClick;
    }

    public IEnumerator ShowUntil(string text, Func<bool> waitUntil)
    {
        gameObject.Show();
        txtMessage.SetText(text);
        _rectTransform.SetAsFirstSibling();
        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x,
            LayoutUtility.GetPreferredHeight(txtMessage.rectTransform) + 25);
        TweenSequence?.Kill();
        TweenSequence = OpenTween.Play();
        yield return new WaitUntil(waitUntil);
        TweenSequence?.Kill();
        TweenSequence = CloseTween.Play();
    }

    public void OnToastClick()
    {
        TweenSequence?.Kill();
        TweenSequence = CloseTween.Play();
    }

    private void RebuildLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform.parent as RectTransform);
    }

    private void OnDisable()
    {
        TweenSequence?.Kill();
    }
}