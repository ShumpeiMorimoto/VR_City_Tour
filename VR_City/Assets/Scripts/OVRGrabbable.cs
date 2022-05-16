/************************************************************************************
Copyright : Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.

Your use of this SDK or tool is subject to the Oculus SDK License Agreement, available at
https://developer.oculus.com/licenses/oculussdk/

Unless required by applicable law or agreed to in writing, the Utilities SDK distributed
under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
ANY KIND, either express or implied. See the License for the specific language governing
permissions and limitations under the License.
************************************************************************************/

using System;
using UnityEngine;

/// <summary>
/// An object that can be grabbed and thrown by OVRGrabber.
/// </summary>
public class OVRGrabbable : MonoBehaviour
{
    [SerializeField]
    protected bool m_allowOffhandGrab = true;
    [SerializeField]
    protected bool m_snapPosition = false;
    [SerializeField]
    protected bool m_snapOrientation = false;
    [SerializeField]
    protected Transform m_snapOffset;
    [SerializeField]
    protected Collider[] m_grabPoints = null;

    protected bool m_grabbedKinematic = false;
    protected Collider m_grabbedCollider = null;
    protected OVRGrabber m_grabbedBy = null;

    private bool NeverGrabbed = true;
    private Vector3 pos;
    private Vector3 prevPos;
    private Vector3 planPos;
    private Vector3 currentrot;
    private Vector3 currentpos;
    private Vector3 InitialPos;
    private Quaternion  InitialRot;
    private Vector3 InitialScale;

    private Vector3 CharInitialPos;
    private Quaternion  CharInitialRot;
    private Vector3 CharInitialScale;

    private GameObject planPrefab;
    private Color _color;

    private Collider _collider;
    private Renderer _renderer;
    public Material HalfVisibleMaterial;

    //public GameObject parentObj;
    private GameObject instance;

    const float DELETE_THRESHOLD = 1.5f;

    private bool Grabbing_Object = false;
    private bool GrabActive = false;


    /// <summary>
    /// If true, the object can currently be grabbed.
    /// </summary>
    public bool allowOffhandGrab
    {
        get { return m_allowOffhandGrab; }
    }

    /// <summary>
    /// If true, the object is currently grabbed.
    /// </summary>
    public bool isGrabbed
    {
        get { return m_grabbedBy != null; }
    }

    /// <summary>
    /// If true, the object's position will snap to match snapOffset when grabbed.
    /// </summary>
    public bool snapPosition
    {
        get { return m_snapPosition; }
    }

    /// <summary>
    /// If true, the object's orientation will snap to match snapOffset when grabbed.
    /// </summary>
    public bool snapOrientation
    {
        get { return m_snapOrientation; }
    }

    /// <summary>
    /// An offset relative to the OVRGrabber where this object can snap when grabbed.
    /// </summary>
    public Transform snapOffset
    {
        get { return m_snapOffset; }
    }

    /// <summary>
    /// Returns the OVRGrabber currently grabbing this object.
    /// </summary>
    public OVRGrabber grabbedBy
    {
        get { return m_grabbedBy; }
    }

    /// <summary>
    /// The transform at which this object was grabbed.
    /// </summary>
    public Transform grabbedTransform
    {
        get { return m_grabbedCollider.transform; }
    }

    /// <summary>
    /// The Rigidbody of the collider that was used to grab this object.
    /// </summary>
    public Rigidbody grabbedRigidbody
    {
        get { return m_grabbedCollider.attachedRigidbody; }
    }

    /// <summary>
    /// The contact point(s) where the object was grabbed.
    /// </summary>
    public Collider[] grabPoints
    {
        get { return m_grabPoints; }
    }

    /// <summary>
    /// Notifies the object that it has been grabbed.
    /// </summary>
    virtual public void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        m_grabbedBy = hand;
        m_grabbedCollider = grabPoint;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    /// <summary>
    /// Notifies the object that it has been released.
    /// </summary>
    virtual public void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = m_grabbedKinematic;
        rb.velocity = linearVelocity;
        rb.angularVelocity = angularVelocity;
        m_grabbedBy = null;
        m_grabbedCollider = null;
    }

    void Awake()
    {

        _color = GetComponent<Renderer>().material.color;
        _renderer = GetComponent<Renderer>();

        InitialPos = gameObject.transform.localPosition;
        InitialRot = gameObject.transform.localRotation;
        InitialScale = gameObject.transform.localScale;
        
        if(gameObject.tag == "Player")
        {
            CharInitialPos = new Vector3(0, -50, 10);
            CharInitialRot = Quaternion.identity;
            CharInitialScale = new Vector3(50, 50, 50);
        }


        if (m_grabPoints.Length == 0)
        {
            // Get the collider from the grabbable
            _collider = this.GetComponent<Collider>();
            if (_collider == null)
            {
                throw new ArgumentException("Grabbables cannot have zero grab points and no collider -- please add a grab point or collider.");
            }

            // Create a default grab point
            m_grabPoints = new Collider[1] { _collider };
        }
    }

    protected virtual void Start()
    {
        m_grabbedKinematic = GetComponent<Rigidbody>().isKinematic;
    }

    void OnDestroy()
    {
        if (m_grabbedBy != null)
        {
            // Notify the hand to release destroyed grabbables
            m_grabbedBy.ForceRelease(this);
        }
    }


    void OnTriggerStay(Collider collision)
    {

        if (isGrabbed)
        {
            
            if (gameObject.tag == "BaseItem" || gameObject.tag == "BasePanelItem"  || gameObject.tag == "Player")
            {
                //GetComponent<Renderer>().material.color = Color.red;
                if (collision.gameObject.tag == "Environment" || collision.gameObject.tag == "GroundItem" || collision.gameObject.tag == "BaseItem")
                {
                    Debug.Log(collision.gameObject.name);
                    GetComponent<Renderer>().material.color = Color.red;

                }
            }
        }


        if (gameObject.tag == "PlanItem")
        {
            if (collision.gameObject.tag == "Environment" || collision.gameObject.tag == "GroundItem" || collision.gameObject.tag == "BaseItem")
            {
                GetComponent<Renderer>().material.color = Color.red;

            }
        }
    }


    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Environment" || collision.gameObject.tag == "GroundItem" || collision.gameObject.tag == "BaseItem")
        {
            GetComponent<Renderer>().material.color = Color.white;

        }
        
    }

    void Update()
    {
        currentpos = transform.position;
        //currentrot = transform.localEulerAngles;
        if (isGrabbed)
        {
            Debug.Log(gameObject.name);
            Grabbing_Object = true;
            gameObject.GetComponent<Collider>().isTrigger = true;
            pos = transform.position;
            if (NeverGrabbed)
            {
                NeverGrabbed = false;
                if (gameObject.tag == "PanelItem" || gameObject.tag == "GroundPanelItem" || gameObject.tag == "BasePanelItem")
                {
                    instance = (GameObject)Instantiate(gameObject, pos, gameObject.transform.rotation);
                    instance.name = gameObject.name;
                    instance.transform.rotation = transform.rotation;
                    instance.transform.parent = transform.parent;
                    instance.transform.localScale = this.gameObject.transform.localScale;
                    transform.parent = GameObject.Find("ObjectPlace").transform; //parentObj.transform;
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    if(gameObject.tag == "BasePanelItem" || gameObject.tag == "BaseItem")
                        gameObject.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 4.5f);
                    transform.rotation = Quaternion.identity;
                    transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                    currentpos = transform.position;
                    /*if (gameObject.tag == "GroundPanelItem")
                    {
                        gameObject.tag = "GroundItem";
                        
                    }
                        
                    else if (gameObject.tag == "BasePanelItem")
                    {
                        gameObject.tag = "BaseItem";
                        
                        
                    }
                        
                    else if (gameObject.tag == "PanelItem")
                        gameObject.tag = "Environment";*/
                }
            }

            if (!GrabActive)
            {
                GrabActive = true;
                if (gameObject.tag == "BaseItem" || gameObject.tag == "BasePanelItem")
                {
                    planPos = new Vector3(5 * (Mathf.RoundToInt(currentpos.x / 5)), 0, 5 * (Mathf.RoundToInt(currentpos.z / 5)));
                    planPrefab = Instantiate(gameObject, planPos, gameObject.transform.rotation);
                    planPrefab.name = "PlanPrefab";
                    planPrefab.tag = "PlanItem";
                    //Change to half visible material
                    foreach (Transform child in planPrefab.transform)
                    {
                        foreach (Transform grandchild in child.transform)
                        {
                            var renderer = grandchild.GetComponent<MeshRenderer>();
                            Material[] _material = renderer.sharedMaterials;
                            _material[0] = HalfVisibleMaterial;
                            renderer.sharedMaterials = _material;
                        }
                    }
                    var parentrenderer = planPrefab.GetComponent<MeshRenderer>();
                    Material[] parent_material = parentrenderer.sharedMaterials;
                    parent_material[0] = HalfVisibleMaterial;
                    parentrenderer.sharedMaterials = parent_material;
                    planPrefab.transform.position = new Vector3(5 * (Mathf.RoundToInt(currentpos.x / 5)), 0, 5 * (Mathf.RoundToInt(currentpos.z / 5)));

                }

                else if (gameObject.tag == "GroundItem" || gameObject.tag == "GroundPanelItem")
                {
                    planPos = new Vector3(Mathf.RoundToInt(currentpos.x), 0, Mathf.RoundToInt(currentpos.z));
                    planPrefab = Instantiate(gameObject, planPos, gameObject.transform.rotation);
                    planPrefab.name = "PlanPrefab";
                    //planPrefab.tag = "PlanItem";
                    //Change to half visible material
                    foreach (Transform child in planPrefab.transform)
                    {
                        foreach (Transform grandchild in child.transform)
                        {
                            var renderer = grandchild.GetComponent<MeshRenderer>();
                            Material[] _material = renderer.sharedMaterials;
                            _material[0] = HalfVisibleMaterial;
                            renderer.sharedMaterials = _material;
                        }
                    }
                    var parentrenderer = planPrefab.GetComponent<MeshRenderer>();
                    Material[] parent_material = parentrenderer.sharedMaterials;
                    parent_material[0] = HalfVisibleMaterial;
                    parentrenderer.sharedMaterials = parent_material;
                    planPrefab.transform.position = new Vector3(5 * (Mathf.RoundToInt(currentpos.x / 5)), 0, 5 * (Mathf.RoundToInt(currentpos.z / 5)));

                }
            }

            if (gameObject.tag == "Player")
            {
                transform.parent = null;
                transform.localScale = new Vector3(1f, 1f, 1f);
                //transform.rotation = Quaternion.identity;
                transform.localEulerAngles = new Vector3(0f, 90 * (Mathf.RoundToInt(currentrot.y / 90)), 0f);
                transform.position = new Vector3(Mathf.RoundToInt(currentpos.x), Mathf.RoundToInt(currentpos.y), Mathf.RoundToInt(currentpos.z));
            }

            else if (gameObject.tag == "BasePanelItem" || gameObject.tag == "BaseItem")
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                gameObject.GetComponent<BoxCollider>().size = new Vector3(1f, 4.9f, 1f);
                transform.position = new Vector3(5 * (Mathf.RoundToInt(currentpos.x / 5)), Mathf.RoundToInt(currentpos.y), 5 * (Mathf.RoundToInt(currentpos.z / 5)));
                transform.localEulerAngles = new Vector3(0f, 90 * (Mathf.RoundToInt(currentrot.y / 90)), 0f);
                planPrefab.transform.position = new Vector3(5 * (Mathf.RoundToInt(currentpos.x / 5)), 0, 5 * (Mathf.RoundToInt(currentpos.z / 5)));
                planPrefab.transform.localEulerAngles = new Vector3(0f, 90 * (Mathf.RoundToInt(currentrot.y / 90)), 0f);
                planPrefab.GetComponent<BoxCollider>().size = new Vector3(1f, 4.9f, 1f);

            }

            else if (gameObject.tag == "GroundPanelItem" || gameObject.tag == "GroundItem")
            {
                transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                planPrefab.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                transform.position = new Vector3(Mathf.RoundToInt(currentpos.x), Mathf.RoundToInt(currentpos.y), Mathf.RoundToInt(currentpos.z));
                transform.localEulerAngles = new Vector3(0f, 90 * (Mathf.RoundToInt(currentrot.y / 90)), 0f);
                planPrefab.transform.position = new Vector3(Mathf.RoundToInt(currentpos.x), 0, Mathf.RoundToInt(currentpos.z));
                planPrefab.transform.localEulerAngles = new Vector3(0f, 90 * (Mathf.RoundToInt(currentrot.y / 90)), 0f);

            }

            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                transform.position = new Vector3(Mathf.RoundToInt(currentpos.x), Mathf.RoundToInt(currentpos.y), Mathf.RoundToInt(currentpos.z));
                transform.localEulerAngles = new Vector3(0f, 90 * (Mathf.RoundToInt(currentrot.y / 90)), 0f);
            }

            if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickLeft))
            {
                transform.localEulerAngles = new Vector3(0f, currentrot.y + 90f, 0f);
                currentrot = transform.localEulerAngles;
            }
            
            else if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickRight))
            {
                transform.localEulerAngles = new Vector3(0f, currentrot.y - 90f, 0f);
                currentrot = transform.localEulerAngles;
            }

            if (OVRInput.GetDown(OVRInput.RawButton.A) && gameObject.tag != "Player")
            {
                if (planPrefab.GetComponent<Renderer>().material.color != Color.red)
                {
                    Vector3 dup_pos;

                    if (gameObject.tag == "BasePanelItem" || gameObject.tag == "BaseItem")
                    {
                        dup_pos = new Vector3(5 * (Mathf.RoundToInt(currentpos.x / 5)), 0, 5 * (Mathf.RoundToInt(currentpos.z / 5)));
                    }

                    else if (gameObject.tag == "GroundPanelItem" || gameObject.tag == "GroundItem")
                    {
                        dup_pos = new Vector3(Mathf.RoundToInt(currentpos.x), 0, Mathf.RoundToInt(currentpos.z));
                    }

                    else
                    {
                        dup_pos = new Vector3(Mathf.RoundToInt(currentpos.x), Mathf.RoundToInt(currentpos.y), Mathf.RoundToInt(currentpos.z));
                    }

                    GameObject Duplicated_Obj = (GameObject)Instantiate(gameObject, dup_pos, gameObject.transform.rotation);
                    Duplicated_Obj.name = gameObject.name;
                    Duplicated_Obj.transform.parent = GameObject.Find("ObjectPlace").transform;
                    Duplicated_Obj.transform.localEulerAngles = new Vector3(0f, 90 * (Mathf.RoundToInt(currentrot.y / 90)), 0f);
                    Duplicated_Obj.transform.localScale = new Vector3(1f, 1f, 1f);
                    if (Duplicated_Obj.tag == "BasePanelItem" || Duplicated_Obj.tag == "BaseItem")
                    {
                        Duplicated_Obj.tag = "BaseItem";
                        Duplicated_Obj.GetComponent<BoxCollider>().size = new Vector3(5f, 5f, 5f);
                    }
                    else if (Duplicated_Obj.tag == "GroundPanelItem" || Duplicated_Obj.tag == "GroundItem")
                    {
                        Duplicated_Obj.tag = "GroundItem";
                    }
                    else
                    {
                        Duplicated_Obj.tag = "Environment";
                    }
                }
            }

            
        }
        if (!NeverGrabbed)
        {
            if (!isGrabbed) //When the prefab is placed 
            {
                GrabActive = false;
                gameObject.GetComponent<Collider>().isTrigger = false;
                if(planPrefab != null)
                {
                    if (planPrefab.GetComponent<Renderer>().material.color == Color.red)
                    {
                        Destroy(gameObject);
                    }
                    Destroy(planPrefab);
                }
                if (Grabbing_Object)
                {
                    if (OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude > DELETE_THRESHOLD)
                    {
                        if (gameObject.tag == "Player")
                        {
                            GameObject Instance_Character = Instantiate(gameObject, InitialPos, Quaternion.identity);
                            Instance_Character.name = gameObject.name;
                            Instance_Character.transform.parent = GameObject.Find("CharacterSlot").transform;
                            Instance_Character.transform.localPosition = CharInitialPos;
                            Instance_Character.transform.localRotation = CharInitialRot;
                            Instance_Character.transform.localScale = CharInitialScale;

                        }
                        Destroy(gameObject);
                    }
                }
                

                if (GetComponent<Renderer>().material.color == Color.red)
                {
                    
                    if (gameObject.tag == "Player")
                    {
                        GameObject Instance_Character = Instantiate(gameObject, CharInitialPos, Quaternion.identity);
                        Instance_Character.name = gameObject.name;
                        Instance_Character.transform.parent = GameObject.Find("CharacterSlot").transform;
                        Instance_Character.transform.localPosition = CharInitialPos;
                        Instance_Character.transform.localRotation = CharInitialRot;
                        Instance_Character.transform.localScale = CharInitialScale;
                    }
                    Destroy(gameObject);
                }

                if (gameObject.tag == "BasePanelItem" || gameObject.tag == "BaseItem")
                {

                    //transform.position = new Vector3(Mathf.RoundToInt(currentpos.x), 0, Mathf.RoundToInt(currentpos.z));
                    transform.position = new Vector3(5 * (Mathf.RoundToInt(currentpos.x / 5)), 0, 5 * (Mathf.RoundToInt(currentpos.z / 5)));
                    transform.localEulerAngles = new Vector3(0f, 90 * (Mathf.RoundToInt(currentrot.y / 90)), 0f);
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    gameObject.GetComponent<BoxCollider>().size = new Vector3(5f, 5f, 5f);
                    gameObject.tag = "BaseItem";
                    //transform.rotation = Quaternion.identity;
                }

                else if (gameObject.tag == "GroundPanelItem" || gameObject.tag == "GroundItem")
                {

                    transform.position = new Vector3(Mathf.RoundToInt(currentpos.x), 0, Mathf.RoundToInt(currentpos.z));
                    transform.localEulerAngles = new Vector3(0f, 90 * (Mathf.RoundToInt(currentrot.y / 90)), 0f);
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    gameObject.tag = "GroundItem";
                    //transform.rotation = Quaternion.identity;
                }

                else if (gameObject.tag == "Player")
                {

                    transform.position = new Vector3(Mathf.RoundToInt(currentpos.x), Mathf.RoundToInt(currentpos.y), Mathf.RoundToInt(currentpos.z));
                    transform.localEulerAngles = new Vector3(0f, 90 * (Mathf.RoundToInt(currentrot.y / 90)), 0f);
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    //transform.rotation = Quaternion.identity;
                }

                else
                {
                    
                    transform.position = new Vector3(Mathf.RoundToInt(currentpos.x), Mathf.RoundToInt(currentpos.y), Mathf.RoundToInt(currentpos.z));
                    transform.localEulerAngles = new Vector3(0f, 90 * (Mathf.RoundToInt(currentrot.y / 90)), 0f);
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    if (gameObject.tag != "Player")
                        gameObject.tag = "Environment";
                    //transform.rotation = Quaternion.identity;
                }

                if (GetComponent<Renderer>().material.color == Color.red)
                {

                    if (gameObject.tag == "Player")
                    {
                        GameObject Instance_Character = Instantiate(gameObject, InitialPos, Quaternion.identity);
                        Instance_Character.name = gameObject.name;
                        Instance_Character.transform.parent = GameObject.Find("CharacterSlot").transform;
                        Instance_Character.transform.localPosition = CharInitialPos;
                        Instance_Character.transform.localRotation = CharInitialRot;
                        Instance_Character.transform.localScale = CharInitialScale;
                    }
                    Destroy(gameObject);
                }
                
            }
        }

        if (!isGrabbed)
        {
            Grabbing_Object = false;
        }
        if(gameObject.tag != "PlanItem")
        {
            if (!isGrabbed)
            {
                gameObject.GetComponent<Collider>().isTrigger = false;
            }
        }
        
    }
}
