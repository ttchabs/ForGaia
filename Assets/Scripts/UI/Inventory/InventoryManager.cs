using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public GameObject inventoryPanel;
    [Header("LOADOUT SLOTS")]
    public Transform MeleeSlot;
    public Transform GunSlot;

    public MeleeItemBehaviour equippableMelee;
    public RangedItemBehaviour equippableGun;

    [Space(2)]
    public Transform ConsumableSlot;
    public int amount = 100;

    [Header("SACK SLOTS")]
    public Transform sackStorage;
    public int maxSackStorage;
    public TextMeshProUGUI maxStorageDisplay;

    [Header("Item Prefabs")]
    public GameObject itemPrefab;
    public GameObject meleePrefab;
    public GameObject gunPrefab;

    [Header("Buttons")]
    public Button EquipButton;

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

    public void AddMeleeListener()
    {
        EquipButton.onClick.RemoveAllListeners();
        EquipButton.onClick.AddListener(EquipMelee); ;
    }

    public void AddGunListener()
    {
        EquipButton.onClick.RemoveAllListeners();
        EquipButton.onClick.AddListener(EquipGun);
    }
    public void LoadInventory()
    {
        DontDestroyOnLoad(gameObject);
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

    public void EquipMelee()
    {

            var returnMelee = GetComponentInChildren<MeleeItemBehaviour>();
            var hand = FirstPersonControls.Instance;
            hand.RemoveMelee();
            returnMelee.transform.SetParent(sackStorage);
            equippableMelee.transform.SetParent(MeleeSlot);
            InstantiateMelee(equippableMelee);
            return;
        

    }

    public void InstantiateMelee(MeleeItemBehaviour behaviour) 
    {
        var hand = FirstPersonControls.Instance;
        GameObject weapon = Instantiate(behaviour.itemData.ItemModel, hand.meleeHoldPosition);
        hand.InitialiseMelee(weapon);
    }


    public void EquipGun()
    {

            var returnGun = GetComponentInChildren<RangedItemBehaviour>();
            var hand = FirstPersonControls.Instance;
            returnGun.transform.SetParent(sackStorage);
            equippableGun.transform.SetParent(GunSlot);
            hand.RemoveGun();
            InstantiateGun(equippableGun);
        return;
    }

    public void InstantiateGun(RangedItemBehaviour behaviour)
    {
        var hand = FirstPersonControls.Instance;
        GameObject gun = Instantiate(behaviour.itemData.ItemModel, hand.gunHoldPos);
        hand.InitialiseGun(gun);
    }

    public void UpdateStorageCount()
    {
        maxStorageDisplay.text = $"{sackStorage.transform.childCount} / {maxSackStorage}";
    }


}
