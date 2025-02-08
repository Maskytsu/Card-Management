using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class PlayersHand : MonoBehaviour
{
    [field: SerializeField] public Transform CardsParent { get; private set; }
    [field: SerializeField, ReadOnly] public List<Transform> CardSlots { get; private set; }
    //this contains instantied card game objects
    [ReadOnly] public List<Card> CardsInHand = new();

    [SerializeField] private RectTransform _slotsParent;

    //for more than 8 cards it will look wacky, distance could be created dynamically
    private Vector3 _distanceBetweenSlots = new Vector3(150f, 0f, 0f);

    private void Awake()
    {
        CardSlots = GenerateCardSlots(GameManager.HandSize);
    }

    private List<Transform> GenerateCardSlots(int numberOfSlots)
    {
        if (numberOfSlots == 0) return null;

        List<Transform> generatedSlots = new();
        List<Vector3> slotPositions = new();

        if (numberOfSlots % 2 == 0)
        {
            //calculations for even number of slots

            Vector3 centerLeftPos = transform.position - (_distanceBetweenSlots / 2);
            Vector3 lastLeftPos = centerLeftPos - (((numberOfSlots / 2) - 1) * _distanceBetweenSlots);

            for (int i = 0; i < numberOfSlots; i++)
            {
                GameObject newSlot = new GameObject("Slot " + (i + 1));
                newSlot.transform.position = lastLeftPos + (i * _distanceBetweenSlots);
                generatedSlots.Add(newSlot.transform);
            }
        }
        else
        {
            //calculations for odd number of slots

            Vector3 lastLeftPos = -(((numberOfSlots - 1) / 2)) * _distanceBetweenSlots;

            for (int i = 0; i < numberOfSlots; i++)
            {
                GameObject newSlot = new GameObject("Slot " + (i + 1));
                newSlot.transform.parent = _slotsParent;
                newSlot.transform.localPosition = lastLeftPos + (i * _distanceBetweenSlots);
                generatedSlots.Add(newSlot.transform);
            }
        }

        return generatedSlots;
    }
}


















