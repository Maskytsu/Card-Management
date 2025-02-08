using UnityEngine;

public class SetChoosenDeckAsBaseDeck : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private BaseDeckScriptable _baseDeck;

    private void Awake()
    {
        _gameManager.SetChoosenDeck(_baseDeck.Deck);
    }
}