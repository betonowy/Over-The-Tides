using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
 
public class InventoryObject : ScriptableObject {
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory Container;


    public bool AddItem(Item _item, int _amount) {
        if (EmptySlotCount <= 0)
            return false;
        InventorySlot slot = FindItemOnInventory(_item);
        if (!database.GetItem[_item.Id].stackable || slot == null) {
            SetEmptySlot(_item, _amount);
            return true;
        }
        slot.AddAmount(_amount);
        return true;
    }

    public int EmptySlotCount {
        get {
            int counter = 0;
            for (int i = 0; i < Container.Items.Length; i++) {
                if (Container.Items[i].item.Id <= -1)
                    counter++;
            }
            return counter;
        }
    }

    public InventorySlot FindItemOnInventory(Item _item) {
        for (int i = 0; i < Container.Items.Length; i++) {
            if (Container.Items[i].item.Id == _item.Id)
                return Container.Items[i];
        }
        return null;
    }

    public InventorySlot SetEmptySlot(Item _item, int _amount) {
        for (int i = 0; i < Container.Items.Length; i++) {
            if (Container.Items[i].item.Id <= -1) {
                Container.Items[i].UpdateSlot(_item, _amount);
                return Container.Items[i];
            }
        }
        //set up functionality for full inventory
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2) {
        if(item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject)) {
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }

    }


    public void RemoveItem(Item _item) {
        for (int i = 0; i < Container.Items.Length; i++) {
            if (Container.Items[i].item == _item) {
                Container.Items[i].UpdateSlot(null, 0);
            }
        }
    }

    [ContextMenu("Save")]
    public void Save() {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load() {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath))) {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < Container.Items.Length; i++) {
                Container.Items[i].UpdateSlot(newContainer.Items[i].item, newContainer.Items[i].amount);
            }
            stream.Close();
        }
    }
    [ContextMenu("Clear")]
    public void Clear() {
        Container.Clear();
    }
}

[System.Serializable]
public class Inventory {
    public InventorySlot[] Items = new InventorySlot[24];
    public void Clear() {
        for (int i = 0; i < Items.Length; i++) {
            Items[i].RemoveItem();
        }
    }
}