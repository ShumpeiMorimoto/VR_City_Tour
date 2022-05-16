using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectData
{
    public GameObject _object;
    public string object_name;
    public Vector3 position;
    public Quaternion rotation;
    
}

[System.Serializable]
public class ObjectList
{
    public string WorldName;
    public Vector3 CharacterPos;
    public Vector3 BuilderPos;
    public List<ObjectData> objectData = new List<ObjectData>();
}
