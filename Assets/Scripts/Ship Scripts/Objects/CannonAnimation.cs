using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CannonAnimation : MonoBehaviour {
    public AnimationV1 shootingAnimation;
    public AnimationV1 readyAnimation;

    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void AnimateShoot() {
        shootingAnimation.RunAnimation();
    }

    public void AnimateReady() {
        readyAnimation.RunAnimation();
    }
}
