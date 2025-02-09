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
        //it also makes that the OnEndDrag event is not called on card
        card.enabled = false;

        _cardOnBoard = card;
        card.transform.SetParent(transform);

        GameManager.Instance.SetCursorToBasic();
        GameView.Instance.PlayersHand.RemoveCardFromHand(card);

        Tween moveTween = card.transform.DOLocalMove(Vector3.zero, 0.25f);
        moveTween.onComplete += card.CardPlayAnimation;

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
