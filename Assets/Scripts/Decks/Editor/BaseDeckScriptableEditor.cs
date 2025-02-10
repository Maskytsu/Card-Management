using UnityEditor;

[CustomEditor(typeof(BaseDeckScriptable))]
public class BaseDeckScriptableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BaseDeckScriptable deckScriptable = (BaseDeckScriptable)target;

        if (deckScriptable.BaseDeck.CardPrefabs == null) return;

        if (deckScriptable.BaseDeck.CardPrefabs.Count != GameManager.DeckSize)
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Size of the deck is not valid. It should contain " 
                + GameManager.DeckSize + " cards!", MessageType.Info);
        }
    }
}