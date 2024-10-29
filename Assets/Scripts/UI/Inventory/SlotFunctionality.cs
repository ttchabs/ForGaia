using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotFunctionality : MonoBehaviour, IDropHandler
{
    [SerializeField] bool isSpecialSlot;
    public int maxCount;
    public void OnDrop(PointerEventData eventData) //called the moment the player stops clicking on an item
    {
        HasItemLimit(eventData);
        NoItemLimit(eventData);
    }

    public void HasItemLimit(PointerEventData eventData)
    {
        if (isSpecialSlot == true)
        {
            if (transform.childCount == 0)
            {
                GameObject dropped = eventData.pointerDrag;
                ItemDrag item = dropped.GetComponent<ItemDrag>();
                item.parentDrag = transform;
            }
        }
    }

    public void NoItemLimit(PointerEventData eventData)
    {
        if (isSpecialSlot == false)
        {
            if (transform.childCount > maxCount + 1)
            {
                GameObject dropped = eventData.pointerDrag;
                ItemDrag item = dropped.GetComponent<ItemDrag>();
                item.parentDrag = transform;            
            }

        }
    }
}
