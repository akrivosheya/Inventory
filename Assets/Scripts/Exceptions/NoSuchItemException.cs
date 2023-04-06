using System;

public class NoSuchItemException : Exception
{
    public NoSuchItemException(int itemId) : 
    base("There is not item with id " + itemId)
    {
    }
}
