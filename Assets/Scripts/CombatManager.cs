using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public PortScript[] ports;

    public PlayerScript player;
    public GameObject text;

    IEnumerator ShowMessage() {
        yield return new WaitForSeconds(3);
        text.SetActive(false);
    }

    void OnEnemyDeath(ShipScript ship) {
        if (ship == null)
            Debug.Log("nulls");
        foreach (PortScript port in ports) {
            if(port.quest.isActive) {
                if (ship.GetComponent<ShipScript>().team == ShipScript.teamEnum.teamRed)
                    port.quest.goal.EnemyKilled();
                if(port.quest.goal.IsReached()) {
                    port.quest.isCompleted = true;
                    port.isCompleted = true;
                    text.SetActive(true);
                    port.QuestCompleted();
                    port.quest.isActive = false;
                    StartCoroutine(ShowMessage());
                }
            }
        }
    }
}
