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
    [Range(0f, 1f)] [SerializeField] float _volume;
    [SerializeField] AudioClip _deathSFX;
    [SerializeField] GameObject deathVFX;
    [SerializeField] GameObject damageVFX;

    [Header("ENEMY STATISTICS:")]
    [SerializeField] int _maxEnemyHP;
    [SerializeField] EnemyDamage _enemyAttackDamage;
    [Space(2)]
    [SerializeField] float _enemyKnockbackFactor;
    [SerializeField] float _enemyMoveSpeed;
    [SerializeField] float _attackRate;
    public string EnemyName => _enemyName;
    public string EnemyDescription => _enemyDescription;
    public AudioClip DeathSFX => _deathSFX;
    public float Volume => _volume;
    public int MaxEnemyHP => _maxEnemyHP; 
    public EnemyDamage EnemyAttackDamage => _enemyAttackDamage; 
    public float EnemyKnockbackFactor => _enemyKnockbackFactor;
    public float EnemyMoveSpeed => _enemyMoveSpeed;
    public float AttackRate => _attackRate;
    public GameObject DeathVFX => deathVFX;
    public GameObject DamageVFX => damageVFX;

    public IEnumerator EnemyDeath(GameObject enemySpawn, AudioSource deathSFX)
    {
        GameObject damage = Instantiate(DeathVFX, enemySpawn.transform);
        damage.transform.SetPositionAndRotation(enemySpawn.transform.position, Quaternion.identity);
        deathSFX.PlayOneShot(DeathSFX, Volume);
        yield return new WaitForSeconds(3f);
        Destroy(enemySpawn);
    }

    public IEnumerator DealDamage(Animator damageAnim, Transform position)
    {
        GameObject newVFX = Instantiate(DamageVFX, position);
        //newVFX.transform.position = position.position;
        newVFX.transform.SetPositionAndRotation(position.position, position.rotation);
        //deathSound.PlayOneShot(DeathSFX, Volume);
        damageAnim.SetTrigger("damageTaken");
        yield return new WaitForSeconds(0.7f);
        Destroy(newVFX);
    }
}
