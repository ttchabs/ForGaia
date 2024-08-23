using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [Header("Health CONTROLS")]
    public int enemyCurrentHP;
    public int moveSpeed = 3;
    public Transform player;
    public Transform enemy;

    private int maxHP = 5;

    [Header("DAMAGE CONTROLS")]
    public int gjjb;

    public void Awake()
    {
        enemyCurrentHP = maxHP;

        //player = ;
        enemy = GetComponent<Transform>();
    }
/*    private void OnCollisionEnter(Collision damage)
    {
        if (damage.gameObject.CompareTag("MeleeWeapon"))
        {
            enemyMaxHealth -=1;
        }

        if (enemyMaxHealth == 0)
        {
            Destroy(gameObject);
        }
    }*/

    void Update()
    {
        float distanceBetween = Vector3.Distance(enemy.position, player.position);

        if (distanceBetween < 20f)
        {
            enemy.position = Vector3.MoveTowards(enemy.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    public void DamageDealtToEnemy(int damage)
    {
        enemyCurrentHP -= damage;
        if (enemyCurrentHP <= 0 ) 
        {
            EnemyDeath();
        }
    }

    public void EnemyDeath()
    {
        //Enemy death animations will be called below here

        //Destroy the gameObject
        Destroy(gameObject);
    }

}
