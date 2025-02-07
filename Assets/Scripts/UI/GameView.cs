using UnityEngine;

public class GameView : MonoBehaviour
{
    public static GameView Instance { get; private set; }

    [field: SerializeField] public DeckPile DeckPile { get; private set; }
    [field: SerializeField] public PlayersHand PlayersHand { get; private set; }
    [field: SerializeField] public DiscardPile DiscardPile { get; private set; }
    [field: SerializeField] public Board Board { get; private set; }
    [field: SerializeField] public EndTurnOrGameButton EndTurnOrGameButton { get; private set; }
    [field: SerializeField] public Canvas MainCanvas { get; private set; }
    [Space]
    [SerializeField] private GameObject GameEndedPanel;


    private void Awake()
    {
        CreateInstance();
    }

    private void Start()
    {
        GameManager.Instance.OnGameEnd += () => GameEndedPanel.SetActive(true);
    }

    private void CreateInstance()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one GameView in the scene!");
        }
        Instance = this;
    }
}
