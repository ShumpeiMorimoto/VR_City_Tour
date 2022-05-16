using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadWorld : MonoBehaviour
{
    public string filePath;

    public static LoadWorld instance;
    private void Awake()
    {
        instance = this;

        if (SceneManager.GetActiveScene().name == "World1")
            filePath = Application.persistentDataPath + "/" + "World1_" + "savedata" + ".json"; //System.DateTime.Now.ToString($"{System.DateTime.Now:yyyyMMddHHmmss}") + ".json";
        if (SceneManager.GetActiveScene().name == "World2")
            filePath = Application.persistentDataPath + "/" + "World2_" + "savedata" + ".json";//System.DateTime.Now.ToString($"{System.DateTime.Now:yyyyMMddHHmmss}") + ".json";
        if (SceneManager.GetActiveScene().name == "World3")
            filePath = Application.persistentDataPath + "/" + "World3_" + "savedata" + ".json";//System.DateTime.Now.ToString($"{System.DateTime.Now:yyyyMMddHHmmss}") + ".json";
        else
            filePath = Application.persistentDataPath + "/" + "savedata" + ".json";//System.DateTime.Now.ToString($"{System.DateTime.Now:yyyyMMddHHmmss}") + ".json";

        Debug.Log(SceneManager.GetActiveScene().name);

        LoadWorld.instance.filePath = filePath;
        SaveLoad.instance.LoadObject();
    }
}
