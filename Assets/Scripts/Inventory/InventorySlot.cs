using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class InventorySlot {

    public ItemType[] AllowedItem = new ItemType[0];
    public UserInterface parent;
    public Item item;
    public int amount;

    public ItemClass ItemObject {
        get {
            if (item.Id >= 0) {
                return parent.inventory.database.GetItem[item.Id];
            }
            return null;
        }
    }
    public InventorySlot() {
        item = new Item();
        amount = 0;
    }
    public InventorySlot(Item _item, int _amount) {
        item = _item;
        amount = _amount;
    }
    public void UpdateSlot(Item _item, int _amount) {
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int value) {
        amount += value;
    }

    public bool CanPlaceInSlot(ItemClass _itemObject) {

        if (AllowedItem.Length <= 0 || _itemObject == null || _itemObject.data.Id < 0 )
            return true;
        for (int i = 0; i < AllowedItem.Length; ++i) {
            if (_itemObject.type == AllowedItem[i])
                return true;
        }
        return false;
    }

    public void RemoveItem() {
        item = new Item();
        amount = 0;
    }
}
