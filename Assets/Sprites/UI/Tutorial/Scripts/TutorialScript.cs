using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
    

    private void Start() {
        Image.GetComponent<Image>().sprite = popUps[1];
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

    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
