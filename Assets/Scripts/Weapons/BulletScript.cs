using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : PickUpFunction
{
    public Rigidbody body;
    public delegate void OnHitEvent (GameObject bullet, Collider collision);
    public event OnHitEvent OnHit;

    private void OnEnable()
    {
        body = GetComponent<Rigidbody>();
    }
    public void OnTriggerEnter(Collider collision)
    {
        OnHit?.Invoke(gameObject, collision);
    }

    private void OnDestroy()
    {
        OnHit = null;
    }
}
