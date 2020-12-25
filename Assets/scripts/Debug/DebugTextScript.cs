using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DebugTextScript : MonoBehaviour
{
    public List<string> contextList;
    public TextMeshProUGUI DebugText;
    private Vector2 mousePosition;

    public enum debugCategory{
            PlayerInfo = 0,
            Contexts = 5,
            MouseInfo = 7,
            WorldInfo = 10,



            NumberOfCategories
    }
    
    public List<bool> debugDisplayStatusList;
    
    // Start is called before the first frame update
    void Start()
    {
        DebugText = GetComponent<TextMeshProUGUI>();
        mousePosition = Input.mousePosition;

        /*for(int i = 0; i<(int)debugCategory.NumberOfCategories; i++){
            debugDisplayList.Add(false);
        }*/
        
    }

    // Update is called once per frame
    void Update()
    {
        DebugText.text = "";
        
        if (getDebugDisplayStatus(debugCategory.PlayerInfo)){
            DebugText.text += "Player position: " + GameObject.Find("Player").transform.position.ToString() + "\n";   
            DebugText.text += "Player velocity: " + (GameObject.Find("Player").GetComponent<Rigidbody>().velocity+GameObject.Find("Player").GetComponent<PlayerController>().m_Velocity).ToString() + "\n";  

            //determining the terrain block underneath the mouse cursor
            RaycastHit hit;
            Physics.Raycast(GameObject.Find("Camera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit);
            DebugText.text += "Unitspace under mouse: " + hit.transform.gameObject.transform.position.ToString(); 
            if (hit.point.y < hit.transform.gameObject.transform.position.y+1){
                DebugText.text += " on the side";
            }else{
                DebugText.text += " on the top";
            }
            //this line is a shorthand for a new line.
            DebugText.text += "\n";
        }

        if (getDebugDisplayStatus(debugCategory.Contexts)){
            Debug.Log("trying to display context info");
            contextList.Clear();
            for (int i = 0; i < (int)PlayerController.Context.NumberOfContexts; i++)
            {
                contextList.Add(GameObject.Find("Player").GetComponent<PlayerController>().ContextList[i].ToString());
                
            }
            DebugText.text += 
                "In air:" + contextList[0] + "\n" + 
                "Jump disabled:" + contextList[1] + "\n" ;
        }
        
        
        
        

        
        

    }

    public bool getDebugDisplayStatus (debugCategory desired){
        return debugDisplayStatusList[(int)desired];
    }
    public void setDebugDisplayStatus (int target, bool desired){
        debugDisplayStatusList[target] = desired;
    }
}
