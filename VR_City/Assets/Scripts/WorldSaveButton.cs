using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WorldSaveButton : MonoBehaviour
{

    [SerializeField] private Color enterColor = Color.red;
    [SerializeField] private Color exitColor = Color.white;

    private MeshCollider _collider;
    private MeshRenderer _renderer;

    public string filePath;

    public static WorldSaveButton instance;

    public GameObject Name;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        _collider = GetComponent<MeshCollider>();
        _renderer = GetComponent<MeshRenderer>();
        
        filePath = Application.persistentDataPath + "/" + Name.name + "savedata" + ".json";

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finger")
        {
            _renderer.material.color = enterColor;
            WorldSaveButton.instance.filePath = filePath;
            Debug.Log(filePath);
            SaveLoad.instance.SaveAllGameObject();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Finger")
        {
            _renderer.material.color = exitColor;

        }
    }
}
