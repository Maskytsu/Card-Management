using NaughtyAttributes;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    //this contains instantied card game objects (disabled)
    [ReadOnly, SerializeField] private List<Card> _cardsInPile = new();

    [SerializeField] private Transform _cardsParent;
    [SerializeField] private TextMeshProUGUI _countDisplayTMP;

    public void AddCardToPile(Card card)
    {
        _cardsInPile.Add(card);
        card.MoveTransform.SetParent(_cardsParent);
    }

    public void SetCardParent(Card card)
    {
        card.MoveTransform.SetParent(_cardsParent);
    }

    public void AddOneToDisplayedNumer()
    {
        _countDisplayTMP.text = (Int32.Parse(_countDisplayTMP.text) + 1).ToString();
    }
}
