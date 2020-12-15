using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public TextMeshProUGUI specialText;

    public GameObject enemyShipToSpawn;

    public GameObject acceptButton;
    public GameObject completeButton;

    public GameObject questUI;
    public GameObject questWindow;
    public GameObject questLog;
    public GameObject questGath;
    public GameObject specialCompleteAction;

    private string mess;
    public bool[] playerDistance;
    public bool questWindowFlag = false;

    private void Start() {
        FillPorts();
        FillIslands();
        playerDistance = new bool[ports.Length];
    }

    private void Update() {
        checkWindow();
        if(Input.GetKeyDown(KeyCode.M) && specialCompleteAction.activeSelf == true) {
            specialCompleteAction.SetActive(false);
        }
    }

    private void FillPorts() {
        for(int i = 0; i < ports.Length; i++) {
            StringFillerPorts(i, ports[i]);
        }
    }

    public void close() {
        questLog.SetActive(false);
        questWindow.SetActive(false);
    }

    private void StringFillerPorts(int i, PortScript port) {
        switch (i) {
            case 0: {
                    port.quest.title = "Priate ship";
                    port.quest.description = "The rumor has it that not far to the north, there is a small pirate ship plundering nerby ports and islands. Please do something with it!";
                    port.quest.reward = "Reward: sailor 1";
                    // port.quest.completed = "Thank you very much! But watch out for yourself now. You have angered whole horde of priates and they WILL come for you!";
                    port.quest.completed = "Watch out for yourself now. You have angered whole horde of priates and they WILL come for you! Swim to the north, there you might get some help";
                    port.id = 0;
                 //   mess += port.id + " ";
                    break;
            }
            case 1: {
                    port.quest.title = "Plank exchange";
                    port.quest.description = "We have an amazing offer for you waiting in our port. For only 15 planks we are willing to build a new ship for you! I have heard " +
                            "that you can find some on the island to the east of our port.";
                    port.quest.reward = "Reward: ship 1";
                    port.quest.completed = "We have an amazing offer for you waiting in our port. For only 15 planks we are willing to build a new ship for you!";
                    port.id = 1;
                   // mess += port.id + " ";
                    break;
            }
            case 2: {
                port.quest.title = "Destroy the horde";
                //port.quest.description = "Oooh no! It is coming, the biggest pirate ship on entire sea and it is coming for you for what you have done to the rest of them!";
                port.quest.description = "Try do destroy as many ships as you can! So they will not trouble good people of the seas. Five should be enough :D";
                port.quest.reward = "Reward: victory 1";
                port.quest.completed = "Impossible! You have done it, you have achived something no one even imagined to achive. Congratulations!";
                port.id = 2;
                //    mess += port.id + " ";
                    break;
            }
            case 3: {
                    port.quest.title = "Repair Center";
                    port.quest.description = "Repair for some coins, 10 should be enough";
                    port.quest.reward = "Reward: heal 1";
                    port.quest.completed = "Repair for some coins, 10 should be enough";
                    port.id = 3;
                    //    mess += port.id + " ";
                    break;
            }
        }
    }

    private void FillIslands() {
        for (int i = 0; i < islands.Length; i++) {
            StringFillIslands(i, islands[i]);
        }
    }

    public void OpenPort(PortScript p) {
        questLog.SetActive(true);
        GameObject.Find("playerBoatBlue").GetComponent<PlayerScript>().SendMessage("AllowMovement", false);
        if (p.quest.goal.goalType == GoalType.Gathering) {
            questGath.SetActive(true);
            acceptButton.SetActive(false);
        }
        else {
            questGath.SetActive(false);
            acceptButton.SetActive(true);
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
                island.desc = "You find a drunk pirate on the shore. He claims that his name is capitan Jack. Anyway you decide to took him on board";
                island.rewa = "Reward: sailor 1";
                break;
            }
        }
    }

    public void SetPortText(PortScript port) {
        titeText.text = port.quest.title;
        descriptionText.text = port.quest.description;
        if(port.quest.reward == "Reward: victory 1")
            rewardText.text = "Reward: Victory and peace!";
        rewardText.text = port.quest.reward;
        completedText.text = port.quest.completed;

        //if (port.quest.goal.goalType == GoalType.Gathering)
        //    port.questGath.SetActive(true);
        //else
        //    port.questGath.SetActive(false);

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
        SpecialQuestAction();
    }

    public void CheckQuest() {
        if (port.isCompleted && quest.goal.goalType != GoalType.Gathering) {
            SetCompltedPortText(port);
            acceptButton.SetActive(false);
            completeButton.SetActive(true);
        }
    }

    public void SetQuest(PortScript port) {
        quest = port.quest;
    }

    public void AcceptQuest() {
        if (port.quest.isCompleted == false) {
            quest.isActive = true;
            FindObjectOfType<CombatManager>().SendMessage("SetQuest", quest);
            SpecialQuestAction();
            acceptButton.SetActive(false);
            completeButton.SetActive(true);
            GameObject.Find("QuestLog").gameObject.SetActive(false);
            GameObject.Find("playerBoatBlue").GetComponent<PlayerScript>().SendMessage("AllowMovement", true);
        }
    }

    public void AcceptIsland() {
        island.isCompleted = true;
        GameObject.Find("IslandLog").gameObject.SetActive(false);
        GameObject.Find("playerBoatBlue").GetComponent<PlayerScript>().SendMessage("AllowMovement", true);
    }

    public void CompleteButtonAction() {
        if(port.rewardTaken == false) {
            port.player.GenerateReward(quest.reward);
            port.rewardTaken = true;
            SpecialCompleteAction();
        }
        GameObject.Find("QuestLog").gameObject.SetActive(false);
        GameObject.Find("playerBoatBlue").GetComponent<PlayerScript>().SendMessage("AllowMovement", true);
    }

    public void SpecialQuestAction() {
        if (quest.title == "Priate ship") {
            Vector3 v = new Vector3(0, 100, 0);
            GameObject ship = Instantiate(enemyShipToSpawn, v, gameObject.transform.rotation);
            transform.GetComponent<MapScript>().markerFlag = true;
            transform.GetComponent<MarkerScript>().flag = true;
        }
        if (quest.title == "Plank exchange") {
            transform.GetComponent<MarkerScript>().flag = false;
            transform.GetComponent<MapScript>().markerFlag = false;
        }
    }

    public void SpecialCompleteAction() {
        if (quest.title == "Priate ship") {
            specialCompleteAction.SetActive(true);
            specialText.text = "Press M to open map";
           // StartCoroutine(ShowMessage(specialCompleteAction));
        }
    }

    IEnumerator ShowMessage(GameObject obj) {
        yield return new WaitForSeconds(15);
        obj.SetActive(false);
    }
    public void Quit() {
        GameObject.Find("QuestLog").gameObject.SetActive(false);
        GameObject.Find("playerBoatBlue").GetComponent<PlayerScript>().SendMessage("AllowMovement", true);
    }

    public void show(bool b) {
        if (b)
            questWindow.SetActive(true);
        else
            questWindow.SetActive(false);
    }

    private void checkWindow() {
        int counter = 0;
        foreach (PortScript p in ports) {
            if (p.distance < 20f)
                playerDistance[counter] = true;
            else
                playerDistance[counter] = false;
            counter++;
        }
        for (int i = 0; i < counter; i++)
            if (playerDistance[i] == true)
                questWindowFlag = true;
        if (questWindowFlag == true)
            questWindow.SetActive(true);
        if(questWindowFlag == false)
            questWindow.SetActive(false);
        questWindowFlag = false;
    }
}
