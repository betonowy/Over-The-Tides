using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Plank Object", menuName = "Inventory System/Items/Plank")]
public class PlankObject : ItemClass {

    public void Awake() {
        type = ItemType.Plank;
    }
}
