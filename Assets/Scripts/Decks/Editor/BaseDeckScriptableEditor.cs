using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseDeckScriptable))]
public class BaseDeckScriptableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BaseDeckScriptable deckScriptable = (BaseDeckScriptable)target;
        int curretSizeOfDeck = deckScriptable.Deck.Cards.Count;

        if (curretSizeOfDeck != GameManager.DeckSize)
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Size of the deck is not valid. It should contain " + GameManager.DeckSize + " cards!", MessageType.Info);
        }
    }
}
