using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    public int SlotsCount { get { return _slots.Count; } }
    private int _slotsCount;
    private int _maxSlotsCount;
    private int _slotPrice;
    private Factory _factory;
    private InventoryDataMaster _dataMaster;
    private List<Slot> _slots = new List<Slot>();

    void OnDestroy()
    {
        Save();
    }

    public void Startup()
    {
        _factory = Factory.Instance;
        _dataMaster = new InventoryDataMaster();
        _dataMaster.Update();
        _slotsCount = _dataMaster.MinSlotsCount;
        _maxSlotsCount = _dataMaster.MaxSlotsCount;
        _slotPrice = _dataMaster.SlotPrice;
        while(_dataMaster.HasNextSlotData() && _slots.Count < _maxSlotsCount)
        {
            var slotData = _dataMaster.NextSlotData();
            if(slotData.Id == Item.EmptyItemId)
            {
                for(int i = 0; i < slotData.Count && i < _maxSlotsCount - _slots.Count; ++i)
                {
                    var newSlot = new Slot();
                    _slots.Add(newSlot);
                }
            }
            else
            {
                if(!_factory.TryGetObject(slotData.Id, out Item newItem))
                {
                    var newSlot = new Slot();
                    _slots.Add(newSlot);
                    Debug.Log("Can't add item " + slotData.Id);
                }
                else
                {
                    var newSlot = new Slot(slotData.Count, newItem);
                    _slots.Add(newSlot);
                }
            }
        }
        for(int i = _slots.Count; i < _slotsCount; ++i)
        {
            var newSlot = new Slot();
            _slots.Add(newSlot);
        }
        status = ManagerStatus.Started;
    }

    public void Save()
    {
        _dataMaster.SaveInventoryData(_slots.Count, _maxSlotsCount, _slots);
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

    public bool UseRandomItemWithId(string ItemId)
    {
        List<Slot> properSlots = GetProperSlotsListWithId(ItemId);
        if(properSlots.Count > 0)
        {
            var randomSlot = properSlots[Random.Range(0, properSlots.Count)];
            randomSlot.SlotItem.Use(randomSlot);
            return true;
        }
        return false;
    }

    public bool UseRandomItemWithType(string itemType)
    {
        List<Slot> properSlots = GetProperSlotsListWithType(itemType);
        if(properSlots.Count > 0)
        {
            var randomSlot = properSlots[Random.Range(0, properSlots.Count)];
            randomSlot.SlotItem.Use(randomSlot);
            return true;
        }
        return false;
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

    public bool FillEmptySlotWithId(string Id)
    {
        if(!_factory.TryGetObject(Id, out Item newItem))
        {
            Debug.Log("There is no item " + Id);
            return false;
        }
        foreach(var slot in _slots)
        {
            if(slot.SlotItem.Id == Item.EmptyItemId)
            {
                slot.FillSlot(newItem);
                return true;
            }
        }
        if(_slots.Count < _maxSlotsCount)
        {
            var newSlot = new Slot();
            newSlot.FillSlot(newItem);
            _slots.Add(newSlot);
            return true;
        }
        return false;
    }

    public bool ClearRandomNotEmptySlot()
    {
        var properSlots = GetNotEmptySlots();
        if(properSlots.Count > 0)
        {
            var randomSlot = properSlots[Random.Range(0, properSlots.Count)];
            randomSlot.Clear();
            return true;
        }
        return false;
    }

    public void ClearSlot(int index)
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

    private List<Slot> GetProperSlotsListWithId(string itemId)
    {
        List<Slot> properSlots = new List<Slot>();
        foreach(var slot in _slots)
        {
            if(slot.SlotItem.Id == itemId)
            {
                properSlots.Add(slot);
            }
        }
        return properSlots;
    }

    private List<Slot> GetProperSlotsListWithType(string itemType)
    {
        List<Slot> properSlots = new List<Slot>();
        foreach(var slot in _slots)
        {
            if(slot.SlotItem.GetType().ToString() == itemType)
            {
                properSlots.Add(slot);
            }
        }
        return properSlots;
    }

    private List<Slot> GetNotEmptySlots()
    {
        List<Slot> properSlots = new List<Slot>();
        foreach(var slot in _slots)
        {
            if(slot.SlotItem.Id != Item.EmptyItemId)
            {
                properSlots.Add(slot);
            }
        }
        return properSlots;
    }
}
