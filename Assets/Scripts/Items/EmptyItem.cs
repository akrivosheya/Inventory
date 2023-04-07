public class EmptyItem : Item
{
    public string Id { get; set; }
    public float Weight { get; set; }

    public EmptyItem()
    {
        Id = Item.EmptyItemId;
    }

    public void Use(Slot slot)
    {
    }
}

