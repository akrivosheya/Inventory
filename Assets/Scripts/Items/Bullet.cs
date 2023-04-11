using UnityEngine;

public class Bullet : Item
{
    public string Id { get; set; }
    public float Weight { get; set; }
    public int MaxCount { get; set; }

    public Bullet(string Id, string WeightString, string MaxCountString)
    {
        if(!double.TryParse(WeightString, out double Weight))
        {
            throw new System.ArgumentException(WeightString + " can't be parsed to Weight for Bullet");
        }
        if(!int.TryParse(MaxCountString, out int MaxCount))
        {
            throw new System.ArgumentException(MaxCountString + " can't be parsed to MaxCount for Bullet");
        }
        this.Weight = (float)Weight;
        this.MaxCount = MaxCount;
        this.Id = Id;
    }

    public void Use(Slot slot)
    {
        slot.DeleteItem();
        Debug.Log("Use bullets " + Id);
    }
}
