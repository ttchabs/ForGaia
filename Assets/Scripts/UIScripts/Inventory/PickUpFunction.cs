using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpFunction : MonoBehaviour
{
    public ItemScriptable itemData;

    public virtual void Pickup()
    {
        if (UIManager.Instance.inventoryControls.AddItemToInventory(itemData) == true)
        {
            Destroy(gameObject);
        }
    }
}
