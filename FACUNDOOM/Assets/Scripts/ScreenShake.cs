using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScreenShake : MonoBehaviour
{
    [SerializeField]
    GameObject target;

    [Range(0.1f, 10)]
    public float duration;

    [Range(0.1f, 1000)]
    public float magnitude;

    private bool shaking;



    void Start()
    {
        RoundManager.instance.eBombExploded.AddListener(Shake);
    }

    public void Shake()
    {
        Debug.Log("Shakerino");

        if (!shaking) 
        {
            StopAllCoroutines();
            StartCoroutine(ShakeCoroutine(magnitude, magnitude));
            shaking = true;
        }
    }

    private IEnumerator ShakeCoroutine(float xMagnitude, float yMagnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * xMagnitude;
            float y = Random.Range(-1f, 1f) * yMagnitude;

            GetComponent<PlayerRotation>().setScreenShake(new Vector3(x, y, 0));

            elapsed += Time.deltaTime;

            yield return null;
        }

        GetComponent<PlayerRotation>().setScreenShake(new Vector3(0, 0, 0));
        shaking = false;
    }
}