using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public WeaponScriptable weaponConfigs;
    public Collider hitBox;
    [HideInInspector] public bool cooldown = false;


    public void Awake()
    {
        hitBox.isTrigger = true;
        hitBox.enabled = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        weaponConfigs.Attacking(other);
    }

    public IEnumerator CooldownCounter()
    {
        cooldown = true;
        yield return new WaitForSeconds(weaponConfigs.SwingCooldown);
        cooldown = false; 
    }

}
