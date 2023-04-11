using System;

public class RandomInvenotryException : Exception
{
    public RandomInvenotryException(string file, Exception ex) : 
    base("Can't load ids from file " + file + ": " + ex)
    {
    }
}
