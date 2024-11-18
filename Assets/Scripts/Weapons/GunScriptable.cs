using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Weapons/Gun Statistics Container")]
public class GunScriptable : ItemScriptable
{
    [Header("GUN IDENTIFICATION:")]
    [Space(4)]
    public GunTypes gunTypes; //the type of shot the gun will perform

    [Header("GUN SFX:")]
    [Range(0f, 1f)]
    [SerializeField] float _volume;
    [SerializeField] AudioClip _gunFireSFX;
    [SerializeField] AudioClip _gunReloadSFX;

    [Header("GUN STATISTICS:")]
    [Space(4)]
    [SerializeField] float _projectileSpeed; //the rate at which the bullet travels
    [SerializeField] float _fireRate; // the amount of time elapsed between each gunfire
    [SerializeField] float _gunWeight;
    [SerializeField] int _magSize; //the maximum number of bullets a gun can hold
    [SerializeField] float _maxDistance; //The furhest distance the bullet is allowed to travel
    [SerializeField] AnimatorOverrideController _reloadSequence; //The animation that plays when reloading a gun.

    [Header("BULLET STATISTICS:")]
    [SerializeField] WeaponDamage _bulletDamage; [Space(2)] //amount of damage dealt by the bullet
    [SerializeField] GameObject _bulletPrefab; //the bullets that will be shot
    public LayerMask hitLayers; //only applicable if hitscan

    [Header("RECOIL SYSTEM:")]
    [Range(0f, 7f)] public float recoilX;
    [Range(0f,7f)] public float recoilY;
    public float currentX;
    public float currentY;

    //these make the variables above accessible to other scripts. reference these when necessary.
    public float Volume => _volume; 
    public AudioClip GunFireSFX => _gunFireSFX;
    public AudioClip GunReloadSFX => _gunReloadSFX;
    public GameObject BulletPrefab => _bulletPrefab;
    public float ProjectileSpeed => _projectileSpeed;
    public WeaponDamage BulletDamage => _bulletDamage;
    public float FireRate => _fireRate;
    public float GunWeight => _gunWeight;
    public int MagSize => _magSize;
    public float MaxDistance => _maxDistance;


    public void ProjectileShoot(Transform origin)
    {
        var fab = SpawnProjectile();
        var g = fab.GetComponent<BulletScript>();
        g.OnHit += ProjectileImpact; //subscribes to the bullet onHit event so that when the event is called onCollision, the BulleImpact function is called
        fab.transform.position = origin.position;

        // Get the Rigidbody component of the projectile and set its velocity
        Rigidbody rb = fab.GetComponent<Rigidbody>();
        rb.velocity = origin.forward * ProjectileSpeed + origin.up * ProjectileSpeed /10f;
    }

    public IEnumerator HitScanShooter(Transform origin) //Uses a raycast to find an attackable enemy
    {
        var fab = SpawnProjectile(); 
        fab.transform.position = origin.position;

        Ray bullet = new Ray (origin.position, origin.forward);
        RaycastHit hitCollider;
        Debug.DrawRay(origin.position, origin.forward * MaxDistance, Color.yellow, 5f);
        if (Physics.Raycast(bullet, out hitCollider, MaxDistance, hitLayers))
        {
            float displacement = Vector3.Distance(origin.position, hitCollider.point);
            Vector3 direction = (hitCollider.point - origin.position).normalized;
            float displacementOnHit = displacement;
            while (displacementOnHit > 0)
            {
                Vector3 moveToHit = ProjectileSpeed * Time.deltaTime * direction;
                fab.transform.Translate(moveToHit);
                displacementOnHit -= ProjectileSpeed * Time.deltaTime;
                yield return null;
            }

            if(hitCollider.collider.TryGetComponent(out IDamageable damageComponent))
            {
                damageComponent.DamageReceived(BulletDamage.GetDamage());
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
                Vector3 move = missDisplacement * Time.deltaTime * direction;
                fab.transform.Translate(move);
                displacementOnMiss -= ProjectileSpeed * Time.deltaTime;
                yield return null;
            }
            yield return null;
            Destroy(fab);
        }
    }


    public void ProjectileImpact(GameObject bullet, Collision collision) //projectiles will deal damage to the collided object on impact
    {
        if(collision.gameObject.TryGetComponent(out IDamageable damage))
        {
            damage.DamageReceived(BulletDamage.GetDamage());
            Destroy(bullet);
        }
        else
        {
            Destroy(bullet);
        }
    }

    public GameObject SpawnProjectile() //Spawns the projectile 
    {
        return Instantiate(BulletPrefab);
    }

    public int ReloadGun(AudioSource gunReloadedSound)
    {
        PlayGunReloadedSound(gunReloadedSound);
        return MagSize;
    }

    //----------
    public void PlayGunFireSound(AudioSource gunFireSource)
    {
        gunFireSource.PlayOneShot(GunFireSFX, Volume);
    }

    public void PlayGunReloadedSound(AudioSource gunReloadedSound)
    {
        gunReloadedSound.PlayOneShot(GunFireSFX, Volume);
    }
}
