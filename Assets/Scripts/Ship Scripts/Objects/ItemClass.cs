using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    Gold,
    Cannon
}

public enum Attributes {
    Worth
}
public abstract class ItemClass : ScriptableObject {
    public Sprite uiDisplay;
    public bool stackable;
    public ItemType type;
    public Item data = new Item();

    public Item CreateItem() {
        Item newItem = new Item(this);
        return newItem;
    }
}

[System.Serializable]
public class Item {
    public string Name;
    public int Id = -1;
    public ItemBuff[] buffs;
    public Item() {
        Name = "";
        Id = -1;
    }
    public Item(ItemClass item) {
        Name = item.name;
        Id = item.data.Id;
        buffs = new ItemBuff[item.data.buffs.Length];
        for (int i = 0; i < buffs.Length; i++) {
            buffs[i] = new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max) {
                attribute = item.data.buffs[i].attribute
            };
        }
    }
}

[System.Serializable]
public class ItemBuff {
    public Attributes attribute;
    public int value;
    public int min;
    public int max;
    public ItemBuff(int _min, int _max) {
        min = _min;
        max = _max;
        GenerateValue();
    }
    public void GenerateValue() {
        value = UnityEngine.Random.Range(min, max);
    }
}