using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PieceMove : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Transform center;
    public TypeFood typeFood;
    private RectTransform _rectTransform;
    private Canvas _canvas;
    private Action<int> typeFoodOpen;
    private Vector2 _posInit;
    private Sequence _sqBackPiece;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = FindObjectOfType<Canvas>();
        _posInit = _rectTransform.anchoredPosition;
    }

    public void InitAddListener(Action<int> type)
    {
        gameObject.Show();
        typeFoodOpen = type;
        Debug.Log("InitAddListener");
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("PointerEventData on OnPointerClick");
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
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
        if (distancePiece >= 2f)
        {
            Debug.Log($"{typeFood}-{distancePiece}");
            typeFoodOpen.Invoke((int)typeFood);
            gameObject.Hide();
        }
        else
        {
            _sqBackPiece?.Kill();
            _sqBackPiece = DOTween.Sequence()
                .Append(_rectTransform.DOAnchorPos(_posInit, 0.5f).From(_rectTransform.anchoredPosition));
        }
    }
}