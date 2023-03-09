using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    public GameObject player;

    private PlayerRotation rotation;

    // Start is called before the first frame update
    void Start()
    {
        rotation = player.GetComponent<PlayerRotation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PushUpwards(float magnitude, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(PushUpwardsCoroutine(magnitude, duration));
    }

    private IEnumerator PushUpwardsCoroutine(float magnitude, float duration)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float y = (1 - elapsed / duration) * magnitude * Time.deltaTime;

            rotation.addRotationX(-y);

            elapsed += Time.deltaTime;

            yield return null;
        }
    }
}
