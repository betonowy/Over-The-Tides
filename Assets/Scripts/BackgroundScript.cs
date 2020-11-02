﻿using UnityEngine;
public class BackgroundScript : MonoBehaviour {
    MeshRenderer mr;
    GameObject cam;

    public Sprite[] frames;
    int frameNum = 0;
    int framesTillNextAni;

    public int minDelay = 10;
    public int maxDelay = 20;

    public float spriteScale = 1;

    private void Start() {
        //transform.localScale = new Vector3(transform.localScale.x * 2f, transform.localScale.y * 2f, 0);
        mr = GetComponent<MeshRenderer>();
        //mr.sortingLayerName
        mr.sortingOrder = 0;
        cam = GameObject.Find("Main Camera");
        resetCounter();
        mr.material.mainTextureScale = new Vector2(spriteScale, spriteScale);
    }
    private void Update() {
        mr.material.mainTextureOffset = cam.transform.position * spriteScale;

        if (frameNum == 0) {
            if (framesTillNextAni-- < 0) {
                resetCounter();
                advanceFrame();
            }
        } else {
            advanceFrame();
        }
    }

    void resetCounter() {
        framesTillNextAni = Random.Range(minDelay, maxDelay);
    }

    void advanceFrame() {
        mr.material.mainTexture = frames[frameNum++].texture;
        if (frameNum >= frames.Length) frameNum = 0;
        mr.material.mainTextureScale = new Vector2(spriteScale, spriteScale);
    }

}