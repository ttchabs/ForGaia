using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "NewItem", menuName = "Weapons/Melee Statistics Container")]
public class WeaponScriptable : ScriptableObject
{
    [Header("WEAPON IDENTIFICATION:")]
    [Space(2)]
    /*    [SerializeField] string _weaponName; //Name of the weapon
        [TextArea(3, 3), SerializeField] string _weaponDescription; //lore behind the weapon
        [SerializeField] GameObject _weaponModelPrefab; //Prefab of the weapon for instantiation
        [SerializeField] Sprite _weaponSprite; //2d image of the weapon for the UI aspect*/
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

    //these make the variables above accessible to other scripts. reference these when necessary.
    /*    public string WeaponName { get => _weaponName; } 
        public string WeaponDescription { get => _weaponDescription; }
        public GameObject WeaponModel { get => _weaponModelPrefab; }
        public Sprite WeaponSprite { get => _weaponSprite; }*/

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
            int damage = MeleeDamageRange.GetRandomDamage();
            sliced.DamageReceived(damage);
            PlaySlashSound(slash);
            KnockBack(other);
        }
    }

    public void KnockBack(Collider other)
    {
        var playerPos = FirstPersonControls.Instance;
        Vector3 direction = playerPos.transform.forward;
        Rigidbody rb = other.GetComponent<Rigidbody>();
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
