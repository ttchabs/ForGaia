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

    [Header("Item Prefabs")]
    public GameObject itemPrefab;
    public GameObject meleePrefab;
    public GameObject gunPrefab;

    public void Awake()
    {
        Instance = this;
    }



    public bool AddConsumable(PickUpScriptable item)
    {
        for (int i = 0; i < ConsumableSlot.Length; i++)
        {
            SlotFunctionality slot = ConsumableSlot[i];
            ItemBehaviour itemInInventory = slot.GetComponentInChildren<ItemBehaviour>();
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
            ItemBehaviour itemInInventory = slot.GetComponentInChildren<ItemBehaviour>();
            if (itemInInventory == null)
            {
                SpawnInventoryItem(item, slot.transform);
                return true;
            }
        }
        return false;
    }

    public bool AddItemToInventory(PickUpScriptable item)
    {
        if (sackStorage.transform.childCount <= sackStorage.maxCount)
        {
            SpawnInventoryItem(item, sackStorage.transform);
            return true;
        }
        return false;
    }

    public bool AddMeleeToInventory(PickUpScriptable item, WeaponScriptable meleeItem)
    {
        if (sackStorage.transform.childCount <= sackStorage.maxCount)
        {
            SpawnMeleeItem(item, meleeItem, sackStorage.transform);
            return true;
        }
        return false;
    }

    public bool AddRangedToInventory(PickUpScriptable item, GunScriptable gunItem)
    {
        if (sackStorage.transform.childCount <= sackStorage.maxCount)
        {
            SpawnRangedItem(item, gunItem, sackStorage.transform);
            return true;
        }
        return false;
    }

    //////

    public void SpawnInventoryItem(PickUpScriptable itemID, Transform slot)
    {
        GameObject newSprite = Instantiate(itemPrefab, slot);
        ItemBehaviour itemDrag = newSprite.GetComponent<ItemBehaviour>();
        itemDrag.Initialise(itemID);
    }

    public void SpawnMeleeItem(PickUpScriptable itemID, WeaponScriptable meleeID, Transform slot)
    {
        GameObject newMeleeSprite = Instantiate(meleePrefab, slot);
        MeleeItemBehaviour meleeItemDrag = newMeleeSprite.GetComponent<MeleeItemBehaviour>();
        meleeItemDrag.InitialiseMelee(itemID, meleeID);
    }

    public void SpawnRangedItem(PickUpScriptable itemID, GunScriptable gunID, Transform slot)
    {
        GameObject newRangedSprite = Instantiate(meleePrefab, slot);
        RangedItemBehaviour rangedItemDrag = newRangedSprite.GetComponent<RangedItemBehaviour>();
        rangedItemDrag.InitialiseRanged(itemID, gunID);
    }

    public void RemoveItemFromInventory(PickUpScriptable itemID)
    {

    }
}
