using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideInBuild : MonoBehaviour
{
    [SerializeField]
    bool WebGL = false;

    private void Awake()
    {
        if (WebGL && Application.platform == RuntimePlatform.WebGLPlayer) gameObject.SetActive(false);
    }
    private void Update()
    {
        if(gameObject.activeSelf && WebGL && Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Debug.LogWarning("this object is set to always inactive in this build. deactivating...");
            gameObject.SetActive(false);
        }
    }
}
