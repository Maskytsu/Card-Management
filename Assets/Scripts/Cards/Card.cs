using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class Card : MonoBehaviour, 
    IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static readonly string KillableMoveCardTweensID = "KillableMoveCardTween";

    public event Action OnPlayAnimationEnd;

    public bool IsBeingDragged { get; private set; } = false;

    private RectTransform _cardRectTransform;
    private CanvasGroup _cardCanvasGroup;
    private Outline _cardOutline;

    private Sequence _highlightCardSeq;

    private void Awake()
    {
        _cardRectTransform = GetComponent<RectTransform>();
        _cardCanvasGroup = GetComponent<CanvasGroup>();
        _cardOutline = transform.GetComponentInChildren<Outline>();
    }

    private void OnEnable()
    {
        _cardCanvasGroup.blocksRaycasts = true;
    }

    private void OnDisable()
    {
        _cardCanvasGroup.blocksRaycasts = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        IsBeingDragged = true;
        _cardCanvasGroup.blocksRaycasts = false;

        GameManager.Instance.SetCursorToHolding();
        DOTween.Kill(transform, KillableMoveCardTweensID);
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

        GameManager.Instance.SetCursorToBasic();
        transform.DOLocalMove(Vector3.zero, 1f).SetId(KillableMoveCardTweensID);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.dragging) HighlightCard(1.25f, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!eventData.dragging) HighlightCard(1f, 0f);
    }

    public void SetIsBeingDragged(bool value)
    {
        IsBeingDragged = value;
    }

    public void PlayCard()
    {
        _cardOutline.DOColor(new Color(0, 1f, 1f, 1f), 0.15f);

        Sequence animationSeq = DOTween.Sequence();
        animationSeq.onComplete += () => OnPlayAnimationEnd?.Invoke();

        CardPlayAnimation(animationSeq);
    }

    /// <summary>
    /// Animation should be created as part of given Sequence <paramref name="animationSeq"/>.
    /// </summary>
    protected abstract void CardPlayAnimation(Sequence animationSeq);

    private void HighlightCard(float scale, float fade)
    {
        if (_highlightCardSeq.IsActive()) _highlightCardSeq.Kill();

        _highlightCardSeq = DOTween.Sequence();
        _highlightCardSeq.Append(transform.DOScale(scale, 0.25f));
        _highlightCardSeq.Join(_cardOutline.DOFade(fade, 0.15f));
    }
}