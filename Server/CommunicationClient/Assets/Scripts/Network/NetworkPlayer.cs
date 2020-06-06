using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    public float shootVelocity = 20f;

    GameObject bulletPrefab;

    private void Start()
    {
        bulletPrefab = Resources.Load("Bullet") as GameObject;
    }

    public void UpdatePosition(Vector3 posn)
    {
        transform.position = posn;
    }

    public void InstantiateBullet(Vector3 pos, Vector3 dir)
    {
        GameObject bulletGO = Instantiate(bulletPrefab, pos, Quaternion.identity);
        bulletGO.GetComponent<Bullet>().SetInfo(gameObject.name, dir, shootVelocity);
    }
}
