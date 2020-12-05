using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestScript : MonoBehaviour {
    public List<GoalClass> Goals { get; set; } = new List<GoalClass>();
    public string QuestName { get; set; }
    public string Description { get; set; }
    public Item ItemReward { get; set; }
    public bool Completed { get; set; }

    public InventoryObject inventory;

    public void CheckGoals() {
        Completed = Goals.All(g => g.Completed);
    }
    
    void GiveRward() {
        if (ItemReward != null)
            inventory.AddItem(ItemReward, 1);
    }
}
