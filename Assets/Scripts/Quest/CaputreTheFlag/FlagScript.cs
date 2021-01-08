using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;


[System.Serializable]
public class FlagScript : MonoBehaviour{

    private float distance;
    private bool logDistance;
    private bool isClose;
    
    private List<GameObject> redShips = new List<GameObject>();
    
    public Flag flag;
    
    [System.NonSerialized]
    public PlayerScript player;
    [System.NonSerialized]
    public FlagGiverScript flagGiver;

    public FlagInterface flagInterface;

    private void Start(){
        player = GameObject.Find("playerBoatBlue").GetComponent<PlayerScript>();
        flagGiver = GameObject.Find("LevelManager").GetComponent<FlagGiverScript>();
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.J) && CheckLogDistance()){
            flagGiver.OpenFlag(this);
        }
    }

    private bool CheckLogDistance(){
        distance = Vector3.Distance(transform.position, player.transform.position);
        switch (distance > 20f){
            case true:
                return false;
            default:
                flagGiver.SetFlag(this);
                flagGiver.SetFlagText(this);
                return true;
        }
    }

    public void SetRedShips(List<GameObject> newList){
        redShips = newList;
    }
    
    private void CheckCaptor(){
        bool red = false;
        bool blue = Vector3.Distance(transform.position, player.transform.position) < 100f;

        foreach (GameObject r in redShips){
            if (Vector3.Distance(transform.position, r.transform.position) < 100f)
                red = true;
        }
        
        switch (blue){
            case true when !red:
                flag.goal.BlueCapture();
                break;
            case false when red:
                flag.goal.RedCapture();
                break;
        }
    }
    
}
