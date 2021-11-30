using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    private float dir_y;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        dir_y = Camera.main.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, dir_y, 0);
        transform.position = transform.position + Camera.main.transform.forward * Time.deltaTime * speed;
        //Debug.Log(Camera.main.transform.forward);

        if (Input.GetKeyDown(KeyCode.Space))
            Camera.main.transform.Rotate(0, 90, 0);
    }
}
