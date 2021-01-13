﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class FlagInterface : UserInterface {

    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int Y_SPACE_BETWEEN_ITEMS;
    public int NUMBER_OF_COLUMN;

    public GameObject inventoryPrefab;
    //public GameObject[] slots;

    //public override void CreateSlots() {
    //    slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
    //    for (int i = 0; i < inventory.Container.Items.Length; i++) {
    //        var obj = slots[i]; 
    //        AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
    //        AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
    //        AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
    //        AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
    //        AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

    //        slotsOnInterface.Add(obj, inventory.Container.Items[i]);

    //    }
    //}

    public override void CreateSlots() {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.Container.Items.Length - 23; i++) {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });


            slotsOnInterface.Add(obj, inventory.Container.Items[i]);
        }
    }
    private Vector3 GetPosition(int i) {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMN)), 0f);
    }
}