using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Animations;
using UnityEngine;

public class selectBoxController : MonoBehaviour {
    private Vector3 startPos;
    private List<SailorScript> selectedSailors;

    public float maxDistance = 0.5f;
    public float sailorSpacing = 0.1f;
    public bool enableFreeMove = true;
    public float minDistanceFromShipEdge = 0.1f;
    private Vector2 mouseStart;

    private void Awake() {
        selectedSailors = new List<SailorScript>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && Camera.allCameras.Length > 1 && mouseInsideCameraViewport()) {
            mouseStart = Input.mousePosition;
            foreach (SailorScript sailor in selectedSailors) {
                sailor.ChangeBoolSprite(false);
            }
            selectedSailors.Clear();
        }

        if (Input.GetMouseButtonUp(0) && Camera.allCameras.Length > 1) {
            startPos = Camera.allCameras[1].ScreenToWorldPoint(mouseStart);
            Vector3 tempEnd = Camera.allCameras[1].ScreenToWorldPoint(Input.mousePosition);

            float rotation = Camera.allCameras[1].transform.rotation.eulerAngles.z;
            Vector2 boxSize = startPos - tempEnd;

            boxSize = Quaternion.Euler(0, 0, -rotation) * boxSize;

            if (boxSize.x < 0) boxSize.x = -boxSize.x;
            if (boxSize.y < 0) boxSize.y = -boxSize.y;

            Collider2D[] collider2DArray = Physics2D.OverlapBoxAll((startPos + tempEnd) / 2, boxSize, rotation);

            if (mouseInsideCameraViewport()) {
                foreach (Collider2D collider2D in collider2DArray) {
                    SailorScript sailor = collider2D.GetComponent<SailorScript>();
                    if (sailor != null) {
                        selectedSailors.Add(sailor);
                        sailor.ChangeBoolSprite(true);
                    }
                }
            } else {
                foreach (SailorScript sailor in selectedSailors) {
                    sailor.ChangeBoolSprite(false);
                }
                selectedSailors.Clear();
            }
        }

        if (Input.GetMouseButtonDown(1) && Camera.allCameras.Length > 1 && mouseInsideCameraViewport() && selectedSailors.Count > 0) {
            Vector2 sailorTarget = Camera.allCameras[1].ScreenToWorldPoint(Input.mousePosition);

            Vector2[] points = new Vector2[4];
            int rows = Mathf.CeilToInt(Mathf.Sqrt(selectedSailors.Count));
            int cols = Mathf.CeilToInt(selectedSailors.Count / rows);
            float verticalSpan = rows * sailorSpacing / 2;
            float horizontalSpan = rows * sailorSpacing / 2;

            points[0] = sailorTarget + new Vector2(minDistanceFromShipEdge + horizontalSpan, minDistanceFromShipEdge + verticalSpan);
            points[1] = sailorTarget + new Vector2(minDistanceFromShipEdge + horizontalSpan, -minDistanceFromShipEdge - verticalSpan);
            points[2] = sailorTarget + new Vector2(-minDistanceFromShipEdge - horizontalSpan, minDistanceFromShipEdge + verticalSpan);
            points[3] = sailorTarget + new Vector2(-minDistanceFromShipEdge - horizontalSpan, -minDistanceFromShipEdge - verticalSpan);

            bool placedOnShip = true;

            foreach (Vector2 point in points) {
                bool test = false;
                Collider2D[] cos = Physics2D.OverlapPointAll(point);
                foreach (Collider2D c in cos) {
                    if (c.gameObject.CompareTag("Player")) {
                        test = true;
                    }
                }
                if (!test) {
                    placedOnShip = false;
                }
            }

            GameObject shipParent = GameObject.Find("playerBoat");
            int count = shipParent.transform.childCount;

            ArrayList children = new ArrayList();
            ArrayList usable = new ArrayList();

            for (int i = 0; i < count; i++) {
                children.Add(shipParent.transform.GetChild(i).gameObject);
            }

            foreach (GameObject child in children) {
                if (child.CompareTag("SailorUse")) {
                    usable.Add(child.GetComponent<NodeScript>());
                }
            }

            NodeScript chosenNode = null;
            float closestDistance = maxDistance;

            foreach (NodeScript ns in usable) {
                Vector2 coord = ns.GetParent().GetComponent<Rigidbody2D>().position;
                float dist = (sailorTarget - coord).magnitude;
                if (dist < closestDistance) {
                    chosenNode = ns;
                    closestDistance = dist;
                }
            }

            if (chosenNode != null) {
                int free = chosenNode.getFreeNodes();
                if (free > selectedSailors.Count) free = selectedSailors.Count;

                GameObject[] sentSailors = new GameObject[free];

                while (free-- > 0 && selectedSailors.Count > 0) {
                    sentSailors[free] = selectedSailors[0].gameObject;
                    selectedSailors[0].SendMessage("LeaveNode");
                    selectedSailors[0].ChangeBoolSprite(false);
                    selectedSailors.RemoveAt(0);
                }

                chosenNode.AssignSailors(sentSailors);
            } else if (enableFreeMove && placedOnShip) {

                Vector2 startingPointSquare = new Vector2(sailorSpacing * (-cols + 1) / 2f, sailorSpacing * (-rows + 1) / 2f);

                int rowCounter = 0, colCounter = 0;

                foreach (SailorScript sailor in selectedSailors) {
                    Vector2 offset = startingPointSquare + new Vector2(colCounter++ * sailorSpacing, rowCounter * sailorSpacing);

                    if (colCounter >= cols) {
                        colCounter = 0;
                        rowCounter++;
                    }

                    sailor.SendMessage("LeaveNode");
                    sailor.SendMessage("setTargetWorldSpace", sailorTarget + offset);
                }
            }
        }
    }

    public static bool mouseInsideCameraViewport() {
        Vector2 mousePos = Input.mousePosition;

        if (Camera.allCamerasCount > 1) {
            Vector2 viewportPoint = Camera.allCameras[1].ScreenToViewportPoint(mousePos);
            if (viewportPoint.x > 1f || viewportPoint.y > 1f || viewportPoint.x < 0f || viewportPoint.y < 0f)
                return false;
        }
        return true;
    }

}
