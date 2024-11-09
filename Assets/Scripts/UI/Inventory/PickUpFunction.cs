using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpFunction : MonoBehaviour
{
    public PickUpScriptable itemData;
    public WeaponScriptable weaponData;
    public GunScriptable gunScriptable;

    public void Pickup()
    {
        InventoryManager.Instance.AddItemToInventory(itemData);
/*        GameObject sackCounter = GameObject.Find("Sack");
        transform.SetParent(sackCounter.transform, true);*/
        gameObject.SetActive(false);
    }

    public void MeleePickUp()
    {
        InventoryManager.Instance.AddMeleeToInventory(itemData, weaponData);
    }

    public void GunPickUp() 
    {
        InventoryManager.Instance.AddRangedToInventory(itemData, gunScriptable);
    }
}
