using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[System.Serializable]
public class AnimationV1 : MonoBehaviour {
    [System.Serializable]
    public struct AnimationFrame {
        public Vector2 offset;
        public float timePoint;
    }

    [System.Serializable]
    public struct PictureFrame {
        public Sprite picture;
        public float timePoint;
    }

    public AnimationFrame[] aniFrames;
    public PictureFrame[] picFrames;
    private bool ready = false;
    private float animationTimePoint;
    private int animationIndex;
    private float pictureTimePoint;
    private int pictureIndex;

    private SpriteRenderer sprite;

    void Start() {
        ready = true;
        animationTimePoint = 0;
        pictureTimePoint = 0;

        try {
            animationTimePoint = aniFrames[aniFrames.Length - 1].timePoint;
        } catch {
            ready = false;
        }

        try {
            pictureTimePoint = picFrames[picFrames.Length - 1].timePoint;
        } catch {
            ready = false;
        }

        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        if (ready && animationTimePoint < aniFrames[aniFrames.Length - 1].timePoint && animationIndex < aniFrames.Length) {
            animationTimePoint += Time.deltaTime;
            if (animationTimePoint > aniFrames[animationIndex + 1].timePoint) {
                animationIndex++;
            }
            if (animationIndex + 1 < aniFrames.Length) {
                gameObject.GetComponent<ParentConstraint>().SetTranslationOffset(0, InterpolateVec2Linear(
                    aniFrames[animationIndex].offset, aniFrames[animationIndex + 1].offset,
                    aniFrames[animationIndex].timePoint, aniFrames[animationIndex + 1].timePoint,
                    animationTimePoint
                    ));
            } else {
                gameObject.GetComponent<ParentConstraint>().SetTranslationOffset(0, aniFrames[animationIndex].offset);
            }
        }

        if (ready && pictureTimePoint < picFrames[picFrames.Length - 1].timePoint && pictureIndex < picFrames.Length) {
            pictureTimePoint += Time.deltaTime;
            if (pictureTimePoint > picFrames[pictureIndex].timePoint) {
                pictureIndex++;
                if (pictureIndex < picFrames.Length) {
                    sprite.sprite = picFrames[pictureIndex].picture;
                } else {
                    sprite.sprite = picFrames[0].picture;
                }
            }
        }
    }

    public void RunAnimation() {
        animationTimePoint = 0;
        pictureTimePoint = 0;
        animationIndex = 0;
        pictureIndex = 0;
        gameObject.transform.position = aniFrames[0].offset;
        sprite.sprite = picFrames[0].picture;
    }

    public Vector2 InterpolateVec2Linear(Vector2 first, Vector2 second, float beginVal, float endVal, float point) {
        if (beginVal == endVal) {
            if (point > beginVal) {
                Debug.Log("First");
                return first;
            } else {
                Debug.Log("Second");
                return second;
            }
        }

        float ratio = (point - beginVal) / (endVal - beginVal);

        Debug.Log("Ratio: " + ratio);

        if (ratio > 1) {
            ratio = 1;
        } else if (ratio < 0) {
            ratio = 0;
        }

        return first * (1 - ratio) + second * ratio;
    }
}
