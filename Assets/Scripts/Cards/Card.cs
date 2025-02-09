using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Card : MonoBehaviour, 
    IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action OnPlayAnimationEnd;

    public bool IsBeingDragged { get; private set; } = false;

    private RectTransform _cardRectTransform;
    private CanvasGroup _cardCanvasGroup;

    private Sequence _highlightCardSeq;
    private Tween _moveToSlotTween;

    private void Awake()
    {
        _cardRectTransform = GetComponent<RectTransform>();
        _cardCanvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// It needs to invoke <see cref="OnPlayAnimationEnd"/> on the end of animation.
    /// </summary>
    public abstract void CardPlayAnimation();

    public void OnBeginDrag(PointerEventData eventData)
    {
        IsBeingDragged = true;
        _cardCanvasGroup.blocksRaycasts = false;

        if (_moveToSlotTween.IsActive()) _moveToSlotTween.Kill();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _cardRectTransform.anchoredPosition += eventData.delta / GameView.Instance.MainCanvas.scaleFactor;
    }

    //it is called after OnDrop on Board object
    public void OnEndDrag(PointerEventData eventData)
    {
        IsBeingDragged = false;
        _cardCanvasGroup.blocksRaycasts = true;

        _moveToSlotTween = transform.DOLocalMove(Vector3.zero, 1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.dragging) HighlightCard(1.25f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!eventData.dragging) HighlightCard(1f);
    }

    protected void EndAnimation()
    {
        OnPlayAnimationEnd?.Invoke();
    }

    private void HighlightCard(float scale)
    {
        if (_highlightCardSeq.IsActive()) _highlightCardSeq.Kill();

        _highlightCardSeq = DOTween.Sequence();
        _highlightCardSeq.Append(transform.DOScale(scale, 0.25f));
    }
}
