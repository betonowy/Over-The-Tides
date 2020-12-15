using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkScript : MonoBehaviour
{
    public int blinkTime = 0;
    private int blinkCounter = 0;

    private SpriteRenderer mySprite;

    // Start is called before the first frame update
    void Start()
    {
        mySprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (++blinkCounter >= blinkTime) {
            blinkCounter = 0;
            mySprite.enabled = !mySprite.enabled;
        }
    }
}
