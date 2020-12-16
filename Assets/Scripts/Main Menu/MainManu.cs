using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainManu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public AudioMixer mainMixer;

    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
    public void PlayTutorialGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);
    }
    public void OpenOptions() {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    public void SetVolume(float volume) {
        mainMixer.SetFloat("Volume", volume);
    }
    public void GoBack() {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void QuitGame() {
        Application.Quit();
    }
}
