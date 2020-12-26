using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DebugTextScript : MonoBehaviour
{
    public List<string> contextList;
    public TextMeshProUGUI DebugText;

    public GameObject wireframeCubeWhite;
    public GameObject wireframeCubePink;

    protected PerformanceCounter cpuCounter;
    protected PerformanceCounter ramCounter;




    public enum debugCategory{
            PlayerInfo = 0,

            SystemInfo = 2,
            Contexts = 5,
            MouseInfo = 7,
            WorldInfo = 10,



            NumberOfCategories
    }
    
    public List<bool> debugDisplayStatusList;

    void Start()
    {
        DebugText = GetComponent<TextMeshProUGUI>();

        Camera.onPostRender += OnPostRenderCallback;

        //cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        //ramCounter = new PerformanceCounter("Memory", "Available MBytes");

    }

    void Update()
    {
        DebugText.text = "";
        
        if (getDebugDisplayStatus(debugCategory.PlayerInfo)){
            DebugText.text += "Player position: " + GameObject.Find("Player").transform.position.ToString() + "\n";   
            DebugText.text += "Player velocity: " + (GameObject.Find("Player").GetComponent<Rigidbody>().velocity+GameObject.Find("Player").GetComponent<PlayerController>().m_Velocity).ToString() + "\n";     
        }


        if (getDebugDisplayStatus(debugCategory.SystemInfo))
        {
            DebugText.text += "CPU: " + SystemInfo.processorType + SystemInfo.processorFrequency + " " + "\n";
            DebugText.text += "GPU: " + SystemInfo.graphicsDeviceName + SystemInfo.graphicsDeviceVersion + "\n";
            DebugText.text += "RAM: " + SystemInfo.systemMemorySize + "\n";
        }


        if (getDebugDisplayStatus(debugCategory.Contexts)){
            //UnityEngine.Debug.Log("trying to display context info");
            
            if (contextList.Count != 0) { contextList.Clear(); }
            
            for (int i = 0; i < (int)PlayerController.Context.NumberOfContexts; i++)
            {
                contextList.Add(GameObject.Find("Player").GetComponent<PlayerController>().ContextList[i].ToString());
                
            }
            DebugText.text += 
                "In air:" + contextList[0] + "\n" + 
                "Jump disabled:" + contextList[1] + "\n" ;
        }


        
        if (getDebugDisplayStatus(debugCategory.MouseInfo)){
            //determining the terrain block underneath the mouse cursor
            RaycastHit hit;
            Physics.Raycast(GameObject.Find("Camera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit);
            DebugText.text += "Unitspace under mouse: " + hit.transform.gameObject.transform.position.ToString();

            Vector3 offset = new Vector3(0.5f, 0.5f, 0.5f);
            UnityEngine.Object.Instantiate(wireframeCubePink, WorldScript.EmptyUnitSpaceOnCursor() + offset, Quaternion.identity);
            UnityEngine.Object.Instantiate(wireframeCubeWhite, hit.collider.gameObject.transform.position + offset, Quaternion.identity);

            if (hit.point.y < hit.transform.gameObject.transform.position.y + 1)
            {

                if (hit.point.x == hit.transform.gameObject.transform.position.x)
                {
                    DebugText.text += " west side";
                }

                if (hit.point.x == hit.transform.gameObject.transform.position.x + 1)
                {
                    DebugText.text += " east side";
                }

                if (hit.point.z == hit.transform.gameObject.transform.position.z)
                {
                    DebugText.text += " south side";
                }

                if (hit.point.z == hit.transform.gameObject.transform.position.z + 1)
                {
                    DebugText.text += " north side";
                }

            }
            else
            {
                DebugText.text += " top";
            }
            //this line is a shorthand for a new line.
            DebugText.text += "\n";

        
        }

    }

    public bool getDebugDisplayStatus (debugCategory desired){
        return debugDisplayStatusList[(int)desired];
    }
    public void setDebugDisplayStatus (int target, bool desired){
        debugDisplayStatusList[target] = desired;
    }

    void OnPostRenderCallback(Camera cam)
    {
        if (Application.isPlayer)
        {
            Destroy(GameObject.Find("WireCubeWhite(Clone)"));
            Destroy(GameObject.Find("WireCubePink(Clone)"));
        }

    }
}
