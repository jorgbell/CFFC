using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public List<StackedSpriteEffect> frames;

    public int currentFrame = 0;

    private void Start()
    {
        frames = new List<StackedSpriteEffect>();
        foreach (StackedSpriteEffect sprite in GetComponentsInChildren<StackedSpriteEffect>())
        {
            frames.Add(sprite);
        }
        for (int i = 1; i < frames.Count; i++) frames[i].gameObject.SetActive(false);
    }
    public void NextFrame()
    {
        if (currentFrame < frames.Count)
        {
            frames[currentFrame].gameObject.SetActive(false);
            currentFrame = (currentFrame + 1) % frames.Count;
            frames[currentFrame].gameObject.SetActive(true);
        }
    }

    public void SetStartFrame()
    {
        if (frames.Count > 0)
        {
            frames[currentFrame].gameObject.SetActive(false);
            currentFrame = 0;
            frames[currentFrame].gameObject.SetActive(true);
        }
    }
}
