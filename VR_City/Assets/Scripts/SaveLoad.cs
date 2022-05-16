using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;



public class SaveLoad : MonoBehaviour
{

    //public GameObject ParentGameObject;

    private ObjectData objectData;
    private string objectSaveData;
    private ObjectList objectList;


    public static SaveLoad instance;

    public StreamWriter writer;
    private string filePath;

    private void Awake()
    {
        instance = this;

        if (SceneManager.GetActiveScene().name == "World1")
            filePath = Application.persistentDataPath + "/" + "World1_" + "savedata" + ".json"; //System.DateTime.Now.ToString($"{System.DateTime.Now:yyyyMMddHHmmss}") + ".json";
        else if (SceneManager.GetActiveScene().name == "World2")
            filePath = Application.persistentDataPath + "/" + "World2_" + "savedata" + ".json";//System.DateTime.Now.ToString($"{System.DateTime.Now:yyyyMMddHHmmss}") + ".json";
        else if (SceneManager.GetActiveScene().name == "World3")
            filePath = Application.persistentDataPath + "/" + "World3_" + "savedata" + ".json";//System.DateTime.Now.ToString($"{System.DateTime.Now:yyyyMMddHHmmss}") + ".json";
        else
            filePath = Application.persistentDataPath + "/" + "savedata" + ".json";//System.DateTime.Now.ToString($"{System.DateTime.Now:yyyyMMddHHmmss}") + ".json";

        //Debug.Log(SceneManager.GetActiveScene().name);
        //filePath = Application.persistentDataPath + "/" + "World1_" + "savedata" + ".json";

        if (SceneManager.GetActiveScene().name != "DefaultWorld")
        {
            LoadObject();
        }



    }


    public void SaveAllGameObject()
    {
        filePath = WorldSaveButton.instance.filePath;
        Debug.Log(filePath);
        objectList = new ObjectList();
        writer = new StreamWriter(filePath);
        foreach (Transform child in GameObject.Find("ObjectPlace").transform)
        {
            Debug.Log(child);
            ObjectToJson(child.gameObject);
        }
        SaveObjects();
        writer.Close();
    }

    private void Update()
    {
        //Debug.Log(filePath);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveAllGameObject();
        }

        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            LoadObject();
        }

    }

    /*public void LoadAllGameObject()
    {
        LoadObject();
    }*/

    public void ObjectToJson(GameObject _object)
    {
        objectData = new ObjectData();
        objectData._object = _object.gameObject;
        objectData.object_name = _object.gameObject.name;
        objectData.position = _object.transform.position;
        objectData.rotation = _object.transform.rotation;

        objectList.objectData.Add(objectData);
        objectList.WorldName = SceneManager.GetActiveScene().name;
        /*objectList.CharacterPos = ChangeView.instance.Charpos;
        objectList.BuilderPos = ChangeView.instance.Builderpos;*/
    }

    public void SaveObjects()
    {
        objectSaveData = JsonUtility.ToJson(objectList);

        writer.Write(objectSaveData);
        writer.Flush();
        //writer.Close();
    }

    public void LoadObject()
    {
        //filePath = LoadWorld.instance.filePath;
        if (File.Exists(filePath))
        {
            StreamReader streamReader;
            streamReader = new StreamReader(filePath);
            string data = streamReader.ReadToEnd();
            streamReader.Close();
            ObjectList objects = JsonUtility.FromJson<ObjectList>(data);
            //Instantiate(objects.objectData[0]._object, objects.objectData[0].position, objects.objectData[0].rotation);
            //Debug.Log(objects.objectData[0]._object);
            for(int i = 0; i < objects.objectData.Count; i++)
            {
                Debug.Log(objects.objectData[i]);
                if((GameObject)Resources.Load(objects.objectData[i].object_name) != null)
                {
                    GameObject Loaded_Object = Instantiate((GameObject)Resources.Load(objects.objectData[i].object_name), objects.objectData[i].position, objects.objectData[i].rotation);
                    Loaded_Object.name = objects.objectData[i].object_name;
                    Loaded_Object.transform.parent = GameObject.Find("ObjectPlace").transform;
                }
                else
                {
                    Debug.Log("Not in File!" + objects.objectData[i].object_name);
                }
                
            }
            
        }
        else
        {
            Debug.Log("The save file does not exist!");
        }
    }


    
}
