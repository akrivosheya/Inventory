public class Slot
{
    public Item SlotItem { get; set; }
    public int Count { get; private set; } = 0;
    private readonly EmptyItem emptyItem = new EmptyItem();
    private int _maxCount;
    
    public Slot(int maxCount)
    {
        SlotItem = emptyItem;
        if(maxCount < 0)
        {
            throw new SlotException(maxCount);
        }
        _maxCount = maxCount;
    }

    public Slot(int maxCount, int count, Item item) : this(maxCount)
    {
        if(count < 0 || count > maxCount)
        {
            throw new SlotException(count, maxCount);
        }
        SlotItem = item;
        Count = count;
    }

    public void AddItem(Item item)
    {
        if(SlotItem.Id != item.Id)
        {
            SlotItem = item;
            Count = 1;
        }
        else
        {
            if(Count < _maxCount)
            {
                Count += 1;
            }
        }
    }

    public void FillSlot(Item item)
    {
        SlotItem = item;
        Count = _maxCount;
    }
    

    public void FillSlot()
    {
        FillSlot(SlotItem);
    }

    public void DeleteItem()
    {
        if(Count > 0)
        {
            Count -= 1;
            if(Count == 0)
            {
                SlotItem = emptyItem;
            }
        }
    }

    

    public void Clear()
    {
        SlotItem = emptyItem;
        Count = 0;
    }
}
