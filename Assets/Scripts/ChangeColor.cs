using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public SpriteRenderer[] sprites;
    public float speedChange;
    private float hue = 0;
    public float value;
    public float saturation;

    // Update is called once per frame
    void Update()
    {
        hue = (hue + (speedChange * Time.deltaTime)) % 360;
        for(int i = 0; i < sprites.Length; i++) {
            sprites[i].color = Color.HSVToRGB(hue/360, value, saturation);
        }
    }
}
