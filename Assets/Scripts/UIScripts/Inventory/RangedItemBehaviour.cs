using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RangedItemBehaviour : ItemBehaviour, IPointerClickHandler
{
    public GunScriptable gunData;

    public override void Initialise(ItemScriptable newItemData)
    {
        base.Initialise(newItemData);
    }
    public void InitialiseRanged(GunScriptable newGunData)
    {
        gunData = newGunData;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        //base.OnPointerClick(eventData);
        var gunEquip = InventoryManager.Instance;
        gunEquip.equippableGun = this;
        gunEquip.AddGunListener();
    }
}
