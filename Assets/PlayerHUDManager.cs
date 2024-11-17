using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDManager : MonoBehaviour
{

    [Header("HEALTH DISPLAY:")]
    public Slider healthBar;

    [Header("WEAPON WHEEL DISPLAY:")]
    public TextMeshProUGUI gunAmmoText;
    public Image currentWeaponInHand;
    public Image secondaryWeapon;
    public Image mouseInput;

    public Sprite[] weaponSwitchDisplays;

    [Header("POTION WHEEL DISPLAY:")]
    public Image HealthGrubDisplay;
    public TextMeshProUGUI GrubCountText;

    [Header("APPROACH ITEM UI:")]
    public TextMeshProUGUI pickUpText;

    public void SetMaxSliderHPValue(int maxValue)
    {
        healthBar.maxValue = maxValue;
    }

    public void SetCurrentHPValue(int currentValue)
    {
        healthBar.value = currentValue;
    }

    public void PickUpGrub()
    {
        HealthGrubDisplay.gameObject.SetActive(true);
        FirstPersonControls.Instance.grubCount++;
        GrubCountText.text = $"{FirstPersonControls.Instance.grubCount}";
    }

    public void UseGrub()
    {

    }

    //----------

    public void UsingGunDisplay()
    {
        gunAmmoText.gameObject.SetActive(true);
        currentWeaponInHand.sprite = weaponSwitchDisplays[1];
        secondaryWeapon.sprite = weaponSwitchDisplays[0];
    }

    public void UsingMeleeDisplay()
    {
        gunAmmoText.gameObject.SetActive(false);
        currentWeaponInHand.sprite = weaponSwitchDisplays[0];
        secondaryWeapon.sprite = weaponSwitchDisplays[1];
    }

    public void UpdateGunAmmo(int currentAmount, int maxAmount)
    {
        gunAmmoText.text = $"{currentAmount} / {maxAmount}";
    }


}
