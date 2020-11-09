using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class NodeClass {
    public Vector2 position;
    private GameObject parentObject;
    private GameObject assignedSailor = null;

    private const float epsilonPosition = 0.1f;
    public NodeClass(Vector2 relPos, GameObject parent) {
        position = relPos;
        parentObject = parent;
    }

    public bool SailorReady() {
        try {
            if (assignedSailor != null) {
                Vector2 setPosition = parentObject.transform.position;
                float rotation = parentObject.GetComponent<Rigidbody2D>().rotation * Mathf.Deg2Rad;
                Vector2 relative = new Vector2(position.x * Mathf.Cos(rotation) - position.y * Mathf.Sin(rotation),
                                               position.y * Mathf.Cos(rotation) + position.x * Mathf.Sin(rotation));
                setPosition += relative;
                Vector2 flatPos = assignedSailor.transform.position;
                if ((flatPos - setPosition).magnitude < epsilonPosition) {
                    return true;
                }
            }
        } catch { }
        return false;
    }

    public GameObject GetAssignedSailor() {
        return assignedSailor;
    }

    public bool SetSailor(GameObject sailor) {
        if (assignedSailor == null) {
            assignedSailor = sailor;
            assignedSailor.SendMessage("AssignNode", this);
            AttractAssignedSailor();
            return true;
        } else if (assignedSailor != null && sailor == null) {
            assignedSailor = null;
            return true;
        }
        return false;
    }

    public bool AvailableSailor() {
        return assignedSailor != null;
    }

    private bool AttractAssignedSailor() {
        try {
            if (assignedSailor != null) {
                Vector2 setPosition = parentObject.transform.position;
                float rotation = parentObject.GetComponent<Rigidbody2D>().rotation * Mathf.Deg2Rad;
                Vector2 relative = new Vector2(position.x * Mathf.Cos(rotation) - position.y * Mathf.Sin(rotation),
                                               position.y * Mathf.Cos(rotation) + position.x * Mathf.Sin(rotation));
                setPosition += relative;
                assignedSailor.SendMessage("setTargetWorldSpace", setPosition);
                return true;
            }
        } catch { }
        return false;
    }

}
