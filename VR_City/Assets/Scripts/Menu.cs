using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject LoadPanel;

    public List<GameObject> MenuList = new List<GameObject>();
    public List<GameObject> LoadList = new List<GameObject>();

    private int currentnum;

    void Start()
    {
        MenuPanel.SetActive(true);
        LoadPanel.SetActive(false);

        currentnum = 0;
        MenuList[currentnum].GetComponent<Renderer>().material.color = Color.blue;
        //LoadList[currentnum].GetComponent<Renderer>().material.color = Color.blue;

    }

    private void Update()
    {
        if(MenuPanel.activeSelf == true)
        {
            if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickDown))
            {
                NextPanel(MenuList);
            }

            if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickUp))
            {
                BackPanel(MenuList);
            }

            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                if (MenuList[currentnum].name == "StartButton")
                {
                    
                    SceneManager.LoadScene("DefaultWorld");
                }
                
                if (MenuList[currentnum].name == "ExitButton")
                {
                    
                    Application.Quit();
                }
                if (MenuList[currentnum].name == "LoadButton")
                {
                    MenuList[currentnum].GetComponent<Renderer>().material.color = Color.white;
                    currentnum = -1;
                    LoadPanel.SetActive(true);
                    MenuPanel.SetActive(false);


                }
                //MenuList[currentnum].GetComponent<Renderer>().material.color = Color.white;
            }
        }

        if(LoadPanel.activeSelf == true)
        {
            //Debug.Log(currentnum);
            if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickDown))
            {
                NextPanel(LoadList);
            }

            if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickUp))
            {
                BackPanel(LoadList);
            }
            if(currentnum >= 0)
            {
                Debug.Log(currentnum);
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    Debug.Log("Here");

                    if (LoadList[currentnum].name == "World1Button")
                    {
                        Debug.Log("no");
                        if (File.Exists(Application.persistentDataPath + "/" + "World1_" + "savedata" + ".json"))
                            SceneManager.LoadScene("World1");
                       
                    }
                    if (LoadList[currentnum].name == "World2Button")
                    {

                        if (File.Exists(Application.persistentDataPath + "/" + "World2_" + "savedata" + ".json"))
                            SceneManager.LoadScene("World1");
                    }
                    if (LoadList[currentnum].name == "World3Button")
                    {

                        if (File.Exists(Application.persistentDataPath + "/" + "World3_" + "savedata" + ".json"))
                            SceneManager.LoadScene("World1");
                    }

                    if (LoadList[currentnum].name == "BackButton")
                    {
                        LoadList[currentnum].GetComponent<Renderer>().material.color = Color.white;
                        MenuPanel.SetActive(true);
                        LoadPanel.SetActive(false);

                        currentnum = 0;
                    }
                }
            }
            
        }

        
    }
    private void NextPanel(List<GameObject> list)
    {
        if(currentnum > -1)
        {
            list[currentnum].GetComponent<MeshRenderer>().material.color = Color.white;
        }
        
        if (currentnum == list.Count - 1)
        {
            currentnum = -1;
        }
        list[currentnum + 1].GetComponent<MeshRenderer>().material.color = Color.blue;
        currentnum += 1;
    }

    private void BackPanel(List<GameObject> list)
    {
        if (currentnum == -1)
        {
            currentnum += 2;
        }
        list[currentnum].GetComponent<MeshRenderer>().material.color = Color.white;
        if (currentnum == 0)
        {
            currentnum = list.Count;
        }
        list[currentnum - 1].GetComponent<MeshRenderer>().material.color = Color.blue;

        currentnum -= 1;

    }
}
