using System.Collections.Generic;
using UnityEngine;

public class RandomInventoryMaster : MonoBehaviour
{
    [SerializeField] private string ClassesFileName = "Items";
    private Dictionary<string, List<string>> Items = new Dictionary<string, List<string>>();

    void Awake()
    {
        var objectsJson = Resources.Load<TextAsset>(ClassesFileName);
        try
        {
            var objects = JsonUtility.FromJson<DeserializedJsonData>(objectsJson.ToString());
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
    
    public void AddRandomItem(string ItemType)
    {
        var ItemsWithType = Items[ItemType];
        var newItemIndex = Random.Range(0, ItemsWithType.Count);
        var newItem = ItemsWithType[newItemIndex];
        if(Managers.Inventory.AddItem(newItem))
        {
            Messenger.Broadcast(InventoryEvents.InventoryChanged);
        }
    }
}
