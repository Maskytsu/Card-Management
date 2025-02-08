using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Card : MonoBehaviour, 
    IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform CardTransform { get; private set; }
    public CanvasGroup CardCanvasGroup { get; private set; }

    [ReadOnly] public Transform CurrentCardSlot;

    public Action OnPlayAnimationEnd;

    private Canvas MainCanvas => GameView.Instance.MainCanvas;

    private Sequence _highlightCardSeq;
    private Tween _moveToSlotTween;

    private void Awake()
    {
        CardTransform = GetComponent<RectTransform>();
        CardCanvasGroup = GetComponent<CanvasGroup>();
    }

    public abstract void CardPlayAnimation();

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_moveToSlotTween.IsActive()) _moveToSlotTween.Kill();
        CardCanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        CardTransform.anchoredPosition += eventData.delta / MainCanvas.scaleFactor;
    }

    //it is called after OnDrop on Board object
    public void OnEndDrag(PointerEventData eventData)
    {
        CardCanvasGroup.blocksRaycasts = true;
        _moveToSlotTween = CardTransform.DOMove(CurrentCardSlot.position, 1f);
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
        _highlightCardSeq.Append(CardTransform.DOScale(scale, 0.25f));
    }
}
