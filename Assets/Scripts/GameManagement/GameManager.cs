using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static int DeckSize { get; private set; } = 25;
    public static int HandSize { get; private set; } = 5;

    public event Action OnTurnEnd;
    public event Action OnGameEnd;

    public Deck ChoosenDeck { get; private set; }

    [SerializeField] private InputSystemUIInputModule _inputSystemForUI;

    private void Awake()
    {
        CreateInstance();
    }

    private void Start()
    {
        ValidatePlayersDeck();
    }

    public void SetChoosenDeck(Deck deck)
    {
        ChoosenDeck = deck;
    }

    public void EndTurn()
    {
        OnTurnEnd?.Invoke();
    }

    public void EndGame()
    {
        OnGameEnd?.Invoke();
    }

    private void ValidatePlayersDeck()
    {
        if (ChoosenDeck == null)
        {
            Debug.LogError("Choosen deck is not assigned!");
        }
        else if (ChoosenDeck.Cards.Count != DeckSize)
        {
            Debug.LogError("Choosen deck size is invalid!");
        }
        else
        {
            foreach(Card card in ChoosenDeck.Cards)
            {
                if (card == null)
                {
                    Debug.LogError("Some cards in choosen deck are not assigned!");
                    return;
                }
            }
        }
    }

    private void CreateInstance()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one GameManager in the scene!");
        }
        Instance = this;
    }
}