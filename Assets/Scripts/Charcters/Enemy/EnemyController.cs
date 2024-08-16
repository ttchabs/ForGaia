using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int enemyMaxHealth = 2;
    public int moveSpeed = 3;
    public Transform player;
    public Transform enemy;


    private void OnCollisionEnter(Collision damage)
    {
        if (damage.gameObject.CompareTag("MeleeWeapon"))
        {
            enemyMaxHealth -=1;
            
            
        }

        if (enemyMaxHealth == 0)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        float distanceBetween = Vector3.Distance(enemy.position, player.position);

        if (distanceBetween < 20f)
        {
            enemy.position = Vector3.MoveTowards(enemy.position, player.position, moveSpeed * Time.deltaTime);
        }
    }
}
