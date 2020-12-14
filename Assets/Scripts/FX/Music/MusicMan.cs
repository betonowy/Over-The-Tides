using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMan : MonoBehaviour {
    public enum TRACK_TYPE {
        CHILL,
        HORDE,
        BOSS
    }

    [System.Serializable]
    public class AudioSet {
        public AudioClip track;
        public float volume;
        public TRACK_TYPE type;
    }

    public AudioSet[] music;
    public float fadeOutTime = 2;
    public float fadeInTime = 5;

    private AudioSource aSource;
    private TRACK_TYPE currentMood = TRACK_TYPE.CHILL;
    private int currentTrack = 0;

    void Start() {
        aSource = GetComponent<AudioSource>();
        aSource.volume = 0;
        aSource.clip = music[currentTrack].track;
        aSource.Play();
    }

    void Update() {
        if (TypeCorrect()) {
            Debug.Log("FadeIn");
            FadeIn();
        } else {
            Debug.Log("Wrong track... FadeOut!");
            FadeOut();
            if (Silenced()) {
                Debug.Log("Wrong track... Changing!");
                SetNextTrack();
            }
        }

        if (!aSource.isPlaying) {
            Debug.Log("Track ended... Changing!");
            aSource.volume = 0;
            SetNextTrack();
        }
    }

    void FadeOut() {
        UpdateClipVolume(aSource.volume - music[currentTrack].volume * Time.deltaTime / fadeOutTime, 0, music[currentTrack].volume);
    }

    void FadeIn() {
        UpdateClipVolume(aSource.volume + music[currentTrack].volume * Time.deltaTime / fadeOutTime, 0, music[currentTrack].volume);
    }

    bool TypeCorrect() {
        return music[currentTrack].type == currentMood;
    }

    void SetNextTrack() {
        currentTrack++;
        if (currentTrack >= music.Length) currentTrack = 0;
        ChangeClip(music[currentTrack].track);
        aSource.Play();
    }

    void UpdateClipVolume(float vol, float min, float max) {
        aSource.volume = Mathf.Clamp(vol, min, max);
    }

    void ChangeClip(AudioClip ac) {
        aSource.clip = ac;
    }

    bool Silenced() {
        return aSource.volume == 0;
    }
}
