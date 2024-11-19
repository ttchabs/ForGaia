using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponScript : PickUpFunction
{
    [HideInInspector] public WeaponScriptable weaponConfigs;
    public Collider hitBox;
    public AudioSource meleeSFX; 
    bool _cooldown = false;

    public void Awake()
    {
        if(itemData is WeaponScriptable weapon)
        {
            weaponConfigs = weapon;
        }
    }

    public void Start()
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
            InventoryManager.Instance.playerHUDManager.meleeCooldownSlider.value = 0;
            StartCoroutine(CooldownCounter(weaponConfigs.SwingCooldown));//start swing cooldown
        }
    }

    public IEnumerator CooldownCounter(float timer)
    {
        var cdDisplay = InventoryManager.Instance.playerHUDManager;
        _cooldown = true;
        float endTime = 0;
        while(timer  > endTime)
        {
            cdDisplay.meleeCooldownSlider.value = Mathf.LerpAngle(cdDisplay.meleeCooldownSlider.value, 1.59f, timer * Time.deltaTime);
            endTime += Time.deltaTime;
            yield return null;
        }
        cdDisplay.meleeCooldownSlider.value = 1;
        _cooldown = false; 
        yield return null;
    }

    public void SwingSound()
    {
        weaponConfigs.PlaySwingSound(meleeSFX);
    }

    public void OnDisable()
    {
        StopCoroutine(CooldownCounter(weaponConfigs.SwingCooldown));
    }
}
