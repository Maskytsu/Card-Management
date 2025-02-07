using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public RectTransform CardTransform { get; private set; }

    private CanvasGroup _cardCanvasGroup;

    private Canvas MainCanvas => GameView.Instance.MainCanvas;

    private void Awake()
    {
        CardTransform = GetComponent<RectTransform>();
        _cardCanvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _cardCanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {

        CardTransform.anchoredPosition += eventData.delta / MainCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _cardCanvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }
}
