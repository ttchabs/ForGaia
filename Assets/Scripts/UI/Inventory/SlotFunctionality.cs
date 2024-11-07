using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotFunctionality : MonoBehaviour, IDropHandler
{
    public int maxCount;
    public void OnDrop(PointerEventData eventData) //called the moment the player stops clicking on an item
    {
        NoItemLimit(eventData);
    }

    public void NoItemLimit(PointerEventData eventData)
    {
        if (transform.childCount ==0)
        {
                GameObject dropped = eventData.pointerDrag;
                ItemDrag item = dropped.GetComponent<ItemDrag>();
                item.parentDrag = transform;            
        }
    }
}
