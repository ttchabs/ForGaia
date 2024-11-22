using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GunScript : PickUpFunction
{
    public GunScriptable gunConfigs;
    public Transform firePoint;
    public int currentMagAmount;
    public AudioSource gunSFX;

    float _lastShot = 0f;
    bool _reloading = false;

    public void Awake()
    {
        if(itemData is GunScriptable gun)
        {
            gunConfigs = gun;
        }        
    }

    private void OnEnable()
    {
        if (currentMagAmount == 0)
        {
            _reloading = false;
            currentMagAmount = gunConfigs.ReloadGun(gunSFX);
        }
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
            }
            
            UIManager.Instance.hudControls.UpdateAmmoSlider(currentMagAmount);
        }
        else
        {
            return;
        }
    }

    public void RecoilMath(Transform camera)
    {
        transform.localPosition -= Vector3.forward * gunConfigs.recoilX;
        Quaternion recoilRot = Quaternion.Euler(-gunConfigs.recoilY, 0, 0);
        camera.localRotation *= recoilRot;
    }

    public IEnumerator ReloadGun()
    {
        _reloading = true;
        yield return new WaitForSeconds(2f);
        currentMagAmount = gunConfigs.ReloadGun(gunSFX);
        _reloading = false;
        UIManager.Instance.hudControls.UpdateAmmoSlider(gunConfigs.MagSize);
        StopCoroutine(ReloadGun());
    }

    public void OnDisable()
    {
        StopCoroutine(ReloadGun());
    }
}
