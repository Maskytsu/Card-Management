using NaughtyAttributes;
using System;
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
        GameManager.Instance.OnTurnEnd += DrawCardsToHand;

        SetupDeck();
        DrawCardsToHand();
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

    private void DrawCardsToHand()
    {
        if (_cardsInPile.Count == 0)
        {
            return;
        }
        else
        {
            int amountOfCardsToFillHand = GameManager.HandSize - PlayersHand.CardsInHand.Count;
            List<Card> drawnCardPrefabs = new();

            for (int i = 0; i < amountOfCardsToFillHand; i++)
            {
                int lastIndex = _cardsInPile.Count - 1;
                drawnCardPrefabs.Add(_cardsInPile[lastIndex]);
                _cardsInPile.RemoveAt(lastIndex);
                if (_cardsInPile.Count == 0) break;
            }

            AddDrawnCardsToHand(drawnCardPrefabs);
        }
    }

    private void AddDrawnCardsToHand(List<Card> cardPrefabs)
    {

        List<Card> spawnedCards = new();
        foreach (Card card in cardPrefabs)
        {
            spawnedCards.Add(Instantiate(card, PlayersHand.transform));
        }

        PlayersHand.CardsInHand.AddRange(spawnedCards);
        OnCardsDrawn?.Invoke();
        _countDisplayTMP.text = _cardsInPile.Count.ToString();
    }
}