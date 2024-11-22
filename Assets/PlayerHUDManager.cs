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
    public CanvasGroup gunAmmoSliderAlpha;
    public Slider gunAmmoSlider;
    public CanvasGroup meleeCooldownSliderAlpha;
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
        UpdateGrubUI();
    }

    public void UpdateGrubUI()
    {
        if (grubCount > 0)
        {
            GrubCountText.text = $"{grubCount}";
        }
        else if (grubCount <= 0) 
        {
            GrubCountText.text = null;
            HealthGrubDisplay.gameObject.SetActive(false);
        }
    }

    public void UseGrub()
    {
        grubCount--;
        UpdateGrubUI();
    }

    //----GUN RELATED UI----/

    public void UsingGunDisplay()
    {
        gunAmmoSliderAlpha.alpha = 1;
        meleeCooldownSliderAlpha.alpha = 0;
       
        currentWeaponInHand.sprite = weaponSwitchDisplays[1];
        secondaryWeapon.sprite = weaponSwitchDisplays[0];
    }
    public void SetMaxAmmo(int magSize)
    {
        gunAmmoSlider.maxValue = magSize * 2;
    }

    public void UpdateAmmoSlider(int currentAmount)
    {
        gunAmmoSlider.value = currentAmount;
    }

    //----MELEE RELATED UI----//

    public void UsingMeleeDisplay()
    {
        meleeCooldownSliderAlpha.alpha = 1;
        gunAmmoSliderAlpha.alpha = 0;

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
        if (info.collider.TryGetComponent(out PickUpFunction name) && !info.collider.CompareTag("PowerUp") || info.collider.CompareTag("SorterPuzzleStone") && !info.collider.CompareTag("PowerUp"))
        {
            pickUpText.text = "(E)";
        }
        else if (info.collider.TryGetComponent(out ChestOpenScript chest) && !info.collider.CompareTag("PowerUp"))
        {
            pickUpText.text = "Hit To Open!";
        }
        
        else if (info.collider.CompareTag("PowerUp"))
        {
            pickUpText.text = "walk into health grub (H to use)";
        }
    }





}
