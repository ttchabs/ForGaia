using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GunScriptable gunConfigs;
    public Transform firePoint;
    public int currentMagAmount;
    public AudioSource gunSFX;

    float _lastShot = 0f;
    bool _reloading = false;

    private void Awake()
    {
        currentMagAmount = gunConfigs.MagSize;

    }
    public void Update()
    {
        if (_reloading) return;

        if (currentMagAmount == 0)
        {
            StartCoroutine(ReloadGun());
        }
    }

    public void GunTriggerPulled()
    {
        if (Time.time > _lastShot + gunConfigs.FireRate && currentMagAmount > 0 && _reloading == false)
        {
            _lastShot = Time.time;
            currentMagAmount--;
            gunConfigs.PlayGunFireSound(gunSFX);
            switch (gunConfigs.gunTypes)
            {
                case GunTypes.Projectile:
                    gunConfigs.ProjectileShoot(firePoint);
                    break;
                case GunTypes.HitScan:
                    StartCoroutine(gunConfigs.HitScanShooter(firePoint));
                    break;
                case GunTypes.Thrower:
                    gunConfigs.ThrowerShoot(firePoint);
                    break;
            }
        }
        else
        {
            return;
        }
    }

    public IEnumerator ReloadGun()
    {
        _reloading = true;
        yield return new WaitForSeconds(2f);
        gunConfigs.ReloadGun(currentMagAmount, gunSFX);
        _reloading = false;
        StopCoroutine(ReloadGun());
    }
}
