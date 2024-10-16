using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Enemy/Enemy Container")]
public class EnemyScriptable : ScriptableObject
{
    [Header("ENEMY IDENTIFICATION:")]
    [Space(2)]
    [SerializeField] string _enemyName;
    [SerializeField, TextArea(2,3)] string _enemyDescription;
    public GameObject enemyModelPrefab;
    public EnemyTypes enemyType;

    [Header("ENEMY STATISTICS:")]
    [SerializeField] float _maxEnemyHP;
    [SerializeField] float _enemyAttackDamage;
    [SerializeField] float _enemyKnockbackFactor;
    [SerializeField] float _enemyMoveSpeed;
    [SerializeField] float _attackRate;

    public string ENemyName => _enemyName;
    public string ENemyDescription => _enemyDescription;
    public float MaxEnemyHP { get => _maxEnemyHP; }
    public float EnemyAttackDamage { get => _enemyAttackDamage; }
    public float EnemyKnockbackFactor { get => _enemyKnockbackFactor; }
    public float EnemyMoveSpeed { get => _enemyMoveSpeed;  }
    public float AttackRate { get => _attackRate; }

}
