﻿using System.Collections;
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

    public bool isCompleted = false;

    private bool reward;

    public void OnTriggerEnter2D(Collider2D collision) {
        if(collision.name == "playerBoatBlue") {
            questUI.transform.Find("QuestWindow").gameObject.SetActive(true);
            questWindow = GameObject.Find("QuestWindow");
            questWindow.SetActive(true);
            questGiver.SetQuest(this);
            questGiver.SetPortText(this);
            questGiver.SetPort(this);
          //  questGiver.SetInvetorySlot();

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

            questLog.transform.Find("GatherContaniner").gameObject.SetActive(true);
            questGath = GameObject.Find("GatherContaniner");
            if (quest.goal.goalType == GoalType.Gathering)
                questGath.SetActive(true);
            else
                questGath.SetActive(false);

            questGiver.CheckQuest();
        }
    }

    public void AcceptQuest() {
        questLog.SetActive(false);
        quest.isActive = true;
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
                player.GenerateReward(quest.reward);
                quest.Complete();
                questInteface.RemoveAll();
                QuestCompleted();
                questGiver.SetCompltedPortText(this);
            } else if (i > quest.goal.requiredAmount) {
                int temp = i - quest.goal.requiredAmount;

                if (quest.item.Id == 0) {
                    string s = "Reward: gold " + temp;
                    player.GenerateReward(s);
                } else if (quest.item.Id == 2) {
                    string s = "Reward: plank " + temp;
                    player.GenerateReward(s);
                }
                player.GenerateReward(quest.reward);
                quest.Complete();
                questInteface.RemoveAll();
                QuestCompleted();
                questGiver.SetCompltedPortText(this);
            }
        }
    }
}
