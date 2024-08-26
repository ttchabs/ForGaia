using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{    
    public WeaponScriptable weaponConfigs;
    public Transform weaponBottom, weaponTop;

    public void Awake()
    {

    }

    public void Attack()
    {
/*  REFERENCE FOR ATTACK() METHOD:
    * Title: MEEE COMBAT In Unity
    * Author: Brackeys
    * Date: 24 August 2024
    * Code Version: 2.0
    * Availability: https://www.youtube.com/watch?v=sPiVz1k-fEs&list=PLmYpFgOST70FmSkspN5uvvl68cEjPI86x
*/

        Collider[] hitEntities = Physics.OverlapCapsule(weaponBottom.position, weaponTop.position, weaponConfigs.WeaponHitRange, weaponConfigs.attackable);
        foreach (Collider hitEnemy in hitEntities)
        {
/*            IDamageable damaged = hitEnemy.GetComponent<IDamageable>();
            if (damaged != null)
            {
                damaged.DamageReceived(weaponConfigs.WeaponDamage);
            }
            Debug.Log($"Weapon: {weaponConfigs.WeaponName}, DMG: {weaponConfigs.WeaponDamage} ");*/
            weaponConfigs.Attacking(weaponBottom, weaponTop);
        }

        
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(weaponBottom.position, weaponConfigs.WeaponHitRange);
        Gizmos.DrawWireSphere(weaponTop.position, weaponConfigs.WeaponHitRange);
        Gizmos.DrawLine(weaponTop.position, weaponBottom.position);
    }

}
