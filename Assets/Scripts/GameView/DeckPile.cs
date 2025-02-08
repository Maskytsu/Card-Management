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
    public int AmountOfCardsInPile => _cardsInPile.Count;

    //this contains card prefabs
    [ReadOnly, SerializeField] private List<Card> _cardsInPile;

    [SerializeField] private TextMeshProUGUI _countDisplayTMP;

    private void Start()
    {
        GameManager.Instance.OnTurnEnd += SendCardsToHand;

        SetupDeck();
        SendCardsToHand();
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

    public void MinusOneFromDisplayedNumer()
    {
        _countDisplayTMP.text = (Int32.Parse(_countDisplayTMP.text) - 1).ToString();
    }

    public void SendCardsToHand()
    {
        if (_cardsInPile.Count == 0)
        {
             return;
        }

        List<Card> cardsToSend = new();
        int amountOfCardsToFillHand = GameManager.HandSize - GameView.Instance.PlayersHand.AmountOfCardsInHand;

        for (int i = 0; i < amountOfCardsToFillHand; i++)
        {
            int lastIndexOfPile = _cardsInPile.Count - 1;
            Card spawnedCard = Instantiate(_cardsInPile[lastIndexOfPile]);
            spawnedCard.transform.position = transform.position;
            spawnedCard.transform.localScale = Vector3.zero;
            cardsToSend.Add(spawnedCard);

            _cardsInPile.RemoveAt(lastIndexOfPile);

            if (_cardsInPile.Count == 0) break;
        }

        StartCoroutine(GameView.Instance.PlayersHand.DrawCards(cardsToSend));
    }
}