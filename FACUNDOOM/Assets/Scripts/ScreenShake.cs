using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScreenShake : MonoBehaviour
{
    [Range(0.1f, 10)]
    public float duration;

    [Range(0.1f, 1000)]
    public float magnitude;

    private Vector3 originalPosition;
    private bool shaking;

    void Start()
    {
        
    }

    public void Shake(float xMagnitude, float yMagnitude)
    {
        if (!shaking) 
        {
            StopAllCoroutines();
            StartCoroutine(ShakeCoroutine(xMagnitude, yMagnitude));
            shaking = true;
        }
    }

    private IEnumerator ShakeCoroutine(float xMagnitude, float yMagnitude)
    {
        float elapsed = 0.0f;
        originalPosition = transform.localPosition;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * xMagnitude;
            float y = Random.Range(-1f, 1f) * yMagnitude;

            transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
        shaking = false;
    }
}