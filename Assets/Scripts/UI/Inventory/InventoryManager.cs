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
    public bool isInventoryOpen = false;

    [Header("LOADOUT SLOTS")]
    public Transform MeleeSlot;
    public Transform GunSlot;

    [HideInInspector]public MeleeItemBehaviour equippableMelee;
    [HideInInspector]public RangedItemBehaviour equippableGun;

    [Space(2)]
    public Transform ConsumableSlot;
    public int amount = 5;

    [Header("SACK SLOTS")]
    public Transform sackStorage;
    public int maxSackStorage;
    public TextMeshProUGUI maxStorageDisplay;

    [Header("Item Prefabs")]
    public GameObject itemPrefab;
    public GameObject meleePrefab;
    public GameObject gunPrefab;

    [Header("")]

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

    public void OpenInventoryPanel()
    {
        if (isInventoryOpen == false)
        {
            inventoryPanel.SetActive(true);
            FirstPersonControls.Instance.playerInput.Disable();
            isInventoryOpen = true;
        }
    }

    public void CloseInventoryPanel()
    {
        if (isInventoryOpen == true)
        {
            inventoryPanel.SetActive(false);
            FirstPersonControls.Instance.playerInput.Enable();
            isInventoryOpen = false;
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

    public bool AddConsumableToInventory(PickUpScriptable item)
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
        var returnMelee = MeleeSlot.GetChild(0);
        var hand = FirstPersonControls.Instance;
        hand.RemoveMelee();
        returnMelee.SetParent(sackStorage);
        equippableMelee.transform.SetParent(MeleeSlot);
        StartCoroutine(InstantiateMelee(equippableMelee));
        return;
    }

    public IEnumerator InstantiateMelee(MeleeItemBehaviour behaviour) 
    {
        var hand = FirstPersonControls.Instance;
        GameObject weapon = Instantiate(behaviour.itemData.ItemModel, hand.meleeHoldPosition);
        yield return new WaitForEndOfFrame();
        hand.MeleeInitialise(weapon);
    }


    public void EquipGun()
    {
        var returnGun = GunSlot.GetChild(0);
        var hand = FirstPersonControls.Instance;
        returnGun.SetParent(sackStorage);
        equippableGun.transform.SetParent(GunSlot);
        hand.RemoveGun();
        InstantiateGun(equippableGun);
        return;
    }

    public IEnumerator InstantiateGun(RangedItemBehaviour behaviour)
    {
        var hand = FirstPersonControls.Instance;
        GameObject gun = Instantiate(behaviour.itemData.ItemModel, hand.gunHoldPosition);
        yield return new WaitForEndOfFrame();
        hand.GunInitialise(gun);
    }

    public void UpdateStorageCount()
    {
        maxStorageDisplay.text = $"{sackStorage.transform.childCount} / {maxSackStorage}";
    }


}
