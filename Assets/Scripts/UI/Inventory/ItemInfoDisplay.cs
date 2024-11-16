using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoDisplay : MonoBehaviour
{
    public static ItemInfoDisplay Instance;

    [Header("DISPLAY:")]
    public GameObject InfoPanel;
    [Space(2)]
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;
    public TextMeshProUGUI text4;
    public TextMeshProUGUI text5;
    public TextMeshProUGUI text6;

    [Header("BUTTONS:")]
    [Space(2)]
    public Button exitButton;

    public void Awake()
    {
        Instance = this;
    }
    public void ItemDisplayFunction(ItemScriptable pickUpInfo)
    {
        InfoPanel.SetActive(true);
        Name.text = $"{pickUpInfo.ItemName}";
        Description.text = $"{pickUpInfo.ItemDescription}";

        text1.text = null;
        text2.text = null;
        text3.text = null;
        text4.text = null;
        text5.text = null;
        text6.text = null;
    }

    public void MeleeDisplayFunction(ItemScriptable meleePickUp, WeaponScriptable meleeDescriptionInfo) 
    { 
        InfoPanel.SetActive(true);
        Name.text = $"{meleePickUp.ItemName}";
        Description.text = $"{meleePickUp.ItemDescription}";

        text1.text = "Damage:  " + $"{meleeDescriptionInfo.MeleeDamageRange.minDamage} - {meleeDescriptionInfo.MeleeDamageRange.maxDamage}";
        text2.text = "Knockback:  " + $"{meleeDescriptionInfo.Knockback}";
        text3.text = "Weight:  " + $"{meleeDescriptionInfo.WeaponWeight} KG";
        text4.text = "Sword Class:  " + $"{meleeDescriptionInfo.meleeType}";
        text5.text = "Swing Cooldown:  " + $"{meleeDescriptionInfo.SwingCooldown}";
        text6.text = null ;
    }

    public void GunDisplayFunction(ItemScriptable gunPickUp, GunScriptable gunDescriptionInfo)
    {
        InfoPanel.SetActive(true);
        Name.text = $"{gunPickUp.ItemName}";
        Description.text = $"{gunPickUp.ItemDescription}";

        text1.text = "Damage:  " + $"{gunDescriptionInfo.BulletDamage.minDamage} - {gunDescriptionInfo.BulletDamage.maxDamage}";
        text2.text = "Bullet Speed:  " + $"{gunDescriptionInfo.ProjectileSpeed}";
        text3.text = "Weight:  " + $"{gunDescriptionInfo.GunWeight}";
        text4.text = "Class:  " + $"{gunDescriptionInfo.gunTypes}";
        text5.text = "Fire Rate:  " + $"{gunDescriptionInfo.FireRate}";
        text6.text = "Mag Size:  " + $"{gunDescriptionInfo.MagSize}";
    }

    public void EquipButton()
    {

    }
}
