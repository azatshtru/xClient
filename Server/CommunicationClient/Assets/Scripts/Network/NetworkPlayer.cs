using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    public void UpdatePosition(Vector3 posn)
    {
        transform.position = posn;
    }
}
