using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersHand : MonoBehaviour
{
    public event Action OnCardsDrawn;

    public int AmountOfCardsInHand => _cardsInHand.Count;

    //this contains instantiated card game objects
    [ReadOnly, SerializeField] private List<Card> _cardsInHand = new();

    [ReadOnly, SerializeField] private List<CardSlot> _cardSlots;

    [SerializeField] private Transform _cardSlotsParent;

    private readonly float _baseSlotsDistance = 150f;
    private readonly int _amountOfSlotsToShrinkDistance = 7;
    private readonly float _shrinkAmount = 7.5f;
    private readonly float _minSlotsDistance = 20f;

    public void RemoveCardFromHand(Card card)
    {
        _cardsInHand.Remove(card);

        GenerateCardSlots(AmountOfCardsInHand);
        MoveCardsToSlots();
    }

    public IEnumerator DrawCards(List<Card> cards)
    {
        WaitForSeconds waitForSeconds = new(0.25f);

        GenerateCardSlots(AmountOfCardsInHand + cards.Count);
        MoveCardsToSlots();

        foreach (Card card in cards)
        {
            card.enabled = false;
            _cardsInHand.Add(card);

            Transform cardSlot = _cardSlots[_cardsInHand.Count - 1].CardSlotTransform.transform;
            card.transform.SetParent(cardSlot);

            Sequence drawingSeq = DOTween.Sequence();
            drawingSeq.Append(card.transform.DOScale(1f, 1f));
            drawingSeq.Join(card.transform.DOLocalMove(Vector3.zero, 1f));

            drawingSeq.onComplete += () => { card.enabled = true; };

            GameView.Instance.DeckPile.MinusOneFromDisplayedNumer();

            yield return waitForSeconds;
        }

        OnCardsDrawn?.Invoke();
    }

    private void MoveCardsToSlots()
    {
        foreach (Card card in _cardsInHand)
        {
            if (!card.IsBeingDragged)
            {
                card.transform.DOLocalMove(Vector3.zero, 0.25f);
            }
        }
    }

    private void GenerateCardSlots(int numberOfSlots)
    {
        if (numberOfSlots == 0) return;

        Vector3 distanceBetweenSlots = CalculateSlotsDistance(numberOfSlots);

        List<CardSlot> generatedSlots = new();
        Vector3 startingSlotPos;

        if (numberOfSlots % 2 == 0)
        {
            Vector3 centerLeftPos = - (distanceBetweenSlots / 2);
            startingSlotPos = centerLeftPos - (((numberOfSlots / 2) - 1) * distanceBetweenSlots);
        }
        else
        {
            startingSlotPos = -(((numberOfSlots - 1) / 2)) * distanceBetweenSlots;
        }

        for (int i = 0; i < numberOfSlots; i++)
        {
            GameObject newSlotGameObject = new("Slot " + i);
            newSlotGameObject.transform.SetParent(_cardSlotsParent);
            newSlotGameObject.transform.localPosition = startingSlotPos + (i * distanceBetweenSlots);
            CardSlot newSlot = new(newSlotGameObject.transform);
            generatedSlots.Add(newSlot);
        }

        List<CardSlot> previousCardSlots = _cardSlots;
        _cardSlots = generatedSlots;

        AssignCardsToSlots();

        foreach (CardSlot cardSlot in previousCardSlots)
        {
            Destroy(cardSlot.CardSlotTransform.gameObject);
        }
    }

    private void AssignCardsToSlots()
    {
        for (int i = 0; i < _cardsInHand.Count; i++)
        {
            _cardSlots[i].CardInSlot = _cardsInHand[i];
            _cardsInHand[i].transform.SetParent(_cardSlots[i].CardSlotTransform);
        }
    }

    private Vector3 CalculateSlotsDistance(int numberOfSlots)
    {
        Vector3 distanceBetweenSlots = Vector3.right * _baseSlotsDistance;
        if (numberOfSlots > _amountOfSlotsToShrinkDistance)
        {
            int excessOfHandSize = (numberOfSlots - _amountOfSlotsToShrinkDistance);
            distanceBetweenSlots -= _shrinkAmount * excessOfHandSize * Vector3.right;

            if (distanceBetweenSlots.x < _minSlotsDistance)
            {
                distanceBetweenSlots.x = _minSlotsDistance;
            }
        }

        return distanceBetweenSlots;
    }

    [Serializable]
    private class CardSlot
    {
        public Transform CardSlotTransform;
        public Card CardInSlot;

        public CardSlot(Transform cardSlotTransform)
        {
            CardSlotTransform = cardSlotTransform;
        }
    }
}