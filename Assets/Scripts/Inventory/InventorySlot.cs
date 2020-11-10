using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class InventorySlot {
    public ItemType[] AllowedItems = new ItemType[0];
    public int ID = -1;
    public Item item;
    public int amount;
    public InventorySlot() {
        ID = -1;
        item = null;
        amount = 0;
    }
    public InventorySlot(int _id, Item _item, int _amount) {
        ID = _id;
        item = _item;
        amount = _amount;
    }
    public void UpdateSlot(int _id, Item _item, int _amount) {
        ID = _id;
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int value) {
        amount += value;
    }
    public bool CanPlaceInSlot(ItemClass _item) {
        if (AllowedItems.Length <= 0)
            return true;
        for (int i = 0; i < AllowedItems.Length; i++) {
            if (_item.type == AllowedItems[i])
                return true;
        }
        return false;
    }
}