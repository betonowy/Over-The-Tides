using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestGiverScript : MonoBehaviour {

    public PortScript[] ports;
    public IslandScript[] islands;
    
    private void Start() {
        FillPorts();
        FillIslands();
    }

    private void FillPorts() {
        for(int i = 0; i < ports.Length; i++) {
            StringFillerPorts(i, ports[i]);
        }
    }

    private void StringFillerPorts(int i, PortScript port) {
        switch (i) {
            case 0: {
                port.quest.title = "Save Us";
                port.quest.description = "later";
                port.quest.reward = "Reward: cannon 1";
                port.quest.completed = "Thanks!";
                break;
            }
            case 1: {
               port.quest.title = "later";
               port.quest.description = "New ship";
               port.quest.reward = "Reward: ship 1";
               port.quest.completed = "Thanks!";
               break;
            }
        }
    }

    private void FillIslands() {
        for (int i = 0; i < islands.Length; i++) {
            StringFillIslands(i, islands[i]);
        }
    }

    private void StringFillIslands(int i, IslandScript island) {
        switch(i) {
            case 0: {
                island.desc = "Test1";
                island.rewa = "Reward: plank 15";
                break;
            }
        }
    }
}
