using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class JoystickMovement : MonoBehaviour
{
    public GameObject CenterEye;
    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector2 stickR = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        Vector3 changePosition = new Vector3((stickR.x), 0, (stickR.y));
        Vector3 changeRotation = new Vector3(0, -CenterEye.transform.localEulerAngles.y, 0);
        this.transform.position -=  (Quaternion.Euler(changeRotation) * changePosition) / 200;
    }
}