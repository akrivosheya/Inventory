using UnityEngine;

public class Weapon : Item
{
    public string Id { get; set; }
    public float Weight { get; set; }
    public int MaxCount { get; set; }
    private int _damage;

    public Weapon(string id, string weightString, string maxCountString, string damageString)
    {
        if(!double.TryParse(weightString, out double weight))
        {
            throw new System.ArgumentException(weightString + " can't be parsed to Weight for Weapon");
        }
        if(!int.TryParse(maxCountString, out int maxCount))
        {
            throw new System.ArgumentException(maxCountString + " can't be parsed to MaxCount for Weapon");
        }
        if(!int.TryParse(damageString, out int damage))
        {
            throw new System.ArgumentException(damageString + " can't be parsed to _damage for Weapon");
        }
        Weight = (float)weight;
        MaxCount = maxCount;
        _damage = damage;
        Id = id;
    }

    public void Use(Slot slot)
    {
        Debug.Log("Damage " + _damage + " with weapon " + Id);
    }
}
