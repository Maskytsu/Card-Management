using TMPro;
using UnityEngine;

public class EndTurnOrGameButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _buttonTMP;

    private readonly string _endGameText = "End\nGame";

    private DeckPile DeckPile => GameView.Instance.DeckPile;
    private PlayersHand PlayersHand => GameView.Instance.PlayersHand;

    private void Start()
    {
        DeckPile.OnCardsSent += SwapNameIfLastTurn;
    }

    private void SwapNameIfLastTurn()
    {
        if (DeckPile.AmountOfCardsInPile == 0) _buttonTMP.text = _endGameText;
    }

    public void EndTurnOrGame()
    {
        if (DeckPile.AmountOfCardsInPile == 0) GameManager.Instance.EndGame();
        else if(PlayersHand.AmountOfCardsInHand < GameManager.HandSize) GameManager.Instance.EndTurn();
    }
}