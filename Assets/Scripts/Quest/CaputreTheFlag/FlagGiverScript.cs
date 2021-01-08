using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlagGiverScript : MonoBehaviour{

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI leveUpDescriptionText;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI completedText;

    public GameObject flagWindow;
    public GameObject flagLog;
    
    public FlagScript[] flags;

    private FlagScript flag;
    private InventoryObject inventory;

    public bool dirtyFlag = false;
    private bool[] playerDistance;

    private bool flagWindowFlag = false;
    
    private void Start(){
        FillFlags();
        playerDistance = new bool[flags.Length];
    }

    private void Update(){
        if (dirtyFlag){
            List<GameObject> redShips = SetRedShipArray();
            foreach (FlagScript falg in flags){
                SendMessage("SetRedShips", redShips);
            }
            dirtyFlag = false;
        }
        
            
    }

    private void FillFlags(){
        for (int i = 0; i < flags.Length; i++){
            StringFillerFlags(i, flags[i]);
        }
    }

    private void StringFillerFlags(int i, FlagScript flag){
        switch (i){
            case 0:
                flag.flag.title = "A";
                flag.flag.description = "A";
                flag.flag.leveUpDescription = "A";
                flag.flag.reward = "A";
                flag.flag.completed = "A";
                break;
        }
    }

    public List<GameObject> SetRedShipArray(){
        List<GameObject> redShips = new List<GameObject>();
        GameObject[] ship = GameObject.FindGameObjectsWithTag("Ship");
        foreach (GameObject s in ship) {
            if (s.GetComponent<ShipScript>().team == ShipScript.teamEnum.teamRed) 
                redShips.Add(s);
        }
        return redShips;
    }

    public void SetFlag(FlagScript f){
        flag = f;
    }

    public void SetFlagText(FlagScript f){
        titleText.text = f.flag.title;
        descriptionText.text = f.flag.description;
        leveUpDescriptionText.text = f.flag.leveUpDescription;
        rewardText.text = f.flag.reward;
        completedText.text = f.flag.completed;
    }
    
    private void CheckWindow(){
        int counter = 0;
        foreach (FlagScript f in flags) {
            if (Vector3.Distance(f.transform.position, f.player.transform.position) < 20f)
                playerDistance[counter] = true;
            else
                playerDistance[counter] = false;
            counter++;
        }
        for (int i = 0; i < counter; i++)
            if (playerDistance[i] == true)
                flagWindowFlag = true;
        switch (flagWindowFlag){
            case true:
                flagWindow.SetActive(true);
                break;
            case false:
                flagWindow.SetActive(false);
                break;
        }

        flagWindowFlag = false;
    }
    

    public void SetDirtyFlag(){
        dirtyFlag = true;
    }

    private void ProduceItems(){
        foreach (FlagScript flag in flags){
            if(flag.flag.captor == teamType.Blue)
                flag.player.GenerateReward(flag.flag.reward);
        }
    }

    public void OpenFlag(FlagScript f){
        flagLog.SetActive(true);
        GameObject.Find("playerBoatBlue").GetComponent<PlayerScript>().SendMessage("AllowMovement", false);
        if(f.flag.goal.WhoReached() == teamType.Blue) {
            descriptionText.gameObject.SetActive(false);
            completedText.gameObject.SetActive(true);
        }
        else {
            descriptionText.gameObject.SetActive(true);
            completedText.gameObject.SetActive(false);
        }
            
    }

    public void GiveItems(){
        InventorySlot inventorySlot = inventory.FindItemOnInventory(flag.flag.item);
        int i = inventorySlot.amount;
        if (inventorySlot != null){
            if (i == flag.flag.goal.itemsRequired){
                flag.player.GenerateReward(flag.flag.reward);
                flag.flagInterface.RemoveAll();
            } else if (i > flag.flag.goal.itemsRequired){
                int temp = i - flag.flag.goal.itemsRequired;

                string s;
                switch (flag.flag.item.Id){
                    case 0:
                        s = "Reward: gold " + temp;
                        flag.player.GenerateReward(s);
                        break;
                    case 2:
                        s = "Reward: plank " + temp;
                        flag.player.GenerateReward(s);
                        break;
                }
            }
        } 
    }

    public void Quit(){
        GameObject.Find("FlagLog").gameObject.SetActive(false);
        GameObject.Find("playerBoatBlue").GetComponent<PlayerScript>().SendMessage("AllowMovement", true);
    }
    
}
