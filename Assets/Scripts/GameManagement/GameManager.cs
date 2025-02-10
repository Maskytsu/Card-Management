using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public const int DeckSize = 25;
    public const int HandSize = 5;

    public event Action OnTurnEnd;
    public event Action OnGameEnd;

    public Deck? ChoosenDeck { get; private set; }

    [SerializeField] private Texture2D _basicCursorTex;
    [SerializeField] private Texture2D _holdingCursorTex;

    private void Awake()
    {
        CreateInstance();
        SetCursorToBasic();
    }

    private void Start()
    {
        ValidatePlayersDeck();
    }

    public void SetChoosenDeck(Deck deck)
    {
        ChoosenDeck = deck;
    }

    public void SetCursorToBasic()
    {
        Cursor.SetCursor(_basicCursorTex, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void SetCursorToHolding()
    {
        Cursor.SetCursor(_holdingCursorTex, Vector2.zero, CursorMode.ForceSoftware);
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
        else if (ChoosenDeck.Value.CardPrefabs.Count != DeckSize)
        {
            Debug.LogError("Choosen deck size is invalid!");
        }
        else
        {
            foreach(Card card in ChoosenDeck.Value.CardPrefabs)
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