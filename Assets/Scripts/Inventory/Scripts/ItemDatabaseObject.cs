using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver {
    public ItemClass[] Items;
    public Dictionary<int,ItemClass> GetItem = new Dictionary<int,ItemClass>();

    public void OnAfterDeserialize() {
        for (int i = 0; i < Items.Length; i++) {
            Items[i].data.Id = i;
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize() {
        GetItem = new Dictionary<int,ItemClass>();
    }
}

