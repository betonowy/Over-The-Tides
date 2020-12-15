using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lilHealthBar : MonoBehaviour {

    public float lengthMultipier = 1;
    public float barThickness = 0.5f;
    public Vector2 relativePosition;

    public Sprite redSprite;
    public Sprite blueSprite;

    private GameObject masterObject;
    private SpriteRenderer renderer;

    void Start() {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        var col = renderer.color;
        col.a = 0.5f;
        renderer.color = col;
    }

    void Update() {
        try {
            UpdatePosition();
        } catch {
            LifeBarsDieWhenTheyAreKilled();
        }
    }

    void UpdatePosition() {
        transform.position = masterObject.transform.position + new Vector3(relativePosition.x, relativePosition.y, 0);
    }

    public void SetMaster(GameObject master) {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        masterObject = master;
        if (masterObject.GetComponent<ShipScript>().team == ShipScript.teamEnum.teamBlue) {
            renderer.sprite = blueSprite;
        } else {
            renderer.sprite = redSprite;
        }
        UpdatePosition();
    }

    public void UpdateValue(float val) {
        Vector3 scale = transform.localScale;
        scale.y = val * lengthMultipier;
        transform.localScale = scale;
    }

    public void LifeBarsDieWhenTheyAreKilled() {
        Destroy(gameObject);
    }
}
