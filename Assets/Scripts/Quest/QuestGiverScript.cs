using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestGiverScript : MonoBehaviour {

    public PortScript[] ports;
    public IslandScript[] islands;

    private Quest quest;

    private PortScript port;
    private InventorySlot inventorySlot;
    private IslandScript island;
    public InventoryObject inventory;

    public TextMeshProUGUI titeText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI completedText;

    public TextMeshProUGUI descriptionIslandText;
    public TextMeshProUGUI rewardIslandText;

    public GameObject enemyShipToSpawn;

    public GameObject acceptButton;
    public GameObject completeButton;

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
                    port.quest.reward = "Reward: sailor 1";
                    // port.quest.completed = "Thank you very much! But watch out for yourself now. You have angered whole horde of priates and they WILL come for you!";
                    port.quest.completed = "Watch out for yourself now. You have angered whole horde of priates and they WILL come for you! Swim to the north, there you might get some help";
                    break;
            }
            case 1: {
                    port.quest.title = "Plank exchange";
                    port.quest.description = "We have an amazing offer for you waiting in our port. For only 15 planks we are willing to build a new ship for you! I have heard " +
                            "that you can find some on the island to the east of our port.";
                    port.quest.reward = "Reward: ship 1";
                    port.quest.completed = "We have an amazing offer for you waiting in our port. For only 15 planks we are willing to build a new ship for you!";
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
                island.rewa = "Reward: plank 15";
                break;
            }
            case 1: {
                island.desc = "Wood just wood";
                island.rewa = "Reward: plank 30";
                break;
            }
            case 2: {
                island.desc = "You find a drunk pirate on the store. He claims that his name is Jack and he is a captain. Anyway you decide to took him on board";
                island.rewa = "Reward: sailor 1";
                break;
            }
        }
    }

    public void SetPortText(PortScript port) {
        titeText.text = port.quest.title;
        descriptionText.text = port.quest.description;
        rewardText.text = port.quest.reward;
        completedText.text = port.quest.completed;
        if(port.isCompleted == true) {
            titeText.gameObject.SetActive(false);
            descriptionText.gameObject.SetActive(false);
            rewardText.gameObject.SetActive(false);
            completedText.gameObject.SetActive(true);
        } else {
            titeText.gameObject.SetActive(true);
            descriptionText.gameObject.SetActive(true);
            rewardText.gameObject.SetActive(true);
            completedText.gameObject.SetActive(false);
        }

    }

    public void SetCompltedPortText(PortScript port) {
        titeText.gameObject.SetActive(false);
        descriptionText.gameObject.SetActive(false);
        rewardText.gameObject.SetActive(false);
        completedText.gameObject.SetActive(true);
    }

    public void SetIslandText(IslandScript island) {
        descriptionIslandText.text = island.desc;
        rewardIslandText.text = island.rewa;
    }

    public void SetPort(PortScript p) {
        port = p;
    }

    public void SetIsland(IslandScript i) {
        island = i;
    }

    public void SetInvetorySlot() {
        inventorySlot = inventory.FindItemOnInventory(quest.item);
    }

    public void GiveItems() {
        InventorySlot inventorySlot = inventory.FindItemOnInventory(quest.item);
        int i = inventorySlot.amount;
        if (inventorySlot != null) {
            if (i == port.quest.goal.requiredAmount) {
                port.player.GenerateReward(port.quest.reward);
                port.quest.Complete();
                port.questInteface.RemoveAll();
                port.QuestCompleted();
                port.questGiver.SetCompltedPortText(port);
            }
            else if (i > port.quest.goal.requiredAmount) {
                int temp = i - port.quest.goal.requiredAmount;

                if (port.quest.item.Id == 0) {
                    string s = "Reward: gold " + temp;
                    port.player.GenerateReward(s);
                }
                else if (port.quest.item.Id == 2) {
                    string s = "Reward: plank " + temp;
                    port.player.GenerateReward(s);
                }
                port.player.GenerateReward(port.quest.reward);
                port.quest.Complete();
                port.questInteface.RemoveAll();
                port.QuestCompleted();
                port.questGiver.SetCompltedPortText(port);
            }
        }
    }

    public void CheckQuest() {
        if (port.isCompleted && quest.goal.goalType != GoalType.Gathering) {
            port.player.GenerateReward(quest.reward);
            SetCompltedPortText(port);
            acceptButton.SetActive(false);
            completeButton.SetActive(true);
        }
    }

    public void SetQuest(PortScript port) {
        quest = port.quest;
    }

    public void AcceptQuest() {
        quest.isActive = true;
        FindObjectOfType<CombatManager>().SendMessage("SetQuest", quest);
        SpecialQuestAction();
        acceptButton.SetActive(false);
        completeButton.SetActive(true);
    }

    public void AcceptIsland() {
        island.isCompleted = true;
        GameObject.Find("IslandLog").gameObject.SetActive(false);
    }

    public void CompleteButtonAction() {
        GameObject.Find("QuestLog").gameObject.SetActive(false);
    }

    public void SpecialQuestAction() {
        if (quest.title == "Priate ship") {
            Vector3 v = new Vector3(0, 100, 0);
            GameObject ship = Instantiate(enemyShipToSpawn, v, gameObject.transform.rotation);
        }
    }

    public void Quit() {
        GameObject.Find("QuestLog").gameObject.SetActive(false);
    }
}
