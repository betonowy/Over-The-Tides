using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PortScript : MonoBehaviour
{
    public GameObject questUI;
    public GameObject questWindow;
    public GameObject questLog;
    public GameObject questGath;

    public Quest quest;
    public PlayerScript player;
    public CombatManager combatManager;
    public QuestGiverScript questGiver;

    //public TextMeshProUGUI titeText;
    //public TextMeshProUGUI descriptionText;
    //public TextMeshProUGUI rewardText;
    //public TextMeshProUGUI completedText;

    public QuestInteface questInteface;
    public InventoryObject inventory;

    private bool isCompleted = false;

    private bool reward;

    public void OnTriggerEnter2D(Collider2D collision) {
        if(collision.name == "playerBoatBlue") {
            questUI.transform.Find("QuestWindow").gameObject.SetActive(true);
            questWindow = GameObject.Find("QuestWindow");
            questWindow.SetActive(true);
            questGiver.SetPortText(this);
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if (collision.name == "playerBoatBlue") {
            questWindow.SetActive(false);
            questLog.SetActive(false);
        }
    }

    private void Start() {
        questUI = GameObject.Find("QuestUI");
        player = GameObject.Find("playerBoatBlue").GetComponent<PlayerScript>();
        combatManager = GameObject.Find("LevelManager").GetComponent<CombatManager>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.J) && questWindow.activeSelf == true) {
            questWindow.transform.Find("QuestLog").gameObject.SetActive(true);
            questLog = GameObject.Find("QuestLog");
            questLog.SetActive(true);
            //titeText = questLog.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            //descriptionText = questLog.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            //rewardText = questLog.transform.Find("Reward").GetComponent<TextMeshProUGUI>();
            //completedText = questLog.transform.Find("Completed").GetComponent<TextMeshProUGUI>();

            //titeText.text = quest.title;
            //descriptionText.text = quest.description;
            //rewardText.text = quest.reward;
            //completedText.text = quest.completed;
            //titeText.text = title;
            //descriptionText.text = desc;
            //rewardText.text = reward;

            questLog.transform.Find("GatherContaniner").gameObject.SetActive(true);
            questGath = GameObject.Find("GatherContaniner");
            if (quest.goal.goalType == GoalType.Gathering)
                questGath.SetActive(true);
            else
                questGath.SetActive(false);

            if (isCompleted && quest.goal.goalType != GoalType.Gathering) {
                player.GenerateReward(quest.reward);
                questGiver.SetCompltedPortText(this);
            }
        }
    }

    public void AcceptQuest() {
        questLog.SetActive(false);
        quest.isActive = true;
        FindObjectOfType<CombatManager>().SendMessage("SetQuest", quest);
    }

    public Quest GetQuest() {
        return quest;
    }

    public void closeQuestLog() {
        questLog.SetActive(false);
    }

    public void QuestCompleted() {
        isCompleted = true;
    }

    public void GiveItems() {
        InventorySlot inventorySlot = inventory.FindItemOnInventory(quest.item);
        int i = inventorySlot.amount;
        if (inventorySlot != null) {
            if (i == quest.goal.requiredAmount) {
                Debug.Log(quest.reward);
                player.GenerateReward(quest.reward);
                quest.Complete();
                questInteface.RemoveAll();
                QuestCompleted();
                questGiver.SetCompltedPortText(this);
            }
        }
    }
}
