using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Card : MonoBehaviour, 
    IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action OnPlayAnimationEnd;

    public bool IsBeingDragged { get; private set; } = false;

    public static readonly string MoveTweenID = "move";

    private RectTransform _cardRectTransform;
    private CanvasGroup _cardCanvasGroup;
    private Outline _cardOutline;

    private Sequence _highlightCardSeq;
    private Tween _moveToSlotTween;

    private void Awake()
    {
        _cardRectTransform = GetComponent<RectTransform>();
        _cardCanvasGroup = GetComponent<CanvasGroup>();
        _cardOutline = transform.GetComponentInChildren<Outline>();
    }

    /// <summary>
    /// It needs to invoke <see cref="OnPlayAnimationEnd"/> on the end of animation.
    /// </summary>
    public abstract void CardPlayAnimation();

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameManager.Instance.SetCursorToHolding();

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
        GameManager.Instance.SetCursorToBasic();

        IsBeingDragged = false;
        _cardCanvasGroup.blocksRaycasts = true;

        _moveToSlotTween = transform.DOLocalMove(Vector3.zero, 1f).SetId(MoveTweenID);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.dragging) HighlightCard(1.25f, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!eventData.dragging) HighlightCard(1f, 0f);
    }

    protected void EndAnimation()
    {
        OnPlayAnimationEnd?.Invoke();
    }

    private void HighlightCard(float scale, float fade)
    {
        if (_highlightCardSeq.IsActive()) _highlightCardSeq.Kill();

        _highlightCardSeq = DOTween.Sequence();
        _highlightCardSeq.Append(transform.DOScale(scale, 0.25f));
        _highlightCardSeq.Join(_cardOutline.DOFade(fade, 0.15f));
    }
}
