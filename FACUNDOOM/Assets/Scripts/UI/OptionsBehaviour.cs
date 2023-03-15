using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class OptionsBehaviour : MonoBehaviour
{
    [SerializeField]
    Slider SensXSldr;
    [SerializeField]
    Slider SensYSldr;
    [SerializeField]
    Slider VolumeSldr;
    [SerializeField]
    Slider FOVSldr;

    Settings actualSettings;

    private void Start()
    {
        actualSettings = GameManager._instance.m_settings;
    }


    private void OnEnable()
    {
        VolumeSldr.value = GameManager._instance.m_settings.volume; //AudioListener.volume;
        FOVSldr.value = GameManager._instance.m_settings.fov; ;//CurCam.fieldOfView;
        SensXSldr.value = GameManager._instance.m_settings.xSensitivity; //_roundManager.getPlayer().GetComponent<PlayerRotation>().xSensitivity;
        SensYSldr.value = GameManager._instance.m_settings.ySensitivity;// _roundManager.getPlayer().GetComponent<PlayerRotation>().ySensitivity;
        if (RoundManager.instance != null) TogglePause();
    }
    private void OnDisable()
    {
        GameManager._instance.eSetSettings.Invoke(actualSettings);
        if (RoundManager.instance != null) TogglePause();
    }
    public void GoToMainMenu()
    {
        GameManager._instance.eSetSettings.Invoke(actualSettings);
        if (RoundManager.instance != null) { TogglePause(); }
        GameManager._instance.ebackToMainMenu.Invoke();
    }
    public void TogglePause()
    {
        GameManager._instance.PauseUnpause();
        Cursor.lockState = (GameManager._instance.IsGameRunning()) ? CursorLockMode.Locked : CursorLockMode.None;
    }


    public void SetFOV()
    {
        actualSettings.fov = FOVSldr.value;
        if (RoundManager.instance != null) RoundManager.instance.CurCam.fieldOfView = actualSettings.fov;
    }

    public void SetVol()
    {
        actualSettings.volume = VolumeSldr.value;
        AudioListener.volume = VolumeSldr.value;
    }

    public void SetSens()
    {
        actualSettings.xSensitivity = SensXSldr.value;
        actualSettings.ySensitivity = SensYSldr.value;
    }

}
