using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Dictionary<string, NetworkPlayer> networkPlayers = new Dictionary<string, NetworkPlayer>();

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    public void InstantiateNetworkPlayers()
    {
        NetworkPlayer[] netPlayers = FindObjectsOfType<NetworkPlayer>();
        foreach(NetworkPlayer player in netPlayers)
        {
            networkPlayers.Add(player.gameObject.name, player);
        }
    }

    public void HandleVectors (string _name, Vector3 posn)
    {
        networkPlayers[_name].UpdatePosition(posn);
    }

    public void HandleRays (string _name, Vector3 pos, Vector3 dir)
    {
        networkPlayers[_name].InstantiateBullet(pos, dir);
    }
}
