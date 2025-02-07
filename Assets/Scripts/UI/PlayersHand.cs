using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class PlayersHand : MonoBehaviour
{
    //this contains instantied card game objects
    [ReadOnly] public List<Card> CardsInHand = new();
}