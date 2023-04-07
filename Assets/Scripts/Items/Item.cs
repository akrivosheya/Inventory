public interface Item
{
    public static string EmptyItemId = "Empty";
    public string Id { get; set; }
    public float Weight { get; set; }
    public void Use(Slot slot);
}
