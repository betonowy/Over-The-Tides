using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalClass {
    public QuestScript Quest { get; set; }
    public string Description { get; set; }
    public bool Completed { get; set; }
    public int CurrentAmount { get; set; }
    public int RequiredAmount { get; set; }

    public virtual void Init() {
        
    }

    public void Evaluate() {
        if (CurrentAmount >= RequiredAmount)
            Complete();
    }

    public void Complete() {
        Quest.CheckGoals();
        Completed = true;
        Debug.Log("Goal marked as completed");
    }
}
