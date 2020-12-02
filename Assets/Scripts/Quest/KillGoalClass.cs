using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGoalClass : GoalClass {

    public KillGoalClass(QuestScript quest, string description, bool completed, int currentAmount, int requiredAmount) {
        this.Quest = quest;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
    }

    public override void Init() {
        base.Init();
        
    }

    private void EnemyDied(ShipScript ship) {
        if(ship.team == ShipScript.teamEnum.teamRed) {
            this.CurrentAmount++;
            Evaluate();
        }
    }

}
