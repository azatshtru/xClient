using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSpawn : MonoBehaviour
{
    bool hasSpawned;

    string spawnPointName;

    private void Start()
    {
        spawnPointName = gameObject.name;
    }

    public void SetSpawned()
    {
        Client.Instance.SendString(spawnPointName);
        hasSpawned = true;
    }

    public bool GetSpawned()
    {
        return hasSpawned;
    }

    public void SetSpawnedNetwork()
    {
        hasSpawned = true;
    }

    public string GetName()
    {
        return spawnPointName;
    }
}
