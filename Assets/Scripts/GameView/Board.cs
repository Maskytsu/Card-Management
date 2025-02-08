using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

public class Board : MonoBehaviour, IDropHandler
{
    [ReadOnly, SerializeField] private Card _cardOnBoard;

    //it is called befor OnEndDrag on Card object
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
        _cardOnBoard = card;
        card.enabled = false;

        card.transform.SetParent(transform);
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
