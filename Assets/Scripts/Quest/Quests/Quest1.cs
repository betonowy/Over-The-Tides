using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest1 : QuestScript {
    public ItemDatabaseObject database;
    void Start()
    {
        QuestName = "Save us!";
        Description = "Kill all pirates";
        ItemReward = database.FindItem(1);

        Goals.Add(new KillGoalClass(this, "Kill all pirates", false, 0, 3));

        Goals.ForEach(g => g.Init());
        
    }

}
