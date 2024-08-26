using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [Header("HEALTH CONTROLS:")]
    public EnemyScriptable enemyConfigs;
    public int enemyCurrentHP;

    [Header("ENEMY TRANSFORMS:")]
    public Transform player;
    //public Transform enemy;

    public void Awake()
    {
        enemyCurrentHP = enemyConfigs.maxEnemyHP;
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
            transform.position = Vector3.MoveTowards(transform.position, player.position, enemyConfigs.enemyMoveSpeed * Time.deltaTime); //makes the enemy move towards the player
            Quaternion lookDirection = Quaternion.LookRotation(direction); //makes the enemy face the player
            transform.rotation = lookDirection;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            FirstPersonControls playerHP = collision.collider.GetComponent<FirstPersonControls>();
            playerHP.playerConfigs.LoseHP(playerHP, enemyConfigs.enemyAttackDamage);
            Debug.Log($"{enemyConfigs.enemyAttackDamage} Damage was Taken");

            Vector3 direction = Vector3.forward;
            playerHP.Knockback(direction);

        }
    }


}
