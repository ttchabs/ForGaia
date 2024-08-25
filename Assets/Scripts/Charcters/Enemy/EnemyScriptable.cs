using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Enemy/Enemy")]
public class EnemyScriptable : ScriptableObject
{
    [Header("ENEMY IDENTIFICATION:")] 
    [Space(2)]
    public string enemyName;
    public GameObject enemyModelPrefab; 
    public EnemyTypes enemyType;

    [Header("ENEMY STATISTICS:")]
    public int maxEnemyHP;
    public int enemyAttackDamage;
    public float enemyKnockback;
    public float enemyMoveSpeed;
    public float attackRate;

    public void DamageDealtToEnemy(EnemyController enemyHP, int damage)
    {
        enemyHP.enemyCurrentHP -= damage;
        if (enemyHP.enemyCurrentHP <= 0)
            EnemyDeath(enemyHP);       
    }

    public void EnemyDeath(EnemyController enemyHP)
    {
        //Enemy death animations will be called below here

        //Destroy the gameObject
        Destroy(enemyHP.gameObject);
    }



}
