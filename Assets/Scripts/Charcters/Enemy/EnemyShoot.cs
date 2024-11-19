using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public Transform player;
    public GameObject windBall;
    public Transform firePoint;

    public float rotationSpeed;
    public float fireRate;
    public float windSpeeed;
    public float distance;
    public float shootAgain;

    public float destroyBullet;

    public Animator birdAnimation;
    

// Start is called before the first frame update


    // Update is called once per frame
    public void Update()
    {
        LookAtPlayer();
        if ( player == null )
        
        return;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
       

        if (distanceToPlayer <= distance && Time.time >= shootAgain) 
        {
            Shoot();
            shootAgain = Time.time + fireRate; 

        }

    }


    void Shoot()

    {
        birdAnimation.SetTrigger("Attack");
        GameObject wind = Instantiate(windBall,firePoint.position, firePoint.rotation);
        Rigidbody rb = wind.GetComponent<Rigidbody>();

        if(rb != null )
        {
            rb.velocity = (player.position - firePoint.position).normalized * windSpeeed;    
        }

        Destroy(wind,3f);
        
        
    }


    public void LookAtPlayer() 
    {
        Vector3 directioToPlayer = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directioToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
    }


}
