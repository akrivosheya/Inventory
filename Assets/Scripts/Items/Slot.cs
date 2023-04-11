public class Slot
{
    public Item SlotItem { get; set; }
    public int Count { get; private set; } = 0;
    public bool IsFull { get { return Count == SlotItem.MaxCount; } }
    private readonly EmptyItem emptyItem = new EmptyItem();
    
    public Slot()
    {
        SlotItem = emptyItem;
    }

    public Slot(int count, Item item)
    {
        if(count < 0 || count > item.MaxCount)
        {
            throw new SlotException(count, item.MaxCount);
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
            if(Count < item.MaxCount)
            {
                Count += 1;
            }
        }
    }

    public void FillSlot(Item item)
    {
        SlotItem = item;
        Count = item.MaxCount;
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
