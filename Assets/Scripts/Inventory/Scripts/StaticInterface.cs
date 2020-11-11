using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInterface : UserInterface
{

    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int Y_SPACE_BETWEEN_ITEMS;
    public int NUMBER_OF_COLUMN;
    public ShipScript ship;
    public GameObject inventoryPrefab;
    public GameObject[] slots;
    private Canvas canvas;

    public override void checkShipInvetrory() {
        ship = GameObject.Find("playerBoatBlue").GetComponent<ShipScript>();
        if (ship == null) ship = GameObject.Find("playerBoatRed").GetComponent<ShipScript>();
        if (ship == null) ship = GameObject.Find("playerBoatFFA").GetComponent<ShipScript>();

        bool[] oldShipEq = ship.getCannonExistenceArray();
        for (int i = 0; i < inventory.Container.Items.Length - 16; i++) {
            if (inventory.Container.Items[i].item.Id == 1)
                inventoryStatus[i] = true;
            else
                inventoryStatus[i] = false;
        }
        fixedArray = swapArray(inventoryStatus);
    }

    public bool[] GetEquiplementArray() {
        return fixedArray;
    }

    private bool[] swapArray(bool[] arr) {
        bool[] temp = new bool[8];
        temp[0] = arr[0] ;
        temp[4] = arr[1];
        temp[1] = arr[2];
        temp[5] = arr[3];
        temp[2] = arr[4];
        temp[6] = arr[5];
        temp[3] = arr[6];
        temp[7] = arr[7];
        return temp;
    }

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
        for (int i = 0; i < inventory.Container.Items.Length - 16; i++) {
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
