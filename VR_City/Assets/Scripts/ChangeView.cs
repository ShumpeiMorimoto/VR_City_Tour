using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeView : MonoBehaviour
{
    [SerializeField] private Color enterColor = Color.white;
    [SerializeField] private Color exitColor = Color.white;

    private MeshCollider _collider;
    private MeshRenderer _renderer;

    public GameObject Character_Controller;
    private GameObject CharacterPrefab;
    private Vector3 CharacterPrefab_Place;
    public GameObject OVR_Camera;
    public GameObject RightHand;
    public GameObject Builderviewplace;
    public GameObject CenterEye;

    public Vector3 Builderpos;
    public Vector3 Charpos;

    private Vector3 Builderrot;
    private Vector3 Charrot;

    public bool Builderview;
    private bool ButtonPushed;
    private bool flag;

    private Vector3 InitialPos;

    public static ChangeView instance;
    public GameObject CharacterSlot;

    private void Awake()
    {
        instance = this;
        CharacterPrefab = GameObject.Find("Character_Prefab");

    }

    public void ShowGodView()
    {
        OVR_Camera.GetComponent<GrabMove>().enabled = true;
        RightHand.GetComponent<OVRGrabber>().enabled = true;
        Character_Controller.GetComponent<JoystickMovement>().enabled = false;

        OVR_Camera.transform.parent = Builderviewplace.transform;
        OVR_Camera.transform.position = Builderpos;
        
        CenterEye.transform.localEulerAngles = Builderrot;
        OVR_Camera.transform.localScale = new Vector3(40f, 40f, 40f);

        //CharacterPrefab.transform.position = Character_Controller.transform.position;

        Character_Controller.SetActive(false);
        Builderviewplace.SetActive(true);
        CharacterPrefab.SetActive(true);
        if (GameObject.Find("TrackingSpace") != null)
        {
            GameObject.Find("TrackingSpace").transform.localEulerAngles = Vector3.zero;
        }
    }

    public void ShowCharacterView()
    {
        OVR_Camera.GetComponent<GrabMove>().enabled = false;
        RightHand.GetComponent<OVRGrabber>().enabled = false;
        Character_Controller.GetComponent<JoystickMovement>().enabled = true;

        //Builderviewplace.SetActive(false);
        OVR_Camera.transform.localScale = new Vector3(1f, 1f, 1f);
        OVR_Camera.transform.parent = Character_Controller.transform;
        OVR_Camera.transform.localPosition = Vector3.zero;

        Character_Controller.transform.position = CharacterPrefab.transform.position;
        CharacterPrefab.SetActive(false);

        Character_Controller.SetActive(true);
        Debug.Log(OVR_Camera.transform.position);
    }

    public void SavePosition()
    {
        if (Builderview)
        {
            Builderpos = OVR_Camera.transform.position;
            Builderrot = CenterEye.transform.localEulerAngles;

        }
        if (!Builderview)
        {
            Charpos = OVR_Camera.transform.position;
            Charrot = CenterEye.transform.localEulerAngles;

        }
    }

    private void Start()
    {
        Character_Controller.SetActive(false);
        OVR_Camera.SetActive(true);
        Builderpos = OVR_Camera.transform.position;

        _collider = GetComponent<MeshCollider>();
        _renderer = GetComponent<MeshRenderer>();

        Builderview = true;
        ButtonPushed = false;

        //InitialPos = Character_Controller.transform.position;
       
    }

    private void Update()
    {
        SavePosition();
        

    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Finger")
        {
            
            _renderer.material.color = enterColor;
            if (!ButtonPushed)
            {
                ButtonPushed = true;
                if (Builderview)
                {
                    CharacterPrefab = GameObject.Find("Character_Prefab");
                    if (CharacterPrefab != null)
                    {
                        if (CharacterPrefab.transform.parent != CharacterSlot.transform)
                        {
                            //CharacterPrefab = GameObject.Find("Character_Prefab");
                            ShowCharacterView();
                        }
                        else
                        {
                            Builderview = !Builderview;
                            flag = !flag;
                            ButtonPushed = false;
                        }
                    }   
                }

                else
                {
                    ShowGodView();
                    
                    CharacterPrefab.transform.position = Character_Controller.transform.position;
                    if (CharacterPrefab.transform.position.y < 0)
                        CharacterPrefab.transform.position = new Vector3(CharacterPrefab.transform.position.x, 0, CharacterPrefab.transform.position.z);
                }

                Builderview = !Builderview;
            }
            

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Finger")
        {
            _renderer.material.color = exitColor;
            ButtonPushed = false;
        }
    }

}