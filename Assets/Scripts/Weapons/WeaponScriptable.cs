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
    [SerializeField] string _weaponName; //Name of the weapon
    [TextArea(3, 3), SerializeField] string _weaponDescription; //lore behind the weapon
    [SerializeField] GameObject _weaponModelPrefab; //Prefab of the weapon for instantiation
    [SerializeField] Sprite _weaponSprite; //2d image of the weapon for the UI aspect
    public MeleeWeaponType meleeType; //weapon category

    [Header("WEAPON STATISTICS:")]
    [Space(2)]
    [SerializeField] WeaponDamage _meleeDamage; [Space (3)] //random range of damage dealt
    [SerializeField] float _weaponStanceBreak; //the time in which the enemy will be stunned for
    [SerializeField] float _weaponWeight; //the heaviness of the weapon. slows you down as you walk
    [SerializeField] float _swingCooldown; //the amount of time that needs to elapse betfore next swing
    public AnimatorOverrideController uniqueAnimation; //the animation called when this weapon is swung
    public LayerMask attackable; //the layers which the weapon hitBox can interact with

    //these make the variables above accessible to other scripts. reference these when necessary.
    public string WeaponName { get => _weaponName; } 
    public string WeaponDescription { get => _weaponDescription; }
    public GameObject WeaponModel { get => _weaponModelPrefab; }
    public WeaponDamage MeleeDamageRange { get => _meleeDamage; }
    public float WeaponWeight { get => _weaponWeight; }
    public float WeaponStanceBreak { get => _weaponStanceBreak; }
    public Sprite WeaponSprite { get => _weaponSprite; }
    public float SwingCooldown { get => _swingCooldown; }

    public void Attacking(Collider other) //Attacking function. called in the WeaponScript.
    {             
        if (((1 << other.gameObject.layer) & attackable) != 0) //checks what layers are being attacked
        {            
            IDamageable damaged = other?.GetComponent<IDamageable>();
            int damage = MeleeDamageRange.GetRandomDamage();
            damaged.DamageReceived(damage);
            Debug.Log($"Weapon: {WeaponName}, DMG: {damage} ");
        }
    }

    public void Attacks(Collider other)
    {
        if (other.TryGetComponent(out IDamageable sliced))
        {
            int damage = MeleeDamageRange.GetRandomDamage();
            sliced.DamageReceived(damage);
            Debug.Log($"Weapon Name: {WeaponName}, DMG Dealt: {damage} ");
        }
    }
}
