using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private GameObject Slots;
    [SerializeField] private GameObject EmptySlot;
    [SerializeField] private float yAmplitude = 300;
    [SerializeField]private float StartPositionY = 0;
    [SerializeField] private float yStep = 100;
    [SerializeField] private float xStep = 100;
    [SerializeField] private int xSlotsCount = 5;
    private List<GameObject> _slots = new List<GameObject>();

    void Start()
    {
        var slotsCount = Managers.Inventory.SlotsCount;
        var startPosition = new Vector3(-xStep * (xSlotsCount / 2), StartPositionY, transform.position.z);
        for(int i = 0; i < slotsCount; ++i)
        {
            var newSlot = Instantiate(EmptySlot);
            _slots.Add(newSlot);
            newSlot.transform.SetParent(Slots.transform);
            var offset = new Vector3((i % xSlotsCount) * xStep, -(i / xSlotsCount) * yStep, 0);
            newSlot.transform.localPosition = startPosition + offset;
            var itemId = Managers.Inventory.GetItemId(i);
            if(itemId == Item.EmptyItemId)
            {
                continue;
            }
            var itemCount = Managers.Inventory.GetItemCount(i);
        }
    }

    public void OnScrolling(float scrollingPosition)
    {
        float yPosition = scrollingPosition * yAmplitude + StartPositionY;
        var newPosition = new Vector3(Slots.transform.localPosition.x, yPosition, Slots.transform.localPosition.z);
        Slots.transform.localPosition = newPosition;
    }
}
