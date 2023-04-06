using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

public sealed class Factory
{
	public static Factory Instance { get { return lazy.Value; } }
    private static readonly Lazy<Factory> lazy = new Lazy<Factory>(() => new Factory());
    private Dictionary<string, InitializingData> _objectsDictionary = new Dictionary<string, InitializingData>();
    private Dictionary<string, object> _objectsBuffer = new Dictionary<string, object>();
    private string ClassesFileName = "Items";

    private Factory()
   	{
        var objectsJson = Resources.Load<TextAsset>(ClassesFileName);
        try
        {
            var objects = JsonUtility.FromJson<DeserializedJsonData>(objectsJson.ToString());
            foreach(var obj in objects.Items)
            {
                _objectsDictionary.Add(obj.Id, obj);
            }
        }
        catch(Exception ex)
        {
            throw new FactoryException(ClassesFileName, ex.Message);
        }
    }

    public bool TryGetObject<T>(string Id, out T obj) where T : class
    {
        if(_objectsBuffer.ContainsKey(Id))
        {
            obj = _objectsBuffer[Id] as T;
            if(obj == null)
            {
                return false;
            }
            return true;
        }
        if(!_objectsDictionary.ContainsKey(Id))
        {
            obj = default(T);
            return false;
        }
        var initializingData = _objectsDictionary[Id];
        var type = Type.GetType(initializingData.Type);
        if(type == null)
        {
            obj = default(T);
            return false;
        }
        var constructors = type.GetConstructors();
        if(constructors.Length == 0)
        {
            obj = default(T);
            return false;
        }
        try
        {
            obj = constructors[0].Invoke(initializingData.Arguments.ToArray()) as T;
            if(obj == null)
            {
                return false;
            }
            _objectsBuffer[Id] = obj;
            return true;
        }
        catch(TargetParameterCountException ex)
        {
            Debug.Log("Can't create object " + Id + ": " + ex);
            obj = default(T);
            return false;
        }
    }
}
