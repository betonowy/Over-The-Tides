﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInterface : UserInterface
{
    public GameObject[] slots;
    public ShipScript ship;

    public override void checkShipInvetrory() {
        bool[] oldShipEq = ship.getCannonExistenceArray();
        for (int i = 0; i < inventory.Container.Items.Length; i++) {
            if (inventory.Container.Items[i].item.Id == 1)
                inventoryStatus[i] = true;
            else
                inventoryStatus[i] = false;
        }
        for (int i = 0; i < inventory.Container.Items.Length; i++) {
            if (oldShipEq[i] != inventoryStatus[i])
                ship.UpdateCannonsFromInventory(i, inventoryStatus[i]);
        }
    }

    public override void CreateSlots() {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.Container.Items.Length; i++) {
            var obj = slots[i]; 
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            slotsOnInterface.Add(obj, inventory.Container.Items[i]);
            
        }
    }
}
