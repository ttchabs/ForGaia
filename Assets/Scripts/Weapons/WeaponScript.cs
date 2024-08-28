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
        weaponConfigs.Attacking(weaponBottom, weaponTop);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(weaponBottom.position, weaponConfigs.WeaponHitRange);
        Gizmos.DrawWireSphere(weaponTop.position, weaponConfigs.WeaponHitRange);
        Gizmos.DrawLine(weaponTop.position, weaponBottom.position);
    }

}
