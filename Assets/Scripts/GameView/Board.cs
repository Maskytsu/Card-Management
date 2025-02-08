using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Board : MonoBehaviour, IDropHandler
{
    private bool _cardOnBoard = false;

    //it is called befor OnEndDrag on Card object
    public void OnDrop(PointerEventData eventData)
    {
        if (_cardOnBoard) return;

        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<Card>() != null)
        {
            Card droppedCard = eventData.pointerDrag.GetComponent<Card>();
            PlayCard(droppedCard);
        }
    }

    private void PlayCard(Card card)
    {
        _cardOnBoard = true;

        card.enabled = false;

        GameView.Instance.PlayersHand.CardsInHand.Remove(card);
        GameView.Instance.DiscardPile.CardsInPile.Add(card);

        card.CardTransform.SetParent(transform);
        Tween moveTween = card.transform.DOLocalMove(Vector3.zero, 0.25f);
        moveTween.onComplete += card.CardPlayAnimation;

        card.OnPlayAnimationEnd += () => DiscardCard(card);
    }

    private void DiscardCard(Card card)
    {
        _cardOnBoard = false;

        card.CardTransform.SetParent(GameView.Instance.DiscardPile.CardsParent);

        Sequence discardingSeq = DOTween.Sequence();
        discardingSeq.Append(card.CardTransform.DOScale(0f, 1f));
        discardingSeq.Join(card.CardTransform.DOMove(GameView.Instance.DiscardPile.transform.position, 1f));

        discardingSeq.onComplete += () =>
        {
            GameView.Instance.DiscardPile.AddOneCardToDisplayedNumer();
            card.gameObject.SetActive(false);
        };
    }
}
