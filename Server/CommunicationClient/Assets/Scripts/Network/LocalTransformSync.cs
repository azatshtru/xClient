using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class LocalTransformSync : MonoBehaviour
{
    public float updateWaitTime = 0.2f;

    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        StartCoroutine(SendPosition());
    }

    IEnumerator SendPosition()
    {
        while (true)
        {
            Vector3 pos = transform.position;
            yield return new WaitForSeconds(updateWaitTime);
            if (Mathf.Abs((transform.position - pos).sqrMagnitude) < 0.02f)
            {
                continue;
            }
            //Vector3 posnToShare = new Vector3(Mathf.Round(transform.position.x * 100f) / 100f, Mathf.Round(transform.position.y * 100f) / 100f, 0);
            Client.Instance.SendVector(transform.position);
        }
    }

    public void SendProjectile(Vector3 pos, Vector3 dir)
    {
        Client.Instance.SendRay(pos, dir);
    }
}
