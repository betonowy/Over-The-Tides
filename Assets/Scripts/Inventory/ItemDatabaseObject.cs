using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver {
    public ItemClass[] items;
    public Dictionary<int, ItemClass> GetItem= new Dictionary<int, ItemClass>();

    public void OnAfterDeserialize() {
       
        for(int i=0; i < items.Length; i++) {
            items[i].data.Id = i;
            GetItem.Add(i, items[i]);
        }
    }

    public void OnBeforeSerialize() {
        GetItem = new Dictionary<int, ItemClass>();
    }
}

  
