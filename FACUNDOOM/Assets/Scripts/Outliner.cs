using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Outliner : MonoBehaviour
{
    public Color outlineColor;
    public RenderTexture sourceTexture;
    public Sprite targetImage;
    Texture2D targetTexture;

    private void Awake()
    {
        targetTexture = new Texture2D(sourceTexture.width, sourceTexture.height);
        targetRawImage.texture = targetTexture;
    }

    private void Update()
    {
        RenderTexture.active = sourceTexture;
        targetTexture.ReadPixels(new Rect(0, 0, sourceTexture.width, sourceTexture.height), 0, 0);

        Color[] pixels = targetTexture.GetPixels();

        for(int i = 0; i < pixels.Length; i++) 
        {
            Color pixel = pixels[i];

            if (pixel.r + pixel.g + pixel.b < 0.1) pixels[i].a = 0;
            else pixels[i] = outlineColor;
        }

        targetTexture.SetPixels(pixels);

        targetTexture.Apply();
        RenderTexture.active = null;
    }
}
