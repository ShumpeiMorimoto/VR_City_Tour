using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    [SerializeField] private Color enterColor = Color.red;
    [SerializeField] private Color exitColor = Color.white;

    private MeshCollider _collider;
    private MeshRenderer _renderer;

    public GameObject SaveMenu;
    public GameObject PauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        SaveMenu.SetActive(false);
        _collider = GetComponent<MeshCollider>();
        _renderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finger")
        {
            _renderer.material.color = enterColor;
            SaveMenu.SetActive(false);
            PauseMenu.SetActive(false);

        }
        _renderer.material.color = exitColor;
    }
}
