using System.Collections.Generic;

[System.Serializable]
public class DeserializedInventoryJsonData
{
    public int MinSlotsCount;
    public int MaxSlotsCount;
    public int SlotPrice;
    public List<InitializingSlotData> Slots;
}
