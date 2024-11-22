using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class ItemBehaviour : MonoBehaviour, IPointerClickHandler
{
    [Header("UI ASPECTS:")]
    public Image image;
    public TextMeshProUGUI amountText;

    [Header("BTS ASPECTS")]
    public int amount = 1;
    public ItemScriptable itemData;

    public virtual void Initialise(ItemScriptable newItemData)
    {
        itemData = newItemData;
        image.sprite = newItemData.ItemSprite;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        var display = UIManager.Instance.inventoryControls;
        switch (itemData.ItemType)
        {
            case ItemTypes.Ranged:
                display.equippableGun = this;
                display.displayInfo.GunDisplayFunction(itemData as GunScriptable);
                display.EquipButton.gameObject.SetActive(true);
                display.AddGunListener();
                break;
            case ItemTypes.Melee: 
                display.equippableMelee = this;
                display.displayInfo.MeleeDisplayFunction(itemData as WeaponScriptable);
                display.EquipButton.gameObject.SetActive(true);
                display.AddMeleeListener();
                break;
            case ItemTypes.PickUp:
                display.displayInfo.ItemDisplayFunction(itemData);
                break;
            case ItemTypes.Special: 
                display.displayInfo.ItemDisplayFunction(itemData);
                break;
        }
    }

    public void AmountText()
    {
        if (amount <= 1) 
        {
            amountText.text = null;
        }
        else
        {
            amountText.text = $"{amount}";
        }

    }
}
