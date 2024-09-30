using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "NewItem", menuName = "Weapons/Gun Container")]
public class GunScriptable : ScriptableObject
{
    [Header("GUN IDENTIFICATION:")]
    [Space(4)]
    [SerializeField] string _gunName;
    [TextArea(2, 3), SerializeField] string _gunDescription;
    [SerializeField] GameObject _gunModel;
    [SerializeField] Sprite _gunSprite;
    public GunTypes gunTypes;

    [Header("GUN STATISTICS:")]
    [Space(4)]
    [SerializeField] float _projectileSpeed; //the rate at which the bullet travels
    [SerializeField] float _fireRate; // the amount of time elapsed between each gunfire
    [SerializeField] int _magSize; //the maximum number of bullets a gun can hold
    [SerializeField] float _maxDistance; //The furhest distance the bullet is allowed to travel
    [SerializeField] AnimatorOverrideController _reloadSequence; //The animation that plays when reloading a gun.

    [Header("BULLET STATISTICS:")]
    [SerializeField] WeaponDamage _bulletDamage; [Space(2)]
    [SerializeField] GameObject _bulletPrefab;
    public LayerMask hitLayers;
    
    public string GunName => _gunName;
    public string GunDescription => _gunDescription;
    public GameObject GunModel => _gunModel;
    public Sprite GunSprite => _gunSprite;

    public GameObject BulletPrefab => _bulletPrefab;
    public float ProjectileSpeed => _projectileSpeed;
    public WeaponDamage BulletDamage => _bulletDamage;
    public float FireRate => _fireRate;
    public int MagSize => _magSize;
    public float MaxDistance => _maxDistance;


    public void ProjectileShoot(Transform origin)
    {
        var fab = SpawnProjectile();
        var g = fab.GetComponent<BulletScript>();
        g.OnHit += BulletImpact; //subscribes to the bullet onHit event so that when the event is called onCollision, the BulleImpact function is called
        fab.transform.position = origin.position;

        // Get the Rigidbody component of the projectile and set its velocity
        Rigidbody rb = fab.GetComponent<Rigidbody>();
        rb.velocity = origin.forward * ProjectileSpeed + origin.up * ProjectileSpeed /10f;
    }

    public IEnumerator HitScanShooter(Transform origin) //Uses a raycast to find an attackable enemy
    {
        var fab = SpawnProjectile(); 
        fab.transform.position = origin.position;

        Ray bullet = new Ray(origin.position, origin.forward);
        RaycastHit hitCollider;
        Debug.DrawRay(origin.position, origin.forward * MaxDistance, Color.yellow, 5f);
        if (Physics.Raycast(bullet, out hitCollider, MaxDistance, hitLayers))
        {
            float displacement = Vector3.Distance(origin.position, hitCollider.point);
            Vector3 direction = (hitCollider.point - origin.position).normalized;
            float displacementOnHit = displacement;
            while (displacementOnHit > 0)
            {
                Vector3 moveToHit = direction * ProjectileSpeed * Time.deltaTime;
                fab.transform.Translate(moveToHit);
                displacementOnHit -= ProjectileSpeed * Time.deltaTime;
                yield return null;
            }

            if(hitCollider.collider.TryGetComponent(out IDamageable damageComponent))
            {
                damageComponent.DamageReceived(BulletDamage.GetRandomDamage());
            }
            fab.transform.position = hitCollider.point;
            yield return null;
            Destroy(fab);
        }
        else
        {

            float missDisplacement = Vector3.Distance(origin.position, origin.forward * MaxDistance);
            Vector3 direction = (origin.forward * MaxDistance - origin.position).normalized;
            float displacementOnMiss = missDisplacement;
            while (displacementOnMiss > 0)
            {
                Vector3 move = direction * missDisplacement * Time.deltaTime;
                fab.transform.Translate(move);
                displacementOnMiss -= ProjectileSpeed * Time.deltaTime;
                yield return null;
            }
            yield return null;
            Destroy(fab);
        }
    }

    public void ThrowerShoot(Transform origin)
    {

    }

    public void BulletImpact(GameObject bullet, Collision collision) //projectiles will deal damage to the collided object on impact
    {
        IDamageable damage = collision.collider.GetComponent<IDamageable>();
        if (damage != null)
        {
            damage.DamageReceived(BulletDamage.GetRandomDamage());
            Destroy(bullet.gameObject);
        }
        else
        {
            Destroy(bullet.gameObject);
        }
    }

    public void HitScanShoot(Transform origin)
    {
        Ray bullet = new Ray (origin.position, origin.forward);
        RaycastHit hitCollider;
        Debug.DrawRay(origin.position, origin.forward * MaxDistance, Color.yellow, 5f);
        if (Physics.Raycast(bullet, out hitCollider, MaxDistance))
        {
            if(hitCollider.collider.TryGetComponent(out IDamageable damageComponent))
            {
                damageComponent.DamageReceived(BulletDamage.GetRandomDamage());
            }
        }
        else
        {
            return;
        }
    }

    public GameObject SpawnProjectile() //Spawns the projectile 
    {
        return Instantiate(BulletPrefab);
    }
}
