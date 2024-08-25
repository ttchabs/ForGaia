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
        float distanceBetween = Vector3.Distance(transform.position, player.position);
        
        Vector3 direction = transform.position - player.position;
        direction.y = 0f;

        if (distanceBetween < 20f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, enemyConfigs.enemyMoveSpeed * Time.deltaTime);
            Quaternion lookDirection = Quaternion.LookRotation(direction);
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
            playerHP.Knockback(transform);

        }
    }


}
