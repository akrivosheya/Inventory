using UnityEngine;

public class Bullets : Item
{
    public string Id { get; set; }
    public float Weight { get; set; }

    public Bullets(string Id, string WeightString)
    {
        if(!double.TryParse(WeightString, out double Weight))
        {
            throw new System.ArgumentException(WeightString + " can't be parsed to Weight for Bullets");
        }
        this.Weight = (float)Weight;
        this.Id = Id;
    }

    public void Use(Slot slot)
    {
        slot.DeleteItem();
        Debug.Log("Use bullets " + Id);
    }
}
