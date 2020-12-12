using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestGiverScript : MonoBehaviour {

    public PortScript[] ports;
    public IslandScript[] islands;

    public TextMeshProUGUI titeText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI completedText;

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
                    port.quest.title = "Priate ship";
                    port.quest.description = "The rumor has it that not far to the north, there is a small pirate ship plundering nerby ports and islands. Please do something with it!";
                    port.quest.reward = "Reward: cannon 1";
                    port.quest.completed = "Thank you very much! But watch out for yourself now. You have angered whole horde of priates and they WILL come for you!";
                    break;
            }
            case 1: {
                    port.quest.title = "Plank exchange";
                    port.quest.description = "We have an amazing offer for you waiting in our port. For only 15 planks we are willing to build a new ship for you! I have heard " +
                            "that you can find some on the island to the east of our port.";
                    port.quest.reward = "Reward: ship 1";
                    port.quest.completed = "It is a plasure doing business with you!";
                    break;
            }
            case 2: {
                port.quest.title = "Destroy the horde";
                port.quest.description = "Oooh no! It is coming, the biggest pirate ship on entire sea and it is coming for you for what you have done to the rest of them!";
                port.quest.description = "Try do destroy as many ships as you can! So they will not trouble good people of the seas. Five should be enough :D";
                port.quest.reward = "Reward: Victory and peace!";
                port.quest.completed = "Impossible! You have done it, you have achived something no one even imagined to achive. Congratulations!";
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
                island.desc = "As your ship docks to the as it appears to be deserted island, you can see couple of tress which you decide to cut down.";
                island.rewa = "You man did a great job and you collected 15 planks.";
                break;
            }
            case 1: {
                island.desc = "Asn.";
                island.rewa = "Reward: 1 plank";
                break;
            }
        }
    }

    public void SetPortText(PortScript port) {
        titeText.text = port.quest.title;
        descriptionText.text = port.quest.description;
        rewardText.text = port.quest.reward;
        completedText.text = port.quest.completed;
    }

    public void SetCompltedPortText(PortScript port) {
        titeText.gameObject.SetActive(false);
        descriptionText.gameObject.SetActive(false);
        rewardText.gameObject.SetActive(false);
        completedText.gameObject.SetActive(true);
    }
}
