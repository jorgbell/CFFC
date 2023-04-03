using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SerializaztionManager : MonoBehaviour
{
    [SerializeField]
    string settingsFileName;
    [SerializeField]
    string scoreboardFileName;
    public static SerializaztionManager _instance { get; private set; }
    private void Awake()
    {
        if (_instance == null)
        {
            Debug.Log("Serialization instanced");
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// carga todos los datos necesarios
    /// </summary>

    public void LoadSettings()
    {
        Settings s = new Settings();
        string json = ReadFromFile(settingsFileName);
        if (json != null)
        {
            JsonUtility.FromJsonOverwrite(json, s);
            Debug.Log(settingsFileName + " loaded!");
        }
        else Debug.LogWarning("couldn´t load " + settingsFileName+" . Creating new settings file.");
        GameManager._instance.eSetSettings.Invoke(s);
    }
    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(GameManager._instance.m_settings);
        WriteToFile(settingsFileName, json);
    }
    public bool LoadScoreboard()
    {
        string json = ReadFromFile(scoreboardFileName);
        if (json != null)
        {
            ScoreList sb = new ScoreList();
            JsonUtility.FromJsonOverwrite(json, sb);
            GameManager._instance.localScoreboard = sb;
            Debug.Log(scoreboardFileName + "local scoreboard loaded!");
            return true;
        }
        else Debug.LogWarning("couldn´t load " + scoreboardFileName);
        return false;
    }
    public void SaveScoreboard()
    {
        string json = JsonUtility.ToJson(GameManager._instance.localScoreboard);
        WriteToFile(scoreboardFileName, json);  
    }


    private void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }

    private string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
        {
            Debug.LogWarning("File not found");
        }

        return null;
    }

    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }
}
