using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventoryManager))]

public class Managers : MonoBehaviour
{
    public static InventoryManager Inventory { get; private set; }

    private List<IGameManager> _startSequence;

    void Awake()
    {
        Inventory = GetComponent<InventoryManager>();

        _startSequence = new List<IGameManager>();
        _startSequence.Add(Inventory);

        StartCoroutine(StartupManager());
    }

    private IEnumerator StartupManager()
    {
        foreach(IGameManager manager in _startSequence)
        {
            try
            {
                manager.Startup();
            }
            catch(System.Exception ex)
            {
                Debug.Log("Can't initialize managers: " + ex);
                Application.Quit();
            }
        }

        yield return null;

        int numModules = _startSequence.Count;
        int numReady = 0;

        while(numReady < numModules)
        {
            int lastReady = numReady;
            numReady = 0;

            foreach(IGameManager manager in _startSequence)
            {
                if(manager.status == ManagerStatus.Started)
                {
                    ++numReady;
                }
            }

            if(numReady > lastReady)
            {
                Debug.Log("Progress: " + numReady + "/" + numModules);
            }

            yield return null;
        }
        
        Debug.Log("All managers started up");
    }
}
