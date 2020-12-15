using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PortScript : MonoBehaviour
{
    //public GameObject questUI;
    //public GameObject questWindow;
    //public GameObject questLog;
    //public GameObject questGath;
    //public TextMeshProUGUI titeText;
    //public TextMeshProUGUI descriptionText;
    //public TextMeshProUGUI rewardText;
    //public TextMeshProUGUI completedText;

    //public QuestInteface questInteface;
    //public InventoryObject inventory;



  //  private bool reward;
  //  public bool gather = false;

    //public void OnTriggerEnter2D(Collider2D collision) {
    //    if(collision.name == "playerBoatBlue") {
    //        questUI.transform.Find("QuestWindow").gameObject.SetActive(true);
    //        questWindow = GameObject.Find("QuestWindow");
    //        questWindow.SetActive(true);
    //        questGiver.SetQuest(this);
    //        questGiver.SetPortText(this);
    //        questGiver.SetPort(this);
    //        gather = true;
    //    }
    //}

    //public void OnTriggerExit2D(Collider2D collision) {
    //    if (collision.name == "playerBoatBlue") {
    //        questWindow.SetActive(false);
    //        questLog.SetActive(false);
    //        gather = false;
    //    }
    //}


    //public void AcceptQuest() {
    ////    questLog.SetActive(false);
    //    quest.isActive = true;
    //}


    //public Quest GetQuest() {
    //    return quest;
    //}

    //public void closeQuestLog() {
    // //   questLog.SetActive(false);
    //}

    public void QuestCompleted() {
        isCompleted = true;
    }

    public Quest quest;
    [System.NonSerialized]
    public PlayerScript player;
    [System.NonSerialized]
    public CombatManager combatManager;
    [System.NonSerialized]
    public QuestGiverScript questGiver;

    [System.NonSerialized]
    public int id;
    [System.NonSerialized]
    public bool isCompleted = false;
    [System.NonSerialized]
    public bool dist = false;
    [System.NonSerialized]
    public float distance;

    public QuestInteface questInteface;
    public InventoryObject inventory;

    private void Start() {
        player = GameObject.Find("playerBoatBlue").GetComponent<PlayerScript>();
        combatManager = GameObject.Find("LevelManager").GetComponent<CombatManager>();
        questGiver = GameObject.Find("LevelManager").GetComponent<QuestGiverScript>();
    }

    private void Update() {
        checkDistance();
        if (Input.GetKeyDown(KeyCode.J) && dist) {
            questGiver.OpenPort(this);
            questGiver.CheckQuest();
        }
    }

    private void checkDistance() {
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > 20f) {
            dist = false;
            //questGiver.show(false);
        }
        else {
            dist = true;
            //questGiver.show(true);
            questGiver.SetQuest(this);
            questGiver.SetPortText(this);
            questGiver.SetPort(this);
        }
    }

}
