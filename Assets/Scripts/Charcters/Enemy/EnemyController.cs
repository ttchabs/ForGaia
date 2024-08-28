using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{

    [Header("HEALTH CONTROLS:")]
    public EnemyScriptable enemyConfigs;
    public int enemyCurrentHP;

    [Header("ENEMY TRANSFORMS:")]
    public Transform player;

    private Rigidbody rb;

    public event IDamageable.DamageReceivedEvent OnDamageReceived;


    public void Awake()
    {
        enemyCurrentHP = enemyConfigs.MaxEnemyHP;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>(); 
    }

    void Update()
    {
        TrackPlayer();
    }

    public void TrackPlayer()
    {
        float distanceBetween = Vector3.Distance(transform.position, player.position); //checks the distance between the player and the enemy, stores the value
        
        Vector3 direction = transform.position - player.position; //checks the coords of the player while ignoring the y-axis
        direction.y = 0f;

        if (distanceBetween < 20f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, enemyConfigs.EnemyMoveSpeed * Time.deltaTime); //makes the enemy move towards the player
            Quaternion lookDirection = Quaternion.LookRotation(direction); //makes the enemy face the player
            transform.rotation = lookDirection;
            
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            var playerHP = collision.collider.GetComponent<FirstPersonControls>(); // finds the FPC script on the player tag game object

            var player = collision.collider.GetComponent<IDamageable>(); //

            Vector3 direction = transform.forward * -enemyConfigs.EnemyKnockbackFactor;
            player.DamageReceived(enemyConfigs.EnemyAttackDamage);
            StartCoroutine(playerHP.KnockedBack(direction));
        }
    }


    public void DamageReceived(int damage)
    {
        enemyCurrentHP -= damage;
        EnemyKnocked();
        OnDamageReceived?.Invoke(damage);

        if (enemyCurrentHP < 0)
            EnemyDeath();
    }

    public void EnemyDeath()
    {
        //Enemy death animations will be called below here

        //Destroy the gameObject
        Destroy(gameObject);
    }

    public void EnemyKnocked()
    {
        var weapon = player.gameObject.GetComponent<FirstPersonControls>();
        Vector3 direct = player.forward * weapon.meleeAttacks.weaponConfigs.WeaponKnockback;
        rb.AddForce(direct, ForceMode.Impulse);
    }

}
