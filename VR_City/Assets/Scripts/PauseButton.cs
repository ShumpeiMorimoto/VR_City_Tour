using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private Color enterColor = Color.red;
    [SerializeField] private Color exitColor = Color.white;

    private MeshCollider _collider;
    private MeshRenderer _renderer;

    public GameObject PausePanel;
    public GameObject SaveMenu;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<MeshCollider>();
        _renderer = GetComponent<MeshRenderer>();
        PausePanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Finger")
        {
            _renderer.material.color = enterColor;
            PausePanel.SetActive(!PausePanel.activeSelf);
            SaveMenu.SetActive(false);
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
