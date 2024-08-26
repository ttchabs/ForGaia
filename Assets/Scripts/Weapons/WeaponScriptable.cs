using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Weapons/Weapon Attack Stats")]
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

    public string WeaponName { get { return _weaponName; } set { _weaponName = value; } }
    public int WeaponDamage {  get { return _weaponDamage; } set {  _weaponDamage = value; } }
    public float WeaponHitRange { get { return _weaponHitRange; } set { _weaponHitRange = value; } }
    public float WeaponAttackDelay { get { return _weaponAttackDelay; } set { _weaponAttackDelay = value; } }
    public float SwingCooldown { get { return _swingCooldown; } set { _swingCooldown = value; } }


}
