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
    [SerializeField] int _maxEnemyHP;
    [SerializeField] int _enemyAttackDamage;
    [SerializeField] float _enemyKnockbackFactor;
    [SerializeField] float _enemyMoveSpeed;
    [SerializeField] float _attackRate;

    public int MaxEnemyHP { get => _maxEnemyHP; set => _maxEnemyHP = value; }
    public int EnemyAttackDamage { get => _enemyAttackDamage; set => _enemyAttackDamage = value; }
    public float EnemyKnockbackFactor { get => _enemyAttackDamage; set => _enemyKnockbackFactor = value; }
    public float EnemyMoveSpeed { get => _enemyMoveSpeed; set => _enemyMoveSpeed = value; }
    public float AttackRate { get => _attackRate; set => _attackRate = value; }
}
