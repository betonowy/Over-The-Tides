using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gold Object", menuName = "Inventory System/Items/Gold")]
public class GoldObject : ItemClass {
    public void Awake() {
        type = ItemType.Gold; 
    }
}
