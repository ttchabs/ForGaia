using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInformation : MonoBehaviour
{
    /*Attack With Spherecast: 
     * This script casts a sphere which detects any overlapping colliders. 
     * This allows for multiple enemies to be hit at once. 
     */
    
    [Header("Weapon Raycast Ranges:")]
    public Transform atkOrigin;
    public float swordRange = 3.5f;
    public float knifeRange = 2.0f;
    public float longswordRange = 5f;

    public FirstPersonControls player;
    public EnemyController enemyHP;

    public void Awake()
    {
        player = FindObjectOfType<FirstPersonControls>();
        enemyHP = GetComponent<EnemyController>();
        atkOrigin = player.playerCamera.gameObject.GetComponentInChildren<Transform>();
    }
    public void SwordAttack()
    {
        Collider[] hitEntities = Physics.OverlapSphere(atkOrigin.position, swordRange, player.attackable);
        foreach (Collider collider in hitEntities)
        {
            Debug.Log("Sworded");
        }
    }

    public void KnifeAttack()
    {
        Collider[] hitEntities = Physics.OverlapSphere(atkOrigin.position, swordRange, player.attackable);
        foreach (Collider collider in hitEntities)
        {
            Debug.Log("Sworded");
        }
    }

    public void LongswordAttack()
    {
        Collider[] hitEntities = Physics.OverlapSphere(atkOrigin.position, swordRange, player.attackable);
        foreach (Collider collider in hitEntities)
        {
            Debug.Log("Sworded");
        }
    }
}
