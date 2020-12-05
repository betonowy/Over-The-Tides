using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorScript : MonoBehaviour
{
    public Sprite redInd;
    public Sprite greenInd;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchRed() {
        spriteRenderer.sprite = redInd;
    }


    public void switchGreen() {
        spriteRenderer.sprite = greenInd;
    }    
}
