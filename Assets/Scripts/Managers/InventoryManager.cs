using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    public int SlotsCount { get { return _slots.Count; } }
    [SerializeField] private int _slotsCount = 15;
    [SerializeField] private int _maxSlotsCount = 30;
    private Factory _factory;
    private List<string> ItemsIds;
    private List<Slot> _slots = new List<Slot>();
    private Dictionary<int, int> _items = new Dictionary<int, int>();

    public void Startup()
    {
        _factory = Factory.Instance;
        for(int i = 0; i < _slotsCount; ++i)
        {
            var newSlot = new Slot();
            _slots.Add(newSlot);
        }
        status = ManagerStatus.Started;
    }

    public bool AddItem(string Id)
    {
        if(!_factory.TryGetObject(Id, out Item newItem))
        {
            Debug.Log("There is no item " + Id);
            return false;
        }
        foreach(var slot in _slots)
        {
            if(slot.SlotItem.Id == Item.EmptyItemId || (slot.SlotItem.Id == Id && !slot.IsFull))
            {
                slot.AddItem(newItem);
                return true;
            }
        }
        if(_slots.Count < _maxSlotsCount)
        {
            var newSlot = new Slot();
            newSlot.AddItem(newItem);
            _slots.Add(newSlot);
            return true;
        }
        return false;
    }

    public void UseItem(int index)
    {
        if(index < 0 || _slots.Count <= index)
        {
            Debug.Log("index has to be integer between 0 and " + _slots.Count + ", but it is " + index);
            return;
        }
        else
        {
            var slot = _slots[index];
            slot.SlotItem.Use(slot);
        }
    }

    public void FillSlot(int index)
    {
        if(index < 0 || _slots.Count <= index)
        {
            Debug.Log("index has to be integer between 0 and " + _slots.Count + ", but it is " + index);
            return;
        }
        else
        {
            var slot = _slots[index];
            slot.FillSlot();
        }
    }

    public void DeleteSlot(int index)
    {
        if(index < 0 || _slots.Count <= index)
        {
            Debug.Log("index has to be integer between 0 and " + _slots.Count + ", but it is " + index);
            return;
        }
        else
        {
            var slot = _slots[index];
            slot.Clear();
        }
    }

    public string GetItemId(int index)
    {
        if(index < 0 || _slots.Count <= index)
        {
            throw new System.ArgumentException("index has to be integer between 0 and " + _slots.Count + ", but it is " + index);
        }
        return _slots[index].SlotItem.Id;
    }

    public int GetItemCount(int index)
    {
        if(index < 0 || _slots.Count <= index)
        {
            throw new System.ArgumentException("index has to be integer between 0 and " + _slots.Count + ", but it is " + index);
        }
        return _slots[index].Count;
    }
}
