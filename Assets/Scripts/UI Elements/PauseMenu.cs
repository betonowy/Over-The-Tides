using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    private float timeScaleMulti;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public AudioMixer audioMixer;
    public MusicMan musicMan;

    private void Start() {
        musicMan = GameObject.Find("LevelManager").GetComponent<MusicMan>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                ResumeGame();
            } else {
                PauseGame();
            }
        }
    }

    public void ResumeGame() {
        if (optionsMenu.active) {
            optionsMenu.SetActive(false);
        }
        pauseMenu.SetActive(false);
        Time.timeScale = timeScaleMulti;
        isPaused = false;
    }

    void PauseGame() {
        timeScaleMulti = Time.timeScale;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void OpenOptions() {
        optionsMenu.SetActive(true);
    }

    public void QuitGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void GoBack() {
        optionsMenu.SetActive(false);
    }

    public void SetVolume (float volume) {
        audioMixer.SetFloat("Volume", volume);
    }
}
