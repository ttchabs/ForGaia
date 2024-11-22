using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public InventoryManager inventoryControls;
    public PlayerHUDManager hudControls;
    public PlayerPopUpEvents popUpControls;

    public CanvasGroup[] uiCanvases;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DeactivateAllUI()
    {
        foreach(var ui in uiCanvases)
        {
            ui.alpha = 0;
        }
    }

    public void ReactivateAllUI()
    {
        foreach (var ui in uiCanvases)
        {
            ui.alpha = 1;
        }
    }
}
