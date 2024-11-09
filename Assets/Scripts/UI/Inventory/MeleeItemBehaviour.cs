using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MeleeItemBehaviour : MonoBehaviour, IPointerClickHandler
{
    [Header("UI ASPECTS:")]
    public Image image;

    [Header("BTS ASPECTS")]
    public PickUpScriptable itemData;
    public WeaponScriptable meleeData;

    public void InitialiseMelee(PickUpScriptable newItemData, WeaponScriptable newMeleeData)
    {
        itemData = newItemData;
        image.sprite = newItemData.ItemSprite;

        meleeData = newMeleeData;  
    }

    /*public void OnBeginDrag(PointerEventData eventData)
    {
        parentDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentDrag);
        image.raycastTarget = true;
    }*/

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemInfoDisplay.Instance.MeleeDisplayFunction(itemData, meleeData);
    }
}
