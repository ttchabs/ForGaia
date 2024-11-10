using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("LOADOUT SLOTS")]
    public Transform MeleeSlot;
    public Transform GunSlot;

    [Space(2)]
    public Transform ConsumableSlot;
    public int amount;

    [Header("SACK SLOTS")]
    public Transform sackStorage;
    public int maxSackStorage;
    public TextMeshProUGUI maxStorageDisplay;

    [Header("Item Prefabs")]
    public GameObject itemPrefab;
    public GameObject meleePrefab;
    public GameObject gunPrefab;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            UpdateStorageCount();
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public bool AddConsumable(PickUpScriptable item)
    {
        ItemBehaviour ConsumableInInventory = ConsumableSlot.GetComponentInChildren<ItemBehaviour>();
        if(ConsumableInInventory != null && ConsumableInInventory.itemData == item && ConsumableInInventory.amount <= amount)
        {
            ConsumableInInventory.amount++;
            ConsumableInInventory.AmountText();
            return true;
        }
        else if(ConsumableInInventory == null)
        {
            SpawnInventoryItem(item, ConsumableSlot);
            return true;
        }
        return false;
    }

    public bool AddItemToInventory(PickUpScriptable item)
    {
        if (sackStorage.childCount < maxSackStorage)
        {
            SpawnInventoryItem(item, sackStorage.transform);
            UpdateStorageCount();
            return true;
        }
        return false;
    }

    public bool AddMeleeToInventory(PickUpScriptable item, WeaponScriptable meleeItem)
    {
        if (sackStorage.childCount < maxSackStorage)
        {
            SpawnMeleeItem(item, meleeItem, sackStorage.transform);
            UpdateStorageCount();
            return true;
        }
        return false;
    }

    public bool AddRangedToInventory(PickUpScriptable item, GunScriptable gunItem)
    {
        if (sackStorage.childCount < maxSackStorage)
        {
            SpawnRangedItem(item, gunItem, sackStorage.transform);
            UpdateStorageCount();
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
        GameObject newRangedSprite = Instantiate(gunPrefab, slot);
        RangedItemBehaviour rangedItemDrag = newRangedSprite.GetComponent<RangedItemBehaviour>();
        rangedItemDrag.InitialiseRanged(itemID, gunID);
    }

    public void RemoveItemFromInventory(PickUpScriptable itemID)
    {

    }

    public void UpdateStorageCount()
    {
        maxStorageDisplay.text = $"{sackStorage.transform.childCount} / {maxSackStorage}";
    }
}
