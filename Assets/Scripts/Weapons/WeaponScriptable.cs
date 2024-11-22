using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Weapons/Melee Statistics Container")]
public class WeaponScriptable : ItemScriptable
{
    [Header("WEAPON IDENTIFICATION:")]
    public MeleeWeaponType meleeType; //weapon category   

    [Header("SWORD SFX:")]
    [Range(0f, 1f)]
    [SerializeField] float _volume;
    [SerializeField] AudioClip _slashSound;
    [SerializeField] AudioClip _swingSound;


    [Header("WEAPON STATISTICS:")]
    [Space(2)]
    [SerializeField] WeaponDamage _meleeDamage; [Space (3)] //random range of damage dealt
    [SerializeField] float _knockback; //the time in which the enemy will be stunned for
    [SerializeField] float _weaponWeight; //the heaviness of the weapon. slows you down as you walk
    [SerializeField] float _swingCooldown; //the amount of time that needs to elapse betfore next swing
    public AnimatorOverrideController uniqueAnimation; //the animation called when this weapon is swung

    public float Volume => _volume;
    public AudioClip SlashSound => _slashSound;
    public AudioClip SswingSound => _swingSound;
    public WeaponDamage MeleeDamageRange { get => _meleeDamage; }
    public float WeaponWeight { get => _weaponWeight; }
    public float Knockback { get => _knockback; }
    public float SwingCooldown { get => _swingCooldown; }

    public void Attacks(Collider other, AudioSource slash) //Attack function that is called i nthe weapon script
    {
        if (other.TryGetComponent(out IDamageable sliced))
        {
            int damage = MeleeDamageRange.GetDamage();
            sliced.DamageReceived(damage);
            PlaySlashSound(slash);
            KnockBack(other);
        }
    }

    public void KnockBack(Collider other)
    {
        var playerPos = FirstPersonControls.Instance;
        Rigidbody rb = other.GetComponent<Rigidbody>();
        Vector3 direction = playerPos.transform.forward;
        if(rb != null)
        rb.AddForce(direction * Knockback, ForceMode.Impulse);
    }

    public void PlaySlashSound(AudioSource slash) 
    {
        slash.PlayOneShot(SlashSound,Volume);
    }

    public void PlaySwingSound(AudioSource swing)
    {
        swing.PlayOneShot(_swingSound,Volume);
    }
}
