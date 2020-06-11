using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    public float shootVelocity = 20f;
    public float playerForce = 1000f;

    GameObject bulletPrefab;
    Rigidbody rb;

    private void Start()
    {
        bulletPrefab = Resources.Load("Bullet") as GameObject;
        rb = GetComponent<Rigidbody>();
    }

    public void UpdatePosition(Vector3 posn)
    {
        transform.position = posn;
    }

    public void InstantiateBullet(Vector3 pos, Vector3 dir)
    {
        GameObject bulletGO = Instantiate(bulletPrefab, pos, Quaternion.identity);
        transform.position = pos;
        ForcePlayer(-dir.normalized);
        bulletGO.GetComponent<Bullet>().SetInfo(gameObject.name, dir, shootVelocity);
    }

    void ForcePlayer(Vector3 dir)
    {
        rb.velocity = Vector3.zero;
        Vector3 force = dir * playerForce;
        rb.AddForce(force, ForceMode.Force);
    }
}
