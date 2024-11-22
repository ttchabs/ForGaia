using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    //public static InventoryManager Instance;
    public GameObject inventoryPanel;
    public bool isInventoryOpen = false;

    public PlayerHUDManager playerHUDManager;
    public ItemInfoDisplay displayInfo;

    #region LOADOUT SETUP:
    [Header("LOADOUT SLOTS")]
    public Transform MeleeSlot;
    public Transform GunSlot;
    [Space(2)]
    public Transform ConsumableSlot;
    public int amount = 5;

    [HideInInspector] public ItemBehaviour equippableMelee;
    [HideInInspector] public ItemBehaviour equippableGun;
    #endregion

    #region INVENTORY SETUP:
    [Header("SACK SLOTS")]
    public Transform sackStorage;
    public int maxSackStorage;
    public TextMeshProUGUI maxStorageDisplay;
    #endregion

    [Header("ITEM PREFABS")]
    public GameObject itemPrefab;
    public GameObject meleePrefab;
    public GameObject gunPrefab;

    [Header("Buttons")]
    public Button EquipButton;

    public void OpenInventoryPanel()
    {
        if (isInventoryOpen == false)
        {
            inventoryPanel.SetActive(true);
            FirstPersonControls.Instance.playerInput.Player.Disable();
            UIManager.Instance.hudControls.gameObject.SetActive(false);
            isInventoryOpen = true;
        }
    }

    public void CloseInventoryPanel()
    {
        if (isInventoryOpen == true)
        {
            inventoryPanel.SetActive(false);
            FirstPersonControls.Instance.playerInput.Player.Enable();
            UIManager.Instance.hudControls.gameObject.SetActive(true);
            EquipButton.gameObject.SetActive(false);
            isInventoryOpen = false;
        }
    }

    public void AddMeleeListener(Transform thisItem)
    {
        if (MeleeSlot.GetChild(0) != thisItem)
        {
            EquipButton.gameObject.SetActive(true);
            EquipButton.onClick.RemoveAllListeners();
            EquipButton.onClick.AddListener(EquipMelee);

        }
        else
        {
            EquipButton.gameObject.SetActive(false);
        }
    }

    public void AddGunListener(Transform thisItem)
    {
        if (GunSlot.GetChild(0) != thisItem)
        {
            EquipButton.gameObject.SetActive(true);
            EquipButton.onClick.RemoveAllListeners();
            EquipButton.onClick.AddListener(EquipGun);
        }
        else
        {
            EquipButton.gameObject.SetActive(false);
        }
    }
    public void LoadInventory()
    {
        DontDestroyOnLoad(gameObject);
    }

    //----ADD ITEMS TO INVENTORY---//

    public bool AddConsumableToInventory(ItemScriptable item)
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

    public void RemoveConsumable()
    {

    }

    public bool AddItemToInventory(ItemScriptable item)
    {
        if (sackStorage.childCount < maxSackStorage)
        {
            SpawnInventoryItem(item, sackStorage.transform);
            UpdateStorageCount();
            return true;
        }
        return false;
    }



    //----INTANTIATE ITEMS----//

    public void SpawnInventoryItem(ItemScriptable itemID, Transform slot)
    {
        GameObject newSprite = Instantiate(itemPrefab, slot);
        ItemBehaviour itemDrag = newSprite.GetComponent<ItemBehaviour>();
        itemDrag.Initialise(itemID);
    }
/*    public bool AddMeleeToInventory(WeaponScriptable meleeItem)
    {
        if (sackStorage.childCount < maxSackStorage)
        {
            SpawnMeleeItem(meleeItem, sackStorage.transform);
            UpdateStorageCount();
            return true;
        }
        return false;
    }

    public bool AddRangedToInventory(GunScriptable gunItem)
    {
        if (sackStorage.childCount < maxSackStorage)
        {
            SpawnRangedItem(gunItem, sackStorage.transform);
            UpdateStorageCount();
            return true;
        }
        return false;
    }*/
/*    public void SpawnMeleeItem(WeaponScriptable meleeID, Transform slot)
    {
        GameObject newMeleeSprite = Instantiate(meleePrefab, slot);
        MeleeItemBehaviour meleeItemDrag = newMeleeSprite.GetComponent<MeleeItemBehaviour>();
        //meleeItemDrag.InitialiseMelee(meleeID);
    }

    public void SpawnRangedItem(GunScriptable gunID, Transform slot)
    {
        GameObject newRangedSprite = Instantiate(gunPrefab, slot);
        RangedItemBehaviour rangedItemDrag = newRangedSprite.GetComponent<RangedItemBehaviour>();
        //rangedItemDrag.InitialiseRanged(gunID);
    }*/

    //----
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

    public IEnumerator InstantiateMelee(ItemBehaviour behaviour) 
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
        StartCoroutine(InstantiateGun(equippableGun));
        return;
    }

    public IEnumerator InstantiateGun(ItemBehaviour behaviour)
    {
        var hand = FirstPersonControls.Instance;
        GameObject gun = Instantiate(behaviour.itemData.ItemModel, hand.gunHoldPosition);
        yield return new WaitForEndOfFrame();
        StartCoroutine(hand.GunInitialise(gun));
    }

    public void UpdateStorageCount()
    {
        maxStorageDisplay.text = $"{sackStorage.transform.childCount} / {maxSackStorage}";
    }
}
