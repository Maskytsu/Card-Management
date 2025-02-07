using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
    public void EndTurn()
    {
        GameManager.Instance.EndTurn();
    }
}
