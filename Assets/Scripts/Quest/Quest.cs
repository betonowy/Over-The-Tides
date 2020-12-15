﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest {

    public bool isActive;
    public bool isCompleted = false;

    public string title;
    public string description;
    public string completed;
    public string reward;
    public Item item;

    public QuestGoal goal;

    public void Complete() {
        isActive = false;
        Debug.Log("Quest Completed");
    }
}
