using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("UI ASPECTS:")]
    public Image image;
    public TextMeshProUGUI amountText;

    [Header("BTS ASPECTS")]
    [HideInInspector]public Transform parentDrag;
    public int amount = 1;
    public PickUpScriptable itemData;

    public void Initialise(PickUpScriptable newItemData)
    {
        itemData = newItemData;
        image.sprite = newItemData.ItemSprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
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
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Hey");
    }

    public void AmountText()
    {
        amountText.text = amount.ToString();
    }
}
