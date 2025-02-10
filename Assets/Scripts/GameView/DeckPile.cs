using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DeckPile : MonoBehaviour
{
    public event Action OnCardsSentToHand;

    public int AmountOfCardsInPile => _cardPrefabsInPile.Count;

    //this contains card prefabs
    [ReadOnly, SerializeField] private List<Card> _cardPrefabsInPile;

    [SerializeField] private TextMeshProUGUI _countDisplayTMP;

    private void Start()
    {
        GameManager.Instance.OnTurnEnd += SendCardsToHand;
        GameView.Instance.EndTurnOrGameButton.SetInteractable(false);

        SetupDeck();
    }

    public void MinusOneFromDisplayedNumer()
    {
        _countDisplayTMP.text = (Int32.Parse(_countDisplayTMP.text) - 1).ToString();
    }

    private void SetupDeck()
    {
        _cardPrefabsInPile = GameManager.Instance.ChoosenDeck.Value.CardPrefabs.ToList();

        //shuffle deck with Fisher-Yates shuffle algorithm
        for (int i = _cardPrefabsInPile.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            (_cardPrefabsInPile[i], _cardPrefabsInPile[randomIndex]) = (_cardPrefabsInPile[randomIndex], _cardPrefabsInPile[i]);
        }

        _countDisplayTMP.text = _cardPrefabsInPile.Count.ToString();

        Sequence shuffleAnimation = DOTween.Sequence();
        shuffleAnimation.Append(transform.DOShakePosition(1f, 50f));
        shuffleAnimation.Append(_countDisplayTMP.DOFade(1f, 0.5f));

        shuffleAnimation.onComplete += SendCardsToHand;
    }

    private void SendCardsToHand()
    {
        if (_cardPrefabsInPile.Count == 0) return;

        List<Card> cardsToSend = new();
        int amountOfCardsToFillHand = GameManager.HandSize - GameView.Instance.PlayersHand.AmountOfCardsInHand;

        for (int i = 0; i < amountOfCardsToFillHand; i++)
        {
            int lastIndexOfPile = _cardPrefabsInPile.Count - 1;

            Card spawnedCard = Instantiate(_cardPrefabsInPile[lastIndexOfPile]);

            spawnedCard.SetCardEnabled(false);
            spawnedCard.MoveTransform.position = transform.position;
            spawnedCard.MoveTransform.localScale = Vector3.zero;

            cardsToSend.Add(spawnedCard);
            _cardPrefabsInPile.RemoveAt(lastIndexOfPile);

            if (_cardPrefabsInPile.Count == 0) break;
        }

        OnCardsSentToHand?.Invoke();

        StartCoroutine(GameView.Instance.PlayersHand.DrawCards(cardsToSend));
    }
}