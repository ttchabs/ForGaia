using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item/Item Identification container")]
public class ItemScriptable : ScriptableObject
{
    [SerializeField] string _itemName;
    [SerializeField] [TextArea (3, 10)] string _itemDescription;
    [SerializeField] GameObject _itemModel;
    [SerializeField] Sprite _itemSprite;
    [SerializeField] ItemTypes _itemType;

    public string ItemName { get => _itemName; }
    public string ItemDescription { get => _itemDescription;}
    public GameObject ItemModel { get => _itemModel;}
    public Sprite ItemSprite { get => _itemSprite;}
    public ItemTypes ItemType { get => _itemType;}
}
