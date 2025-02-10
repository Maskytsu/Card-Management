using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayersHand : MonoBehaviour, IPointerMoveHandler
{
    public int AmountOfCardsInHand => _cardsInHand.Count;

    //this contains instantiated card game objects
    [ReadOnly, SerializeField] private List<Card> _cardsInHand = new();

    [ReadOnly, SerializeField] private List<CardSlot> _cardSlots;

    [SerializeField] private Transform _cardSlotsParent;

    private readonly float _slotsOffsetY = -80f;
    private readonly float _baseDistanceBetweenSlots = 150f;
    private readonly int _amountOfSlotsToShrinkDistance = 7;
    private readonly float _shrinkAmount = 8f;
    private readonly float _minDistanceBetweenSlots = 20f;

    private Vector3 _currentDistanceBetweenSlots;

    public void OnPointerMove(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<Card>() != null)
        {
            Card draggedCard = eventData.pointerDrag.GetComponent<Card>();
            GetCardSlot(draggedCard, out CardSlot draggedCardSlot, out int? draggedCardSlotIndex);

            if (draggedCardSlot == null) return;

            float cardPosX = draggedCard.transform.position.x;
            float slotPosX = draggedCardSlot.CardSlotTransform.position.x;
            float cardToSlotDistanceX = cardPosX - slotPosX;

            if (cardToSlotDistanceX > _currentDistanceBetweenSlots.x
                && draggedCardSlotIndex.Value < _cardSlots.Count - 1)
            {
                SwapCards(draggedCardSlotIndex.Value, draggedCardSlotIndex.Value + 1);
            }

            if (cardToSlotDistanceX < -_currentDistanceBetweenSlots.x
                && draggedCardSlotIndex.Value > 0)
            {
                SwapCards(draggedCardSlotIndex.Value, draggedCardSlotIndex.Value - 1);
            }
        }
    }

    public void RemoveCardFromHand(Card card)
    {
        _cardsInHand.Remove(card);

        GenerateCardSlots(AmountOfCardsInHand);
        MoveAllAssignedCardsToSlots();
    }

    public IEnumerator DrawCards(List<Card> cards)
    {
        GameView.Instance.EndTurnOrGameButton.SetInteractable(false);

        WaitForSeconds waitForSeconds = new(0.25f);

        GenerateCardSlots(AmountOfCardsInHand + cards.Count);
        MoveAllAssignedCardsToSlots();

        foreach (Card card in cards)
        {
            DOTween.Kill(card.transform, Card.KillableMoveCardTweensID);

            _cardsInHand.Add(card);
            _cardSlots[_cardsInHand.Count - 1].AssignCardToSlot(card);

            float drawingDuration = 1f;
            Tween scaleCardTween = card.transform.DOScale(1f, drawingDuration);
            card.transform.DOLocalMove(Vector3.zero, drawingDuration).SetId(Card.KillableMoveCardTweensID);

            scaleCardTween.onComplete += () => { card.enabled = true; };

            GameView.Instance.DeckPile.MinusOneFromDisplayedNumer();

            yield return waitForSeconds;
        }

        GameView.Instance.EndTurnOrGameButton.SetInteractable(true);
    }

    private void GenerateCardSlots(int numberOfSlots)
    {
        if (numberOfSlots == 0) return;

        CalculateCurrentDistanceBetweenSlots(numberOfSlots);
        List<CardSlot> generatedSlots = new();
        Vector3 startingSlotPos;

        if (numberOfSlots % 2 == 0)
        {
            //calculate starting left position for even number of slots
            Vector3 centerLeftPos = - (_currentDistanceBetweenSlots / 2);
            startingSlotPos = centerLeftPos - (((numberOfSlots / 2) - 1) * _currentDistanceBetweenSlots);
        }
        else
        {
            //calculate starting left position for odd number of slots
            startingSlotPos = -(((numberOfSlots - 1) / 2)) * _currentDistanceBetweenSlots;
        }

        for (int i = 0; i < numberOfSlots; i++)
        {
            GameObject newSlotGameObject = new("Slot " + i);
            newSlotGameObject.transform.SetParent(_cardSlotsParent);
            newSlotGameObject.transform.localPosition = startingSlotPos + (i * _currentDistanceBetweenSlots);
            newSlotGameObject.transform.localPosition += Vector3.up * _slotsOffsetY;

            CardSlot newSlot = new(newSlotGameObject.transform);
            generatedSlots.Add(newSlot);
        }

        List<CardSlot> previousCardSlots = _cardSlots.ToList();
        _cardSlots = generatedSlots;

        AssignCardsToSlots();

        //destroy old slots after assigning old cards to new slots
        foreach (CardSlot cardSlot in previousCardSlots)
        {
            Destroy(cardSlot.CardSlotTransform.gameObject);
        }
    }

    private void CalculateCurrentDistanceBetweenSlots(int numberOfSlots)
    {
        Vector3 distanceBetweenSlots = Vector3.right * _baseDistanceBetweenSlots;
        if (numberOfSlots > _amountOfSlotsToShrinkDistance)
        {
            int excessOfHandSize = (numberOfSlots - _amountOfSlotsToShrinkDistance);
            distanceBetweenSlots -= _shrinkAmount * excessOfHandSize * Vector3.right;

            if (distanceBetweenSlots.x < _minDistanceBetweenSlots)
            {
                distanceBetweenSlots.x = _minDistanceBetweenSlots;
            }
        }

        _currentDistanceBetweenSlots = distanceBetweenSlots;
    }

    private void SwapCards(int slotIndex1, int slotIndex2)
    {
        CardSlot cardSlot1 = _cardSlots[slotIndex1];
        CardSlot cardSlot2 = _cardSlots[slotIndex2];

        Card card1 = cardSlot1.CardInSlot;
        Card card2 = cardSlot2.CardInSlot;

        _cardsInHand[slotIndex1] = card2;
        _cardsInHand[slotIndex2] = card1;

        cardSlot1.AssignCardToSlot(card2);
        cardSlot2.AssignCardToSlot(card1);

        MoveCardToLocalCenter(card1);
        MoveCardToLocalCenter(card2);
    }

    private void AssignCardsToSlots()
    {
        for (int i = 0; i < _cardsInHand.Count; i++)
        {
            _cardSlots[i].AssignCardToSlot(_cardsInHand[i]);
        }
    }

    private void MoveAllAssignedCardsToSlots()
    {
        foreach (Card card in _cardsInHand)
        {
            MoveCardToLocalCenter(card);
        }
    }

    private void MoveCardToLocalCenter(Card card)
    {
        if (!card.IsBeingDragged && card.transform.localPosition != Vector3.zero)
        {
            DOTween.Kill(card.transform, Card.KillableMoveCardTweensID);
            card.transform.DOLocalMove(Vector3.zero, 0.25f).SetId(Card.KillableMoveCardTweensID);
        }
    }

    private void GetCardSlot(Card card, out CardSlot cardSlot, out int? cardSlotIndex)
    {
        cardSlot = null;
        cardSlotIndex = null;

        for (int i = 0; i < _cardSlots.Count; i++)
        {
            if (_cardSlots[i].CardInSlot == card)
            {
                cardSlot = _cardSlots[i];
                cardSlotIndex = i;
                return;
            }
        }

        Debug.LogError("Card not assigned to any slot!");
    }

    [Serializable]
    private class CardSlot
    {
        public Transform CardSlotTransform { get; private set; }

        public Card CardInSlot;

        public CardSlot(Transform cardSlotTransform)
        {
            CardSlotTransform = cardSlotTransform;
        }

        public void AssignCardToSlot(Card card)
        {
            CardInSlot = card;
            card.transform.SetParent(CardSlotTransform);
        }
    }
}