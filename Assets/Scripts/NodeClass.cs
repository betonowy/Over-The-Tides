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
    }

    public bool SailorReady() {
        try {
            if (assignedSailor != null) {
                Vector2 flatPos = assignedSailor.transform.position;
                if ((flatPos - position).magnitude < epsilonPosition) {
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
            AttractAssignedSailor();
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
                setPosition += position;
                assignedSailor.SendMessage("setTargetWorldSpace", setPosition);
                return true;
            }
        } catch { }
        return false;
    }

}
