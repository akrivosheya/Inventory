using System;

public class SlotException : Exception
{
    public SlotException(int maxCount) : 
    base(maxCount + " can't be maxCount: it has to be integer more than 0")
    {
    }

    
    public SlotException(int count, int maxCount) : 
    base(count + " can't be Count: it has to be integer more than 0 and less than " + maxCount)
    {
    }
}
