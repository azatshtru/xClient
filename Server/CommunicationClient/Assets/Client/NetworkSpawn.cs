using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSpawn : MonoBehaviour
{
    bool hasSpawned;

    public void SetSpawned()
    {
        hasSpawned = true;
    }

    public bool GetSpawned()
    {
        return hasSpawned;
    }
}
