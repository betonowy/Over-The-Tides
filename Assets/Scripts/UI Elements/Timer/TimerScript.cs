using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TimerScript : MonoBehaviour
{
    private float timeModifier = 1f;
    private float countdownTime;
    public TextMeshProUGUI countdownDisplay;

    public HordeMan hordeMan;

    private void Start() {
        countdownTime = hordeMan.GetCountdown();
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart() {
        while (true) {
            if (countdownTime >= 0) {
                TimeSpan t = TimeSpan.FromSeconds(countdownTime);
                countdownDisplay.text = t.Minutes + ":" + t.Seconds;

                yield return new WaitForSeconds(1f);
                countdownTime--;
            } else {
                countdownTime = hordeMan.GetCountdown();
            }
        }
        
    }

    public void SetcountdownTime(float time) {
        countdownTime = time;
    }

    public void NormalSpeed() {
        timeModifier = 1f;
    }
    public void DoubleSpeed() {
        timeModifier = 2f;
    }
    public void FourfoldSpeed() {
        timeModifier = 4f;
    }
    public void EightfoldSpeed() {
        timeModifier = 8f;
    }

    public float GetTimeModifier() {
        return timeModifier;
    }


}
