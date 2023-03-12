using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverButton : MonoBehaviour
{
    bool hoverButton = false;
    public void enterHoverButton()
    {
        if (!hoverButton)
        {
            AudioManager.instance.Play("SFX_moveButton");
            hoverButton = true;
        }
    }
    public void exitHoverButton()
    {
        hoverButton = false;
    }
}
