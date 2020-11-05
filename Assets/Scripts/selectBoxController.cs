using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectBoxController : MonoBehaviour {
    private Vector3 startPos;
    private List<SailorScript> selectedSailors;

    private void Awake() {
        selectedSailors = new List<SailorScript>();
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            var mouse = Input.mousePosition;
            mouse.z = 10;
            Vector3 tempStart = Camera.allCameras[0].ScreenToWorldPoint(mouse);
            startPos = tempStart;
            foreach (SailorScript sailor in selectedSailors)
            {
                sailor.ChangeBoolSprite(false);
            }
        }

        if(Input.GetMouseButton(0)) {
            var mouse = Input.mousePosition;
            mouse.z = 10;
            Vector3 currentMousePosition = Camera.allCameras[0].ScreenToWorldPoint(mouse);

            Vector3 lowerLeft = new Vector3(Mathf.Min(startPos.x, currentMousePosition.x),
                Mathf.Min(startPos.y, currentMousePosition.y));
            Vector3 upperRigt = new Vector3(Mathf.Max(startPos.x, currentMousePosition.x),
                Mathf.Max(startPos.y, currentMousePosition.y));
        }

        if(Input.GetMouseButtonUp(0)) {

            var mouse = Input.mousePosition;
            mouse.z = 10;
            Vector3 tempEnd = Camera.allCameras[0].ScreenToWorldPoint(mouse);
            Collider2D [] collider2DArray = Physics2D.OverlapAreaAll(startPos, tempEnd);

            selectedSailors.Clear();
            foreach(Collider2D collider2D in collider2DArray) {
                SailorScript sailor = collider2D.GetComponent<SailorScript>();
                if(sailor != null) {
                    selectedSailors.Add(sailor);
                    sailor.ChangeBoolSprite(true);
                }
            }

        }

        if(Input.GetMouseButtonDown(1)) {
            var mouse = Input.mousePosition;
            mouse.z = 10;
            Vector2 sailorTarget = Camera.allCameras[0].ScreenToWorldPoint(mouse);
            /*
            foreach(SailorScript sailor in selectedSailors) {
                sailor.SendMessage("setTargetWorldSpace", sailorTarget);
            }
            */

        }
    }

    void GetChildrenObjects() {
        GameObject shipParent = GameObject.Find("playerBoat");
        int count = shipParent.transform.childCount;
        for (int i = 0; i < count; i++) {
        }
    }

}
