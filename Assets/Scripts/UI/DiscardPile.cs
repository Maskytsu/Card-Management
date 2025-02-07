using NaughtyAttributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countDisplayTMP;

    //this contains instantied card game objects (disabled)
    [ReadOnly, SerializeField] private List<Card> _cardsInPile = new();

    public void AddCardToPile(Card card)
    {
        _cardsInPile.Add(card);
        _countDisplayTMP.text = _cardsInPile.Count.ToString();
    }
}
