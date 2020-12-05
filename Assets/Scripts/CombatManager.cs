using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public Quest quest;

    public PlayerScript player;
    public GameObject text;

    public void SetQuest(Quest playerQuest) {
        quest = playerQuest;
    }

    IEnumerator ShowMessage() {
        yield return new WaitForSeconds(3);
        text.SetActive(false);
    }

    void OnEnemyDeath(ShipScript ship) {
        if (ship == null)
            Debug.Log("nulls");
        if (quest.isActive) {
            if (ship.GetComponent<ShipScript>().team == ShipScript.teamEnum.teamRed) {
                quest.goal.EnemyKilled();
            }
            if (quest.goal.IsReached()) {
                player.createCannon();
                quest.Complete();
                text.SetActive(true);
                StartCoroutine(ShowMessage());
            }
        }
    }

}
