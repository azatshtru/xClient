using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public float shootVelocity = 8f;
    public float shootDelay = 0.4f;
    public float playerForce = 1000f;

    GameObject bulletPrefab;
    Plane plane;

    Rigidbody rb;
    LocalTransformSync sync;

    float nextShootTime;

    // Start is called before the first frame update
    void Start()
    {
        bulletPrefab = Resources.Load("Bullet") as GameObject;
        plane = new Plane(Vector3.forward, Vector3.zero);

        rb = GetComponent<Rigidbody>();
        sync = GetComponent<LocalTransformSync>();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float enter = 0.0f;
            if (plane.Raycast(ray, out enter))
            {
                Shoot(enter, ray);
            }
        }
#elif UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            float enter = 0.0f;
            if (plane.Raycast(ray, out enter))
            {
                Shoot(enter, ray);
            }
        }
#endif
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
            ForcePlayer(-dir.normalized);
            bulletGO.GetComponent<Bullet>().SetInfo(gameObject.name, dir.normalized, shootVelocity);

            nextShootTime = Time.time + shootDelay;
        }
    }

    void ForcePlayer (Vector3 dir)
    {
        rb.velocity = Vector3.zero;
        Vector3 force = dir * playerForce;
        rb.AddForce(force, ForceMode.Force);
    }
}
