using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePanelNext : MonoBehaviour
{
    public List<GameObject> PanelList = new List<GameObject>();
    public List<GameObject> PanelButtonList = new List<GameObject>();

    private int currentPanelnum;
    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < PanelList.Count; i++)
        {
            PanelList[i].SetActive(false); 
        }

        currentPanelnum = 0;
        PanelList[currentPanelnum].SetActive(true);
        PanelButtonList[currentPanelnum].GetComponent<Renderer>().material.color = Color.gray;

    }

    private void Update()
    {
        
        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickDown))
        {
            BackPanel();
        }

        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickUp))
        {
            NextPanel();
        }
    }

    private void NextPanel()
    {
        PanelList[currentPanelnum].SetActive(false);
        PanelButtonList[currentPanelnum].GetComponent<MeshRenderer>().material.color = Color.white;
        if (currentPanelnum == PanelList.Count - 1)
        {
            currentPanelnum = -1;
        }
        
        PanelList[currentPanelnum + 1].SetActive(true);
        PanelButtonList[currentPanelnum + 1].GetComponent<MeshRenderer>().material.color  = Color.gray;
        currentPanelnum += 1;
    }

    private void BackPanel()
    {
        PanelList[currentPanelnum].SetActive(false);
        PanelButtonList[currentPanelnum].GetComponent<MeshRenderer>().material.color = Color.white;
        if (currentPanelnum == 0)
        {
            currentPanelnum = PanelList.Count;
        }
        
        PanelList[currentPanelnum - 1].SetActive(true);
        PanelButtonList[currentPanelnum - 1].GetComponent<MeshRenderer>().material.color = Color.gray;
        
        currentPanelnum -= 1;
        
    }

    
    

    
}
