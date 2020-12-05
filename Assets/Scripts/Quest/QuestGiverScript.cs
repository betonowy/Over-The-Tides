using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestGiverScript : MonoBehaviour
{
    public PortScript[] ports;

    private void Start() {
        FillPorts();
    }

    private void FillPorts() {
        for(int i = 0; i < ports.Length; i++) {
            StringFiller(i, ports[i]);
        }
    }

    private void StringFiller(int i, PortScript port) {
        switch (i) {
            case 0: {
                port.quest.title = "Save Us";
                port.quest.description = "later";
                break;
            }
            case 1: {
                port.quest.description = "New ship";
                port.quest.title = "later";
                break;
            }
        }
    }
}
