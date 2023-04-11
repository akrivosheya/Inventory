public class EmptyItem : Item
{
    public string Id { get; set; }
    public float Weight { get; set; }
    public int MaxCount { get; set; }

    public EmptyItem()
    {
        Id = Item.EmptyItemId;
        MaxCount = 1;
    }

    public void Use(Slot slot)
    {
    }
}

