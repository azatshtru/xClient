using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public float shootVelocity = 8f;
    public float shootDelay = 0.4f;

    GameObject bulletPrefab;
    Plane plane;

    LocalTransformSync sync;

    float nextShootTime;

    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = Resources.Load("Bullet") as GameObject;
        plane = new Plane(Vector3.forward, Vector3.zero);

        sync = GetComponent<LocalTransformSync>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float enter = 0.0f;
            if (plane.Raycast(ray, out enter))
            {
                Shoot(enter, ray);
            }
        }
    }

    void Shoot(float enter, Ray ray)
    {
        if(Time.time > nextShootTime)
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 dir = hitPoint - transform.position;

            if(sync != null)
            {
                sync.SendProjectile(transform.position, dir.normalized);
            }

            GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bulletGO.GetComponent<Bullet>().SetInfo(gameObject.name, dir.normalized, shootVelocity);

            nextShootTime = Time.time + shootDelay;
        }
    }
}
