using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, IDamageable
{

    [Header("STATS CONTROLS:")]
    public EnemyScriptable enemyConfigs; //data container for enemy stats
    public int enemyCurrentHP;

    [Header("ENEMY TRANSFORMS:")]
    public Transform player;
    private Rigidbody rb;

    [Header("ENEMY HEALTH DISPLAY")]
    public Slider enemyHealth;

    public event IDamageable.DamageReceivedEvent OnDamageReceived;


    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    public void Start()
    {
        SetMaxEnemyHP();        
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

            var player = collision.collider.GetComponent<IDamageable>(); //finds the IDamageable component on the player

            Vector3 direction = transform.forward * -enemyConfigs.EnemyKnockbackFactor;
            player.DamageReceived(enemyConfigs.EnemyAttackDamage);
            StartCoroutine(playerHP.KnockedBack(direction));
        }
    }

    public void DamageReceived(int damage)
    {
        enemyCurrentHP -= damage;
        UpdateEnemyHealthBar();
        OnDamageReceived?.Invoke(damage);
        BreakStance();
        if (enemyCurrentHP < 0)
            StartCoroutine(enemyConfigs.EnemyDeath(gameObject));
    }

    public void BreakStance()
    {
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        rb.AddForce(direction * -3, ForceMode.Impulse);
        
    }

    public void SetMaxEnemyHP()
    {
        enemyHealth.maxValue = enemyConfigs.MaxEnemyHP;
        UpdateEnemyHealthBar();
    }

    public void UpdateEnemyHealthBar()
    {
        enemyHealth.value = enemyCurrentHP;
    }

    public void OnDisable()
    {
        OnDamageReceived = null;
        rb = null;
    }
}
