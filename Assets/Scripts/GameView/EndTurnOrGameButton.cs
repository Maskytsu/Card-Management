using TMPro;
using UnityEngine;

public class EndTurnOrGameButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _buttonTMP;

    private DeckPile DeckPile => GameView.Instance.DeckPile;

    private void Start()
    {
        DeckPile.OnCardsDrawn += SwapNameIfLastTurn;
    }

    private void SwapNameIfLastTurn()
    {
        if (DeckPile.AmountOfCardsInPile == 0) _buttonTMP.text = "End\nGame";
    }

    public void EndTurnOrGame()
    {
        if (DeckPile.AmountOfCardsInPile == 0) GameManager.Instance.EndGame();
        else GameManager.Instance.EndTurn();
    }
}
