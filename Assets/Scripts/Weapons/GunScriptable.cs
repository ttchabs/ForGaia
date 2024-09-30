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
    [SerializeField] float _projectileSpeed;
    [SerializeField] float _fireRate;
    [SerializeField] int _magSize;
    [SerializeField] float _maxDistance;


    [Header("BULLET STATISTICS:")]
    [SerializeField] WeaponDamage _bulletDamage; [Space(2)]
    [SerializeField] GameObject _bulletPrefab;
    public LayerMask hitLayers;
    public List<GameObject> _pool;
    public ExplosiveDamage _explosiveDamage;
    
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
        //var g = fab.GetComponent<BulletScript>();
        //g.OnHit += BulletImpact;
        fab.transform.position = origin.position;

        // Get the Rigidbody component of the projectile and set its velocity
        Rigidbody rb = fab.GetComponent<Rigidbody>();
        rb.velocity = origin.forward * ProjectileSpeed;
    }

    public IEnumerator HitScanShooter(Transform origin)
    {
        var fab = SpawnProjectile();
        fab.transform.position = origin.position;

        Ray bullet = new Ray(origin.position, origin.forward);
        RaycastHit hitCollider;
        Debug.DrawRay(origin.position, origin.forward * MaxDistance, Color.yellow, 5f);
        if (Physics.Raycast(bullet, out hitCollider, MaxDistance, hitLayers))
        {
            float displacement = Vector3.Distance(origin.position, hitCollider.point);
            float timer = displacement / ProjectileSpeed;
            float displacementOnHit = displacement;
            while (displacementOnHit > 0)
            {
                fab.transform.position = Vector3.Lerp(origin.position, hitCollider.point, Mathf.Clamp01(1 - displacementOnHit / displacement));
                displacementOnHit -= ProjectileSpeed * Time.deltaTime;
                yield return null;
            }

            if(hitCollider.collider.TryGetComponent(out IDamageable damageComponent))
            {
                damageComponent.DamageReceived(BulletDamage.GetRandomDamage());
            }

            fab.transform.position = hitCollider.point;

            yield return new WaitForSeconds(timer);
            Destroy(fab);
        }
        else
        {
            float missDisplacement = Vector3.Distance(origin.position, origin.forward * MaxDistance);
            float travelTime = missDisplacement / ProjectileSpeed;
            float displacementOnMiss = missDisplacement;
            while (missDisplacement < MaxDistance)
            {
                fab.transform.position += Vector3.Lerp(origin.position, origin.forward * MaxDistance, Mathf.Clamp01(1 - displacementOnMiss / missDisplacement));
                displacementOnMiss -= ProjectileSpeed * Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(travelTime);
            Destroy(fab);
        }

    }

    public void ThrowerShoot(Transform origin)
    {

    }

    public void BulletImpact(GameObject bullet, Collision collision)
    {
        IDamageable damage = collision.collider.GetComponent<IDamageable>();
        if (damage != null)
        {
            damage.DamageReceived(BulletDamage.GetRandomDamage());
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

    public GameObject SpawnProjectile()
    {
        return Instantiate(BulletPrefab);
    }
}
