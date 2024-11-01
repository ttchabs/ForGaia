using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpFunction : MonoBehaviour
{
    public PickUpScriptable itemData;

    public void Pickup()
    {
        InventoryManager.Instance.AddItemToInventory(itemData);
/*        GameObject sackCounter = GameObject.Find("Sack");
        transform.SetParent(sackCounter.transform, true);*/
        //gameObject.SetActive(false);
    }
}
