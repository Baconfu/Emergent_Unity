using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenuController : MonoBehaviour
{
    
    public bool panelActive;

    GameObject panel;
    GameObject togglesColumn;
    public List<bool> toggles;


    // Start is called before the first frame update
    void Start()
    {
        
        

        panel = transform.Find("Panel").gameObject;
        
        togglesColumn = panel.transform.Find("Column").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        panelActive = panel.activeInHierarchy;
        if (!panelActive){
            if (Input.GetKeyDown(KeyCode.F3)){
                //Debug.Log("checked for f3");
                panel.SetActive(true);
            }
        }
        if (panelActive){
            
            
            
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F3)){
                panel.SetActive(false);
            }
        }
        
    }
}
