using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponScript : PickUpFunction
{
    public WeaponScriptable weaponConfigs;
    public Collider hitBox;
    public AudioSource meleeSFX; 
    bool _cooldown = false;

    public void Awake()
    {
        hitBox.isTrigger = true;
        hitBox.enabled = false;
    }

    public void OnEnable()
    {
        _cooldown = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        weaponConfigs.Attacks(other, meleeSFX);
    }

    public void SwordSwung(Animator animator)
    {
        if(_cooldown == false)
        {
            _cooldown = true;
            animator.runtimeAnimatorController = weaponConfigs.uniqueAnimation;
            //checks what is currently in the player's melee hand and activates an animation based on the weapon type
            switch (weaponConfigs.meleeType)
            {
                case MeleeWeaponType.Light:
                    animator.SetTrigger("LightWeaponAttack");
                    break;
                case MeleeWeaponType.Medium:
                    animator.SetTrigger("MediumWeaponAttack");
                    break;
                case MeleeWeaponType.Heavy:
                    animator.SetTrigger("HeavyWeaponAttack");
                    break;
            }
            SwingSound();
            StartCoroutine(CooldownCounter());//start swing cooldown
        }
    }

    public IEnumerator CooldownCounter()
    {
        _cooldown = true;
        yield return new WaitForSeconds(weaponConfigs.SwingCooldown);
        _cooldown = false; 
    }

    public void SwingSound()
    {
        weaponConfigs.PlaySwingSound(meleeSFX);
    }

    public override void MeleePickUp(WeaponScriptable meleeData)
    {
        base.MeleePickUp(meleeData);
    }

    public void OnDisable()
    {
        StopCoroutine(CooldownCounter());
    }
}
