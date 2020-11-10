using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public abstract class UserInterface : MonoBehaviour {

    public InventoryObject inventory;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

    protected bool[] inventoryStatus;

    void Start() {
        for (int i = 0; i < inventory.Container.Items.Length; i++) {
            inventory.Container.Items[i].parent = this;
        }
        inventoryStatus = new bool[inventory.Container.Items.Length];
        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
        
    }

    // Update is called once per frame
    void Update() {

        UpdateSlots();
    }
    public abstract void CreateSlots();
  
    public void UpdateSlots() {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in slotsOnInterface) {
            if (_slot.Value.item.Id >= 0) {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.Value.ItemObject.uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }
    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action) {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj) {
        MouseData.slotHoverdOver = obj;
        
    }
    public void OnExit(GameObject obj) {
        MouseData.slotHoverdOver = null;
    }
    public void OnDragStart(GameObject obj) {

        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }

    public GameObject CreateTempItem(GameObject obj) {
        GameObject tempItem = null;
        if(slotsOnInterface[obj].item.Id >= 0) {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.uiDisplay;
            img.raycastTarget = false;
        }
        return tempItem;
    }

    public abstract void checkShipInvetrory();
    
    public void OnDragEnd(GameObject obj) {
        Destroy(MouseData.tempItemBeingDragged);

        if (MouseData.InterfaceMouseIsOver == null) {
            //slotsOnInterface[obj].RemoveItem();
            return;
        }
        if(MouseData.slotHoverdOver) {
            InventorySlot moveHoverSlotData = MouseData.InterfaceMouseIsOver.slotsOnInterface[MouseData.slotHoverdOver];
            inventory.SwapItems(slotsOnInterface[obj], moveHoverSlotData);
            checkShipInvetrory();
        }

    }

    public void OnDrag(GameObject obj) {
        if (MouseData.tempItemBeingDragged != null)
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void OnExitInterface(GameObject obj) {
        MouseData.InterfaceMouseIsOver = null;
    }

    public void OnEnterInterface(GameObject obj) {
        MouseData.InterfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }



}

public static class MouseData {

    [System.NonSerialized]
    public static UserInterface InterfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoverdOver;
}