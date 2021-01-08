using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Flag{

    public teamType captor;
    public defenceLevel defenceLevel;

    public bool isProducing;
    public string description;
    public string leveUpDescription;
    public string reward;
    public string completed;
    
    public FlagGoal goal;

    public Item itemProducing;
    public Item item;
    
    public void Captured(teamType team){
        captor = team;
        Debug.Log("Captured"); //delete later!
    }

    public void LevelUp(){
        defenceLevel++;
    }

    public void LevelDown(){
        defenceLevel = defenceLevel.Zero;
    }
    
}

public enum defenceLevel{
    Zero,
    One,
    Two,
    Three,
    Four
}


