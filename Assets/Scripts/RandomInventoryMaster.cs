using System.Collections.Generic;
using UnityEngine;

/*
копирование кода в InventoryManager
*/

public class RandomInventoryMaster : MonoBehaviour
{
    [SerializeField] private string ClassesFileName = "Items";
    [SerializeField] private char ListSeparator = ';';
    private Dictionary<string, List<string>> Items = new Dictionary<string, List<string>>();
    private readonly string EmptyString = "";

    void Awake()
    {
        var objectsJson = Resources.Load<TextAsset>(ClassesFileName);
        try
        {
            var objects = JsonUtility.FromJson<DeserializedItemsJsonData>(objectsJson.ToString());
            foreach(var obj in objects.Items)
            {
                if(!Items.ContainsKey(obj.Type))
                {
                    Items.Add(obj.Type, new List<string>());
                }
                Items[obj.Type].Add(obj.Id);
            }
        }
        catch(System.Exception ex)
        {
            throw new RandomInvenotryException(ClassesFileName, ex);
        }
        
    }
    
    public void AddRandomItemFromTypeList(string types)
    {
        var typesList = new List<string>(types.Split(ListSeparator));
        var choseType = false;
        while(!choseType && types.Length > 0)
        {
            var index = Random.Range(0, typesList.Count);
            if(Items.ContainsKey(typesList[index]))
            {
                AddRandomItem(typesList[index]);
                choseType = true;
            }
            else
            {
                typesList.RemoveAt(index);
            }
        }
    }
    
    public void AddRandomItem(string itemType)
    {
        var newItem = GetRandomItemId(itemType);
        if(newItem == EmptyString)
        {
            return;
        }
        if(Managers.Inventory.AddItem(newItem))
        {
            Messenger.Broadcast(InventoryEvents.InventoryChanged);
        }
    }
    
    public void FillEmptySlotsWithType(string itemType)
    {
        if(!Items.ContainsKey(itemType))
        {
            Debug.Log(itemType + " item type doesn't exist");
            return;
        }
        var changedInventory = false;
        foreach(var itemId in Items[itemType])
        {
            changedInventory = Managers.Inventory.FillEmptySlotWithId(itemId) || changedInventory;
        }
        if(changedInventory)
        {
            Messenger.Broadcast(InventoryEvents.InventoryChanged);
        }
    }

    public void UseRandomItemWithId(string itemType)
    {
        var newItem = GetRandomItemId(itemType);
        if(newItem == EmptyString)
        {
            return;
        }
        if(Managers.Inventory.UseRandomItemWithId(newItem))
        {
            Messenger.Broadcast(InventoryEvents.InventoryChanged);
        }
    }

    public void UseRandomItemWithType(string itemType)
    {
        if(Managers.Inventory.UseRandomItemWithType(itemType))
        {
            Messenger.Broadcast(InventoryEvents.InventoryChanged);
        }
    }

    public void ClearRandomSlot()
    {
        if(Managers.Inventory.ClearRandomNotEmptySlot())
        {
            Messenger.Broadcast(InventoryEvents.InventoryChanged);
        }
        else
        {
            Debug.Log("Inventory is empty");
        }
    }

    private string GetRandomItemId(string itemType)
    {
        if(!Items.ContainsKey(itemType))
        {
            Debug.Log(itemType + " item type doesn't exist");
            return EmptyString;
        }
        var ItemsWithType = Items[itemType];
        var newItemIndex = Random.Range(0, ItemsWithType.Count);
        return ItemsWithType[newItemIndex];
    }
}
