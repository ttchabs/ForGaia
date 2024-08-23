using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    /*Attack Wth Raycast:
     * This script uses rays to attack the enemy in front. 
     * This does mean that only one enemy can be hit at a time.
     */
    [Header("Weapon Raycast Ranges:")] [Space(5)]
    public GameObject meleeWeapon; 
    public Transform atkOrigin;
    [Space(5)]
    public float swordRange = 3.5f;
    public float knifeRange = 2.0f;
    public float longswordRange = 5f;
    [Space(5)]
    public FirstPersonControls player;

    public void SwordAttack()
    {
        Ray damageRay = new Ray(player.playerCamera.position, player.playerCamera.forward);
        RaycastHit hitEntity;

        Debug.DrawRay(player.playerCamera.position, player.playerCamera.forward * swordRange, Color.blue, 2f);
        if (Physics.Raycast(damageRay, out hitEntity, player.hitRange, player.attackable))
        {
            if (hitEntity.collider)
            {
                Debug.Log("Sworded");
            }
        }
    }

    public void KnifeAttack()
    {
        Ray damageRay = new Ray(player.playerCamera.position, player.playerCamera.forward);
        RaycastHit hitEntity;

        Debug.DrawRay(player.playerCamera.position, player.playerCamera.forward * knifeRange, Color.green, 2f);
        if (Physics.Raycast(damageRay, out hitEntity, player.hitRange, player.attackable))
        {
            if (hitEntity.collider)
            {
                Debug.Log("knifed");
            }
        }
    }

    public void LongswordAttack()
    {
        Ray damageRay = new Ray(player.playerCamera.position, player.playerCamera.forward);
        RaycastHit hitEntity;

        Debug.DrawRay(player.playerCamera.position, player.playerCamera.forward * longswordRange, Color.magenta, 2f);
        if (Physics.Raycast(damageRay, out hitEntity, player.hitRange, player.attackable))
        {
            if (hitEntity.collider)
            {
                Debug.Log("longsworded");
            }
        }
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(atkOrigin.position, swordRange);
    }
}
