using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpFunction : MonoBehaviour
{
    public ItemScriptable itemData;

    public virtual void Pickup()
    {
/*        GameObject sackCounter = GameObject.Find("Sack");
        transform.SetParent(sackCounter.transform, true);*/
        if (InventoryManager.Instance.AddItemToInventory(itemData) == true)
        {
            Destroy(gameObject);
        }
    }

 /*   public virtual void MeleePickUp(WeaponScriptable meleeData)
    {

        if (InventoryManager.Instance.AddMeleeToInventory(meleeData) == true)
        {
            Destroy(gameObject);
        }
    }

    public virtual void GunPickUp(GunScriptable gunData) 
    {
        if (InventoryManager.Instance.AddRangedToInventory(gunData) == true)
        {
            Destroy(gameObject);
        }
    }*/
}
