using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    List<StackedSpriteEffect> frames;

    int currentFrame = 0;

    private void Start()
    {
        frames = new List<StackedSpriteEffect>();
        foreach (StackedSpriteEffect sprite in GetComponentsInChildren<StackedSpriteEffect>())
        {
            frames.Add(sprite);
        }
    }
    public void NextFrame()
    {
        frames[currentFrame].gameObject.SetActive(false);
        currentFrame = (currentFrame + 1) % frames.Count;
        frames[currentFrame].gameObject.SetActive(true);
    }
}
