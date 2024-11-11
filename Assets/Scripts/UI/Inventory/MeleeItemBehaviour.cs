using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MeleeItemBehaviour : MonoBehaviour, IPointerClickHandler
{
    [Header("UI ASPECTS:")]
    public Image image;

    [Header("BTS ASPECTS")]
    public PickUpScriptable itemData;
    public WeaponScriptable meleeData;

    public void InitialiseMelee(PickUpScriptable newItemData, WeaponScriptable newMeleeData)
    {
        itemData = newItemData;
        meleeData = newMeleeData;
        image.sprite = newItemData.ItemSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemInfoDisplay.Instance.MeleeDisplayFunction(itemData, meleeData);
        var meleeEquip = InventoryManager.Instance;
        meleeEquip.AddMeleeListener();
        meleeEquip.equippableMelee = this;
    }
}
