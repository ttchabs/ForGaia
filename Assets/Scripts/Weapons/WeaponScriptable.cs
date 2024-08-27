using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "NewItem", menuName = "Weapons/Weapon Container")]
public class WeaponScriptable : ScriptableObject
{
    [Header("WEAPON IDENTIFICATION:")]
    [Space(2)]
    public string _weaponName;
    public GameObject weaponModel;
    public MeleeWeaponType meleeType;

    [Header("WEAPON STATISTICS:")]
    [Space(2)]
    [SerializeField] int _weaponDamage;
    [SerializeField] float _weaponHitRange;
    [SerializeField] float _weaponAttackDelay;
    [SerializeField] float _swingCooldown;
    public LayerMask attackable;

    public IDamageable damaged;

    public string WeaponName { get { return _weaponName; } set { _weaponName = value; } }
    public int WeaponDamage { get { return _weaponDamage; } set { _weaponDamage = value; } }
    public float WeaponHitRange { get { return _weaponHitRange; } set { _weaponHitRange = value; } }

    public float WeaponAttackDelay { get { return _weaponAttackDelay; } set { _weaponAttackDelay = value; } }
    public float SwingCooldown { get { return _swingCooldown; } set { _swingCooldown = value; } }

    public void Attacking(Transform weaponBottom, Transform weaponTop)
    {
        /*  REFERENCE FOR ATTACK() METHOD:
            * Title: MEEE COMBAT In Unity
            * Author: Brackeys
            * Date: 24 August 2024
            * Code Version: 2.0
            * Availability: https://www.youtube.com/watch?v=sPiVz1k-fEs&list=PLmYpFgOST70FmSkspN5uvvl68cEjPI86x
        */
        Collider[] hitEntities = Physics.OverlapCapsule(weaponBottom.position, weaponTop.position, WeaponHitRange, attackable);
        foreach (Collider hitEnemy in hitEntities)
        {
            damaged = hitEnemy.GetComponent<IDamageable>();
            if (damaged != null)
            {
                damaged.DamageReceived(WeaponDamage);
            }
            Debug.Log($"Weapon: {WeaponName}, DMG: {WeaponDamage} ");
        }
    }

}
