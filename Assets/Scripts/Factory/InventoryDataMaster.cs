using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDataMaster
{
    public int MinSlotsCount { get { return _inventoryData.MinSlotsCount; } }
    public int MaxSlotsCount { get { return _inventoryData.MaxSlotsCount; } }
    private readonly string FileName = "Inventory";
    private readonly string EmptyString = "";
    private DeserializedInventoryJsonData _inventoryData;
    private readonly int DefaultMinCount = 15;
    private readonly int DefaultMaxCount = 30;
    private int _nextSlot = 0;

    public InventoryDataMaster()
    {
        var objectsJson = GetJson();
        if(objectsJson == EmptyString)
        {
            _inventoryData = new DeserializedInventoryJsonData(){ MinSlotsCount=DefaultMinCount, MaxSlotsCount=DefaultMaxCount,
                Slots=new List<InitializingSlotData>() };
            Debug.Log("Can't load data from " + FileName);
            return;
        }
        try
        {
            _inventoryData = JsonUtility.FromJson<DeserializedInventoryJsonData>(objectsJson.ToString());
            Debug.Log(_inventoryData.Slots.Count);
            if(_inventoryData == null)
            {
                _inventoryData = new DeserializedInventoryJsonData(){ MinSlotsCount=DefaultMinCount, MaxSlotsCount=DefaultMaxCount,
                    Slots=new List<InitializingSlotData>() };
                Debug.Log("Can't load data from " + FileName);
            }
        }
        catch(System.Exception ex)
        {
            _inventoryData = new DeserializedInventoryJsonData(){ MinSlotsCount=DefaultMinCount, MaxSlotsCount=DefaultMaxCount,
                Slots=new List<InitializingSlotData>() };
            Debug.Log("Can't load data from " + FileName + ": " + ex);
        }
    }

    public void SaveInventoryData(int minSlotsCount, int MaxSlotsCount, List<Slot> slots)
    {
        _inventoryData.MinSlotsCount = minSlotsCount;
        _inventoryData.MaxSlotsCount = MaxSlotsCount;
        _inventoryData.Slots.Clear();
        for(int i = 0; i < slots.Count; ++i)
        {
            var slot = slots[i];
            if(slot.SlotItem.Id == Item.EmptyItemId)
            {
                var emptyItemsCount = 0;
                for(; i < slots.Count; ++i)
                {
                    slot = slots[i];
                    if(slot.SlotItem.Id == Item.EmptyItemId)
                    {
                        ++emptyItemsCount;
                    }
                    else
                    {
                        var slotData = new InitializingSlotData(){ Id=Item.EmptyItemId, Count=emptyItemsCount };
                        _inventoryData.Slots.Add(slotData);
                        --i;
                        break;
                    }
                }
            }
            else
            {
                var slotData = new InitializingSlotData(){ Id=slot.SlotItem.Id, Count=slot.Count };
                _inventoryData.Slots.Add(slotData);
            }
        }
        SaveToJson();
    }

    public void Update()
    {
        _nextSlot = 0;
    }

    public InitializingSlotData NextSlotData()
    {
        if(_nextSlot >= _inventoryData.Slots.Count)
        {
            throw new System.ArgumentOutOfRangeException("There is not more data for initializing Inventory");
        }
        return _inventoryData.Slots[_nextSlot++];
    }

    public bool HasNextSlotData()
    {
        if(_nextSlot >= _inventoryData.Slots.Count)
        {
            return false;
        }
        return true;
    }

    private string GetJson()
    {
        var objectsJson = EmptyString;
        var filePath = Path.Combine(Application.persistentDataPath, FileName);
        try
        {
            using(var reader = new StreamReader(File.OpenRead(filePath)))
            {
                objectsJson = reader.ReadToEnd();
            }
        }
        catch(System.ArgumentException ex)
        {
            Debug.Log("Can't open file to load inventory data from " + filePath + ": " + ex);
        }
        catch(System.OutOfMemoryException ex)
        {
            Debug.Log("Can't load inventory data from " + filePath + ": " + ex);
        }
        catch(System.IO.IOException ex)
        {
            Debug.Log("Can't load inventory data from " + filePath + ": " + ex);
        }
        return objectsJson;
    }

    private void SaveToJson()
    {
        var objectsJson = JsonUtility.ToJson(_inventoryData);
        var filePath = Path.Combine(Application.persistentDataPath, FileName);
        try
        {
            using(var writer = new StreamWriter(File.Create(filePath)))
            {
                writer.Write(objectsJson);
                Debug.Log("Wrote to " + filePath);
            }
        }
        catch(System.ArgumentException ex)
        {
            Debug.Log("Can't open file to save inventory data to " + filePath + ": " + ex);
        }
        catch(System.ObjectDisposedException ex)
        {
            Debug.Log("Can't save inventory data to " + filePath + ": " + ex);
        }
        catch(System.NotSupportedException ex)
        {
            Debug.Log("Can't save inventory data to " + filePath + ": " + ex);
        }
        catch(System.IO.IOException ex)
        {
            Debug.Log("Can't save inventory data to " + filePath + ": " + ex);
        }
    }
}