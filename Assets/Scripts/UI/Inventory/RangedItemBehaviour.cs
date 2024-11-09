using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RangedItemBehaviour : MonoBehaviour, IPointerClickHandler
{
    [Header("UI ASPECTS:")]
    public Image image;

    [Header("BTS ASPECTS")]
    public PickUpScriptable itemData;
    public GunScriptable gunData;

    public void InitialiseRanged(PickUpScriptable newItemData, GunScriptable newGunData)
    {
        itemData = newItemData;
        image.sprite = newItemData.ItemSprite;

        gunData = newGunData;
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
        ItemInfoDisplay.Instance.GunDisplayFunction(itemData, gunData);
    }
}
