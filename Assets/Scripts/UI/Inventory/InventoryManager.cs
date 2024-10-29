using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public SlotFunctionality MeleeSlot;
    public SlotFunctionality GunSlot;
    public SlotFunctionality[] consumableSlots;

    public Transform sackStorage;

    public GameObject itemPrefab;
    public List<PickUpScriptable> itemDrags;

    public void Awake()
    {
        Instance = this;
    }

    public void AddItemToInventory(PickUpScriptable item)
    {
        SpawnInventoryItem(item, sackStorage);
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
