using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlagGiverScript : MonoBehaviour{

    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI leveUpDescriptionText;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI completedText;
    
    public FlagScript[] flags;

    private FlagScript flag;
    private InventoryObject inventory;

    public bool dirtyFlag = false;
    private bool[] playerDistance;

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

    private bool CheckWindow(){
        return true;
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
}
