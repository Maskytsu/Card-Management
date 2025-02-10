using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnOrGameButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _buttonTMP;
    [SerializeField] private Button _button;

    private const string _endGameText = "End\nGame";

    private DeckPile DeckPile => GameView.Instance.DeckPile;
    private PlayersHand PlayersHand => GameView.Instance.PlayersHand;

    private void Start()
    {
        DeckPile.OnCardsSentToHand += SwapNameIfLastTurn;
    }

    public void SetInteractable(bool value)
    {
        _button.interactable = value;
    }

    public void EndTurnOrGame()
    {
        if (DeckPile.AmountOfCardsInPile == 0) GameManager.Instance.EndGame();
        else if (PlayersHand.AmountOfCardsInHand < GameManager.HandSize) GameManager.Instance.EndTurn();
    }

    private void SwapNameIfLastTurn()
    {
        if (DeckPile.AmountOfCardsInPile == 0) _buttonTMP.text = _endGameText;
    }
}