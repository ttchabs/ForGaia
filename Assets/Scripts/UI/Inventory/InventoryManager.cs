using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("LOADOUT SLOTS")]
    public SlotFunctionality MeleeSlot;
    public SlotFunctionality GunSlot;
    [Space(2)]
    public SlotFunctionality[] ConsumableSlot;

    [Header("SACK SLOTS")]
    public SlotFunctionality[] sackStorage;

    public GameObject itemPrefab;

    public void Awake()
    {
        Instance = this;
    }

    public bool AddItemToInventory(PickUpScriptable item)
    {
    

        for (int i = 0; i < sackStorage.Length; i++)
        {
            SlotFunctionality slot = sackStorage[i];
            ItemDrag itemInInventory = slot.GetComponentInChildren<ItemDrag>();
            if (itemInInventory != null && itemInInventory.itemData == item && itemInInventory.amount < 5)
            {
                itemInInventory.amount++;
                itemInInventory.AmountText();
                return true;
            }
        }

        for (int i = 0; i < sackStorage.Length; i++)
        {
            SlotFunctionality slot = sackStorage[i];
            ItemDrag itemInInventory = slot.GetComponentInChildren<ItemDrag>();
            if (itemInInventory == null)
            {
                SpawnInventoryItem(item, slot.transform);
                return true;
            }
        }


        /*        if ( itemInInventory == null && sackStorage.transform.childCount <= sackStorage.maxCount)
                {
                    SpawnInventoryItem(item, sackStorage.transform);
                    return true;
                }

                else if (itemInInventory != null && itemInInventory.itemData == item && itemInInventory.amount < 5)
                {
                    itemInInventory.amount++;
                    itemInInventory.AmountText();
                    return true;
                }*/

        return false;
    }

    public void SpawnInventoryItem(PickUpScriptable itemID, Transform slot)
    {
        //itemDrags.Add(itemID);
        GameObject newSprite = Instantiate(itemPrefab, slot);
        ItemDrag itemDrag = newSprite.GetComponent<ItemDrag>();
        itemDrag.Initialise(itemID);
    }

    public void RemoveItemFromInventory(PickUpScriptable itemID)
    {

    }
}
