using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TimerScript : MonoBehaviour
{
    private float timeModifier = 1f;
    public float countdownTime;
    public TextMeshProUGUI countdownDisplay;
    private void Start() {
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart() {
        while (countdownTime >= 0f) {
            TimeSpan t = TimeSpan.FromSeconds(countdownTime);
            countdownDisplay.text = t.Minutes+ ":" + t.Seconds;

            yield return new WaitForSeconds(1f);
            countdownTime--;
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
