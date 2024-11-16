using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ItemBehaviour : MonoBehaviour, IPointerClickHandler
{
    [Header("UI ASPECTS:")]
    public Image image;
    public TextMeshProUGUI amountText;

    [Header("BTS ASPECTS")]
    public int amount = 1;
    public ItemScriptable itemData;

    public void Initialise(ItemScriptable newItemData)
    {
        itemData = newItemData;
        image.sprite = newItemData.ItemSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemInfoDisplay.Instance.ItemDisplayFunction(itemData);
    }

    public void AmountText()
    {
        amountText.text = amount.ToString();
    }
}
