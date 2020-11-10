using UnityEngine;
public class BackgroundScript : MonoBehaviour {
    MeshRenderer mr;
    GameObject cam;

    public Sprite[] frames;
    int frameNum = 0;
    int framesTillNextAni;

    public int minDelay = 10;
    public int maxDelay = 20;

    public float spriteScale = 1;
    public int frameUnskip = 4;
    private int frameCounter = 0;

    private float offsetScale;

    private void Start() {
        cam = transform.parent.gameObject;
        //transform.localScale = new Vector3(transform.localScale.x * 2f, transform.localScale.y * 2f, 0);
        mr = GetComponent<MeshRenderer>();
        //mr.sortingLayerName
        mr.sortingOrder = 0;
        resetCounter();
        mr.material.mainTextureScale = new Vector2(spriteScale, spriteScale);
        offsetScale = 1 / ((gameObject.transform.lossyScale.x + gameObject.transform.lossyScale.x) / 2);
    }
    private void Update() {
        mr.material.mainTextureOffset = cam.transform.position * spriteScale * offsetScale;

        if (frameNum == 0) {
            if (framesTillNextAni-- < 0) {
                resetCounter();
                advanceFrame();
            }
        } else {
            if (++frameCounter > frameUnskip) {
                advanceFrame();
                frameCounter = 0;
            }
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
