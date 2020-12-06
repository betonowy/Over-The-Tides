using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PortScript : MonoBehaviour
{
    public GameObject questUI;
    public GameObject questWindow;
    public GameObject questLog;

    public Quest quest;
    public PlayerScript player;
    public CombatManager combatManager;

    public TextMeshProUGUI titeText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI completedText;

    public QuestInteface questInteface;
    public InventoryObject inventory;

    private bool isCompleted = false;
    private bool flag = true;

    public void OnTriggerEnter2D(Collider2D collision) {
        if(collision.name == "playerBoatBlue") {
            questUI.transform.Find("QuestWindow").gameObject.SetActive(true);
            questWindow = GameObject.Find("QuestWindow");

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
        if (Input.GetKey(KeyCode.J) && questWindow.activeSelf == true) {
            questWindow.transform.Find("QuestLog").gameObject.SetActive(true);
            questLog = GameObject.Find("QuestLog");
    
            titeText = questLog.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            descriptionText = questLog.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            rewardText = questLog.transform.Find("Reward").GetComponent<TextMeshProUGUI>();
            completedText = questLog.transform.Find("Completed").GetComponent<TextMeshProUGUI>();
            titeText.text = quest.title;
            descriptionText.text = quest.description;
            rewardText.text = quest.reward;
            completedText.text = quest.completed;
            
            if(isCompleted) {
                if (flag) {
                    player.GenerateReward(rewardText.text);
                    titeText.gameObject.SetActive(false);
                    descriptionText.gameObject.SetActive(false);
                    rewardText.gameObject.SetActive(false);
                    completedText.gameObject.SetActive(true);
                    flag = false;
                }
            }
        }
    }

    public void AcceptQuest() {
        questLog.SetActive(false);
        quest.isActive = true;
        //FindObjectOfType<CombatManager>().SendMessage("SetQuest", quest);
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
            if(i == quest.goal.requiredAmount) {
                Debug.Log("UDAŁO SIE");
                quest.Complete();
                questInteface.RemoveAll();
                QuestCompleted();
            }

        }
    }
}
