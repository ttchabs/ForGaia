using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "NewItem", menuName = "Weapons/Melee Container")]
public class WeaponScriptable : ScriptableObject
{
    [Header("WEAPON IDENTIFICATION:")]
    [Space(2)]
    [SerializeField] string _weaponName;
    [TextArea(3, 3), SerializeField] string _weaponDescription;
    [SerializeField] GameObject _weaponModelPrefab;
    [SerializeField] Sprite _weaponSprite;
    public MeleeWeaponType meleeType;

    [Header("WEAPON STATISTICS:")]
    [Space(2)]
    [SerializeField] WeaponDamage _meleeDamage; [Space (3)]
    [SerializeField] float _weaponStanceBreak;
    [SerializeField] float _weaponWeight;
    [SerializeField] float _swingCooldown;
    public AnimatorOverrideController uniqueAnimation;
    public LayerMask attackable;

    public string WeaponName { get => _weaponName; } 
    public string WeaponDescription { get => _weaponDescription; }
    public GameObject WeaponModel { get => _weaponModelPrefab; }
    public WeaponDamage MeleeDamageRange { get => _meleeDamage; }
    public float WeaponWeight { get => _weaponWeight; }
    public float WeaponStanceBreak { get => _weaponStanceBreak; }

    public float SwingCooldown { get => _swingCooldown; }

    public void Attacking(Collider other)
    {             
        if (((1 << other.gameObject.layer) & attackable) != 0)
        {            
            IDamageable damaged = other?.GetComponent<IDamageable>();
            int damage = MeleeDamageRange.GetRandomDamage();
            damaged.DamageReceived(damage);
/*            if (damaged != null)
            {
                int damage = MeleeDamageRange.GetRandomDamage();
                damaged.DamageReceived(damage);
            }*/
            Debug.Log($"Weapon: {WeaponName}, DMG: {damage} ");
        }
    }

    public void Attacks(Collider other)
    {
        if (((1 << other.gameObject.layer) & attackable) != 0)
        {
            IDamageable damaged = other?.GetComponent<IDamageable>();
            int damage = MeleeDamageRange.GetRandomDamage();
            damaged.DamageReceived(damage);

            Debug.Log($"Weapon: {WeaponName}, DMG: {damage} ");
        }
    }
}
