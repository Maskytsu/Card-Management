using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Board : MonoBehaviour, IDropHandler
{
    [SerializeField] private RectTransform _boardTransform;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<Card>() != null)
        {
            Card droppedCard = eventData.pointerDrag.GetComponent<Card>();
            droppedCard.CardTransform.position = _boardTransform.position;

            StartCoroutine(DiscardCard(droppedCard));
        }
    }

    private IEnumerator DiscardCard(Card card)
    {
        GameManager.Instance.SetInputEnabled(false);

        yield return new WaitForSeconds(0.75f);
        GameView.Instance.PlayersHand.CardsInHand.Remove(card);
        card.gameObject.SetActive(false);
        GameView.Instance.DiscardPile.AddCardToPile(card);

        GameManager.Instance.SetInputEnabled(true);
    }
}
