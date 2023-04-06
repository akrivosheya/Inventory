using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    private Factory _factory;
    private List<string> ItemsIds;
    private Dictionary<int, int> _items = new Dictionary<int, int>();

    public void Startup()
    {
        _factory = Factory.Instance;
        status = ManagerStatus.Started;
    }

    public void AddItem(int itemId)
    {
        if(itemId >= ItemsIds.Count || itemId < 0)
        {
            throw new NoSuchItemException(itemId);
        }
        if(!_items.ContainsKey(itemId))
        {
            _items.Add(itemId, 0);
        }
        _items[itemId]++;
    }

    public int GetCount(int itemId)
    {
        if(!_items.ContainsKey(itemId))
        {
            return 0;
        }
        return _items[itemId];
    }

    public IEnumerator<int> GetEnumerator()
    {
        foreach(var id in _items.Keys)
        {
            if(_items[id] > 0)
            {
                yield return id;
            }
            else
            {
                continue;
            }
        }
    }
}
