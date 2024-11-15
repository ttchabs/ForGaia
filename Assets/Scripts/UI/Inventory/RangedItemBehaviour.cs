using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RangedItemBehaviour : MonoBehaviour, IPointerClickHandler
{
    [Header("UI ASPECTS:")]
    public Image image;

    [Header("BTS ASPECTS")]
    public PickUpScriptable itemData;
    public GunScriptable gunData;

    public void InitialiseRanged(PickUpScriptable newItemData, GunScriptable newGunData)
    {
        itemData = newItemData;
        image.sprite = newItemData.ItemSprite;

        gunData = newGunData;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemInfoDisplay.Instance.GunDisplayFunction(itemData, gunData);
        var gunEquip = InventoryManager.Instance;
        gunEquip.equippableGun = this;
        gunEquip.AddGunListener();

    }
}
