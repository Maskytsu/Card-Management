using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersHand : MonoBehaviour
{
    public event Action OnCardsDrawn;

    //this contains instantiated card game objects
    [ReadOnly] public List<Card> CardsInHand = new();

    [SerializeField] private Transform _cardSlotsParent;
    [ReadOnly, SerializeField] private List<CardSlot> _cardSlots;

    private float _baseSlotsDistance = 150f;
    private int _amountOfSlotsToShrinkDistance = 7;
    private float _shrinkAmount = 10f;
    private float _minSlotsDistance = 20f;

    private void Awake()
    {
        _cardSlots = GenerateCardSlots(GameManager.HandSize);
    }

    public IEnumerator DrawCards(List<Card> cards)
    {
        WaitForSeconds waitForSeconds = new(0.25f);
        
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];

            CardsInHand.Add(card);

            card.enabled = false;

            Transform cardSlot = GameView.Instance.PlayersHand._cardSlots[i].Slot.transform;
            card.CurrentCardSlot = cardSlot;
            card.transform.SetParent(cardSlot);

            Sequence drawingSeq = DOTween.Sequence();
            drawingSeq.Append(card.CardTransform.DOScale(1f, 1f));
            drawingSeq.Join(card.CardTransform.DOLocalMove(Vector3.zero, 1f));

            drawingSeq.onComplete += () => { card.enabled = true; };

            GameView.Instance.DeckPile.MinusOneFromDisplayedNumer();

            yield return waitForSeconds;
        }

        OnCardsDrawn?.Invoke();
    }

    private List<CardSlot> GenerateCardSlots(int numberOfSlots)
    {
        if (numberOfSlots == 0) return null;

        Vector3 distanceBetweenSlots = CalculateSlotsDistance(numberOfSlots);

        List<CardSlot> generatedSlots = new();
        Vector3 startingSlotPos;

        if (numberOfSlots % 2 == 0)
        {
            Vector3 centerLeftPos = transform.localPosition - (distanceBetweenSlots / 2);
            startingSlotPos = centerLeftPos - (((numberOfSlots / 2) - 1) * distanceBetweenSlots);
        }
        else
        {
            startingSlotPos = -(((numberOfSlots - 1) / 2)) * distanceBetweenSlots;
        }

        for (int i = 0; i < numberOfSlots; i++)
        {
            GameObject newSlotGameObject = new("Slot " + i);
            newSlotGameObject.transform.parent = _cardSlotsParent;
            newSlotGameObject.transform.localPosition = startingSlotPos + (i * distanceBetweenSlots);
            CardSlot newSlot = new(newSlotGameObject);
            generatedSlots.Add(newSlot);
        }

        return generatedSlots;
    }

    private Vector3 CalculateSlotsDistance(int numberOfSlots)
    {
        Vector3 distanceBetweenSlots = Vector3.right * _baseSlotsDistance;
        if (numberOfSlots > _amountOfSlotsToShrinkDistance)
        {
            int excessOfHandSize = (numberOfSlots - _amountOfSlotsToShrinkDistance);
            distanceBetweenSlots -= excessOfHandSize * Vector3.right * _shrinkAmount;

            if (distanceBetweenSlots.x < _minSlotsDistance) distanceBetweenSlots.x = _minSlotsDistance;
        }

        return distanceBetweenSlots;
    }
}

[Serializable]
public class CardSlot
{
    public GameObject Slot;
    public Card CardInSlot;

    public CardSlot(GameObject slot)
    {
        Slot = slot;
    }
}