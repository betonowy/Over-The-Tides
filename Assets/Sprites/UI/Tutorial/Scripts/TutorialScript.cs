using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    public Sprite[] popUps;

    [SerializeField]
    private GameObject obj;

    [SerializeField]
    private GameObject Image;

    private int popUpIndex = 0;
    private bool flag = true;

    private bool playerScriptCaptured = false;
    private PlayerScript ps;

    private void Start() {
        Image.GetComponent<Image>().sprite = popUps[0];
    }

    public void Next() {
        popUpIndex++;
        Debug.Log(popUpIndex);
        if(popUpIndex <= 6)
            Image.GetComponent<Image>().sprite = popUps[popUpIndex];
    }

    public void Back() {
        popUpIndex--;
        if(popUpIndex >= 0)
            Image.GetComponent<Image>().sprite = popUps[popUpIndex];
    }

    public void Hide() {
        if (flag) {
            obj.SetActive(false);
            flag = false;
        } else {
            obj.SetActive(true);
            flag = true;
        }
    }

    private void Update() {
        if (!playerScriptCaptured) {
            ps = FindObjectOfType<PlayerScript>();
            if (ps != null) {
                playerScriptCaptured = true;
                Rect[] newRects = new Rect[2];
                Rect newRect = new Rect();
                float ratio = (float)Screen.height / Screen.width;
                // tutorial window
                newRect.width = 0.47f * ratio;
                newRect.height = 0.53f;
                newRect.x = 0.07f * ratio;
                newRect.y = 0.5f - newRect.height / 2;
                newRects[0] = newRect;

                // hide button
                newRect.width = 0.16f * ratio;
                newRect.height = 0.1f;
                newRect.x = 0.04f * ratio;
                newRect.y = 0.03f;
                newRects[1] = newRect;

                ps.deniedRects = newRects;
                ps.rectDenied = true;
            }
        }
    }

    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
