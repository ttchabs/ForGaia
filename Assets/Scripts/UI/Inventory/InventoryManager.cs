using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("LOADOUT SLOTS")]
    public SlotFunctionality MeleeSlot;
    public SlotFunctionality GunSlot;
    public int amount;
    [Space(2)]
    public SlotFunctionality[] ConsumableSlot;

    [Header("SACK SLOTS")]
    public SlotFunctionality sackStorage;
    public Transform sack;

    [Header("Item Prefabs")]
    public GameObject itemPrefab;
    public GameObject meleePrefab;
    public GameObject gunPrefab;

    public void Awake()
    {
        Instance = this;
    }

    public bool AddItemToConsumable(PickUpScriptable item)
    {
        for (int i = 0; i < ConsumableSlot.Length; i++)
        {
            SlotFunctionality slot = ConsumableSlot[i];
            ItemDrag itemInInventory = slot.GetComponentInChildren<ItemDrag>();
            if (itemInInventory != null && itemInInventory.itemData == item && itemInInventory.amount <= amount)
            {
                itemInInventory.amount++;
                itemInInventory.AmountText();
                return true;
            }
        }

        for (int i = 0; i < ConsumableSlot.Length; i++)
        {
            SlotFunctionality slot = ConsumableSlot[i];
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

    public bool AddItemToInventory(PickUpScriptable item)
    {
        if (sack.childCount <= sackStorage.maxCount)
        {
            SpawnInventoryItem(item, sackStorage.transform);
            return true;
        }
        return false;
    }

    public void SpawnInventoryItem(PickUpScriptable itemID, Transform slot)
    {
        //itemDrags.Add(itemID);
        GameObject newSprite = Instantiate(itemPrefab, slot);
        ItemDrag itemDrag = newSprite.GetComponent<ItemDrag>();
        itemDrag.Initialise(itemID);
    }

    public void SpawnMeleeItem(PickUpScriptable itemID, Transform slot)
    {
        GameObject newMeleeSprite = Instantiate(meleePrefab, slot);

    }

    public void RemoveItemFromInventory(PickUpScriptable itemID)
    {

    }
}
