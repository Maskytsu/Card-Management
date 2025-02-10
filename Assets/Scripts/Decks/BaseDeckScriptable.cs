using UnityEngine;

[CreateAssetMenu(fileName = "BaseDeck", menuName = "ScriptableObjects/BaseDeck")]
public class BaseDeckScriptable : ScriptableObject
{
    [field: SerializeField] public Deck BaseDeck { get; private set; }
}