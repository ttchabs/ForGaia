using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Characters/Enemy/Enemy Data Container")]
public class EnemyScriptable : ScriptableObject
{
    [Header("ENEMY IDENTIFICATION:")]
    [Space(2)]
    [SerializeField] string _enemyName;
    [SerializeField, TextArea(2,3)] string _enemyDescription;
    public GameObject enemyModelPrefab;
    public EnemyTypes enemyType;

    [Header("ENEMY STATISTICS:")]
    [SerializeField] int _maxEnemyHP;
    [SerializeField] EnemyDamage _enemyAttackDamage;
    [Space(2)]
    [SerializeField] float _enemyKnockbackFactor;
    [SerializeField] float _enemyMoveSpeed;
    [SerializeField] float _attackRate;
    public string EnemyName => _enemyName;
    public string EnemyDescription => _enemyDescription;
    public int MaxEnemyHP { get => _maxEnemyHP; }
    public EnemyDamage EnemyAttackDamage { get => _enemyAttackDamage; }
    public float EnemyKnockbackFactor { get => _enemyKnockbackFactor; }
    public float EnemyMoveSpeed { get => _enemyMoveSpeed;  }
    public float AttackRate { get => _attackRate; }

    public IEnumerator EnemyDeath(GameObject enemySpawn)
    {
        yield return new WaitForSeconds(2f);
        Destroy(enemySpawn);
    }
}
