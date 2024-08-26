using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    
    public WeaponScriptable weaponConfigs;
    public int weaponDamage;
    public float weaponHitRange;
    public float weaponCooldown;
    public Transform atkOrigin;

    public void Awake()
    {
        weaponDamage = weaponConfigs.WeaponDamage;
        weaponHitRange = weaponConfigs.WeaponHitRange;
        weaponCooldown = weaponConfigs.SwingCooldown;
        //atkOrigin = gameObject.transform.GetChild(0);
    }

    public void Attack()
    {
        string item = weaponConfigs.meleeType.ToString();
        Collider[] hitEntities = Physics.OverlapSphere(atkOrigin.position, weaponConfigs.WeaponHitRange, weaponConfigs.attackable);
        foreach (Collider hitEnemy in hitEntities)
        {
            EnemyController enemyHP = hitEnemy.GetComponent<EnemyController>();
            enemyHP.enemyConfigs.DamageDealtToEnemy(enemyHP, weaponConfigs.WeaponDamage);
            Debug.Log($"This is a {item} type");
            Debug.Log($"Weapon: {weaponConfigs.WeaponName}, DMG: {weaponConfigs.WeaponDamage} ");
        }
    }
    //Pls reference these Mudalo

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(atkOrigin.position, weaponConfigs.WeaponHitRange);
    }

}
