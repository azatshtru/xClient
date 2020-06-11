using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    string owner;
    Vector3 dir;
    float velocity;

    private void FixedUpdate()
    {
        transform.Translate(dir * velocity * Time.fixedDeltaTime);
    }

    public void SetInfo (string _owner, Vector3 _dir, float _velocity)
    {
        owner = _owner;
        dir = _dir;
        velocity = _velocity;

        StartCoroutine(DestroyThis());
    }

    IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(1.25f);
        Destroy(gameObject);
    }
}
