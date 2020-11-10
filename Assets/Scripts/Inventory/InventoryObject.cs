using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public Inventory Container;
    public ItemDatabaseObject database;

    public bool AddItem(Item item, int amount) {
        if (EmptySlotCount <= 0)
            return false;
        InventorySlot slot = FindItemOnInventory(item);
        if(!database.GetItem[item.Id].stackable || slot == null) {
            SetEmptySlot(item, amount);
            return true;
        }
        slot.AddAmount(amount);
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

    public InventorySlot FindItemOnInventory(Item item) {
        for (int i = 0; i < Container.Items.Length; i++) {
            if(Container.Items[i].item.Id == item.Id) {
                return Container.Items[i];
            }
        }
        return null; 
    }

    public InventorySlot SetEmptySlot(Item item, int amount) {
        for (int i = 0; i < Container.Items.Length; i++) {
            if(Container.Items[i].item.Id <= -1) {
                Container.Items[i].UpdateSlot(item, amount);
                return Container.Items[i];
            }
        }
        return null; 
    }


    public void SwapItems(InventorySlot item1, InventorySlot item2) {
        if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject)) {
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }
    }

    //public void RemoveItem(Item item)

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
            Container = (Inventory)formatter.Deserialize(stream);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for(int i=0; i<Container.Items.Length; i++) {
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
            Items[i].UpdateSlot(new Item(), 0);
        }
    }
}
