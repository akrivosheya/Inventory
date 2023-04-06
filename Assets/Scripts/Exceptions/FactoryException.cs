using System;

public class FactoryException : Exception
{
    public FactoryException(string file, string message) : 
    base("Can't initialize factory " + file + ": " + message)
    {
    }
}
