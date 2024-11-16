using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public WeaponScriptable weaponConfigs;
    public Collider hitBox;
    public AudioSource meleeSFX; 
    [HideInInspector] public bool cooldown = false;

    public void Awake()
    {
        hitBox.isTrigger = true;
        hitBox.enabled = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        weaponConfigs.Attacks(other, meleeSFX);
    }

    public IEnumerator CooldownCounter()
    {
        cooldown = true;
        yield return new WaitForSeconds(weaponConfigs.SwingCooldown);
        cooldown = false; 
    }

    public void SwingSound()
    {
        weaponConfigs.PlaySwingSound(meleeSFX);
    }
}
