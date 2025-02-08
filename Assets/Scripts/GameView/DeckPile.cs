using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DeckPile : MonoBehaviour
{
    public event Action OnCardsDrawn;
    public int AmountOfCardsInPile => _cardsInPile.Count;

    [SerializeField] private TextMeshProUGUI _countDisplayTMP;

    //this contains card prefabs
    [ReadOnly, SerializeField] private List<Card> _cardsInPile;

    private PlayersHand PlayersHand => GameView.Instance.PlayersHand;

    private void Start()
    {
        GameManager.Instance.OnTurnEnd += () => StartCoroutine(DrawCardsToHand());

        SetupDeck();
        StartCoroutine(DrawCardsToHand());
    }

    private void SetupDeck()
    {
        _cardsInPile = GameManager.Instance.ChoosenDeck.Cards.ToList();

        //shuffle deck with Fisher-Yates shuffle algorithm
        for (int i = _cardsInPile.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            (_cardsInPile[i], _cardsInPile[randomIndex]) = (_cardsInPile[randomIndex], _cardsInPile[i]);
        }

        _countDisplayTMP.text = _cardsInPile.Count.ToString();
    }

    private IEnumerator DrawCardsToHand()
    {
        if (_cardsInPile.Count == 0)
        {
            yield break;
        }

        int amountOfCardsToFillHand = GameManager.HandSize - PlayersHand.CardsInHand.Count;
        WaitForSeconds waitForSeconds = new(0.25f);

        for (int i = 0; i < amountOfCardsToFillHand; i++)
        {
            int lastIndexOfPile = _cardsInPile.Count - 1;
            Card drawnCard = Instantiate(_cardsInPile[lastIndexOfPile], PlayersHand.CardsParent);

            _cardsInPile.RemoveAt(lastIndexOfPile);
            _countDisplayTMP.text = _cardsInPile.Count.ToString();
            PlayersHand.CardsInHand.Add(drawnCard);

            drawnCard.CardTransform.position = transform.position;
            drawnCard.CardTransform.localScale = Vector3.zero;
            drawnCard.enabled = false;

            Transform cardSlot = GameView.Instance.PlayersHand.CardSlots[i];
            drawnCard.CurrentCardSlot = cardSlot;

            Sequence drawingSeq = DOTween.Sequence();
            drawingSeq.Append(drawnCard.CardTransform.DOScale(1f, 1f));
            drawingSeq.Join(drawnCard.CardTransform.DOMove(cardSlot.position, 1f));

            drawingSeq.onComplete += () =>
            {
                drawnCard.enabled = true;
            };


            if (_cardsInPile.Count == 0) break;

            yield return waitForSeconds;
        }

        OnCardsDrawn?.Invoke();
    }
}