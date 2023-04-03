using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    Settings previousSettings;
    bool saved = false;

    private void Start()
    {
    }


    private void OnEnable()
    {
        saved = false;
        actualSettings = GameManager._instance.m_settings;
        previousSettings = new Settings { fov = actualSettings.fov, volume = actualSettings.volume, xSensitivity = actualSettings.xSensitivity, ySensitivity = actualSettings.ySensitivity};
        SetUIValues();
        if (RoundManager.instance != null) TogglePause();
    }
    private void OnDisable()
    {
        if (RoundManager.instance != null) TogglePause();
    }
    public void GoToMainMenu()
    {
        //si no se ha pulsado en guardar, resetea las opciones a las que había cuando se entró en el menú
        if (!saved)
        {
            actualSettings = previousSettings;
            SetUIValues();
        }
        if (RoundManager.instance != null) { TogglePause(); }
        GameManager._instance.ebackToMainMenu.Invoke();
    }
    public void TogglePause()
    {
        GameManager._instance.PauseUnpause();
        Cursor.lockState = (GameManager._instance.IsGameRunning()) ? CursorLockMode.Locked : CursorLockMode.None;
    }
    public void SaveCurrent()
    {
        saved = true;
        GameManager._instance.eSetSettings.Invoke(actualSettings);
        SerializaztionManager._instance.SaveSettings();
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

    public void SetSensX()
    {
        actualSettings.xSensitivity = SensXSldr.value;
    }
    public void SetSensY()
    {
        actualSettings.ySensitivity = SensYSldr.value;
    }

    void SetUIValues()
    {
        SensXSldr.value = actualSettings.xSensitivity;
        SensYSldr.value = actualSettings.ySensitivity;
        VolumeSldr.value = actualSettings.volume;
        FOVSldr.value = actualSettings.fov;
    }

}
