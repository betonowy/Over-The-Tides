using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScreenScript : MonoBehaviour
{
    public GameObject endingScreen;

    public void CloseEndingScreen() {
        endingScreen.SetActive(false);
    }
}
