using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class PieceMove : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Transform center;
    public TypeFood typeFood;
    [SerializeField] private RectTransform rectTransform;
    private Canvas _canvas;
    private Action<int> typeFoodOpen;
    private Vector2 _posInit;
    private Sequence _sqBackPiece;
    public bool isChoose;

    private void Start()
    {
        _canvas = FindObjectOfType<Canvas>();
        _posInit = rectTransform.anchoredPosition;
    }

    public void InitAddListener(Action<int> type)
    {
        gameObject.Show();
        isChoose = true;
        typeFoodOpen = type;
        if(_posInit != Vector2.zero)
            rectTransform.anchoredPosition = _posInit;
        Debug.Log("InitAddListener");
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("PointerEventData on OnPointerClick");
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("PointerEventData on begin drag");
        transform.SetAsLastSibling();
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("PointerEventData on end drag");
        var distancePiece = Vector3.Distance(center.position, transform.parent.position);
        if (distancePiece >= 2f && isChoose)
        {
            Debug.Log($"{typeFood}-{distancePiece}");
            typeFoodOpen.Invoke((int)typeFood);
            gameObject.Hide();
        }
        else
        {
            _sqBackPiece?.Kill();
            _sqBackPiece = DOTween.Sequence()
                .Append(rectTransform.DOAnchorPos(_posInit, 0.5f).From(rectTransform.anchoredPosition));
        }
    }
}