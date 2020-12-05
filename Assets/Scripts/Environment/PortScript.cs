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
        }
    }

    public void AcceptQuest() {
        questLog.SetActive(false);
        quest.isActive = true;
        player.quest = quest;
        combatManager.SendMessage("SetQuest", quest);
        //FindObjectOfType<CombatManager>().SendMessage("SetQuest", quest);
    }

    public Quest GetQuest() {
        return quest;
    }

    public void closeQuestLog() {
        questLog.SetActive(false);
    }
}
