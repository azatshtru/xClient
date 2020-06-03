using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class LocalTransformSync : MonoBehaviour
{
    public float updateWaitTime = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SendPosition());
    }

    IEnumerator SendPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateWaitTime);
            //Vector3 posnToShare = new Vector3(Mathf.Round(transform.position.x * 100f) / 100f, Mathf.Round(transform.position.y * 100f) / 100f, 0);
            Client.Instance.SendVector(transform.position);
        }
    }
}
