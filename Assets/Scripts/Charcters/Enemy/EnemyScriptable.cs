using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Enemy/Enemy Container")]
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
    public float enemyKnockbackFactor;
    public float enemyMoveSpeed;
    public float attackRate;

}
