using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    private GameObject _HUD;
    // Start is called before the first frame update
    void Start()
    {
        _HUD = GameObject.Find("HUD");
        //_HUD.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickLeft))
        {
            _HUD.SetActive(true);
        }

        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickRight))
        {
            _HUD.SetActive(false);
        }
    }
}
