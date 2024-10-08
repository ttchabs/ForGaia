using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Rigidbody body;
    public delegate void OnHitEvent (GameObject bullet, Collision collision);
    public event OnHitEvent OnHit;

    private void OnEnable()
    {
        body = GetComponent<Rigidbody>();
    }
    public void OnCollisionEnter(Collision collision)
    {
        OnHit?.Invoke(gameObject, collision);
    }

    private void OnDisable()
    {
        OnHit = null;
    }
}
