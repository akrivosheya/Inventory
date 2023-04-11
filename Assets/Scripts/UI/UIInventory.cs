using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private GameObject Slots;
    [SerializeField] private UIItem ItemPrefab;
    [SerializeField] private GameObject EmptySlot;
    [SerializeField]private float StartPositionY = 0;
    [SerializeField] private float yStep = 100;
    [SerializeField] private float xStep = 100;
    [SerializeField] private int xSlotsCount = 5;
    private List<GameObject> _slots = new List<GameObject>();
    private List<UIItem> _items = new List<UIItem>();
    private float _yAmplitude;

    void Awake()
    {
        Messenger.AddListener(InventoryEvents.InventoryChanged, OnInventoryChanged);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(InventoryEvents.InventoryChanged, OnInventoryChanged);
    }

    void Start()
    {
        OnInventoryChanged();
    }

    public void OnInventoryChanged()
    {
        var inventorySlotsCount = Managers.Inventory.SlotsCount;
        _yAmplitude = inventorySlotsCount / xSlotsCount * yStep;
        var startPosition = new Vector3(-xStep * (xSlotsCount / 2), StartPositionY, transform.position.z);
        int i = 0;
        for(; i < _slots.Count; ++i)
        {
            _items[i].UpdateItem();
        }
        for(; i < inventorySlotsCount; ++i)
        {
            var newItem = Instantiate(ItemPrefab);
            var newSlot = Instantiate(EmptySlot);
            newItem.Index = i;
            newItem.gameObject.transform.position = newSlot.transform.position;
            newItem.gameObject.transform.SetParent(newSlot.transform);
            _items.Add(newItem);
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
        float yPosition = scrollingPosition * _yAmplitude + StartPositionY;
        var newPosition = new Vector3(Slots.transform.localPosition.x, yPosition, Slots.transform.localPosition.z);
        Slots.transform.localPosition = newPosition;
    }
}
