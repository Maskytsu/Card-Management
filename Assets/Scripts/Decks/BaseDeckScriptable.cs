using UnityEngine;

[CreateAssetMenu(fileName = "BaseDeck", menuName = "ScriptableObjects/BaseDeck")]
public class BaseDeckScriptable : ScriptableObject
{
    [field: SerializeField] public Deck Deck { get; private set; }
}
