using UnityEngine;

public class WaterRipple : MonoBehaviour {

    public float timeToLive = 5;
    public float scaleSpeed = 0.1f;
    public float startingScale = 0.1f;

    private float currentScale = 0;
    private float startLive = 0;

    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start() {
        sr = GetComponent<SpriteRenderer>();
        currentScale = startingScale;
        startLive = timeToLive;
        Initialize();
    }

    // Update is called once per frame
    void Update() {
        UpdateScale();
        UpdateAlpha();
        LifeCheck();
    }

    private void Initialize() {
        UpdateScale();
        UpdateAlpha();
    }

    private void UpdateScale() {
        currentScale += scaleSpeed * Time.deltaTime;
        Vector3 scale = gameObject.transform.localScale;
        scale.x = currentScale;
        scale.y = currentScale;
        scale.z = currentScale;
        gameObject.transform.localScale = scale;
    }

    private void UpdateAlpha() {
        Color c = sr.material.color;
        c.a = timeToLive / startLive;
        sr.material.color = c;
    }

    private void LifeCheck() {
        if ((timeToLive -= Time.deltaTime) < 0) {
            Destroy(gameObject);
        }
    }
}
