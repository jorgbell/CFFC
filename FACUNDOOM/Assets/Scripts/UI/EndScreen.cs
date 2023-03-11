using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
public class EndScreen : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    TMPro.TextMeshProUGUI text;
    void Start()
    {
        text.text = "Your score:\n" + GameManager._instance.GetPlayerScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
