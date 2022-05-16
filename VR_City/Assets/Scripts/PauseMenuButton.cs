using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButton : MonoBehaviour
{
    [SerializeField] private Color enterColor = Color.white;
    [SerializeField] private Color exitColor = Color.white;

    private MeshCollider _collider;
    private MeshRenderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<MeshCollider>();
        _renderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hand")
        {
            _renderer.material.color = enterColor;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hand")
        {
            _renderer.material.color = exitColor;

        }
    }
}