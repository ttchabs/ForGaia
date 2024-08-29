using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{    
    public WeaponScriptable weaponConfigs;
    public Transform weaponBottom, weaponTop;

    public void Attack()
    {
        weaponConfigs.Attacking(weaponBottom, weaponTop);
    }

    public void OnDrawGizmos()
    {
        //displays the radius of the capsule
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(weaponBottom.position, weaponConfigs.WeaponHitRange);
        Gizmos.DrawWireSphere(weaponTop.position, weaponConfigs.WeaponHitRange);

        //displays the height of the capsule
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(weaponTop.position, weaponBottom.position);
    }
}
