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
    public Image currentWeaponInHand;
    public Image secondaryWeapon;
    public Image mouseInput;

    public Sprite[] weaponSwitchDisplays;

    [Header("POTION WHEEL DISPLAY:")]
    public Image HealthGrubDisplay;
    public TextMeshProUGUI GrubCountText;
    public int grubCount;

    [Header("APPROACH ITEM UI:")]
    public TextMeshProUGUI pickUpText;

    [Header("CROSSHAIRS DISPLAY")]
    public Image crosshairs;
    public Slider gunAmmoSlider;
    public Slider meleeCooldownSlider;


    //----HP RELATED FUNCTIONS----//
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
        grubCount++;
    }

    public void UpdateGrubUI()
    {
        if (grubCount > 0)
        {
            GrubCountText.text = $"{FirstPersonControls.Instance.grubCount}";
        }
        else if (grubCount <= 0) 
        { 
            HealthGrubDisplay.gameObject.SetActive(false);
        }
    }

    public void UseGrub()
    {

    }

    //----GUN RELATED UI----/

    public void UsingGunDisplay()
    {
        gunAmmoSlider.gameObject.SetActive(true);
        meleeCooldownSlider.gameObject.SetActive(false);
       
        currentWeaponInHand.sprite = weaponSwitchDisplays[1];
        secondaryWeapon.sprite = weaponSwitchDisplays[0];
    }
    public void SetMaxAmmo(int magSize)
    {
        gunAmmoSlider.maxValue = magSize * 2;
        UpdateAmmoSlider(magSize);
    }

    public void UpdateAmmoSlider(int currentAmount)
    {
        gunAmmoSlider.value = currentAmount;
    }

    //----MELEE RELATED UI----//

    public void UsingMeleeDisplay()
    {
        meleeCooldownSlider.gameObject.SetActive(true);
        gunAmmoSlider.gameObject.SetActive(false);

        currentWeaponInHand.sprite = weaponSwitchDisplays[0];
        secondaryWeapon.sprite = weaponSwitchDisplays[1];
    }
    public IEnumerator FillCooldonBar(float time)
    {
       
        meleeCooldownSlider.value = 0;
        float endTime = 0;
        while(time > endTime)
        {
            endTime += Time.deltaTime;
            //meleeCooldownSlider.value = Mathf.Lerp(meleeCooldownSlider.value, 1, time * Time.deltaTime);
            meleeCooldownSlider.value = Mathf.LerpAngle(meleeCooldownSlider.value, 180, time * Time.deltaTime);
            yield return null;
        }
        meleeCooldownSlider.value = 1;
    }

    public void Interactable(RaycastHit info)
    {
        if (info.collider.TryGetComponent(out PickUpFunction name))
        {
            pickUpText.text = "(E)";
        }
        else
        {
            pickUpText.text = null;
        }
    }





}
