using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Weapons/Weapon Attack Stats")]
public class WeaponScriptable : ScriptableObject
{
    [Header("WEAPON IDENTIFICATION:")]
    [Space(2)]
    public string _weaponName;
    public GameObject weaponModel;
    public MeleeWeaponType meleeType;

    [Header("WEAPON STATISTICS:")]
    [Space(2)]
    [SerializeField] int _weaponDamage;
    [SerializeField] float _weaponHitRange;
    [SerializeField] float _swingCooldown;
    public LayerMask attackable;

    public string weaponName { get { return _weaponName; } set { _weaponName = value; } }
    public int weaponDamage {  get { return _weaponDamage; } set {  _weaponDamage = value; } }
    public float weaponHitRange { get { return _weaponHitRange; } set { _weaponHitRange = value; } }
    public float swingCooldown { get { return _swingCooldown; } set { _swingCooldown = value; } }

    public void Attack(Transform atkOrigin)
    {
        string item = meleeType.ToString();
        Collider[] hitEntities = Physics.OverlapSphere(atkOrigin.transform.position, weaponHitRange, attackable);
        foreach (Collider hitEnemy in hitEntities)
        {
            EnemyController enemyStats = hitEnemy.GetComponent<EnemyController>();
            enemyStats.enemyConfigs.DamageDealtToEnemy(enemyStats, weaponDamage);
            //DamageDealtToEnemy(enemyStats, weaponStats.weaponDamage);
            Debug.Log($"This is a {item} type");
            Debug.Log($"Weapon: {weaponName}, DMG: {weaponDamage} ");
        }
    }
}
