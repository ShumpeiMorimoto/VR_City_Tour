using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    /*public GameObject _character;
    public GameObject _camera;
    private Vector3 InitialPos;
    private GameObject instance;*/

    private Vector3 CharInitialPos;
    private Quaternion CharInitialRot;
    private Vector3 CharInitialScale;
    private GameObject Character_Prefab;

    //public GameObject CharacterPrefab;
    // Start is called before the first frame update
    void Start()
    {
        /*Character_Prefab = GameObject.Find("Character_Prefab");
        CharInitialPos = Character_Prefab.transform.localPosition;
        CharInitialRot = Character_Prefab.transform.localRotation;
        CharInitialScale = Character_Prefab.transform.localScale;*/
        //InitialPos = _character.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -20)
        {
            //gameObject.SetActive(false);
            /*instance = (GameObject)Instantiate(_character, InitialPos, Quaternion.identity);
            _camera.transform.parent = instance.transform;
            _camera.transform.localPosition = new Vector3(0, 0, 0);
            instance.SetActive(true);*/
            /*GameObject Instance_Character_Prefab = Instantiate(Character_Prefab, CharInitialPos, CharInitialRot);
            Instance_Character_Prefab.name = "Character_Prefab";
            Instance_Character_Prefab.transform.parent = GameObject.Find("CharacterSlot").transform;
            Instance_Character_Prefab.transform.position = CharInitialPos;
            Instance_Character_Prefab.transform.rotation = CharInitialRot;
            Instance_Character_Prefab.transform.localScale = CharInitialScale;*/
            //Destroy(gameObject);
            ChangeView.instance.ShowGodView();
            ChangeView.instance.Builderview = true;
        }

    }
}
