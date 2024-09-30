using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GunScriptable gunConfigs;
    public Transform firePoint;
    public int currentMagAmount;
    float lastShot = 0f;
    public bool reloading = false;

    private void Awake()
    {
        currentMagAmount = gunConfigs.MagSize;

    }
    public void Update()
    {
        if (currentMagAmount <= 0)
        {
            reloading = true;
            Invoke("Reload", 3f);
        }
    }

    public void GunTriggerPulled()
    {
        if (Time.time > lastShot + gunConfigs.FireRate && reloading == false)
        {
            lastShot = Time.time;
            currentMagAmount--;
            switch (gunConfigs.gunTypes)
            {
                case GunTypes.Projectile:
                    gunConfigs.ProjectileShoot(firePoint);
                    break;
                case GunTypes.HitScan:
                    //gunConfigs.HitScanShoot(firePoint);
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

    public void ShootForce()
    {
        gunConfigs.ProjectileShoot(firePoint);
    }

    public void Reload()
    {
        //reloading = false;
        currentMagAmount = gunConfigs.MagSize;
        reloading = false;

    }
}
