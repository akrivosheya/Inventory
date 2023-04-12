using UnityEngine;

public class Bullet : Item
{
    public string Id { get; set; }
    public float Weight { get; set; }
    public int MaxCount { get; set; }

    public Bullet(string id, string weightString, string maxCountString)
    {
        if(!double.TryParse(weightString, out double weight))
        {
            throw new System.ArgumentException(weightString + " can't be parsed to Weight for Bullet");
        }
        if(!int.TryParse(maxCountString, out int maxCount))
        {
            throw new System.ArgumentException(maxCountString + " can't be parsed to MaxCount for Bullet");
        }
        Weight = (float)weight;
        MaxCount = maxCount;
        Id = id;
    }

    public void Use(Slot slot)
    {
        slot.DeleteItem();
        Debug.Log("Use bullets " + Id);
    }
}
