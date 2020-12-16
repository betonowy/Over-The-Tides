using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TimerScript : MonoBehaviour
{
    private float countdownTime;
    public TextMeshProUGUI countdownDisplay;

    public HordeMan hordeMan;

    private void Update() {
        countdownTime = hordeMan.GetCountdown();
        TimeSpan time = TimeSpan.FromSeconds(countdownTime);
        string s = string.Format("{0:D2}:{1:D2}",time.Minutes,time.Seconds);
        countdownDisplay.text = s;
    }

    public void SetcountdownTime(float time) {
        countdownTime = time;
    }

    public void NormalSpeed() {
        Time.timeScale = 1f;
    }
    public void DoubleSpeed() {
        Time.timeScale = 2f;
    }
    public void FourfoldSpeed() {
        // timeModifier = 4f;
        Time.timeScale = 4f;
    }
    public void EightfoldSpeed() {
        Time.timeScale = 4;
    }
}
