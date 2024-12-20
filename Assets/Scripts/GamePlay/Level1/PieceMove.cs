using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PieceMove : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Transform center;
    private RectTransform _rectTransform;
    private Canvas _canvas;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = FindObjectOfType<Canvas>();
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
            Debug.Log($"{distancePiece}");
        }
    }
}