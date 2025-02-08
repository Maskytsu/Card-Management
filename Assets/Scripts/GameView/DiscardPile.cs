using NaughtyAttributes;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    [field: SerializeField] public Transform CardsParent { get; private set; }
    //this contains instantied card game objects (disabled)
    [ReadOnly, SerializeField] public List<Card> CardsInPile = new();

    [SerializeField] private TextMeshProUGUI _countDisplayTMP;

    public void AddOneCardToDisplayedNumer()
    {
        _countDisplayTMP.text = (Int32.Parse(_countDisplayTMP.text) + 1).ToString();
    }
}
