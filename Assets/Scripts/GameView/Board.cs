using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

public class Board : MonoBehaviour, IDropHandler
{
    [ReadOnly, SerializeField] private Card _cardOnBoard;

    //it is called before OnEndDrag on Card object
    public void OnDrop(PointerEventData eventData)
    {
        if (_cardOnBoard != null) return;

        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<Card>() != null)
        {
            Card droppedCard = eventData.pointerDrag.GetComponent<Card>();
            PlayCard(droppedCard);
        }
    }

    private void PlayCard(Card card)
    {
        //this is preventing card from being dragged
        //it also makes that OnEndDrag and OnPointerExit events is not called on the card
        card.enabled = false;

        //these two lines fixes potential buggs caused by not calling OnEndDrag event on the card
        GameManager.Instance.SetCursorToBasic();
        card.SetIsBeingDragged(false);

        GameView.Instance.PlayersHand.RemoveCardFromHand(card);

        _cardOnBoard = card;
        card.transform.SetParent(transform);

        Tween moveCardToBoardTween = card.transform.DOLocalMove(Vector3.zero, 0.25f);
        moveCardToBoardTween.onComplete += card.PlayCard;

        card.OnPlayAnimationEnd += () => DiscardCard(card);
    }

    private void DiscardCard(Card card)
    {
        _cardOnBoard = null;

        GameView.Instance.DiscardPile.AddCardToPile(card);
        GameView.Instance.DiscardPile.SetCardParent(card);

        Sequence discardingSeq = DOTween.Sequence();
        discardingSeq.Append(card.transform.DOScale(0f, 1f));
        discardingSeq.Join(card.transform.DOMove(GameView.Instance.DiscardPile.transform.position, 1f));

        discardingSeq.onComplete += () =>
        {
            GameView.Instance.DiscardPile.AddOneToDisplayedNumer();
            card.gameObject.SetActive(false);
        };
    }
}