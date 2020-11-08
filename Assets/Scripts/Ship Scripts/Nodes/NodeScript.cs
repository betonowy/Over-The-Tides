﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class NodeScript : MonoBehaviour
{
    GameObject parentGameObject;
    ArrayList nodes;

    public int ReadyCrewCount() {
        int count = 0;
        for (int i = 0; i < nodes.Count; i++) {
            NodeClass current = (NodeClass)nodes[i];
            if (current.SailorReady()) {
                count++;
            }
        }
        return count;
    }

    public void CreateRelativeNode(Vector2 relPos) {
        new NodeClass(relPos, parentGameObject);
        nodes.Add((new NodeClass(relPos, parentGameObject)));
    }

    public void SetParent(GameObject parent) {
        parentGameObject = parent;
        nodes = new ArrayList();
    }

    public GameObject GetParent() {
        return parentGameObject;
    }

    public bool AssignSailor(GameObject sailor) {
        for (int i = 0; i < nodes.Count; i++) {
            if (((NodeClass)nodes[i]).SetSailor(sailor)) {
                return true;
            }
        }
        return false;
    }

    public int AssignSailors(GameObject[] sailors) {
        int assigned = 0;
        for (int i = 0; i < sailors.Length; i++, assigned++) {
            if (!AssignSailor(sailors[i])) {
                break;
            }
        }
        return assigned;
    }

    public int getFreeNodes() {
        int free = 0;
        for (int i = 0; i < nodes.Count; i++) {
            if (!((NodeClass)nodes[i]).AvailableSailor()) {
                free++;
            }
        }
        return free;
    }
}