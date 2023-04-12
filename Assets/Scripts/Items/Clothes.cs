using UnityEngine;

public class Clothes : Item
{
    public string Id { get; set; }
    public float Weight { get; set; }
    public int MaxCount { get; set; }
    private int _armor;

    public Clothes(string id, string weightString, string maxCountString, string armorString)
    {
        if(!double.TryParse(weightString, out double weight))
        {
            throw new System.ArgumentException(weightString + " can't be parsed to Weight for Clothes");
        }
        if(!int.TryParse(maxCountString, out int maxCount))
        {
            throw new System.ArgumentException(maxCountString + " can't be parsed to MaxCount for Clothes");
        }
        if(!int.TryParse(armorString, out int armor))
        {
            throw new System.ArgumentException(armorString + " can't be parsed to _armor for Clothes");
        }
        Weight = (float)weight;
        MaxCount = maxCount;
        _armor = armor;
        Id = id;
    }

    public void Use(Slot slot)
    {
        Debug.Log("Wear armor " + Id);
    }
}
