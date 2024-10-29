using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public PickUpScriptable PickupID;
    public GunScriptable gunConfigs;
    public Transform firePoint;
    public int currentMagAmount;
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
            _reloading = true;
            Invoke("ReloadGun", 3f);
            return;
        }
    }

    public void GunTriggerPulled()
    {
        if (Time.time > _lastShot + gunConfigs.FireRate && currentMagAmount > 0 && _reloading == false)
        {
            _lastShot = Time.time;
            currentMagAmount--;
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

    public void ReloadGun()
    {
        currentMagAmount = gunConfigs.MagSize;
        _reloading = false;
    }

    public void AddGunToInventory()
    {
        InventoryManager.Instance.AddItemToInventory(PickupID);
        gameObject.SetActive(false);
    }
}
