using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabMove : MonoBehaviour
{
    public float adjustDistance = 0.0f;
    //public GameObject CenterEye;
    public GameObject righthand;
    public GameObject lefthand;

    [SerializeField]
    bool grabJudge = false;
    bool rotationgrabJudge = false;
    Vector3 movePosition;
    Vector3 startGrabPosition;
    Vector3 startViewPosition;
    Vector3 rotationPivot;

    Vector3 rightGrabPosition;
    Vector3 leftGrabPosition;

    void Update()
    {
        CheckGrab();
        Move();
        RotateWorld();
        //this.transform.localEulerAngles = Vector3.zero;
    }

    void CheckGrab()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger))
        {
            if (OVRInput.Get(OVRInput.RawButton.RHandTrigger))
            {
                grabJudge = false;
                rotationgrabJudge = true;
                rightGrabPosition = righthand.transform.position;
                leftGrabPosition = lefthand.transform.position;
                rotationPivot = new Vector3((rightGrabPosition.x + leftGrabPosition.x) / 2, 0, (rightGrabPosition.z + leftGrabPosition.z) / 2);
            }      
            
        }
        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
        {
            if (!rotationgrabJudge)
            {
                grabJudge = true;
                startGrabPosition = righthand.transform.localPosition;
                startViewPosition = this.transform.position;
            }
            if (OVRInput.Get(OVRInput.RawButton.LHandTrigger))
            {
                grabJudge = false;
                rotationgrabJudge = true;
                rightGrabPosition = righthand.transform.position;
                leftGrabPosition = lefthand.transform.position;
                rotationPivot = new Vector3((rightGrabPosition.x + leftGrabPosition.x) / 2, 0, (rightGrabPosition.z + leftGrabPosition.z) / 2);
            }
            
        }

        if (OVRInput.GetUp(OVRInput.RawButton.LHandTrigger))
        {
            rotationgrabJudge = false;
            if (OVRInput.Get(OVRInput.RawButton.RHandTrigger))
            {
                grabJudge = true;
                startGrabPosition = righthand.transform.localPosition;
                startViewPosition = this.transform.position;
            }

        }
        if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger))
        {
            grabJudge = false;
            
        }

        
    }

    void Move()
    {
        if (grabJudge == true)
        {
            Debug.Log("moving");
            movePosition = startGrabPosition - righthand.transform.localPosition;
            this.transform.position = startViewPosition + this.transform.rotation * (movePosition * adjustDistance);
        }
    }

    void RotateWorld()
    {
        if (rotationgrabJudge)
        {
            Vector3 vec_a_right = rightGrabPosition - rotationPivot;
            Vector3 vec_c_right = righthand.transform.position - rotationPivot;

            Vector3 vec_a_left = leftGrabPosition - rotationPivot;
            Vector3 vec_c_left = lefthand.transform.position - rotationPivot;

            var angle_right = Vector3.SignedAngle(vec_a_right, vec_c_right, Vector3.up);
            var angle_left = Vector3.SignedAngle(vec_a_left, vec_c_left, Vector3.up);

            var angle = -(angle_right + angle_left);
            transform.RotateAround(rotationPivot, Vector3.up, angle);
            angle = 0;
            rightGrabPosition = righthand.transform.position;
            leftGrabPosition = lefthand.transform.position;


        }
    }
}
