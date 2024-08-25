using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    
    public WeaponScriptable weaponConfigs;
    public int weaponDamage;
    public float weaponHitRange;
    public float weaponCooldown;

    //public FirstPersonControls playerStats;

    public void OnEnable()
    {
        weaponDamage = weaponConfigs.weaponDamage;
        weaponHitRange = weaponConfigs.weaponHitRange;
        weaponCooldown = weaponConfigs.swingCooldown;
    }

    //Pls reference these Mudalo

    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, weaponHitRange);
    }
}
