using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PortScript : MonoBehaviour
{
    public GameObject QuestWindow;
    public GameObject QuestLog;

    public Quest quest;
    public PlayerScript player;

    public TextMeshProUGUI titeText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI rewardText;

    public TextMeshProUGUI completedText;

    public void OnTriggerEnter2D(Collider2D collision) {
        if(collision.name == "playerBoatBlue") {
            QuestWindow.SetActive(true);
            titeText.text = quest.title;
            descriptionText.text = quest.description;
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if (collision.name == "playerBoatBlue") {
            QuestWindow.SetActive(false);
            QuestLog.SetActive(false);
        }
    }

    private void Start() {
        titeText.text = quest.title;
        descriptionText.text = quest.description;
    }

    private void Update() {
        if (Input.GetKey(KeyCode.J) && QuestWindow.activeSelf == true) {
            QuestLog.SetActive(true);
        }
    }

    public void AcceptQuest() {
        QuestLog.SetActive(false);
        quest.isActive = true;
        player.quest = quest;
        FindObjectOfType<CombatManager>().SendMessage("SetQuest", quest);
    }

    public Quest GetQuest() {
        return quest;
    }

    public void closeQuestLog() {
        QuestLog.SetActive(false);
    }
}
