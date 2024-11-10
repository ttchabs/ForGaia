using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpFunction : MonoBehaviour
{
    public PickUpScriptable itemData;

    public void Pickup()
    {

/*        GameObject sackCounter = GameObject.Find("Sack");
        transform.SetParent(sackCounter.transform, true);*/
        if (InventoryManager.Instance.AddItemToInventory(itemData) == false)
        {
            gameObject.SetActive(false);
        }
    }

    public void MeleePickUp(WeaponScriptable meleeData)
    {

        if (InventoryManager.Instance.AddMeleeToInventory(itemData, meleeData) == true)
        {
            gameObject.SetActive(false);
        }
    }

    public void GunPickUp(GunScriptable gunData) 
    {
        if (InventoryManager.Instance.AddRangedToInventory(itemData, gunData) == true)
        {
            gameObject.SetActive(false);
        }
    }
}
